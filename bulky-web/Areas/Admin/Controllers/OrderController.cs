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

  [BindProperty]
  public OrderVM OrderVM { get; set; }

  public OrderController(IUnitOfWork uow)
  {
    _uow = uow;
  }

  public IActionResult Index()
  {
    return View();
  }

  public IActionResult Details(int orderId)
  {
    OrderVM = new()
    {
      OrderHeader = _uow.OrderHeader.Get(item => item.Id == orderId, includeProperties: "ApplicationUser"),
      OrderDetails = _uow.OrderDetail.GetAll(item => item.OrderHeaderId == orderId, includeProperties: "Product")
    };

    return View(OrderVM);
  }

  [HttpPost]
  [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
  public IActionResult UpdateOrderDetail()
  {
    var orderHeaderFromDb = _uow.OrderHeader.Get(obj => obj.Id == OrderVM.OrderHeader.Id);

    orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
    orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
    orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
    orderHeaderFromDb.City = OrderVM.OrderHeader.City;
    orderHeaderFromDb.State = OrderVM.OrderHeader.State;
    orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;

    if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
      orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;

    if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
      orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;

    _uow.OrderHeader.Update(orderHeaderFromDb);
    _uow.Save();

    TempData["success"] = "Order Details Updated Successfully.";

    return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });
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
