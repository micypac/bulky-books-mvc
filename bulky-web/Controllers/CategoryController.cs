using bulky_web.Data;
using Microsoft.AspNetCore.Mvc;
using bulky_web.Models;

namespace bulky_web.Controllers;

public class CategoryController : Controller
{
  private readonly ApplicationDbContext _db;

  public CategoryController(ApplicationDbContext db)
  {
    _db = db;
  }

  public IActionResult Index()
  {
    List<Category> objCategoryList = _db.Categories.ToList();
    return View(objCategoryList);
  }

  public IActionResult Create()
  {
    return View();
  }
}
