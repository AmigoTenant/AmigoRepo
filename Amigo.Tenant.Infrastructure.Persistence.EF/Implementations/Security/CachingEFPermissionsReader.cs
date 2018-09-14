using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Common.ServiceConstants.Security;
using Amigo.Tenant.Infrastructure.Persistence.EF.Context;
using Amigo.Tenant.Security.Permissions.Abstract;
using CacheFactory = Amigo.Tenant.Caching.Provider.CacheFactory;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Implementations.Security
{
    public class CachingEFPermissionsReader: IPermissionsReader
    {        
        private readonly ICacheManager<List<Permission>> _cacheManager;

        public CachingEFPermissionsReader()
        {            
            _cacheManager = CacheFactory.Current.CreateCacheManager<List<Permission>>();
        }

        public async Task<List<Permission>> GetAllPermissionsWithActionsAsync()
        {
            var list =  _cacheManager.Get<List<Permission>>(CacheKeys.PermissionListKey);
            if (list == null)
            {
                list = await QueryAllPermissions().ConfigureAwait(false);
                _cacheManager.AddOrUpdate(CacheKeys.PermissionListKey,list,(original)=> list);                
            }
            return list;
        }

        protected async Task<List<Permission>> QueryAllPermissions()
        {
            using (var ctx = new AmigoTenantDbContext())
            {
                var permissions = await ctx.Set<Permission>().AsNoTracking()
                    .Include(x => x.Action)
                    .Include(x=> x.AmigoTenantTRole)
                    .Where(x => x.AmigoTenantTRole.RowStatus == true).ToListAsync().ConfigureAwait(false);

                return permissions;
            }
        }

        public async Task UpdatePermissionsCache()
        {
            var list = await QueryAllPermissions().ConfigureAwait(false);
            _cacheManager.AddOrUpdate(CacheKeys.PermissionListKey, list, (original) => list);
        }
    }
}
