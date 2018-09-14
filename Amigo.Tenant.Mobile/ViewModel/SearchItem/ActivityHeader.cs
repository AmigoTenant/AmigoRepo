using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Entity.Infrastructure;

namespace XPO.ShuttleTracking.Mobile.ViewModel.SearchItem
{
    public class ActivityHeader : BaseEntity
    {
        public int ShuttleTServiceId { get; set; }
        public string ChargeType { get; set; }
        public string ActivityType { get; set; }
        public string StartDate { get; set; }
        public string ChargeNo { get; set; }
        public string EquipmentType { get; set; }
        public string Chassis { get; set; }
        public string ActionType { get; set; }
        public string FromBlock { get; set; }
        public string ToBlock { get; set; }
        public BEServiceBase Activity { get; set; }
    }
}
