using bulky.DataAccess.Data;
using bulky.DataAccess.Repository.IRepository;

namespace bulky.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
  private ApplicationDbContext _db;

  public ICategoryRepository Category { get; private set; }

  public UnitOfWork(ApplicationDbContext db)
  {
    _db = db;
    Category = new CategoryRepository(_db);
  }

  public ICategoryRepository CategoryRepository => throw new NotImplementedException();

  public void Save()
  {
    _db.SaveChanges();
  }
}
