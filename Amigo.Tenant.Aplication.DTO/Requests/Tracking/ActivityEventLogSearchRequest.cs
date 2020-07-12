using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class ActivityEventLogSearchRequest
    {
        //public int? ActivityTypeId
        //{
        //    get; set;
        //}
        public List<int?> ActivityTypeIds
        {
            get; set;
        }
        public string UserName
        {
            get; set;
        }
        public DateTimeOffset? ReportedActivityDateFrom
        {
            get; set;
        }
        public DateTimeOffset? ReportedActivityDateTo
        {
            get; set;
        }
        public string chargeNumber
        {
            get; set;
        }
        public string ResultCode
        {
            get; set;
        }
        public int Page
        {
            get; set;
        }
        public int PageSize
        {
            get; set;
        }

    }
}
