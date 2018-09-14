using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Resources;

namespace Amigo.Tenant.Application.Services.Common.Extensions
{
    public static class CommandResultExtensions
    {
        public static ResponseDTO ToResponse(this CommandResult result)
        {
            if (result == null) return ResponseBuilder.InCorrect().WithMessage(TextResources.GenericError);

            return new ResponseDTO(result.IsCorrect)
                .WithMessages(result.Errors.Select(x=> new ApplicationMessage(null,x)).ToArray());
        }

        public static ResponseDTO<int> ToResponse(this RegisteredCommandResult result)
        {
            if (result == null) return ResponseBuilder.InCorrect<int>()
                    .WithMessage(TextResources.GenericError);

            return new ResponseDTO<int>(result.IsCorrect)
            {
                Data = result.Id
            }.WithMessages(result.Errors.Select(x => new ApplicationMessage(null, x)).ToArray());
        }
    }
}
