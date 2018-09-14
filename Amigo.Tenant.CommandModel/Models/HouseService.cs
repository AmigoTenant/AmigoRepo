using Amigo.Tenant.CommandModel.Abstract;
using Amigo.Tenant.CommandModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandModel.Models
{
    public partial class HouseService : EntityBase
    {
        public HouseService()
        {
            HouseServicePeriods = new HashSet<HouseServicePeriod>();
        }

        public int HouseServiceId { get; set; }

        public int HouseId { get; set; }

        public int ServiceId { get; set; }

        public bool RowStatus { get; set; }

        public virtual ICollection<HouseServicePeriod> HouseServicePeriods { get; set; }

        public ServiceHouse ServiceHouse { get; set; }

        //public House House { get; set; }


        private static Month[] months = { new Month() { Number = "01", Name = "January" }, new Month() { Number = "02", Name = "February" }, new Month() { Number = "03", Name = "March" }, new Month() { Number = "04", Name = "April" }, new Month() { Number = "05", Name = "May" }, new Month() { Number = "06", Name = "June" }, new Month() { Number = "07", Name = "July" }, new Month() { Number = "08", Name = "August" }, new Month() { Number = "09", Name = "September" }, new Month() { Number = "10", Name = "October" }, new Month() { Number = "11", Name = "November" }, new Month() { Number = "12", Name = "December" } };
        private static string[] days = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };

        //public static List<HouseService> CreatePeriods(int serviceId)
        //{
        //    var list = new List<HouseService>();
        //    for (int i = months.GetLowerBound(0); i <= months.GetUpperBound(0); i++)
        //    {
        //        list.Add(new HouseService()
        //        {
        //            DueDateMonth = MonthDay.Create(months[i].LastDay, months[i].Number).ToString(),
        //            DueDateDay = MonthDay.Create(months[i].FirstDay, months[i].Number).ToString(),
        //            CutOffMonth = MonthDay.Create(months[i].LastDay, months[i].Number).ToString(),
        //            HouseId = 0,
        //            HouseServiceId = 0,
        //            RowStatus = true,
        //            ServiceId = serviceId
        //        });
        //    }

        //    return list;
        //}

        protected partial struct Month
        {
            public string Number { get; set; }
            public string Name { get; set; }

            public string FirstDay => days[days.GetLowerBound(0)];

            public string LastDay => DateTime.DaysInMonth(DateTime.Now.Year, Convert.ToInt32(Number)).ToString().PadLeft(2, '0');

        }

        //protected partial struct MonthDay
        //{
        //    public string Day { get; set; }
        //    public Month Month { get; set; }

        //    public static MonthDay Create(string day, string month)
        //    {
        //        return new MonthDay()
        //        {
        //            Day = day,
        //            Month = new Month() { Number = months[Convert.ToInt32(month) - 1].Number, Name = months[Convert.ToInt32(month) - 1].Name }
        //        };
        //    }

        //    public override string ToString()
        //    {
        //        return string.Format("{0}{1}", Day, Month.Number);
        //    }
        //}
    }
}
