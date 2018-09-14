using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Data.Spatial;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.Tracking.Location
{
 
    public class UpdateLocationCommand : IAsyncRequest<CommandResult>
    {

        public string Code { get; set; }
        public string Name { get; set; }
        public string LocationTypeCode { get; set; }
        public string ParentLocationCode { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCode { get; set; }
        public string CityCode { get; set; }
        public bool RowStatus { get; set; }

        public List<RegisterLocationCoordinateItem> Coordinates { get; set; }
    }


}
