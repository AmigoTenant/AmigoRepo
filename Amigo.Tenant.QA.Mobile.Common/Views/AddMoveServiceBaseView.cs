using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public abstract class AddMoveServiceBaseView : BaseView
    {
        public AddMoveServiceBaseView(IApp app) : base(app)
        {
        }

        public void EnterActivityType(string moveTypeDescription)
        {
            Func<AppQuery, AppQuery> ActivityTypeButton = c => c.Marked("ActivityType");
            Func<AppQuery, AppQuery> ActivityType = c => c.Marked("text1").Class("AppCompatTextView").Text(moveTypeDescription);

            TapView(ActivityTypeButton);
            TapView(ActivityType);
        }

        public void EnterMoveType(string moveTypeDescription)
        {
            Func<AppQuery, AppQuery> ActivityTypeButton = c => c.Marked("PkrMoveType");
            Func<AppQuery, AppQuery> ActivityType = c => c.Marked("text1").Class("AppCompatTextView").Text(moveTypeDescription);

            TapView(ActivityTypeButton);
            TapView(ActivityType);
        }

        public void EnterFromBlock(string fromBlock)
        {
            Func<AppQuery, AppQuery> FromBlockTxt = c => c.Marked("TxtFromBlock");
            Func<AppQuery, AppQuery> FromBlock = c => c.Class("FormsTextView").Text(fromBlock);

            TapView(FromBlockTxt);
            TapView(FromBlock);
        }

        public void EnterToBlock(string toBlock)
        {
            Func<AppQuery, AppQuery> ToBlockTxt = c => c.Marked("TxtToBlock");
            Func<AppQuery, AppQuery> ToBlock = c => c.Class("FormsTextView").Text(toBlock);

            TapView(ToBlockTxt);
            TapView(ToBlock);
        }

        public void EnterDispatcher(string dispatcher)
        {
            Func<AppQuery, AppQuery> DispatchingButton = c => c.Marked("PkrToDispatching");
            Func<AppQuery, AppQuery> Dispatching = c => c.Marked("text1").Class("AppCompatTextView").Text(dispatcher);

            ScrollDownTo(DispatchingButton);
            TapView(DispatchingButton);
            TapView(Dispatching);
        }

        public void TapNextButton()
        {
            Func<AppQuery, AppQuery> RegisterButton = c => c.Marked("GoStartButton");
            ScrollDownTo(RegisterButton);
            TapView(RegisterButton);
        }
        
        public void EnterProduct(string productDescription)
        {
            Func<AppQuery, AppQuery> SearchProductButton = c => c.Marked("TxtProduct");
            Func<AppQuery, AppQuery> SelectedProduct = c => c.Class("FormsTextView").Text(productDescription);

            TapView(SearchProductButton);
            TapView(SelectedProduct);
        }

        public void EnterEquipmentType(string equipmentTypeDescription)
        {
            Func<AppQuery, AppQuery> EquipTypeButton = c => c.Marked("PkrEquipmentType");
            Func<AppQuery, AppQuery> SelectedEquipType = c => c.Marked("text1").Class("AppCompatTextView").Text(equipmentTypeDescription);

            ScrollDownTo(EquipTypeButton);
            TapView(EquipTypeButton);
            TapView(SelectedEquipType);
        }

        public void EnterEquipmentSize(string equipmentSizeDescription)
        {
            Func<AppQuery, AppQuery> EquipSizeButton = c => c.Marked("PkrEquipmentSize");
            Func<AppQuery, AppQuery> SelectedEquipSize = c => c.Marked("text1").Class("AppCompatTextView").Text(equipmentSizeDescription);

            ScrollDownTo(EquipSizeButton);
            TapView(EquipSizeButton);
            TapView(SelectedEquipSize);
        }

        public void EnterEquipmentNumber(string equipmentNumber)
        {
            Func<AppQuery, AppQuery> EquipNumber = c => c.Marked("PkrEquipmentNo");
            ScrollDownTo(EquipNumber);
            EnterTextView(EquipNumber, equipmentNumber);
        }

        public void EnterEquipmentStatus(string equipmentStatusDescription)
        {
            Func<AppQuery, AppQuery> EquipStatus = c => c.Marked("PkrEquipmentStatus");
            Func<AppQuery, AppQuery> SelectedEquipStatus = c => c.Marked("text1").Class("AppCompatTextView").Text(equipmentStatusDescription);

            TapView(EquipStatus);
            TapView(SelectedEquipStatus);
        }

        public void Enter2dot5YearTestDate(string date)
        {
            Func<AppQuery, AppQuery> Dp25YearDate = c => c.Marked("txtDp25yrTestDate");
            ScrollDownTo(Dp25YearDate);
            EnterTextView(Dp25YearDate, date);
        }

        public void Enter5YearTestDate(string date)
        {
            Func<AppQuery, AppQuery> Dp5YearDate = c => c.Marked("TxtDp5yrTestDate");
            ScrollDownTo(Dp5YearDate);
            EnterTextView(Dp5YearDate, date);
        }

        public void EnterChassisNumber(string chassisNumber)
        {
            Func<AppQuery, AppQuery> ChassisNumber = c => c.Marked("PkrChassisNo");
            ScrollDownTo(ChassisNumber);
            EnterTextView(ChassisNumber, chassisNumber);
        }

        public void SelectCostCenterTab()
        {
            Func<AppQuery, AppQuery> CostCenterTab = c => c.Marked("By Cost Center");
            TapView(CostCenterTab);
        }

        public void SelectShipmentIdTab()
        {
            Func<AppQuery, AppQuery> CostCenterTab = c => c.Marked("By Shipment Id");
            TapView(CostCenterTab);
        }
        
    }
}
