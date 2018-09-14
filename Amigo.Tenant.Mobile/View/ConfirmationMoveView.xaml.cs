using System;
using Xamarin.Forms;

using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.View.Abstract;

namespace XPO.ShuttleTracking.Mobile.View
{
    public partial class ConfirmationMoveView : NavigatingPage
    {
        public ConfirmationMoveView()
        {
            InitializeComponent();
            LoadLabel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasBackButton(this, false);
        }
        private void LoadLabel()
        {
            Title = AppString.titleRegisterMove;
            lblHeaderTimeMsg.Text = AppString.lblHeaderTimeMsg;

            lblResumeMoveFromBlock.Text = AppString.lblResumeMoveFromBlock;
            lblResumeMoveToBlock.Text = AppString.lblResumeMoveToBlock;
            lblResumeMoveMoveType.Text = AppString.lblMoveType;
            lblResumeMoveEquipmentSize.Text = AppString.lblSize;
            lblResumeMoveEquipmentStatus.Text = AppString.lblStatus;
            lblChassisNumber.Text = AppString.lblChassis;
            lblResumeMoveProduct.Text = AppString.lblResumeMoveProduct;
            lblResumeMoveStartTime.Text = AppString.lblResumeMoveStartTime;
            lblResumeMoveFinishTime.Text = AppString.lblResumeMoveFinishTime;
            lblTimeElapsed.Text = AppString.lblElapsedTime;
            lblDriverComments.Text = AppString.lblDriverComments;
            lblBobtailAuth.Text = AppString.lblBobTail;

            lblCancelMove.Text = AppString.lblCancelMove;
            lblSuccessMove.Text = AppString.lblSuccessMove;

            BtnSetNewOne.Text = AppString.lblStartNewOne;
            btnMainMenu.Text = AppString.btnMainMenu;

        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
