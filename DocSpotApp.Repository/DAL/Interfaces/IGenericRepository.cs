﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DocSpotApp.Repository.DAL.Interfaces
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null,
                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                string includeProperties = "");
        (IEnumerable<T> items, int total) GetAll(int pageNo, int perPage, Expression<Func<T, bool>> filter = null,
                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                string includeProperties = "");
        Task<T> GetById(int id );
        Task<List<T>> Read();
        Task<T> Add(T item);
        Task<T> Update(T item);
        Task<T> Delete(T item);
    }


}
