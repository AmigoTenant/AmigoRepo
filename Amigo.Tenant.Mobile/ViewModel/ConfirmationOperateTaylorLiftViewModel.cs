using System.Windows.Input;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Resource;
using System;
using System.Globalization;
using System.Threading.Tasks;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Entity;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Entity.Service;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class ConfirmationOperateTaylorLiftViewModel : TodayViewModel
    {
        private readonly INavigator _navigator;
        private readonly IMoveRepository _moveRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IDetentionRepository _detentionRepository;
        private readonly IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private bool _isNewElement = false;
        private bool _isPerHour;
        private static readonly string NotRequired = AppString.lblMsgNotRequired;

        //drop down menu
        private readonly IEquipmentSizeRepository _equipmentSizeRepository;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly ISessionRepository _sessionRepository;

        public ConfirmationOperateTaylorLiftViewModel(INavigator navigator,
            IMoveRepository moveRepository,
            IServiceRepository serviceRepository,
            IDetentionRepository detentionRepository,
            IOperateTaylorLiftRepository operateTaylorLiftRepository,
            IEquipmentSizeRepository equipmentSizeRepository,
            ISessionRepository sessionRepository)
        {
            _navigator = navigator;
            _moveRepository = moveRepository;
            _detentionRepository = detentionRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _equipmentSizeRepository = equipmentSizeRepository;
            _sessionRepository = sessionRepository;
            _serviceRepository = serviceRepository;
        }
        public override void OnPushed()
        {
            base.OnPushed();
            LblId = AppString.lblShipmentId.ToUpper();
            if (!string.IsNullOrEmpty(GeneralOperateTaylorLift.ShipmentID))
            {
                LblId = AppString.lblShipmentId.ToUpper();
                LblNumber = GeneralOperateTaylorLift.ShipmentID;
            }
            else if (!string.IsNullOrEmpty(GeneralOperateTaylorLift.CostCenterName))
            {
                LblId = AppString.lblCostCenter;
                LblNumber = GeneralOperateTaylorLift.CostCenterName;
            }

            var session = _sessionRepository.GetSessionObject();
            _isPerHour = session.TypeUser == UserTypeCode.PerHour;

            TxtBlock = string.IsNullOrEmpty(GeneralOperateTaylorLift.StartName) ? NotRequired : GeneralOperateTaylorLift.StartName;

            TxtChassisNumber = string.IsNullOrEmpty(GeneralOperateTaylorLift.ChassisNumber) ? NotRequired : GeneralOperateTaylorLift.ChassisNumber;
            TxtEquipmentNumber = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentNumber) ? NotRequired : GeneralOperateTaylorLift.EquipmentNumber;
            TxtEquipmentType = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentNumber) ? AppString.lblEquipmentType : GeneralOperateTaylorLift.EquipmentTypeDesc;
            TxtEquipmentSize = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentSizeDesc) ? NotRequired : GeneralOperateTaylorLift.EquipmentSizeDesc;
            TxtEquipmentStatus = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentStatusDesc) ? NotRequired : GeneralOperateTaylorLift.EquipmentStatusDesc;
            TxtEquipmentProduct = string.IsNullOrEmpty(GeneralOperateTaylorLift.ProductDescription) ? NotRequired : GeneralOperateTaylorLift.ProductDescription;
            TxtServiceType = string.IsNullOrEmpty(GeneralOperateTaylorLift.MoveTypeDesc) ? NotRequired : GeneralOperateTaylorLift.MoveTypeDesc;
            TxtH34 = GeneralOperateTaylorLift.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;

            var start = DateTime.ParseExact(GeneralOperateTaylorLift.ServiceStartDate, "O", CultureInfo.InvariantCulture);
            StartTime = start.ToString(DateFormats.StandardHHmmss);
            if (!string.IsNullOrEmpty(GeneralOperateTaylorLift.ServiceFinishDate)) {
                var end = DateTime.ParseExact(GeneralOperateTaylorLift.ServiceFinishDate, "O", CultureInfo.InvariantCulture);    
                var span = end - start;
                ElapsedTime = span.ToString(@"hh\:mm\:ss");
                StopTime = end.ToString(DateFormats.StandardHHmmss);
                success = 0;
                cancel = 1;
            }
            else {
                success = 1;
                cancel = 0;
            }

            ContinueWith = string.Concat(Resource.AppString.lblContinueWith, LblNumber);
            if (_isPerHour)
                LstContinueWith = new List<BasePicker>()
                {
                    new BasePicker() {Code = (int) CodeType.Move, Name = AppString.OptionMove},
                    new BasePicker() {Code = (int) CodeType.Service, Name = AppString.OptionService}
                };
            else
                LstContinueWith = new List<BasePicker>()
                {
                    new BasePicker() {Code = (int) CodeType.Move, Name = AppString.OptionMove},
                    new BasePicker() {Code = (int) CodeType.Service, Name = AppString.OptionService},
                    new BasePicker() {Code = (int) CodeType.Detention, Name = AppString.OptionDetention},
                    new BasePicker() {Code = (int) CodeType.Taylor, Name = AppString.OptionTaylor}
                };
            DriverComments = string.IsNullOrEmpty(GeneralOperateTaylorLift.DriverComments) ? AppString.lblNone : GeneralOperateTaylorLift.DriverComments;
        }

        #region Command
        public ICommand ContinueSame => CreateCommand(() =>
        {
            _isNewElement = false;
            SelectedOptionToContinue(AppString.lblContinueWith);
        });

        public ICommand ContinueNew => CreateCommand(() =>
        {
            _isNewElement = true;
            SelectedOptionToContinue(AppString.lblStartANew);
        });

        async void SelectedOptionToContinue(string title)
        {
            var options = LstContinueWith.Select(x => x.Name).ToArray();
            var selectedOption = await UserDialogs.Instance.ActionSheetAsync(title, AppString.btnDialogCancel, null, cancelToken: null, buttons: options);

            if (selectedOption == AppString.OptionMove)
            {
                await GetMove();
            }
            else if (selectedOption == AppString.OptionService)
            {
                await GetService();
            }
            else if (selectedOption == AppString.OptionDetention)
            {
                await GetDetention();
            }
            else if (selectedOption == AppString.OptionTaylor)
            {
                await GetOperateTaylorLift();
            }
        }

        public ICommand PickerCommand => CreateCommand(() =>
        {
            LstEquipmentSize = _equipmentSizeRepository.FindAll(x => x.EquipmentTypeId == selEquipmentType);

            OnPropertyChanged("LstEquipmentSize");
        });
        public ICommand GoRegisterMoveCommand => CreateCommand(async () =>
        {
            await GetMove();
        });
        public ICommand GoAddServiceCommand => CreateCommand(async () =>
        {
            await GetService();
        });

        public ICommand GoDetentionCommand => CreateCommand(async () =>
        {
            await GetDetention();
        });

        public ICommand GoTaylorLiftCommand => CreateCommand(async () =>
        {
            await GetOperateTaylorLift();
        });

        public ICommand GoMainMenuCommand => CreateCommand(async () =>
        {
            await _navigator.PopToRootAsync();
        });

        #endregion

        #region Task
        private async Task GetMove()
        {
            var generalMove = new BEMove
            {
                InternalId = Guid.NewGuid(),
                CurrentState = MoveState.CreatedMove,
                IsModified = true
            };

            if (!_isNewElement)
            {
                generalMove.ShipmentID = GeneralOperateTaylorLift.ShipmentID;
                generalMove.CostCenter = GeneralOperateTaylorLift.CostCenter;
                generalMove.CostCenterName = GeneralOperateTaylorLift.CostCenterName;

                generalMove.DispatchingParty = GeneralOperateTaylorLift.DispatchingParty;

                generalMove.EquipmentNumber = GeneralOperateTaylorLift.EquipmentNumber;
                generalMove.EquipmentType = GeneralOperateTaylorLift.EquipmentType;
                generalMove.EquipmentSize = GeneralOperateTaylorLift.EquipmentSize;
                generalMove.ChassisNumber = GeneralOperateTaylorLift.ChassisNumber;

                generalMove.EquipmentTestDate25Year = GeneralOperateTaylorLift.EquipmentTestDate25Year;
                generalMove.EquipmentTestDate5Year = GeneralOperateTaylorLift.EquipmentTestDate5Year;

                generalMove.Product = GeneralOperateTaylorLift.Product;
                generalMove.ProductDescription = GeneralOperateTaylorLift.ProductDescription;

                generalMove.Start = GeneralOperateTaylorLift.Start;
                generalMove.StartName = GeneralOperateTaylorLift.StartName;
                generalMove.Finish = string.Empty;
                generalMove.FinishName = string.Empty;
                generalMove.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
                generalMove.Service = MoveCode.DefaultValue.ValuePickerDefault;

                generalMove.ServiceFinishDate = string.Empty;
                generalMove.ServiceFinishDateUTC = string.Empty;
                generalMove.ServiceFinishDateTZ = string.Empty;
                generalMove.ServiceStartDate = string.Empty;
                generalMove.ServiceStartDateUTC = string.Empty;
                generalMove.ServiceStartDateTZ = string.Empty;
            }
            _navigator.ClearNavigationStackToRoot(removePersistentPages: true);
            _moveRepository.Add(generalMove);            
            await _navigator.PushAsync<RegisterMoveViewModel>(x => { x.GeneralMove = generalMove; x.isAnotherMove = true; });
            _navigator.ClearNavigationStackToRoot();
        }

        private async Task GetService()
        {
            var generalService = new BEService
            {
                InternalId = Guid.NewGuid(),
                CurrentState = MoveState.CreatedMove,
                IsModified = true
            };

            if (!_isNewElement)
            {
                generalService.ShipmentID = GeneralOperateTaylorLift.ShipmentID;
                generalService.CostCenter = GeneralOperateTaylorLift.CostCenter;
                generalService.CostCenterName = GeneralOperateTaylorLift.CostCenterName;

                generalService.DispatchingParty = GeneralOperateTaylorLift.DispatchingParty;

                generalService.EquipmentNumber = GeneralOperateTaylorLift.EquipmentNumber;
                generalService.EquipmentType = GeneralOperateTaylorLift.EquipmentType;
                generalService.EquipmentSize = GeneralOperateTaylorLift.EquipmentSize;
                generalService.ChassisNumber = GeneralOperateTaylorLift.ChassisNumber;

                generalService.EquipmentTestDate25Year = GeneralOperateTaylorLift.EquipmentTestDate25Year;
                generalService.EquipmentTestDate5Year = GeneralOperateTaylorLift.EquipmentTestDate5Year;

                generalService.Product = GeneralOperateTaylorLift.Product;
                generalService.ProductDescription = GeneralOperateTaylorLift.ProductDescription;

                generalService.Start = GeneralOperateTaylorLift.Start;
                generalService.StartName = GeneralOperateTaylorLift.StartName;
                generalService.Finish = string.Empty;
                generalService.FinishName = string.Empty;
                generalService.EquipmentStatus = 0;
                generalService.Service = 0;

                generalService.ServiceFinishDate = string.Empty;
                generalService.ServiceFinishDateUTC = string.Empty;
                generalService.ServiceFinishDateTZ = string.Empty;
                generalService.ServiceStartDate = string.Empty;
                generalService.ServiceStartDateUTC = string.Empty;
                generalService.ServiceStartDateTZ = string.Empty;
            }
            _navigator.ClearNavigationStackToRoot(removePersistentPages: true);
            _serviceRepository.Add(generalService);            
            await _navigator.PushAsync<RegisterAdditionalServiceViewModel>(x => { x.GeneralService = generalService; x.isAnotherService = true; });
            _navigator.ClearNavigationStackToRoot();
        }

        private async Task GetDetention()
        {
            var generalDetention = new BEDetention
            {
                InternalId = Guid.NewGuid(),
                CurrentState = MoveState.CreatedMove,
                IsModified = true
            };
            if (!_isNewElement)
            {
                generalDetention.ShipmentID = GeneralOperateTaylorLift.ShipmentID;
                generalDetention.CostCenter = GeneralOperateTaylorLift.CostCenter;
                generalDetention.CostCenterName = GeneralOperateTaylorLift.CostCenterName;

                generalDetention.DispatchingParty = GeneralOperateTaylorLift.DispatchingParty;

                generalDetention.EquipmentNumber = GeneralOperateTaylorLift.EquipmentNumber;
                generalDetention.EquipmentType = GeneralOperateTaylorLift.EquipmentType;
                generalDetention.EquipmentSize = GeneralOperateTaylorLift.EquipmentSize;
                generalDetention.ChassisNumber = GeneralOperateTaylorLift.ChassisNumber;

                generalDetention.EquipmentTestDate25Year = GeneralOperateTaylorLift.EquipmentTestDate25Year;
                generalDetention.EquipmentTestDate5Year = GeneralOperateTaylorLift.EquipmentTestDate5Year;

                generalDetention.Product = GeneralOperateTaylorLift.Product;
                generalDetention.ProductDescription = GeneralOperateTaylorLift.ProductDescription;

                generalDetention.Start = GeneralOperateTaylorLift.Start;
                generalDetention.StartName = GeneralOperateTaylorLift.StartName;
                generalDetention.Finish = string.Empty;
                generalDetention.FinishName = string.Empty;
                generalDetention.EquipmentStatus = 0;
                generalDetention.Service = 0;

                generalDetention.ServiceFinishDate = string.Empty;
                generalDetention.ServiceFinishDateUTC = string.Empty;
                generalDetention.ServiceFinishDateTZ = string.Empty;
                generalDetention.ServiceStartDate = string.Empty;
                generalDetention.ServiceStartDateUTC = string.Empty;
                generalDetention.ServiceStartDateTZ = string.Empty;
            }
            _navigator.ClearNavigationStackToRoot(removePersistentPages: true);
            _detentionRepository.Add(generalDetention);            
            await _navigator.PushAsync<RegisterDetentionViewModel>(x => { x.GeneralDetention = generalDetention; x.isAnotherDetention = true; });
            _navigator.ClearNavigationStackToRoot();
        }

        private async Task GetOperateTaylorLift()
        {
            GeneralOperateTaylorLift.CurrentState = MoveState.CreatedMove;
            GeneralOperateTaylorLift.FinishName = string.Empty;
            GeneralOperateTaylorLift.Finish = string.Empty;

            GeneralOperateTaylorLift.DriverComments = string.Empty;
            GeneralOperateTaylorLift.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralOperateTaylorLift.Service = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralOperateTaylorLift.HasH34 = false;

            if (_isNewElement)
            {
                GeneralOperateTaylorLift = new BEOperateTaylorLift
                {
                    CurrentState = MoveState.CreatedMove,
                    IsModified = true
                };
            }
            GeneralOperateTaylorLift.InternalId = Guid.NewGuid();
            GeneralOperateTaylorLift.ServiceFinishDate = string.Empty;
            GeneralOperateTaylorLift.ServiceFinishDateUTC = string.Empty;
            GeneralOperateTaylorLift.ServiceFinishDateTZ = string.Empty;
            GeneralOperateTaylorLift.ServiceStartDate = string.Empty;
            GeneralOperateTaylorLift.ServiceStartDateUTC = string.Empty;
            GeneralOperateTaylorLift.ServiceStartDateTZ = string.Empty;

            _navigator.ClearNavigationStackToRoot(removePersistentPages: true);
            _operateTaylorLiftRepository.Add(GeneralOperateTaylorLift);            
            await _navigator.PushAsync<RegisterOperateTaylorLiftViewModel>(x => { x.GeneralOperateTaylorLift = GeneralOperateTaylorLift; x.isAnotherOperateTaylorLift = true; });
            _navigator.ClearNavigationStackToRoot();
        }
        private void ContinueChargeWith()
        {
            var continueWith = _isNewElement ? SelSetNewOne : SelContinueWith;
            switch (continueWith)
            {
                case (int)CodeType.Move:
                    GoRegisterMoveCommand.Execute(0);
                    break;
                case (int)CodeType.Service:
                    GoAddServiceCommand.Execute(0);
                    break;
                case (int)CodeType.Detention:
                    GoDetentionCommand.Execute(0);
                    break;
                case (int)CodeType.Taylor:
                    GoTaylorLiftCommand.Execute(0);
                    break;
            }
        }
        #endregion

        #region Variables   

        private BEOperateTaylorLift _generalOperateTaylorLift;
        public BEOperateTaylorLift GeneralOperateTaylorLift
        {
            get { return _generalOperateTaylorLift; }
            set { SetProperty(ref _generalOperateTaylorLift, value); }
        }
        public IEnumerable<EquipmentSizeDTO> LstEquipmentSize { get; set; }
        private int _equipmentSize;
        public int selEquipmentSize
        {
            get { return _equipmentSize; }
            set
            {
                SetProperty(ref _equipmentSize, value);
                OnPropertyChanged("selEquipmentSize");
            }
        }
        private string _driverComments;
        public string DriverComments
        {
            get { return _driverComments; }
            set { SetProperty(ref _driverComments, value); }
        }
        private int _equipmentType;
        public int selEquipmentType
        {
            get { return _equipmentType; }
            set
            {
                var equ = _equipmentType;
                SetProperty(ref _equipmentType, value);
                if (value != 0 && equ != value) PickerCommand.Execute(0);
            }
        }
        public IEnumerable<ServiceDTO> LstService { get; set; }
        private int _service;
        public int selService
        {
            get { return _service; }
            set
            {
                SetProperty(ref _service, value);
            }
        }
        public IEnumerable<EquipmentStatusDTO> LstEquipmentStatus { get; set; }
        private int _equipmentStatus;
        public int selEquipmentStatus
        {
            get { return _equipmentStatus; }
            set
            {
                SetProperty(ref _equipmentStatus, value);

            }
        }

        private string _lblId;
        public string LblId
        {
            get { return _lblId; }
            set { SetProperty(ref _lblId, value); }
        }
        private string _lblNumber;
        public string LblNumber
        {
            get { return _lblNumber; }
            set { SetProperty(ref _lblNumber, value); }
        }

        private string _txtServiceType;
        public string TxtServiceType
        {
            get { return _txtServiceType; }
            set { SetProperty(ref _txtServiceType, value); }
        }

        private string _txtBlock;
        public string TxtBlock
        {
            get { return _txtBlock; }
            set { SetProperty(ref _txtBlock, value); }
        }

        private string _startTime;
        public string StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }

        private string _stopTime;
        public string StopTime
        {
            get { return _stopTime; }
            set { SetProperty(ref _stopTime, value); }
        }

        private string _elapsedTime;
        public string ElapsedTime
        {
            get { return _elapsedTime; }
            set { SetProperty(ref _elapsedTime, value); }
        }

        private string _txtEquipmentNumber;
        public string TxtEquipmentNumber
        {
            get { return _txtEquipmentNumber; }
            set { SetProperty(ref _txtEquipmentNumber, value); }
        }
        private string _txtEquipmentType;
        public string TxtEquipmentType
        {
            get { return _txtEquipmentType; }
            set { SetProperty(ref _txtEquipmentType, value); }
        }

        private string _txtEquipmentSize;
        public string TxtEquipmentSize
        {
            get { return _txtEquipmentSize; }
            set { SetProperty(ref _txtEquipmentSize, value); }
        }

        private string _txtEquipmentStatus;
        public string TxtEquipmentStatus
        {
            get { return _txtEquipmentStatus; }
            set { SetProperty(ref _txtEquipmentStatus, value); }
        }

        private string _txtChassisNumber;
        public string TxtChassisNumber
        {
            get { return _txtChassisNumber; }
            set { SetProperty(ref _txtChassisNumber, value); }
        }
        private string _txtEquipmentProduct;
        public string TxtEquipmentProduct
        {
            get { return _txtEquipmentProduct; }
            set { SetProperty(ref _txtEquipmentProduct, value); }
        }

        private string _txtH34;
        public string TxtH34
        {
            get { return _txtH34; }
            set { SetProperty(ref _txtH34, value); }
        }
        private string _continueWith;
        public string ContinueWith
        {
            get { return _continueWith; }
            set { SetProperty(ref _continueWith, value); }
        }
        private int _selContinueWith;
        public int SelContinueWith
        {
            get { return _selContinueWith; }
            set
            {
                SetProperty(ref _selContinueWith, value);
                _isNewElement = false;
                ContinueChargeWith();
            }
        }

        private int _cancel;
        public int cancel
        {
            get { return _cancel; }
            set { SetProperty(ref _cancel, value); }
        }

        private int _success;
        public int success
        {
            get { return _success; }
            set { SetProperty(ref _success, value); }
        }

        private IEnumerable<BasePicker> _lstContinueWith;
        public IEnumerable<BasePicker> LstContinueWith
        {
            get { return _lstContinueWith; }
            set { SetProperty(ref _lstContinueWith, value); }
        }
        private int _selSetNewOne;
        public int SelSetNewOne
        {
            get { return _selSetNewOne; }
            set
            {
                SetProperty(ref _selSetNewOne, value);
                _isNewElement = true;
                ContinueChargeWith();
            }
        }
        private enum CodeType : int
        {
            Move = 1, Service, Detention, Taylor
        }
        #endregion
    }
}
