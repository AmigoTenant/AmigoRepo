using System;
using System.Web.Http;
using Amigo.Tenant.Security.Api;
using Amigo.Tenant.Security.Permissions.Abstract;

namespace Amigo.Tenant.Application.Services.WebApi.Filters
{
    public class AmigoTenantClaimsAuthorizeAttribute: ClaimsAuthorizeAttribute
    {        
        /// <summary>
        /// Return the permission reader instance to be use by the AuthorizeAttribute
        /// </summary>
        /// <returns></returns>
        protected override IPermissionsReader GetPermissionsReader()
        {
            return (IPermissionsReader) GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IPermissionsReader));
        }
    }
}