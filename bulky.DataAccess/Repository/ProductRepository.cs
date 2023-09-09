using bulky.DataAccess.Data;
using bulky.DataAccess.Repository;
using bulky.DataAccess.Repository.IRepository;
using bulkyBook.Models;

namespace bulky.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
  private ApplicationDbContext _db;

  public ProductRepository(ApplicationDbContext db) : base(db)
  {
    _db = db;
  }

  public void Update(Product obj)
  {
    _db.Products.Update(obj);
  }
}
