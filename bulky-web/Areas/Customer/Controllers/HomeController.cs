using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using bulkyBook.Models;
using bulky.DataAccess.Repository.IRepository;

namespace bulky_web.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> prodList = _unitOfWork.Product.GetAll(includeProperties: "Category");
        return View(prodList);
    }

    public IActionResult Details(int prodId)
    {
        ShoppingCart cart = new()
        {

            Product = _unitOfWork.Product.Get(item => item.Id == prodId, includeProperties: "Category"),
            Count = 1,
            ProductId = prodId

        };

        return View(cart);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart cartObj)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        cartObj.ApplicationUserId = userId;

        _unitOfWork.ShoppingCart.Add(cartObj);
        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
