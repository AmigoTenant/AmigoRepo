using System;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.View.Abstract;
using XPO.ShuttleTracking.Mobile.ViewModel;

namespace XPO.ShuttleTracking.Mobile.View
{
    public partial class SummaryView : NavigatingPage
    {
        public SummaryView()
        {
            InitializeComponent();
        }
        private SummaryViewModel ViewModel => BindingContext as SummaryViewModel;
        private static readonly string[] MenuOptions = new[]
        {
            AppString.btnSummaryLegend
        };
        private async void ShowActionSheet(object sender, EventArgs e)
        {
            var menuClicked = await DisplayActionSheet(null, AppString.btnToolbarCancel, null, MenuOptions);

            if (menuClicked == AppString.btnSummaryLegend)
            {
                ViewModel.ShowLegendCommand.Execute(null);
            }
        }
    }
}
