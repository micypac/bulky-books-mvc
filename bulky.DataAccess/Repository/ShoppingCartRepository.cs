using bulky.DataAccess.Repository.IRepository;
using bulkyBook.Models;
using bulky.DataAccess.Data;

namespace bulky.DataAccess.Repository;

public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
{
  private ApplicationDbContext _db;

  public ShoppingCartRepository(ApplicationDbContext db) : base(db)
  {
    _db = db;
  }

  public void Update(ShoppingCart obj)
  {
    _db.ShoppingCarts.Update(obj);
  }
}
