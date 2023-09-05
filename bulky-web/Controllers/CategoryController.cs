using bulky_web.Data;
using Microsoft.AspNetCore.Mvc;
using bulky_web.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

  [HttpPost]
  public IActionResult Create(Category obj)
  {
    // if (obj.Name == obj.DisplayOrder.ToString()) // this is a custom validation
    // {
    //   ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
    // }

    if (ModelState.IsValid)
    {
      _db.Categories.Add(obj);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    return View(obj);
  }
}
