using bulky.DataAccess.Repository.IRepository;
using bulkyBook.Models;
using bulky.DataAccess.Data;

namespace bulky.DataAccess.Repository;

public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
{
  private ApplicationDbContext _db;

  public ApplicationUserRepository(ApplicationDbContext db) : base(db)
  {
    _db = db;
  }
}
