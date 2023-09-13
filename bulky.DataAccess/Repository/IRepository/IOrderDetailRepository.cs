using bulkyBook.Models;

namespace bulky.DataAccess.Repository.IRepository;

public interface IOrderDetailRepository : IRepository<OrderDetail>
{
  void Update(OrderDetail obj);
}
