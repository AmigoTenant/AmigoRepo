using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Entity.Acknowledge;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Navigation;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class AcknowledgeChargeNoViewModel : TodayViewModel
    {
        private readonly INavigator _navigator;

        public AcknowledgeChargeNoViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        public override void OnPushed()
        {
            base.OnPushed();
            Device.BeginInvokeOnMainThread(() =>
            {
                //Read the initial values sent to this screen
                _oldChargeNumber = new List<BESelectChargeNumber>();
                foreach (var x in LstChargeNumber)
                    _oldChargeNumber.Add(new BESelectChargeNumber() { Description = x.Description, IsSelected = x.IsSelected });

                //Set the values for the current list Binding
                NewChargeNumber = new List<BESelectChargeNumber>();
                foreach (var x in LstChargeNumber)
                    NewChargeNumber.Add(new BESelectChargeNumber() { Description = x.Description, IsSelected = x.IsSelected });
            });
        }

        public ICommand SelectChargeNumberCommand => CreateCommand<BESelectChargeNumber>((cost) =>
        {
            //Change its value
            cost.IsSelected = !cost.IsSelected;

            //If it is the "Select All" button, change all
            if (NewChargeNumber[0] == cost)
            {
                foreach (var x in NewChargeNumber)
                    x.IsSelected = cost.IsSelected;
            }
            else
            {
                //If its not, then check if it should update its value
                var allSelected = true;
                for (var i = 1; i < NewChargeNumber.Count; i++)
                    if (!NewChargeNumber[i].IsSelected)
                    {
                        allSelected = false;
                        break;
                    }
                NewChargeNumber[0].IsSelected = allSelected;
            }
        });

        public ICommand CancelCommand => CreateCommand(() =>
        {
            //Restore the old values
            for (var i = 0; i < _oldChargeNumber.Count; i++)
            {
                LstChargeNumber[i].IsSelected = _oldChargeNumber[i].IsSelected;
                LstChargeNumber[i].Description = _oldChargeNumber[i].Description;
            }
            _navigator.RemoveLastPageFromStack();
        });
        public ICommand AcceptCommand => CreateCommand(() =>
        {
            //Set the new values
            for (var i = 0; i < NewChargeNumber.Count; i++)
            {
                LstChargeNumber[i].IsSelected = NewChargeNumber[i].IsSelected;
                LstChargeNumber[i].Description = NewChargeNumber[i].Description;
            }
            _navigator.RemoveLastPageFromStack();
        });

        private IList<BESelectChargeNumber> _oldChargeNumber;
        private IList<BESelectChargeNumber> _newChargeNumber;
        public IList<BESelectChargeNumber> NewChargeNumber
        {
            get { return _newChargeNumber; }
            set { SetProperty(ref _newChargeNumber, value); }
        }
        private IList<BESelectChargeNumber> _lstChargeNumber;
        public IList<BESelectChargeNumber> LstChargeNumber
        {
            get { return _lstChargeNumber;}
            set { SetProperty(ref _lstChargeNumber, value); }
        }
    }
}
