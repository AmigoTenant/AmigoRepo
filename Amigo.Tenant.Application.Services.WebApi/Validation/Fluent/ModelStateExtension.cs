using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using Amigo.Tenant.Application.DTOs.Responses.Common;

namespace Amigo.Tenant.Application.Services.WebApi.Validation.Fluent
{
    public static class ModelStateExtension
    {
        public static ResponseDTO ToResponse(this ModelStateDictionary modelState)
        {
            var resp = new ResponseDTO();
            GetApplicationMessages(modelState, resp);
            return resp;
        }

        public static ResponseDTO<T> ToResponse<T>(this ModelStateDictionary modelState)
        {
            var resp = new ResponseDTO<T>();
            GetApplicationMessages(modelState, resp);
            return resp;
        }

        private static void GetApplicationMessages(ModelStateDictionary modelState, ResponseDTO resp)
        {
            resp.IsValid = true;

            foreach (var prop in modelState)
            foreach (var error in prop.Value.Errors)
            {
               resp.Messages.Add(new ApplicationMessage(prop.Key, error.ErrorMessage));
            }

            if (resp.Messages.Count > 0)
                resp.IsValid = false;
        }
    }
}