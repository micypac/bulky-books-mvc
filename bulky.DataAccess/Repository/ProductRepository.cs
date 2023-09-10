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
    // _db.Products.Update(obj);

    var retrievedProduct = _db.Products.FirstOrDefault(x => x.Id == obj.Id);

    if (retrievedProduct != null)
    {
      retrievedProduct.Title = obj.Title;
      retrievedProduct.Description = obj.Description;
      retrievedProduct.ISBN = obj.ISBN;
      retrievedProduct.Author = obj.Author;
      retrievedProduct.ListPrice = obj.ListPrice;
      retrievedProduct.Price = obj.Price;
      retrievedProduct.Price50 = obj.Price50;
      retrievedProduct.Price100 = obj.Price100;
      retrievedProduct.CategoryId = obj.CategoryId;

      if (obj.ImageUrl != null)
        retrievedProduct.ImageUrl = obj.ImageUrl;
    }

  }
}
