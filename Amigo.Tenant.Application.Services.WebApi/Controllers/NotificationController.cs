using Amigo.Tenant.Application.DTOs.Requests.Notification;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Notification;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    public class NotificationController : ApiController
    {

        [HttpPost]
        public async Task<ResponseDTO> SendNotification(SendNotificationRequest request)
        {
            WaMessageSender msgSender = new WaMessageSender();
            try
            {
                var response = msgSender.sendMessage(request.DestinationNumbers, request.TextMessage);
                return new ResponseDTO() { IsValid = true, Messages = new List<ApplicationMessage>() { new ApplicationMessage("1", response) } };
            }
            catch (System.Exception ex)
            {
                return (new ResponseDTO(false)).WithMessage(ex.Message);
            }
        }
    }
}
