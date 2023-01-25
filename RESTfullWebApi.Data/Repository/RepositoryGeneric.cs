using Microsoft.EntityFrameworkCore;
using RESTfullWebApi.Data;
using RESTfullWebApi.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRecord.Data.Data.IRepository
{
  public class RepositoryGeneric<T> : IRepositoryGeneric<T> where T : class
  {
    private readonly DataContext _context;
    public DbSet<T> dbSet; 
    
    public RepositoryGeneric(DataContext context)
    {
      _context = context;
      dbSet = _context.Set<T>();
    }
    public async Task<T> Add(T entity)
    {
      await dbSet.AddAsync(entity);
      return entity;
    }

    public async Task AddRange(IEnumerable<T> entity)
    {
      await dbSet.AddRangeAsync(entity);
    }

    public async Task<bool> Exists(Expression<Func<T, bool>> filter = null)
    {
      return await dbSet.AnyAsync(filter);
    }

    public async Task<T> Get(int id)
    {
      return await dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
    {
      IQueryable<T> query = dbSet;

      if (filter != null)
      {
        query = query.Where(filter);
      }

      if (includeProperties != null)
      {
        foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
          query = query.Include(includeProp);
        }
      }

      if (orderBy != null)
      {
        return await orderBy(query).ToListAsync();
      }

      return await query.ToListAsync();
    }


    public async Task<T> FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
    {
      IQueryable<T> query = dbSet;

      if (filter != null)
      {
        query = query.Where(filter);
      }

      if (includeProperties != null)
      {
        foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
          query = query.Include(includeProp);
        }
      }

      return await query.FirstOrDefaultAsync();
    }

    public void Remove(int id)
    {
      T entity = dbSet.Find(id);

      if (entity != null)
      {
        Remove(entity);
      }
    }

    public void Remove(T entity)
    {
      dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
      dbSet.RemoveRange(entity);
    }

    public void Update(T entity)
    {
      //dbSet.Update(entity);
      dbSet.Attach(entity);
      _context.Entry(entity).State = EntityState.Modified;
    }

    public void Dispose()
    {
      _context.Dispose();
      GC.SuppressFinalize(this);
    }

  }
}
