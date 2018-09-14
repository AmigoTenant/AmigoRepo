using System;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.CustomControls
{
    public class CustomTab : Grid
    {
        private enum SelectionStatus
        {
            Left,
            Right,
            Unselected
        }

        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;
        private readonly BoxView _boxLeft, _boxRight;
        private readonly Label _labelLeft, _labelRight;
        
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create<CustomTab, Object>(t => t.SelectedItem, null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

        public CustomTab()
        {
            try
            {
                //Config selected mark
                _boxLeft = new BoxView();
                _boxRight = new BoxView();

                //Config gestures for labels
                var tapGestureNegative = new TapGestureRecognizer();
                tapGestureNegative.Tapped +=
                    (s, e) => OnSelectedItemChanged(this, ItemSelected, (int) SelectionStatus.Left);
                var tapGesturePositive = new TapGestureRecognizer();
                tapGesturePositive.Tapped +=
                    (s, e) => OnSelectedItemChanged(this, ItemSelected, (int) SelectionStatus.Right);
                
                //Config Home Label
                _labelLeft = new Label();
                _labelLeft.GestureRecognizers.Add(tapGestureNegative);
                _labelLeft.Style = (Style)Xamarin.Forms.Application.Current.Resources["TabLabelSelected"];

                //Config Summary Label
                _labelRight = new Label();
                _labelRight.GestureRecognizers.Add(tapGesturePositive);
                _labelRight.Style = (Style)Xamarin.Forms.Application.Current.Resources["TabLabelUnSelected"];
                
                //Config container grid
                this.RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition {Height = new GridLength((double)Xamarin.Forms.Application.Current.Resources["EntrySizeS"], GridUnitType.Absolute)},
                    new RowDefinition {Height = new GridLength(5, GridUnitType.Absolute)}
                };

                this.Style = (Style)Xamarin.Forms.Application.Current.Resources["TabBackground"];
                this.Children.Add(_labelLeft, 0, 0);
                this.Children.Add(_labelRight, 1, 0);
                this.Children.Add(_boxLeft, 0, 1);
                this.Children.Add(_boxRight, 1, 1);
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
            _labelLeft.Style = (Style)Xamarin.Forms.Application.Current.Resources["TabLabelSelected"];
            _boxLeft.Style = (Style)Xamarin.Forms.Application.Current.Resources["TabBoxSelected"];

            _labelRight.Style = (Style)Xamarin.Forms.Application.Current.Resources["TabLabelUnSelected"];
            _boxRight.Style = (Style)Xamarin.Forms.Application.Current.Resources["TabBoxUnSelected"];
        }

        private void SelectRight()
        {
            _labelLeft.Style = (Style)Xamarin.Forms.Application.Current.Resources["TabLabelUnSelected"];
            _boxLeft.Style = (Style)Xamarin.Forms.Application.Current.Resources["TabBoxUnSelected"];

            _labelRight.Style = (Style)Xamarin.Forms.Application.Current.Resources["TabLabelSelected"];
            _boxRight.Style = (Style)Xamarin.Forms.Application.Current.Resources["TabBoxSelected"];
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                if (newValue == null) return;
                var boundSwitch = (CustomTab)bindable;

                if ((int)newValue != (int)SelectionStatus.Unselected)
                    boundSwitch.SelectedItem = (int)newValue == (int)SelectionStatus.Left ? (int)SelectionStatus.Left : (int)SelectionStatus.Right;

                boundSwitch.ItemSelected?.Invoke(boundSwitch, new SelectedItemChangedEventArgs(newValue));
                boundSwitch.InternalUpdateSelected();
            }catch (Exception e) { }
            }

    }
}
