using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amigo.Tenant.Query.Common;

namespace Amigo.Tenant.Infrastructure.Persistence.Abstract
{
    public interface IQueryDataAccess<T> where T:class 
    {
        Task<T> FirstAsync(Expression<Func<T, bool>> whereExpression,OrderExpression<T>[] orderExpression = null, string[] includes = null);

        Task<TOut> FirstAsync<TOut>(Expression<Func<T, bool>> whereExpression,
            OrderExpression<T>[] orderExpression = null, string[] includes = null, Expression<Func<T, TOut>> projectionExpression = null);

        Task<IEnumerable<T>> ListAllAsync(OrderExpression<T>[] orderExpression = null, string[] includes = null);

        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> whereExpression, OrderExpression<T>[] orderExpression = null, string[] includes = null);

        Task<IEnumerable<TOut>> ListAsync<TOut>(Expression<Func<T, bool>> whereExpression,
            OrderExpression<T>[] orderExpression = null, string[] includes = null, Expression<Func<T, TOut>> projectionExpression = null);

        Task<PagedList<T>> ListPagedAsync(Expression<Func<T, bool>> whereExpression,int page,int pageSize,OrderExpression<T>[] orderExpression = null, string[] includes = null);

        Task<long> CountAsync(Expression<Func<T, bool>> whereExpression = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>> whereExpression);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereExpression, OrderExpression<T>[] orderExpression = null, string[] includes = null);
    }
}
