using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Data.Spatial;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.Tracking.Location
{
    public class RegisterLocationCoordinatesCommand : IAsyncRequest<CommandResult>
    {
        public List<RegisterLocationCoordinateItem> RegisterLocationCoordinatesList { get; set; }
    }

    public class RegisterLocationCoordinateItem
    {
        public string LocationCode { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
    }
}
