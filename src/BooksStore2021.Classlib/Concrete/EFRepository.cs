using BooksStore2021.Classlib.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore2021.Classlib.Concrete
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly EFDbContext _db;

        public EFRepository(EFDbContext db)
        {
            _db = db;
        }


        public async Task<T> FindByIdAsync(long id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = _db.Set<T>();
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
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = _db.Set<T>();
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
                query = orderBy(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public void Add(T entity)
        {
            _db.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _db.Set<T>().Update(entity);
        }
        public void Remove(T entity)
        {
            _db.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _db.Set<T>().RemoveRange(entity);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
