using Microsoft.AspNetCore.Mvc;
using bulkyBook.Models;
using bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using bulkyBook.Models.ViewModels;

namespace bulky_web.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
  // fields
  private readonly IUnitOfWork _unitOfWork;

  // constructor
  public ProductController(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }


  // methods

  public IActionResult Index()
  {
    List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();

    return View(objProductList);
  }

  public IActionResult Create()
  {

    // _unitOfWork.Category is returning IEnumerable of Category so we convert it into IEnumerable of SelectListItem
    // EF Core Projections
    IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
      .Select(obj => new SelectListItem
      {
        Text = obj.Name,
        Value = obj.Id.ToString()
      });

    /*
      VIEWBAG
      - transfers data from controller to view, not vice-versa. Ideal for situations in which the temp data is not in a model.
      - any number of properties and values can be assigned to viewbag.
      - life only lasts during the current http req. Viewbag values will be null if redirection occurs.
      - viewbag is actually a wrapper around viewdata.
    */

    // ViewBag.CategoryList = CategoryList;

    /*
      VIEWDATA
      - transfers data from controller to view, not vice-versa. Ideal for situations in which the temp data is not in a model.
      - is derived from viewdatadictionary which is a dictionary type.
      - value must be type cast before use.
      - life only lasts during the current http req. Viewdata values will be null if redirection occurs.
    */

    // ViewData["CategoryList"] = CategoryList;

    /*
      TEMPDATA
      - tempdata can be used to store data between two consecutive req.
      - internally use session to store the data. 
      - tempdata value must be typecast before use. Check for null values to avoid runtime errors.
      - can be used to store only one time message like success, error, and validation messages.
    */


    ProductVM productVM = new()
    {
      Product = new Product(),
      CategoryList = CategoryList
    };

    return View(productVM);
  }

  [HttpPost]
  public IActionResult Create(ProductVM obj)
  {
    if (ModelState.IsValid)
    {
      _unitOfWork.Product.Add(obj.Product);
      _unitOfWork.Save();
      TempData["success"] = "Product created successfully";
      return RedirectToAction("Index");
    }

    obj.CategoryList = _unitOfWork.Category.GetAll()
      .Select(obj => new SelectListItem
      {
        Text = obj.Name,
        Value = obj.Id.ToString()
      });

    return View(obj);
  }

  public IActionResult Edit(int? id)
  {
    if (id == null || id == 0)
      return NotFound();

    Product? retrievedProduct = _unitOfWork.Product.Get(u => u.Id == id);

    if (retrievedProduct == null)
      return NotFound();

    return View(retrievedProduct);
  }

  [HttpPost]
  public IActionResult Edit(Product obj)
  {
    if (ModelState.IsValid)
    {
      _unitOfWork.Product.Update(obj);
      _unitOfWork.Save();
      TempData["success"] = "Product updated successfully";
      return RedirectToAction("Index");
    }

    return View(obj);
  }

  public IActionResult Delete(int? id)
  {
    if (id == null || id == 0)
      return NotFound();

    Product? retrievedProduct = _unitOfWork.Product.Get(u => u.Id == id);

    if (retrievedProduct == null)
      return NotFound();

    return View(retrievedProduct);
  }

  [HttpPost, ActionName("Delete")]
  public IActionResult DeletePOST(int? id)
  {
    Product? obj = _unitOfWork.Product.Get(u => u.Id == id);

    if (obj == null)
      return NotFound();

    _unitOfWork.Product.Remove(obj);
    _unitOfWork.Save();
    TempData["success"] = "Product deleted successfully";
    return RedirectToAction("Index");

  }
}
