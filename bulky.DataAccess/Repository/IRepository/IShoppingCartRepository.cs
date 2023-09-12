using bulkyBook.Models;

namespace bulky.DataAccess.Repository.IRepository;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
  void Update(ShoppingCart obj);
}
