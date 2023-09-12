using bulkyBook.Models;

namespace bulky.DataAccess.Repository.IRepository;

public interface ICompanyRepository : IRepository<Company>
{
  void Update(Company obj);
}
