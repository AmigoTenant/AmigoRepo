using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XPO.ShuttleTracking.Mobile.View.Abstract;

namespace XPO.ShuttleTracking.Mobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DailyActivitiesFilteredView : NavigatingPage
    {
        public DailyActivitiesFilteredView()
        {
            InitializeComponent();
        }
    }
}
