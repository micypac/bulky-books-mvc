using bulky.DataAccess.Repository.IRepository;
using bulky.DataAccess.Data;
using bulkyBook.Models;

namespace bulky.DataAccess.Repository;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
  private ApplicationDbContext _db;

  public CompanyRepository(ApplicationDbContext db) : base(db)
  {
    _db = db;
  }

  public void Update(Company company)
  {
    _db.Companies.Update(company);
  }
}
