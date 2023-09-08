using bulky.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using bulkyBook.Models;
using System.Data.Common;
using bulky.DataAccess.Repository.IRepository;

namespace bulky_web.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
  private readonly IUnitOfWork _unitOfWork;

  public CategoryController(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  public IActionResult Index()
  {
    List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
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
      _unitOfWork.Category.Add(obj);
      _unitOfWork.Save();
      TempData["success"] = "Category created successfully";
      return RedirectToAction("Index");
    }

    return View(obj);
  }

  public IActionResult Edit(int? id)
  {
    if (id == null || id == 0)
      return NotFound();

    Category? retrievedCategory = _unitOfWork.Category.Get(u => u.Id == id); // only works on primary keys
    // Category? retrievedCategory1 = _db.Categories.FirstOrDefault(u => u.Id == id); // will return null if not found
    // Category? retrievedCategory2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault(); // if need more filter criteria

    if (retrievedCategory == null)
      return NotFound();

    return View(retrievedCategory);
  }

  [HttpPost]
  public IActionResult Edit(Category obj)
  {
    if (ModelState.IsValid)
    {
      _unitOfWork.Category.Update(obj);
      _unitOfWork.Save();
      TempData["success"] = "Category updated successfully";
      return RedirectToAction("Index");
    }

    return View(obj);
  }

  public IActionResult Delete(int? id)
  {
    if (id == null || id == 0)
      return NotFound();

    Category? retrievedCategory = _unitOfWork.Category.Get(u => u.Id == id);

    if (retrievedCategory == null)
      return NotFound();

    return View(retrievedCategory);
  }

  [HttpPost, ActionName("Delete")]
  public IActionResult DeletePOST(int? id)
  {
    Category? obj = _unitOfWork.Category.Get(u => u.Id == id);

    if (obj == null)
      return NotFound();

    _unitOfWork.Category.Remove(obj);
    _unitOfWork.Save();
    TempData["success"] = "Category deleted successfully";
    return RedirectToAction("Index");

  }
}
