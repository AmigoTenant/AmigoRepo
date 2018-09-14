using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Entity;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.View.Abstract;
using XPO.ShuttleTracking.Mobile.ViewModel;

namespace XPO.ShuttleTracking.Mobile.View
{
    public partial class StartAdditionalServiceView : IPersistentView
    {
        public StartAdditionalServiceView()
        {
            InitializeComponent();

            _lstTools = new List<BEToolItems>();
            _lstTools.Add(new BEToolItems(BEMenuOption.HOME, AppString.btnToolbarHome));
            _lstTools.Add(new BEToolItems(BEMenuOption.SETTINGS, AppString.btnToolbarSettings));

            BuildToolbar();
            LoadLabel();
        }

        private void LoadLabel()
        {
            Title = AppString.titleAdditionalService;
            LblBlock.Text = AppString.lblBlock;
            LblH34.Text = AppString.lblH34;
            LblServiceType.Text = AppString.lblService;
            LblEquipmentSize.Text = AppString.lblSize;
            LblEquipmentStatus.Text = AppString.lblStatus;
            LblChassis.Text = AppString.lblChassis;
            LblProduct.Text = AppString.lblProduct;
            LblStartTime.Text = AppString.lblStartTime;
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
        public StartAdditionalServiceViewModel ViewModel => BindingContext as StartAdditionalServiceViewModel;

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
        private readonly IDictionary<string, Action<StartAdditionalServiceViewModel>> _actionMenu = new Dictionary<string, Action<StartAdditionalServiceViewModel>>()
        {
            //{BEMenuOption.HOME, vm=> vm.HomeCommand.Execute(null) },
            //{BEMenuOption.SETTINGS, vm=> vm.SettingsCommand.Execute(null) }
        };

        protected override bool OnBackButtonPressed()
        {
            return true;
            // return base.OnBackButtonPressed();
        }
    }
}
