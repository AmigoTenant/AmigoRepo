using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Query.Common;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Abstract
{
    public class EFBaseRepository<T> :IRepository<T> where T:class
    {
        private readonly DbContext _context;

        public EFBaseRepository(DbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _context = context;
        }        
        public async Task<T> FirstAsync(Expression<Func<T, bool>> whereExpression, 
            OrderExpression<T>[] orderExpression = null,string[] includes=null)
        {
            DbQuery<T> baseQuery = _context.Set<T>();
            if (includes != null && includes.Any())
            {
                baseQuery = includes.Aggregate(baseQuery, (current, include) => current.Include(include));
            }

            var includedExpression =  baseQuery
                .Where(whereExpression);

            if (orderExpression != null && orderExpression.Any())
            {
                if (orderExpression.Length > 1)
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    ordered = orderExpression.Skip(1).Aggregate(ordered,ApplyOrderExpression);
                    return await ordered.FirstOrDefaultAsync();
                }
                else
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());                    
                    return await ordered.FirstOrDefaultAsync();
                }
            }
            return await includedExpression.FirstOrDefaultAsync();
        }

        protected virtual IQueryable<T> ApplyOrderExpression(IQueryable<T> query,OrderExpression<T> orderExpression)
        {
            if (orderExpression.Type == OrderType.Asc)
            {
                return query.OrderBy(orderExpression.Expression);
            }
            else
            {
                return query.OrderByDescending(orderExpression.Expression);
            }
        }

        protected virtual IQueryable<T> ApplyOrderExpression(IOrderedQueryable<T> query, OrderExpression<T> orderExpression)
        {
            if (orderExpression.Type == OrderType.Asc)
            {
                return query.ThenBy(orderExpression.Expression);
            }
            else
            {
                return query.ThenByDescending(orderExpression.Expression);
            }
        }

        public async Task<TOut> FirstAsync<TOut>(Expression<Func<T, bool>> whereExpression, OrderExpression<T>[] orderExpression = null, string[] includes = null,
            Expression<Func<T, TOut>> projectionExpression = null)
        {
            Contract.Assume(projectionExpression!=null);

            DbQuery<T> baseQuery = _context.Set<T>();
            if (includes != null && includes.Any())
            {
                baseQuery = includes.Aggregate(baseQuery, (current, include) => current.Include(include));
            }

            var includedExpression = baseQuery
                .Where(whereExpression);

            if (orderExpression != null && orderExpression.Any())
            {
                if (orderExpression.Length > 1)
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    ordered = orderExpression.Skip(1).Aggregate(ordered, ApplyOrderExpression);
                    return await ordered.Select(projectionExpression).FirstOrDefaultAsync();
                }
                else
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    return await ordered.Select(projectionExpression).FirstOrDefaultAsync();
                }
            }
            return await includedExpression.Select(projectionExpression).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ListAllAsync(OrderExpression<T>[] orderExpression = null, string[] includes = null)
        {
            DbQuery<T> baseQuery = _context.Set<T>();
            if (includes != null && includes.Any())
            {
                baseQuery = includes.Aggregate(baseQuery, (current, include) => current.Include(include));
            }

            var includedExpression = baseQuery;

            if (orderExpression != null && orderExpression.Any())
            {
                if (orderExpression.Length > 1)
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    ordered = orderExpression.Skip(1).Aggregate(ordered, ApplyOrderExpression);
                    return await ordered.ToListAsync();
                }
                else
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    return await ordered.ToListAsync();
                }
            }
            return await includedExpression.ToListAsync();
        }


        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> whereExpression, OrderExpression<T>[] orderExpression = null, string[] includes = null)
        {
            DbQuery<T> baseQuery = _context.Set<T>();
            if (includes != null && includes.Any())
            {
                baseQuery = includes.Aggregate(baseQuery, (current, include) => current.Include(include));
            }

            var includedExpression = baseQuery
                .Where(whereExpression);

            if (orderExpression != null && orderExpression.Any())
            {
                if (orderExpression.Length > 1)
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    ordered = orderExpression.Skip(1).Aggregate(ordered, ApplyOrderExpression);
                    return await ordered.ToListAsync();
                }
                else
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    return await ordered.ToListAsync();
                }
            }
            return await includedExpression.ToListAsync();
        }

        public async Task<IEnumerable<TOut>> ListAsync<TOut>(Expression<Func<T, bool>> whereExpression, OrderExpression<T>[] orderExpression = null, string[] includes = null,
            Expression<Func<T, TOut>> projectionExpression = null)
        {
            Contract.Assume(projectionExpression!=null);

            DbQuery<T> baseQuery = _context.Set<T>();
            if (includes != null && includes.Any())
            {
                baseQuery = includes.Aggregate(baseQuery, (current, include) => current.Include(include));
            }

            var includedExpression = baseQuery
                .Where(whereExpression);

            if (orderExpression != null && orderExpression.Any())
            {
                if (orderExpression.Length > 1)
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    ordered = orderExpression.Skip(1).Aggregate(ordered, ApplyOrderExpression);
                    return await ordered.Select(projectionExpression).ToListAsync();
                }
                else
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    return await ordered.Select(projectionExpression).ToListAsync();
                }
            }
            return await includedExpression.Select(projectionExpression).ToListAsync();
        }

        public async Task<PagedList<T>> ListPagedAsync(Expression<Func<T, bool>> whereExpression, int page, int pageSize,
            OrderExpression<T>[] orderExpression = null, string[] includes = null)
        {
            DbQuery<T> baseQuery = _context.Set<T>();
            if (includes != null && includes.Any())
            {
                baseQuery = includes.Aggregate(baseQuery, (current, include) => current.Include(include));
            }

            var includedExpression = baseQuery
                .Where(whereExpression);
            var total = 0;

            if (orderExpression != null && orderExpression.Any())
            {
                if (orderExpression.Length > 1)
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    ordered = orderExpression.Skip(1).Aggregate(ordered, ApplyOrderExpression);

                    total = await ordered.CountAsync();
                    var list = await ordered.Skip((page - 1)*pageSize).Take(pageSize).ToListAsync();
                    return new PagedList<T>()
                    {
                        Items = list,Page = page,Total = total
                    };
                }
                else
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    total = await ordered.CountAsync();
                    var list = await ordered.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                    return new PagedList<T>()
                    {
                        Items = list,
                        Page = page,
                        Total = total
                    };
                }
            }
            total = await includedExpression.CountAsync();
            var items = await includedExpression.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>()
            {
                Items = items,
                Page = page,
                Total = total
            };            
        }
        
        public async Task<long> CountAsync(Expression<Func<T, bool>> whereExpression = null)
        {
            return whereExpression == null
                ? await _context.Set<T>().LongCountAsync()
                : await _context.Set<T>().Where(whereExpression).LongCountAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> whereExpression = null)
        {
            return await _context.Set<T>().AnyAsync(whereExpression);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public virtual void Update(T entityToUpdate)
        {
            if (_context.Entry(entityToUpdate).State == EntityState.Detached)
                _context.Set<T>().Attach(entityToUpdate);

            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void UpdatePartial(T entityToUpdate, params string[] changedPropertyNames)
        {
            var entityEntry = _context.Entry(entityToUpdate);

            if (entityEntry.State != EntityState.Detached) entityEntry.State = EntityState.Detached;
            _context.Set<T>().Attach(entityToUpdate);                       

            foreach (var prop in changedPropertyNames)
            {
                _context.Entry(entityToUpdate).Property(prop).IsModified = true;
            }

            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public void UpdatePartialExcluding(T entity, params string[] noChangedPropertyNames)
        {
            var entityEntry = _context.Entry(entity);
            if (entityEntry.State != EntityState.Detached) entityEntry.State = EntityState.Detached;
            _context.Set<T>().Attach(entity);

            entityEntry.State = EntityState.Modified;

            foreach (var propertyName in noChangedPropertyNames)
                _context.Entry(entity).Property(propertyName).IsModified = false;

            _context.Configuration.ValidateOnSaveEnabled = false;
        } 

        public virtual void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entityToDelete);
            }
            _context.Entry(entityToDelete).State = EntityState.Deleted;            
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereExpression,
            OrderExpression<T>[] orderExpression = null, string[] includes = null)
        {
            DbQuery<T> baseQuery = _context.Set<T>();
            if (includes != null && includes.Any())
            {
                baseQuery = includes.Aggregate(baseQuery, (current, include) => current.Include(include));
            }

            var includedExpression = baseQuery
                .Where(whereExpression);

            if (orderExpression != null && orderExpression.Any())
            {
                if (orderExpression.Length > 1)
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    ordered = orderExpression.Skip(1).Aggregate(ordered, ApplyOrderExpression);
                    return await ordered.FirstOrDefaultAsync();
                }
                else
                {
                    var ordered = ApplyOrderExpression(includedExpression, orderExpression.First());
                    return await ordered.FirstOrDefaultAsync();
                }
            }
            return await includedExpression.FirstOrDefaultAsync();
        }
    }
}
