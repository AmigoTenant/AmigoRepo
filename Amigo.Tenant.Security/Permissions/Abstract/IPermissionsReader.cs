using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Security.Permissions.Abstract
{
    public interface IPermissionsReader
    {
        Task<List<Permission>> GetAllPermissionsWithActionsAsync();
        Task UpdatePermissionsCache();
    }
}
