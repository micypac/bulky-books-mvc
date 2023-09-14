using bulkyBook.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bulkyBook.Models.ViewModels;

public class OrderVM
{
  public OrderHeader OrderHeader { get; set; }
  public IEnumerable<OrderDetail> OrderDetails { get; set; }
}
