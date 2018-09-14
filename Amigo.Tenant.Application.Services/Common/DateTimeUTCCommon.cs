using System;


namespace Amigo.Tenant.Application.Services.Common
{
  public static  class DateTimeUTCCommon
    {

        public  static DateTime DatetimeToDateUTC(DateTimeOffset? date)
        {
            string datetime = date.ToString();
            var dtOffset = DateTimeOffset.Parse(datetime);

            return dtOffset.UtcDateTime;
        }

    }
}
