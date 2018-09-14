using System.Windows.Input;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Helpers;

namespace XPO.ShuttleTracking.Mobile.CustomControls
{
    public class ListView : Xamarin.Forms.ListView
    {
        public static BindableProperty ItemClickCommandProperty = BindableProperty.Create("ItemClickCommand", typeof(ICommand), typeof(ListView), null, BindingMode.OneWay, null, OnCommandSet);

        public ListView(ListViewCachingStrategy strategy) :base(strategy)
        {
            this.ItemTapped += this.OnItemTapped;
        }

        protected override void SetupContent(Cell pContent, int pIndex)
        {
            base.SetupContent(pContent, pIndex);

            var currentViewCell = pContent as ViewCell;

            if (currentViewCell != null)
            {
                currentViewCell.View.BackgroundColor = pIndex % 2 == 0 ? Color.FromHex("#FFFFFF") : Color.FromHex("#F6F6F6");
            }
        }

        private static void OnCommandSet(BindableObject bindable, object oldValue, object newValue)
        {
            var listView = bindable as ListView;
            if (listView != null) listView.ItemClickCommand = newValue as ICommand;
        }

        public ListView()
        {
            this.ItemTapped += this.OnItemTapped;
        }


        public ICommand ItemClickCommand
        {
            get { return (ICommand)this.GetValue(ItemClickCommandProperty); }
            set { this.SetValue(ItemClickCommandProperty, value); }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && this.ItemClickCommand != null && this.ItemClickCommand.CanExecute(e.Item))
            {
                this.ItemClickCommand.Execute(e.Item);
                this.SelectedItem = null;
            }
        }
    }
}
