using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Amigo.Tenant.Application.Services.WebApi.Helpers.Caching
{
    public class LastModifiedCachingAttribute: ActionFilterAttribute
    {
        public static Dictionary<string,string> LastModifiedCache = new Dictionary<string, string>();
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext.Request.Method == HttpMethod.Options || actionContext.Request.Method == HttpMethod.Trace) return;

            var ifModified = actionContext.Request.Headers.Any(x => string.Equals(x.Key, "If-Modified-Since", StringComparison.InvariantCultureIgnoreCase));
            if (!ifModified)return;

            var headerValue = actionContext.Request.Headers.First(x => string.Equals(x.Key, "If-Modified-Since", StringComparison.InvariantCultureIgnoreCase)).Key;
            var url = actionContext.Request.RequestUri.PathAndQuery;
            var lastModified = LastModifiedCache[url];            
            if(lastModified == null)return;

            if(!string.Equals(headerValue,lastModified,StringComparison.InvariantCultureIgnoreCase))return;

            actionContext.Response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotModified
            };            
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
    }    
}