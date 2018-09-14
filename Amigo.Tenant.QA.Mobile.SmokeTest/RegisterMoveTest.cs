using NUnit.Framework;
using System;
using System.CodeDom;
using System.Threading;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using XPO.ShuttleTracking.QA.Mobile.Common.Views;
using static XPO.ShuttleTracking.QA.Mobile.Common.Constants;

namespace XPO.ShuttleTracking.QA.Mobile.SmokeTest
{
    [TestFixture(Platform.Android)]
    public class RegisterMoveTest : BaseSmokeTest
    {
        public RegisterMoveTest(Platform currentPlatform) : base(currentPlatform)
        {
            
        }

        [Test]
        public void TC2_AcknowledgeMove_WithBobTailMove()
        {
            const string shipmentId = "123456";
            const string moveType = "Bobtail";
            const string fromBlock = "A1300";
            const string toBlock = "A1324";
            const string dispatchingParty = "XPO";

            LoginView loginView = new LoginView(CurrentApp);
            LicenseView licenseView = new LicenseView(CurrentApp);
            MainMenuView mainMenuView = new MainMenuView(CurrentApp);

            licenseView.AcceptTermOfServices(CurrentApp);
            loginView.LoginIntoApplication(Username, Password);

            mainMenuView.StartWorkday(YesNoAction.PressYes);
            mainMenuView.TapAddMoveOption();

            AddMoveByShipmentView addMoveByShipmentView = new AddMoveByShipmentView(CurrentApp);

            addMoveByShipmentView.EnterShipmentId(shipmentId);
            addMoveByShipmentView.EnterActivityType(moveType);
            addMoveByShipmentView.EnterFromBlock(fromBlock);
            addMoveByShipmentView.EnterToBlock(toBlock);
            addMoveByShipmentView.EnterDispatcher(dispatchingParty);
            addMoveByShipmentView.Screenshot("Step 1");
            addMoveByShipmentView.TapNextButton();

            MoveServiceInitialSummaryView addMoveInitialSummaryView = new MoveServiceInitialSummaryView(CurrentApp);
            addMoveInitialSummaryView.ConfirmStartAction();
            addMoveByShipmentView.Screenshot("Step 2");
            MoveServiceExecutingView addMoveFinishSummaryView = new MoveServiceExecutingView(CurrentApp);
            addMoveFinishSummaryView.ConfirmFinishAction();
            addMoveByShipmentView.Screenshot("Step 3");
            MoveServiceFinishSummaryView moveServiceFinishSummaryView = new MoveServiceFinishSummaryView(CurrentApp);
            moveServiceFinishSummaryView.GoToMainMenu();
            addMoveByShipmentView.Screenshot("Step 4");
            mainMenuView.TapAcknowledge();
            AcknowledgeView acknowledgeView = new AcknowledgeView(CurrentApp);
            acknowledgeView.EnterChargeNo(shipmentId);
            acknowledgeView.ToggleSwitch();
            acknowledgeView.Authorize("ABC");
            addMoveByShipmentView.Screenshot("Step 5");
            acknowledgeView.Approve();
            addMoveByShipmentView.Screenshot("Step 6");
            Assert.Pass();
        }
        
        [Test]
        public void TC3_AddMoveByShipmentId_MoveTypeBobTail()
        {
            const string shipmentId = "123456";
            const string moveType = "Bobtail";
            const string fromBlock = "A1300";
            const string toBlock = "A1324";
            const string dispatchingParty = "XPO";

            LoginView loginView = new LoginView(CurrentApp);
            LicenseView licenseView = new LicenseView(CurrentApp);
            MainMenuView mainMenuView  = new MainMenuView(CurrentApp);

            licenseView.AcceptTermOfServices(CurrentApp);
            loginView.LoginIntoApplication(Username, Password);

            mainMenuView.StartWorkday(YesNoAction.PressYes);
            mainMenuView.TapAddMoveOption();
            
            AddMoveByShipmentView addMoveByShipmentView = new AddMoveByShipmentView(CurrentApp);

            addMoveByShipmentView.EnterShipmentId(shipmentId);
            addMoveByShipmentView.EnterActivityType(moveType);
            addMoveByShipmentView.EnterFromBlock(fromBlock);
            addMoveByShipmentView.EnterToBlock(toBlock);
            addMoveByShipmentView.EnterDispatcher(dispatchingParty);
            addMoveByShipmentView.Screenshot("Step 1");
            addMoveByShipmentView.TapNextButton();

            MoveServiceInitialSummaryView addMoveInitialSummaryView = new MoveServiceInitialSummaryView(CurrentApp);
            addMoveByShipmentView.Screenshot("Step 2");
            addMoveInitialSummaryView.ConfirmStartAction();
            addMoveByShipmentView.Screenshot("Step 3");
            MoveServiceExecutingView addMoveFinishSummaryView = new MoveServiceExecutingView(CurrentApp);
            addMoveFinishSummaryView.ConfirmFinishAction();
            addMoveByShipmentView.Screenshot("Step 4");
            MoveServiceFinishSummaryView moveServiceFinishSummaryView = new MoveServiceFinishSummaryView(CurrentApp);
            moveServiceFinishSummaryView.GoToMainMenu();

            Assert.Pass();
        }

        [Test]
        public void TC4_AddMoveByShipmentId_MoveTypeOutPlantMove()
        {
            const string shipmentId = "123456";
            const string activityType = "Out Plant Move";
            const string fromBlock = "A1300";
            const string toBlock = "A1324";
            const string dispatchingParty = "XPO";
            const string productName = "1-OCTENE";
            const string equipTypeName = "Tank";
            const string equipSizeName = "25000";
            const string equipNumber = "PACU0000004";
            const string equipStatusName = "Load";
            const string chassisNumber = "GRSD4831";
            const string date25 = "05/05/2024";
            const string date5 = "05/05/2024";

            LoginView loginView = new LoginView(CurrentApp);
            LicenseView licenseView = new LicenseView(CurrentApp);
            MainMenuView mainMenuView = new MainMenuView(CurrentApp);

            licenseView.AcceptTermOfServices(CurrentApp);
            loginView.LoginIntoApplication(Username, Password);

            mainMenuView.StartWorkday(YesNoAction.PressYes);
            mainMenuView.TapAddMoveOption();

            AddMoveByShipmentView addMoveByShipmentView = new AddMoveByShipmentView(CurrentApp);

            addMoveByShipmentView.EnterShipmentId(shipmentId);
            addMoveByShipmentView.EnterActivityType(activityType);
            addMoveByShipmentView.EnterFromBlock(fromBlock);
            addMoveByShipmentView.EnterToBlock(toBlock);
            addMoveByShipmentView.EnterDispatcher(dispatchingParty);
            addMoveByShipmentView.EnterProduct(productName);
            addMoveByShipmentView.EnterEquipmentType(equipTypeName);
            addMoveByShipmentView.EnterEquipmentSize(equipSizeName);
            addMoveByShipmentView.EnterEquipmentNumber(equipNumber);
            addMoveByShipmentView.EnterEquipmentStatus(equipStatusName);
            addMoveByShipmentView.Enter2dot5YearTestDate(date25);
            addMoveByShipmentView.Enter5YearTestDate(date5);
            addMoveByShipmentView.EnterChassisNumber(chassisNumber);
            addMoveByShipmentView.Screenshot("Step 1");
            addMoveByShipmentView.TapNextButton();

            MoveServiceInitialSummaryView addMoveInitialSummaryView = new MoveServiceInitialSummaryView(CurrentApp);
            addMoveByShipmentView.Screenshot("Step 2");
            addMoveInitialSummaryView.ConfirmStartAction();

            MoveServiceExecutingView addMoveFinishSummaryView = new MoveServiceExecutingView(CurrentApp);
            addMoveByShipmentView.Screenshot("Step 3");
            addMoveFinishSummaryView.ConfirmFinishAction();
            addMoveByShipmentView.Screenshot("Step 4");
            MoveServiceFinishSummaryView moveServiceFinishSummaryView = new MoveServiceFinishSummaryView(CurrentApp);
            moveServiceFinishSummaryView.GoToMainMenu();

            Assert.Pass();
        }

        [Test]
        public void TC5_AddMoveByShipmentId_MoveTypeInPlantMove()
        {
            const string shipmentId = "123456";
            const string activityType = "In Plant Move";
            const string fromBlock = "A1300";
            const string toBlock = "A1324";
            const string dispatchingParty = "XPO";
            const string productName = "1-OCTENE";
            const string equipTypeName = "Container";
            const string equipSizeName = "40 foot";
            const string equipNumber = "PACU843347";
            const string equipStatusName = "Load";
            const string chassisNumber = "PACZ123456";
            

            LoginView loginView = new LoginView(CurrentApp);
            LicenseView licenseView = new LicenseView(CurrentApp);
            MainMenuView mainMenuView = new MainMenuView(CurrentApp);

            licenseView.AcceptTermOfServices(CurrentApp);
            loginView.LoginIntoApplication(Username, Password);

            mainMenuView.StartWorkday(YesNoAction.PressYes);
            mainMenuView.TapAddMoveOption();

            AddMoveByShipmentView addMoveByShipmentView = new AddMoveByShipmentView(CurrentApp);

            addMoveByShipmentView.EnterShipmentId(shipmentId);
            addMoveByShipmentView.EnterActivityType(activityType);
            addMoveByShipmentView.EnterFromBlock(fromBlock);
            addMoveByShipmentView.EnterToBlock(toBlock);
            addMoveByShipmentView.EnterDispatcher(dispatchingParty);
            addMoveByShipmentView.EnterProduct(productName);
            addMoveByShipmentView.EnterEquipmentType(equipTypeName);
            addMoveByShipmentView.EnterEquipmentSize(equipSizeName);
            addMoveByShipmentView.EnterEquipmentNumber(equipNumber);
            addMoveByShipmentView.EnterEquipmentStatus(equipStatusName);
            addMoveByShipmentView.EnterChassisNumber(chassisNumber);
            addMoveByShipmentView.Screenshot("Step 1");
            addMoveByShipmentView.TapNextButton();

            MoveServiceInitialSummaryView addMoveInitialSummaryView = new MoveServiceInitialSummaryView(CurrentApp);
            addMoveByShipmentView.Screenshot("Step 2");
            addMoveInitialSummaryView.ConfirmStartAction();

            MoveServiceExecutingView addMoveFinishSummaryView = new MoveServiceExecutingView(CurrentApp);
            addMoveByShipmentView.Screenshot("Step 3");
            addMoveFinishSummaryView.ConfirmFinishAction();
            addMoveByShipmentView.Screenshot("Step 4");
            MoveServiceFinishSummaryView moveServiceFinishSummaryView = new MoveServiceFinishSummaryView(CurrentApp);
            moveServiceFinishSummaryView.GoToMainMenu();

            Assert.Pass();
        }

        [Test]
        public void TC6_AddMoveByShipmentId_MoveTypeInPlantMove_WithContinueServiceShipmentId()
        {
            const string shipmentId = "123456";
            const string activityType = "In Plant Move";
            const string fromBlock = "A1300";
            const string toBlock = "A1324";
            const string dispatchingParty = "XPO";
            const string productName = "1-OCTENE";
            const string equipTypeName = "Container";
            const string equipSizeName = "40 foot";
            const string equipNumber = "PACU843347";
            const string equipStatusName = "Load";
            const string chassisNumber = "PACZ123456";

            const string serviceMoveType = "Loading at Block";
            const string serviceEquipmentStatus = "Load";
            const string serviceShipmentId = "123456";
            
            LoginView loginView = new LoginView(CurrentApp);
            LicenseView licenseView = new LicenseView(CurrentApp);
            MainMenuView mainMenuView = new MainMenuView(CurrentApp);

            licenseView.AcceptTermOfServices(CurrentApp);
            loginView.LoginIntoApplication(Username, Password);

            mainMenuView.StartWorkday(YesNoAction.PressYes);
            mainMenuView.TapAddMoveOption();

            AddMoveByShipmentView addMoveByShipmentView = new AddMoveByShipmentView(CurrentApp);

            addMoveByShipmentView.EnterShipmentId(shipmentId);
            addMoveByShipmentView.EnterActivityType(activityType);
            addMoveByShipmentView.EnterFromBlock(fromBlock);
            addMoveByShipmentView.EnterToBlock(toBlock);
            addMoveByShipmentView.EnterDispatcher(dispatchingParty);
            addMoveByShipmentView.EnterProduct(productName);
            addMoveByShipmentView.EnterEquipmentType(equipTypeName);
            addMoveByShipmentView.EnterEquipmentSize(equipSizeName);
            addMoveByShipmentView.EnterEquipmentNumber(equipNumber);
            addMoveByShipmentView.EnterEquipmentStatus(equipStatusName);
            addMoveByShipmentView.EnterChassisNumber(chassisNumber);
            addMoveByShipmentView.Screenshot("Step 1");
            addMoveByShipmentView.TapNextButton();

            MoveServiceInitialSummaryView moveServiceInitialSummaryView = new MoveServiceInitialSummaryView(CurrentApp);
            addMoveByShipmentView.Screenshot("Step 2");
            moveServiceInitialSummaryView.ConfirmStartAction();

            MoveServiceExecutingView moveServiceExecutingView = new MoveServiceExecutingView(CurrentApp);
            addMoveByShipmentView.Screenshot("Step 3");
            moveServiceExecutingView.ConfirmFinishAction();
            
            MoveServiceFinishSummaryView moveServiceFinishSummaryView = new MoveServiceFinishSummaryView(CurrentApp);
            moveServiceFinishSummaryView.ContinueAction(MoveServiceAction.PressService);
            addMoveByShipmentView.Screenshot("Step 4");
            AddServiceByShipmentView addServiceByShipmentView = new AddServiceByShipmentView(CurrentApp);
            addServiceByShipmentView.EnterMoveType(serviceMoveType);
            addServiceByShipmentView.EnterEquipmentStatus(serviceEquipmentStatus);
            addServiceByShipmentView.TapNextButton();
            Thread.Sleep(1000);
            addMoveByShipmentView.Screenshot("Step 5");
            moveServiceInitialSummaryView.ConfirmStartActionByText();
            Thread.Sleep(1000);
            addMoveByShipmentView.Screenshot("Step 6");
            moveServiceExecutingView.ConfirmFinishActionByText();
            Thread.Sleep(5000);
            addMoveByShipmentView.Screenshot("Step 7");
            moveServiceFinishSummaryView.GoToMainMenuByText();
            
            Assert.Pass();
        }

        [Test]
        public void TC7_RegisterMove_MoveTypeRespot_WithContinueServiceByCostCenter()
        {
            const string shipmentId = "123456";
            const string activityType = "Respot";
            const string fromBlock = "A1300";
            const string toBlock = "A1324";
            const string dispatchingParty = "XPO";
            const string productName = "1-OCTENE";
            const string equipTypeName = "Tank";
            const string equipSizeName = "25000";
            const string equipNumber = "PAPU0000004";
            const string equipStatusName = "Load";
            const string chassisNumber = "GRSD4831";
            const string date25 = "05/05/2024";
            const string date5 = "05/05/2024";

            const string serviceMoveType = "Loading at Block";
            const string serviceEquipmentType = "Tank";
            const string serviceEquipmentStatus = "Load";
            const string serviceChargeNo = "0001-D002-56955";
            const string serviceDispatcher = "XPO";
            const string serviceProductName = "1-OCTENE";

            LoginView loginView = new LoginView(CurrentApp);
            LicenseView licenseView = new LicenseView(CurrentApp);
            MainMenuView mainMenuView = new MainMenuView(CurrentApp);

            licenseView.AcceptTermOfServices(CurrentApp);
            loginView.LoginIntoApplication(Username, Password);

            mainMenuView.StartWorkday(YesNoAction.PressYes);
            mainMenuView.TapAddMoveOption();

            AddMoveByShipmentView addMoveByShipmentView = new AddMoveByShipmentView(CurrentApp);
            addMoveByShipmentView.EnterShipmentId(shipmentId);
            addMoveByShipmentView.EnterActivityType(activityType);
            addMoveByShipmentView.EnterFromBlock(fromBlock);
            addMoveByShipmentView.EnterToBlock(toBlock);
            addMoveByShipmentView.EnterDispatcher(dispatchingParty);
            addMoveByShipmentView.EnterProduct(productName);
            addMoveByShipmentView.EnterEquipmentType(equipTypeName);
            addMoveByShipmentView.EnterEquipmentSize(equipSizeName);
            addMoveByShipmentView.EnterEquipmentNumber(equipNumber);
            addMoveByShipmentView.EnterEquipmentStatus(equipStatusName);
            addMoveByShipmentView.Enter2dot5YearTestDate(date25);
            addMoveByShipmentView.Enter5YearTestDate(date5);
            addMoveByShipmentView.EnterChassisNumber(chassisNumber);
            addMoveByShipmentView.Screenshot("Step 1");
            addMoveByShipmentView.TapNextButton();

            MoveServiceInitialSummaryView moveServiceInitialSummaryView = new MoveServiceInitialSummaryView(CurrentApp);
            addMoveByShipmentView.Screenshot("Step 2");
            moveServiceInitialSummaryView.ConfirmStartAction();

            MoveServiceExecutingView moveServiceExecutingView = new MoveServiceExecutingView(CurrentApp);
            addMoveByShipmentView.Screenshot("Step 3");
            moveServiceExecutingView.ConfirmFinishAction();

            MoveServiceFinishSummaryView moveServiceFinishSummaryView = new MoveServiceFinishSummaryView(CurrentApp);
            addMoveByShipmentView.Screenshot("Step 4");
            moveServiceFinishSummaryView.StartNewMoveService(MoveServiceAction.PressService);

            AddServiceByCostCenterView addServiceByCostCenterView = new AddServiceByCostCenterView(CurrentApp);
            addServiceByCostCenterView.SelectCostCenterTab();
            addServiceByCostCenterView.EnterChargeNo(serviceChargeNo);
            addServiceByCostCenterView.EnterMoveType(serviceMoveType);
            addServiceByCostCenterView.EnterFromBlock(toBlock);
            addServiceByCostCenterView.EnterDispatcher(serviceDispatcher);
            addServiceByCostCenterView.EnterProduct(serviceProductName);
            addServiceByCostCenterView.EnterEquipmentType(serviceEquipmentType);
            addServiceByCostCenterView.EnterEquipmentSize(equipSizeName);
            addServiceByCostCenterView.EnterEquipmentNumber(equipNumber);
            addServiceByCostCenterView.EnterEquipmentStatus(serviceEquipmentStatus);
            addServiceByCostCenterView.Enter2dot5YearTestDate(date25);
            addServiceByCostCenterView.Enter5YearTestDate(date5);
            addServiceByCostCenterView.EnterChassisNumber(chassisNumber);
            addMoveByShipmentView.Screenshot("Step 5");
            addServiceByCostCenterView.TapNextButton();
            Thread.Sleep(1000);
            addMoveByShipmentView.Screenshot("Step 6");
            moveServiceInitialSummaryView.ConfirmStartActionByText();
            Thread.Sleep(1000);
            addMoveByShipmentView.Screenshot("Step 7");
            moveServiceExecutingView.ConfirmFinishActionByText();
            Thread.Sleep(5000);
            moveServiceFinishSummaryView.GoToMainMenuByText();

            Assert.Pass();
        }

        [Test]
        public void TC8_RegisterServiceByCostCenter_ServiceTypeLoadingAtBlock_WithContinueServiceByShipmentId()
        {
            const string serviceChargeNo = "0001-D002-56955";
            const string serviceType = "Loading at Block";
            const string block = "A1300";
            const string productName = "1-OCTENE";
            const string equipTypeName = "Container";
            const string equipSizeName = "40 foot";
            const string equipNumber = "PACU843347";
            const string equipStatusName = "Load";
            const string chassisNumber = "PACZ123456";
            const string dispatchingParty = "XPO";

            const string serviceMoveType = "Loading at Block";
            const string serviceEquipmentStatus = "Load";
            const string serviceShipmentId = "123456";
            const string serviceEquipTypeName = "Container";
            const string serviceEquipSizeName = "40 foot";
            const string serviceEquipNumber = "PACU843347";
            const string serviceChassisNumber = "PACZ123456";

            LoginView loginView = new LoginView(CurrentApp);
            LicenseView licenseView = new LicenseView(CurrentApp);
            MainMenuView mainMenuView = new MainMenuView(CurrentApp);

            licenseView.AcceptTermOfServices(CurrentApp);
            loginView.LoginIntoApplication(Username, Password);

            mainMenuView.StartWorkday(YesNoAction.PressYes);
            mainMenuView.TapAddServiceOption();

            AddServiceByCostCenterView addServiceByCostCenterView = new AddServiceByCostCenterView(CurrentApp);
            addServiceByCostCenterView.SelectCostCenterTab();
            addServiceByCostCenterView.EnterChargeNo(serviceChargeNo);
            addServiceByCostCenterView.EnterMoveType(serviceType);
            addServiceByCostCenterView.EnterFromBlock(block);
            addServiceByCostCenterView.EnterProduct(productName);
            addServiceByCostCenterView.EnterDispatcher(dispatchingParty);
            addServiceByCostCenterView.EnterEquipmentType(equipTypeName);
            addServiceByCostCenterView.EnterEquipmentSize(equipSizeName);
            addServiceByCostCenterView.EnterEquipmentNumber(equipNumber);
            addServiceByCostCenterView.EnterEquipmentStatus(equipStatusName);
            addServiceByCostCenterView.EnterChassisNumber(chassisNumber);
            addServiceByCostCenterView.Screenshot("Step 1");
            addServiceByCostCenterView.TapNextButton();
            
            MoveServiceInitialSummaryView moveServiceInitialSummaryView = new MoveServiceInitialSummaryView(CurrentApp);
            Thread.Sleep(1000);
            addServiceByCostCenterView.Screenshot("Step 2");
            moveServiceInitialSummaryView.ConfirmStartActionByText();
            
            MoveServiceExecutingView moveServiceExecutingView = new MoveServiceExecutingView(CurrentApp);
            Thread.Sleep(2000);
            addServiceByCostCenterView.Screenshot("Step 3");
            moveServiceExecutingView.ConfirmFinishActionByText();

            MoveServiceFinishSummaryView moveServiceFinishSummaryView = new MoveServiceFinishSummaryView(CurrentApp);
            Thread.Sleep(5000);
            moveServiceFinishSummaryView.StartNewMoveServiceByText(MoveServiceAction.PressService);
            Thread.Sleep(1000);
            AddServiceByShipmentView addServiceByShipmentView = new AddServiceByShipmentView(CurrentApp);
            addServiceByShipmentView.EnterShipmentId(serviceShipmentId);
            addServiceByShipmentView.EnterMoveType(serviceMoveType);
            addServiceByShipmentView.EnterDispatcher(dispatchingParty);
            addServiceByShipmentView.EnterFromBlock(block);
            addServiceByShipmentView.EnterEquipmentType(serviceEquipTypeName);
            addServiceByShipmentView.EnterEquipmentSize(serviceEquipSizeName);
            addServiceByShipmentView.EnterEquipmentNumber(serviceEquipNumber);
            addServiceByShipmentView.EnterEquipmentStatus(serviceEquipmentStatus);
            addServiceByShipmentView.EnterChassisNumber(serviceChassisNumber);
            addServiceByShipmentView.Screenshot("Step 4");
            addServiceByShipmentView.TapNextButton();
            Thread.Sleep(1000);
            addServiceByShipmentView.Screenshot("Step 5");
            moveServiceInitialSummaryView.ConfirmStartActionByText();
            Thread.Sleep(1000);
            addServiceByShipmentView.Screenshot("Step 6");
            moveServiceExecutingView.ConfirmFinishActionByText();
            Thread.Sleep(5000);
            addServiceByShipmentView.Screenshot("Step 7");
            moveServiceFinishSummaryView.GoToMainMenuByText();

            Assert.Pass();
        }

        [Test]
        public void TC9_AddMoveByShipmentId_MoveTypeBobTail_ThenStoreAndForward()
        {
            const string shipmentId = "123456";
            const string moveType = "Bobtail";
            const string fromBlock = "A1300";
            const string toBlock = "A1324";
            const string dispatchingParty = "XPO";

            LoginView loginView = new LoginView(CurrentApp);
            LicenseView licenseView = new LicenseView(CurrentApp);
            MainMenuView mainMenuView = new MainMenuView(CurrentApp);
            StoreAndForwardView storeAndForwardView = new StoreAndForwardView(CurrentApp);

            licenseView.AcceptTermOfServices(CurrentApp);
            loginView.LoginIntoApplication(Username, Password);

            mainMenuView.StartWorkday(YesNoAction.PressYes);

            for (int i = 0; i < 10; i++)
            {
                mainMenuView.TapAddMoveOption();

                AddMoveByShipmentView addMoveByShipmentView = new AddMoveByShipmentView(CurrentApp);

                addMoveByShipmentView.EnterShipmentId(string.Format("{0}{1}",shipmentId, i));
                addMoveByShipmentView.EnterActivityType(moveType);
                addMoveByShipmentView.EnterFromBlock(fromBlock);
                addMoveByShipmentView.EnterToBlock(toBlock);
                addMoveByShipmentView.EnterDispatcher(dispatchingParty);
                addMoveByShipmentView.TapNextButton();

                MoveServiceInitialSummaryView addMoveInitialSummaryView = new MoveServiceInitialSummaryView(CurrentApp);
                addMoveInitialSummaryView.ConfirmStartAction();

                MoveServiceExecutingView addMoveFinishSummaryView = new MoveServiceExecutingView(CurrentApp);
                addMoveFinishSummaryView.ConfirmFinishAction();

                MoveServiceFinishSummaryView moveServiceFinishSummaryView = new MoveServiceFinishSummaryView(CurrentApp);
                moveServiceFinishSummaryView.GoToMainMenu();
                moveServiceFinishSummaryView.Screenshot("Step " + i + " -1");
                mainMenuView.TapStoreAndForward();
                Thread.Sleep(5000);
                moveServiceFinishSummaryView.Screenshot("Step " + i + " -2");
                storeAndForwardView.BackToMainMenu();
            }

            Assert.Pass();
        }

        [Test]
        public void TC10_TryToAddMoveByShipmentId_ThenGoStoreAndForward()
        {
            const string shipmentId = "123456";
            const string moveType = "Bobtail";
            const string fromBlock = "A1300";

            LoginView loginView = new LoginView(CurrentApp);
            LicenseView licenseView = new LicenseView(CurrentApp);
            MainMenuView mainMenuView = new MainMenuView(CurrentApp);
            StoreAndForwardView storeAndForwardView = new StoreAndForwardView(CurrentApp);

            licenseView.AcceptTermOfServices(CurrentApp);
            loginView.LoginIntoApplication(Username, Password);

            mainMenuView.StartWorkday(YesNoAction.PressYes);

            for (int i = 0; i < 10; i++)
            {
                mainMenuView.TapAddMoveOption();

                AddMoveByShipmentView addMoveByShipmentView = new AddMoveByShipmentView(CurrentApp);

                addMoveByShipmentView.EnterShipmentId(string.Format("{0}{1}", shipmentId, i));
                addMoveByShipmentView.EnterActivityType(moveType);
                addMoveByShipmentView.EnterFromBlock(fromBlock);
                addMoveByShipmentView.Screenshot("Step " + i + " -1");
                addMoveByShipmentView.BackToMainMenu();
               
                mainMenuView.TapStoreAndForward();
                addMoveByShipmentView.Screenshot("Step " + i + " -2");
                Thread.Sleep(3000);
                storeAndForwardView.BackToMainMenu();
            }

            Assert.Pass();
        }

    }
}
