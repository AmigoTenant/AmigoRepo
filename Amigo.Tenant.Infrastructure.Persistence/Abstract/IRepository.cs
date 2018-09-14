using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.Abstract
{
    public interface IRepository<T>: IQueryDataAccess<T> where T:class
    {
        void Add(T entity);
        void Update(T entity);
        void UpdatePartial(T entity, params string[] changedPropertyNames);
        void Delete(T entity);
    }
}