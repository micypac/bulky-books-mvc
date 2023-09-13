using bulkyBook.Models;

namespace bulky.DataAccess.Repository.IRepository;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
  void Update(OrderHeader obj);
}
