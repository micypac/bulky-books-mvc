using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using bulkyBook.Models;
using bulkyBook.Models.ViewModels;
using bulky.DataAccess.Repository.IRepository;
using bulky.Utility;

namespace bulky_web.Areas.Admin.Controllers;

[Area("Admin")]
public class OrderController : Controller
{
  private readonly IUnitOfWork _uow;

  public OrderController(IUnitOfWork uow)
  {
    _uow = uow;
  }

  public IActionResult Index()
  {
    return View();
  }



  #region API CALLS

  [HttpGet]
  public IActionResult GetAll()
  {
    List<OrderHeader> listOfOrderHeaders = _uow.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();

    return Json(new { data = listOfOrderHeaders });
  }

  #endregion
}
