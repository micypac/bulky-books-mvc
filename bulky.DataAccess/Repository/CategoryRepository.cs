namespace bulky.DataAccess;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using bulky.DataAccess.Data;
using bulky.DataAccess.Repository;
using bulky.DataAccess.Repository.IRepository;
using bulkyBook.Models;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
  private ApplicationDbContext _db;

  public CategoryRepository(ApplicationDbContext db) : base(db)
  {
    _db = db;
  }

  public void Save()
  {
    _db.SaveChanges();
  }

  public void Update(Category obj)
  {
    _db.Categories.Update(obj);
  }
}
