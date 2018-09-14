using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CacheManager.Core;
using Amigo.Tenant.Caching.Provider;
using CacheFactory = Amigo.Tenant.Caching.Provider.CacheFactory;

namespace Amigo.Tenant.Caching.Web.Filters
{
    public class CachingMasterDataAttribute: ActionFilterAttribute
    {        
        private readonly ICacheManager<object> _cacheManager;
        public bool VaryByUser { get; set; }

        public CachingMasterDataAttribute():this(CacheFactory.Current)
        {                                   
        }
        public CachingMasterDataAttribute(ICacheFactory cacheFactory)
        {            
            _cacheManager = cacheFactory.CreateCacheManager<object>();
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            var region = actionContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var method = actionContext.Request.Method;
            if (method == HttpMethod.Get && actionContext.Response!=null)
            {
                AddResponseToCache(actionContext, region);
                AddCacheHeaders(actionContext.ActionContext,DateTimeOffset.UtcNow);
            }
            else if (actionContext.Response != null && (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Delete))
            {
                InvalidateCache(actionContext, region);
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var region = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var method = actionContext.Request.Method;
            if (method != HttpMethod.Get) return;

            var cacheKey = GetCacheKey(actionContext);
            var cachedItem = _cacheManager.GetCacheItem(cacheKey, region);
            if (actionContext.Request.Headers.IfModifiedSince.HasValue && cachedItem!=null)
            {
                var header = actionContext.Request.Headers.IfModifiedSince.Value.UtcDateTime.ToString("R");
                var cache = cachedItem.CreatedUtc.ToString("R");
                if (header == cache)
                {
                    actionContext.Response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NotModified
                    };
                }
                else
                {
                    GetResponseFromCache(actionContext,cacheKey,region);
                    AddCacheHeaders(actionContext, cachedItem.CreatedUtc);
                }
            }
            else
            {
                var date = cachedItem?.CreatedUtc ?? DateTimeOffset.UtcNow;
                GetResponseFromCache(actionContext, cacheKey, region);                
            }
        }

        private string GetCacheKey(HttpActionContext actionContext)
        {
            var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            var id = GetUserId(identity);
            var cacheKey = VaryByUser
                ? $"{id}.{actionContext.Request.RequestUri.PathAndQuery}"
                : $"{actionContext.Request.RequestUri.PathAndQuery}";
            return cacheKey;
        }

        private static string GetUserId(ClaimsIdentity identity)
        {
            if (identity == null) return null;
            if (identity.Claims.All(x => x.Type != ClaimTypes.NameIdentifier)) return null;
            var id = identity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return id;
        }

        private void InvalidateCache(HttpActionExecutedContext actionExecutedContext,string region)
        {
            if (actionExecutedContext.Response == null || !actionExecutedContext.Response.IsSuccessStatusCode)
                return;            
            _cacheManager.ClearRegion(region);
        }

        private void AddResponseToCache(HttpActionExecutedContext actionContext, string region)
        {
            var cacheKey = GetCacheKey(actionContext.ActionContext);
            var objectContent = actionContext.Response.Content as ObjectContent;
            if (objectContent == null) return;
            var cachedItem = objectContent.Value;
            _cacheManager.Add(cacheKey, cachedItem, region);            
        }

        private void GetResponseFromCache(HttpActionContext actionContext, string cacheKey, string region)
        {
            var cachedItem = _cacheManager.GetCacheItem(cacheKey, region);
            if (cachedItem != null)
            {
                actionContext.Response = actionContext.Request?.CreateResponse(HttpStatusCode.OK, cachedItem.Value);
                AddCacheHeaders(actionContext,cachedItem.CreatedUtc);
            }
        }

        private void AddCacheHeaders(HttpActionContext actionContext,DateTimeOffset cachedDate)
        {
            actionContext.Response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(3)
            };
            actionContext.Response.Content.Headers.LastModified =cachedDate;
            if(actionContext.Response.Headers.CacheControl==null) actionContext.Response.Headers.CacheControl = new CacheControlHeaderValue();
            actionContext.Response.Headers.CacheControl.MaxAge = TimeSpan.Zero;
            actionContext.Response.Headers.CacheControl.MustRevalidate = true;            
        }
    }
}
