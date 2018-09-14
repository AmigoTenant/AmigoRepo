using System;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.CustomControls
{
    class CustomSwitcher : Grid
    {
        private enum SelectionStatus
        {
            Left,
            Right,
            Unselected
        }

        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;
        private readonly Label _labelLeft, _labelRight;

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create<CustomTab, Object>(t => t.SelectedItem, null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

        public CustomSwitcher()
        {
            try
            {
                //Config gestures for labels
                var tapGestureNegative = new TapGestureRecognizer();
                tapGestureNegative.Tapped +=
                    (s, e) => OnSelectedItemChanged(this, ItemSelected, (int)SelectionStatus.Left);
                var tapGesturePositive = new TapGestureRecognizer();
                tapGesturePositive.Tapped +=
                    (s, e) => OnSelectedItemChanged(this, ItemSelected, (int)SelectionStatus.Right);

                //Config Home Label
                _labelLeft = new Label();
                _labelLeft.GestureRecognizers.Add(tapGestureNegative);
                _labelLeft.Style = (Style)Xamarin.Forms.Application.Current.Resources["SwitchLabelSelected"];

                //Config Summary Label
                _labelRight = new Label();
                _labelRight.GestureRecognizers.Add(tapGesturePositive);
                _labelRight.Style = (Style)Xamarin.Forms.Application.Current.Resources["SwitchLabelUnSelected"];

                //Config container grid
                this.RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                };

                this.Style = (Style)Xamarin.Forms.Application.Current.Resources["SwitchBackground"];
                this.Children.Add(_labelLeft, 0, 0);
                this.Children.Add(_labelRight, 1, 0);
            }
            catch (System.Exception ex)
            {
                //Error
            }

        }

        public string TextTabRight
        {
            get { return _labelRight.Text; }
            set
            {
                if (TextTabRight != value)
                    _labelRight.Text = value;
            }
        }
        public string TextTabLeft
        {
            get { return _labelLeft.Text; }
            set
            {
                if (TextTabLeft != value)
                    _labelLeft.Text = value;
            }
        }
        public Object SelectedItem
        {
            get { return base.GetValue(SelectedItemProperty); }
            set
            {
                if (SelectedItem != value)
                {
                    base.SetValue(SelectedItemProperty, value);
                    InternalUpdateSelected();
                }
            }
        }

        private void InternalUpdateSelected()
        {
            switch ((int)SelectedItem)
            {
                case (int)SelectionStatus.Left:
                    SelectLeft();
                    break;
                case (int)SelectionStatus.Right:
                    SelectRight();
                    break;
                default:
                    SelectLeft();
                    break;
            }
        }

        private void SelectLeft()
        {
            _labelLeft.Style = (Style)Xamarin.Forms.Application.Current.Resources["SwitchLabelSelected"];
            _labelRight.Style = (Style)Xamarin.Forms.Application.Current.Resources["SwitchLabelUnSelected"];
        }

        private void SelectRight()
        {
            _labelLeft.Style = (Style)Xamarin.Forms.Application.Current.Resources["SwitchLabelUnSelected"];
            _labelRight.Style = (Style)Xamarin.Forms.Application.Current.Resources["SwitchLabelSelected"];
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var boundSwitch = (CustomSwitcher)bindable;

            if ((int)newValue != (int)SelectionStatus.Unselected)
                boundSwitch.SelectedItem = (int)newValue == (int)SelectionStatus.Left ? (int)SelectionStatus.Left : (int)SelectionStatus.Right;

            boundSwitch.ItemSelected?.Invoke(boundSwitch, new SelectedItemChangedEventArgs(newValue));
            boundSwitch.InternalUpdateSelected();
        }
    }
}
