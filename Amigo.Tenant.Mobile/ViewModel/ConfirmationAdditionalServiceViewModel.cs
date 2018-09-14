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
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
using XPO.ShuttleTracking.Mobile.Entity.Service;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class ConfirmationAdditionalServiceViewModel : TodayViewModel
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
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IEquipmentSizeRepository _equipmentSizeRepository;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IEquipmentStatusRepository _equipmentStatusRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IDispatchingPartyRepository _dispatchingPartyRepository;

        public ConfirmationAdditionalServiceViewModel(INavigator navigator,
            IMoveRepository moveRepository,
            IServiceRepository serviceRepository,          
            IDetentionRepository detentionRepository,
            IOperateTaylorLiftRepository operateTaylorLiftRepository,
            IServiceTypeRepository serviceTypeRepository,
            IEquipmentSizeRepository equipmentSizeRepository,
            IEquipmentTypeRepository equipmentTypeRepository,
            IEquipmentStatusRepository equipmentStatusRepository,
            ISessionRepository sessionRepository,
            IDispatchingPartyRepository dispatchingPartyRepository)
        {
            _navigator = navigator;
            _moveRepository = moveRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _equipmentSizeRepository = equipmentSizeRepository;
            _equipmentTypeRepository = equipmentTypeRepository;
            _equipmentStatusRepository = equipmentStatusRepository;
            _sessionRepository = sessionRepository;
            _serviceRepository = serviceRepository;
            _dispatchingPartyRepository = dispatchingPartyRepository;
            _detentionRepository = detentionRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
        }
        public override void OnPushed()
        {
            base.OnPushed();
            LblId = AppString.lblShipmentId.ToUpper();
            if (!string.IsNullOrEmpty(GeneralService.ShipmentID))
            {
                LblId = AppString.lblShipmentId.ToUpper();
                LblNumber = GeneralService.ShipmentID;
            }
            else if (!string.IsNullOrEmpty(GeneralService.CostCenterName))
            {
                LblId = AppString.lblCostCenter;
                LblNumber = GeneralService.CostCenterName;
            }

            var session = _sessionRepository.GetSessionObject();
            _isPerHour = session.TypeUser == UserTypeCode.PerHour;

            TxtBlock = string.IsNullOrEmpty(GeneralService.StartName) ? NotRequired : GeneralService.StartName;

            TxtChassisNumber = string.IsNullOrEmpty(GeneralService.ChassisNumber) ? NotRequired : GeneralService.ChassisNumber;
            TxtEquipmentNumber = string.IsNullOrEmpty(GeneralService.EquipmentNumber) ? NotRequired : GeneralService.EquipmentNumber;
            TxtEquipmentType = string.IsNullOrEmpty(GeneralService.EquipmentNumber) ? AppString.lblEquipmentType : GeneralService.EquipmentTypeDesc;
            TxtEquipmentSize = string.IsNullOrEmpty(GeneralService.EquipmentSizeDesc) ? NotRequired : GeneralService.EquipmentSizeDesc;
            TxtEquipmentStatus = string.IsNullOrEmpty(GeneralService.EquipmentStatusDesc) ? NotRequired : GeneralService.EquipmentStatusDesc;
            TxtEquipmentProduct = string.IsNullOrEmpty(GeneralService.ProductDescription) ? NotRequired : GeneralService.ProductDescription;
            TxtServiceType = string.IsNullOrEmpty(GeneralService.MoveTypeDesc) ? NotRequired : GeneralService.MoveTypeDesc;
            TxtH34 = GeneralService.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;

            var start = DateTime.ParseExact(GeneralService.ServiceStartDate, "O", CultureInfo.InvariantCulture);
            StartTime = start.ToString(DateFormats.StandardHHmmss);

            if (!string.IsNullOrEmpty(GeneralService.ServiceFinishDate)) {                
                var end = DateTime.ParseExact(GeneralService.ServiceFinishDate, "O", CultureInfo.InvariantCulture);                
                var span = end - start;
                ElapsedTime = span.ToString(@"hh\:mm\:ss");
                StopTime = end.ToString(DateFormats.StandardHHmmss);
                success = 0;
                cancel = 1;
            }else
            {
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
            DriverComments = string.IsNullOrEmpty(GeneralService.DriverComments) ? AppString.lblNone : GeneralService.DriverComments;
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
        public ICommand GoMainMenuCommand => CreateCommand(async () =>
        {
            await _navigator.PopToRootAsync();
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
        #endregion

        #region Task
        private async Task GetMove()
        {

            BEMove GeneralMove = new BEMove();

            GeneralMove.InternalId = Guid.NewGuid();
            GeneralMove.CurrentState = MoveState.CreatedMove;
            GeneralMove.IsModified = true;
            if (!_isNewElement)
            {
                GeneralMove.ShipmentID = GeneralService.ShipmentID;
                GeneralMove.CostCenter = GeneralService.CostCenter;
                GeneralMove.CostCenterName = GeneralService.CostCenterName;

                GeneralMove.DispatchingParty = GeneralService.DispatchingParty;

                GeneralMove.EquipmentNumber = GeneralService.EquipmentNumber;
                GeneralMove.EquipmentType = GeneralService.EquipmentType;
                GeneralMove.EquipmentSize = GeneralService.EquipmentSize;
                GeneralMove.ChassisNumber = GeneralService.ChassisNumber;

                GeneralMove.EquipmentTestDate25Year = GeneralService.EquipmentTestDate25Year;
                GeneralMove.EquipmentTestDate5Year = GeneralService.EquipmentTestDate5Year;

                GeneralMove.Product = GeneralService.Product;
                GeneralMove.ProductDescription = GeneralService.ProductDescription;

                GeneralMove.Start = GeneralService.Start;
                GeneralMove.StartName = GeneralService.StartName;
                GeneralService.HasH34 = false;
                GeneralMove.Finish = string.Empty;
                GeneralMove.FinishName = string.Empty;
                GeneralMove.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
                GeneralMove.Service = MoveCode.DefaultValue.ValuePickerDefault;

                GeneralMove.ServiceFinishDate = string.Empty;
                GeneralMove.ServiceFinishDateUTC = string.Empty;
                GeneralMove.ServiceFinishDateTZ = string.Empty;
                GeneralMove.ServiceStartDate = string.Empty;
                GeneralMove.ServiceStartDateUTC = string.Empty;
                GeneralMove.ServiceStartDateTZ = string.Empty;
            }
            _navigator.ClearNavigationStackToRoot(removePersistentPages: true);
            _moveRepository.Add(GeneralMove);            
             await _navigator.PushAsync<RegisterMoveViewModel>(x => { x.GeneralMove = GeneralMove; x.isAnotherMove = true; });
            _navigator.ClearNavigationStackToRoot();
        }

        private async Task GetService()
        {
            GeneralService.CurrentState = MoveState.CreatedMove;
            GeneralService.FinishName = string.Empty;
            GeneralService.Finish = string.Empty;

            GeneralService.DriverComments = string.Empty;
            GeneralService.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralService.Service = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralService.HasH34 = false;
            if (_isNewElement)
            {
                GeneralService = new BEService
                {
                    CurrentState = MoveState.CreatedMove,
                    IsModified = true
                };
            }
            GeneralService.InternalId = Guid.NewGuid();
            GeneralService.ServiceFinishDate = string.Empty;
            GeneralService.ServiceFinishDateUTC = string.Empty;
            GeneralService.ServiceFinishDateTZ = string.Empty;
            GeneralService.ServiceStartDate = string.Empty;
            GeneralService.ServiceStartDateUTC = string.Empty;
            GeneralService.ServiceStartDateTZ = string.Empty;

            _navigator.ClearNavigationStackToRoot(removePersistentPages: true);
            _serviceRepository.Add(GeneralService);            
            await _navigator.PushAsync<RegisterAdditionalServiceViewModel>(x => { x.GeneralService = GeneralService; x.isAnotherService = true; });
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
                generalDetention.ShipmentID = GeneralService.ShipmentID;
                generalDetention.CostCenter = GeneralService.CostCenter;
                generalDetention.CostCenterName = GeneralService.CostCenterName;

                generalDetention.DispatchingParty = GeneralService.DispatchingParty;

                generalDetention.EquipmentNumber = GeneralService.EquipmentNumber;
                generalDetention.EquipmentType = GeneralService.EquipmentType;
                generalDetention.EquipmentSize = GeneralService.EquipmentSize;
                generalDetention.ChassisNumber = GeneralService.ChassisNumber;

                generalDetention.EquipmentTestDate25Year = GeneralService.EquipmentTestDate25Year;
                generalDetention.EquipmentTestDate5Year = GeneralService.EquipmentTestDate5Year;

                generalDetention.Product = GeneralService.Product;
                generalDetention.ProductDescription = GeneralService.ProductDescription;

                generalDetention.Start = GeneralService.Start;
                generalDetention.StartName = GeneralService.StartName;
                generalDetention.Finish = string.Empty;
                generalDetention.FinishName = string.Empty;
                generalDetention.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
                generalDetention.Service = MoveCode.DefaultValue.ValuePickerDefault;

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
            var generalOperateTaylorLift = new BEOperateTaylorLift
            {
                InternalId = Guid.NewGuid(),
                CurrentState = MoveState.CreatedMove,
                IsModified = true
            };
            if (!_isNewElement)
            {
                generalOperateTaylorLift.ShipmentID = GeneralService.ShipmentID;
                generalOperateTaylorLift.CostCenter = GeneralService.CostCenter;
                generalOperateTaylorLift.CostCenterName = GeneralService.CostCenterName;

                generalOperateTaylorLift.DispatchingParty = GeneralService.DispatchingParty;

                generalOperateTaylorLift.EquipmentNumber = string.Empty;
                generalOperateTaylorLift.EquipmentType = MoveCode.DefaultValue.ValuePickerDefault;
                generalOperateTaylorLift.EquipmentSize = MoveCode.DefaultValue.ValuePickerDefault;
                generalOperateTaylorLift.ChassisNumber = string.Empty;

                generalOperateTaylorLift.EquipmentTestDate25Year = string.Empty;
                generalOperateTaylorLift.EquipmentTestDate5Year = string.Empty;

                generalOperateTaylorLift.Product = string.Empty;
                generalOperateTaylorLift.ProductDescription = string.Empty;

                generalOperateTaylorLift.Start = string.Empty;
                generalOperateTaylorLift.StartName = string.Empty;
                generalOperateTaylorLift.Finish = string.Empty;
                generalOperateTaylorLift.FinishName = string.Empty;
                generalOperateTaylorLift.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
                generalOperateTaylorLift.Service = MoveCode.DefaultValue.ValuePickerDefault;

                generalOperateTaylorLift.ServiceFinishDate = string.Empty;
                generalOperateTaylorLift.ServiceFinishDateUTC = string.Empty;
                generalOperateTaylorLift.ServiceFinishDateTZ = string.Empty;
                generalOperateTaylorLift.ServiceStartDate = string.Empty;
                generalOperateTaylorLift.ServiceStartDateUTC = string.Empty;
                generalOperateTaylorLift.ServiceStartDateTZ = string.Empty;
            }
            _navigator.ClearNavigationStackToRoot(removePersistentPages: true);
            _operateTaylorLiftRepository.Add(generalOperateTaylorLift);            
            await _navigator.PushAsync<RegisterOperateTaylorLiftViewModel>(x => { x.GeneralOperateTaylorLift = generalOperateTaylorLift; x.isAnotherOperateTaylorLift = true; });
            _navigator.ClearNavigationStackToRoot();
        }
        #endregion

        #region Variables   

        private BEService _generalService;
        public BEService GeneralService
        {
            get { return _generalService; }
            set { SetProperty(ref _generalService, value); }
        }
        private BEDetention _generalDetention;
        public BEDetention GeneralDetention
        {
            get { return _generalDetention; }
            set { SetProperty(ref _generalDetention, value); }
        }
        private BEOperateTaylorLift _generalOperateTaylorLift;
        public BEOperateTaylorLift GeneralOperateTaylorLift
        {
            get { return _generalOperateTaylorLift; }
            set { SetProperty(ref _generalOperateTaylorLift, value); }
        }
        public IEnumerable<DispatchingPartyDTO> LstDispatcher { get; set; }
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

        private bool _isH34;
        public bool IsH34
        {
            get { return _isH34; }
            set
            {
                if (_isH34 != value)
                {
                    GeneralService.HasH34 = _isH34;
                    SetProperty(ref _isH34, value);
                }
            }
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

        private int _cancel;
        public int cancel {
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

        private enum CodeType : int
        {
            Move = 1, Service, Detention, Taylor
        }
        #endregion
    }
}
