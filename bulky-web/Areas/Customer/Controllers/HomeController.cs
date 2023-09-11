using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
        Product prod = _unitOfWork.Product.Get(item => item.Id == prodId, includeProperties: "Category");
        return View(prod);
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
