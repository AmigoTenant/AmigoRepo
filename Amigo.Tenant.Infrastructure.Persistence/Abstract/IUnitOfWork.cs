using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.Abstract
{
    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync();
        void Rollback();
    }
}