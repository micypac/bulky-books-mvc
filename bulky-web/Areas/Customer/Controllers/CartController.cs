﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bulkyBook.Models;
using bulkyBook.Models.ViewModels;
using bulky.DataAccess.Repository.IRepository;
using System.Security.Claims;

namespace bulky_web.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
  private readonly IUnitOfWork _unitOfWork;

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
    return View();
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
