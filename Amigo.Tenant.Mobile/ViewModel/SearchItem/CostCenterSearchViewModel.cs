using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TSI.Xamarin.Forms.Mvvm.ViewModels;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Event;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Entity.Service;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
using XPO.ShuttleTracking.Mobile.Model;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class CostCenterSearchViewModel : TodayViewModel
    {
        public Action AfterSelectItem { get; set; }
        private readonly INavigator _navigator;
        private readonly ICostCenterRepository _costCenterRepository;
        private readonly IMoveRepository _moveRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IDetentionRepository _detentionRepository;
        private readonly IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private readonly ChangeCostCenterTextHadler _changeTextHadler;
        private const int LimitRegister = 20;

        public CostCenterSearchViewModel(INavigator navigator, 
                                         ICostCenterRepository costCenterRepository,
                                         IMoveRepository moveRepository,
                                         IServiceRepository serviceRepository,
                                         IDetentionRepository detentionRepository,
                                         IOperateTaylorLiftRepository operateTaylorLiftRepository)
        {
            _navigator = navigator;
            _costCenterRepository = costCenterRepository;
            _moveRepository = moveRepository;
            _serviceRepository = serviceRepository;
            _detentionRepository = detentionRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _changeTextHadler = new ChangeCostCenterTextHadler(_costCenterRepository);
            _changeTextHadler.SearchInList += SearchInList;
        }
        
        private int _generalObjectType;
        public int GeneralObjectType
        {
            get { return _generalObjectType; }
            set
            {
                SetProperty(ref _generalObjectType, value);
            }
        }

        private BEMove _generalMove;
        public BEMove GeneralMove
        {
            get { return _generalMove; }
            set { SetProperty(ref _generalMove, value); }
        }

        private BEService _generalService;
        public BEService GeneralService
        {
            get { return _generalService; }
            set { SetProperty(ref _generalService, value); }
        }

        private BEDetention _generalDetention;
        public BEDetention GeneralDetention
        {
            get { return _generalDetention; }
            set { SetProperty(ref _generalDetention, value); }
        }
        private BEOperateTaylorLift _generalOperateTaylorLift;
        public BEOperateTaylorLift GeneralOperateTaylorLift
        {
            get { return _generalOperateTaylorLift; }
            set { SetProperty(ref _generalOperateTaylorLift, value); }
        }
        public override void OnPushed()
        {
            base.OnPushed();
            CostCenter = string.Empty;
            LstCostCenter = new List<CostCenterDTO>();
            Task.Run(() =>
            {
                IsLoading = true;
                LstCostCenter = _costCenterRepository.GetAll().Where(x => !string.IsNullOrEmpty(x.Name)).OrderBy(dto => dto.Name).Take(LimitRegister).ToList();
                IsLoading = false;
            });
        }

        private void SearchInList(object sender, SearchInListEventArgs e)
        {
            LstCostCenter = e.LstCostCenter;            
        }

        public ICommand SetCurrentCostCenter => CreateCommand<CostCenterDTO>((cost) =>
        {
            switch (GeneralObjectType)
            {
                case (int)GeneralObject.Object.Move:
                    GeneralMove.CostCenter = cost.CostCenterId;
                    GeneralMove.CostCenterName = cost.Name;
                    _moveRepository.Update(GeneralMove);                    
                    break;

                case (int)GeneralObject.Object.Service:
                    GeneralService.CostCenter = cost.CostCenterId;
                    GeneralService.CostCenterName = cost.Name;
                    _serviceRepository.Update(GeneralService);                    
                    break;

                case (int)GeneralObject.Object.Detention:
                    GeneralDetention.CostCenter = cost.CostCenterId;
                    GeneralDetention.CostCenterName = cost.Name;
                    _detentionRepository.Update(GeneralDetention);                    
                    break;

                case (int)GeneralObject.Object.OperateTaylorLift:
                    GeneralOperateTaylorLift.CostCenter = cost.CostCenterId;
                    GeneralOperateTaylorLift.CostCenterName = cost.Name;
                    _operateTaylorLiftRepository.Update(GeneralOperateTaylorLift);                    
                    break;
            }

            _navigator.RemoveLastPageFromStack();
            AfterSelectItem();
        });

        private string _costCenter;
        public string CostCenter
        {
            get { return _costCenter; }
            set
            {
                SetProperty(ref _costCenter, value);
                _changeTextHadler.Search(value);
            }
        }
        private IList<CostCenterDTO> _lstCostCenter;
        public IList<CostCenterDTO> LstCostCenter
        {
            get { return _lstCostCenter; }
            set { SetProperty(ref _lstCostCenter, value); }
        }

        public enum ObjectType:int
        {
            Move = 0,
            Service,
            Detention,
            OperateTaylorLift
        }
    }
    
}
