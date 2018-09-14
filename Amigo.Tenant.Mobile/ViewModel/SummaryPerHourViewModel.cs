using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Entity.Service;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Resource;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class PerHourHoursData
    {
        public string Charge { get; set; }

        public string Time { get; set; }
        public string Moves { get; set; }
    }

    public class SummaryPerHourViewModel : TodayViewModel
    {
        private IMoveRepository _moveRepository;
        private IServiceRepository _serviceRepository;
        private ISessionRepository _sessionRepository;
        private IServiceTypeRepository _serviceTypeRepository;

        public SummaryPerHourViewModel(IMoveRepository moveRepository,
                                IServiceRepository serviceRepository,
                                ISessionRepository sessionRepository,
                                IServiceTypeRepository serviceTypeRepository)
        {
            _moveRepository = moveRepository;
            _serviceRepository = serviceRepository;
            _sessionRepository = sessionRepository;
            _serviceTypeRepository = serviceTypeRepository;
        }
        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }

        private int MoveTypeOutPlant = 0;
        private int MoveTypeInPlant = 0;
        private int MoveTypeLoadingAtBlock = 0;
        private int MoveTypeUnloadingAtBlock = 0;
        private int MoveTypeWaitingAtScales = 0;
        private int MoveTypeWaitingOnBlock = 0;

        private async Task RecoverMoveTypeCode()
        {
            try
            {                
                MoveTypeOutPlant = _serviceTypeRepository.GetByCode(ServiceCode.OutPlantMove).ServiceId;
                MoveTypeInPlant = _serviceTypeRepository.GetByCode(ServiceCode.InPlantMove).ServiceId;

                MoveTypeLoadingAtBlock = _serviceTypeRepository.GetByCode(ServiceCode.LoadingAtBlock).ServiceId;
                MoveTypeUnloadingAtBlock = _serviceTypeRepository.GetByCode(ServiceCode.UnloadingAtBlock).ServiceId;
                MoveTypeWaitingAtScales = _serviceTypeRepository.GetByCode(ServiceCode.WaitingAtScales).ServiceId;
                MoveTypeWaitingOnBlock = _serviceTypeRepository.GetByCode(ServiceCode.WaitingOnBlock).ServiceId;
            }
            catch (Exception e)
            {
                await ShowError(ErrorCode.SummaryPerHourRecoverMoveTypeCode, AppString.errorServiceCode);
            }
        }
        private List<BEMove> LstMovesBD { get; set; }
        private List<BEService> LstServiceBD { get; set; }

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

            LstMovesBD = _moveRepository.GetAll().ToList();
            LstServiceBD = _serviceRepository.GetAll().ToList();

            if (LstMovesBD.Any(x => x.CreationDate.Date < currentWorkdayDate.Value.Date))
                previousWorkdayDate = LstMovesBD.Where(x => x.CreationDate.Date < currentWorkdayDate).Max(x => x.CreationDate.Date);
            if (LstServiceBD.Any(x => x.CreationDate.Date < currentWorkdayDate.Value.Date))
            {
                var prev = LstServiceBD.Where(x => x.CreationDate.Date < currentWorkdayDate).Max(x => x.CreationDate.Date);
                previousWorkdayDate = previousWorkdayDate == null ? prev : previousWorkdayDate < prev ? prev : previousWorkdayDate;
            }

            _detentionRoundMin = 15;
            var parameter = Parameters.Get(ParameterCode.DetentionRoundMin);
            if (!int.TryParse(parameter, out _detentionRoundMin))
                _detentionRoundMin = 15;

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
            var lstMoves = LstMovesBD.FindAll(x => (x.CreationDate >= workDayStart && x.CreationDate < workDayFinish) && (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove));
            var lstServices = LstServiceBD.FindAll(x => (x.CreationDate >= workDayStart && x.CreationDate < workDayFinish) && (x.CurrentState == MoveState.StartedMove || x.CurrentState == MoveState.FinishedMove));

            SummaryPerHour = new ObservableCollection<PerHourHoursData>();

            var lstChargueNo = new HashSet<String>();

            double totalMinutesPerHour = 0;
            foreach (var operation in lstMoves)
            {
                var start = DateTimeParse(operation.ServiceStartDate);
                var finish = DateTimeParse(operation.ServiceFinishDate);
                if (finish.HasValue)
                {
                    var span = finish - start;
                    totalMinutesPerHour += span.Value.TotalMinutes;
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
                    totalMinutesPerHour += span.Value.TotalMinutes;
                }

                if (operation.ShipmentID?.Length > 0)
                    lstChargueNo.Add(operation.ShipmentID);
                else if (operation?.CostCenterName.Length > 0)
                    lstChargueNo.Add(operation.CostCenterName);
            }

            var round = new DateTime();
            round = round.AddMinutes(totalMinutesPerHour);
            //TODO: Redondear cuando lo confirmen
            //var es2 = RoundUp(round, TimeSpan.FromMinutes(DetentionRoundMin));
            //TotalHours = es2.ToString(DateFormats.TimeHHmm);
            TotalHours = round.ToString(DateFormats.TimeHHmm);

            TotalMoves = (lstMoves.Count + lstServices.Count).ToString();

            foreach (var charge in lstChargueNo)
            {
                var time = lstMoves.Where(op => op.ShipmentID.Equals(charge) || op.CostCenterName.Equals(charge)).Sum(op => (DateTime.ParseExact(op.ServiceFinishDate, "O", CultureInfo.InvariantCulture) -
                                                DateTime.ParseExact(op.ServiceStartDate, "O", CultureInfo.InvariantCulture)).TotalMinutes) +
                           lstServices.Where(op => op.ShipmentID.Equals(charge) || op.CostCenterName.Equals(charge)).Sum(op => (DateTime.ParseExact(op.ServiceFinishDate, "O", CultureInfo.InvariantCulture) -
                                                DateTime.ParseExact(op.ServiceStartDate, "O", CultureInfo.InvariantCulture)).TotalMinutes);
                var moves = lstMoves.Count(op => op.ShipmentID.Equals(charge) || op.CostCenterName.Equals(charge)) +
                            lstServices.Count(op => op.ShipmentID.Equals(charge) || op.CostCenterName.Equals(charge));

                if (moves > 0)
                    SummaryPerHour.Add(new PerHourHoursData()
                    {
                        Charge = charge,
                        Time = new DateTime().AddMinutes(time).ToString(DateFormats.TimeHHmm),
                        Moves = moves.ToString()
                    });
            }

            HeaderTimeMsg = string.Concat(headerLabel, ":");
            TextTodayDate = (lstMoves.Any() || lstServices.Any())
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
                ShowError(ErrorCode.SummaryPerHourDateTimeParse, AppString.errorServiceCode);
            }
            return null;
        }

        private ObservableCollection<PerHourHoursData> _summaryPerHour;
        public ObservableCollection<PerHourHoursData> SummaryPerHour
        {
            get { return _summaryPerHour; }
            protected set { SetProperty(ref _summaryPerHour, value); }
        }
        private int _detentionRoundMin = 15;
        private DateTime? currentWorkdayDate, previousWorkdayDate;

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

        private int _selectedTab;
        public int SelectedTab
        {
            get { return 1; }
            set { SetProperty(ref _selectedTab, value); }
        }
        private string _headerTimeMsg;
        public string HeaderTimeMsg
        {
            get { return _headerTimeMsg; }
            set { SetProperty(ref _headerTimeMsg, value); }
        }
        public ICommand ShowLegendCommand => CreateCommand(async () =>
        {
            await ShowOkAlert(AppString.lblSummaryLegendTitle, string.Format(AppString.lblSummaryPerHourLegend.Replace(@"\n", Environment.NewLine)));
        });
    }
}
