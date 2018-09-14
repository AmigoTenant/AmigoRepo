using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Properties;
using Amigo.Tenant.Security.Permissions.Abstract;

namespace Amigo.Tenant.Security.Api
{
    public abstract class ClaimsAuthorizeAttribute: System.Web.Http.AuthorizeAttribute
    {
        public string ActionCode { get; set; }
        protected abstract IPermissionsReader GetPermissionsReader();
        protected ClaimsAuthorizeAttribute()
        {
            _permissionsReaderlazy = new Lazy<IPermissionsReader>(GetPermissionsReader);
        }
        private readonly Lazy<IPermissionsReader> _permissionsReaderlazy;
        protected IPermissionsReader PermissionsReader => _permissionsReaderlazy.Value;
        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (!IsAuthorized(actionContext))
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }
            var user = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            if (user == null)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            var roleClaim = user.Claims.LastOrDefault(x => x.Type == ClaimTypes.Role);// && x.Value.StartsWith(RolePrefix));
            if (roleClaim == null)
            {
                HandleForbiddenRequest(actionContext);                
                return;
            }
            var roleName = roleClaim.Value;

            if (string.IsNullOrWhiteSpace(ActionCode)) return;

            var permissions = (await PermissionsReader.GetAllPermissionsWithActionsAsync()).Where(x=> x.AmigoTenantTRole.Code == roleName).ToList();
            var actions = permissions.Select(x => x.Action).ToList();

            if (actions.All(x => x.Code != ActionCode))
                HandleForbiddenRequest(actionContext);
            
        }

        private void HandleForbiddenRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden,"You don´t have permissions to this service.");
        }
    }
}
