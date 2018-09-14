using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XPO.ShuttleTracking.Application.DTOs.Requests.Tracking;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Domain.Util;
using XPO.ShuttleTracking.Mobile.Entity;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Helpers.Util;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Infrastructure.CustomException;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.ViewModel.SearchItem;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class RegisterMoveViewModel : TodayViewModel
    {
        private readonly INavigator _navigator;
        private readonly IMoveRepository _moveRepository;
        private string _serviceTypeCode = string.Empty;
        private int CommentMaxLenght = 50;
        //drop down menu
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IEquipmentSizeRepository _equipmentSizeRepository;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IEquipmentStatusRepository _equipmentStatusRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IDispatchingPartyRepository _dispatchingPartyRepository;
        public RegisterMoveViewModel(INavigator navigator,
            IMoveRepository moveRepository,
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
            _dispatchingPartyRepository = dispatchingPartyRepository;
        }

        public override void OnPushed()
        {
            base.OnPushed();
            Title = AppString.titleRegisterMove;
            LoadComponents();
        }
        #region DataLoad
        private void UpdateFromBlock()
        {
            if (GeneralMove.CurrentState <= MoveState.CreatedMove && !isAnotherMove) return;
            SelectedTab = string.IsNullOrEmpty(GeneralMove.CostCenterName) ? 0 : 1;

            selEquipmentType = GeneralMove.EquipmentType;
            selEquipmentSize = GeneralMove.EquipmentSize;
            selDispatcher = GeneralMove.DispatchingParty;
            selService = GeneralMove.Service;
            selEquipmentStatus = GeneralMove.EquipmentStatus;
            isAnotherMove = false;
        }
        private void UpdateScreen()
        {
            GeneralMove = _moveRepository.FindByKey(GeneralMove.InternalId);

            TxtDp25yrTestDate = string.IsNullOrEmpty(GeneralMove.EquipmentTestDate25Year) ? string.Empty : GeneralMove.EquipmentTestDate25Year;
            TxtDp5yrTestDate = string.IsNullOrEmpty(GeneralMove.EquipmentTestDate5Year) ? string.Empty : GeneralMove.EquipmentTestDate5Year;
            
            ProductSeachImage = string.IsNullOrEmpty(GeneralMove.ProductDescription) ? 
                MoveCode.SearchIcon : MoveCode.DeleteIcon;

            FromBlockSeachImage = string.IsNullOrEmpty(GeneralMove.StartName) ? 
                MoveCode.SearchIcon : MoveCode.DeleteIcon;

            ToBlockSeachImage = string.IsNullOrEmpty(GeneralMove.FinishName) ? 
                MoveCode.SearchIcon : MoveCode.DeleteIcon;
        }
        private void UpdateScreenAndCopyBlock()
        {
            UpdateScreen();

            if (LstServiceTypeRepository.FirstOrDefault(x => x.ServiceId == selService)?.Code == ServiceCode.Respot)
            {
                GeneralMove.FinishName = GeneralMove.StartName;
                GeneralMove.Finish = GeneralMove.Start;
            }
        }
        private void LoadComponents()
        {
            var session = _sessionRepository.GetSessionObject();

            LoadServiceType(session);            
            LoadDispatcher();            
            LoadEquipment();
            LoadValidations();
            LoadProductBlocks();

            var old = isAnotherMove;
            isAnotherMove = false;
            UpdateFromBlock();
            isAnotherMove = old;

            OnPropertyChanged("LstService");
            OnPropertyChanged("LstDispatcher");
        }

        private void LoadProductBlocks()
        {
            ProductSeachImage = string.IsNullOrEmpty(GeneralMove.ProductDescription)
                ? MoveCode.SearchIcon
                : MoveCode.DeleteIcon;

            FromBlockSeachImage = string.IsNullOrEmpty(GeneralMove.StartName) ? MoveCode.SearchIcon : MoveCode.DeleteIcon;

            ToBlockSeachImage = string.IsNullOrEmpty(GeneralMove.FinishName) ? MoveCode.SearchIcon : MoveCode.DeleteIcon;
        }

        private void LoadValidations()
        {
            //Chassis
            var varChassisNoValidFormat = Parameters.Get(ParameterCode.ChassisNoValidFormat);
            ChassisNoValidFormat = string.IsNullOrEmpty(varChassisNoValidFormat)
                ? @"^(?=[a-zA-Z]{3,4}\d{2,7}).{6,10}$"
                : varChassisNoValidFormat;

            var varEqpNoPrefixLength = 0;
            if (!int.TryParse(Parameters.Get(ParameterCode.EqpNoPrefixLength), out varEqpNoPrefixLength))
                varEqpNoPrefixLength = 4;
            EqpNoPrefixLength = varEqpNoPrefixLength;

            var varEqSizeCalcEqChkDigit = Parameters.Get(ParameterCode.EqSizeCalcEqChkDigit);
            EqSizeCalcEqChkDigit = string.IsNullOrEmpty(varEqSizeCalcEqChkDigit)
                ? new List<string>() {"20", "40", "45"}
                : varEqSizeCalcEqChkDigit.Split(',').ToList();

            var varEqSizeValEqChkDigit = Parameters.Get(ParameterCode.EqSizeValEqChkDigit);
            EqSizeValEqChkDigit = string.IsNullOrEmpty(varEqSizeValEqChkDigit)
                ? new List<string>() {"20", "40", "45"}
                : varEqSizeValEqChkDigit.Split(',').ToList();

            var varEqpNoValidFormat = Parameters.Get(ParameterCode.EqpNoValidFormat);
            EqpNoValidFormat = string.IsNullOrEmpty(varEqpNoValidFormat) ? @"^[a-zA-Z]{4}\d{6,7}$" : varEqpNoValidFormat;

            var varEqpNoMaxLength = 0;
            if (!int.TryParse(Parameters.Get(ParameterCode.EqpNoMaxLength), out varEqpNoMaxLength))
                varEqpNoMaxLength = 11;
            EqpNoMaxLength = varEqpNoMaxLength;

            var varEqpNoMinLength = 0;
            if (!int.TryParse((Parameters.Get(ParameterCode.EqpNoMinLength)), out varEqpNoMinLength))
                varEqpNoMinLength = 10;
            EqpNoMinLength = varEqpNoMinLength;

            var varEqSizeTypeDisChassis = Parameters.Get(ParameterCode.EqSizeTypeDisChassis);
            EqSizeTypeDisChassis = string.IsNullOrEmpty(varEqSizeTypeDisChassis)
                ? new List<string>() {"53~DRV"}
                : varEqSizeTypeDisChassis.Split(',').ToList();
        }

        private void LoadEquipment()
        {
            //Equipment Type
            if (LstEquipmentType == null || !LstEquipmentType.Any())
            {
                var lstEqType = _equipmentTypeRepository.GetAllOrdeByName().ToList();
                lstEqType.Insert(0,
                    new EquipmentTypeDTO()
                    {
                        EquipmentTypeId = MoveCode.DefaultValue.ValuePickerDefault,
                        Name = string.Format(AppString.lblDefaultSelection, AppString.lblType)
                    });
                LstEquipmentType = lstEqType;                
            }
            _equipmentType = isAnotherMove ? GeneralMove.EquipmentType : MoveCode.DefaultValue.ValuePickerDefault;
            OnPropertyChanged("selEquipmentType");

            //Equipment Size
            if (LstEquipmentSizeRepository == null || !LstEquipmentSizeRepository.Any())
                LstEquipmentSizeRepository = _equipmentSizeRepository.GetAll().ToList();

            var lstEqSize = LstEquipmentSizeRepository.Where(x => x.EquipmentTypeId == GeneralMove.EquipmentType).OrderBy(x => x.Name).ToList();
            lstEqSize.Insert(0,
                new EquipmentSizeDTO()
                {
                    EquipmentSizeId = MoveCode.DefaultValue.ValuePickerDefault,
                    Name = string.Format(AppString.lblDefaultSelection, AppString.lblSize)
                });

            LstEquipmentSize = lstEqSize;
            
            Task.Run(async () =>
            {
                await Task.Delay(500);
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = isAnotherMove ? GeneralMove.EquipmentSize : MoveCode.DefaultValue.ValuePickerDefault;
                    selEquipmentSize = value;
                });
            }).ConfigureAwait(continueOnCapturedContext:true);
            
            
            //Equipment Status
            var lstEqStatus = _equipmentStatusRepository.GetAll().OrderBy(x => x.Name).ToList();
            lstEqStatus.Insert(0,
                new EquipmentStatusDTO()
                {
                    EquipmentStatusId = MoveCode.DefaultValue.ValuePickerDefault,
                    Name = string.Format(AppString.lblDefaultSelection, AppString.lblStatus)
                });
            LstEquipmentStatus = lstEqStatus;
            selEquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;

            //Equipment Test Date
            if (!string.IsNullOrEmpty(GeneralMove?.EquipmentTestDate5Year)) TxtDp5yrTestDate = GeneralMove.EquipmentTestDate5Year;
            if (!string.IsNullOrEmpty(GeneralMove?.EquipmentTestDate25Year)) TxtDp25yrTestDate = GeneralMove.EquipmentTestDate25Year;
            OnPropertyChanged("LstEquipmentStatus");
        }

        private void LoadDispatcher()
        {
            if (LstDispatcher == null || !LstDispatcher.Any())
            {
                var lstDisp = _dispatchingPartyRepository.GetAll().OrderBy(x => x.Name).ToList();
                lstDisp.Insert(0,
                    new DispatchingPartyDTO()
                    {
                        DispatchingPartyId = MoveCode.DefaultValue.ValuePickerDefault,
                        Name = string.Format(AppString.lblDefaultSelection, AppString.lblDispatching)
                    });
                LstDispatcher = lstDisp;
            }
            _dispatcher = isAnotherMove ? GeneralMove.DispatchingParty : MoveCode.DefaultValue.ValuePickerDefault;
            OnPropertyChanged("selDispatcher");
        }

        private void LoadServiceType(BESession session)
        {
            if (LstServiceTypeRepository == null)
                LstServiceTypeRepository = _serviceTypeRepository.GetAll().ToList();
            if (LstEquipmentTypeRepository == null)
                LstEquipmentTypeRepository = _equipmentTypeRepository.GetAll().ToList();

            var defaultService = new ServiceDTO()
            {
                ServiceId = MoveCode.DefaultValue.ValuePickerDefault,
                Name = string.Format(AppString.lblDefaultSelection, AppString.lblMoveType)
            };
            switch (session.TypeUser)
            {
                case UserTypeCode.PerHour:
                    var lstHour =
                        LstServiceTypeRepository.Where(
                            m => m.ServiceTypeCode.Equals(ServiceTypeCode.Move) && m.IsPerHour.Equals("1"))
                            .OrderBy(x => x.Name)
                            .ToList();
                    lstHour.Insert(0, defaultService);
                    LstService = lstHour;
                    break;
                case UserTypeCode.PerMove:
                    var lstMove =
                        LstServiceTypeRepository.Where(
                            m => m.ServiceTypeCode.Equals(ServiceTypeCode.Move) && m.IsPerMove.Equals("1"))
                            .OrderBy(x => x.Name)
                            .ToList();
                    lstMove.Insert(0, defaultService);
                    LstService = lstMove;
                    break;
            }

            SelectedTab = string.IsNullOrEmpty(GeneralMove.CostCenterName) ? 0 : 1;
            selService = MoveCode.DefaultValue.ValuePickerDefault;
            TxtDp25yrTestDate = string.Empty;
            TxtDp5yrTestDate = string.Empty;
        }

        private async Task getAllEquipment()
        {
            if (!ContainerValidator()) return;
            using (StartLoading(AppString.lblGeneralLoading))
            {
                var equipmentSearchRequestTemp = new EquipmentSearchRequest()
                {
                    Page = 1,
                    PageSize = 10,
                    EquipmentNo = GeneralMove.EquipmentNumber
                };

                //Load Equipment
                var responseEquipment = await GetAllEquipment(equipmentSearchRequestTemp);
                HandleResult(responseEquipment, () =>
                {
                    GeneralMove.EquipmentSize = selEquipmentSize = responseEquipment.Data.Items[0].EquipmentSizeId;
                    GeneralMove.EquipmentType = selEquipmentType = responseEquipment.Data.Items[0].EquipmentTypeId;
                    GeneralMove.EquipmentNumber = responseEquipment.Data.Items[0].EquipmentNo;
                    _moveRepository.Update(GeneralMove);

                    UpdateScreen();
                    UpdateFromBlock();
                    OnPropertyChanged("selEquipmentType");
                    OnPropertyChanged("selEquipmentSize");
                    OnPropertyChanged("GeneralMove.EquipmentNumber");
                });
            }
        }
        private async Task<ResponseDTO<PagedList<EquipmentDTO>>> GetAllEquipment(EquipmentSearchRequest equipmentSearchRequest)
        {
            var request = new EquipmentSearchTaskDefinition {EquipementSearch = equipmentSearchRequest};
            return await TaskManager.Current.ExecuteTaskAsync<ResponseDTO<PagedList<EquipmentDTO>>>(request);
        }
        private async void SaveMoveGeneral()
        {
            try
            {
                GeneralMove.CurrentState = MoveState.SavedMove;
                GeneralMove.EquipmentSize = selEquipmentSize;
                GeneralMove.EquipmentSizeDesc = selEquipmentSize == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _equipmentSizeRepository.GetAll().FirstOrDefault(x => x.EquipmentSizeId == selEquipmentSize).Name;
                GeneralMove.EquipmentType = selEquipmentType;
                GeneralMove.EquipmentTypeDesc = selEquipmentType == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _equipmentTypeRepository.GetAll().FirstOrDefault(x => x.EquipmentTypeId == selEquipmentType).Name;
                GeneralMove.EquipmentStatus = selEquipmentStatus;
                GeneralMove.EquipmentStatusDesc = selEquipmentStatus == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _equipmentStatusRepository.GetAll().FirstOrDefault(x => x.EquipmentStatusId == selEquipmentStatus).Name;
                GeneralMove.DispatchingParty = selDispatcher;
                GeneralMove.DispatchingPartyDesc = selDispatcher == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _dispatchingPartyRepository.GetAll().FirstOrDefault(x => x.DispatchingPartyId == selDispatcher).Name;
                GeneralMove.MoveType = selService;
                GeneralMove.MoveTypeDesc = selService == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _serviceTypeRepository.GetAll().FirstOrDefault(x => x.ServiceId == selService)?.Name ?? string.Empty;
                GeneralMove.Service = selService;
                GeneralMove.ServiceFinishDate = null;
                GeneralMove.ServiceAcknowledgedDateTZ = null;
                if (_txtIdEqType != null)
                {
                    GeneralMove.EquipmentTestDate25Year = !_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode) ? string.Empty : GeneralMove.EquipmentTestDate25Year;
                    GeneralMove.EquipmentTestDate5Year = !_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode) ? string.Empty : GeneralMove.EquipmentTestDate5Year;
                }
                if (string.IsNullOrEmpty(GeneralMove.ShipmentID)) GeneralMove.ShipmentID = string.Empty;
                if (string.IsNullOrEmpty(GeneralMove.CostCenterName)) GeneralMove.CostCenterName = string.Empty;
                _moveRepository.Update(GeneralMove);
            }
            catch (Exception)
            {
                await ShowError(ErrorCode.RegisterMoveSaveGeneral, AppString.lblErrorUnknown);
            }
        }

        #endregion
        #region Navigation
        public ICommand GoStartMove => CreateCommand(async () =>
        {
            if (FieldValidator())
            {
 
                SaveMoveGeneral();
                await _navigator.PushAsync<StartMoveViewModel>(m => m.GeneralMove = GeneralMove);
            }
        });
        public ICommand GoSearchCostCenter => CreateCommand(async () =>
        {
            _moveRepository.Update(GeneralMove);
            await _navigator.PushAsync<CostCenterSearchViewModel>(x => { x.GeneralMove = GeneralMove; x.AfterSelectItem = UpdateScreen; });
        });
        public ICommand GoFromBlock => CreateCommand(async () =>
        {
            if (string.IsNullOrEmpty(GeneralMove.StartName))
            {
                _moveRepository.Update(GeneralMove);
                await _navigator.PushAsync<FromBlockSearchViewModel>(x => { x.GeneralMove = GeneralMove; x.AfterSelectItem = UpdateScreenAndCopyBlock; x.GeneralObjectType = (int)GeneralObject.Object.Move; });
            }
            else
            {
                GeneralMove.StartName = string.Empty;
                FromBlockSeachImage = MoveCode.SearchIcon;
                OnPropertyChanged(nameof(GeneralMove));
            }
        });
        public ICommand GoToBlock => CreateCommand(async () =>
        {
            if (string.IsNullOrEmpty(GeneralMove.FinishName))
            {
                _moveRepository.Update(GeneralMove);
                await _navigator.PushAsync<ToBlockSearchViewModel>(x => { x.GeneralMove = GeneralMove; x.AfterSelectItem = UpdateScreen; ; x.GeneralObjectType = (int)GeneralObject.Object.Move; });
            }
            else
            {
                GeneralMove.FinishName = string.Empty;
                ToBlockSeachImage = MoveCode.SearchIcon;
                OnPropertyChanged(nameof(GeneralMove));
            }
        });
        public ICommand GoToSearchProduct => CreateCommand(async () =>
        {
            if (string.IsNullOrEmpty(GeneralMove.ProductDescription))
            {
                _moveRepository.Update(GeneralMove);
                await _navigator.PushAsync<ProductSearchViewModel>(x => { x.GeneralMove = GeneralMove; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.Move; });
            }
            else
            {
                GeneralMove.ProductDescription = string.Empty;
                GeneralMove.Product = string.Empty;
                ProductSeachImage = MoveCode.SearchIcon;
            }

            OnPropertyChanged("ProductSeachImage");
            OnPropertyChanged("GeneralMove");
        });

        public ICommand PickerCommand => CreateCommand(() =>
        {
            var lstEqSize = LstEquipmentSizeRepository.Where(x => x.EquipmentTypeId == selEquipmentType).OrderBy(x => x.Name).ToList();
            lstEqSize.Insert(0,new EquipmentSizeDTO() { EquipmentSizeId = MoveCode.DefaultValue.ValuePickerDefault, Name = string.Format(AppString.lblDefaultSelection, AppString.lblSize) });
            LstEquipmentSize = lstEqSize;
            OnPropertyChanged("selEquipmentSize");

            ActivityRequired();
        });

        async void ActivityRequired()
        {
            try
            {
                ProductRequired = RequiredFieldValidator.RequiredField(LstServiceTypeRepository,
                    FieldRequirementCode.Product, selService);
                BlockRequired = RequiredFieldValidator.RequiredField(LstServiceTypeRepository,
                    FieldRequirementCode.Block, selService);
                EquipmentRequired =
                    EquipmentNumberRequired =
                        RequiredFieldValidator.RequiredField(LstServiceTypeRepository, FieldRequirementCode.Equipment,
                            selService);

                ChassisRequired = RequiredFieldValidator.RequiredField(LstServiceTypeRepository,FieldRequirementCode.Chassis, selService);

                StatusRequired = RequiredFieldValidator.RequiredField(LstServiceTypeRepository,
                    FieldRequirementCode.EqStatus, selService);

                if (EquipmentRequired != FieldRequirementCode.Value.Hidden)
                    EquipmentTypeRequired();

                EquipmentSectionRequired = (EquipmentRequired == FieldRequirementCode.Value.Hidden &&
                                            StatusRequired == FieldRequirementCode.Value.Hidden &&
                                            ChassisRequired == FieldRequirementCode.Value.Hidden)
                    ? FieldRequirementCode.Value.Hidden
                    : FieldRequirementCode.Value.Required;

                OnPropertyChanged("ProductRequired");
                OnPropertyChanged("BlockRequired");
                OnPropertyChanged("EquipmentRequired");
                OnPropertyChanged("EquipmentNumberRequired");
                OnPropertyChanged("EquipmentSectionRequired");
                OnPropertyChanged("ChassisRequired");
                OnPropertyChanged("StatusRequired");
            }
            catch (Exception)
            {
                await ShowError(ErrorCode.RegisterMoveActivityRequired, AppString.lblErrorUnknown);
            }
        }

        async void EquipmentTypeRequired()
        {
            try
            {
                var _productRequired = RequiredFieldValidator.RequiredField(LstEquipmentTypeRepository, FieldRequirementCode.Product, selEquipmentType);
                var _chassisRequired = RequiredFieldValidator.RequiredField(LstEquipmentTypeRepository, FieldRequirementCode.Chassis, selEquipmentType);
                var _equipmentNumberRequired = RequiredFieldValidator.RequiredField(LstEquipmentTypeRepository, FieldRequirementCode.EquipNumber, selEquipmentType);
                var _statusRequired = RequiredFieldValidator.RequiredField(LstEquipmentTypeRepository, FieldRequirementCode.EqStatus, selEquipmentType);

                //Assign new configuration. If there is no configuration, leave it's previous value
                ProductRequired = _productRequired ?? ProductRequired;
                ChassisRequired = _chassisRequired ?? ChassisRequired;
                EquipmentNumberRequired = _equipmentNumberRequired ?? EquipmentNumberRequired;
                StatusRequired = _statusRequired ?? StatusRequired;
            }
            catch (Exception)
            {
                await ShowError(ErrorCode.RegisterMoveEquipmentTypeRequired, AppString.lblErrorUnknown);
            }
        }
        public ICommand GetEquipment => CreateCommand(async () =>
        {
            try
            {
                await getAllEquipment();
            }
            catch (IdentityException)
            {
                await ShowOkAlert(AppString.lblError, AppString.errorInvalidUsername);
            }
            catch (ConnectivityException)
            {
                await ShowOkAlert(AppString.lblAppName, Infrastructure.Resources.StringResources.ConnectiviyError);
            }
            catch (GpsEnableException)
            {
                await ShowOkAlert(AppString.lblAppName, Infrastructure.Resources.StringResources.GSPError);
            }
            catch (InvalidOperationException ex)
            {
                await ShowOkAlert(AppString.lblAppName, ex.Message);
            }
            catch (Exception e)
            {
                await ShowError(ErrorCode.RegisterMoveGetEquipment, Infrastructure.Resources.StringResources.GenericError);
            }
        });

        public ICommand ServiceTypeSearch => CreateCommand(async () =>
        {
            try
            {
                var sType = LstServiceTypeRepository?.FirstOrDefault(x => x.ServiceId == selService);
                if (sType != null)
                    _serviceTypeCode = sType.Code;

                ActivityRequired();
            }
            catch (Exception)
            {
                await ShowError(ErrorCode.RegisterMoveServiceTypeSearch, Infrastructure.Resources.StringResources.GenericError);
            }
        });
        public ICommand ClearEquipmentCommand => CreateCommand(() =>
        {
            GeneralMove.EquipmentType = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralMove.EquipmentSize = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralMove.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralMove.EquipmentNumber = string.Empty;
            GeneralMove.ChassisNumber = string.Empty;

            selEquipmentType = GeneralMove.EquipmentType;
            selEquipmentSize = GeneralMove.EquipmentSize;
            selEquipmentStatus = GeneralMove.EquipmentStatus;
            clearTestDates();
            //if (!string.IsNullOrEmpty(GeneralMove?.EquipmentTestDate5Year)) TxtDp5yrTestDate = GeneralMove.EquipmentTestDate5Year;
            //if (!string.IsNullOrEmpty(GeneralMove?.EquipmentTestDate25Year)) TxtDp25yrTestDate = GeneralMove.EquipmentTestDate25Year;
            ActivityRequired();
            OnPropertyChanged("GeneralMove");
        });
        public ICommand SettingsCommand => CreateCommand(async () =>
        {
            await _navigator.PushAsync<SettingsViewModel>();
        });
        public ICommand HomeCommand => CreateCommand(async () =>
        {
            if (await ShowYesNoAlert(AppString.lblDialogDiscard))
            {
                await _navigator.PopToRootAsync();
            }
        });
        public ICommand TosCommand => CreateCommand(async () =>
        {
            await _navigator.PushAsync<TermOfServicesViewModel>();
        });
        public ICommand SearchChargeNumberCommand => CreateCommand(async () =>
        {
            await _navigator.PushAsync<ChargeNumberSearchViewModel>();
        });
        public ICommand CommentsCommand => CreateCommand(async () =>
        {
            var result = await PromptText(GeneralMove.DriverComments, string.Format(AppString.lblCommentsMaxLength, CommentMaxLenght), CommentMaxLenght);
            if (!result.Ok) return;
            GeneralMove.DriverComments = result.Text;
            OnPropertyChanged("GeneralMove");
        });

        #endregion
        #region UIValidation
        private bool FieldValidator()
        {
            switch (SelectedTab)
            {
                case 0:
                    int m = 8;
                    if (string.IsNullOrEmpty(GeneralMove.ShipmentID)) return false;
                    var result = new UtilMove().isShipmentIdValid(GeneralMove.ShipmentID);
                    if (!result) return result;
                    GeneralMove.CostCenterName = string.Empty;
                    GeneralMove.CostCenter = 0;
                    break;
                case 1:
                    if (string.IsNullOrEmpty(GeneralMove.CostCenterName)) return false;
                    GeneralMove.ShipmentID = string.Empty;
                    break;
            }
            if (selService == MoveCode.DefaultValue.ValuePickerDefault) return false;
 
            //Location Validation
            if (BlockRequired == FieldRequirementCode.Value.Required)
            {
                if (string.IsNullOrEmpty(GeneralMove.StartName)) return false;
                if (string.IsNullOrEmpty(GeneralMove.FinishName)) return false;
            }
            else if (BlockRequired == FieldRequirementCode.Value.Hidden)
            {
                GeneralMove.StartName = string.Empty;
                GeneralMove.Start = string.Empty;
                GeneralMove.FinishName = string.Empty;
                GeneralMove.Finish = string.Empty;
            }
            if (selDispatcher == MoveCode.DefaultValue.ValuePickerDefault) return false;

            //Product Validation
            if (ProductRequired == FieldRequirementCode.Value.Required)
            {
                  if (string.IsNullOrEmpty(GeneralMove.Product)) return false;
            }
            else if (ProductRequired == FieldRequirementCode.Value.Hidden)
            {
                GeneralMove.Product = string.Empty;
                GeneralMove.ProductDescription = string.Empty;
            }
            //Equipment Validation
            if (EquipmentRequired == FieldRequirementCode.Value.Required)
            {
                if (selEquipmentType == MoveCode.DefaultValue.ValuePickerDefault) return false;
                if (selEquipmentSize == MoveCode.DefaultValue.ValuePickerDefault || string.IsNullOrEmpty(TxtIdEqSize)) return false;
            }
            else if (EquipmentRequired == FieldRequirementCode.Value.Hidden)
            {
                selEquipmentType = MoveCode.DefaultValue.ValuePickerDefault;
                selEquipmentSize = MoveCode.DefaultValue.ValuePickerDefault;
                if(!string.IsNullOrEmpty(_txtIdEqType))
                if (_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode))
                {
                    TxtDp25yrTestDate = string.Empty;
                    TxtDp5yrTestDate = string.Empty;
                }
            }
            //Equipment Validation
            if (EquipmentNumberRequired == FieldRequirementCode.Value.Required)
            {
                if (!ContainerValidator()) return false;
            }else if (EquipmentNumberRequired == FieldRequirementCode.Value.Hidden) GeneralMove.EquipmentNumber = string.Empty;

            //Status Validation
            if (StatusRequired == FieldRequirementCode.Value.Required)
                if (selEquipmentStatus == MoveCode.DefaultValue.ValuePickerDefault) return false;
            else if (StatusRequired == FieldRequirementCode.Value.Hidden) selEquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
            //Chassis Validation
            if (ChassisRequired == FieldRequirementCode.Value.Required)
            {
                var chassisValidatorParams = new ChassisValidatorParams()
                {
                    ChassisNoValidFormat = ChassisNoValidFormat,
                    Size = TxtIdEqSize,
                    Type = TxtIdEqType,
                    chassisValue = GeneralMove.ChassisNumber,
                    EqSizeTypeDisChassis = EqSizeTypeDisChassis
                };
                switch (ContainerChassisValidator.ChassisValidation(chassisValidatorParams))
                {
                    case ContainerChassisValidator.ChassisValidationCode.NoChassisValue:
                    case ContainerChassisValidator.ChassisValidationCode.WrongFormat:
                        return false;
                }
            }else if (ChassisRequired == FieldRequirementCode.Value.Hidden) GeneralMove.ChassisNumber = string.Empty;

            //Equipment Validation
            if (EquipmentRequired == FieldRequirementCode.Value.Required)
            {
                if (_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode))
                {
                    if (!DateValidator(TxtDp25yrTestDate, 0)) return false;
                    if (!DateValidator(TxtDp5yrTestDate, 1)) return false;
                }
            }

            if (string.IsNullOrWhiteSpace(GeneralMove.Bobtail) && _serviceTypeCode.Equals(MoveCode.MoveType.BobTailCode)) return false;

            
            return true;
        }


        private bool ContainerValidator()
        {
            var containerResult = string.Empty;
            if (selEquipmentType == MoveCode.DefaultValue.ValuePickerDefault) return false;
            if (selEquipmentSize == MoveCode.DefaultValue.ValuePickerDefault || string.IsNullOrEmpty(TxtIdEqSize)) return false;

            var containerValidatorParams = new ContainerValidatorParams()
            {
                ContainerValue = GeneralMove.EquipmentNumber,
                Size = TxtIdEqSize,
                Type = TxtIdEqType,

                EqpNoPrefixLength = EqpNoPrefixLength,
                EqSizeCalcEqChkDigit = EqSizeCalcEqChkDigit,
                EqSizeValEqChkDigit = EqSizeValEqChkDigit,
                EqpNoMaxLength = EqpNoMaxLength,
                EqpNoMinLength = EqpNoMinLength,
                EqpNoValidFormat = EqpNoValidFormat,
            };


            switch (ContainerChassisValidator.ContainerValidation(containerValidatorParams, out containerResult))
            {
                case ContainerChassisValidator.ContainerValidationCode.AppendCheckDigit:
                    GeneralMove.EquipmentNumber = containerResult;
                    break;
                case ContainerChassisValidator.ContainerValidationCode.NoContainerValue:
                case ContainerChassisValidator.ContainerValidationCode.IncorrectCheckDigit:
                case ContainerChassisValidator.ContainerValidationCode.InvalidLength:
                case ContainerChassisValidator.ContainerValidationCode.WrongFormat:
                    OnPropertyChanged("GeneralMove");
                    return false;
            }
            OnPropertyChanged("GeneralMove");
            return true;
        }
        private bool DateValidator(string isCorrect, int showLabel)
        {
            var response = false;
            if (Domain.Util.DateValidator.FormatDate(isCorrect) == 0)
            {
                switch (showLabel)
                {
                    case 0:
                        GeneralMove.EquipmentTestDate25Year = isCorrect;
                        break;
                    case 1:
                        GeneralMove.EquipmentTestDate5Year = isCorrect;
                        break;
                }
                _moveRepository.Update(GeneralMove);
                response = true;
            }
            return response;
        }

        public void clearTestDates()
        {
            GeneralMove.EquipmentTestDate5Year = string.Empty;
            GeneralMove.EquipmentTestDate25Year = string.Empty;
            TxtDp5yrTestDate = string.Empty;
            TxtDp25yrTestDate = string.Empty;
        }
        #endregion
        #region VarZone
        private BEMove _generalMove;
        public BEMove GeneralMove
        {
            get { return _generalMove; }
            set { SetProperty(ref _generalMove, value); }
        }
        public IEnumerable<DispatchingPartyDTO> LstDispatcher { get; set; }
        private int _dispatcher;
        public int selDispatcher
        {
            get { return _dispatcher; }
            set { SetProperty(ref _dispatcher, value); }
        }

        public IEnumerable<EquipmentSizeDTO> LstEquipmentSize
        {
            get { return _lstEquipmentSize; }
            set { SetProperty(ref _lstEquipmentSize,value); }
        }

        private int _equipmentSize;
        public int selEquipmentSize
        {
            get { return _equipmentSize; }
            set
            {
                if (value == _equipmentSize) return;
                SetProperty(ref _equipmentSize, value);
            }
        }

        public IEnumerable<EquipmentTypeDTO> LstEquipmentType
        {
            get { return _lstEquipmentType; }
            set { SetProperty(ref _lstEquipmentType,value); }
        }

        private int _equipmentType;
        public int selEquipmentType
        {
            get { return _equipmentType; }
            set
            {
                if (value == _equipmentType) return;
                SetProperty(ref _equipmentType, value);
                PickerCommand.Execute(0);
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
                ServiceTypeSearch.Execute(0);
            }
        }
        public IEnumerable<EquipmentStatusDTO> LstEquipmentStatus { get; set; }
        private int _equipmentStatus;
        public int selEquipmentStatus
        {
            get { return _equipmentStatus; }
            set { SetProperty(ref _equipmentStatus, value); }
        }
        private string _searchProduct;
        public string SearchProduct
        {
            get { return _searchProduct; }
            set { SetProperty(ref _searchProduct, value); }
        }
        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            { SetProperty(ref _selectedTab, value); }
        }
        private int _selectedType;
        public int SelectedType
        {
            get { return _selectedType; }
            set { SetProperty(ref _selectedType, value); }
        }
        private string _txtDp25yrTestDate;
        public string TxtDp25yrTestDate
        {
            get { return _txtDp25yrTestDate; }
            set
            {
                if (value?.Length == 11) value = value.Substring(0, 10);
                DateValidator(value, 0);
                SetProperty(ref _txtDp25yrTestDate, value);
                OnPropertyChanged("TxtDp25yrTestDate");
            }
        }
        private string _txtDp5yrTestDate;
        public string TxtDp5yrTestDate
        {
            get { return _txtDp5yrTestDate; }
            set
            {
                if (value?.Length == 11) value = value.Substring(0, 10);
                DateValidator(value, 1);
                SetProperty(ref _txtDp5yrTestDate, value);
                OnPropertyChanged("TxtDp5yrTestDate");
            }
        }
        private string _txtIdEqType;
        public string TxtIdEqType
        {
            get { return _txtIdEqType; }
            set { SetProperty(ref _txtIdEqType, value); }
        }
        private string _txtIdEqSize;
        public string TxtIdEqSize
        {
            get { return _txtIdEqSize; }
            set { SetProperty(ref _txtIdEqSize, value); }
        }
        public bool isAnotherMove { get; set; }
        public string ProductRequired { get; set; }
        public string BlockRequired { get; set; }
        public string EquipmentSectionRequired { get; set; }
        public string EquipmentRequired { get; set; }
        public string EquipmentNumberRequired { get; set; }
        public string ChassisRequired { get; set; }
        public string StatusRequired { get; set; }



        public IList<ServiceDTO> LstServiceTypeRepository { get; set; }
        public IList<EquipmentTypeDTO> LstEquipmentTypeRepository { get; set; }
        public IList<EquipmentSizeDTO> LstEquipmentSizeRepository { get; set; }


        //Container validator
        public string ChassisNoValidFormat { get; set; }
        public int EqpNoPrefixLength { get; set; }
        public List<string> EqSizeCalcEqChkDigit { get; set; }
        public List<string> EqSizeValEqChkDigit { get; set; }
        public int EqpNoMaxLength { get; set; }
        public int EqpNoMinLength { get; set; }
        public string EqpNoValidFormat { get; set; }
        public List<string> EqSizeTypeDisChassis { get; set; }

        private string _productSeachImage;
        public string ProductSeachImage
        {
            get { return _productSeachImage; }
            set { SetProperty(ref _productSeachImage, value); }
        }

        private string _toBlockSeachImage;
        public string ToBlockSeachImage
        {
            get { return _toBlockSeachImage; }
            set { SetProperty(ref _toBlockSeachImage, value); }
        }

        private string _fromBlockSeachImage;
        private IEnumerable<EquipmentSizeDTO> _lstEquipmentSize;
        private IEnumerable<EquipmentTypeDTO> _lstEquipmentType;

        public string FromBlockSeachImage
        {
            get { return _fromBlockSeachImage; }
            set { SetProperty(ref _fromBlockSeachImage, value); }
        }
        #endregion
    }
}
