using bulkyBook.Models;

namespace bulky.DataAccess.Repository.IRepository;

public interface IOrderHeaderRepository : IRepository
{
  void Update(OrderHeader obj);
}
