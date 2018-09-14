using System.Threading.Tasks;

namespace XPO.ShuttleTracking.Domain.QueryModel.Abstract
{
    public interface IQueryHandler<TSearch,TOut> where TSearch:class where TOut:class 
    {
        Task<TOut> Handle(TSearch search);
    }

    public interface IQueryHandler<TOut> where TOut : class
    {
        Task<TOut> Handle();
    }
}
