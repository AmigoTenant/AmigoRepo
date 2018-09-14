using System.Collections.Generic;
using System.Threading.Tasks;
using CacheManager.Core;
using MediatR;
using Amigo.Tenant.Caching.Provider;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Common.ServiceConstants.Security;
using Amigo.Tenant.Events.Security;

namespace Amigo.Tenant.EventHandlers.Security
{
    public class RoleEditedEventHandler : 
        IAsyncNotificationHandler<RoleEditionEvent>, 
        IAsyncNotificationHandler<PermissionDeleted>, 
        IAsyncNotificationHandler<PermissionRegistered>
    {
        private readonly ICacheManager<List<Permission>> _cacheManager;

        public RoleEditedEventHandler(ICacheFactory cacheFactory)
        {
            _cacheManager = cacheFactory.CreateCacheManager<List<Permission>>();
        }

        public async Task Handle(RoleEditionEvent notification)
        {
            await CleanCache();
        }

        public async Task Handle(PermissionDeleted notification)
        {
            await CleanCache();
        }

        public async Task Handle(PermissionRegistered notification)
        {
            await CleanCache();
        }

        private async Task CleanCache()
        {
            await Task.Run(() => _cacheManager.Remove(CacheKeys.PermissionListKey));
        }
    }
}
