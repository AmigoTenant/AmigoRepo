using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Entity;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.Entity.Service;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class ConfirmationMoveViewModel : TodayViewModel
    {
        private readonly INavigator _navigator;
        private readonly IMoveRepository _moveRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IDetentionRepository _detentionRepository;
        private readonly IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private bool _isNewElement;
        private bool _isPerHour;
        private static readonly string NotRequired = AppString.lblMsgNotRequired;

        public ConfirmationMoveViewModel(INavigator navigator,
            IMoveRepository moveRepository, 
            IDetentionRepository detentionRepository,
            IOperateTaylorLiftRepository operateTaylorLiftRepository,
            IServiceRepository serviceRepository,
            ISessionRepository sessionRepository,
            IServiceTypeRepository serviceTypeRepository)
        {
            _navigator = navigator;
            _moveRepository = moveRepository;
            _serviceRepository = serviceRepository;
            _sessionRepository = sessionRepository;
            _detentionRepository = detentionRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _serviceTypeRepository = serviceTypeRepository;
        }
        public override void OnPushed()
        {
            base.OnPushed();
            LblId = AppString.lblShipmentId.ToUpper();
            if (!string.IsNullOrEmpty(GeneralMove.ShipmentID))
            {
                LblId = AppString.lblShipmentId.ToUpper();
                LblNumber = GeneralMove.ShipmentID;
            }
            else if (!string.IsNullOrEmpty(GeneralMove.CostCenterName))
            {
                LblId = AppString.lblCostCenter.ToUpper();
                LblNumber = GeneralMove.CostCenterName;
            }

            var session = _sessionRepository.GetSessionObject();
            _isPerHour = session.TypeUser == UserTypeCode.PerHour;

            AuthorizationCode = GeneralMove.AuthorizationCode;
            ToBlock = string.IsNullOrEmpty(GeneralMove.FinishName)? NotRequired : GeneralMove.FinishName;
            FromBlock = string.IsNullOrEmpty(GeneralMove.StartName) ? NotRequired : GeneralMove.StartName;
            ChassisNo = string.IsNullOrEmpty(GeneralMove.ChassisNumber) ? NotRequired : GeneralMove.ChassisNumber;
            EquipmentNumber = string.IsNullOrEmpty(GeneralMove.EquipmentNumber) ? NotRequired : GeneralMove.EquipmentNumber;
            EquipmentTypeDesc = string.IsNullOrEmpty(GeneralMove.EquipmentNumber) ? AppString.lblEquipmentType : GeneralMove.EquipmentTypeDesc;
            EquipmentSizeDesc = string.IsNullOrEmpty(GeneralMove.EquipmentSizeDesc) ? NotRequired : GeneralMove.EquipmentSizeDesc;
            EquipmentStatusDesc = string.IsNullOrEmpty(GeneralMove.EquipmentStatusDesc)? NotRequired : GeneralMove.EquipmentStatusDesc;
            Product = string.IsNullOrEmpty(GeneralMove.ProductDescription) ? NotRequired : GeneralMove.ProductDescription;
            MoveType = string.IsNullOrEmpty(GeneralMove.MoveTypeDesc) ? NotRequired : GeneralMove.MoveTypeDesc;

            BobtailAuthorization = GeneralMove.Bobtail;
            var lstAuthBobTail = _serviceTypeRepository.FindAll(m => m.ServiceTypeCode.Equals(ServiceTypeCode.Move) && m.Code.Equals(ServiceCode.AuthorizedBobtail));

            if (lstAuthBobTail.Any())
                ShowBobtailAuth = GeneralMove.MoveType == lstAuthBobTail.First().ServiceId;
            else
                ShowBobtailAuth = false;

            var start = DateTime.ParseExact(GeneralMove.ServiceStartDate, "O", CultureInfo.InvariantCulture);
            StartTime = start.ToString(DateFormats.StandardHHmmss);
            if (!string.IsNullOrEmpty(GeneralMove.ServiceFinishDate))
            {                
                var end = DateTime.ParseExact(GeneralMove.ServiceFinishDate, "O", CultureInfo.InvariantCulture);               
                var span = end - start;
                ElapsedTime = span.ToString(@"hh\:mm\:ss");
                FinishTime = end.ToString(DateFormats.StandardHHmmss);

                success = 0;
                cancel = 1;
            }
            else {
                success = 1;
                cancel = 0;
            }

            ContinueWith =string.Concat(Resource.AppString.lblContinueWith, LblNumber);
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
            DriverComments = string.IsNullOrEmpty(GeneralMove.DriverComments) ? AppString.lblNone : GeneralMove.DriverComments;
        }

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

        public ICommand GoMainMenuCommand => CreateCommand(async () =>
        {
            await _navigator.PopToRootAsync();
        });
        public ICommand GoRegisterMoveCommand => CreateCommand<BasePicker>(async (valor) =>
        {
            await GetMove();
        });

        public ICommand GoDetentionCommand => CreateCommand(async () =>
        {
            await GetDetention();
        });

        public ICommand GoTaylorLiftCommand => CreateCommand(async () =>
        {
            await GetOperateTaylorLift();
        });
        public ICommand GoAddServiceCommand => CreateCommand(async () =>
        {
            await GetService();
        });

        private async Task GetMove()
        {
            GeneralMove.CurrentState = MoveState.CreatedMove;
            GeneralMove.IsModified = true;
            GeneralMove.Start = GeneralMove.Finish;
            GeneralMove.StartName = GeneralMove.FinishName;

            GeneralMove.Finish = string.Empty;
            GeneralMove.FinishName = string.Empty;
            GeneralMove.DriverComments = string.Empty;

            GeneralMove.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralMove.Service = MoveCode.DefaultValue.ValuePickerDefault;
            if (_isNewElement)
            {
                GeneralMove = new BEMove();
                GeneralMove.CurrentState = MoveState.CreatedMove;
                GeneralMove.IsModified = true;
            }      
            GeneralMove.InternalId = Guid.NewGuid();
            GeneralMove.ServiceFinishDate = string.Empty;
            GeneralMove.ServiceFinishDateUTC = string.Empty;
            GeneralMove.ServiceFinishDateTZ = string.Empty;
            GeneralMove.ServiceStartDate = string.Empty;
            GeneralMove.ServiceStartDateUTC = string.Empty;
            GeneralMove.ServiceStartDateTZ = string.Empty;

            _navigator.ClearNavigationStackToRoot(removePersistentPages: true);
            _moveRepository.Add(GeneralMove);            
            await _navigator.PushAsync<RegisterMoveViewModel>(x => { x.GeneralMove = GeneralMove; x.isAnotherMove = true; });
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
                generalService.ShipmentID = GeneralMove.ShipmentID;
                generalService.CostCenter = GeneralMove.CostCenter;
                generalService.CostCenterName = GeneralMove.CostCenterName;

                generalService.DispatchingParty = GeneralMove.DispatchingParty;

                generalService.EquipmentNumber = GeneralMove.EquipmentNumber;
                generalService.EquipmentType = GeneralMove.EquipmentType;
                generalService.EquipmentSize = GeneralMove.EquipmentSize;
                generalService.ChassisNumber = GeneralMove.ChassisNumber;
            
                generalService.EquipmentTestDate25Year = GeneralMove.EquipmentTestDate25Year;
                generalService.EquipmentTestDate5Year = GeneralMove.EquipmentTestDate5Year;

                generalService.Product = GeneralMove.Product;
                generalService.ProductDescription = GeneralMove.ProductDescription;

                generalService.Start = GeneralMove.Finish;
                generalService.StartName = GeneralMove.FinishName;
                generalService.Finish = string.Empty;
                generalService.FinishName = string.Empty;
                generalService.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
                generalService.Service = MoveCode.DefaultValue.ValuePickerDefault;

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
            GeneralDetention = new BEDetention
            {
                InternalId = Guid.NewGuid(),
                CurrentState = MoveState.CreatedMove,
                IsModified = true
            };

            if (!_isNewElement)
            {
                GeneralDetention.ShipmentID = GeneralMove.ShipmentID;
                GeneralDetention.CostCenter = GeneralMove.CostCenter;
                GeneralDetention.CostCenterName = GeneralMove.CostCenterName;

                GeneralDetention.DispatchingParty = GeneralMove.DispatchingParty;

                GeneralDetention.EquipmentNumber = GeneralMove.EquipmentNumber;
                GeneralDetention.EquipmentType = GeneralMove.EquipmentType;
                GeneralDetention.EquipmentSize = GeneralMove.EquipmentSize;
                GeneralDetention.ChassisNumber = GeneralMove.ChassisNumber;

                GeneralDetention.EquipmentTestDate25Year = GeneralMove.EquipmentTestDate25Year;
                GeneralDetention.EquipmentTestDate5Year = GeneralMove.EquipmentTestDate5Year;

                GeneralDetention.Product = GeneralMove.Product;
                GeneralDetention.ProductDescription = GeneralMove.ProductDescription;

                GeneralDetention.Start = GeneralMove.Finish;
                GeneralDetention.StartName = GeneralMove.FinishName;
                GeneralDetention.Finish = string.Empty;
                GeneralDetention.FinishName = string.Empty;
                GeneralDetention.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
                GeneralDetention.Service = MoveCode.DefaultValue.ValuePickerDefault;

                GeneralDetention.ServiceFinishDate = string.Empty;
                GeneralDetention.ServiceFinishDateUTC = string.Empty;
                GeneralDetention.ServiceFinishDateTZ = string.Empty;
                GeneralDetention.ServiceStartDate = string.Empty;
                GeneralDetention.ServiceStartDateUTC = string.Empty;
                GeneralDetention.ServiceStartDateTZ = string.Empty;
            }

            _navigator.ClearNavigationStackToRoot(removePersistentPages: true);
            _detentionRepository.Add(GeneralDetention);            
            await _navigator.PushAsync<RegisterDetentionViewModel>(x => { x.GeneralDetention = GeneralDetention; x.isAnotherDetention = true; });
            _navigator.ClearNavigationStackToRoot();
        }

        private async Task GetOperateTaylorLift()
        {
            GeneralOperateTaylorLift = new BEOperateTaylorLift
            {
                InternalId = Guid.NewGuid(),
                CurrentState = MoveState.CreatedMove,
                IsModified = true
            };
            if (!_isNewElement)
            {
                GeneralOperateTaylorLift.ShipmentID = GeneralMove.ShipmentID;
                GeneralOperateTaylorLift.CostCenter = GeneralMove.CostCenter;
                GeneralOperateTaylorLift.CostCenterName = GeneralMove.CostCenterName;

                GeneralOperateTaylorLift.DispatchingParty = GeneralMove.DispatchingParty;

                GeneralOperateTaylorLift.EquipmentNumber = string.Empty;
                GeneralOperateTaylorLift.EquipmentType = MoveCode.DefaultValue.ValuePickerDefault;
                GeneralOperateTaylorLift.EquipmentSize = MoveCode.DefaultValue.ValuePickerDefault;
                GeneralOperateTaylorLift.ChassisNumber = string.Empty;

                GeneralOperateTaylorLift.EquipmentTestDate25Year = string.Empty;
                GeneralOperateTaylorLift.EquipmentTestDate5Year = string.Empty;

                GeneralOperateTaylorLift.Product = string.Empty;
                GeneralOperateTaylorLift.ProductDescription = string.Empty;

                GeneralOperateTaylorLift.Start = string.Empty;
                GeneralOperateTaylorLift.StartName = string.Empty;
                GeneralOperateTaylorLift.Finish = string.Empty;
                GeneralOperateTaylorLift.FinishName = string.Empty;
                GeneralOperateTaylorLift.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
                GeneralOperateTaylorLift.Service = MoveCode.DefaultValue.ValuePickerDefault;

                GeneralOperateTaylorLift.ServiceFinishDate = string.Empty;
                GeneralOperateTaylorLift.ServiceFinishDateUTC = string.Empty;
                GeneralOperateTaylorLift.ServiceFinishDateTZ = string.Empty;
                GeneralOperateTaylorLift.ServiceStartDate = string.Empty;
                GeneralOperateTaylorLift.ServiceStartDateUTC = string.Empty;
                GeneralOperateTaylorLift.ServiceStartDateTZ = string.Empty;
            }

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
        private BEMove _generalMove;
        public BEMove GeneralMove
        {
            get { return _generalMove; }
            set { SetProperty(ref _generalMove, value); }
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
        
        private string _equipmentTypeDesc;
        public string EquipmentTypeDesc
        {
            get { return _equipmentTypeDesc; }
            set { SetProperty(ref _equipmentTypeDesc, value); }
        }

        private string _equipmentNumber;
        public string EquipmentNumber
        {
            get { return _equipmentNumber; }
            set { SetProperty(ref _equipmentNumber, value); }
        }

        private string _chassisNo;
        public string ChassisNo
        {
            get { return _chassisNo; }
            set { SetProperty(ref _chassisNo, value); }
        }

        private string _finishTime;
        public string FinishTime
        {
            get { return _finishTime; }
            set { SetProperty(ref _finishTime, value); }
        }

        private string _fromBlock;
        public string FromBlock
        {
            get { return _fromBlock; }
            set { SetProperty(ref _fromBlock, value); }
        }

        private string _toBlock;
        public string ToBlock
        {
            get { return _toBlock; }
            set { SetProperty(ref _toBlock, value); }
        }

        private string _startTime;
        public string StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }

        private string _elapsedTime;
        public string ElapsedTime
        {
            get { return _elapsedTime; }
            set { SetProperty(ref _elapsedTime, value); }
        }

        private string _driverComments;
        public string DriverComments
        {
            get { return _driverComments; }
            set { SetProperty(ref _driverComments, value); }
        }

        private string _product;
        public string Product
        {
            get { return _product; }
            set { SetProperty(ref _product, value); }
        }

        private string _moveType;
        public string MoveType
        {
            get { return _moveType; }
            set { SetProperty(ref _moveType, value); }
        }

        private string _equipmentSizeDesc;
        public string EquipmentSizeDesc
        {
            get { return _equipmentSizeDesc; }
            set { SetProperty(ref _equipmentSizeDesc, value); }
        }

        private string _equipmentStatusDesc;
        public string EquipmentStatusDesc
        {
            get { return _equipmentStatusDesc; }
            set { SetProperty(ref _equipmentStatusDesc, value); }
        }

        private string _authorizationCode;
        public string AuthorizationCode
        {
            get { return _authorizationCode; }
            set
            {
                SetProperty(ref _authorizationCode, value);
            }
        }
        private string _bobtailAuthorization;
        public string BobtailAuthorization
        {
            get { return _bobtailAuthorization; }
            set { SetProperty(ref _bobtailAuthorization, value); }
        }
        private bool _showBobtailAuth;
        public bool ShowBobtailAuth
        {
            get { return _showBobtailAuth; }
            set { SetProperty(ref _showBobtailAuth, value); }
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
            Move = 1,Service,Detention,Taylor
        }

    }
}
