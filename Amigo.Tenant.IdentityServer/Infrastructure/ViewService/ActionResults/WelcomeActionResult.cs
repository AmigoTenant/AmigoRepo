using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Amigo.Tenant.IdentityServer.Infrastructure.Extensions;

namespace Amigo.Tenant.IdentityServer.Infrastructure.ViewService.ActionResults
{
    internal class WelcomeActionResult : IHttpActionResult
    {
        readonly IOwinContext context;

        public WelcomeActionResult(IOwinContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            this.context = context;
        }

        public Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            var baseUrl = context.Environment.GetIdentityServerBaseUrl();
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            var html = AssetManager.LoadWelcomePage(baseUrl, version);
            var content = new StringContent(html, Encoding.UTF8, "text/html");

            var response = new HttpResponseMessage
            {
                Content = content
            };

            return Task.FromResult(response);
        }
    }
}