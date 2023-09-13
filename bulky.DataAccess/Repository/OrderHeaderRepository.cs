using bulky.DataAccess.Data;
using bulky.DataAccess.Repository.IRepository;
using bulkyBook.Models;

namespace bulky.DataAccess.Repository;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
  private ApplicationDbContext _db;

  public OrderHeaderRepository(ApplicationDbContext db) : base(db)
  {
    _db = db;
  }

  public void Update(OrderHeader obj)
  {
    _db.OrderHeaders.Update(obj);
  }
}
