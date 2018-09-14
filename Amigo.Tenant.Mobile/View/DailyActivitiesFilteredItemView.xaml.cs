using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.View.Abstract;
namespace XPO.ShuttleTracking.Mobile.View
{

    public partial class DailyActivitiesFilteredItemView : NavigatingPage
    {
        public DailyActivitiesFilteredItemView()
        {
            InitializeComponent();
            LoadLabel();
        }

        private void LoadLabel()
        {
            Title = AppString.titleRegisterMove;
            lblHeaderTimeMsg.Text = AppString.lblHeaderTimeMsg;
            lblResumeMoveFromBlock.Text = AppString.lblFromBlock;//****
            lblResumeMoveToBlock.Text = AppString.lblToBlock;//****
            lblResumeMoveMoveStatus.Text = AppString.lblStatus;//****
            lblH34.Text = AppString.lblH34;//****
            lblResumeMoveEquipmentSize.Text = AppString.lblSize;//****
            lblResumeMoveEquipmentStatus.Text = AppString.lblStatus;// ****
            lblChassisNumber.Text = AppString.lblChassis; // ****
            lblResumeMoveProduct.Text = AppString.lblProduct;//***
            lblResumeMoveStartTime.Text = AppString.lblStartTime;//****
            lblResumeMoveFinishTime.Text = AppString.lblFinishTime;//****
            lblTimeElapsed.Text = AppString.lblElapsedTime;//****
            lblDriverComments.Text = AppString.lblDriverComments;//****
        }
    }

}
