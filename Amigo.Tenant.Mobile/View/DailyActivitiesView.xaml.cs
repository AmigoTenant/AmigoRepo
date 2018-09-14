using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.View.Abstract;

namespace XPO.ShuttleTracking.Mobile.View
{
    public partial class DailyActivitiesView : NavigatingPage
    {
        public DailyActivitiesView()
        {
            InitializeComponent();
            //ReferencesNew.ItemSelected += OnSelection;
        }

      private void LVDailyActivities_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row
            if (e.Item == null) return;
            // do something with e.SelectedItem
            ((ListView)sender).SelectedItem = null; // de-select the row
        }
    }
}
