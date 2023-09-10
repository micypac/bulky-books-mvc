using bulky.DataAccess.Data;
using bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace bulky.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
  // fields
  private readonly ApplicationDbContext _db;
  internal DbSet<T> dbSet;

  // constructor
  public Repository(ApplicationDbContext db)
  {
    _db = db;
    this.dbSet = _db.Set<T>(); // _db.Categories == dbSet

    _db.Products.Include(prod => prod.Category).Include(prod => prod.CategoryId);
  }

  public void Add(T entity)
  {
    dbSet.Add(entity);
  }

  public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
  {
    IQueryable<T> query = dbSet;
    query = query.Where(filter);

    if (!string.IsNullOrEmpty(includeProperties))
    {
      foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
      {
        query = query.Include(prop);
      }
    }

    return query.FirstOrDefault();
  }

  public IEnumerable<T> GetAll(string? includeProperties = null)
  {
    IQueryable<T> query = dbSet;

    if (!string.IsNullOrEmpty(includeProperties))
    {
      foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
      {
        query = query.Include(prop);
      }
    }

    return query.ToList();
  }

  public void Remove(T entity)
  {
    dbSet.Remove(entity);
  }

  public void RemoveRange(IEnumerable<T> entities)
  {
    dbSet.RemoveRange(entities);
  }
}