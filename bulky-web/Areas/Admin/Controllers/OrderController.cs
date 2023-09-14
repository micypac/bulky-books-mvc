using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using bulkyBook.Models;
using bulkyBook.Models.ViewModels;
using bulky.DataAccess.Repository.IRepository;
using bulky.Utility;
using Stripe;
using System.Diagnostics;
using System.Security.Claims;

namespace bulky_web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
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

  [HttpPost]
  [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
  public IActionResult StartProcessing()
  {
    _uow.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusProcessing);
    _uow.Save();

    TempData["success"] = "Order Details Updated Successfully.";
    return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
  }

  [HttpPost]
  [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
  public IActionResult ShipOrder()
  {
    var orderHeader = _uow.OrderHeader.Get(obj => obj.Id == OrderVM.OrderHeader.Id);
    orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
    orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
    orderHeader.OrderStatus = SD.StatusShipped;
    orderHeader.ShippingDate = DateTime.Now;

    if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
      orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));

    _uow.OrderHeader.Update(orderHeader);
    _uow.Save();

    TempData["success"] = "Order Shipped Successfully.";

    return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
  }

  [HttpPost]
  [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
  public IActionResult CancelOrder()
  {
    var orderHeader = _uow.OrderHeader.Get(obj => obj.Id == OrderVM.OrderHeader.Id);

    if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
    {
      var options = new RefundCreateOptions
      {
        Reason = RefundReasons.RequestedByCustomer,
        PaymentIntent = orderHeader.PaymentIntentId
      };

      var service = new RefundService();
      Refund refund = service.Create(options);

      _uow.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
    }
    else
    {
      _uow.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
    }

    _uow.Save();

    TempData["success"] = "Order Cancelled Successfully.";

    return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
  }


  #region API CALLS

  [HttpGet]
  public IActionResult GetAll(string status)
  {
    IEnumerable<OrderHeader> listOfOrderHeaders;

    if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
    {
      listOfOrderHeaders = _uow.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
    }
    else
    {
      var claimsIdentity = (ClaimsIdentity)User.Identity;
      var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

      listOfOrderHeaders = _uow.OrderHeader.GetAll(obj => obj.ApplicationUserId == userId, includeProperties: "ApplicationUser").ToList();
    }

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
