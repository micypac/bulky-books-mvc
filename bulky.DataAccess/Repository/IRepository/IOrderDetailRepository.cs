using bulkyBook.Models;

namespace bulky.DataAccess.Repository.IRepository;

public interface IOrderDetailRepository : IRepository
{
  void Update(OrderDetail obj);
}
