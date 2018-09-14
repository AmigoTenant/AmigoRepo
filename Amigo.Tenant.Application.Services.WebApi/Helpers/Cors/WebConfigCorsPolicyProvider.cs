using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace Amigo.Tenant.Application.Services.WebApi.Helpers.Cors
{
    public class WebConfigCorsPolicyProvider : ICorsPolicyProvider
    {
        public const string CorsDomainKey = "cors.domains";

        private readonly CorsPolicy _policy;
        public WebConfigCorsPolicyProvider()
        {
            // Create a CORS policy.
            _policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true
            };

            var origins = ConfigurationManager.AppSettings[CorsDomainKey];
            if (string.IsNullOrEmpty(origins)) throw new InvalidOperationException("No Cors configuration especified in the web.config");

            var corsDomain = origins.Split(',').ToList();
            corsDomain.ForEach(_policy.Origins.Add);
        }

        Task<CorsPolicy> ICorsPolicyProvider.GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }
}