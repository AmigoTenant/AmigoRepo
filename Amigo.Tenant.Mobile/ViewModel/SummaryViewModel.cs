using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
using XPO.ShuttleTracking.Mobile.Entity.Service;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Resource;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class PerMoveMovesData
    {
        public string Charge { get; set; }
        public string InPlant { get; set; }
        public string OutPlantBobTail { get; set; }
        public string Respot { get; set; }
        public string Prep { get; set; }
        public string Yardcheck { get; set; }
        public string Trailer { get; set; }
        public string Bobtail { get; set; }
    }

    public class PerMoveHoursData
    {
        public string Charge { get; set; }
        public string Detention { get; set; }
        public string Lift { get; set; }
    }

    public class SummaryViewModel : TodayViewModel
    {
        private IMoveRepository _moveRepository;
        private IServiceRepository _serviceRepository;
        private IDetentionRepository _detentionRepository;
        private IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private ISessionRepository _sessionRepository;
        private IServiceTypeRepository _serviceTypeRepository;

        private String TotalHoursPerMove;
        private String TotalHoursPerHour;
        private String TotalMovesPerMove;
        private String TotalMovesPerHour;

        public SummaryViewModel(IMoveRepository moveRepository,
                                IServiceRepository serviceRepository,
                                IDetentionRepository detentionRepository,
                                IOperateTaylorLiftRepository operateTaylorLiftRepository,
                                ISessionRepository sessionRepository,
                                IServiceTypeRepository serviceTypeRepository)
        {
            _moveRepository = moveRepository;
            _serviceRepository = serviceRepository;
            _detentionRepository = detentionRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _sessionRepository = sessionRepository;
            _serviceTypeRepository = serviceTypeRepository;
        }
        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }

        Double RoundUpSeconds(Double valueInSec, int roundInSec)
        {
            return Math.Truncate((valueInSec + roundInSec - 1)/roundInSec)*roundInSec;
        }

        private int MoveTypeOutPlant = 0;
        private int MoveTypeInPlant = 0;
        private int MoveTypeAuthorizedBobtail = 0;
        private int MoveTypeRespot = 0;
        private int MoveTypePrep = 0;
        private int MoveTypeYardCheck = 0;
        private int MoveTypeTrailer = 0;
        private int MoveTypeUnPrep = 0;

        private int MoveTypeBobtail = 0;
        private int MoveTypeBafflePrep = 0;
        private int MoveTypeReplacePart = 0;
        private int MoveTypeRepairISO = 0;


        private async Task RecoverMoveTypeCode()
        {
            try
            {
                MoveTypeOutPlant = _serviceTypeRepository.GetByCode(ServiceCode.OutPlantMove).ServiceId;
                MoveTypeInPlant = _serviceTypeRepository.GetByCode((ServiceCode.InPlantMove)).ServiceId;
                MoveTypeAuthorizedBobtail = _serviceTypeRepository.GetByCode((ServiceCode.AuthorizedBobtail)).ServiceId;
                MoveTypeRespot = _serviceTypeRepository.GetByCode((ServiceCode.Respot)).ServiceId;
                MoveTypePrep = _serviceTypeRepository.GetByCode((ServiceCode.Prep)).ServiceId;
                MoveTypeYardCheck = _serviceTypeRepository.GetByCode((ServiceCode.YardCheck)).ServiceId;
                MoveTypeTrailer = _serviceTypeRepository.GetByCode((ServiceCode.TrailerInterchange)).ServiceId;
                MoveTypeUnPrep = _serviceTypeRepository.GetByCode((ServiceCode.Unprep)).ServiceId;
                MoveTypeBobtail = _serviceTypeRepository.GetByCode((ServiceCode.Bobtail)).ServiceId;
                MoveTypeBafflePrep = _serviceTypeRepository.GetByCode((ServiceCode.BafflePrep)).ServiceId;
                MoveTypeReplacePart = _serviceTypeRepository.GetByCode((ServiceCode.ReplacePart)).ServiceId;
                MoveTypeRepairISO = _serviceTypeRepository.GetByCode((ServiceCode.RepairIso)).ServiceId;
            }
            catch (Exception e)
            {
                await ShowError(ErrorCode.SummaryRecoverMoveTypeCode, AppString.errorServiceCode);
            }
        }

        private List<BEMove> LstMovesBD { get; set; }
        private List<BEService> LstServiceBD { get; set; }
        private List<BEDetention> LstDetentionBD { get; set; }
        private List<BEOperateTaylorLift> LstTaylorLiftBD { get; set; }

        public override async void OnPushed()
        {
            base.OnPushed();

            await RecoverMoveTypeCode();

            var session = _sessionRepository.GetSessionObject();
            Driver = string.Concat(session.FirstName, " ", session.LastName);
            UnitNo = session.Username;

            currentWorkdayDate = string.IsNullOrEmpty(session.LastDateArrivalInfo)
                ? DateTime.Now.Date
                : DateTime.ParseExact(session.LastDateArrivalInfo, "O", CultureInfo.InvariantCulture).Date;

            LstMovesBD = _moveRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove).ToList();
            LstServiceBD = _serviceRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove).ToList();
            LstDetentionBD = _detentionRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove).ToList();
            LstTaylorLiftBD = _operateTaylorLiftRepository.FindAll(x => x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove).ToList();

            if (LstMovesBD.Any(x => x.CreationDate.Date < currentWorkdayDate.Value.Date))
                previousWorkdayDate = LstMovesBD.Where(x => x.CreationDate.Date < currentWorkdayDate).Max(x => x.CreationDate.Date);
            if (LstServiceBD.Any(x => x.CreationDate.Date < currentWorkdayDate.Value.Date))
            {
                var prev = LstServiceBD.Where(x => x.CreationDate.Date < currentWorkdayDate).Max(x => x.CreationDate.Date);
                previousWorkdayDate = previousWorkdayDate == null ? prev : previousWorkdayDate < prev ? prev : previousWorkdayDate;
            }
            if (LstDetentionBD.Any(x => x.CreationDate.Date < currentWorkdayDate.Value.Date))
            {
                var prev = LstDetentionBD.Where(x => x.CreationDate.Date < currentWorkdayDate).Max(x => x.CreationDate.Date);
                previousWorkdayDate = previousWorkdayDate == null ? prev : previousWorkdayDate < prev ? prev : previousWorkdayDate;
            }
            if (LstTaylorLiftBD.Any(x => x.CreationDate.Date < currentWorkdayDate.Value.Date))
            {
                var prev = LstTaylorLiftBD.Where(x => x.CreationDate.Date < currentWorkdayDate).Max(x => x.CreationDate.Date);
                previousWorkdayDate = previousWorkdayDate == null ? prev : previousWorkdayDate < prev ? prev : previousWorkdayDate;
            }

            _detentionRoundMin = 15;
            var parameter = Parameters.Get(ParameterCode.DetentionRoundMin);
            if (!int.TryParse(parameter, out _detentionRoundMin))
                _detentionRoundMin = 15;

            //Load
            LoadWorkDay(currentWorkdayDate, DateTime.Now, AppString.lblDailyActivitiesCurrentWorkday);

            SelectedTab = 0;
        }
        public ICommand YesterdayCommand => CreateCommand(() =>
        {
            LoadWorkDay(previousWorkdayDate, currentWorkdayDate, AppString.lblDailyActivitiesPreviousWorkday);
        });
        public ICommand TodayCommand => CreateCommand(() =>
        {
            LoadWorkDay(currentWorkdayDate, DateTime.Now, AppString.lblDailyActivitiesCurrentWorkday);
        });
        void LoadWorkDay(DateTime? workDayStart, DateTime? workDayFinish, string headerLabel)
        {
            List<int> moveTypes = new List<int>
            {
                MoveTypeInPlant,
                MoveTypeOutPlant,
                MoveTypeAuthorizedBobtail,
                MoveTypeRespot,
                MoveTypeYardCheck,
                MoveTypePrep,
                MoveTypeUnPrep,
                MoveTypeTrailer,
                MoveTypeBobtail,
                MoveTypeBafflePrep,
                MoveTypeReplacePart,
                MoveTypeRepairISO
            };

            var lstMoves = LstMovesBD.FindAll(x => x.CreationDate >= workDayStart && x.CreationDate < workDayFinish && moveTypes.Any(c => c == x.MoveType));
            var lstServices = LstServiceBD.FindAll(x => x.CreationDate >= workDayStart && x.CreationDate < workDayFinish && moveTypes.Any(c => c == x.MoveType));
            var lstDetentions = LstDetentionBD.FindAll(x => x.CreationDate >= workDayStart && x.CreationDate < workDayFinish);
            var lstTaylorLift = LstTaylorLiftBD.FindAll(x => x.CreationDate >= workDayStart && x.CreationDate < workDayFinish);

            var lstChargueNo = new HashSet<String>();

            double totalMinutesPerMove = 0, totalMinutesPerHour = 0;
            foreach (var operation in lstMoves)
            {
                var start = DateTimeParse(operation.ServiceStartDate);
                var finish = DateTimeParse(operation.ServiceFinishDate);
                if (finish.HasValue)
                {
                    var span = finish - start;
                    totalMinutesPerMove += span.Value.TotalMinutes;
                }

                if (operation.ShipmentID?.Length > 0)
                    lstChargueNo.Add(operation.ShipmentID);
                else if (operation?.CostCenterName.Length > 0)
                    lstChargueNo.Add(operation.CostCenterName);
            }
            foreach (var operation in lstServices)
            {
                var start = DateTimeParse(operation.ServiceStartDate);
                var finish = DateTimeParse(operation.ServiceFinishDate);
                if (finish.HasValue)
                {
                    var span = finish - start;
                    totalMinutesPerMove += span.Value.TotalMinutes;
                }

                if (operation.ShipmentID?.Length > 0)
                    lstChargueNo.Add(operation.ShipmentID);
                else if (operation?.CostCenterName.Length > 0)
                    lstChargueNo.Add(operation.CostCenterName);
            }
            foreach (var operation in lstDetentions)
            {
                var start = DateTimeParse(operation.ServiceStartDate);
                var finish = DateTimeParse(operation.ServiceFinishDate);
                if (finish.HasValue)
                {
                    var span = finish - start;
                    totalMinutesPerHour += RoundUpSeconds(span.Value.TotalSeconds, _detentionRoundMin*60)/60;
                }

                if (operation.ShipmentID?.Length > 0)
                    lstChargueNo.Add(operation.ShipmentID);
                else if (operation?.CostCenterName.Length > 0)
                    lstChargueNo.Add(operation.CostCenterName);
            }
            foreach (var operation in lstTaylorLift)
            {
                var start = DateTimeParse(operation.ServiceStartDate);
                var finish = DateTimeParse(operation.ServiceFinishDate);
                if (finish.HasValue)
                {
                    var span = finish - start;
                    totalMinutesPerHour += RoundUpSeconds(span.Value.TotalSeconds, _detentionRoundMin*60)/60;
                }

                if (operation.ShipmentID?.Length > 0)
                    lstChargueNo.Add(operation.ShipmentID);
                else if (operation?.CostCenterName.Length > 0)
                    lstChargueNo.Add(operation.CostCenterName);
            }

            //var round = new DateTime();
            //round = round.AddMinutes(totalMinutesPerMove);
            //RoundUp(round, TimeSpan.FromMinutes(_detentionRoundMin)).ToString("t");
            //var es1 = RoundUp(round, TimeSpan.FromMinutes(_detentionRoundMin));
            //TotalHoursPerMove = es1.ToString(DateFormats.TimeHHmm);
            TotalHoursPerMove = new DateTime().AddMinutes(totalMinutesPerMove).ToString(DateFormats.TimeHHmm);

            //round = new DateTime();
            //round = round.AddMinutes(totalMinutesPerHour);
            //var es2 = RoundUp(round, TimeSpan.FromMinutes(_detentionRoundMin));
            //TotalHoursPerHour = es2.ToString(DateFormats.TimeHHmm);
            TotalHoursPerHour = new DateTime().AddMinutes(totalMinutesPerHour).ToString(DateFormats.TimeHHmm);

            TotalMovesPerMove = (lstMoves.Count).ToString();
            TotalMovesPerHour = (lstDetentions.Count + lstTaylorLift.Count).ToString();

            SummaryPerHour = new ObservableCollection<PerMoveHoursData>();
            SummaryPerMove = new ObservableCollection<PerMoveMovesData>();

            foreach (var charge in lstChargueNo)
            {
                var liftCount = lstTaylorLift.Count(lift => lift.ShipmentID.Equals(charge) || lift.CostCenterName.Equals(charge));
                var detentionCount = lstDetentions.Count(det => det.ShipmentID.Equals(charge) || det.CostCenterName.Equals(charge));

                if (liftCount + detentionCount > 0)
                    SummaryPerHour.Add(new PerMoveHoursData()
                    {
                        Charge = charge,
                        Lift = liftCount.ToString(),
                        Detention = detentionCount.ToString()
                    });

                var inPlantCount = lstMoves.Count(op => (op.ShipmentID.Equals(charge) || op.CostCenterName.Equals(charge)) && op.MoveType == MoveTypeInPlant);
                var outPlantBobTailCount = lstMoves.Count(op => (op.ShipmentID.Equals(charge) || op.CostCenterName.Equals(charge)) && (op.MoveType == MoveTypeOutPlant || op.MoveType == MoveTypeAuthorizedBobtail));
                var respotCount = lstMoves.Count(op => (op.ShipmentID.Equals(charge) || op.CostCenterName.Equals(charge)) && op.MoveType == MoveTypeRespot);
                var yardcheckCount = lstServices.Count(op => (op.ShipmentID.Equals(charge) || op.CostCenterName.Equals(charge)) && op.MoveType == MoveTypeYardCheck);
                var prepCount = lstServices.Count(op => (op.ShipmentID.Equals(charge) || op.CostCenterName.Equals(charge)) &&
                                                        (op.MoveType == MoveTypePrep || op.MoveType == MoveTypeUnPrep || op.MoveType == MoveTypeBafflePrep || op.MoveType == MoveTypeReplacePart || op.MoveType == MoveTypeRepairISO));
                var trailerCount = lstServices.Count(op => ((op.ShipmentID.Equals(charge) || op.CostCenterName.Equals(charge)) && op.MoveType == MoveTypeTrailer));
                var bobtailCount = lstMoves.Count(op => ((op.ShipmentID.Equals(charge) || op.CostCenterName.Equals(charge)) && op.MoveType == MoveTypeBobtail));
                if (inPlantCount + outPlantBobTailCount + respotCount + yardcheckCount + prepCount + trailerCount + bobtailCount > 0)
                    SummaryPerMove.Add(new PerMoveMovesData()
                    {
                        Charge = charge,
                        InPlant = inPlantCount.ToString(),
                        OutPlantBobTail = outPlantBobTailCount.ToString(),
                        Respot = respotCount.ToString(),

                        Yardcheck = yardcheckCount.ToString(),
                        Prep = prepCount.ToString(),
                        Trailer = trailerCount.ToString(),
                        Bobtail = bobtailCount.ToString()
                    });
            }
            if (SelectedTab == 0)
            {
                TotalMoves = TotalMovesPerMove;
                TotalHours = TotalHoursPerMove;
            }
            else
            {
                TotalMoves = TotalMovesPerHour;
                TotalHours = TotalHoursPerHour;
            }

            HeaderTimeMsg = string.Concat(headerLabel, ":");
            TextTodayDate = (lstMoves.Any() || lstServices.Any() || lstDetentions.Any() || lstTaylorLift.Any())
                ? workDayStart.Value.ToString(DateFormats.CurrentWorkday)
                : "---";
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
                ShowError(ErrorCode.SummaryDateTimeParse, AppString.errorServiceCode);
            }
            return null;
        }
        private int _detentionRoundMin = 15;
        private DateTime? currentWorkdayDate, previousWorkdayDate;

        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (value.Equals(0))
                {
                    TotalMoves = TotalMovesPerMove;
                    TotalHours = TotalHoursPerMove;
                    ShowTotalMoves = true;
                }
                else
                {
                    TotalMoves = TotalMovesPerHour;
                    TotalHours = TotalHoursPerHour;
                    ShowTotalMoves = false;
                }
                SetProperty(ref _selectedTab, value);
            }
        }

        private ObservableCollection<PerMoveMovesData> _summaryPerMove;
        public ObservableCollection<PerMoveMovesData> SummaryPerMove
        {
            get { return _summaryPerMove; }
            protected set { SetProperty(ref _summaryPerMove, value); }
        }

        private ObservableCollection<PerMoveHoursData> _summaryPerHour;
        public ObservableCollection<PerMoveHoursData> SummaryPerHour
        {
            get { return _summaryPerHour; }
            protected set { SetProperty(ref _summaryPerHour, value); }
        }

        private string _driver;
        public string Driver
        {
            get { return _driver; }
            set { SetProperty(ref _driver, value); }
        }

        private string _unitNo;
        public string UnitNo
        {
            get { return _unitNo; }
            set { SetProperty(ref _unitNo, value); }
        }

        private string _totalHours;
        public string TotalHours
        {
            get { return _totalHours; }
            set { SetProperty(ref _totalHours, value); }
        }

        private string _totalMoves;
        public string TotalMoves
        {
            get { return _totalMoves; }
            set { SetProperty(ref _totalMoves, value); }
        }
        private string _headerTimeMsg;
        public string HeaderTimeMsg
        {
            get { return _headerTimeMsg; }
            set { SetProperty(ref _headerTimeMsg, value); }
        }
        private bool _showTotalMoves;
        public bool ShowTotalMoves
        {
            get { return _showTotalMoves; }
            set { SetProperty(ref _showTotalMoves, value); }
        }
        public ICommand ShowLegendCommand => CreateCommand(async () =>
        {
            await ShowOkAlert(AppString.lblSummaryLegendTitle, string.Format(AppString.lblSummaryLegendText.Replace(@"\n", Environment.NewLine)));
        });

    }
}
