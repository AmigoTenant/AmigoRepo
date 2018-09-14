using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Amigo.Tenant.Common;

namespace Amigo.Tenant.Web.Logging
{
    public class LogRequestActionFilter: ActionFilterAttribute
    {
        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (!bool.Parse(ConfigurationManager.AppSettings["requestLog.enabled"]))
            {
                return;
            }

            var controller = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var action = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            var serviceName = $"{controller}/{action}";
            var xmlServiceName = $"{controller}-{action}";
            var url = actionExecutedContext.Request.RequestUri;
            //string requestJson;
            //using (var stream = actionExecutedContext.Request.Content.ReadAsStreamAsync().Result)
            //{
            //    if (stream.CanSeek)
            //    {
            //        stream.Position = 0;
            //    }
            //    requestJson = actionExecutedContext.Request.Content.ReadAsStringAsync().Result;

            //}

            var requestString = "";

            if (actionExecutedContext.ActionContext.ActionArguments.Count > 0)
            {
                var requestObject = actionExecutedContext.ActionContext.ActionArguments.First();
                var requestXml = XmlHelper.GenerateXmlServiceRequest(requestObject.Value, xmlServiceName);
                requestString = requestXml.InnerXml;

            }
            //var requestJson = await actionExecutedContext.ActionContext.Request.Content.ReadAsStringAsync();
            //var request = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(requestJson), new XmlDictionaryReaderQuotas()));
            
            var exception = actionExecutedContext.Exception;
            var responseString = "";
            if (exception == null)
            {
                var responseJson = await actionExecutedContext.ActionContext.Response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseJson))
                {
                    var response = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(responseJson), new XmlDictionaryReaderQuotas()));
                    responseString = response.ToString();
                }
            }
            else
            {
                var errorMessage = exception.InnerException != null
                    ? exception.InnerException.InnerException?.Message ?? exception.InnerException.Message
                    : exception.Message;

                var xmlRespone = XmlHelper.GenerateXmlServiceLineResponse(errorMessage, xmlServiceName);
                responseString = xmlRespone.InnerXml;
            }
            
            var identity = (actionExecutedContext.ActionContext.RequestContext.Principal as ClaimsPrincipal);
            var iduser = 0;
            if (identity !=null && identity.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                iduser = int.Parse(identity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            }
            var requestinfo = new RequestInfo(url.AbsoluteUri, serviceName, requestString, responseString, DateTime.UtcNow, iduser);

            WorkItem.QueuePageView(requestinfo,1);
        }

    }
}
