using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Amigo.Tenant.IdentityServer.Infrastructure.ViewService.ActionResults;

namespace Amigo.Tenant.IdentityServer.Controllers
{
    [AllowAnonymous]
    public class WelcomeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Index()
        {
            return new WelcomeActionResult(Request.GetOwinContext());
        }
    }
}
