using bulky.DataAccess.Repository.IRepository;
using bulkyBook.Models;
using bulky.DataAccess.Data;

namespace bulky.DataAccess.Repository;

public class OderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
  private ApplicationDbContext _db;

  public OderHeaderRepository(ApplicationDbContext db) : base(db)
  {
    _db = db;
  }

  public void Update(OrderHeader obj)
  {
    _db.OrderHeaders.Update(obj);
  }
}
