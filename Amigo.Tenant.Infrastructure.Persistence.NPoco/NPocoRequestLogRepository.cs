using System;
using NPoco;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.NPoco.Abstract;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco
{
    public class NPocoRequestLogRepository: NPocoDataAccess<RequestLog>,IRepository<RequestLog>
    {
        private readonly IDatabase _database;

        public NPocoRequestLogRepository(IDatabase database) : base(database)
        {
            _database = database;
        }

        public void Add(RequestLog entity)
        {
            _database.Insert(entity);
        }

        public void Update(RequestLog entity)
        {
            _database.Update(entity);
        }

        public void UpdatePartial(RequestLog entity, params string[] changedPropertyNames)
        {
            throw new NotImplementedException();
        }

        public void Delete(RequestLog entity)
        {
            _database.Delete(entity);
        }
    }
}
