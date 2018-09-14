using System;
using System.Windows.Input;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Resource;
using System.Threading.Tasks;
using System.Collections.Generic;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using XPO.ShuttleTracking.Application.DTOs.Requests.Tracking;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Mobile.Domain.Util;
using XPO.ShuttleTracking.Mobile.ViewModel.SearchItem;
using XPO.ShuttleTracking.Mobile.Entity.Service;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Entity;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.CustomException;
using XPO.ShuttleTracking.Mobile.Helpers.Util;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class RegisterAdditionalServiceViewModel : TodayViewModel
    {
        private const string _typeMove = "ADS";
        private int CommentMaxLenght = 50;
        private readonly INavigator _navigator;
        private readonly IServiceRepository _serviceRepository;
        //drop down menu
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IEquipmentSizeRepository _equipmentSizeRepository;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IEquipmentStatusRepository _equipmentStatusRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IDispatchingPartyRepository _dispatchingPartyRepository;

        public RegisterAdditionalServiceViewModel(INavigator navigator,
            IServiceRepository serviceRepository,
            IServiceTypeRepository serviceTypeRepository,
            IEquipmentSizeRepository equipmentSizeRepository,
            IEquipmentTypeRepository equipmentTypeRepository,
            IEquipmentStatusRepository equipmentStatusRepository,
            ISessionRepository sessionRepository,
            IDispatchingPartyRepository dispatchingPartyRepository)
        {
            _navigator = navigator;
            _serviceRepository = serviceRepository;
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
            LoadComponents();
        }

        #region DataLoad
        private void UpdateScreenFromMain()
        {
            if (GeneralService.CurrentState <= MoveState.CreatedMove && !isAnotherService) return;
            SelectedTab = string.IsNullOrEmpty(GeneralService.CostCenterName) ? 0 : 1;

            selEquipmentType = GeneralService.EquipmentType;
            selEquipmentSize = GeneralService.EquipmentSize;
            selDispatcher = GeneralService.DispatchingParty;
            selService = GeneralService.Service;
            selEquipmentStatus = GeneralService.EquipmentStatus;

            isAnotherService = false;
        }
        private void UpdateScreen()
        {
            GeneralService = _serviceRepository.FindByKey(GeneralService.InternalId);
            TxtDp25yrTestDate = string.IsNullOrEmpty(GeneralService.EquipmentTestDate25Year) ? string.Empty : GeneralService.EquipmentTestDate25Year;
            TxtDp5yrTestDate = string.IsNullOrEmpty(GeneralService.EquipmentTestDate5Year) ? string.Empty : GeneralService.EquipmentTestDate5Year;

            if (string.IsNullOrEmpty(GeneralService.ProductDescription))
                ProductSeachImage = MoveCode.SearchIcon;
            else ProductSeachImage = MoveCode.DeleteIcon;

            if (string.IsNullOrEmpty(GeneralService.StartName))
                FromBlockSeachImage = MoveCode.SearchIcon;
            else FromBlockSeachImage = MoveCode.DeleteIcon;

            if (string.IsNullOrEmpty(GeneralService.FinishName))
                ToBlockSeachImage = MoveCode.SearchIcon;
            else ToBlockSeachImage = MoveCode.DeleteIcon;

            //OnPropertyChanged("ProductSeachImage");
            OnPropertyChanged("GeneralService");
        }
        private void LoadComponents()
        {
            var session = _sessionRepository.GetSessionObject();

            LoadServiceType(session);
            LoadDispatcher();
            LoadEquipment();
            LoadValidations();
            LoadProductBlocks();

            var old = isAnotherService;
            isAnotherService = false;
            UpdateScreenFromMain();
            isAnotherService = old;

            //OnPropertyChanged("selH34");
            OnPropertyChanged("LstService");
            OnPropertyChanged("LstDispatcher");
            //OnPropertyChanged("selService");
            //OnPropertyChanged("selDispatcher");
            //OnPropertyChanged("selEquipmentType");
            //OnPropertyChanged("LstEquipmentType");
            //OnPropertyChanged("selEquipmentSize");
            //OnPropertyChanged("LstEquipmentSize");
            //OnPropertyChanged("selEquipmentStatus");
            //OnPropertyChanged("LstEquipmentStatus");
        }

        private void LoadProductBlocks()
        {
            ProductSeachImage = string.IsNullOrEmpty(GeneralService.ProductDescription)
                ? MoveCode.SearchIcon
                : MoveCode.DeleteIcon;

            FromBlockSeachImage = string.IsNullOrEmpty(GeneralService.StartName) ? MoveCode.SearchIcon : MoveCode.DeleteIcon;

            ToBlockSeachImage = string.IsNullOrEmpty(GeneralService.FinishName) ? MoveCode.SearchIcon : MoveCode.DeleteIcon;
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
                ? new List<string>() { "20", "40", "45" }
                : varEqSizeCalcEqChkDigit.Split(',').ToList();

            var varEqSizeValEqChkDigit = Parameters.Get(ParameterCode.EqSizeValEqChkDigit);
            EqSizeValEqChkDigit = string.IsNullOrEmpty(varEqSizeValEqChkDigit)
                ? new List<string>() { "20", "40", "45" }
                : varEqSizeValEqChkDigit.Split(',').ToList();

            var varEqpNoValidFormat = Parameters.Get(ParameterCode.EqpNoValidFormat);
            EqpNoValidFormat = string.IsNullOrEmpty(varEqpNoValidFormat) ? @"^[a-zA-Z]{4}\d{6,7}$" : varEqpNoValidFormat;

            var varEqpNoMaxLength = 0;
            if (!int.TryParse((Parameters.Get(ParameterCode.EqpNoMaxLength)), out varEqpNoMaxLength))
                varEqpNoMaxLength = 11;
            EqpNoMaxLength = varEqpNoMaxLength;

            var varEqpNoMinLength = 0;
            if (!int.TryParse(Parameters.Get(ParameterCode.EqpNoMinLength), out varEqpNoMinLength))
                varEqpNoMinLength = 10;
            EqpNoMinLength = varEqpNoMinLength;

            var varEqSizeTypeDisChassis = Parameters.Get(ParameterCode.EqSizeTypeDisChassis);
            EqSizeTypeDisChassis = string.IsNullOrEmpty(varEqSizeTypeDisChassis)
                ? new List<string>() { "53~DRV" }
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
            _equipmentType = isAnotherService ? GeneralService.EquipmentType : MoveCode.DefaultValue.ValuePickerDefault;
            OnPropertyChanged("selEquipmentType");

            //Equipment Size
            if (LstEquipmentSizeRepository == null || !LstEquipmentSizeRepository.Any())
                LstEquipmentSizeRepository = _equipmentSizeRepository.GetAll().ToList();

            var lstEqSize = LstEquipmentSizeRepository.Where(x => x.EquipmentTypeId == GeneralService.EquipmentType).OrderBy(x => x.Name).ToList();
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
                    var value = isAnotherService ? GeneralService.EquipmentSize : MoveCode.DefaultValue.ValuePickerDefault;
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
            if (!string.IsNullOrEmpty(GeneralService?.EquipmentTestDate5Year)) TxtDp5yrTestDate = GeneralService.EquipmentTestDate5Year;
            if (!string.IsNullOrEmpty(GeneralService?.EquipmentTestDate25Year)) TxtDp25yrTestDate = GeneralService.EquipmentTestDate25Year;

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
            _dispatcher = isAnotherService ? GeneralService.DispatchingParty : MoveCode.DefaultValue.ValuePickerDefault;
            OnPropertyChanged("selDispatcher");
        }

        private void LoadServiceType(BESession session)
        {
            //Load Service Type
            if (LstServiceTypeRepository == null)
                LstServiceTypeRepository = _serviceTypeRepository.GetAll().ToList();
            if (LstEquipmentTypeRepository == null)
                LstEquipmentTypeRepository = _equipmentTypeRepository.GetAll().ToList();

            var defaultService = new ServiceDTO()
            {
                ServiceId = MoveCode.DefaultValue.ValuePickerDefault,
                Name = string.Format(AppString.lblDefaultSelection, AppString.lblServiceType)
            };
            switch (session.TypeUser)
            {
                case UserTypeCode.PerHour:
                    var lstHour =
                        LstServiceTypeRepository.Where(
                            m => m.ServiceTypeCode.Equals(_typeMove) && m.IsPerHour.Equals("1"))
                            .OrderBy(x => x.Name)
                            .ToList();
                    lstHour.Insert(0, defaultService);
                    LstService = lstHour;
                    break;
                case UserTypeCode.PerMove:
                    var lstMove =
                        LstServiceTypeRepository.Where(
                            m => m.ServiceTypeCode.Equals(_typeMove) && m.IsPerMove.Equals("1"))
                            .OrderBy(x => x.Name)
                            .ToList();
                    lstMove.Insert(0, defaultService);
                    LstService = lstMove;
                    break;
            }
            SelectedTab = string.IsNullOrEmpty(GeneralService.CostCenterName) ? 0 : 1;
            selService = MoveCode.DefaultValue.ValuePickerDefault;
            TxtDp25yrTestDate = string.Empty;
            TxtDp5yrTestDate = string.Empty;
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
                    EquipmentNo = GeneralService.EquipmentNumber
                };

                //Load Equipment
                var responseEquipment = await GetAllEquipment(equipmentSearchRequestTemp);
                HandleResult(responseEquipment, () =>
                 {
                     GeneralService.EquipmentSize = selEquipmentSize = responseEquipment.Data.Items[0].EquipmentSizeId;//9
                    GeneralService.EquipmentType = selEquipmentType = responseEquipment.Data.Items[0].EquipmentTypeId;//4
                    GeneralService.EquipmentNumber = responseEquipment.Data.Items[0].EquipmentNo;
                     _serviceRepository.Update(GeneralService);

                     UpdateScreen();
                     UpdateScreenFromMain();
                     OnPropertyChanged("selEquipmentType");
                     OnPropertyChanged("selEquipmentSize");
                     OnPropertyChanged("GeneralService.EquipmentNumber");
                     OnPropertyChanged("GeneralService.HasH34");
                 });
            }
        }
        private async Task<ResponseDTO<PagedList<EquipmentDTO>>> GetAllEquipment(EquipmentSearchRequest equipmentSearchRequest)
        {
            var request = new EquipmentSearchTaskDefinition { EquipementSearch = equipmentSearchRequest };
            return await TaskManager.Current.ExecuteTaskAsync<ResponseDTO<PagedList<EquipmentDTO>>>(request);
        }
        private void SaveMoveGeneral()
        {
            try
            {
                GeneralService.CurrentState = MoveState.SavedMove;
                GeneralService.EquipmentSize = selEquipmentSize;
                GeneralService.EquipmentSizeDesc = selEquipmentSize == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _equipmentSizeRepository.GetAll().First(x => x.EquipmentSizeId == selEquipmentSize).Name;
                GeneralService.EquipmentType = selEquipmentType;
                GeneralService.EquipmentTypeDesc = selEquipmentType == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _equipmentTypeRepository.GetAll().FirstOrDefault(x => x.EquipmentTypeId == selEquipmentType).Name;
                GeneralService.EquipmentStatus = selEquipmentStatus;
                GeneralService.EquipmentStatusDesc = selEquipmentStatus == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _equipmentStatusRepository.GetAll().FirstOrDefault(x => x.EquipmentStatusId == selEquipmentStatus).Name;
                GeneralService.DispatchingParty = selDispatcher;
                GeneralService.DispatchingPartyDesc = selDispatcher == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _dispatchingPartyRepository.GetAll().FirstOrDefault(x => x.DispatchingPartyId == selDispatcher).Name;
                GeneralService.MoveType = selService;
                GeneralService.MoveTypeDesc = selService == MoveCode.DefaultValue.ValuePickerDefault ? string.Empty : _serviceTypeRepository.GetAll().FirstOrDefault(x => x.ServiceId == selService)?.Name ?? string.Empty;
                GeneralService.Service = selService;
                GeneralService.ServiceFinishDate = null;
                GeneralService.ServiceAcknowledgedDateTZ = null;
                if (_txtIdEqType != null)
                {
                    GeneralService.EquipmentTestDate25Year = !_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode) ? string.Empty : GeneralService.EquipmentTestDate25Year;
                    GeneralService.EquipmentTestDate5Year = !_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode) ? string.Empty : GeneralService.EquipmentTestDate5Year;
                }
                if (string.IsNullOrEmpty(GeneralService.ShipmentID)) GeneralService.ShipmentID = string.Empty;
                if (string.IsNullOrEmpty(GeneralService.CostCenterName)) GeneralService.CostCenterName = string.Empty;
                _serviceRepository.Update(GeneralService);
            }
            catch (Exception ex)
            {
                ShowError(ErrorCode.RegisterServiceSaveGeneral, AppString.lblErrorUnknown);
                Logger.Current.LogWarning($"Error: {ex.ToString()}");
            }
        }

        #endregion
        #region Navgation
        public ICommand GoStartAdditionalService => CreateCommand(async () =>
        {
            if (FieldValidator())
            {
                SaveMoveGeneral();
                await _navigator.PushAsync<StartAdditionalServiceViewModel>(m => m.GeneralService = GeneralService);
            }
        });
        public ICommand GoSearchCostCenter => CreateCommand(async () =>
        {
            _serviceRepository.Update(GeneralService);
            await _navigator.PushAsync<CostCenterSearchViewModel>((x) => { x.GeneralService = GeneralService; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.Service; });
        });
        public ICommand GoFromBlock => CreateCommand(async () =>
        {
            if (string.IsNullOrEmpty(GeneralService.StartName))
            {
                _serviceRepository.Update(GeneralService);
                await _navigator.PushAsync<FromBlockSearchViewModel>(x => { x.GeneralService = GeneralService; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.Service; });
            }
            else
            {
                GeneralService.StartName = string.Empty;
                FromBlockSeachImage = MoveCode.SearchIcon;
            }

            OnPropertyChanged("GeneralService");
        });
        public ICommand GoToBlock => CreateCommand(async () =>
        {
            if (string.IsNullOrEmpty(GeneralService.FinishName))
            {
                _serviceRepository.Update(GeneralService);
                await _navigator.PushAsync<ToBlockSearchViewModel>(x => { x.GeneralService = GeneralService; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.Service; });
            }
            else
            {
                GeneralService.FinishName = string.Empty;
                ToBlockSeachImage = MoveCode.SearchIcon;
            }

            OnPropertyChanged("GeneralService");
        });
        public ICommand GoToSearchProduct => CreateCommand(async () =>
        {
            if (string.IsNullOrEmpty(GeneralService.ProductDescription))
            {
                _serviceRepository.Update(GeneralService);
                await _navigator.PushAsync<ProductSearchViewModel>(x => { x.GeneralService = GeneralService; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.Service; });
            }
            else
            {
                GeneralService.ProductDescription = string.Empty;
                GeneralService.Product = string.Empty;
                ProductSeachImage = MoveCode.SearchIcon;
            }

            OnPropertyChanged("ProductSeachImage");
            OnPropertyChanged("GeneralService");
        });

        public ICommand PickerCommand => CreateCommand(() =>
        {
            var lstEqSize = LstEquipmentSizeRepository.Where(x => x.EquipmentTypeId == selEquipmentType).ToList();
            lstEqSize = lstEqSize.OrderByDescending(x => x.Name).ToList();
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
                await ShowError(ErrorCode.RegisterServiceActivityRequired, AppString.lblErrorUnknown);
            }
        }
        async void EquipmentTypeRequired()
        {
            try
            {
                var productRequired = RequiredFieldValidator.RequiredField(LstEquipmentTypeRepository, FieldRequirementCode.Product, selEquipmentType);
                var chassisRequired = RequiredFieldValidator.RequiredField(LstEquipmentTypeRepository, FieldRequirementCode.Chassis, selEquipmentType);
                var equipmentNumberRequired = RequiredFieldValidator.RequiredField(LstEquipmentTypeRepository, FieldRequirementCode.EquipNumber, selEquipmentType);
                var statusRequired = RequiredFieldValidator.RequiredField(LstEquipmentTypeRepository, FieldRequirementCode.EqStatus, selEquipmentType);

                ProductRequired = productRequired ?? ProductRequired;
                ChassisRequired = chassisRequired ?? ChassisRequired;
                EquipmentNumberRequired = equipmentNumberRequired ?? EquipmentNumberRequired;
                StatusRequired = statusRequired ?? StatusRequired;
            }
            catch (Exception)
            {
                await ShowError(ErrorCode.RegisterServiceEquipmentTypeRequired, AppString.lblErrorUnknown);
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
            catch (Exception)
            {
                await ShowError(ErrorCode.RegisterServiceGetEquipment, Infrastructure.Resources.StringResources.GenericError);
            }
        });

        public ICommand ServiceTypeSearch => CreateCommand(async () =>
        {
            try
            {
                ActivityRequired();
            }
            catch (Exception)
            {
                await ShowError(ErrorCode.RegisterServiceServiceTypeSearch, Infrastructure.Resources.StringResources.GenericError);
            }
        });
        public ICommand ClearEquipmentCommand => CreateCommand(() =>
        {
            GeneralService.EquipmentType = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralService.EquipmentSize = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralService.EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
            GeneralService.EquipmentNumber = string.Empty;
            GeneralService.ChassisNumber = string.Empty;

            selEquipmentType = GeneralService.EquipmentType;
            selEquipmentSize = GeneralService.EquipmentSize;
            selEquipmentStatus = GeneralService.EquipmentStatus;
            clearTestDates();
            ActivityRequired();
            OnPropertyChanged("GeneralService");
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
            var result = await PromptText(GeneralService.DriverComments, string.Format(AppString.lblCommentsMaxLength, CommentMaxLenght), CommentMaxLenght);
            if (!result.Ok) return;
            GeneralService.DriverComments = result.Text;
            OnPropertyChanged("GeneralService");
        });
        #endregion
        #region UIValidation
        private bool FieldValidator()
        {
            switch (SelectedTab)
            {
                case 0:
                    int m = 8;
                    if (string.IsNullOrEmpty(GeneralService.ShipmentID)) return false;
                    var result = new UtilMove().isShipmentIdValid(GeneralService.ShipmentID);
                    if (!result) return result;
                    GeneralService.CostCenterName = string.Empty;
                    GeneralService.CostCenter = 0;
                    break;
                case 1:
                    if (string.IsNullOrEmpty(GeneralService.CostCenterName)) return false;
                    GeneralService.ShipmentID = string.Empty;
                    break;
            }
            if (selService == MoveCode.DefaultValue.ValuePickerDefault) return false;

            //////Clean Fields///////
            if (BlockRequired == FieldRequirementCode.Value.Hidden)
            {
                GeneralService.StartName = string.Empty;
                GeneralService.Start = string.Empty;
            }
            if (ProductRequired == FieldRequirementCode.Value.Hidden)
            {
                GeneralService.Product = string.Empty;
                GeneralService.ProductDescription = string.Empty;
            }
            if (EquipmentRequired == FieldRequirementCode.Value.Hidden)
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
            if (EquipmentNumberRequired == FieldRequirementCode.Value.Hidden) GeneralService.EquipmentNumber = string.Empty;
            if (StatusRequired == FieldRequirementCode.Value.Hidden) selEquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault;
            if (ChassisRequired == FieldRequirementCode.Value.Hidden) GeneralService.ChassisNumber = string.Empty;
            //////Clean Fields///////

            //Location Validation
            if (BlockRequired == FieldRequirementCode.Value.Required)
            {
                if (string.IsNullOrEmpty(GeneralService.StartName)) return false;
            }

            if (selDispatcher == MoveCode.DefaultValue.ValuePickerDefault) return false;

            //Product Validation
            if (ProductRequired == FieldRequirementCode.Value.Required)
                if (string.IsNullOrEmpty(GeneralService.Product)) return false;

            //Equipment Validation
            if (EquipmentRequired == FieldRequirementCode.Value.Required)
            {
                if (selEquipmentType == MoveCode.DefaultValue.ValuePickerDefault) return false;
                if (selEquipmentSize == MoveCode.DefaultValue.ValuePickerDefault) return false;
            }

            //Equipment Number Validation
            if (EquipmentNumberRequired == FieldRequirementCode.Value.Required)
            {
                if (!ContainerValidator()) return false;
            }

            //Status Validation
            if (StatusRequired == FieldRequirementCode.Value.Required)
                if (selEquipmentStatus == MoveCode.DefaultValue.ValuePickerDefault) return false;

            //Equipment Validation
            if (ChassisRequired == FieldRequirementCode.Value.Required)
            {
                var chassisValidatorParams = new ChassisValidatorParams()
                {
                    ChassisNoValidFormat = ChassisNoValidFormat,
                    Size = TxtIdEqSize,
                    Type = TxtIdEqType,
                    chassisValue = GeneralService.ChassisNumber,
                    EqSizeTypeDisChassis = EqSizeTypeDisChassis
                };
                switch (ContainerChassisValidator.ChassisValidation(chassisValidatorParams))
                {
                    case ContainerChassisValidator.ChassisValidationCode.NoChassisValue:
                    case ContainerChassisValidator.ChassisValidationCode.WrongFormat:
                        return false;
                }
            }

            if (EquipmentRequired == FieldRequirementCode.Value.Required)
            {
                if (_txtIdEqType.Equals(MoveCode.EquipmentType.TankCode))
                {
                    if (!DateValidator(TxtDp25yrTestDate, 0)) return false;
                    if (!DateValidator(TxtDp5yrTestDate, 1)) return false;
                }
            }

            //if (string.IsNullOrWhiteSpace(GeneralService.Bobtail) && selService.Equals(MoveCode.MoveType.BobTailCode)) return false;
            return true;
        }

        private bool ContainerValidator()
        {
            var containerResult = string.Empty;
            if (selEquipmentType == MoveCode.DefaultValue.ValuePickerDefault) return false;
            if (selEquipmentSize == MoveCode.DefaultValue.ValuePickerDefault) return false;

            var containerValidatorParams = new ContainerValidatorParams()
            {
                ContainerValue = GeneralService.EquipmentNumber,
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
                    GeneralService.EquipmentNumber = containerResult;
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
                        GeneralService.EquipmentTestDate25Year = isCorrect;
                        break;
                    case 1:
                        GeneralService.EquipmentTestDate5Year = isCorrect;
                        break;
                }
                _serviceRepository.Update(GeneralService);
                response = true;
            }
            return response;
        }

        public void clearTestDates()
        {
            GeneralService.EquipmentTestDate5Year = string.Empty;
            GeneralService.EquipmentTestDate25Year = string.Empty;
            TxtDp5yrTestDate = string.Empty;
            TxtDp25yrTestDate = string.Empty;
        }
        #endregion
        #region VarZone
        private BEService _generalService;
        public BEService GeneralService
        {
            get { return _generalService; }
            set { SetProperty(ref _generalService, value); }
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
        public IEnumerable<EquipmentTypeDTO> LstEquipmentType
        {
            get { return _lstEquipmentType; }
            set { SetProperty(ref _lstEquipmentType, value); }
        }
        private IEnumerable<EquipmentSizeDTO> _lstEquipmentSize;
        private IEnumerable<EquipmentTypeDTO> _lstEquipmentType;
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

        public bool isAnotherService { get; set; }
        public string ProductRequired { get; set; }
        public string BlockRequired { get; set; }
        public string EquipmentRequired { get; set; }
        public string EquipmentNumberRequired { get; set; }
        public string EquipmentSectionRequired { get; set; }
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
        public string FromBlockSeachImage
        {
            get { return _fromBlockSeachImage; }
            set { SetProperty(ref _fromBlockSeachImage, value); }
        }
        #endregion        
    }
}
