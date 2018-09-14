using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Amigo.Tenant.Infrastructure.Persistence.Extensions
{
    //public static class LinqExtensions
    //{
    //    //public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    //    //{
    //    //    var parameter = Expression.Parameter(typeof(T));
    //    //    // return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(first,second), parameter);

    //    //}
    //    public static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    //    {
    //        var toInvoke = Expression.Invoke(second, first.Parameters);

    //        return (Expression<Func<T, bool>>)Expression.Lambda(Expression.AndAlso(first.Body, toInvoke), first.Parameters);
    //    }
    //    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    //    {
    //        var parameter = Expression.Parameter(typeof(T));
    //        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(first, second), parameter);
    //    }

    //    private class ExpressionParameterReplacer : ExpressionVisitor
    //    {
    //        public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
    //        {
    //            ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
    //            for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
    //                ParameterReplacements.Add(fromParameters[i], toParameters[i]);
    //        }

    //        private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }

    //        protected override Expression VisitParameter(ParameterExpression node)
    //        {
    //            ParameterExpression replacement;
    //            if (ParameterReplacements.TryGetValue(node, out replacement))
    //                node = replacement;
    //            return base.VisitParameter(node);
    //        }
    //    }

    //}

}
