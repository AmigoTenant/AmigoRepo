using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Entity;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
using XPO.ShuttleTracking.Mobile.Entity.Service;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Infrastructure.CustomException;

namespace XPO.ShuttleTracking.Mobile.ViewModel.SearchItem
{
    public class ChargeNumberSearchViewModel : TodayViewModel
    {
        private readonly INavigator _navigator;
        private readonly IMoveRepository _moveRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IDetentionRepository _detentionRepository;
        private readonly IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ISessionRepository _sessionRepository;
        private bool _isPerHour;
        private const int LimitRegister = 100;

        public ChargeNumberSearchViewModel(INavigator navigator,
                                        ISessionRepository sessionRepository,
                                        IMoveRepository moveRepository,
                                        IServiceRepository serviceRepository,
                                        IDetentionRepository detentionRepository,
                                        IOperateTaylorLiftRepository operateTaylorLiftRepository,
                                        ILocationRepository locationRepository)
        {
            _navigator = navigator;
            _moveRepository = moveRepository;
            _serviceRepository = serviceRepository;
            _detentionRepository = detentionRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _locationRepository = locationRepository;
            _sessionRepository = sessionRepository;
        }

        private void LoadLocalData()
        {
            var session =  _sessionRepository.GetSessionObject();
            _isPerHour = session.TypeUser == UserTypeCode.PerHour;

            var lstMoves = _moveRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove, LimitRegister).OrderByDescending(x=> StringToDate(x.ServiceStartDate)).ToList();
            var lstServices = _serviceRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove, LimitRegister).OrderByDescending(x => StringToDate(x.ServiceStartDate)).ToList();
            var lstDetentions = _detentionRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove, LimitRegister).OrderByDescending(x => StringToDate(x.ServiceStartDate)).ToList();
            var lstTaylorLift = _operateTaylorLiftRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove, LimitRegister).OrderByDescending(x => StringToDate(x.ServiceStartDate)).ToList();
            
            LstDailyActivities = new List<ActivityHeader>();
            foreach (var move in lstMoves)
            {
                LstDailyActivities.Add(new ActivityHeader()
                {
                    ActionType = ActionType.Move,
                    ChargeType = !string.IsNullOrEmpty(move.ShipmentID) ? AppString.lblShipmentId : AppString.lblCostCenter,
                    ActivityType = string.IsNullOrEmpty(move.MoveTypeDesc) ? string.Empty : move.MoveTypeDesc,
                    StartDate = FormatStartDate(move.ServiceStartDate),
                    ChargeNo = !string.IsNullOrEmpty(move.ShipmentID) ? move.ShipmentID : move.CostCenterName,
                    Chassis = string.IsNullOrEmpty(move.ChassisNumber) ? AppString.lblNone : move.ChassisNumber,
                    EquipmentType = (string.IsNullOrEmpty(move.EquipmentNumber) ? AppString.lblNone : move.EquipmentNumber),
                    FromBlock = string.IsNullOrEmpty(move.Start) ? string.Empty : _locationRepository.GetLocationById(move.Start)?.Name,
                    ToBlock = string.IsNullOrEmpty(move.Finish) ? string.Empty : _locationRepository.GetLocationById(move.Finish)?.Name,
                    ShuttleTServiceId = move.MoveId,
                    Activity = move
                });
            }

            foreach (var service in lstServices)
            {
                LstDailyActivities.Add(new ActivityHeader()
                {
                    ActionType = ActionType.Service,
                    ChargeType = !string.IsNullOrEmpty(service.ShipmentID)? AppString.lblShipmentId: AppString.lblCostCenter,
                    ActivityType = string.IsNullOrEmpty(service.MoveTypeDesc) ? string.Empty : service.MoveTypeDesc,
                    StartDate = FormatStartDate(service.ServiceStartDate),
                    ChargeNo = !string.IsNullOrEmpty(service.ShipmentID)? service.ShipmentID : service.CostCenterName,
                    Chassis = string.IsNullOrEmpty(service.ChassisNumber) ? AppString.lblNone : service.ChassisNumber,
                    EquipmentType = string.IsNullOrEmpty(service.EquipmentNumber) ? AppString.lblNone : service.EquipmentNumber,
                    FromBlock = string.IsNullOrEmpty(service.Start) ? string.Empty : _locationRepository.GetLocationById(service.Start)?.Name,
                    ToBlock = string.IsNullOrEmpty(service.Finish)? string.Empty: _locationRepository.GetLocationById(service.Finish)?.Name,
                    ShuttleTServiceId = service.ServiceId,

                    Activity = service
                });
            }

            foreach (var detention in lstDetentions)
            {
                LstDailyActivities.Add(new ActivityHeader()
                {
                    ActionType = ActionType.Detention,
                    ChargeType = !string.IsNullOrEmpty(detention.ShipmentID) ? AppString.lblShipmentId : AppString.lblCostCenter,
                    ActivityType = string.IsNullOrEmpty(detention.MoveTypeDesc) ? string.Empty : detention.MoveTypeDesc,
                    StartDate = FormatStartDate(detention.ServiceStartDate),
                    ChargeNo = !string.IsNullOrEmpty(detention.ShipmentID) ? detention.ShipmentID : detention.CostCenterName,
                    Chassis = string.IsNullOrEmpty(detention.ChassisNumber) ? AppString.lblNone : detention.ChassisNumber,
                    EquipmentType = string.IsNullOrEmpty(detention.EquipmentNumber) ? AppString.lblNone : detention.EquipmentNumber,
                    FromBlock = string.IsNullOrEmpty(detention.Start)? string.Empty : _locationRepository.GetLocationById(detention.Start)?.Name,
                    ToBlock = string.Empty,
                    ShuttleTServiceId = detention.DetentionId,

                    Activity = detention
                });
            }

            foreach (var operateTaylorLift in lstTaylorLift)
            {
                LstDailyActivities.Add(new ActivityHeader()
                {
                    ActionType = ActionType.OperateTaylorLift,
                    ChargeType = !string.IsNullOrEmpty(operateTaylorLift.ShipmentID) ? AppString.lblShipmentId : AppString.lblCostCenter,
                    ActivityType = string.IsNullOrEmpty(operateTaylorLift.MoveTypeDesc) ? string.Empty : operateTaylorLift.MoveTypeDesc,
                    StartDate = FormatStartDate(operateTaylorLift.ServiceStartDate),
                    ChargeNo = !string.IsNullOrEmpty(operateTaylorLift.ShipmentID) ? operateTaylorLift.ShipmentID : operateTaylorLift.CostCenterName,
                    Chassis = string.IsNullOrEmpty(operateTaylorLift.ChassisNumber) ? AppString.lblNone : operateTaylorLift.ChassisNumber,
                    EquipmentType = string.IsNullOrEmpty(operateTaylorLift.EquipmentNumber) ? AppString.lblNone : operateTaylorLift.EquipmentNumber,
                    FromBlock = string.IsNullOrEmpty(operateTaylorLift.Start) ? string.Empty : _locationRepository.GetLocationById(operateTaylorLift.Start??"")?.Name,
                    ToBlock = string.Empty,
                    ShuttleTServiceId = operateTaylorLift.OperateTaylorLiftId,

                    Activity = operateTaylorLift
                });
            }
            LstDailyActivities = LstDailyActivities.OrderByDescending(x => StringToDate(x.Activity.ServiceStartDate)).ToList();
            LstFilteredDailyActivities = LstDailyActivities;
        }

        private void LoadWebServiceData(BeChargeNumber chargeNumber)
        {
            //TODO:Descartar repetidos por MoveID
            var lstMoves = chargeNumber.ListMove;
            var lstServices = chargeNumber.ListService;
            var lstDetentions = chargeNumber.ListDetention;
            var lstTaylorLift = chargeNumber.ListOperateTaylorLift;
            
            foreach (var move in lstMoves)
            {
                var chargeNo = !string.IsNullOrEmpty(move.ShipmentID) ? move.ShipmentID : move.CostCenterName;
                if (LstFilteredDailyActivities.All(x => x.ShuttleTServiceId != move.MoveId)) //Descartar repetidos
                {
                    LstFilteredDailyActivities.Add(new ActivityHeader()
                    {
                        ActionType = ActionType.Move,
                        ChargeType = !string.IsNullOrEmpty(move.ShipmentID) ? AppString.lblShipmentId : AppString.lblCostCenter,
                        ActivityType = string.IsNullOrEmpty(move.MoveTypeDesc) ? string.Empty : move.MoveTypeDesc,
                        StartDate = FormatStartDate(move.ServiceStartDate),
                        ChargeNo = chargeNo,
                        Chassis = string.IsNullOrEmpty(move.ChassisNumber) ? AppString.lblNone : move.ChassisNumber,
                        EquipmentType = (string.IsNullOrEmpty(move.EquipmentNumber) ? AppString.lblNone : move.EquipmentNumber),
                        FromBlock = string.IsNullOrEmpty(move.Start) ? string.Empty : _locationRepository.GetLocationById(move.Start)?.Name,
                        ToBlock = string.IsNullOrEmpty(move.Finish) ? string.Empty : _locationRepository.GetLocationById(move.Finish)?.Name,
                        ShuttleTServiceId = move.MoveId,

                        Activity = move
                    });
                }
            }

            foreach (var service in lstServices)
            {
                var chargeNo = !string.IsNullOrEmpty(service.ShipmentID) ? service.ShipmentID : service.CostCenterName;
                if (LstFilteredDailyActivities.All(x => x.ShuttleTServiceId != service.ServiceId)) //Descartar repetidos
                {
                    LstFilteredDailyActivities.Add(new ActivityHeader()
                    {
                        ActionType = ActionType.Service,
                        ChargeType = !string.IsNullOrEmpty(service.ShipmentID)? AppString.lblShipmentId: AppString.lblCostCenter,
                        ActivityType = string.IsNullOrEmpty(service.MoveTypeDesc) ? string.Empty : service.MoveTypeDesc,
                        StartDate = FormatStartDate(service.ServiceStartDate),
                        ChargeNo = chargeNo,
                        Chassis = string.IsNullOrEmpty(service.ChassisNumber) ? AppString.lblNone : service.ChassisNumber,
                        EquipmentType = string.IsNullOrEmpty(service.EquipmentNumber) ? AppString.lblNone : service.EquipmentNumber,
                        FromBlock = string.IsNullOrEmpty(service.Start)? string.Empty: _locationRepository.GetLocationById(service.Start)?.Name,
                        ToBlock = string.IsNullOrEmpty(service.Finish)? string.Empty: _locationRepository.GetLocationById(service.Finish)?.Name,
                        ShuttleTServiceId = service.ServiceId,

                        Activity = service
                    });
                }
            }

            foreach (var detention in lstDetentions)
            {
                var chargeNo = !string.IsNullOrEmpty(detention.ShipmentID)? detention.ShipmentID: detention.CostCenterName;
                if (LstFilteredDailyActivities.All(x => x.ShuttleTServiceId != detention.DetentionId)) //Descartar repetidos
                {
                    LstFilteredDailyActivities.Add(new ActivityHeader()
                    {
                        ActionType = ActionType.Detention,
                        ChargeType = !string.IsNullOrEmpty(detention.ShipmentID)? AppString.lblShipmentId: AppString.lblCostCenter,
                        ActivityType = string.IsNullOrEmpty(detention.MoveTypeDesc) ? string.Empty : detention.MoveTypeDesc,
                        StartDate = FormatStartDate(detention.ServiceStartDate),
                        ChargeNo = chargeNo,
                        Chassis = string.IsNullOrEmpty(detention.ChassisNumber) ? AppString.lblNone : detention.ChassisNumber,
                        EquipmentType = string.IsNullOrEmpty(detention.EquipmentNumber)? AppString.lblNone: detention.EquipmentNumber,
                        FromBlock = string.IsNullOrEmpty(detention.Start)? string.Empty: _locationRepository.GetLocationById(detention.Start)?.Name,
                        ToBlock = string.Empty,
                        ShuttleTServiceId = detention.DetentionId,

                        Activity = detention
                    });
                }
            }

            foreach (var operateTaylorLift in lstTaylorLift)
            {
                var chargeNo = !string.IsNullOrEmpty(operateTaylorLift.ShipmentID)? operateTaylorLift.ShipmentID: operateTaylorLift.CostCenterName;
                if (LstFilteredDailyActivities.All(x => x.ShuttleTServiceId != operateTaylorLift.OperateTaylorLiftId)) //Descartar repetidos
                {                                                                                      
                    LstFilteredDailyActivities.Add(new ActivityHeader()
                    {
                        ActionType = ActionType.OperateTaylorLift,
                        ChargeType = !string.IsNullOrEmpty(operateTaylorLift.ShipmentID) ? AppString.lblShipmentId : AppString.lblCostCenter,
                        ActivityType = string.IsNullOrEmpty(operateTaylorLift.MoveTypeDesc) ? string.Empty : operateTaylorLift.MoveTypeDesc,
                        StartDate = FormatStartDate(operateTaylorLift.ServiceStartDate),
                        ChargeNo = chargeNo,
                        Chassis = string.IsNullOrEmpty(operateTaylorLift.ChassisNumber) ? AppString.lblNone : operateTaylorLift.ChassisNumber,
                        EquipmentType = string.IsNullOrEmpty(operateTaylorLift.EquipmentNumber) ? AppString.lblNone : operateTaylorLift.EquipmentNumber,
                        FromBlock = string.IsNullOrEmpty(operateTaylorLift.Start) ? string.Empty : _locationRepository.GetLocationById(operateTaylorLift.Start??string.Empty)?.Name,
                        ToBlock = string.Empty,
                        ShuttleTServiceId = operateTaylorLift.OperateTaylorLiftId,

                        Activity = operateTaylorLift
                    });
                }
            }
            
        }
        public override void OnPushed()
        {
            base.OnPushed();
            this.ChargeNumber = string.Empty;
            LstFilteredDailyActivities = new List<ActivityHeader>() { new ActivityHeader() };
            Task.Run(() =>
            {
                IsLoading = true;

                LoadLocalData();

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
                LstFilteredDailyActivities = LstDailyActivities;

                IsLoading = false;
            });
        }
        private Task<ResponseDTO<BeChargeNumber>> SearchActivities(string chargeNumber)
        {
            var task = new ChargeNumberSearchTaskDefinition()
            {
                Page = 1,
                PageSize = 20,
                ChargeNumber = chargeNumber,
            };

            return TaskManager.Current.ExecuteTaskAsync<ResponseDTO<BeChargeNumber>>(task);
        }

        #region Command

        public ICommand ChooseCommand => CreateCommand(async (ActivityHeader activity) =>
        {
            var optionslabel = this.LstContinueWith.Select(x => x.Name).ToArray();
            var option = await UserDialogs.Instance.ActionSheetAsync(AppString.lblSearchCreateA, AppString.btnDialogCancel,null,cancelToken: null,buttons: optionslabel);

            SelectedActivity = activity;

            if (option == AppString.OptionMove)
            {
                await GetMove();
            }
            else if (option== AppString.OptionService)
            {
                await GetService();
            }
            else if (option == AppString.OptionDetention)
            {
                await GetDetention();

            }
            else if (option == AppString.OptionTaylor)
            {
                await GetOperateTaylorLift();
            }           
        });
        public ICommand OnChargeNumberTextChanged => CreateCommand(() =>
        {
            if (LstFilteredDailyActivities == null || LstDailyActivities == null) return;
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () => { 
            try
            {
                if (string.IsNullOrWhiteSpace(ChargeNumber))
                    LstFilteredDailyActivities = LstDailyActivities;
                else if (ChargeNumber.Length >= 3)
                {
                    using (StartLoading(AppString.lblDialogSearching))
                    {
                        //Call web service
                        LstFilteredDailyActivities =
                            LstDailyActivities.FindAll(
                                x =>
                                    x.ChargeNo.ToUpper().StartsWith(ChargeNumber.ToUpper()) ||
                                    x.EquipmentType.ToUpper().StartsWith(ChargeNumber.ToUpper()) ||
                                    x.Chassis.ToUpper().StartsWith(ChargeNumber.ToUpper()));
                        var result = await SearchActivities(ChargeNumber.ToUpper());
                        if (result.IsValid)
                        {
                            if (result.Data.ListMove.Any() || result.Data.ListService.Any() ||
                                result.Data.ListDetention.Any() || result.Data.ListOperateTaylorLift.Any())
                                LoadWebServiceData(result.Data);
                        }
                            LstFilteredDailyActivities = LstFilteredDailyActivities.OrderByDescending(x => StringToDate(x.Activity.ServiceStartDate)).ToList();
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                await ShowOkAlert(AppString.lblAppName, ex.Message);
            }
            catch (ConnectivityException)
            {
                await ShowOkAlert(AppString.lblAppName, Infrastructure.Resources.StringResources.ConnectiviyError);
            }
            catch (Exception ex)
            {
                await ShowError(ErrorCode.ChargeNumberSearchTextChanged, ex.Message);
            }
            });
        });

        private DateTime StringToDate(string text)
        {
            try
            {
                return text == null ? new DateTime() : DateTime.ParseExact(text, "O", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Logger.Current.LogError($"SearchChargeNumber:{ex}");
                return new DateTime();
            }
        }
        private string FormatStartDate(string date)
        {
            var startTime = string.Empty;
            try
            {
                startTime = DateTime.ParseExact(date, "O", CultureInfo.InvariantCulture, DateTimeStyles.None).ToString(DateFormats.MasterDataFormat);
            }
            catch
            {
                // ignored
            }
            return startTime;
        }
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
        #endregion

        #region getActivities
        private async Task GetMove()
        {
            _navigator.ClearNavigationStackToRoot(removePersistentPages:true);

            var generalMove = CopyFromActivity<BEMove>();

            _moveRepository.Add(generalMove);

            await _navigator.PushAsync<RegisterMoveViewModel>(x =>
            {
                x.GeneralMove = generalMove;
                x.isAnotherMove = true;
            });
            //Destroy();
            _navigator.ClearNavigationStackToRoot();
        }

        private void Destroy()
        {
            LstFilteredDailyActivities = null;
            LstDailyActivities = null;
            LstContinueWith = null;
            SelectedActivity = null;
        }

        private async Task GetService()
        {
            _navigator.ClearNavigationStackToRoot(removePersistentPages: true);

            var generalService = CopyFromActivity<BEService>();

            _serviceRepository.Add(generalService);            

            await _navigator.PushAsync<RegisterAdditionalServiceViewModel>(x =>
            {
                x.GeneralService = generalService;
                x.isAnotherService = true;
            });

            _navigator.ClearNavigationStackToRoot();
        }        

        private async Task GetDetention()
        {
            _navigator.ClearNavigationStackToRoot(removePersistentPages:true);

            var generalDetention = CopyFromActivity<BEDetention>();

            _detentionRepository.Add(generalDetention);

            await _navigator.PushAsync<RegisterDetentionViewModel>(x => 
            {
                x.GeneralDetention = generalDetention;
                x.isAnotherDetention = true;
            });

            _navigator.ClearNavigationStackToRoot();
        }
        private async Task GetOperateTaylorLift()
        {
            _navigator.ClearNavigationStackToRoot(removePersistentPages: true);

            var generalOperateTaylorLift = CopyFromActivity<BEOperateTaylorLift>();

            generalOperateTaylorLift.Clear();

            _operateTaylorLiftRepository.Add(generalOperateTaylorLift);            

            await _navigator.PushAsync<RegisterOperateTaylorLiftViewModel>(x =>
            {
                x.GeneralOperateTaylorLift = generalOperateTaylorLift;
                x.isAnotherOperateTaylorLift = true;
            });

            _navigator.ClearNavigationStackToRoot();
        }

        private T CopyFromActivity<T>() where T : BEServiceBase, new()
        {
            var lstLocations = _locationRepository.GetAll();
            var activity = SelectedActivity.Activity;
            var action = SelectedActivity.ActionType;
            var start = action == ActionType.Move ? activity.Finish : activity.Start;

            var move = new T
            {
                InternalId = Guid.NewGuid(),
                CurrentState = MoveState.CreatedMove,
                IsModified = true,
                ShipmentID = activity.ShipmentID,
                CostCenter = activity.CostCenter,
                CostCenterName = activity.CostCenterName,
                DispatchingParty = activity.DispatchingParty,
                EquipmentNumber = activity.EquipmentNumber,
                EquipmentType = activity.EquipmentType,
                EquipmentSize = activity.EquipmentSize,
                ChassisNumber = activity.ChassisNumber,
                EquipmentTestDate25Year = activity.EquipmentTestDate25Year,
                EquipmentTestDate5Year = activity.EquipmentTestDate5Year,
                Product = activity.Product,
                ProductDescription = activity.ProductDescription,
                Start = action == ActionType.Move ? activity.Finish : activity.Start,
                StartName = lstLocations.FirstOrDefault(x => x.LocationId.ToString() == start)?.Name,
                FinishName = string.Empty,
                Finish = string.Empty,
                HasH34 = false,
                EquipmentStatus = MoveCode.DefaultValue.ValuePickerDefault,
                Service = MoveCode.DefaultValue.ValuePickerDefault,
                ServiceFinishDate = string.Empty,
                ServiceFinishDateUTC = string.Empty,
                ServiceFinishDateTZ = string.Empty,
                ServiceStartDate = string.Empty,
                ServiceStartDateUTC = string.Empty,
                ServiceStartDateTZ = string.Empty
            };
            
            return move;
        }
        #endregion

        #region Variables
        private string _chargeNumber;
        public string ChargeNumber
        {
            get { return _chargeNumber; }
            set { SetProperty(ref _chargeNumber, value); }
        }

        private ActivityHeader _selectedActivity;
        public ActivityHeader SelectedActivity
        {
            get { return _selectedActivity; }
            set { SetProperty(ref _selectedActivity, value); }
        }

        private List<ActivityHeader> _lstDailyActivities;
        public List<ActivityHeader> LstDailyActivities
        {
            get { return _lstDailyActivities; }
            set { SetProperty(ref _lstDailyActivities, value); }
        }

        private List<ActivityHeader> _lstFilteredDailyActivities;
        public List<ActivityHeader> LstFilteredDailyActivities
        {
            get { return _lstFilteredDailyActivities; }
            set { SetProperty(ref _lstFilteredDailyActivities, value); }
        }
        private IEnumerable<BasePicker> _lstContinueWith;
        public IEnumerable<BasePicker> LstContinueWith
        {
            get { return _lstContinueWith; }
            set { SetProperty(ref _lstContinueWith, value); }
        }

        private int _selContinueWith;
        public int SelContinueWith
        {
            get { return _selContinueWith; }
            set
            {
                SetProperty(ref _selContinueWith, value);                
            }
        }
        private enum CodeType : int
        {
            Move = 1, Service, Detention, Taylor
        }

        #endregion
    }
}
