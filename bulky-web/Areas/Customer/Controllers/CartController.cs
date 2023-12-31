﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bulky.Utility;
using bulkyBook.Models;
using bulkyBook.Models.ViewModels;
using bulky.DataAccess.Repository.IRepository;
using System.Security.Claims;
using Stripe.Checkout;

namespace bulky_web.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
  private readonly IUnitOfWork _unitOfWork;

  [BindProperty]
  public ShoppingCartVM ShoppingCartVM { get; set; }

  public CartController(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  public IActionResult Index()
  {
    var claimsIdentity = (ClaimsIdentity)User.Identity;
    var userid = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

    ShoppingCartVM = new()
    {
      ShoppingCartList = _unitOfWork.ShoppingCart
        .GetAll(item => item.ApplicationUserId == userid, includeProperties: "Product"),
      OrderHeader = new()
    };

    foreach (var cart in ShoppingCartVM.ShoppingCartList)
    {
      cart.Price = GetPriceBasedOnQuantity(cart);
      ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
    }

    return View(ShoppingCartVM);
  }

  public IActionResult Summary()
  {
    var claimsIdentity = (ClaimsIdentity)User.Identity;
    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

    ShoppingCartVM = new()
    {
      ShoppingCartList = _unitOfWork.ShoppingCart
        .GetAll(item => item.ApplicationUserId == userId, includeProperties: "Product"),
      OrderHeader = new()
    };

    ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(item => item.Id == userId);
    ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
    ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
    ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
    ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
    ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
    ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

    foreach (var cart in ShoppingCartVM.ShoppingCartList)
    {
      cart.Price = GetPriceBasedOnQuantity(cart);
      ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
    }

    return View(ShoppingCartVM);
  }

  [HttpPost]
  [ActionName("Summary")]
  public IActionResult SummaryPOST()
  {
    var claimsIdentity = (ClaimsIdentity)User.Identity;
    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

    ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(item => item.ApplicationUserId == userId,
      includeProperties: "Product");

    ShoppingCartVM.OrderHeader.OrderDate = DateTime.UtcNow;
    ShoppingCartVM.OrderHeader.ApplicationUserId = userId;
    ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(obj => obj.Id == userId);

    foreach (var cart in ShoppingCartVM.ShoppingCartList)
    {
      cart.Price = GetPriceBasedOnQuantity(cart);
      ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
    }

    if (applicationUser.CompanyId.GetValueOrDefault() == 0)
    {
      ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
      ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
    }
    else
    {
      ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
      ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
    }

    _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
    _unitOfWork.Save();

    foreach (var cart in ShoppingCartVM.ShoppingCartList)
    {
      OrderDetail orderDetail = new()
      {
        ProductId = cart.ProductId,
        OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
        Price = cart.Price,
        Count = cart.Count
      };

      _unitOfWork.OrderDetail.Add(orderDetail);
      _unitOfWork.Save();
    }

    if (applicationUser.CompanyId.GetValueOrDefault() == 0)
    {
      // Stripe logic
      var domain = "http://localhost:5278/";
      var options = new SessionCreateOptions
      {
        SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
        CancelUrl = domain + "customer/cart/index",
        LineItems = new List<SessionLineItemOptions>(),
        Mode = "payment",
      };

      foreach (var items in ShoppingCartVM.ShoppingCartList)
      {
        var sessionLineItem = new SessionLineItemOptions
        {
          PriceData = new SessionLineItemPriceDataOptions
          {
            UnitAmount = (long)(items.Price * 100),
            Currency = "usd",
            ProductData = new SessionLineItemPriceDataProductDataOptions
            {
              Name = items.Product.Title
            }
          },
          Quantity = items.Count
        };

        options.LineItems.Add(sessionLineItem);
      }

      var service = new SessionService();
      Session session = service.Create(options);

      _unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
      _unitOfWork.Save();

      Response.Headers.Add("Location", session.Url);
      return new StatusCodeResult(303);
    }

    return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
  }

  public IActionResult OrderConfirmation(int id)
  {
    OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(obj => obj.Id == id, includeProperties: "ApplicationUser");

    if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
    {
      var service = new SessionService();
      Session session = service.Get(orderHeader.SessionId);

      if (session.PaymentStatus.ToLower() == "paid")
      {
        _unitOfWork.OrderHeader.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
        _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);

        _unitOfWork.Save();
      }
    }

    List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
      .GetAll(obj => obj.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

    _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
    _unitOfWork.Save();

    return View(id);
  }

  public IActionResult Plus(int cartId)
  {
    var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
    cartFromDb.Count += 1;
    _unitOfWork.ShoppingCart.Update(cartFromDb);
    _unitOfWork.Save();

    return RedirectToAction(nameof(Index));
  }

  public IActionResult Minus(int cartId)
  {
    var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

    if (cartFromDb.Count <= 1)
      _unitOfWork.ShoppingCart.Remove(cartFromDb);
    else
    {
      cartFromDb.Count -= 1;
      _unitOfWork.ShoppingCart.Update(cartFromDb);
    }

    _unitOfWork.Save();
    return RedirectToAction(nameof(Index));
  }

  public IActionResult Remove(int cartId)
  {
    var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
    _unitOfWork.ShoppingCart.Remove(cartFromDb);
    _unitOfWork.Save();

    return RedirectToAction(nameof(Index));
  }

  private double GetPriceBasedOnQuantity(ShoppingCart cart)
  {
    if (cart.Count <= 50)
      return cart.Product.Price;
    else
    {
      if (cart.Count <= 100)
        return cart.Product.Price50;
      else
        return cart.Product.Price100;
    }
  }
}
