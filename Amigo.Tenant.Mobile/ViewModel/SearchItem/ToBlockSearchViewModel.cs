using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TSI.Xamarin.Forms.Mvvm.ViewModels;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Common.Util;
using XPO.ShuttleTracking.Mobile.Entity;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
using XPO.ShuttleTracking.Mobile.Event;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Entity.Service;
using XPO.ShuttleTracking.Mobile.Model;

namespace XPO.ShuttleTracking.Mobile.ViewModel.SearchItem
{
    public class ToBlockSearchViewModel : TodayViewModel
    {
        public Action AfterSelectItem { get; set; }

        private readonly INavigator _navigator;
        private readonly ILocationRepository _locationRepository;
        private readonly IMoveRepository _moveRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IDetentionRepository _detentionRepository;
        private readonly IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private volatile ChangeBlockTextHadler _changeBlockTextHadler;
        private const int LimitRegister = 20;
        public ToBlockSearchViewModel(INavigator navigator,
                                            ILocationRepository locationRepository,
                                            IMoveRepository moveRepository,
                                            IDetentionRepository detentionRepository,
                                            IServiceRepository serviceRepository,
                                            IOperateTaylorLiftRepository operateTaylorLiftRepository)
        {
            _navigator = navigator;
            _locationRepository = locationRepository;
            _moveRepository = moveRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _serviceRepository = serviceRepository;
            _detentionRepository = detentionRepository;
            _changeBlockTextHadler = new ChangeBlockTextHadler(_locationRepository);
            _changeBlockTextHadler.SearchInList += SearchInList;
        }
        private BEMove _generalMove;
        public BEMove GeneralMove
        {
            get { return _generalMove; }
            set { SetProperty(ref _generalMove, value); }
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
            Block = string.Empty;
            Task.Run(() =>
            {
                try
                {
                    IsLoading = true;
                    var repo = _locationRepository.GetAllSortedByName().Take(LimitRegister).ToList();
                    LstBlock = repo;
                    IsLoading = false;
                }
                catch (Exception ex)
                {
                    ShowOkAlert(ex.ToString());
                }
            });

        }

        private void SearchInList(object sender, SearchBlockInListEventArgs e)
        {
            LstBlock = e.LstBlock;
            OnPropertyChanged("LstBlock");
        }

        public ICommand SetCurrentToBlock => CreateCommand<Location>((block) =>
        {
            switch (_generalObjectType)
            {
                case (int)GeneralObject.Object.Move:
                    GeneralMove.Finish = block.LocationId.ToString();
                    GeneralMove.FinishName = block.Name;
                    _moveRepository.Update(GeneralMove);
                    break;

                case (int)GeneralObject.Object.Service:
                    GeneralService.Finish = block.LocationId.ToString();
                    GeneralService.FinishName = block.Name;
                    _serviceRepository.Update(GeneralService);
                    break;

                case (int)GeneralObject.Object.Detention:
                    GeneralDetention.Finish = block.LocationId.ToString();
                    GeneralDetention.FinishName = block.Name;
                    _detentionRepository.Update(GeneralDetention);
                    break;
                case (int)GeneralObject.Object.OperateTaylorLift:
                    GeneralOperateTaylorLift.Finish = block.LocationId.ToString();
                    GeneralOperateTaylorLift.FinishName = block.Name;
                    _operateTaylorLiftRepository.Update(GeneralOperateTaylorLift);
                    break;
            }

            Block = string.Empty;
            LstBlock = new List<Location>();
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
                _changeBlockTextHadler.Search(value);
            }
        }

        private IList<Location> _lstBlock;
        public IList<Location> LstBlock
        {
            get { return _lstBlock; }
            set { SetProperty(ref _lstBlock, value); }
        }
        private string _block;
        public string Block
        {
            get { return _block; }
            set
            {
                SetProperty(ref _block, value);
                _changeBlockTextHadler.Search(value);
            }
        }
    }
}
