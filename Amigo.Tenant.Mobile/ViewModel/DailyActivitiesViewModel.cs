using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Entity.DailyActivities;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Resource;
using System.Windows.Input;
using TSI.Xamarin.Forms.Logging;
using Logger = XPO.ShuttleTracking.Mobile.Infrastructure.Logger;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class DailyActivitiesViewModel : TodayViewModel
    {
        private IMoveRepository _moveRepository;
        private IServiceRepository _serviceRepository;
        private IDetentionRepository _detentionRepository;
        private IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private ISessionRepository _sessionRepository;

        const int lineCharLimit = 25, headerCharLimit = 30;

        public class ActivityDetail : TodayViewModel
        {
            public String Tags { get; set; }
            public String Desc { get; set; }
            public String CommentTags { get; set; }
            public String CommentDesc { get; set; }
            //public String StartTime { get; set; }
            //public String FinishTime { get; set; }
            //public String ElapsedTime { get; set; }
            //public String Chasis { get; set; }
            //public String EquipmentSize { get; set; }
            //public String MoveType { get; set; }
            //public String Status { get; set; }
            //public String EquipmentNumber { get; set; }
            //public String EquipmentType { get; set; }
            //public String HasH34 { get; set; }
            //public String MoveTypeDesc { get; set; }
            //public String FromBlock { get; set; }
            //public String ToBlock { get; set; }
            //public String Product { get; set; }
            //public String DispatchingParty { get; set; }
        }

        public class Section : TodayViewModel
        {
            public string Header { get; set; }
            public string ChargeNo { get; set; }
            public string EquipmentType { get; set; }
            public string ActionType { get; set; }
            public string Location { get; set; }
            public DateTime CreationDate { get; set; }
            public string AcknowledgeState { get; set; }
            public IEnumerable<ActivityDetail> List { get; set; }
        }

        public class ViewModel : TodayViewModel
        {
            public IEnumerable<Section> List { get; set; }
        }

        public DailyActivitiesViewModel(IMoveRepository moveRepository,
                                        IServiceRepository serviceRepository,
                                        IDetentionRepository detentionRepository,
                                        IOperateTaylorLiftRepository operateTaylorLiftRepository,
                                        ISessionRepository sessionRepository)
        {
            _moveRepository = moveRepository;
            _serviceRepository = serviceRepository;
            _detentionRepository = detentionRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _sessionRepository = sessionRepository;

            LstMoves = new ViewModel() { List = new List<Section>() { new Section() } };
            LstDailyActivities = new List<BEDailyActivities>() { new BEDailyActivities() };
        }

        public override async void OnPushed()
        {
            base.OnPushed();

            await LoadData().ConfigureAwait(false);
            //await LoadExperiment().ConfigureAwait(false);
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
        private async Task LoadData()
        {
            var lstMoves = _moveRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove, 40);
            var lstServices = _serviceRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove, 40);
            var lstDetentions = _detentionRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove, 40);
            var lstTaylorLift = _operateTaylorLiftRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove, 40);

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
                    var EquipmentSize = move.EquipmentSizeDesc;
                    var Chasis = move.ChassisNumber;
                    var EquipmentType = move.EquipmentTypeDesc;
                    var MoveTypeDesc = AppString.lblMoveType;
                    var MoveType = move.MoveTypeDesc;
                    var Status = move.EquipmentStatusDesc;
                    var EquipmentNumber = move.EquipmentNumber;
                    var FromBlock = move.StartName;
                    var ToBlock = move.FinishName;
                    var Product = move.ProductDescription;
                    var Comment = move.DriverComments;

                    var hChargeNo = !string.IsNullOrEmpty(move.ShipmentID)
                        ? string.Concat(AppString.lblShipmentId, ": ", move.ShipmentID)
                        : string.Concat(AppString.lblCostCenter, ": ", move.CostCenterName);
                    var hEquipmentType = string.Concat(AppString.lblEqupNo, ": ",
                        (string.IsNullOrEmpty(move.EquipmentNumber) ? AppString.lblNone : move.EquipmentNumber));
                    var hLocation = string.Concat(AppString.lblBlock, ": ", move.StartName, "  -> ", move.FinishName);
                    LstSections.Add(new Section()
                    {
                        Header =
                            addNewLine(hChargeNo, headerCharLimit) + addNewLine(hEquipmentType, headerCharLimit) +
                            addNewLine(hLocation, headerCharLimit, true),
                        ActionType = ActionType.Move,
                        CreationDate = DateTimeParse(move.ServiceStartDate).Value,
                        AcknowledgeState =
                            move.AcknowledgeState == AcknowledgeState.Authorized
                                ? AppString.lblDailyActivitiesAuthorized
                                : string.Empty,


                        List = new List<ActivityDetail>()
                        {
                            new ActivityDetail()
                            {
                                Tags = addNewTag(AppString.lblStartTime, StartTime) +
                                       addNewTag(AppString.lblFinishTime, FinishTime) +
                                       addNewTag(EquipmentType, EquipmentNumber) +
                                       addNewTag(AppString.lblChassis, Chasis) +
                                       addNewTag(AppString.lblSize, EquipmentSize) +
                                       addNewTag(MoveTypeDesc, MoveType) +
                                       addNewTag(AppString.lblStatus, Status) +
                                       addNewTag(AppString.lblFromBlock, FromBlock) +
                                       addNewTag(AppString.lblToBlock, ToBlock) +
                                       addNewTag(AppString.lblProduct, Product) +
                                       addNewTag(AppString.lblElapsedTime, ElapsedTime, true),

                                Desc = addNewLine(StartTime, lineCharLimit) +
                                       addNewLine(FinishTime, lineCharLimit) +
                                       addNewLine(EquipmentNumber, lineCharLimit) +
                                       addNewLine(Chasis, lineCharLimit) +
                                       addNewLine(EquipmentSize, lineCharLimit) +
                                       addNewLine(MoveType, lineCharLimit) +
                                       addNewLine(Status, lineCharLimit) +
                                       addNewLine(FromBlock, lineCharLimit) +
                                       addNewLine(ToBlock, lineCharLimit) +
                                       addNewLine(Product, lineCharLimit) +
                                       addNewLine(ElapsedTime, lineCharLimit, true),
                                CommentTags = AppString.lblDriverComments,
                                CommentDesc = Comment
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Current.LogWarning($"Error loading Moves repository: {ex.ToString()}");
                await ShowError(ErrorCode.DailyActivitiesMoveRepository, AppString.errorLoadingMoves);
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
                    var EquipmentSize = service.EquipmentSizeDesc;
                    var Chasis = service.ChassisNumber;
                    var EquipmentType = service.EquipmentTypeDesc;
                    var HasH34 = service.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
                    var MoveTypeDesc = AppString.lblServiceType;
                    var MoveType = service.MoveTypeDesc;
                    var Status = service.EquipmentStatusDesc;
                    var EquipmentNumber = service.EquipmentNumber;
                    var FromBlock = service.StartName;
                    var ToBlock = service.FinishName;
                    var Product = service.ProductDescription;
                    var Comment = service.DriverComments;

                    var hChargeNo = !string.IsNullOrEmpty(service.ShipmentID)
                        ? string.Concat(AppString.lblShipmentId, ": ", service.ShipmentID)
                        : string.Concat(AppString.lblCostCenter, ": ", service.CostCenterName);
                    var hEquipmentType = string.Concat(AppString.lblEqupNo, ": ",
                        (string.IsNullOrEmpty(service.EquipmentNumber) ? AppString.lblNone : service.EquipmentNumber));
                    var hLocation = string.Concat(AppString.lblBlock, ": ", service.StartName);

                    LstSections.Add(new Section()
                    {
                        Header =
                            addNewLine(hChargeNo, headerCharLimit) + addNewLine(hEquipmentType, headerCharLimit) +
                            addNewLine(hLocation, headerCharLimit, true),

                        ActionType = ActionType.Service,
                        CreationDate = DateTimeParse(service.ServiceStartDate).Value,
                        AcknowledgeState =
                            service.AcknowledgeState == AcknowledgeState.Authorized
                                ? AppString.lblDailyActivitiesAuthorized
                                : string.Empty,
                        List = new List<ActivityDetail>()
                        {
                            new ActivityDetail()
                            {
                                Tags = addNewTag(AppString.lblStartTime, StartTime) +
                                       addNewTag(AppString.lblFinishTime, FinishTime) +
                                       addNewTag(EquipmentType, EquipmentNumber) +
                                       addNewTag(AppString.lblChassis, Chasis) +
                                       addNewTag(AppString.lblSize, EquipmentSize) +
                                       addNewTag(MoveTypeDesc, MoveType) +
                                       addNewTag(AppString.lblStatus, Status) +
                                       addNewTag(AppString.lblH34, HasH34) +
                                       addNewTag(AppString.lblFromBlock, FromBlock) +
                                       addNewTag(AppString.lblToBlock, ToBlock) +
                                       addNewTag(AppString.lblProduct, Product) +
                                       addNewTag(AppString.lblElapsedTime, ElapsedTime, true),

                                Desc = addNewLine(StartTime, lineCharLimit) +
                                       addNewLine(FinishTime, lineCharLimit) +
                                       addNewLine(EquipmentNumber, lineCharLimit) +
                                       addNewLine(Chasis, lineCharLimit) +
                                       addNewLine(EquipmentSize, lineCharLimit) +
                                       addNewLine(MoveType, lineCharLimit) +
                                       addNewLine(Status, lineCharLimit) +
                                       addNewLine(HasH34, lineCharLimit) +
                                       addNewLine(FromBlock, lineCharLimit) +
                                       addNewLine(ToBlock, lineCharLimit) +
                                       addNewLine(Product, lineCharLimit) +
                                       addNewLine(ElapsedTime, lineCharLimit, true),
                                CommentTags = AppString.lblDriverComments,
                                CommentDesc = Comment
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Current.LogWarning($"Error loading Services repository: {ex.ToString()}");
                await ShowError(ErrorCode.DailyActivitiesServiceRepository, AppString.errorLoadingServices);
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
                    var EquipmentSize = detention.EquipmentSizeDesc;
                    var Chasis = detention.ChassisNumber;
                    var EquipmentType = detention.EquipmentTypeDesc;
                    var HasH34 = detention.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
                    var MoveTypeDesc = AppString.lblServiceType;
                    var MoveType = detention.MoveTypeDesc;
                    var Status = detention.EquipmentStatusDesc;
                    var EquipmentNumber = detention.EquipmentNumber;
                    var FromBlock = detention.StartName;
                    var ToBlock = detention.FinishName;
                    var Product = detention.ProductDescription;
                    var Comment = detention.DriverComments;

                    var hChargeNo = !string.IsNullOrEmpty(detention.ShipmentID)
                        ? string.Concat(AppString.lblShipmentId, ": ", detention.ShipmentID)
                        : string.Concat(AppString.lblCostCenter, ": ", detention.CostCenterName);
                    var hEquipmentType = string.Concat(AppString.lblEqupNo, ": ",
                        (string.IsNullOrEmpty(detention.EquipmentNumber) ? AppString.lblNone : detention.EquipmentNumber));
                    var hLocation = string.Concat(AppString.lblBlock, ": ", detention.StartName);
                    LstSections.Add(new Section()
                    {
                        Header =
                            addNewLine(hChargeNo, headerCharLimit) + addNewLine(hEquipmentType, headerCharLimit) +
                            addNewLine(hLocation, headerCharLimit, true),

                        ActionType = ActionType.Detention,
                        CreationDate = DateTimeParse(detention.ServiceStartDate).Value,
                        AcknowledgeState =
                            detention.AcknowledgeState == AcknowledgeState.Authorized
                                ? AppString.lblDailyActivitiesAuthorized
                                : string.Empty,
                        List = new List<ActivityDetail>()
                        {
                            new ActivityDetail()
                            {
                                Tags = addNewTag(AppString.lblStartTime, StartTime) +
                                       addNewTag(AppString.lblFinishTime, FinishTime) +
                                       addNewTag(EquipmentType, EquipmentNumber) +
                                       addNewTag(AppString.lblChassis, Chasis) +
                                       addNewTag(AppString.lblSize, EquipmentSize) +
                                       addNewTag(MoveTypeDesc, MoveType) +
                                       addNewTag(AppString.lblStatus, Status) +
                                       addNewTag(AppString.lblH34, HasH34) +
                                       addNewTag(AppString.lblFromBlock, FromBlock) +
                                       addNewTag(AppString.lblToBlock, ToBlock) +
                                       addNewTag(AppString.lblProduct, Product) +
                                       addNewTag(AppString.lblElapsedTime, ElapsedTime, true),

                                Desc = addNewLine(StartTime, lineCharLimit) +
                                       addNewLine(FinishTime, lineCharLimit) +
                                       addNewLine(EquipmentNumber, lineCharLimit) +
                                       addNewLine(Chasis, lineCharLimit) +
                                       addNewLine(EquipmentSize, lineCharLimit) +
                                       addNewLine(MoveType, lineCharLimit) +
                                       addNewLine(Status, lineCharLimit) +
                                       addNewLine(HasH34, lineCharLimit) +
                                       addNewLine(FromBlock, lineCharLimit) +
                                       addNewLine(ToBlock, lineCharLimit) +
                                       addNewLine(Product, lineCharLimit) +
                                       addNewLine(ElapsedTime, lineCharLimit, true),
                                CommentTags = AppString.lblDriverComments,
                                CommentDesc = Comment
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Current.LogWarning($"Error loading Detentions repository: {ex.ToString()}");
                await ShowError(ErrorCode.DailyActivitiesDetentionRepository, AppString.errorLoadingDetentions);
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
                    var EquipmentSize = operateTaylorLift.EquipmentSizeDesc;
                    var Chasis = operateTaylorLift.ChassisNumber;
                    var EquipmentType = operateTaylorLift.EquipmentTypeDesc;
                    var HasH34 = operateTaylorLift.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
                    var MoveTypeDesc = AppString.lblServiceType;
                    var MoveType = operateTaylorLift.MoveTypeDesc;
                    var Status = operateTaylorLift.EquipmentStatusDesc;
                    var EquipmentNumber = operateTaylorLift.EquipmentNumber;
                    var FromBlock = operateTaylorLift.StartName;
                    var ToBlock = operateTaylorLift.FinishName;
                    var Product = operateTaylorLift.ProductDescription;
                    var Comment = operateTaylorLift.DriverComments;

                    var hChargeNo = !string.IsNullOrEmpty(operateTaylorLift.ShipmentID)
                        ? AppString.lblShipmentId + ": " + operateTaylorLift.ShipmentID
                        : AppString.lblCostCenter + ": " + operateTaylorLift.CostCenterName;
                    var hEquipmentType = AppString.lblEqupNo + ": " +
                                         (string.IsNullOrEmpty(operateTaylorLift.EquipmentNumber)
                                             ? AppString.lblNone
                                             : operateTaylorLift.EquipmentNumber);
                    var hLocation = AppString.lblBlock + ": " +
                                    (string.IsNullOrEmpty(operateTaylorLift.StartName)
                                        ? AppString.lblNone
                                        : operateTaylorLift.StartName);
                    LstSections.Add(new Section()
                    {
                        Header =
                            addNewLine(hChargeNo, headerCharLimit) + addNewLine(hEquipmentType, headerCharLimit) +
                            addNewLine(hLocation, headerCharLimit, true),

                        ActionType = ActionType.OperateTaylorLift,
                        CreationDate = DateTimeParse(operateTaylorLift.ServiceStartDate).Value,
                        AcknowledgeState =
                            operateTaylorLift.AcknowledgeState == AcknowledgeState.Authorized
                                ? AppString.lblDailyActivitiesAuthorized
                                : string.Empty,
                        List = new List<ActivityDetail>()
                        {
                            new ActivityDetail()
                            {

                                Tags = addNewTag(AppString.lblStartTime, StartTime) +
                                       addNewTag(AppString.lblFinishTime, FinishTime) +
                                       addNewTag(EquipmentType, EquipmentNumber) +
                                       addNewTag(AppString.lblChassis, Chasis) +
                                       addNewTag(AppString.lblSize, EquipmentSize) +
                                       addNewTag(MoveTypeDesc, MoveType) +
                                       addNewTag(AppString.lblStatus, Status) +
                                       addNewTag(AppString.lblH34, HasH34) +
                                       addNewTag(AppString.lblFromBlock, FromBlock) +
                                       addNewTag(AppString.lblToBlock, ToBlock) +
                                       addNewTag(AppString.lblProduct, Product) +
                                       addNewTag(AppString.lblElapsedTime, ElapsedTime, true),

                                Desc = addNewLine(StartTime, lineCharLimit) +
                                       addNewLine(FinishTime, lineCharLimit) +
                                       addNewLine(EquipmentNumber, lineCharLimit) +
                                       addNewLine(Chasis, lineCharLimit) +
                                       addNewLine(EquipmentSize, lineCharLimit) +
                                       addNewLine(MoveType, lineCharLimit) +
                                       addNewLine(Status, lineCharLimit) +
                                       addNewLine(HasH34, lineCharLimit) +
                                       addNewLine(FromBlock, lineCharLimit) +
                                       addNewLine(ToBlock, lineCharLimit) +
                                       addNewLine(Product, lineCharLimit) +
                                       addNewLine(ElapsedTime, lineCharLimit, true),
                                CommentTags = AppString.lblDriverComments,
                                CommentDesc = Comment
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Current.LogWarning($"Error loading Taylor Lifts repository: {ex.ToString()}");
                await ShowError(ErrorCode.DailyActivitiesTaylorLiftRepository, AppString.errorLoadingLifts);
            }

            var session = _sessionRepository.GetSessionObject();

            currentWorkdayDate = string.IsNullOrEmpty(session.LastDateArrivalInfo)
                ? DateTime.Now.Date
                : DateTimeParse(session.LastDateArrivalInfo)?.Date;

            if (LstSections.Any(x => x.CreationDate.Date < currentWorkdayDate.Value.Date))
                previousWorkdayDate = LstSections.Where(x => x.CreationDate.Date < currentWorkdayDate).Max(x => x.CreationDate);

            var lista = LstSections.Where(x => x.CreationDate.Date == currentWorkdayDate.Value.Date).ToList();
            LstMoves = new ViewModel() { List = lista.OrderByDescending(x => x.CreationDate).ToList() };
            Title = string.Concat(AppString.titleDailyActivities, " - ", LstMoves.List.Count());
            HeaderTimeMsg = string.Concat(AppString.lblDailyActivitiesCurrentWorkday, ":");
            TextTodayDate = currentWorkdayDate.Value.ToString(DateFormats.CurrentWorkday);
        }



        #region Command
        public ICommand TodayCommand => CreateCommand(() =>
        {
            var lista = LstSections.Where(x => x.CreationDate.Date == currentWorkdayDate.Value.Date).ToList();
            LstMoves = new ViewModel() { List = lista.OrderByDescending(x => x.CreationDate).ToList() };
            Title = string.Concat(AppString.titleDailyActivities, " - ", LstMoves.List.Count());
            HeaderTimeMsg = string.Concat(AppString.lblDailyActivitiesCurrentWorkday, ":");
            TextTodayDate = currentWorkdayDate.Value.ToString(DateFormats.CurrentWorkday);

            //Experiment
            //LoadWorkDay(currentWorkdayDate, AppString.lblDailyActivitiesCurrentWorkday);
        });

        public ICommand YesterdayCommand => CreateCommand(() =>
        {
            var lista = new List<Section>();
            if (previousWorkdayDate != null)
            {
                lista = LstSections.Where(x => x.CreationDate.Date == previousWorkdayDate.Value.Date).ToList();
            }
            LstMoves = new ViewModel() { List = lista.OrderByDescending(x => x.CreationDate).ToList() };
            Title = string.Concat(AppString.titleDailyActivities, " - ", LstMoves.List.Count());
            HeaderTimeMsg = string.Concat(AppString.lblDailyActivitiesPreviousWorkday, ":");
            TextTodayDate = lista.Any() ? previousWorkdayDate.Value.ToString(DateFormats.CurrentWorkday) : "---";

            //Experiment
            //LoadWorkDay(previousWorkdayDate, AppString.lblDailyActivitiesPreviousWorkday);
        });
        #endregion

        #region Variables
        private DateTime? currentWorkdayDate, previousWorkdayDate;

        private List<Section> _lstSections;
        public List<Section> LstSections
        {
            get { return _lstSections; }
            set { SetProperty(ref _lstSections, value); }
        }

        private ViewModel _lstMoves;
        public ViewModel LstMoves
        {
            get { return _lstMoves; }
            set { SetProperty(ref _lstMoves, value); }
        }
        private string _headerTimeMsg;
        public string HeaderTimeMsg
        {
            get { return _headerTimeMsg; }
            set { SetProperty(ref _headerTimeMsg, value); }
        }

        #endregion

        #region Constants
        public static class ActionType
        {
            public static String Move { get { return AppString.lblActionTypeMove; } }
            public static String Service { get { return AppString.lblActionTypeService; } }
            public static String Detention { get { return AppString.lblActionTypeDetention; } }
            public static String OperateTaylorLift { get { return AppString.lblActionTypeTaylorLift; } }
        }
        #endregion




        private List<BEDailyActivities> _lstDailyActivities;
        public List<BEDailyActivities> LstDailyActivities
        {
            get { return _lstDailyActivities; }
            set { SetProperty(ref _lstDailyActivities, value); }
        }

        public List<BEDailyActivities> LstDailyActivitiesBD { get; set; }

        private async Task LoadExperiment()
        {
            var lstMoves = _moveRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove, 40);
            var lstServices = _serviceRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove, 40);
            var lstDetentions = _detentionRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove, 40);
            var lstTaylorLift = _operateTaylorLiftRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove, 40);

            LstDailyActivitiesBD = new List<BEDailyActivities>();
            foreach (var move in lstMoves)
            {
                LstDailyActivitiesBD.Add(new BEDailyActivities()
                {
                    ChargeNo = !string.IsNullOrEmpty(move.ShipmentID) ? string.Concat(AppString.lblShipmentId, ": ", move.ShipmentID) : string.Concat(AppString.lblCostCenter, ": ", move.CostCenterName),
                    EquipmentType = string.Concat(AppString.lblEqupNo, ": ", (string.IsNullOrEmpty(move.EquipmentNumber) ? AppString.lblNone : move.EquipmentNumber)),
                    Location = string.Concat(AppString.lblBlock, ": ", move.StartName, "  -> ", move.FinishName),
                    ActionType = ActionType.Move,
                    CreationDate = DateTime.ParseExact(move.ServiceStartDate, "O", CultureInfo.InvariantCulture),
                    AcknowledgeState = move.AcknowledgeState == AcknowledgeState.Authorized ? AppString.lblDailyActivitiesAuthorized : string.Empty,
                    IsVisible = false,
                    StartTime = string.Concat(AppString.lblStartTime, ": ", DateTime.ParseExact(move.ServiceStartDate, "O", CultureInfo.InvariantCulture).ToString(DateFormats.StandardHHmmss)),
                    FinishTime = string.Concat(AppString.lblFinishTime, ": ", DateTime.ParseExact(move.ServiceFinishDate, "O", CultureInfo.InvariantCulture).ToString(DateFormats.StandardHHmmss)),
                    EquipmentSize = string.Concat(AppString.lblSize, ": ", move.EquipmentSizeDesc),
                    Chasis = string.Concat(AppString.lblChassis, ": ", move.ChassisNumber),
                    MoveType = string.Concat(AppString.lblMoveType, ": ", move.MoveTypeDesc),
                    Status = string.Concat(AppString.lblStatus, ": ", move.EquipmentStatusDesc),
                    EquipmentNumber = string.Concat(move.EquipmentTypeDesc, ": ", move.EquipmentNumber),
                    FromBlock = string.Concat(AppString.lblFromBlock, ": ", move.StartName),
                    ToBlock = string.Concat(AppString.lblToBlock, ": ", move.FinishName),
                    Product = string.Concat(AppString.lblProduct, ":", move.ProductDescription),
                    DispatchingParty = string.Concat(AppString.lblDispatching, ":", move.DispatchingPartyDesc),
                    ElapsedTime = string.Concat(AppString.lblElapsedTime, string.IsNullOrEmpty(move.ServiceFinishDate) ? string.Empty : (DateTime.ParseExact(move.ServiceFinishDate, "O", CultureInfo.InvariantCulture).TimeOfDay - DateTime.ParseExact(move.ServiceStartDate, "O", CultureInfo.InvariantCulture).TimeOfDay).ToString(@"hh\:mm\:ss"))
                });
            }

            foreach (var service in lstServices)
            {
                LstDailyActivitiesBD.Add(new BEDailyActivities()
                {
                    ChargeNo = !string.IsNullOrEmpty(service.ShipmentID) ? string.Concat(AppString.lblShipmentId, ": ", service.ShipmentID) : string.Concat(AppString.lblCostCenter, ": ", service.CostCenterName),
                    EquipmentType = string.Concat(AppString.lblEqupNo, ": ", (string.IsNullOrEmpty(service.EquipmentNumber) ? AppString.lblNone : service.EquipmentNumber)),
                    Location = string.Concat(AppString.lblBlock, ": ", service.StartName),
                    ActionType = ActionType.Service,
                    CreationDate = DateTime.ParseExact(service.ServiceStartDate, "O", CultureInfo.InvariantCulture),
                    AcknowledgeState = service.AcknowledgeState == AcknowledgeState.Authorized ? AppString.lblDailyActivitiesAuthorized : string.Empty,
                    IsVisible = true,
                    StartTime = string.Concat(AppString.lblStartTime, ": ", DateTime.ParseExact(service.ServiceStartDate, "O", CultureInfo.InvariantCulture).ToString(DateFormats.StandardHHmmss)),
                    FinishTime = string.Concat(AppString.lblFinishTime, ": ", DateTime.ParseExact(service.ServiceFinishDate, "O", CultureInfo.InvariantCulture).ToString(DateFormats.StandardHHmmss)),
                    EquipmentSize = string.Concat(AppString.lblSize, ": ", service.EquipmentSizeDesc),
                    Chasis = string.Concat(AppString.lblChassis, ": ", service.ChassisNumber),
                    HasH34 = string.Concat(AppString.lblH34, ": ", service.HasH34 ? AppString.lblH34Yes : AppString.lblH34No),
                    MoveType = string.Concat(AppString.lblMoveType, ": ", service.MoveTypeDesc),
                    Status = string.Concat(AppString.lblStatus, ": ", service.EquipmentStatusDesc),
                    EquipmentNumber = string.Concat(service.EquipmentTypeDesc, ": ", service.EquipmentNumber),
                    FromBlock = string.Concat(AppString.lblFromBlock, ": ", service.StartName),
                    ToBlock = string.Concat(AppString.lblToBlock, ": ", service.FinishName),
                    Product = string.Concat(AppString.lblProduct, ":", service.ProductDescription),
                    DispatchingParty = string.Concat(AppString.lblDispatching, ":", service.DispatchingPartyDesc),
                    ElapsedTime = string.Concat(AppString.lblElapsedTime, string.IsNullOrEmpty(service.ServiceFinishDate) ? string.Empty : (DateTime.ParseExact(service.ServiceFinishDate, "O", CultureInfo.InvariantCulture).TimeOfDay - DateTime.ParseExact(service.ServiceStartDate, "O", CultureInfo.InvariantCulture).TimeOfDay).ToString(@"hh\:mm\:ss"))


                });
            }

            foreach (var detention in lstDetentions)
            {
                LstDailyActivitiesBD.Add(new BEDailyActivities()
                {
                    ChargeNo = !string.IsNullOrEmpty(detention.ShipmentID) ? AppString.lblShipmentId + ": " + detention.ShipmentID : AppString.lblCostCenter + ": " + detention.CostCenterName,
                    EquipmentType = AppString.lblEqupNo + ": " + (string.IsNullOrEmpty(detention.EquipmentNumber) ? AppString.lblNone : detention.EquipmentNumber),
                    Location = AppString.lblBlock + ": " + detention.StartName,
                    ActionType = ActionType.Detention,
                    CreationDate = DateTime.ParseExact(detention.ServiceStartDate, "O", CultureInfo.InvariantCulture),
                    AcknowledgeState = detention.AcknowledgeState == AcknowledgeState.Authorized ? AppString.lblDailyActivitiesAuthorized : string.Empty,
                    IsVisible = true,
                    StartTime = string.Concat(AppString.lblStartTime, ": ", DateTime.ParseExact(detention.ServiceStartDate, "O", CultureInfo.InvariantCulture).ToString(DateFormats.StandardHHmmss)),
                    FinishTime = string.Concat(AppString.lblFinishTime, ": ", DateTime.ParseExact(detention.ServiceFinishDate, "O", CultureInfo.InvariantCulture).ToString(DateFormats.StandardHHmmss)),
                    DispatchingParty = string.Concat(AppString.lblDispatching, ":", detention.DispatchingPartyDesc),
                    ToBlock = string.Concat(AppString.lblToBlock, ": ", detention.FinishName),
                    Product = string.Concat(AppString.lblProduct, ":", detention.ProductDescription),
                    ElapsedTime = string.Concat(AppString.lblElapsedTime, string.IsNullOrEmpty(detention.ServiceFinishDate) ? string.Empty : (DateTime.ParseExact(detention.ServiceFinishDate, "O", CultureInfo.InvariantCulture).TimeOfDay - DateTime.ParseExact(detention.ServiceStartDate, "O", CultureInfo.InvariantCulture).TimeOfDay).ToString(@"hh\:mm\:ss"))
                });
            }

            foreach (var operateTaylorLift in lstTaylorLift)
            {
                LstDailyActivitiesBD.Add(new BEDailyActivities()
                {
                    ChargeNo = !string.IsNullOrEmpty(operateTaylorLift.ShipmentID) ? AppString.lblShipmentId + ": " + operateTaylorLift.ShipmentID : AppString.lblCostCenter + ": " + operateTaylorLift.CostCenterName,
                    EquipmentType = AppString.lblEqupNo + ": " + (string.IsNullOrEmpty(operateTaylorLift.EquipmentNumber) ? AppString.lblNone : operateTaylorLift.EquipmentNumber),
                    Location = AppString.lblBlock + ": " + (string.IsNullOrEmpty(operateTaylorLift.StartName) ? AppString.lblNone : operateTaylorLift.StartName),
                    ActionType = ActionType.OperateTaylorLift,
                    CreationDate = DateTime.ParseExact(operateTaylorLift.ServiceStartDate, "O", CultureInfo.InvariantCulture),
                    AcknowledgeState = operateTaylorLift.AcknowledgeState == AcknowledgeState.Authorized ? AppString.lblDailyActivitiesAuthorized : string.Empty,
                    IsVisible = true,
                    StartTime = string.Concat(AppString.lblStartTime, ": ", DateTime.ParseExact(operateTaylorLift.ServiceStartDate, "O", CultureInfo.InvariantCulture).ToString(DateFormats.StandardHHmmss)),
                    FinishTime = string.Concat(AppString.lblFinishTime, ": ", DateTime.ParseExact(operateTaylorLift.ServiceFinishDate, "O", CultureInfo.InvariantCulture).ToString(DateFormats.StandardHHmmss)),
                    DispatchingParty = string.Concat(AppString.lblDispatching, ":", operateTaylorLift.DispatchingPartyDesc),
                    ToBlock = string.Concat(AppString.lblToBlock, ": ", operateTaylorLift.FinishName),
                    Product = string.Concat(AppString.lblProduct, ":", operateTaylorLift.ProductDescription),
                    ElapsedTime = string.Concat(AppString.lblElapsedTime, string.IsNullOrEmpty(operateTaylorLift.ServiceFinishDate) ? string.Empty : (DateTime.ParseExact(operateTaylorLift.ServiceFinishDate, "O", CultureInfo.InvariantCulture).TimeOfDay - DateTime.ParseExact(operateTaylorLift.ServiceStartDate, "O", CultureInfo.InvariantCulture).TimeOfDay).ToString(@"hh\:mm\:ss"))
                });
            }


            var session = _sessionRepository.GetSessionObject();

            currentWorkdayDate = string.IsNullOrEmpty(session.LastDateArrivalInfo)
                ? DateTime.Now.Date
                : DateTime.ParseExact(session.LastDateArrivalInfo, "O", CultureInfo.InvariantCulture).Date;

            if (LstDailyActivitiesBD.Any(x => x.CreationDate.Date < currentWorkdayDate.Value.Date))
                previousWorkdayDate = LstDailyActivitiesBD.Where(x => x.CreationDate.Date < currentWorkdayDate.Value).Max(x => x.CreationDate);

            LoadWorkDay(currentWorkdayDate, AppString.lblDailyActivitiesCurrentWorkday);
        }

        void LoadWorkDay(DateTime? workDay, string headerLabel)
        {
            var lista = new List<BEDailyActivities>();
            if (workDay != null)
            {
                lista = LstDailyActivitiesBD.Where(x => x.CreationDate.Date == workDay.Value.Date).ToList();
            }
            LstDailyActivities = lista.OrderByDescending(x => x.CreationDate).ToList();
            Title = string.Concat(AppString.titleDailyActivities, " - ", LstDailyActivities.Count());
            HeaderTimeMsg = string.Concat(headerLabel, ":");
            TextTodayDate = LstDailyActivities.Any() ? workDay.Value.ToString(DateFormats.CurrentWorkday) : "---";
        }

        public ICommand AcordionCommand => CreateCommand<object>((detail) =>
        {
            var objeto = LstDailyActivities.Find(x => x == detail);
            objeto.IsVisible = !objeto.IsVisible;

            OnPropertyChanged("LstDailyActivities");
        });
    }
}
