using System;

using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Resource;

namespace XPO.ShuttleTracking.Mobile.View
{
    public partial class ConfirmationOperateTaylorLiftView : ContentPage
    {
        public ConfirmationOperateTaylorLiftView()
        {
            InitializeComponent();
            LoadLabel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasBackButton(this, false);
        }
        public void LoadLabel()
        {
            Title = AppString.titleOperateTaylorLift;
            LblBlock.Text = AppString.lblBlock;
            LblService.Text = AppString.lblService;
            LblBlock.Text = AppString.lblBlock;
            LblH34.Text = AppString.lblH34;
            LblStartTime.Text = AppString.lblStartTime;
            LblStopTime.Text = AppString.lblStopTime;
            LblTimeElapsed.Text = AppString.lblElapsedTime;
            LblEquipmentSize.Text = AppString.lblSize;
            LblEquipmentStatus.Text = AppString.lblStatus;
            LblChassis.Text = AppString.lblChassis;
            LblProduct.Text = AppString.lblProduct;
            lblDriverComments.Text = AppString.lblDriverComments;

            BtnSetNewOne.Text = AppString.lblStartNewOne;
            btnMainMenu.Text = AppString.btnMainMenu;
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
