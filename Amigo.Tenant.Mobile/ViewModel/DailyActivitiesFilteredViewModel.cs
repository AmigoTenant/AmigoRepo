using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Resource;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class DailyActivitiesFilteredViewModel: TodayViewModel
    {
        private INavigator _navigator;
        private IMoveRepository _moveRepository;
        private IServiceRepository _serviceRepository;
        private IDetentionRepository _detentionRepository;
        private IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private ISessionRepository _sessionRepository;
        const int lineCharLimit = 25, headerCharLimit = 30;

        public static class ActionType
        {
            public static String Move { get { return AppString.lblActionTypeMove; } }
            public static String Service { get { return AppString.lblActionTypeService; } }
            public static String Detention { get { return AppString.lblActionTypeDetention; } }
            public static String OperateTaylorLift { get { return AppString.lblActionTypeTaylorLift; } }
        }

        public class Section : TodayViewModel
        {
            public string HeaderLabel { get; set; }
            public string HeaderDesc { get; set; }
            public string DetailLabel { get; set; }
            public string DetailText { get; set; }
            public string ActionType { get; set; }
            public DateTime CreationDate { get; set; }
            public string AcknowledgeState { get; set; }
            public Guid idSection { get; set; }
        }

        private List<Section> _lstSections;
        public List<Section> LstSections
        {
            get { return _lstSections; }
            set { SetProperty(ref _lstSections, value); }
        }

        private string _headerTimeMsg;
        public string HeaderTimeMsg
        {
            get { return _headerTimeMsg; }
            set { SetProperty(ref _headerTimeMsg, value); }
        }
        private DateTime? currentWorkdayDate, previousWorkdayDate;

        public DailyActivitiesFilteredViewModel(IMoveRepository moveRepository,
                                        IServiceRepository serviceRepository,
                                        IDetentionRepository detentionRepository,
                                        IOperateTaylorLiftRepository operateTaylorLiftRepository,
                                        ISessionRepository sessionRepository,
                                        INavigator navigator)
        {
            _navigator = navigator;
            _moveRepository = moveRepository;
            _serviceRepository = serviceRepository;
            _detentionRepository = detentionRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _sessionRepository = sessionRepository;
        }

        public override async void OnPushed()
        {
            base.OnPushed();
            Title = AppString.titleDailyActivities;
            await LoadData().ConfigureAwait(false);
            //await LoadExperiment().ConfigureAwait(false);
        }

        public ICommand TodayCommand => CreateCommand(() =>
        {
            var lista = LstSections.Where(x => x.CreationDate.Date == currentWorkdayDate.Value.Date).ToList();
            LstDailyActivities = lista.OrderByDescending(x => x.CreationDate).ToList();
            Title = string.Concat(AppString.titleDailyActivities, " - ", LstDailyActivities.Count());
            HeaderTimeMsg = string.Concat(AppString.lblDailyActivitiesCurrentWorkday, ":");
            TextTodayDate = currentWorkdayDate.Value.ToString(DateFormats.CurrentWorkday);
        });

        public ICommand YesterdayCommand => CreateCommand(() =>
        {
            var lista = new List<Section>();
            if (previousWorkdayDate != null)
                lista = LstSections.Where(x => x.CreationDate.Date == previousWorkdayDate.Value.Date).ToList();
            LstDailyActivities = lista.OrderByDescending(x => x.CreationDate).ToList();
            Title = string.Concat(AppString.titleDailyActivities, " - ", LstDailyActivities.Count());
            HeaderTimeMsg = string.Concat(AppString.lblDailyActivitiesPreviousWorkday, ":");
            TextTodayDate = lista.Any() ? previousWorkdayDate.Value.ToString(DateFormats.CurrentWorkday) : "---";
        });

        public ICommand ShowDetail => CreateCommand<Section>( async (block) =>
        {
            await _navigator.PushAsync<DailyActivitiesFilteredItemNewViewModel>(x => {  x.IdItem = block.idSection;
                                                                                        x.ActionType = block.ActionType;
                                                                                        x.LabelTime = HeaderTimeMsg;
                                                                                        x.CurrentTime = TextTodayDate; });
        });

        private async Task LoadData()
        {
            var lstMoves = _moveRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove);
            var lstServices = _serviceRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove);
            var lstDetentions = _detentionRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove);
            var lstTaylorLift = _operateTaylorLiftRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove);

            LstSections = new List<Section>();

            try
            {
                foreach (var move in lstMoves)
                {
                    var StartTime = DateTimeParse(move.ServiceStartDate).HasValue
                        ? DateTimeParse(move.ServiceStartDate).Value.ToString(DateFormats.StandardHHmmss)
                        : AppString.lblMsgNotRegistered;
                    var FinishTime = DateTimeParse(move.ServiceFinishDate).HasValue
                        ? DateTimeParse(move.ServiceFinishDate).Value.ToString(DateFormats.StandardHHmmss)
                        : AppString.lblMsgNotRegistered;
                    var ElapsedTime = string.IsNullOrEmpty(move.ServiceFinishDate)
                        ? AppString.lblDailyIndeterminable
                        : (DateTimeParse(move.ServiceFinishDate).Value.TimeOfDay -
                           DateTimeParse(move.ServiceStartDate).Value.TimeOfDay).ToString(@"hh\:mm\:ss");

                    var EquipmentSize = AssignStringOrDefault(move.EquipmentSizeDesc, AppString.lblMsgNotRegistered);
                    var Chasis = AssignStringOrDefault(move.ChassisNumber, AppString.lblMsgNotRegistered);
                    var EquipmentType = AssignStringOrDefault(move.EquipmentTypeDesc, AppString.lblMsgNotRegistered);
                    var MoveTypeDesc = AppString.lblMoveType;
                    var MoveType = AssignStringOrDefault(move.MoveTypeDesc, AppString.lblMsgNotRegistered);
                    var Status = AssignStringOrDefault(move.EquipmentStatusDesc, AppString.lblMsgNotRegistered);
                    var EquipmentNumber = AssignStringOrDefault(move.EquipmentNumber, AppString.lblMsgNotRegistered);
                    var FromBlock = AssignStringOrDefault(move.StartName, AppString.lblMsgNotRegistered);
                    var ToBlock = AssignStringOrDefault(move.FinishName, AppString.lblMsgNotRegistered);
                    var Product = AssignStringOrDefault(move.ProductDescription, AppString.lblMsgNotRegistered);
                    var Comment = AssignStringOrDefault(move.DriverComments, AppString.lblMsgNotRegistered);

                    var hChargeType = !string.IsNullOrEmpty(move.ShipmentID) ? AppString.lblShipmentId : AppString.lblCostCenter;
                    var hChargeNo = !string.IsNullOrEmpty(move.ShipmentID) ? move.ShipmentID : move.CostCenterName;
                    var hEquipmentType = string.IsNullOrEmpty(move.EquipmentNumber) ? AppString.lblNone : move.EquipmentNumber;
                    var hLocation = string.Concat(FromBlock, "  -> ", ToBlock);

                    LstSections.Add(new Section()
                    {
                        HeaderLabel = addNewTag(hChargeType, hChargeType) +
                                 addNewTag(AppString.lblEqupNo, hEquipmentType) +
                                 addNewTag(AppString.lblBlock, hLocation) +
                                 addNewTag(AppString.lblStartTime, StartTime, true),

                        HeaderDesc = addNewLine(hChargeNo, headerCharLimit) +
                                 addNewLine(hEquipmentType, headerCharLimit) +
                                 addNewLine(hLocation, headerCharLimit) +
                                 addNewLine(StartTime, headerCharLimit, true),

                        DetailLabel = addNewTag(AppString.lblStartTime, StartTime) +
                                       addNewTag(AppString.lblFinishTime, FinishTime) +
                                       addNewTag(EquipmentType, EquipmentNumber) +
                                       addNewTag(AppString.lblChassis, Chasis) +
                                       addNewTag(AppString.lblSize, EquipmentSize) +
                                       addNewTag(MoveTypeDesc, MoveType) +
                                       addNewTag(AppString.lblStatus, Status) +
                                       addNewTag(AppString.lblFromBlock, FromBlock) +
                                       addNewTag(AppString.lblToBlock, ToBlock) +
                                       addNewTag(AppString.lblProduct, Product) +
                                       addNewTag(AppString.lblDriverComments, Comment) +
                                       addNewTag(AppString.lblElapsedTime, ElapsedTime, true),
                        DetailText = addNewLine(StartTime, lineCharLimit) +
                                       addNewLine(FinishTime, lineCharLimit) +
                                       addNewLine(EquipmentNumber, lineCharLimit) +
                                       addNewLine(Chasis, lineCharLimit) +
                                       addNewLine(EquipmentSize, lineCharLimit) +
                                       addNewLine(MoveType, lineCharLimit) +
                                       addNewLine(Status, lineCharLimit) +
                                       addNewLine(FromBlock, lineCharLimit) +
                                       addNewLine(ToBlock, lineCharLimit) +
                                       addNewLine(Product, lineCharLimit) +
                                       addNewLine(Comment, lineCharLimit) +
                                       addNewLine(ElapsedTime, lineCharLimit, true),
                        ActionType = ActionType.Move,
                        CreationDate = DateTimeParse(move.ServiceStartDate).Value,
                        AcknowledgeState = move.AcknowledgeState == AcknowledgeState.Authorized ? AppString.lblDailyActivitiesAuthorized : string.Empty,
                        idSection = move.InternalId,
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Current.LogWarning($"Error loading Moves repository: {ex.ToString()}");
                //await ShowError(ErrorCode.DailyActivitiesMoveRepository, AppString.errorLoadingMoves);
            }

            try
            {
                foreach (var service in lstServices)
                {
                    var StartTime = DateTimeParse(service.ServiceStartDate).HasValue
                        ? DateTimeParse(service.ServiceStartDate).Value.ToString(DateFormats.StandardHHmmss)
                        : AppString.lblMsgNotRegistered;
                    var FinishTime = DateTimeParse(service.ServiceFinishDate).HasValue
                        ? DateTimeParse(service.ServiceFinishDate).Value.ToString(DateFormats.StandardHHmmss)
                        : AppString.lblMsgNotRegistered;
                    var ElapsedTime = string.IsNullOrEmpty(service.ServiceFinishDate)
                        ? AppString.lblDailyIndeterminable
                        : (DateTimeParse(service.ServiceFinishDate).Value.TimeOfDay -
                           DateTimeParse(service.ServiceStartDate).Value.TimeOfDay).ToString(@"hh\:mm\:ss");

                    var EquipmentSize = AssignStringOrDefault(service.EquipmentSizeDesc, AppString.lblMsgNotRegistered);
                    var Chasis = AssignStringOrDefault(service.ChassisNumber, AppString.lblMsgNotRegistered);
                    var EquipmentType = AssignStringOrDefault(service.EquipmentTypeDesc, AppString.lblMsgNotRegistered); 
                    var HasH34 = service.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
                    var MoveTypeDesc = AppString.lblServiceType;
                    var MoveType = AssignStringOrDefault(service.MoveTypeDesc, AppString.lblMsgNotRegistered);
                    var Status = AssignStringOrDefault(service.EquipmentStatusDesc, AppString.lblMsgNotRegistered);
                    var EquipmentNumber = AssignStringOrDefault(service.EquipmentNumber, AppString.lblMsgNotRegistered);
                    var FromBlock = AssignStringOrDefault(service.StartName, AppString.lblMsgNotRegistered);
                    var ToBlock = AssignStringOrDefault(service.FinishName, AppString.lblMsgNotRegistered);
                    var Product = AssignStringOrDefault(service.ProductDescription, AppString.lblMsgNotRegistered);
                    var Comment = AssignStringOrDefault(service.DriverComments, AppString.lblMsgNotRegistered);

                    var hChargeType = !string.IsNullOrEmpty(service.ShipmentID) ? AppString.lblShipmentId : AppString.lblCostCenter;
                    var hChargeNo = !string.IsNullOrEmpty(service.ShipmentID)? service.ShipmentID : service.CostCenterName;
                    var hEquipmentType = string.IsNullOrEmpty(service.EquipmentNumber) ? AppString.lblNone : service.EquipmentNumber;
                    var hLocation = FromBlock;

                    LstSections.Add(new Section()
                    {
                        HeaderLabel = addNewTag(hChargeType, hChargeType) +
                                 addNewTag(AppString.lblEqupNo, hEquipmentType) +
                                 addNewTag(AppString.lblBlock, hLocation) +
                                 addNewTag(AppString.lblStartTime, StartTime, true),

                        HeaderDesc = addNewLine(hChargeNo, headerCharLimit) +
                                 addNewLine(hEquipmentType, headerCharLimit) +
                                 addNewLine(hLocation, headerCharLimit) +
                                 addNewLine(StartTime, headerCharLimit, true),

                        DetailLabel = addNewTag(AppString.lblStartTime, StartTime) +     addNewLine(StartTime, lineCharLimit) +
                                addNewTag(AppString.lblFinishTime, FinishTime) +    addNewLine(FinishTime, lineCharLimit) +
                                addNewTag(EquipmentType, EquipmentNumber) +         addNewLine(EquipmentNumber, lineCharLimit) +
                                addNewTag(AppString.lblChassis, Chasis) +           addNewLine(Chasis, lineCharLimit) +
                                addNewTag(AppString.lblSize, EquipmentSize) +       addNewLine(EquipmentSize, lineCharLimit) +
                                addNewTag(MoveTypeDesc, MoveType) +                 addNewLine(MoveType, lineCharLimit) +
                                addNewTag(AppString.lblStatus, Status) +            addNewLine(Status, lineCharLimit) +
                                addNewTag(AppString.lblH34, HasH34) +               addNewLine(HasH34, lineCharLimit) +
                                addNewTag(AppString.lblFromBlock, FromBlock) +      addNewLine(FromBlock, lineCharLimit) +
                                addNewTag(AppString.lblToBlock, ToBlock) +          addNewLine(ToBlock, lineCharLimit) +
                                addNewTag(AppString.lblProduct, Product) +          addNewLine(Product, lineCharLimit) +
                                addNewTag(AppString.lblDriverComments, Comment) + addNewLine(Comment, lineCharLimit) +
                                addNewTag(AppString.lblElapsedTime, ElapsedTime) +  addNewLine(ElapsedTime, lineCharLimit, true),
                        ActionType = ActionType.Service,
                        CreationDate = DateTimeParse(service.ServiceStartDate).Value,
                        AcknowledgeState =
                            service.AcknowledgeState == AcknowledgeState.Authorized
                                ? AppString.lblDailyActivitiesAuthorized
                                : string.Empty,
                        idSection = service.InternalId,
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Current.LogWarning($"Error loading Services repository: {ex.ToString()}");
                //await ShowError(ErrorCode.DailyActivitiesServiceRepository, AppString.errorLoadingServices);
            }

            try
            {
                foreach (var detention in lstDetentions)
                {
                    var StartTime = DateTimeParse(detention.ServiceStartDate).HasValue
                        ? DateTimeParse(detention.ServiceStartDate).Value.ToString(DateFormats.StandardHHmmss)
                        : AppString.lblMsgNotRegistered;
                    var FinishTime = DateTimeParse(detention.ServiceFinishDate).HasValue
                        ? DateTimeParse(detention.ServiceFinishDate).Value.ToString(DateFormats.StandardHHmmss)
                        : AppString.lblMsgNotRegistered;
                    var ElapsedTime = string.IsNullOrEmpty(detention.ServiceFinishDate)
                        ? AppString.lblDailyIndeterminable
                        : (DateTimeParse(detention.ServiceFinishDate).Value.TimeOfDay -
                           DateTimeParse(detention.ServiceStartDate).Value.TimeOfDay).ToString(@"hh\:mm\:ss");

                    var EquipmentSize = AssignStringOrDefault(detention.EquipmentSizeDesc, AppString.lblMsgNotRegistered);
                    var Chasis = AssignStringOrDefault(detention.ChassisNumber, AppString.lblMsgNotRegistered);
                    var EquipmentType = AssignStringOrDefault(detention.EquipmentTypeDesc, AppString.lblMsgNotRegistered);
                    var HasH34 = detention.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
                    var MoveTypeDesc = AppString.lblServiceType;
                    var MoveType = AssignStringOrDefault(detention.MoveTypeDesc, AppString.lblMsgNotRegistered);
                    var Status = AssignStringOrDefault(detention.EquipmentStatusDesc, AppString.lblMsgNotRegistered);
                    var EquipmentNumber = AssignStringOrDefault(detention.EquipmentNumber, AppString.lblMsgNotRegistered);
                    var FromBlock = AssignStringOrDefault(detention.StartName, AppString.lblMsgNotRegistered);
                    var ToBlock = AssignStringOrDefault(detention.FinishName, AppString.lblMsgNotRegistered);
                    var Product = AssignStringOrDefault(detention.ProductDescription, AppString.lblMsgNotRegistered);
                    var Comment = AssignStringOrDefault(detention.DriverComments, AppString.lblMsgNotRegistered);

                    var hChargeType = !string.IsNullOrEmpty(detention.ShipmentID) ? AppString.lblShipmentId : AppString.lblCostCenter;
                    var hChargeNo = !string.IsNullOrEmpty(detention.ShipmentID) ? detention.ShipmentID : detention.CostCenterName;
                    var hEquipmentType = string.IsNullOrEmpty(detention.EquipmentNumber) ? AppString.lblNone : detention.EquipmentNumber;
                    var hLocation = FromBlock;

                    LstSections.Add(new Section()
                    {
                        HeaderLabel = addNewTag(hChargeType, hChargeType) +
                                 addNewTag(AppString.lblEqupNo, hEquipmentType) +
                                 addNewTag(AppString.lblBlock, hLocation) +
                                 addNewTag(AppString.lblStartTime, StartTime, true),

                        HeaderDesc = addNewLine(hChargeNo, headerCharLimit) +
                                 addNewLine(hEquipmentType, headerCharLimit) +
                                 addNewLine(hLocation, headerCharLimit) +
                                 addNewLine(StartTime, headerCharLimit, true),

                        DetailLabel = addNewTag(AppString.lblStartTime, StartTime) +     addNewLine(StartTime, lineCharLimit) +
                                addNewTag(AppString.lblFinishTime, FinishTime) +    addNewLine(FinishTime, lineCharLimit) +
                                addNewTag(EquipmentType, EquipmentNumber) +         addNewLine(EquipmentNumber, lineCharLimit) +
                                addNewTag(AppString.lblChassis, Chasis) +           addNewLine(Chasis, lineCharLimit) +
                                addNewTag(AppString.lblSize, EquipmentSize) +       addNewLine(EquipmentSize, lineCharLimit) +
                                addNewTag(MoveTypeDesc, MoveType) +                 addNewLine(MoveType, lineCharLimit) +
                                addNewTag(AppString.lblStatus, Status) +            addNewLine(Status, lineCharLimit) +
                                addNewTag(AppString.lblH34, HasH34) +               addNewLine(HasH34, lineCharLimit) +
                                addNewTag(AppString.lblFromBlock, FromBlock) +      addNewLine(FromBlock, lineCharLimit) +
                                addNewTag(AppString.lblToBlock, ToBlock) +          addNewLine(ToBlock, lineCharLimit) +
                                addNewTag(AppString.lblProduct, Product) +          addNewLine(Product, lineCharLimit) +
                                addNewTag(AppString.lblDriverComments, Comment) +   addNewLine(Comment, lineCharLimit) +
                                addNewTag(AppString.lblElapsedTime, ElapsedTime) +  addNewLine(ElapsedTime, lineCharLimit, true),
                        ActionType = ActionType.Detention,
                        CreationDate = DateTimeParse(detention.ServiceStartDate).Value,
                        AcknowledgeState =
                            detention.AcknowledgeState == AcknowledgeState.Authorized
                                ? AppString.lblDailyActivitiesAuthorized
                                : string.Empty,
                        idSection = detention.InternalId,
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Current.LogWarning($"Error loading Detentions repository: {ex.ToString()}");
                //await ShowError(ErrorCode.DailyActivitiesDetentionRepository, AppString.errorLoadingDetentions);
            }

            try
            {
                foreach (var operateTaylorLift in lstTaylorLift)
                {
                    var StartTime = DateTimeParse(operateTaylorLift.ServiceStartDate).HasValue
                        ? DateTimeParse(operateTaylorLift.ServiceStartDate).Value.ToString(DateFormats.StandardHHmmss)
                        : AppString.lblMsgNotRegistered;
                    var FinishTime = DateTimeParse(operateTaylorLift.ServiceFinishDate).HasValue
                        ? DateTimeParse(operateTaylorLift.ServiceFinishDate).Value.ToString(DateFormats.StandardHHmmss)
                        : AppString.lblMsgNotRegistered;
                    var ElapsedTime = string.IsNullOrEmpty(operateTaylorLift.ServiceFinishDate)
                        ? AppString.lblDailyIndeterminable
                        : (DateTimeParse(operateTaylorLift.ServiceFinishDate).Value.TimeOfDay -
                           DateTimeParse(operateTaylorLift.ServiceStartDate).Value.TimeOfDay).ToString(@"hh\:mm\:ss");
                    var EquipmentSize = AssignStringOrDefault(operateTaylorLift.EquipmentSizeDesc, AppString.lblMsgNotRegistered);
                    var Chasis = AssignStringOrDefault(operateTaylorLift.ChassisNumber, AppString.lblMsgNotRegistered);
                    var EquipmentType = AssignStringOrDefault(operateTaylorLift.EquipmentTypeDesc, AppString.lblMsgNotRegistered);
                    var HasH34 = operateTaylorLift.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
                    var MoveTypeDesc = AppString.lblServiceType;
                    var MoveType = AssignStringOrDefault(operateTaylorLift.MoveTypeDesc, AppString.lblMsgNotRegistered);
                    var Status = AssignStringOrDefault(operateTaylorLift.EquipmentStatusDesc, AppString.lblMsgNotRegistered);
                    var EquipmentNumber = AssignStringOrDefault(operateTaylorLift.EquipmentNumber, AppString.lblMsgNotRegistered);
                    var FromBlock = AssignStringOrDefault(operateTaylorLift.StartName, AppString.lblMsgNotRegistered);
                    var ToBlock = AssignStringOrDefault(operateTaylorLift.FinishName, AppString.lblMsgNotRegistered);
                    var Product = AssignStringOrDefault(operateTaylorLift.ProductDescription, AppString.lblMsgNotRegistered);
                    var Comment = AssignStringOrDefault(operateTaylorLift.DriverComments, AppString.lblMsgNotRegistered);

                    var hChargeType = !string.IsNullOrEmpty(operateTaylorLift.ShipmentID) ? AppString.lblShipmentId : AppString.lblCostCenter;
                    var hChargeNo = !string.IsNullOrEmpty(operateTaylorLift.ShipmentID) ? operateTaylorLift.ShipmentID : operateTaylorLift.CostCenterName;
                    var hEquipmentType = string.IsNullOrEmpty(operateTaylorLift.EquipmentNumber) ? AppString.lblNone : operateTaylorLift.EquipmentNumber;
                    var hLocation = FromBlock;


                    LstSections.Add(new Section()
                    {
                        HeaderLabel = addNewTag(hChargeType, hChargeType) +
                                 addNewTag(AppString.lblEqupNo, hEquipmentType) +
                                 addNewTag(AppString.lblBlock, hLocation) +
                                 addNewTag(AppString.lblStartTime, StartTime, true),

                        HeaderDesc = addNewLine(hChargeNo, headerCharLimit) +
                                 addNewLine(hEquipmentType, headerCharLimit) +
                                 addNewLine(hLocation, headerCharLimit) +
                                   addNewLine(StartTime, headerCharLimit, true),

                        DetailLabel = addNewTag(AppString.lblStartTime, StartTime) +     addNewLine(StartTime, lineCharLimit) +
                                addNewTag(AppString.lblFinishTime, FinishTime) +    addNewLine(FinishTime, lineCharLimit) +
                                addNewTag(EquipmentType, EquipmentNumber) +         addNewLine(EquipmentNumber, lineCharLimit) +
                                addNewTag(AppString.lblChassis, Chasis) +           addNewLine(Chasis, lineCharLimit) +
                                addNewTag(AppString.lblSize, EquipmentSize) +       addNewLine(EquipmentSize, lineCharLimit) +
                                addNewTag(MoveTypeDesc, MoveType) +                 addNewLine(MoveType, lineCharLimit) +
                                addNewTag(AppString.lblStatus, Status) +            addNewLine(Status, lineCharLimit) +
                                addNewTag(AppString.lblH34, HasH34) +               addNewLine(HasH34, lineCharLimit) +
                                addNewTag(AppString.lblFromBlock, FromBlock) +      addNewLine(FromBlock, lineCharLimit) +
                                addNewTag(AppString.lblToBlock, ToBlock) +          addNewLine(ToBlock, lineCharLimit) +
                                addNewTag(AppString.lblProduct, Product) +          addNewLine(Product, lineCharLimit) +
                                addNewTag(AppString.lblDriverComments, Comment) + addNewLine(Comment, lineCharLimit) +
                                addNewTag(AppString.lblElapsedTime, ElapsedTime) +  addNewLine(ElapsedTime, lineCharLimit, true),
                        ActionType = ActionType.OperateTaylorLift,
                        CreationDate = DateTimeParse(operateTaylorLift.ServiceStartDate).Value,
                        AcknowledgeState =
                            operateTaylorLift.AcknowledgeState == AcknowledgeState.Authorized
                                ? AppString.lblDailyActivitiesAuthorized
                                : string.Empty,
                        idSection = operateTaylorLift.InternalId,
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Current.LogWarning($"Error loading Taylor Lifts repository: {ex.ToString()}");
                //await ShowError(ErrorCode.DailyActivitiesTaylorLiftRepository, AppString.errorLoadingLifts);
            }

            var session = _sessionRepository.GetSessionObject();

            currentWorkdayDate = string.IsNullOrEmpty(session.LastDateArrivalInfo)
                ? DateTime.Now.Date
                : DateTimeParse(session.LastDateArrivalInfo)?.Date;

            if (LstSections.Any(x => x.CreationDate.Date < currentWorkdayDate.Value.Date))
                previousWorkdayDate = LstSections.Where(x => x.CreationDate.Date < currentWorkdayDate).Max(x => x.CreationDate);

            var lista = LstSections.Where(x => x.CreationDate.Date == currentWorkdayDate.Value.Date).ToList();
            LstDailyActivities = lista.OrderByDescending(x => x.CreationDate).ToList();
            Title = string.Concat(AppString.titleDailyActivities, " - ", LstDailyActivities.Count());
            HeaderTimeMsg = string.Concat(AppString.lblDailyActivitiesCurrentWorkday, ":");
            TextTodayDate = currentWorkdayDate.Value.ToString(DateFormats.CurrentWorkday);
        }

        private string AssignStringOrDefault(string value, string def)
        {
            return string.IsNullOrEmpty(value) ? def : value;
        }
        private string addNewTag(string tagToWrite, string desc, bool lastLine = false)
        {
            return string.IsNullOrEmpty(desc) ? string.Empty : tagToWrite + (lastLine ? string.Empty : Environment.NewLine);
        }
        private string addNewLine(string line, int charLimit, bool lastLine = false)
        {
            line = (string.IsNullOrEmpty(line)) ? string.Empty : line.Length > charLimit ? line.Substring(0, Math.Min(line.Length, charLimit)) + "..." : line;
            return string.IsNullOrEmpty(line) ? string.Empty : line + (lastLine ? string.Empty : Environment.NewLine);
        }
        private DateTime? DateTimeParse(string date)
        {
            if (date == null) return null;
            try
            {
                return DateTime.ParseExact(date, "O", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                ShowError(ErrorCode.DailyActivitiesDateTimeParse, AppString.lblErrorReadingDateTime);
            }
            return null;
        }

        private List<Section> _lstDailyActivities;
        public List<Section> LstDailyActivities
        {
            get { return _lstDailyActivities; }
            set { SetProperty(ref _lstDailyActivities, value); }
        }
    }
}
