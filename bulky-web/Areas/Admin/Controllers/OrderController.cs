using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using bulkyBook.Models;
using bulkyBook.Models.ViewModels;
using bulky.DataAccess.Repository.IRepository;
using bulky.Utility;
using System.Diagnostics;

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
  public IActionResult GetAll(string status)
  {
    IEnumerable<OrderHeader> listOfOrderHeaders = _uow.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();

    switch (status)
    {
      case "pending":
        listOfOrderHeaders = listOfOrderHeaders.Where(item => item.PaymentStatus == SD.PaymentStatusDelayedPayment);
        break;
      case "inprocess":
        listOfOrderHeaders = listOfOrderHeaders.Where(item => item.OrderStatus == SD.StatusProcessing);
        break;
      case "completed":
        listOfOrderHeaders = listOfOrderHeaders.Where(item => item.OrderStatus == SD.StatusShipped);
        break;
      case "approved":
        listOfOrderHeaders = listOfOrderHeaders.Where(item => item.OrderStatus == SD.StatusApproved);
        break;
      default:
        break;
    }

    return Json(new { data = listOfOrderHeaders });
  }

  #endregion
}
