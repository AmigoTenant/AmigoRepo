using System;
using System.Globalization;
using System.Threading.Tasks;
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
    public class DailyActivitiesFilteredItemViewModel : CustomViewModel
    {
        private readonly INavigator _navigator;
        private IMoveRepository _moveRepository;
        private IServiceRepository _serviceRepository;
        private IDetentionRepository _detentionRepository;
        private IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private ISessionRepository _sessionRepository;

        public DailyActivitiesFilteredItemViewModel(INavigator navigator,
                                        IMoveRepository moveRepository,
                                        IServiceRepository serviceRepository,
                                        IDetentionRepository detentionRepository,
                                        IOperateTaylorLiftRepository operateTaylorLiftRepository,
                                        ISessionRepository sessionRepository)
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

            await LoadData().ConfigureAwait(false);

        }

        private async Task LoadData()
        {
            var lstMoves = _moveRepository.FindAll(x => x.InternalId == IdItem, 40);
            var lstServices = _serviceRepository.FindAll(x => x.InternalId == IdItem, 40);
            var lstDetentions = _detentionRepository.FindAll(x => x.InternalId == IdItem, 40);
            var lstTaylorLift = _operateTaylorLiftRepository.FindAll(x => x.InternalId == IdItem, 40);

            BEMove move = null;
            BEService service = null;
            BEDetention detention = null;
            BEOperateTaylorLift operateTaylorLift = null;

            if (lstMoves != null && lstMoves.Count > 0){ move = lstMoves[0]; }
            else if (lstServices != null && lstServices.Count > 0) { service = lstServices[0]; }
            else if (lstDetentions != null && lstDetentions.Count > 0) { detention = lstDetentions[0]; }
            else if (lstTaylorLift != null && lstTaylorLift.Count > 0) { operateTaylorLift = lstTaylorLift[0]; }

            if (move != null)
            {
                LblId = AppString.lblShipmentId.ToUpper();
                if (!string.IsNullOrEmpty(move.ShipmentID))
                {
                    LblId = AppString.lblShipmentId.ToUpper();
                    LblNumber = move.ShipmentID;
                }
                else if (!string.IsNullOrEmpty(move.CostCenterName))
                {
                    LblId = AppString.lblCostCenter.ToUpper();
                    LblNumber = move.CostCenterName;
                }

                StartTime = DateTimeParse(move.ServiceStartDate).HasValue ? DateTimeParse(move.ServiceStartDate).Value.ToString(DateFormats.StandardHHmmss) : AppString.lblMsgNotRegistered;
                FinishTime = DateTimeParse(move.ServiceFinishDate).HasValue ? DateTimeParse(move.ServiceFinishDate).Value.ToString(DateFormats.StandardHHmmss) : AppString.lblMsgNotRegistered;
                
                EquipmentTypeDesc = move.EquipmentTypeDesc;
                EquipmentNumber = move.EquipmentNumber;
                ChassisNo = move.ChassisNumber;
                EquipmentSizeDesc = move.EquipmentSizeDesc;

                MoveTypeDesc = AppString.lblMoveType; 
                MoveType = move.MoveTypeDesc;
                MoveStatus = move.EquipmentStatusDesc;
                Has34 = move.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
                FromBlock = move.StartName;
                ToBlock = move.FinishName;
                Product = move.ProductDescription;
                DriverComments = move.DriverComments;

                ElapsedTime = string.IsNullOrEmpty(move.ServiceFinishDate) ? AppString.lblDailyIndeterminable : (DateTimeParse(move.ServiceFinishDate).Value.TimeOfDay - DateTimeParse(move.ServiceStartDate).Value.TimeOfDay).ToString(@"hh\:mm\:ss");
            }


            if (service != null)
            {
                LblId = AppString.lblShipmentId.ToUpper();
                if (!string.IsNullOrEmpty(service.ShipmentID))
                {
                    LblId = AppString.lblShipmentId.ToUpper();
                    LblNumber = service.ShipmentID;
                }
                else if (!string.IsNullOrEmpty(service.CostCenterName))
                {
                    LblId = AppString.lblCostCenter.ToUpper();
                    LblNumber = service.CostCenterName;
                }

                StartTime = DateTimeParse(service.ServiceStartDate).HasValue ? DateTimeParse(service.ServiceStartDate).Value.ToString(DateFormats.StandardHHmmss) : AppString.lblMsgNotRegistered;
                FinishTime = DateTimeParse(service.ServiceFinishDate).HasValue ? DateTimeParse(service.ServiceFinishDate).Value.ToString(DateFormats.StandardHHmmss) : AppString.lblMsgNotRegistered;

                EquipmentTypeDesc = service.EquipmentTypeDesc;
                EquipmentNumber = service.EquipmentNumber;
                ChassisNo = service.ChassisNumber;
                EquipmentSizeDesc = service.EquipmentSizeDesc;

                MoveTypeDesc = AppString.lblServiceType;
                MoveType = service.MoveTypeDesc;
                MoveStatus = service.EquipmentStatusDesc;
                Has34 = service.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
                FromBlock = service.StartName;
                ToBlock = service.FinishName;
                Product = service.ProductDescription;
                DriverComments = service.DriverComments;

                ElapsedTime = string.IsNullOrEmpty(service.ServiceFinishDate) ? AppString.lblDailyIndeterminable : (DateTimeParse(service.ServiceFinishDate).Value.TimeOfDay - DateTimeParse(service.ServiceStartDate).Value.TimeOfDay).ToString(@"hh\:mm\:ss");
            }

            if (detention != null)
            {
                LblId = AppString.lblShipmentId.ToUpper();
                if (!string.IsNullOrEmpty(detention.ShipmentID))
                {
                    LblId = AppString.lblShipmentId.ToUpper();
                    LblNumber = detention.ShipmentID;
                }
                else if (!string.IsNullOrEmpty(detention.CostCenterName))
                {
                    LblId = AppString.lblCostCenter.ToUpper();
                    LblNumber = detention.CostCenterName;
                }

                StartTime = DateTimeParse(detention.ServiceStartDate).HasValue ? DateTimeParse(detention.ServiceStartDate).Value.ToString(DateFormats.StandardHHmmss) : AppString.lblMsgNotRegistered;
                FinishTime = DateTimeParse(detention.ServiceFinishDate).HasValue ? DateTimeParse(detention.ServiceFinishDate).Value.ToString(DateFormats.StandardHHmmss) : AppString.lblMsgNotRegistered;

                EquipmentTypeDesc = detention.EquipmentTypeDesc;
                EquipmentNumber = detention.EquipmentNumber;
                ChassisNo = detention.ChassisNumber;
                EquipmentSizeDesc = detention.EquipmentSizeDesc;

                MoveTypeDesc = AppString.lblServiceType;
                MoveType = detention.MoveTypeDesc;
                MoveStatus = detention.EquipmentStatusDesc;
                Has34 = detention.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
                FromBlock = detention.StartName;
                ToBlock = detention.FinishName;
                Product = detention.ProductDescription;
                DriverComments = detention.DriverComments;

                ElapsedTime = string.IsNullOrEmpty(detention.ServiceFinishDate) ? AppString.lblDailyIndeterminable : (DateTimeParse(detention.ServiceFinishDate).Value.TimeOfDay - DateTimeParse(detention.ServiceStartDate).Value.TimeOfDay).ToString(@"hh\:mm\:ss");
            }

            if (operateTaylorLift != null)
            {
                LblId = AppString.lblShipmentId.ToUpper();
                if (!string.IsNullOrEmpty(operateTaylorLift.ShipmentID))
                {
                    LblId = AppString.lblShipmentId.ToUpper();
                    LblNumber = operateTaylorLift.ShipmentID;
                }
                else if (!string.IsNullOrEmpty(operateTaylorLift.CostCenterName))
                {
                    LblId = AppString.lblCostCenter.ToUpper();
                    LblNumber = operateTaylorLift.CostCenterName;
                }

                StartTime = DateTimeParse(operateTaylorLift.ServiceStartDate).HasValue ? DateTimeParse(operateTaylorLift.ServiceStartDate).Value.ToString(DateFormats.StandardHHmmss) : AppString.lblMsgNotRegistered;
                FinishTime = DateTimeParse(operateTaylorLift.ServiceFinishDate).HasValue ? DateTimeParse(operateTaylorLift.ServiceFinishDate).Value.ToString(DateFormats.StandardHHmmss) : AppString.lblMsgNotRegistered;

                EquipmentTypeDesc = operateTaylorLift.EquipmentTypeDesc;
                EquipmentNumber = operateTaylorLift.EquipmentNumber;
                ChassisNo = operateTaylorLift.ChassisNumber;
                EquipmentSizeDesc = operateTaylorLift.EquipmentSizeDesc;

                MoveTypeDesc = AppString.lblServiceType;
                MoveType = operateTaylorLift.MoveTypeDesc;
                MoveStatus = operateTaylorLift.EquipmentStatusDesc;
                Has34 = operateTaylorLift.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
                FromBlock = operateTaylorLift.StartName;
                ToBlock = operateTaylorLift.FinishName;
                Product = operateTaylorLift.ProductDescription;
                DriverComments = operateTaylorLift.DriverComments;

                ElapsedTime = string.IsNullOrEmpty(operateTaylorLift.ServiceFinishDate) ? AppString.lblDailyIndeterminable : (DateTimeParse(operateTaylorLift.ServiceFinishDate).Value.TimeOfDay - DateTimeParse(operateTaylorLift.ServiceStartDate).Value.TimeOfDay).ToString(@"hh\:mm\:ss");
            }

        }

        #region Variables
        private Guid _idItem;
        public Guid IdItem
        {
            get { return _idItem; }
            set { SetProperty(ref _idItem, value); }
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

        private string _moveTypeDesc;
        public string MoveTypeDesc
        {
            get { return _moveTypeDesc; }
            set { SetProperty(ref _moveTypeDesc, value); }
        }

        private string _moveType;
        public string MoveType
        {
            get { return _moveType; }
            set { SetProperty(ref _moveType, value); }
        }

        private string _moveStatus;
        public string MoveStatus
        {
            get { return _moveStatus; }
            set { SetProperty(ref _moveStatus, value); }
        }
        private string _has34;
        public string Has34
        {
            get { return _has34; }
            set { SetProperty(ref _has34, value); }
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

        private string _chassisNo;
        public string ChassisNo
        {
            get { return _chassisNo; }
            set { SetProperty(ref _chassisNo, value); }
        }

        private string _product;
        public string Product
        {
            get { return _product; }
            set { SetProperty(ref _product, value); }
        }
        private string _startTime;
        public string StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }

        private string _finishTime;
        public string FinishTime
        {
            get { return _finishTime; }
            set { SetProperty(ref _finishTime, value); }
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
        #endregion

        #region Methods
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
        #endregion
    }
}
