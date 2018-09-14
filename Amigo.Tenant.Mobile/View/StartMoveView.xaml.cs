using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Entity;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.View.Abstract;
using XPO.ShuttleTracking.Mobile.ViewModel;

namespace XPO.ShuttleTracking.Mobile.View
{    
    public partial class StartMoveView : IPersistentView
    {
        public StartMoveView()
        {
            InitializeComponent();

            _lstTools = new List<BEToolItems>();
            _lstTools.Add(new BEToolItems(BEMenuOption.HOME, AppString.btnToolbarHome));
            _lstTools.Add(new BEToolItems(BEMenuOption.SETTINGS, AppString.btnToolbarSettings));

            BuildToolbar();
            LoadLabel();
        }

        private void BuildToolbar()
        {
            switch (Device.OS)
            {
                case TargetPlatform.iOS:
                    BuildIosToolbar();
                    break;
                case TargetPlatform.Android:
                    BuildAndToolbar();
                    break;
            }
            NavigationPage.SetHasBackButton(this, true);
        }

        private void BuildIosToolbar()
        {
            ToolbarItems.Clear();
            var toolbarItem = new ToolbarItem
            {
                Text = AppString.btnToolbarName,
                Command = new Command(ShowOptionsMenu),
                Order = ToolbarItemOrder.Primary,
                Icon = "menu.png"
            };
            ToolbarItems.Add(toolbarItem);
        }

        private void BuildAndToolbar()
        {
            ToolbarItems.Clear();
            foreach (var tool in _lstTools)
            {
                var toolbarItem = new ToolbarItem
                {
                    Text = tool.Name,
                    Order = ToolbarItemOrder.Secondary
                };
                var commandBinding = new Binding(tool.Id);
                toolbarItem.SetBinding(MenuItem.CommandProperty, commandBinding);
                ToolbarItems.Add(toolbarItem);
            }
        }
        public StartMoveViewModel ViewModel => BindingContext as StartMoveViewModel;

        private IList<BEToolItems> _lstTools;
        private async void ShowOptionsMenu()
        {
            var menu = new List<string>();
            foreach (var tool in _lstTools)
                menu.Add(tool.Name);

            var menuClicked = await DisplayActionSheet(null, AppString.btnToolbarCancel, null, menu.ToArray());

            foreach (var tool in _lstTools)
            {
                if (menuClicked.Equals(tool.Name))
                    _actionMenu[tool.Id](ViewModel);
            }
        }
        private readonly IDictionary<string, Action<StartMoveViewModel>> _actionMenu = new Dictionary<string, Action<StartMoveViewModel>>()
        {
            {BEMenuOption.HOME, vm=> vm.HomeCommand.Execute(null) },
            {BEMenuOption.SETTINGS, vm=> vm.SettingsCommand.Execute(null) }
        };

        private void LoadLabel()
        {
            Title = AppString.titleRegisterMove;
            lblHeaderTimeMsg.Text = AppString.lblHeaderTimeMsg;
            lblResumeMoveFromBlock.Text = AppString.lblFromBlock;
            lblResumeMoveToBlock.Text = AppString.lblToBlock;
            lblMoveType.Text = AppString.lblMoveType;
            lblSize.Text = AppString.lblSize;
            lblStatus.Text = AppString.lblStatus;
            lblChasisNo.Text = AppString.lblChassis;
            lblProduct.Text = AppString.lblProduct;
            lblStartTime.Text = AppString.lblStartTime;
            lblBobtailAuth.Text = AppString.lblBobTail;
            lblInstructionStartMove.Text = AppString.lblInstructionStartMove;
            btnStartMove.Text = AppString.btnStartMove;
        }
    }
}
