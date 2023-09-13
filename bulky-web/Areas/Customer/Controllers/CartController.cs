using Microsoft.AspNetCore.Mvc;

namespace bulky_web.Areas.Customer.Controllers;

[Area("Customer")]
public class CartController : Controller
{
  public IActionResult Index()
  {
    return View();
  }
}
