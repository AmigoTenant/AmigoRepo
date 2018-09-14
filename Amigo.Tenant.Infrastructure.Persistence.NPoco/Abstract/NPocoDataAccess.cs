using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NPoco;
using NPoco.Linq;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Query.Common;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Abstract
{
    public class NPocoDataAccess<T> : IQueryDataAccess<T> where T:class 
    {
        private readonly IDatabase _database;

        public NPocoDataAccess(IDatabase database)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            _database = database;
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> whereExpression = null)
        {
            return await _database.Query<T>().Where(whereExpression).CountAsync();
        }

        public async Task<T> FirstAsync(Expression<Func<T, bool>> whereExpression, OrderExpression<T>[] orderExpression = null, string[] includes = null)
        {
            var baseQuery = _database.Query<T>()
                .Where(whereExpression);
            if(orderExpression != null && orderExpression.Any())
            {
                baseQuery = orderExpression.Aggregate(baseQuery, ApplyOrderStatement);
            }
            return await baseQuery.FirstAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereExpression, OrderExpression<T>[] orderExpression = null, string[] includes = null)
        {
            var baseQuery = _database.Query<T>()
                .Where(whereExpression);
            if (orderExpression != null && orderExpression.Any())
            {
                baseQuery = orderExpression.Aggregate(baseQuery, ApplyOrderStatement);
            }
            return await baseQuery.FirstOrDefaultAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> whereExpression)
        {
            var any = _database.Query<T>().Where(whereExpression)
                .AnyAsync();
            return any;
        }

        public async Task<TOut> FirstAsync<TOut>(Expression<Func<T, bool>> whereExpression, OrderExpression<T>[] orderExpression = null, string[] includes = null, Expression<Func<T, TOut>> projectionExpression = null)
        {
            var baseQuery = _database.Query<T>()
                .Where(whereExpression);
            baseQuery = ApplySorting(orderExpression, baseQuery);
            return (await baseQuery.Limit(1).ProjectToAsync(projectionExpression)).FirstOrDefault();
        }

        public async Task<IEnumerable<T>> ListAllAsync(OrderExpression<T>[] orderExpression = null, string[] includes = null)
        {
            var baseQuery = _database.Query<T>() as IQueryProvider<T>;

            baseQuery = ApplySorting(orderExpression, baseQuery);

            return await baseQuery.ToListAsync();
        }

        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> whereExpression, OrderExpression<T>[] orderExpression = null, string[] includes = null)
        {
            var baseQuery = _database.Query<T>()
                .Where(whereExpression);
            baseQuery = ApplySorting(orderExpression, baseQuery);
            return await baseQuery.ToListAsync();
        }

        public async Task<IEnumerable<TOut>> ListAsync<TOut>(Expression<Func<T, bool>> whereExpression, OrderExpression<T>[] orderExpression = null, string[] includes = null, Expression<Func<T, TOut>> projectionExpression = null)
        {
            var baseQuery = _database.Query<T>().Where(whereExpression);
            baseQuery = ApplySorting(orderExpression, baseQuery);
            return await baseQuery.ProjectToAsync(projectionExpression);
        }

        public async Task<PagedList<T>> ListPagedAsync(Expression<Func<T, bool>> whereExpression, int page, int pageSize, OrderExpression<T>[] orderExpression = null, string[] includes = null)
        {
            var baseQuery = _database.Query<T>().Where(whereExpression);
            baseQuery = ApplySorting(orderExpression, baseQuery);
            var pagedResult = await baseQuery.ToPageAsync(page,pageSize);
            
            return new PagedList<T>()
            {
                Page = page,
                PageSize = pageSize,
                Total = (int)pagedResult.TotalItems,
                Items = pagedResult.Items
            };
        }
        #region helper methods
        private IQueryProvider<T> ApplyOrderStatement(IQueryProvider<T> query, OrderExpression<T> order)
        {
            return order.Type == OrderType.Asc ? query.OrderBy(order.Expression) : query.OrderByDescending(order.Expression);
        }

        private IQueryProvider<T> ApplyThenOrderStatement(IQueryProvider<T> query, OrderExpression<T> order)
        {
            return order.Type == OrderType.Asc ? query.ThenBy(order.Expression) : query.ThenByDescending(order.Expression);
        }
        private IQueryProvider<T> ApplySorting(OrderExpression<T>[] orderExpression, IQueryProvider<T> baseQuery)
        {
            if (orderExpression == null || !orderExpression.Any()) return baseQuery;

            for (var i = 0; i < orderExpression.Length; i++)
            {
                baseQuery = i == 0 ? ApplyOrderStatement(baseQuery,orderExpression[i]) : ApplyThenOrderStatement(baseQuery,orderExpression[i]);
            }
            return baseQuery;
        }
        #endregion
    }
}
