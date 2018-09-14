using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Amigo.Tenant.Security.Api
{
    public class AuthorizeAttribute : AuthorizationFilterAttribute
    {
        public string Roles { get; set; }
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {

            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if (!principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }

            if (!(principal.HasClaim(x => x.Type.EndsWith("role",StringComparison.CurrentCultureIgnoreCase) && x.Value == Roles)))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                return Task.FromResult<object>(null);
            }
            //User is Authorized, complete execution
            return Task.FromResult<object>(null);
        }
    }
}
