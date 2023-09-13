﻿using bulky.DataAccess.Data;
using bulky.DataAccess.Repository.IRepository;
using bulkyBook.Models;

namespace bulky.DataAccess.Repository;

public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
{
  private ApplicationDbContext _db;

  public OrderDetailRepository(ApplicationDbContext db) : base(db)
  {
    _db = db;
  }

  public void Update(OrderDetail obj)
  {
    _db.OrderDetails.Update(obj);
  }
}
