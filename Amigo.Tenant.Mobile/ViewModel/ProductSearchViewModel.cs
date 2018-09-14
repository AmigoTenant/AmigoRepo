using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Event;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
using XPO.ShuttleTracking.Mobile.Entity.Service;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class ProductSearchViewModel : TodayViewModel
    {
        public Action AfterSelectItem { get; set; }
        private readonly INavigator _navigator;
        private readonly IMoveRepository _moveRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IDetentionRepository _detentionRepository;
        private readonly IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IProductRepository _productRepository;
        private ChangeProductTextHandler changeProductTextHadler;
   
        public ProductSearchViewModel(INavigator navigator, 
            IMoveRepository moveRepository,
            IServiceRepository serviceRepository,
            IDetentionRepository detentionRepository,
            ISessionRepository sessionRepository,
            IProductRepository productRepository,
            IOperateTaylorLiftRepository operateTaylorLiftRepository) {

            _navigator = navigator;
            _moveRepository = moveRepository;
            _serviceRepository = serviceRepository;
            _detentionRepository = detentionRepository;
            _sessionRepository = sessionRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _productRepository = productRepository;
            changeProductTextHadler = new ChangeProductTextHandler(_productRepository);
            changeProductTextHadler.SearchProductInList += SearchProductInList;
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
            Product = string.Empty;
            Task.Run(() =>
            {
                IsLoading = true;
                LstProducts = _productRepository.GetAll().Where(x => !string.IsNullOrEmpty(x.Name)).OrderBy(dto => dto.Name).Take(40).ToList();
                IsLoading = false;
            });
        }

        private void SearchProductInList(object sender, SearchProductInListEventArgs e)
        {
            LstProducts = e.LstProducts;
            OnPropertyChanged("LstProducts");
        }

        public ICommand GetSearchProductItem => CreateCommand<ProductDTO>((product) =>
        {

            switch (GeneralObjectType)
            {
                case (int)GeneralObject.Object.Move:
                    GeneralMove.Product = product.ProductId.ToString();
                    GeneralMove.ProductDescription = product.Name;
                     _moveRepository.Update(GeneralMove);                   
                    break;

                case (int)GeneralObject.Object.Service:
                    GeneralService.Product = product.ProductId.ToString();
                    GeneralService.ProductDescription = product.Name;                
                    _serviceRepository.Update(GeneralService);                                       
                    break;

                case (int)GeneralObject.Object.Detention:
                    GeneralDetention.Product = product.ProductId.ToString();
                    GeneralDetention.ProductDescription = product.Name;
                    _detentionRepository.Update(GeneralDetention);
                    break;
                case (int)GeneralObject.Object.OperateTaylorLift:
                    GeneralOperateTaylorLift.Product = product.ProductId.ToString();
                    GeneralOperateTaylorLift.ProductDescription = product.Name;
                    _operateTaylorLiftRepository.Update(GeneralOperateTaylorLift);
                    break;
            }

            _product = product.ShortName;
            _navigator.RemoveLastPageFromStack();
            AfterSelectItem();
        });

        private string _product;
        public string Product
        {
            get { return _product; }
            set
            {
                SetProperty(ref _product, value);
                changeProductTextHadler.SearchProduct(value);
            }
        }

        private IList<ProductDTO> _lstProducts;
        public IList<ProductDTO> LstProducts
        {
            get { return _lstProducts; }
            set { SetProperty(ref _lstProducts, value); }
        }
        
    }
}
