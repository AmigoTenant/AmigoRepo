using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Mindscape.Raygun4Net;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Configuration;

namespace Amigo.Tenant.Application.Services.WebApi.ExceptionHandling
{
    public class RayGunExceptionLogger : IExceptionLogger
    {
        private static readonly Lazy<RaygunClient> RaygunClient = new Lazy<RaygunClient>(() => new RaygunClient());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            try
            {
                var exception = context.Exception;
                //RaygunClient.Value.SendInBackground(exception,new [] { Settings.RaygunTag });
                await Task.Run (() => System.Diagnostics.Trace.TraceError($"DateTime: {DateTime.Now} Error: {exception.ToString()}"));
            }
            catch (Exception ex)
            {
                await Task.Run(() => System.Diagnostics.Trace.TraceError($"DateTime: {DateTime.Now} Error: {ex.ToString()}"));
            }
        }
    }
}