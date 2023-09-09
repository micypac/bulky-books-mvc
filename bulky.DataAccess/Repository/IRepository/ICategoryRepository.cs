using bulkyBook.Models;

namespace bulky.DataAccess.Repository.IRepository;

public interface ICategoryRepository : IRepository<Category>
{
  void Update(Category obj);
}
