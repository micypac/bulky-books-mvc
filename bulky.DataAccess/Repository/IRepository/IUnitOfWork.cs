using bulky.DataAccess.Repository.IRepository;

namespace bulky.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
  ICategoryRepository Category { get; }

  void Save();
}
