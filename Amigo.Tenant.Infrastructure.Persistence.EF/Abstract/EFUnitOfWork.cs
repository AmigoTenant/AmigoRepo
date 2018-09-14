using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Abstract
{
    public class EFUnitOfWork: IUnitOfWork
    {
        private readonly DbContext _context;

        public EFUnitOfWork(DbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                foreach (var failure in dbEx.EntityValidationErrors)
                {
                    var validationErrors = failure.ValidationErrors.Aggregate("", (current, error) => current + (error.PropertyName + "  " + error.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                StringBuilder msg = new StringBuilder();
                foreach (var failure in dbEx.EntityValidationErrors)
                {
                    var validationErrors = failure.ValidationErrors.Aggregate("", (current, error) => current + (error.PropertyName + "  " + error.ErrorMessage));
                    msg.AppendLine(validationErrors);
                }
                throw new Exception(msg.ToString(),dbEx.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex.InnerException);
            }
        }

        public void Rollback()
        {
            _context.ChangeTracker.DetectChanges();
            var entries = _context.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged).ToList();

            foreach (var dbEntityEntry in entries)
            {
                var entity = dbEntityEntry.Entity;

                if (entity == null) continue;

                if (dbEntityEntry.State == EntityState.Added)
                {
                    var dbSet = _context.Set(entity.GetType());
                    dbSet.Remove(entity);
                }
                else if (dbEntityEntry.State == EntityState.Modified)
                {
                    dbEntityEntry.Reload();
                }
                else if (dbEntityEntry.State == EntityState.Deleted)
                    dbEntityEntry.State = EntityState.Unchanged;
            }
        }
    }
}
