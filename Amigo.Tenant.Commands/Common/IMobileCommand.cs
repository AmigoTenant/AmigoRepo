﻿using System;

namespace Amigo.Tenant.Commands.Common
{
    public interface IMobileCommand
    {
        bool? IsAutoDateTime { get; set; }
        bool? IsSpoofingGPS { get; set; }
        bool? IsRootedJailbreaked { get; set; }
        string Platform { get; set; }
        string OSVersion { get; set; }
        string AppVersion { get; set; }
        decimal? Latitude { get; set; }
        decimal? Longitude { get; set; }
        int? Accuracy { get; set; }
        string LocationProvider { get; set; }
        DateTimeOffset? ReportedActivityDate { get; set; }
        string ReportedActivityTimeZone { get; set; }
    }
}