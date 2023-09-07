namespace bulky.DataAccess.Repository.IRepository;
using bulkyBook.Models;

public interface ICategoryRepository : IRepository<Category>
{
  void Update(Category obj);
}
