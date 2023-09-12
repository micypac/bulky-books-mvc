using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using bulkyBook.Models;
using bulkyBook.Models.ViewModels;
using bulky.DataAccess.Repository.IRepository;
using bulky.Utility;


namespace bulky_web.Areas.Admin.Controllers;

[Area("Admin")]
public class CompanyController : Controller
{
  private readonly IUnitOfWork _unitOfWork;

  public CompanyController(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  public IActionResult Index()
  {
    List<Company> companies = _unitOfWork.Company.GetAll().ToList();

    return View(companies);
  }

  public IActionResult Upsert(int? id)
  {
    if (id == null || id == 0)
      return View(new Company());

    Company company = _unitOfWork.Company.Get(obj => obj.Id == id);
    return View(company);
  }

  [HttpPost]
  public IActionResult Upsert(Company companyObj)
  {
    if (ModelState.IsValid)
    {
      if (companyObj.Id == 0)
        _unitOfWork.Company.Add(companyObj);
      else
        _unitOfWork.Company.Update(companyObj);

      _unitOfWork.Save();
      TempData["success"] = "Company created successfully";
      return RedirectToAction("Index");
    }

    return View(companyObj);
  }

  #region API CALLS

  public IActionResult GetAll()
  {
    List<Company> companyList = _unitOfWork.Company.GetAll().ToList();
    return Json(new { data = companyList });
  }

  public IActionResult Delete(int? id)
  {
    var companyToDelete = _unitOfWork.Company.Get(obj => obj.Id == id);
    if (companyToDelete == null)
      return Json(new { success = false, message = "Error while deleting" });

    _unitOfWork.Company.Remove(companyToDelete);
    _unitOfWork.Save();

    return Json(new { success = true, message = "Delete successful" });
  }

  #endregion
}
