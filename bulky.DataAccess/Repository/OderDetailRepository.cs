using bulky.DataAccess.Repository.IRepository;
using bulkyBook.Models;
using bulky.DataAccess.Data;

namespace bulky.DataAccess.Repository;

public class OderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
{
  private ApplicationDbContext _db;

  public OderDetailRepository(ApplicationDbContext db) : base(db)
  {
    _db = db;
  }

  public void Update(OrderDetail obj)
  {
    _db.OrderDetails.Update(obj);
  }
}
