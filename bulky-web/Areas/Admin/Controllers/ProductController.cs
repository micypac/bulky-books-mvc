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
  private readonly IWebHostEnvironment _webHostEnvironment; // for image uploads

  // constructor
  public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
  {
    _unitOfWork = unitOfWork;
    _webHostEnvironment = webHostEnvironment;
  }


  // methods

  public IActionResult Index()
  {
    List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

    return View(objProductList);
  }

  // combined CREATE and UPDATE
  public IActionResult Upsert(int? id)
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

    if (id == null || id == 0)
    {
      // create 
      return View(productVM);
    }
    else
    { // update
      productVM.Product = _unitOfWork.Product.Get(obj => obj.Id == id);
      return View(productVM);
    }

  }

  [HttpPost]
  public IActionResult Upsert(ProductVM obj, IFormFile? attachedFile)
  {
    if (ModelState.IsValid)
    {
      // create and store the product image file
      string wwwRootPath = _webHostEnvironment.WebRootPath;
      if (attachedFile != null)
      {
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(attachedFile.FileName);
        string productPath = Path.Combine(wwwRootPath, @"images/product");

        // on *Update*, delete old image
        if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
        {
          var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('/'));

          if (System.IO.File.Exists(oldImagePath))
            System.IO.File.Delete(oldImagePath);
        }

        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
        {
          attachedFile.CopyTo(fileStream);
        }

        obj.Product.ImageUrl = @"/images/product/" + fileName;
      }

      if (obj.Product.Id == 0)
        _unitOfWork.Product.Add(obj.Product);
      else
        _unitOfWork.Product.Update(obj.Product);

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

  // public IActionResult Edit(int? id)
  // {
  //   if (id == null || id == 0)
  //     return NotFound();

  //   Product? retrievedProduct = _unitOfWork.Product.Get(u => u.Id == id);

  //   if (retrievedProduct == null)
  //     return NotFound();

  //   return View(retrievedProduct);
  // }

  // [HttpPost]
  // public IActionResult Edit(Product obj)
  // {
  //   if (ModelState.IsValid)
  //   {
  //     _unitOfWork.Product.Update(obj);
  //     _unitOfWork.Save();
  //     TempData["success"] = "Product updated successfully";
  //     return RedirectToAction("Index");
  //   }

  //   return View(obj);
  // }

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
