using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using XPO.ShuttleTracking.Application.DTOs.Requests.Tracking;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Domain.Util;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
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
    public class RegisterOperateTaylorLiftViewModel : TodayViewModel
    {
        private readonly INavigator _navigator;
        private readonly IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private int CommentMaxLenght = 50;

        //drop down menu
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IEquipmentSizeRepository _equipmentSizeRepository;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IEquipmentStatusRepository _equipmentStatusRepository;
        private readonly IDispatchingPartyRepository _dispatchingPartyRepository;
        private int _defaultValuePicker = MoveCode.DefaultValue.ValuePickerDefault;

        public RegisterOperateTaylorLiftViewModel(INavigator navigator,
                                                IOperateTaylorLiftRepository operateTaylorLiftRepository,
                                                IServiceTypeRepository serviceTypeRepository,
                                                IEquipmentSizeRepository equipmentSizeRepository,
                                                IEquipmentTypeRepository equipmentTypeRepository,
                                                IEquipmentStatusRepository equipmentStatusRepository,
                                                ISessionRepository sessionRepository,
                                                IDispatchingPartyRepository dispatchingPartyRepository)
        {
            _navigator = navigator;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _equipmentSizeRepository = equipmentSizeRepository;
            _equipmentTypeRepository = equipmentTypeRepository;
            _equipmentStatusRepository = equipmentStatusRepository;
            _dispatchingPartyRepository = dispatchingPartyRepository;
        }

        public override void OnPushed()
        {
            base.OnPushed();
            LoadComponents();
        }

        #region DataLoad
        private void UpdateScreenFromMain()
        {
            if (GeneralOperateTaylorLift.CurrentState > MoveState.CreatedMove || isAnotherOperateTaylorLift)
            {
                SelectedTab = string.IsNullOrEmpty(GeneralOperateTaylorLift.CostCenterName) ? 0 : 1;

                selEquipmentType = GeneralOperateTaylorLift.EquipmentType;
                selEquipmentSize = GeneralOperateTaylorLift.EquipmentSize;
                selDispatcher = GeneralOperateTaylorLift.DispatchingParty;
                selEquipmentStatus = GeneralOperateTaylorLift.EquipmentStatus;

                if (!string.IsNullOrEmpty(GeneralOperateTaylorLift?.EquipmentTestDate5Year)) TxtDp5yrTestDate = GeneralOperateTaylorLift.EquipmentTestDate5Year;
                if (!string.IsNullOrEmpty(GeneralOperateTaylorLift?.EquipmentTestDate25Year)) TxtDp25yrTestDate = GeneralOperateTaylorLift.EquipmentTestDate25Year;

                IsH34 = GeneralOperateTaylorLift.HasH34;

                StartName = string.IsNullOrEmpty(GeneralOperateTaylorLift.StartName) ? string.Empty : GeneralOperateTaylorLift.StartName;
                Product = string.IsNullOrEmpty(GeneralOperateTaylorLift.ProductDescription) ? string.Empty : GeneralOperateTaylorLift.ProductDescription;
                EquipmentNumber = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentNumber) ? string.Empty : GeneralOperateTaylorLift.EquipmentNumber;

                isAnotherOperateTaylorLift = false;
                OnPropertyChanged("GeneralOperateTaylorLift");
            }
        }
        private void UpdateScreen()
        {
            GeneralOperateTaylorLift = _operateTaylorLiftRepository.FindByKey(GeneralOperateTaylorLift.InternalId);
            StartName = GeneralOperateTaylorLift.StartName;
            Product = GeneralOperateTaylorLift.ProductDescription;
            TxtDp25yrTestDate = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentTestDate25Year) ? string.Empty : GeneralOperateTaylorLift.EquipmentTestDate25Year;
            TxtDp5yrTestDate = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentTestDate5Year) ? string.Empty : GeneralOperateTaylorLift.EquipmentTestDate5Year;

            if (string.IsNullOrEmpty(GeneralOperateTaylorLift.ProductDescription))
            ProductSeachImage = MoveCode.SearchIcon;
            else ProductSeachImage = MoveCode.DeleteIcon;

            if (string.IsNullOrEmpty(GeneralOperateTaylorLift.StartName))
                FromBlockSeachImage = MoveCode.SearchIcon;
            else FromBlockSeachImage = MoveCode.DeleteIcon;

            OnPropertyChanged("GeneralOperateTaylorLift");
        }

        private void LoadComponents()
        {
            SelectedTab = string.IsNullOrEmpty(GeneralOperateTaylorLift.CostCenterName) ? 0 : 1;
            TxtDp25yrTestDate = string.Empty;
            TxtDp5yrTestDate = string.Empty;

            //Dispatch
            if (LstDispatcher == null || !LstDispatcher.Any())
            {
                var lstDisp = _dispatchingPartyRepository.GetAll().ToList();
                lstDisp = lstDisp.OrderByDescending(x => x.Name).ToList();
                lstDisp.Add(new DispatchingPartyDTO() { DispatchingPartyId = MoveCode.DefaultValue.ValuePickerDefault, Name = string.Format(AppString.lblDefaultSelection, AppString.lblDispatching) });
                lstDisp.Reverse();
                LstDispatcher = lstDisp;
            }
            var lstEqType = _equipmentTypeRepository.GetAll().OrderBy(x => x.Name).ToList();
            lstEqType.Insert(0, new EquipmentTypeDTO() { EquipmentTypeId = MoveCode.DefaultValue.ValuePickerDefault, Name = string.Format(AppString.lblDefaultSelection, AppString.lblType) });
            LstEquipmentType = lstEqType;

            selEquipmentType = GeneralOperateTaylorLift.EquipmentType;
            var lstEqSize = _equipmentSizeRepository.FindAll(x => x.EquipmentTypeId == selEquipmentType).OrderBy(x => x.Name).ToList();
            lstEqSize.Insert(0, new EquipmentSizeDTO() { EquipmentSizeId = MoveCode.DefaultValue.ValuePickerDefault, Name = string.Format(AppString.lblDefaultSelection, AppString.lblSize) });
            LstEquipmentSize = lstEqSize;
            selEquipmentSize = MoveCode.DefaultValue.ValuePickerDefault;

            var lstEqStatus = _equipmentStatusRepository.GetAll().OrderBy(x => x.Name).ToList();
            lstEqStatus.Insert(0, new EquipmentStatusDTO() { EquipmentStatusId = MoveCode.DefaultValue.ValuePickerDefault, Name = string.Format(AppString.lblDefaultSelection, AppString.lblStatus) });
            LstEquipmentStatus = lstEqStatus;

            var varChassisNoValidFormat = Parameters.Get(ParameterCode.ChassisNoValidFormat);
            ChassisNoValidFormat = string.IsNullOrEmpty(varChassisNoValidFormat) ? @"^(?=[a-zA-Z]{3,4}\d{2,7}).{6,10}$" : varChassisNoValidFormat;

            var varEqpNoPrefixLength = 0;
            if (!int.TryParse((Parameters.Get(ParameterCode.EqpNoPrefixLength)), out varEqpNoPrefixLength))
                varEqpNoPrefixLength = 4;
            EqpNoPrefixLength = varEqpNoPrefixLength;

            var varEqSizeCalcEqChkDigit = Parameters.Get(ParameterCode.EqSizeCalcEqChkDigit);
            EqSizeCalcEqChkDigit = string.IsNullOrEmpty(varEqSizeCalcEqChkDigit) ? new List<string>() { "20", "40", "45" } : varEqSizeCalcEqChkDigit.Split(',').ToList<string>();

            var varEqSizeValEqChkDigit = Parameters.Get(ParameterCode.EqSizeValEqChkDigit);
            EqSizeValEqChkDigit = string.IsNullOrEmpty(varEqSizeValEqChkDigit) ? new List<string>() { "20", "40", "45" } : varEqSizeValEqChkDigit.Split(',').ToList<string>();

            var varEqpNoValidFormat = Parameters.Get(ParameterCode.EqpNoValidFormat);
            EqpNoValidFormat = string.IsNullOrEmpty(varEqpNoValidFormat) ? @"^[a-zA-Z]{4}\d{6,7}$" : varEqpNoValidFormat;

            var varEqpNoMaxLength = 0;
            if (!int.TryParse(Parameters.Get(ParameterCode.EqpNoMaxLength), out varEqpNoMaxLength))
                varEqpNoMaxLength = 11;
            EqpNoMaxLength = varEqpNoMaxLength;

            var varEqpNoMinLength = 0;
            if (!int.TryParse(Parameters.Get(ParameterCode.EqpNoMinLength), out varEqpNoMinLength))
                varEqpNoMinLength = 11;
            EqpNoMinLength = varEqpNoMinLength;

            var varEqSizeTypeDisChassis = Parameters.Get(ParameterCode.EqSizeTypeDisChassis);
            EqSizeTypeDisChassis = string.IsNullOrEmpty(varEqSizeTypeDisChassis) ? new List<string>() { "53~DRV" } : varEqSizeTypeDisChassis.Split(',').ToList<string>();

            if (string.IsNullOrEmpty(GeneralOperateTaylorLift.ProductDescription))
            ProductSeachImage = MoveCode.SearchIcon;
            else ProductSeachImage = MoveCode.DeleteIcon;

            if (string.IsNullOrEmpty(GeneralOperateTaylorLift.StartName))
                FromBlockSeachImage = MoveCode.SearchIcon;
            else FromBlockSeachImage = MoveCode.DeleteIcon;

            OnPropertyChanged("ProductSeachImage");

            //selService = MoveCode.DefaultValue.ValuePickerDefault;
            selDispatcher = MoveCode.DefaultValue.ValuePickerDefault;
            selEquipmentType = MoveCode.DefaultValue.ValuePickerDefault;
            selEquipmentSize = MoveCode.DefaultValue.ValuePickerDefault;
            selEquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;

            UpdateScreenFromMain();

            LstServiceTypeRepository = _serviceTypeRepository.FindAll(x => x.Code == ServiceCode.OperateLift && x.ServiceTypeCode == ServiceTypeCode.OperateTaylorLift).ToList();
            selService = LstServiceTypeRepository.FirstOrDefault(x => x.Code == ServiceCode.OperateLift && x.ServiceTypeCode == ServiceTypeCode.OperateTaylorLift).ServiceId;

            ActivityRequired();

            OnPropertyChanged("selH34");
            OnPropertyChanged("LstService");
            OnPropertyChanged("selService");
            OnPropertyChanged("selDispatcher");
            OnPropertyChanged("LstDispatcher");
            OnPropertyChanged("selEquipmentType");
            OnPropertyChanged("LstEquipmentType");
            OnPropertyChanged("selEquipmentSize");
            OnPropertyChanged("LstEquipmentSize");
            OnPropertyChanged("selEquipmentStatus");
            OnPropertyChanged("LstEquipmentStatus");
            
        }
        private async Task GetAllEquipment()
        {
            if (!ContainerValidator()) return;
            using (StartLoading(AppString.lblGeneralLoading))
            {
                var equipmentSearchRequestTemp = new EquipmentSearchRequest
                {
                    Page = 1,
                    PageSize = 10,
                    EquipmentNo = GeneralOperateTaylorLift.EquipmentNumber
                };
                var responseEquipment = await GetAllEquipment(equipmentSearchRequestTemp);
                HandleResult(responseEquipment, () =>
                {
                    GeneralOperateTaylorLift.EquipmentSize = selEquipmentSize = responseEquipment.Data.Items[0].EquipmentSizeId;//9
                    GeneralOperateTaylorLift.EquipmentType = selEquipmentType = responseEquipment.Data.Items[0].EquipmentTypeId;//4
                    GeneralOperateTaylorLift.EquipmentNumber = responseEquipment.Data.Items[0].EquipmentNo;
                    _operateTaylorLiftRepository.Update(GeneralOperateTaylorLift);
                    UpdateScreen();
                    UpdateScreenFromMain();
                    OnPropertyChanged("selEquipmentType");
                    OnPropertyChanged("selEquipmentSize");
                    OnPropertyChanged("GeneralOperateTaylorLift.EquipmentNumber");

                });
            }
        }
        private async Task<ResponseDTO<PagedList<EquipmentDTO>>> GetAllEquipment(EquipmentSearchRequest equipmentSearchRequest)
        {
            var request = new EquipmentSearchTaskDefinition { EquipementSearch = equipmentSearchRequest };
            return await TaskManager.Current.ExecuteTaskAsync<ResponseDTO<PagedList<EquipmentDTO>>>(request);
        }
        private async void SaveMoveGeneral()
        {
            try
            {
                var service = _serviceTypeRepository.FindAll(m => m.ServiceTypeCode.Equals(MoveCode.MoveType.OperateTaylorLiftCode)).First();
                GeneralOperateTaylorLift.CurrentState = MoveState.SavedMove;
                GeneralOperateTaylorLift.EquipmentSize = selEquipmentSize;
                GeneralOperateTaylorLift.EquipmentSizeDesc = selEquipmentSize == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _equipmentSizeRepository.GetAll().First(x => x.EquipmentSizeId == selEquipmentSize).Name;
                GeneralOperateTaylorLift.EquipmentType = selEquipmentType;
                GeneralOperateTaylorLift.EquipmentTypeDesc = selEquipmentType == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _equipmentTypeRepository.GetAll().First(x => x.EquipmentTypeId == selEquipmentType).Name;
                GeneralOperateTaylorLift.EquipmentStatus = selEquipmentStatus;
                GeneralOperateTaylorLift.EquipmentStatusDesc = selEquipmentStatus == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _equipmentStatusRepository.GetAll().First(x => x.EquipmentStatusId == selEquipmentStatus).Name;
                GeneralOperateTaylorLift.DispatchingParty = selDispatcher;
                GeneralOperateTaylorLift.DispatchingPartyDesc = selDispatcher == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _dispatchingPartyRepository.GetAll().FirstOrDefault(x => x.DispatchingPartyId == selDispatcher).Name;
                GeneralOperateTaylorLift.Service = service.ServiceId;
                GeneralOperateTaylorLift.MoveType = service.ServiceId;
                GeneralOperateTaylorLift.MoveTypeDesc = service.Name;
                GeneralOperateTaylorLift.ServiceFinishDate = null;
                GeneralOperateTaylorLift.ServiceAcknowledgedDateTZ = null;
                if (_txtIdEqType != null)
                {
                    GeneralOperateTaylorLift.EquipmentTestDate25Year = !_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode) ? string.Empty: GeneralOperateTaylorLift.EquipmentTestDate25Year;
                    GeneralOperateTaylorLift.EquipmentTestDate5Year = !_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode)? string.Empty: GeneralOperateTaylorLift.EquipmentTestDate5Year;
                }
                if (string.IsNullOrEmpty(GeneralOperateTaylorLift.ShipmentID)) GeneralOperateTaylorLift.ShipmentID = string.Empty;
                if (string.IsNullOrEmpty(GeneralOperateTaylorLift.CostCenterName)) GeneralOperateTaylorLift.CostCenterName = string.Empty;
                _operateTaylorLiftRepository.Update(GeneralOperateTaylorLift);
            }
            catch (Exception)
            {
                await ShowError(ErrorCode.RegisterLiftSaveGeneral, AppString.lblErrorUnknown);
            }
            
        }
        #endregion

        #region Navigation
        public ICommand GoStartOperateTaylorLift => CreateCommand(async () => {
        
            if (FieldValidator())
            {
                SaveMoveGeneral();
                await _navigator.PushAsync<StartOperateTaylorLiftViewModel>(m => m.GeneralOperateTaylorLift = GeneralOperateTaylorLift);
            }
        });

        public ICommand GoSearchCostCenter => CreateCommand(async () =>
        {
            SavePreviusData();
            await _navigator.PushAsync<CostCenterSearchViewModel>((x) => { x.GeneralOperateTaylorLift = GeneralOperateTaylorLift; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.OperateTaylorLift; });
        });
        public ICommand GoFromBlock => CreateCommand(async () =>
        {
            if (string.IsNullOrEmpty(GeneralOperateTaylorLift.StartName))
            {
                SavePreviusData();
                await _navigator.PushAsync<FromBlockSearchViewModel>(x => { x.GeneralOperateTaylorLift = GeneralOperateTaylorLift; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.OperateTaylorLift; });
            }
            else
            {
                GeneralOperateTaylorLift.StartName = string.Empty;
                StartName = string.Empty;
                FromBlockSeachImage = MoveCode.SearchIcon;
            }

            OnPropertyChanged("GeneralOperateTaylorLift");
        });
        public ICommand GoToBlock => CreateCommand(async () =>
        {
            if (string.IsNullOrEmpty(GeneralOperateTaylorLift.FinishName))
            {
                SavePreviusData();
                await _navigator.PushAsync<ToBlockSearchViewModel>(x => { x.GeneralOperateTaylorLift = GeneralOperateTaylorLift; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.OperateTaylorLift; });
            }
            else
            {
                GeneralOperateTaylorLift.FinishName = string.Empty;
                FromBlockSeachImage = MoveCode.SearchIcon;
            }

            OnPropertyChanged("GeneralOperateTaylorLift");
        });
        public ICommand GoToSearchProduct => CreateCommand(async () =>
        {
            if (string.IsNullOrEmpty(GeneralOperateTaylorLift.ProductDescription))
            {
                SavePreviusData();
                await _navigator.PushAsync<ProductSearchViewModel>(x => { x.GeneralOperateTaylorLift = GeneralOperateTaylorLift; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.OperateTaylorLift; });
            }
            else
            {
                GeneralOperateTaylorLift.ProductDescription = string.Empty;
                GeneralOperateTaylorLift.Product = string.Empty;
                Product = string.Empty;
                ProductSeachImage = MoveCode.SearchIcon;
            }

            OnPropertyChanged("ProductSeachImage");
            OnPropertyChanged("GeneralOperateTaylorLift");


        });
        private void SavePreviusData()
        {
            _operateTaylorLiftRepository.Update(GeneralOperateTaylorLift);
        }
        public ICommand PickerCommand => CreateCommand(() =>
        {
            var lstEqSize = _equipmentSizeRepository.FindAll(x => x.EquipmentTypeId == selEquipmentType).OrderByDescending(x => x.Name).ToList();
            lstEqSize.Add(new EquipmentSizeDTO() { EquipmentSizeId = MoveCode.DefaultValue.ValuePickerDefault, Name = string.Format(AppString.lblDefaultSelection, AppString.lblSize) });
            lstEqSize.Reverse();
            LstEquipmentSize = lstEqSize;
            OnPropertyChanged("LstEquipmentSize");
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
                ChassisRequired = RequiredFieldValidator.RequiredField(LstServiceTypeRepository,
                    FieldRequirementCode.Chassis, selService);
                StatusRequired = RequiredFieldValidator.RequiredField(LstServiceTypeRepository,
                    FieldRequirementCode.EqStatus, selService);

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
                await ShowError(ErrorCode.RegisterLiftActivityRequired, AppString.lblErrorUnknown);
            }
        }

        async void EquipmentTypeRequired()
        {
            try
            {
                var _productRequired = RequiredFieldValidator.RequiredField(LstEquipmentType, FieldRequirementCode.Product, selEquipmentType);
                var _chassisRequired = RequiredFieldValidator.RequiredField(LstEquipmentType, FieldRequirementCode.Chassis, selEquipmentType);
                var _equipmentNumberRequired = RequiredFieldValidator.RequiredField(LstEquipmentType, FieldRequirementCode.EquipNumber, selEquipmentType);
                var _statusRequired = RequiredFieldValidator.RequiredField(LstEquipmentType, FieldRequirementCode.EqStatus, selEquipmentType);

                ProductRequired = _productRequired ?? ProductRequired;
                ChassisRequired = _chassisRequired ?? ChassisRequired;
                EquipmentNumberRequired = _equipmentNumberRequired ?? EquipmentNumberRequired;
                StatusRequired = _statusRequired ?? StatusRequired;
            }
            catch (Exception)
            {
                await ShowError(ErrorCode.RegisterLiftEquipmentTypeRequired, AppString.lblErrorUnknown);
            }
        }
        public ICommand GetEquipment => CreateCommand(async () =>
        {
            try
            {
                await GetAllEquipment();
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
                await ShowError(ErrorCode.RegisterLiftGetEquipment, Infrastructure.Resources.StringResources.GenericError);
            }
        });
        public ICommand ClearEquipmentCommand => CreateCommand(() =>
        {
            GeneralOperateTaylorLift.EquipmentType = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralOperateTaylorLift.EquipmentSize = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralOperateTaylorLift.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralOperateTaylorLift.EquipmentNumber = string.Empty;
            GeneralOperateTaylorLift.ChassisNumber = string.Empty;
            EquipmentNumber = string.Empty;

            selEquipmentType = GeneralOperateTaylorLift.EquipmentType;
            selEquipmentSize = GeneralOperateTaylorLift.EquipmentSize;
            selEquipmentStatus = GeneralOperateTaylorLift.EquipmentStatus;
            clearTestDates();
            //if (!string.IsNullOrEmpty(GeneralOperateTaylorLift?.EquipmentTestDate5Year)) TxtDp5yrTestDate = GeneralOperateTaylorLift.EquipmentTestDate5Year;
            //if (!string.IsNullOrEmpty(GeneralOperateTaylorLift?.EquipmentTestDate25Year)) TxtDp25yrTestDate = GeneralOperateTaylorLift.EquipmentTestDate25Year;
            ActivityRequired();
            OnPropertyChanged("GeneralOperateTaylorLift");
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
            var result = await PromptText(GeneralOperateTaylorLift.DriverComments, string.Format(AppString.lblCommentsMaxLength, CommentMaxLenght), CommentMaxLenght);
            if (!result.Ok) return;
            GeneralOperateTaylorLift.DriverComments = result.Text;
            OnPropertyChanged("GeneralOperateTaylorLift");
        });
        #endregion

        #region UIValidation
        private bool FieldValidator()
        {
            switch (SelectedTab)
            {
                case 0:
                    int m = 8;
                    if (string.IsNullOrEmpty(GeneralOperateTaylorLift.ShipmentID)) return false;
                    var result = new UtilMove().isShipmentIdValid(GeneralOperateTaylorLift.ShipmentID);
                    if(!result) return result;
                    GeneralOperateTaylorLift.CostCenter = 0;
                    GeneralOperateTaylorLift.CostCenterName = string.Empty;
                    break;
                case 1:
                    if (string.IsNullOrEmpty(GeneralOperateTaylorLift.CostCenterName)) return false;
                    GeneralOperateTaylorLift.ShipmentID = string.Empty;
                    break;
            }
            //Location Validation
            if (BlockRequired == FieldRequirementCode.Value.Required)
            {
                if (string.IsNullOrEmpty(GeneralOperateTaylorLift.StartName)) return false;
            }
            else if (BlockRequired == FieldRequirementCode.Value.Hidden)
            {
                GeneralOperateTaylorLift.StartName = string.Empty;
                GeneralOperateTaylorLift.Start = string.Empty;
            }

            if (selDispatcher == MoveCode.DefaultValue.ValuePickerDefault) return false;

            //Product Validation
            if (ProductRequired == FieldRequirementCode.Value.Required)
            {
                if (string.IsNullOrEmpty(GeneralOperateTaylorLift.Product)) return false;
            }
            else if (ProductRequired == FieldRequirementCode.Value.Hidden)
            {
                GeneralOperateTaylorLift.Product = string.Empty;
                GeneralOperateTaylorLift.ProductDescription = string.Empty;
            }
            
            //Equipment Validation
            if (EquipmentRequired == FieldRequirementCode.Value.Required)
            {
                if (selEquipmentType == MoveCode.DefaultValue.ValuePickerDefault) return false;
                if (selEquipmentSize == MoveCode.DefaultValue.ValuePickerDefault) return false;
            }
            else if (EquipmentRequired == FieldRequirementCode.Value.Hidden)
            {
                selEquipmentType = MoveCode.DefaultValue.ValuePickerDefault;
                selEquipmentSize = MoveCode.DefaultValue.ValuePickerDefault;
                if (!string.IsNullOrEmpty(_txtIdEqType))
                    if (_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode))
                    {
                        TxtDp25yrTestDate = string.Empty;
                        TxtDp5yrTestDate = string.Empty;
                    }
            }

            //Equipment Number Validation
            if (EquipmentNumberRequired == FieldRequirementCode.Value.Required)
            {
                if (!ContainerValidator()) return false;
            }
            else if (EquipmentNumberRequired == FieldRequirementCode.Value.Hidden) GeneralOperateTaylorLift.EquipmentNumber = string.Empty;
            //Status Validation
            if (StatusRequired == FieldRequirementCode.Value.Required)
            {
               if (selEquipmentStatus == MoveCode.DefaultValue.ValuePickerDefault) return false;
            }
            else if (StatusRequired == FieldRequirementCode.Value.Hidden) selEquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
            
            //Equipment Validation
            if (EquipmentRequired == FieldRequirementCode.Value.Required)
            {
                
                if (_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode))
                {
                    if (!DateValidator(TxtDp25yrTestDate, 0)) return false;
                    if (!DateValidator(TxtDp5yrTestDate, 1)) return false;
                }
            }

            if (ChassisRequired == FieldRequirementCode.Value.Required)
            {
                var chassisValidatorParams = new ChassisValidatorParams()
                {
                    ChassisNoValidFormat = ChassisNoValidFormat,
                    Size = TxtIdEqSize,
                    Type = TxtIdEqType,
                    chassisValue = GeneralOperateTaylorLift.ChassisNumber,
                    EqSizeTypeDisChassis = EqSizeTypeDisChassis
                };
                switch (ContainerChassisValidator.ChassisValidation(chassisValidatorParams))
                {
                    case ContainerChassisValidator.ChassisValidationCode.NoChassisValue:
                    case ContainerChassisValidator.ChassisValidationCode.WrongFormat:
                        return false;
                }
            }else if (ChassisRequired == FieldRequirementCode.Value.Hidden) GeneralOperateTaylorLift.ChassisNumber = string.Empty;

            //if (string.IsNullOrWhiteSpace(GeneralDetention.Bobtail) && selService.Equals(MoveCode.MoveType.BobTailCode)) return false;
            return true;
        }
        private bool ContainerValidator()
        {
            var containerResult = string.Empty;
            if (selEquipmentType == MoveCode.DefaultValue.ValuePickerDefault) return false;
            if (selEquipmentSize == MoveCode.DefaultValue.ValuePickerDefault) return false;

            var containerValidatorParams = new ContainerValidatorParams()
            {
                ContainerValue = GeneralOperateTaylorLift.EquipmentNumber,
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
                    EquipmentNumber = containerResult;
                    break;
                case ContainerChassisValidator.ContainerValidationCode.NoContainerValue:
                case ContainerChassisValidator.ContainerValidationCode.IncorrectCheckDigit:
                case ContainerChassisValidator.ContainerValidationCode.InvalidLength:
                case ContainerChassisValidator.ContainerValidationCode.WrongFormat:
                    return false;
            }
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
                        GeneralOperateTaylorLift.EquipmentTestDate25Year = isCorrect;
                        break;
                    case 1:
                        GeneralOperateTaylorLift.EquipmentTestDate5Year = isCorrect;
                        break;
                }
                _operateTaylorLiftRepository.Update(GeneralOperateTaylorLift);
                response = true;
            }
            return response;
        }

        public void clearTestDates()
        {
            GeneralOperateTaylorLift.EquipmentTestDate5Year = string.Empty;
            GeneralOperateTaylorLift.EquipmentTestDate25Year = string.Empty;
            TxtDp5yrTestDate = string.Empty;
            TxtDp25yrTestDate = string.Empty;
        }
        #endregion
        #region Var Zone
        private BEOperateTaylorLift _generalOperateTaylorLift;
        public BEOperateTaylorLift GeneralOperateTaylorLift
        {
            get { return _generalOperateTaylorLift; }
            set { SetProperty(ref _generalOperateTaylorLift, value); }
        }
        public IEnumerable<DispatchingPartyDTO> LstDispatcher { get; set; }
        private int _dispatcher;
        public int selDispatcher
        {
            get { return _dispatcher; }
            set
            {
                SetProperty(ref _dispatcher, value);
            }
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
        public IList<ServiceDTO> LstServiceTypeRepository { get; set; }
        public IList<EquipmentTypeDTO> LstEquipmentType { get; set; }
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
        private int _selH34;
        public int selH34
        {
            get { return _selH34; }
            set
            {
                SetProperty(ref _selH34, value);
            }
        }
        private int _service;
        public int selService
        {
            get { return _service; }
            set { SetProperty(ref _service, value); }
        }
        //public IEnumerable<ServiceDTO> LstService { get; set; }
        //private int _service;
        //public int selService
        //{
        //    get { return _service; }
        //    set
        //    {
        //        SetProperty(ref _service, value);
        //        //if (_service != _defaultValuePicker) { LblMoveType = string.Empty; }
        //    }
        //}
        public IEnumerable<EquipmentStatusDTO> LstEquipmentStatus { get; set; }
        private int _equipmentStatus;
        public int selEquipmentStatus
        {
            get { return _equipmentStatus; }
            set { SetProperty(ref _equipmentStatus, value); }
        }

        private IList<CostCenterDTO> _lstCostCenter;
        public IList<CostCenterDTO> LstCostCenter
        {
            get { return _lstCostCenter; }
            set { SetProperty(ref _lstCostCenter, value); }
        }

        private IList<LocationDTO> _lstBlock;
        public IList<LocationDTO> LstBlock
        {
            get { return _lstBlock; }
            set { SetProperty(ref _lstBlock, value); }
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
            {
                SetProperty(ref _selectedTab, value);

            }
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
            }
        }
        private string _startName;
        public string StartName
        {
            get { return _startName; }
            set
            {
                SetProperty(ref _startName, value);
                GeneralOperateTaylorLift.StartName = _startName;
                OnPropertyChanged("GeneralOperateTaylorLift.StartName");
            }
        }
        private bool _isH34;
        public bool IsH34
        {
            get { return _isH34; }
            set
            {
                SetProperty(ref _isH34, value);
                GeneralOperateTaylorLift.HasH34 = _isH34;
                OnPropertyChanged("GeneralOperateTaylorLift.HasH34");
            }
        }
        private string _equipmentNumber;
        public string EquipmentNumber
        {
            get { return _equipmentNumber; }
            set
            {
                SetProperty(ref _equipmentNumber, value);
                GeneralOperateTaylorLift.EquipmentNumber = _equipmentNumber;
                OnPropertyChanged("GeneralOperateTaylorLift.EquipmentNumber");
            }
        }
        private string _product;
        public string Product
        {
            get { return _product; }
            set
            {
                SetProperty(ref _product, value);
                GeneralOperateTaylorLift.ProductDescription = _product;
                OnPropertyChanged("GeneralOperateTaylorLift.ProductDescription");
            }
        }



        private string _txtIdEqType;
        public string TxtIdEqType
        {
            get { return _txtIdEqType; }
            set
            {
                SetProperty(ref _txtIdEqType, value);
            }
        }
        private string _txtIdEqSize;
        public string TxtIdEqSize
        {
            get { return _txtIdEqSize; }
            set
            {
                SetProperty(ref _txtIdEqSize, value);
            }
        }

        public bool isAnotherOperateTaylorLift { get; set; }
        public string ProductRequired { get; set; }
        public string BlockRequired { get; set; }
        public string EquipmentRequired { get; set; }
        public string EquipmentNumberRequired { get; set; }
        public string EquipmentSectionRequired { get; set; }
        public string ChassisRequired { get; set; }
        public string StatusRequired { get; set; }

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
        public string FromBlockSeachImage
        {
            get { return _fromBlockSeachImage; }
            set { SetProperty(ref _fromBlockSeachImage, value); }
        }

        #endregion     
    }
}
