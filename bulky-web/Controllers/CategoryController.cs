using bulky_web.Data;
using Microsoft.AspNetCore.Mvc;
using bulky_web.Models;
using System.Data.Common;

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

  public IActionResult Edit(int? id)
  {
    if (id == null || id == 0)
    {
      return NotFound();
    }

    Category? retrievedCategory = _db.Categories.Find(id); // only works on primary keys
    // Category? retrievedCategory1 = _db.Categories.FirstOrDefault(u => u.Id == id); // will return null if not found
    // Category? retrievedCategory2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault(); // if need more filter criteria

    if (retrievedCategory == null)
    {
      return NotFound();
    }

    return View(retrievedCategory);
  }

  [HttpPost]
  public IActionResult Edit(Category obj)
  {
    if (ModelState.IsValid)
    {
      _db.Categories.Update(obj);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    return View(obj);
  }

  public IActionResult Delete(int? id)
  {
    if (id == null || id == 0)
      return NotFound();

    Category? retrievedCategory = _db.Categories.Find(id);

    if (retrievedCategory == null)
      return NotFound();

    return View(retrievedCategory);
  }

  [HttpPost, ActionName("Delete")]
  public IActionResult DeletePOST(int? id)
  {
    Category? obj = _db.Categories.Find(id);

    if (obj == null)
      return NotFound();

    _db.Categories.Remove(obj);
    _db.SaveChanges();
    return RedirectToAction("Index");

  }
}
