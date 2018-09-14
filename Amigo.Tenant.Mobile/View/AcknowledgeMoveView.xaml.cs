using System;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.View.Abstract;

namespace XPO.ShuttleTracking.Mobile.View
{
    public partial class AcknowledgeMoveView : NavigatingPage
    {
        public AcknowledgeMoveView()
        {
            InitializeComponent();
            TxtAuthorizedBy.TextChanged += TxtAuthorizedByOnTextChanged;
            btnNext.Clicked += BtnNextOnClicked;
        }

        private void BtnNextOnClicked(object sender, EventArgs e)
        {
            Validation();
        }

        private void TxtAuthorizedByOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (string.IsNullOrEmpty(TxtAuthorizedBy.Text)) return;

            TxtAuthorizedBy.Text = textChangedEventArgs.NewTextValue.ToUpper();
            var text = TxtAuthorizedBy.Text;      //Get Current Text
            TxtAuthorizedBy.Text = text.ToUpper();
            CleanLabel(LblAuthorizedByError);
        }

        private void Validation()
        {
            if(string.IsNullOrEmpty(TxtAuthorizedBy.Text))
                ShowLabel(LblAuthorizedByError, string.Format(Resource.AppString.lblMandatoryField, "Authorized By"));
        }
        protected override bool OnBackButtonPressed()
        {
            // btnBack.Command.Execute(null);
            return true;
        }
        private void CleanLabel(Label toModify)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                toModify.Text = string.Empty;
                toModify.IsVisible = false;
            });
        }
        private void ShowLabel(Label toModify, string toShow)
        {
            Device.BeginInvokeOnMainThread(() => {
                toModify.Text = toShow;
                toModify.IsVisible = true;
                toModify.Focus();

            });
        }

    }
}
