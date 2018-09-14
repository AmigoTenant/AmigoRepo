using System;
using System.Linq;
using System.Collections.ObjectModel;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using XPO.ShuttleTracking.Application.DTOs.Requests.Tracking;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Entity;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
using XPO.ShuttleTracking.Mobile.Entity.Service;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Entity.Acknowledge;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Infrastructure;
using XPO.ShuttleTracking.Mobile.Navigation;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class AcknowledgeMoveViewModel : TodayViewModel
    {
        private readonly IMoveRepository _moveRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IDetentionRepository _detentionRepository;
        private readonly IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private readonly INavigator _navigator;
        private bool _isAutomatic;
        private AcknowledgeMoveTaskDefinition _request;
        private readonly IWebServiceCallingInfomationProvider _infomationProvider;
        private readonly INetworkInfoManager _networkInfoManager;

        public AcknowledgeMoveViewModel(IMoveRepository moveRepository,
                                        IServiceRepository serviceRepository,
                                        IDetentionRepository detentionRepository,
                                        IOperateTaylorLiftRepository operateTaylorLiftRepository,
                                        IWebServiceCallingInfomationProvider infomationProvider,
                                        INetworkInfoManager networkInfoManager,
                                        INavigator navigator)
        {
            _moveRepository = moveRepository;
            _serviceRepository = serviceRepository;
            _detentionRepository = detentionRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _infomationProvider = infomationProvider;
            _networkInfoManager = networkInfoManager;
            _navigator = navigator;
        }
        public override void OnPushed()
        {
            base.OnPushed();
            Device.BeginInvokeOnMainThread(() =>
            {
                ShowContent = false;
                LstAcknowledge = new ObservableCollection<BEAcknowledgeMove>();
                SelectedChargeNumbers = new List<BESelectChargeNumber>();
                UpdateAcknowledgeMoveTask();
                UpdateSelectedChargeNumberText();
            });
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            UpdateSelectedChargeNumberText();
        }
        private void UpdateSelectedChargeNumberText()
        {
            if (SelectedChargeNumbers == null || !SelectedChargeNumbers.Any(x => x.IsSelected))
            {
                SelectedChargeNoDesc = AppString.lblSelectChargeNumber;
                UpdateAcknowledgeMoveDetail(null);
            }
            else
            {
                var lstChargeNumbers = SelectedChargeNumbers.Where(x => x.IsSelected && x.Description != AppString.lblSelectAll).Select(x => x.Description).ToList();
                SelectedChargeNoDesc = string.Join(", ", lstChargeNumbers);
                UpdateAcknowledgeMoveDetail(lstChargeNumbers);
            }
        }
        private async void UpdateAcknowledgeMoveTask()
        {
            await Task.Run(() => UpdateAcknowledgeMove());
        }
        private async void UpdateAcknowledgeMove()
        {
            using (StartLoading(AppString.lblAcknowledgeLoading))
            {
                await Task.Run(() =>
                {
                    //Move
                    var lstMShipment =
                        (_moveRepository.FindAll(
                            x => (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove)
                                 && !string.IsNullOrEmpty(x.ShipmentID)
                                 && x.AcknowledgeState == AcknowledgeState.NotAuthorized))
                            .Select(x => x.ShipmentID).Distinct();
                    var lstMCostCenter =
                        (_moveRepository.FindAll(
                            x => (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove)
                                 && !string.IsNullOrEmpty(x.CostCenterName)
                                 && x.AcknowledgeState == AcknowledgeState.NotAuthorized))
                            .Select(x => x.CostCenterName).Distinct();
                    //Service
                    var lstSShipment =
                        (_serviceRepository.FindAll(
                            x => (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove)
                                 && !string.IsNullOrEmpty(x.ShipmentID)
                                 && x.AcknowledgeState == AcknowledgeState.NotAuthorized))
                            .Select(x => x.ShipmentID).Distinct();
                    var lstSCostCenter =
                        (_serviceRepository.FindAll(
                            x => (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove)
                                 && !string.IsNullOrEmpty(x.CostCenterName)
                                 && x.AcknowledgeState == AcknowledgeState.NotAuthorized))
                            .Select(x => x.CostCenterName).Distinct();
                    //Detention
                    var lstDShipment =
                        (_detentionRepository.FindAll(
                            x => (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove)
                                 && !string.IsNullOrEmpty(x.ShipmentID)
                                 && x.AcknowledgeState == AcknowledgeState.NotAuthorized))
                            .Select(x => x.ShipmentID).Distinct();
                    var lstDCostCenter =
                        (_detentionRepository.FindAll(
                            x => (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove)
                                 && !string.IsNullOrEmpty(x.CostCenterName)
                                 && x.AcknowledgeState == AcknowledgeState.NotAuthorized))
                            .Select(x => x.CostCenterName).Distinct();
                    //Taylor
                    var lstTShipment =
                        (_operateTaylorLiftRepository.FindAll(
                            x => (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove)
                                 && !string.IsNullOrEmpty(x.ShipmentID)
                                 && x.AcknowledgeState == AcknowledgeState.NotAuthorized))
                            .Select(x => x.ShipmentID).Distinct();
                    var lstTCostCenter =
                        (_operateTaylorLiftRepository.FindAll(
                            x => (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove)
                                 && !string.IsNullOrEmpty(x.CostCenterName)
                                 && x.AcknowledgeState == AcknowledgeState.NotAuthorized))
                            .Select(x => x.CostCenterName).Distinct();

                    var sec = new ObservableCollection<string>(lstMShipment)
                        .Concat(lstMCostCenter)
                        .Concat(lstSShipment)
                        .Concat(lstSCostCenter)
                        .Concat(lstDShipment)
                        .Concat(lstDCostCenter)
                        .Concat(lstTShipment)
                        .Concat(lstTCostCenter);
                    sec = sec.Distinct().OrderByDescending(s => s).Reverse();
                    LstCharge = new ObservableCollection<string>(new List<string>() {AppString.lblSelectAll}).Concat(sec);

                    ShowSelectedNumberFromMain();
                    UpdateAcknowledgeMoveDetail(null);
                    PkrChargeTitle = AppString.lblSummaryChargeNoWDot;
                });

                SelectedChargeNumbers = LstCharge.Select(charge => new BESelectChargeNumber() { Description = charge, IsSelected = false }).ToList();
            }
        }
        private void UpdateAcknowledgeMoveDetail(List<string> searchField)
        {
            LstAcknowledge = new ObservableCollection<BEAcknowledgeMove>();
            ShowContent = false;
            SelectedLabel = TxtAuthorizedBy = string.Empty;
            IsAllSelected = false;
            if (searchField != null && searchField.Any())
            {
                var lstMRepo = (_moveRepository.FindAll(x => (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove) && x.AcknowledgeState == AcknowledgeState.NotAuthorized))
                    .Where(x => searchField.Any(y=> y== x.ShipmentID) || searchField.Any(y => y == x.CostCenterName)).ToList();

                var lstSRepo = (_serviceRepository.FindAll(x => (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove) && x.AcknowledgeState == AcknowledgeState.NotAuthorized))
                    .Where(x => searchField.Any(y => y == x.ShipmentID) || searchField.Any(y => y == x.CostCenterName)).ToList();

                var lstDRepo = (_detentionRepository.FindAll(x => (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove) && x.AcknowledgeState == AcknowledgeState.NotAuthorized))
                    .Where(x => searchField.Any(y => y == x.ShipmentID) || searchField.Any(y => y == x.CostCenterName)).ToList();

                var lstTRepo = (_operateTaylorLiftRepository.FindAll(x => (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove) && x.AcknowledgeState == AcknowledgeState.NotAuthorized))
                    .Where(x => searchField.Any(y => y == x.ShipmentID) || searchField.Any(y => y == x.CostCenterName)).ToList();

                foreach (var repo in lstMRepo)
                    LstAcknowledge.Add(new BEAcknowledgeMove() {ChargeNumber = GetChargeNumber(repo), MoveLine = repo, TypeSelector = AcknowledgeType.Move, ShowSelectedNumber = ShowSelectedNumber });
                foreach (var repo in lstSRepo)
                    LstAcknowledge.Add(new BEAcknowledgeMove() { ChargeNumber = GetChargeNumber(repo), ServiceLine = repo, TypeSelector = AcknowledgeType.Service, ShowSelectedNumber = ShowSelectedNumber });
                foreach (var repo in lstDRepo)
                    LstAcknowledge.Add(new BEAcknowledgeMove() { ChargeNumber = GetChargeNumber(repo), DetentionLine = repo, TypeSelector = AcknowledgeType.Detention, ShowSelectedNumber = ShowSelectedNumber });
                foreach (var repo in lstTRepo)
                    LstAcknowledge.Add(new BEAcknowledgeMove() { ChargeNumber = GetChargeNumber(repo), OperateTaylorLine = repo, TypeSelector = AcknowledgeType.OperateTaylor, ShowSelectedNumber = ShowSelectedNumber });
            }
            if (LstAcknowledge.Count > 0)
                ShowContent = true;
            ShowSelectedNumberFromMain();
        }

        private static string GetChargeNumber(BEServiceBase service)
        {
            return string.IsNullOrEmpty(service.ShipmentID) ? service.CostCenterName : service.ShipmentID;
        }
        private void ChangeListState(bool value)
        {
            var lstTemp = new ObservableCollection<BEAcknowledgeMove>();
            foreach (var acknowledge in LstAcknowledge)
            {
                var newAcknow = new BEAcknowledgeMove()
                {
                    DetentionLine = acknowledge.DetentionLine,
                    MoveLine = acknowledge.MoveLine,
                    ServiceLine = acknowledge.ServiceLine,
                    OperateTaylorLine = acknowledge.OperateTaylorLine,
                    TypeSelector = acknowledge.TypeSelector,
                    IsSelected = value,
                    ShowSelectedNumber = ShowSelectedNumber
                };
                lstTemp.Add(newAcknow);
            }
            LstAcknowledge = lstTemp;
            ShowSelectedNumberFromMain();
        }

        private Infrastructure.BackgroundTasks.TaskStatus RegisterAcknowledgeMove()
        {
            DateTime dateTimeTemp = DateTime.Now;
            _request.RegisteredDate = dateTimeTemp;
            _request.ExecutionDate = dateTimeTemp;
            _request.ActivityCode = ActivityCode.AcknowledgeMove;

            ShuttleTEventLogDTO shuttleTEventLogDTO = _infomationProvider.FillTaskShuttleTEventLogDTO(_request.ActivityCode, dateTimeTemp);

            _request.UpdateShuttleTServiceAckRequest = new UpdateShuttleTServiceAckRequest
            {
                AcknowledgeBy = TxtAuthorizedBy,
                ServiceAcknowledgeDate = DateTimeOffset.Now,
                ServiceAcknowledgeDateTZ = TimeZoneInfo.Local.ToString(),

                IsAutoDateTime = shuttleTEventLogDTO.IsAutoDateTime,
                IsSpoofingGPS = shuttleTEventLogDTO.IsSpoofingGPS,
                IsRootedJailbreaked = shuttleTEventLogDTO.IsRootedJailbreaked,
                Platform = shuttleTEventLogDTO.Platform,
                OSVersion = shuttleTEventLogDTO.OSVersion,
                AppVersion = shuttleTEventLogDTO.AppVersion,
                Latitude = shuttleTEventLogDTO.Latitude,
                Longitude = shuttleTEventLogDTO.Longitude,
                Accuracy = shuttleTEventLogDTO.Accuracy,
                LocationProvider = shuttleTEventLogDTO.LocationProvider,
                ReportedActivityTimeZone = shuttleTEventLogDTO.ReportedActivityTimeZone,
                ReportedActivityDate = shuttleTEventLogDTO.ReportedActivityDate,
                ActivityTypeId = shuttleTEventLogDTO.ActivityTypeId ?? 0,
            };

            _request.UpdateShuttleTServiceAckRequest.ServiceOrderNoList = GetStringForRequest(_request);

            TaskManager.Current.RegisterStoreAndForward(_request);
            return Infrastructure.BackgroundTasks.TaskStatus.Pendent;
        }

        private List<ServiceOrderNoRequest> GetStringForRequest(AcknowledgeMoveTaskDefinition data)
        {
            var listIdForRequest = (from acknowledge in data.MoveLine select _moveRepository.FindByKey(acknowledge.InternalId) into move where move != null select new ServiceOrderNoRequest() { ServiceOrderNo = move.InternalId, ShuttleTServiceId = move.MoveId }).ToList();
            listIdForRequest.AddRange((from acknowledge in data.ServiceLine select _serviceRepository.FindByKey(acknowledge.InternalId) into move where move != null select new ServiceOrderNoRequest() { ServiceOrderNo = move.InternalId, ShuttleTServiceId = move.ServiceId }));
            listIdForRequest.AddRange((from acknowledge in data.DetentionLine select _detentionRepository.FindByKey(acknowledge.InternalId) into move where move != null select new ServiceOrderNoRequest() { ServiceOrderNo = move.InternalId, ShuttleTServiceId = move.DetentionId }));
            listIdForRequest.AddRange((from acknowledge in data.OperateTaylorLine select _operateTaylorLiftRepository.FindByKey(acknowledge.InternalId) into move where move != null select new ServiceOrderNoRequest() { ServiceOrderNo = move.InternalId, ShuttleTServiceId = move.OperateTaylorLiftId }));
            return listIdForRequest;
        }

        private void ShowSelectedNumberFromMain()
        {
            if (_isAutomatic)
            {
                _isAutomatic = false;
                return;
            }
            if (LstAcknowledge == null) return;
            var count = 0;
            foreach (var acknow in LstAcknowledge)
            {
                if (acknow.IsSelected)
                    count++;
            }
            _selectedAcknowlegdeMoves = count;
            SelectedLabel = string.Format(AppString.lblSelectedOf, _selectedAcknowlegdeMoves, LstAcknowledge.Count);
            LblListByError = string.Empty;
        }
        private void ShowSelectedNumber()
        {
            if (LstAcknowledge == null) return;
            var count = 0;
            foreach (var acknow in LstAcknowledge)
            {
                if (acknow.IsSelected)
                    count++;
            }
            _selectedAcknowlegdeMoves = count;
            if (_selectedAcknowlegdeMoves == LstAcknowledge.Count && !IsAllSelected)
            {
                _isAutomatic = true;
                IsAllSelected = true;
            }
            SelectedLabel = string.Format(AppString.lblSelectedOf, _selectedAcknowlegdeMoves, LstAcknowledge.Count);
            LblListByError = string.Empty;
        }

        #region Commands
        public ICommand SelectChargeNumberCommand => CreateCommand(async () =>
        {
            if (SelectedChargeNumbers.Count > 1) //Read selected values from next screen
                await _navigator.PushAsync<AcknowledgeChargeNoViewModel>(x => x.LstChargeNumber = SelectedChargeNumbers);
            else //Show a message if there are no more moves left
                await ShowOkAlert(AppString.lblDialogNoChargeNumbers);
        });
        public ICommand SendAcknowledgeInformation => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection())
                return;

            _request = new AcknowledgeMoveTaskDefinition();
            _request.MoveLine = new List<BEMove>();
            _request.ServiceLine = new List<BEService>();
            _request.DetentionLine = new List<BEDetention>();
            _request.OperateTaylorLine = new List<BEOperateTaylorLift>();
            var result = LstAcknowledge.Count(n => n.IsSelected);
            LblListByError = string.Empty;
            if (result == 0)
            {
                LblListByError = AppString.lblAcknowledgeSelectOneOption;
                return;
            }
            if (string.IsNullOrEmpty(TxtAuthorizedBy))
                return;
            
            foreach (var acknowledge in LstAcknowledge)
            {
                if (!acknowledge.IsSelected) continue;
                switch (acknowledge.TypeSelector)
                {
                    case AcknowledgeType.Move:
                        acknowledge.MoveLine.AcknowledgeState = AcknowledgeState.PendentProcess;
                        _request.MoveLine.Add(acknowledge.MoveLine);
                        _moveRepository.Update(acknowledge.MoveLine);
                        break;
                    case AcknowledgeType.Service:
                        acknowledge.ServiceLine.AcknowledgeState = AcknowledgeState.PendentProcess;
                        _request.ServiceLine.Add(acknowledge.ServiceLine);
                        _serviceRepository.Update(acknowledge.ServiceLine);
                        break;
                    case AcknowledgeType.Detention:
                        acknowledge.DetentionLine.AcknowledgeState = AcknowledgeState.PendentProcess;
                        _request.DetentionLine.Add(acknowledge.DetentionLine);
                        _detentionRepository.Update(acknowledge.DetentionLine);
                        break;
                    case AcknowledgeType.OperateTaylor:
                        acknowledge.OperateTaylorLine.AcknowledgeState = AcknowledgeState.PendentProcess;
                        _request.OperateTaylorLine.Add(acknowledge.OperateTaylorLine);
                        _operateTaylorLiftRepository.Update(acknowledge.OperateTaylorLine);
                        break;
                }
            }
            using (StartLoading(AppString.lblGeneralLoading))
            {
                var responseMove = RegisterAcknowledgeMove();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                        await ShowOkAlert(AppString.lblAcknowledgeSuccessful);
                        UpdateAcknowledgeMove();
                        break;
                }
            }
        });
        #endregion

        #region Variables

        private string _selectedChargeNoDesc;

        public string SelectedChargeNoDesc
        {
            get { return _selectedChargeNoDesc; }
            set { SetProperty(ref _selectedChargeNoDesc, value); }
        }
        private int _selectedAcknowlegdeMoves;
        private string _selectedLabel;
        public string SelectedLabel
        {
            get { return _selectedLabel; }
            set
            {
                SetProperty(ref _selectedLabel, value);
            }
        }
        private bool _showContent;
        public bool ShowContent
        {
            get { return _showContent; }
            set { SetProperty(ref _showContent, value); }
        }
        private bool _isAllSelected;
        public bool IsAllSelected
        {
            get { return _isAllSelected; }
            set
            {
                SetProperty(ref _isAllSelected, value);
                ChangeListState(value);
            }
        }
        private string _txtAuthorizedBy;
        public string TxtAuthorizedBy
        {
            get { return _txtAuthorizedBy; }
            set { SetProperty(ref _txtAuthorizedBy, value); }
        }
        private string _lblAuthorizedByError;
        public string LblAuthorizedByError
        {
            get { return _lblAuthorizedByError; }
            set { SetProperty(ref _lblAuthorizedByError, value); }
        }
        private string _pkrChargeTitle;
        public string PkrChargeTitle
        {
            get { return _pkrChargeTitle; }
            set { SetProperty(ref _pkrChargeTitle, value); }
        }
        private string _lblListByError;
        public string LblListByError
        {
            get { return _lblListByError; }
            set { SetProperty(ref _lblListByError, value); }
        }
        private IList<BESelectChargeNumber> _selectedChargeNumbers;
        public IList<BESelectChargeNumber> SelectedChargeNumbers
        {
            get { return _selectedChargeNumbers; }
            set
            {
                SetProperty(ref _selectedChargeNumbers, value);
                UpdateSelectedChargeNumberText();
            }
        }
        private IEnumerable<string> _lstCharge;
        public IEnumerable<string> LstCharge
        {
            get { return _lstCharge; }
            protected set { SetProperty(ref _lstCharge, value); }
        }
        private ObservableCollection<BEAcknowledgeMove> _lstAcknowledge = new ObservableCollection<BEAcknowledgeMove>();
        public ObservableCollection<BEAcknowledgeMove> LstAcknowledge
        {
            get { return _lstAcknowledge; }
            set { SetProperty(ref _lstAcknowledge, value); }
        }
        #endregion
    }
}
