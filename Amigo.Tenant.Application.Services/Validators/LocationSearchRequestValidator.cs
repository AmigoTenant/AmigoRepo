using FluentValidation;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.Services.Validators.Common;
using System.Data.SqlTypes;
using System;

namespace Amigo.Tenant.Application.Services.Validators
{

    public class LocationSearchRequestValidator : PagedRequestValidator<LocationSearchRequest>
    {
        public LocationSearchRequestValidator()
        {

        }
    }
}
