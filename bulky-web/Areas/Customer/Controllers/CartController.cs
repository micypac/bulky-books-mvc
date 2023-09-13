using Microsoft.AspNetCore.Authorization;
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
        .GetAll(item => item.ApplicationUserId == userid, includeProperties: "Product")
    };

    foreach (var cart in ShoppingCartVM.ShoppingCartList)
    {
      cart.Price = GetPriceBasedOnQuantity(cart);
      ShoppingCartVM.OrderTotal += cart.Price * cart.Count;
    }

    return View(ShoppingCartVM);
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
