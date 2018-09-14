using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Newtonsoft.Json;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Resources;

namespace Amigo.Tenant.Application.Services.WebApi.ExceptionHandling
{
    public class AmigoTenantExceptionHandler : IExceptionHandler
    {        
        public async Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            context.Result = new JsonResult<ResponseDTO>(new ResponseDTO()
            {
                IsValid = false,
                Messages = new List<ApplicationMessage>()
                {
                    new ApplicationMessage() { Message = TextResources.GenericError}
                }
            },new JsonSerializerSettings(), Encoding.UTF8,context.Request);
        }
    }
}