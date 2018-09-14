using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.View.Abstract;

namespace XPO.ShuttleTracking.Mobile.View
{
    public partial class CostCenterSearchView : NavigatingPage, IPersistentView
    {
        public CostCenterSearchView()
        {
            InitializeComponent();
            Title = AppString.titleCostCenter; 
        }
    }
}
