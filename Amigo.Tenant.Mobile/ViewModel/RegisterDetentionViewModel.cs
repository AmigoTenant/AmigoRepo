using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XPO.ShuttleTracking.Application.DTOs.Requests.Tracking;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Domain.Util;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
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
    public class RegisterDetentionViewModel : TodayViewModel
    {
        private readonly INavigator _navigator;
        private readonly IDetentionRepository _detentionRepository;
        private int CommentMaxLenght = 50;

        //drop down menu
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IEquipmentSizeRepository _equipmentSizeRepository;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IEquipmentStatusRepository _equipmentStatusRepository;
        private readonly IDispatchingPartyRepository _dispatchingPartyRepository;
        private int _defaultValuePicker = MoveCode.DefaultValue.ValuePickerDefault;

        public RegisterDetentionViewModel(INavigator navigator,
            IDetentionRepository detentionRepository,
            IServiceTypeRepository serviceTypeRepository,
            IEquipmentSizeRepository equipmentSizeRepository,
            IEquipmentTypeRepository equipmentTypeRepository,
            IEquipmentStatusRepository equipmentStatusRepository,
            IDispatchingPartyRepository dispatchingPartyRepository)
        {
            _navigator = navigator;
            _detentionRepository = detentionRepository;
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
            if (GeneralDetention.CurrentState <= MoveState.CreatedMove && !isAnotherDetention) return;
            SelectedTab = string.IsNullOrEmpty(GeneralDetention.CostCenterName) ? 0 : 1;

            selEquipmentType = GeneralDetention.EquipmentType;
            selEquipmentSize = GeneralDetention.EquipmentSize;
            selDispatcher = GeneralDetention.DispatchingParty;

            selEquipmentStatus = GeneralDetention.EquipmentStatus;

            isAnotherDetention = false;
        }
        private void UpdateScreen()
        {
            GeneralDetention = _detentionRepository.FindByKey(GeneralDetention.InternalId);
            TxtDp25yrTestDate = string.IsNullOrEmpty(GeneralDetention.EquipmentTestDate25Year) ? string.Empty : GeneralDetention.EquipmentTestDate25Year;
            TxtDp5yrTestDate = string.IsNullOrEmpty(GeneralDetention.EquipmentTestDate5Year) ? string.Empty : GeneralDetention.EquipmentTestDate5Year;

            if (string.IsNullOrEmpty(GeneralDetention.ProductDescription))
            ProductSeachImage = MoveCode.SearchIcon;
            else ProductSeachImage = MoveCode.DeleteIcon;

            if (string.IsNullOrEmpty(GeneralDetention.StartName))
                FromBlockSeachImage = MoveCode.SearchIcon;
            else FromBlockSeachImage = MoveCode.DeleteIcon;

            if (string.IsNullOrEmpty(GeneralDetention.FinishName))
                ToBlockSeachImage = MoveCode.SearchIcon;
            else ToBlockSeachImage = MoveCode.DeleteIcon;

            OnPropertyChanged("GeneralDetention");
        }
        
        private void LoadComponents()
        {
            SelectedTab = string.IsNullOrEmpty(GeneralDetention.CostCenterName) ? 0 : 1;
            TxtDp25yrTestDate = string.Empty;
            TxtDp5yrTestDate = string.Empty;

            LstServiceTypeRepository = _serviceTypeRepository.GetAll().Where(x => x.ServiceTypeCode == ServiceCode.Detention).ToList();
            selService = LstServiceTypeRepository.FirstOrDefault(x => x.ServiceTypeCode == ServiceCode.Detention).ServiceId;

            LoadDispatcher();
            LoadEquipment();
            LoadValidations();
            LoadProductBlocks();

            var old = isAnotherDetention;
            isAnotherDetention = false;
            UpdateScreenFromMain();
            isAnotherDetention = old;

            //UpdateScreenFromMain();
            ActivityRequired();
            //OnPropertyChanged("selH34");
            OnPropertyChanged("LstService");
            //OnPropertyChanged("selService");
            //OnPropertyChanged("selDispatcher");
            OnPropertyChanged("LstDispatcher");
            //OnPropertyChanged("selEquipmentType");
            //OnPropertyChanged("LstEquipmentType");
            //OnPropertyChanged("selEquipmentSize");
            //OnPropertyChanged("LstEquipmentSize");
            //OnPropertyChanged("selEquipmentStatus");
            //OnPropertyChanged("LstEquipmentStatus");
        }

        private void LoadProductBlocks()
        {
            ProductSeachImage = string.IsNullOrEmpty(GeneralDetention.ProductDescription)
                ? MoveCode.SearchIcon
                : MoveCode.DeleteIcon;

            FromBlockSeachImage = string.IsNullOrEmpty(GeneralDetention.StartName) ? MoveCode.SearchIcon : MoveCode.DeleteIcon;

            ToBlockSeachImage = string.IsNullOrEmpty(GeneralDetention.FinishName) ? MoveCode.SearchIcon : MoveCode.DeleteIcon;
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
                : varEqSizeCalcEqChkDigit.Split(',').ToList<string>();

            var varEqSizeValEqChkDigit = Parameters.Get(ParameterCode.EqSizeValEqChkDigit);
            EqSizeValEqChkDigit = string.IsNullOrEmpty(varEqSizeValEqChkDigit)
                ? new List<string>() {"20", "40", "45"}
                : varEqSizeValEqChkDigit.Split(',').ToList<string>();

            var varEqpNoValidFormat = Parameters.Get(ParameterCode.EqpNoValidFormat);
            EqpNoValidFormat = string.IsNullOrEmpty(varEqpNoValidFormat) ? @"^[a-zA-Z]{4}\d{6,7}$" : varEqpNoValidFormat;

            var varEqpNoMaxLength = 0;
            if (!int.TryParse((Parameters.Get(ParameterCode.EqpNoMaxLength)), out varEqpNoMaxLength))
                varEqpNoMaxLength = 11;
            EqpNoMaxLength = varEqpNoMaxLength;

            var varEqpNoMinLength = 0;
            if (!int.TryParse((Parameters.Get(ParameterCode.EqpNoMinLength)), out varEqpNoMinLength))
                varEqpNoMinLength = 11;
            EqpNoMinLength = varEqpNoMinLength;

            var varEqSizeTypeDisChassis = Parameters.Get(ParameterCode.EqSizeTypeDisChassis);
            EqSizeTypeDisChassis = string.IsNullOrEmpty(varEqSizeTypeDisChassis)
                ? new List<string>() {"53~DRV"}
                : varEqSizeTypeDisChassis.Split(',').ToList<string>();
        }

        private void LoadEquipment()
        {
            //Equipment Type
            if (LstEquipmentType == null || !LstEquipmentType.Any())
            {
                var lstEqType = _equipmentTypeRepository.GetAll().OrderBy(x => x.Name).ToList();
                lstEqType.Insert(0,
                    new EquipmentTypeDTO()
                    {
                        EquipmentTypeId = MoveCode.DefaultValue.ValuePickerDefault,
                        Name = string.Format(AppString.lblDefaultSelection, AppString.lblType)
                    });
                LstEquipmentType = lstEqType;
            }
            _equipmentType = isAnotherDetention ? GeneralDetention.EquipmentType : MoveCode.DefaultValue.ValuePickerDefault;
            OnPropertyChanged("selEquipmentType");

            //Equipment Size



            var lstEqSize = _equipmentSizeRepository.FindAll(x => x.EquipmentTypeId == GeneralDetention.EquipmentType).OrderBy(x => x.Name).ToList();
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
                    var value = isAnotherDetention ? GeneralDetention.EquipmentSize : MoveCode.DefaultValue.ValuePickerDefault;
                    selEquipmentSize = value;
                });
            }).ConfigureAwait(continueOnCapturedContext: true);


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
            if (!string.IsNullOrEmpty(GeneralDetention?.EquipmentTestDate5Year)) TxtDp5yrTestDate = GeneralDetention.EquipmentTestDate5Year;
            if (!string.IsNullOrEmpty(GeneralDetention?.EquipmentTestDate25Year)) TxtDp25yrTestDate = GeneralDetention.EquipmentTestDate25Year;

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
            _dispatcher = isAnotherDetention ? GeneralDetention.DispatchingParty : MoveCode.DefaultValue.ValuePickerDefault;
            OnPropertyChanged("selDispatcher");
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
                    EquipmentNo = GeneralDetention.EquipmentNumber
                };
                var responseEquipment = await GetAllEquipment(equipmentSearchRequestTemp);
                HandleResult(responseEquipment, () =>
                {
                    GeneralDetention.EquipmentSize = selEquipmentSize = responseEquipment.Data.Items[0].EquipmentSizeId;//9
                    GeneralDetention.EquipmentType = selEquipmentType = responseEquipment.Data.Items[0].EquipmentTypeId;//4
                    GeneralDetention.EquipmentNumber = responseEquipment.Data.Items[0].EquipmentNo;
                    _detentionRepository.Update(GeneralDetention);
                    UpdateScreen();
                    UpdateScreenFromMain();
                    OnPropertyChanged("selEquipmentType");
                    OnPropertyChanged("selEquipmentSize");
                    OnPropertyChanged("GeneralDetention.EquipmentNumber");

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
                var service = _serviceTypeRepository.FindAll(m => m.ServiceTypeCode.Equals(MoveCode.MoveType.DetentionCode)).First();
                GeneralDetention.CurrentState = MoveState.SavedMove;
                GeneralDetention.EquipmentSize = selEquipmentSize;
                GeneralDetention.EquipmentSizeDesc = selEquipmentSize == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _equipmentSizeRepository.GetAll().First(x => x.EquipmentSizeId == selEquipmentSize).Name;
                GeneralDetention.EquipmentType = selEquipmentType;
                GeneralDetention.EquipmentTypeDesc = selEquipmentType == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _equipmentTypeRepository.GetAll().First(x => x.EquipmentTypeId == selEquipmentType).Name;
                GeneralDetention.EquipmentStatus = selEquipmentStatus;
                GeneralDetention.EquipmentStatusDesc = selEquipmentStatus == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _equipmentStatusRepository.GetAll().First(x => x.EquipmentStatusId == selEquipmentStatus).Name;
                GeneralDetention.DispatchingParty = selDispatcher;
                GeneralDetention.DispatchingPartyDesc = selDispatcher == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _dispatchingPartyRepository.GetAll().FirstOrDefault(x => x.DispatchingPartyId == selDispatcher).Name;
                GeneralDetention.Service = service.ServiceId;
                GeneralDetention.MoveType = service.ServiceId;
                GeneralDetention.MoveTypeDesc = service.Name;
                GeneralDetention.ServiceFinishDate = null;
                GeneralDetention.ServiceAcknowledgedDateTZ = null;
                if (_txtIdEqType != null)
                {
                    GeneralDetention.EquipmentTestDate25Year = !_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode)? string.Empty: GeneralDetention.EquipmentTestDate25Year;
                    GeneralDetention.EquipmentTestDate5Year = !_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode)? string.Empty: GeneralDetention.EquipmentTestDate5Year;
                }
                if (string.IsNullOrEmpty(GeneralDetention.ShipmentID)) GeneralDetention.ShipmentID = string.Empty;
                if (string.IsNullOrEmpty(GeneralDetention.CostCenterName)) GeneralDetention.CostCenterName = string.Empty;
                _detentionRepository.Update(GeneralDetention);
            }
            catch (Exception)
            {
                await ShowError(ErrorCode.RegisterDetentionSaveGeneral, AppString.lblErrorUnknown);
            }
                       
        }
        #endregion

        #region Navigation
        public ICommand GoStartDetention => CreateCommand(async () => {

            if (FieldValidator())
            {
                SaveMoveGeneral();
                await _navigator.PushAsync<StartDetentionViewModel>(m => m.GeneralDetention = GeneralDetention);
            }
        });

        public ICommand GoSearchCostCenter => CreateCommand(async () =>
        {
            SavePreviusData();
            await _navigator.PushAsync<CostCenterSearchViewModel>((x) => { x.GeneralDetention = GeneralDetention; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.Detention; });
        });
        public ICommand GoFromBlock => CreateCommand(async () =>
        {
            if (string.IsNullOrEmpty(GeneralDetention.StartName))
            {
                SavePreviusData();
                await _navigator.PushAsync<FromBlockSearchViewModel>(x =>
                {
                    x.GeneralDetention = GeneralDetention; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.Detention;
                });
            }
            else
            {
                GeneralDetention.StartName = string.Empty;
                FromBlockSeachImage = MoveCode.SearchIcon;
            }

            OnPropertyChanged("GeneralDetention");
        });
        public ICommand GoToBlock => CreateCommand(async () =>
        {
            if (string.IsNullOrEmpty(GeneralDetention.FinishName))
            {
                SavePreviusData();
                await _navigator.PushAsync<ToBlockSearchViewModel>(x => { x.GeneralDetention = GeneralDetention; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.Detention; });
            }
            else
            {
                GeneralDetention.FinishName = string.Empty;
                ToBlockSeachImage = MoveCode.SearchIcon;
            }

            OnPropertyChanged("GeneralDetention");
        });
        public ICommand GoToSearchProduct => CreateCommand(async () =>
        {
            if (string.IsNullOrEmpty(GeneralDetention.ProductDescription))
            {
                SavePreviusData();
                await _navigator.PushAsync<ProductSearchViewModel>(x => { x.GeneralDetention = GeneralDetention; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.Detention; });
            }
            else
            {
                GeneralDetention.ProductDescription = string.Empty;
                GeneralDetention.Product = string.Empty;
                ProductSeachImage = MoveCode.SearchIcon;
            }

            OnPropertyChanged("ProductSeachImage");
            OnPropertyChanged("GeneralDetention");

        });
        private void SavePreviusData()
        {
            _detentionRepository.Update(GeneralDetention);            
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
                ProductRequired = RequiredFieldValidator.RequiredField(LstServiceTypeRepository, FieldRequirementCode.Product, selService);
                BlockRequired = RequiredFieldValidator.RequiredField(LstServiceTypeRepository, FieldRequirementCode.Block, selService);
                EquipmentRequired = EquipmentNumberRequired = RequiredFieldValidator.RequiredField(LstServiceTypeRepository, FieldRequirementCode.Equipment, selService);
                ChassisRequired = RequiredFieldValidator.RequiredField(LstServiceTypeRepository, FieldRequirementCode.Chassis, selService);
                StatusRequired = RequiredFieldValidator.RequiredField(LstServiceTypeRepository, FieldRequirementCode.EqStatus, selService);

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
                await ShowError(ErrorCode.RegisterDetentionActivityRequired, AppString.lblErrorUnknown);
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
                await ShowError(ErrorCode.RegisterDetentionEquipmentTypeRequired, AppString.lblErrorUnknown);
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
                await ShowError(ErrorCode.RegisterDetentionGetEquipment, Infrastructure.Resources.StringResources.GenericError);
            }
        });
        public ICommand ClearEquipmentCommand => CreateCommand(() =>
        {
            GeneralDetention.EquipmentType = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralDetention.EquipmentSize = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralDetention.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralDetention.EquipmentNumber = string.Empty;
            GeneralDetention.ChassisNumber = string.Empty;

            selEquipmentType = GeneralDetention.EquipmentType;
            selEquipmentSize = GeneralDetention.EquipmentSize;
            selEquipmentStatus = GeneralDetention.EquipmentStatus;
            clearTestDates();
            //if (!string.IsNullOrEmpty(GeneralDetention?.EquipmentTestDate5Year)) TxtDp5yrTestDate = GeneralDetention.EquipmentTestDate5Year;
            //if (!string.IsNullOrEmpty(GeneralDetention?.EquipmentTestDate25Year)) TxtDp25yrTestDate = GeneralDetention.EquipmentTestDate25Year;
            ActivityRequired();
            OnPropertyChanged("GeneralDetention");
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
            var result = await PromptText(GeneralDetention.DriverComments, string.Format(AppString.lblCommentsMaxLength, CommentMaxLenght), CommentMaxLenght);
            if (!result.Ok) return;
            GeneralDetention.DriverComments = result.Text;
            OnPropertyChanged("GeneralDetention");
        });
        #endregion

        #region UIValidation
        private bool FieldValidator()
        {
            switch (SelectedTab)
            {
                case 0:
                    int m = 8;
                    if (string.IsNullOrEmpty(GeneralDetention.ShipmentID)) return false;
                    var result = new UtilMove().isShipmentIdValid(GeneralDetention.ShipmentID);
                    if (!result) return result;
                    GeneralDetention.CostCenter = 0;
                    GeneralDetention.CostCenterName = string.Empty;
                    break;
                case 1:
                    if (string.IsNullOrEmpty(GeneralDetention.CostCenterName)) return false;
                    GeneralDetention.ShipmentID = string.Empty;
                    break;
            }
            //Location Validation
            if (BlockRequired == FieldRequirementCode.Value.Required)
            {
                if (string.IsNullOrEmpty(GeneralDetention.StartName)) return false;
            }
            else if (BlockRequired == FieldRequirementCode.Value.Hidden)
            {
                GeneralDetention.StartName = string.Empty;
                GeneralDetention.Start = string.Empty;
            }
            if (selDispatcher == MoveCode.DefaultValue.ValuePickerDefault) return false;

            //Product Validation
            if (ProductRequired == FieldRequirementCode.Value.Required)
            { 
                if (string.IsNullOrEmpty(GeneralDetention.Product)) return false;
            }
            else if (ProductRequired == FieldRequirementCode.Value.Hidden)
            {
                GeneralDetention.Product = string.Empty;
                GeneralDetention.ProductDescription = string.Empty;
            }
            //Equipment Validation
            if (EquipmentRequired == FieldRequirementCode.Value.Required)
            {
                if (selEquipmentType == MoveCode.DefaultValue.ValuePickerDefault) return false;
                if (selEquipmentSize == MoveCode.DefaultValue.ValuePickerDefault) return false;
            }else if (EquipmentRequired == FieldRequirementCode.Value.Hidden)
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
            }else if (EquipmentNumberRequired == FieldRequirementCode.Value.Hidden) GeneralDetention.EquipmentNumber = string.Empty;

            //Status Validation
            if (StatusRequired == FieldRequirementCode.Value.Required)
            {
                if (selEquipmentStatus == MoveCode.DefaultValue.ValuePickerDefault) return false;
            }
            else if (StatusRequired == FieldRequirementCode.Value.Hidden) selEquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
            //Chassis Validation
            if (ChassisRequired == FieldRequirementCode.Value.Required)
            {
                var chassisValidatorParams = new ChassisValidatorParams()
                {
                    ChassisNoValidFormat = ChassisNoValidFormat,
                    Size = TxtIdEqSize,
                    Type = TxtIdEqType,
                    chassisValue = GeneralDetention.ChassisNumber,
                    EqSizeTypeDisChassis = EqSizeTypeDisChassis
                };
                switch (ContainerChassisValidator.ChassisValidation(chassisValidatorParams))
                {
                    case ContainerChassisValidator.ChassisValidationCode.NoChassisValue:
                    case ContainerChassisValidator.ChassisValidationCode.WrongFormat:
                        return false;
                }
            }
            else if(ChassisRequired == FieldRequirementCode.Value.Hidden) GeneralDetention.ChassisNumber = string.Empty;
            //Equipment Validation
            if (EquipmentRequired == FieldRequirementCode.Value.Required)
            {
                if (_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode))
                {
                    if (!DateValidator(TxtDp25yrTestDate, 0)) return false;
                    if (!DateValidator(TxtDp5yrTestDate, 1)) return false;
                }
            }

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
                ContainerValue = GeneralDetention.EquipmentNumber,
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
                    GeneralDetention.EquipmentNumber = containerResult;
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
                        GeneralDetention.EquipmentTestDate25Year = isCorrect;
                        break;
                    case 1:
                        GeneralDetention.EquipmentTestDate5Year = isCorrect;
                        break;
                }
                _detentionRepository.Update(GeneralDetention);
                response = true;
            }
            return response;
        }

        public void clearTestDates()
        {
            GeneralDetention.EquipmentTestDate5Year = string.Empty;
            GeneralDetention.EquipmentTestDate25Year = string.Empty;
            TxtDp5yrTestDate = string.Empty;
            TxtDp25yrTestDate = string.Empty;
        }
        #endregion
        #region Var Zone
        private BEDetention _generalDetention;
        public BEDetention GeneralDetention
        {
            get { return _generalDetention; }
            set { SetProperty(ref _generalDetention, value); }
        }
        public IEnumerable<DispatchingPartyDTO> LstDispatcher { get; set; }
        private int _dispatcher;
        public int selDispatcher
        {
            get { return _dispatcher; }
            set { SetProperty(ref _dispatcher, value); }
        }
        public IList<EquipmentSizeDTO> LstEquipmentSize
        {
            get { return _lstEquipmentSize; }
            set { SetProperty(ref _lstEquipmentSize, value); }
        }
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
        public IList<EquipmentTypeDTO> LstEquipmentType
        {
            get { return _lstEquipmentType; }
            set { SetProperty(ref _lstEquipmentType, value); }
        }
        private IList<EquipmentSizeDTO> _lstEquipmentSize;
        private IList<EquipmentTypeDTO> _lstEquipmentType;
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
        private int _service;
        public int selService
        {
            get { return _service; }
            set { SetProperty(ref _service, value); }
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

        public bool isAnotherDetention { get; set; }
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
