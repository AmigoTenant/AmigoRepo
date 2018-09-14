using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSI.Xamarin.Forms.Persistence.NoSQL.Abstract;
using TSI.Xamarin.Forms.Persistence.Storage;
using Xamarin.Forms;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Common.Constants.XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Entity.Extensions;
using XPO.ShuttleTracking.Mobile.Entity.Tasks.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Infrastructure.CustomException;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.WebServices;
using XPO.ShuttleTracking.Mobile.PubSubEvents;
using Location = XPO.ShuttleTracking.Mobile.Entity.Location;

namespace XPO.ShuttleTracking.Mobile.Services
{
    public class MasterDataDownloaderTask : TaskDefinition
    {
        public override TaskType TaskType { get; protected set; } = TaskType.MasterDataDownloader;
        public bool NeedToClean { get; set; }
    }

    public class MasterDataDownloaderRunner : BackgroundTask<MasterDataDownloaderTask, ResponseDTO>
    {
        private readonly IShuttleServiceClient _serviceClient;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IEquipmentSizeRepository _equipmentSizeRepository;
        private readonly IEquipmentStatusRepository _equipmentStatusRepository;
        private readonly ICostCenterRepository _costCenterRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IActivityTypeRepository _activityTypeRepository;
        private readonly IDispatchingPartyRepository _dispatchingPartyRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPersistentStorageManager _persistentStorageManager;
        private readonly IPlatformFactory _platformFactory;

        public MasterDataDownloaderRunner(IShuttleServiceClient serviceClient,
            IEquipmentTypeRepository equipmentTypeRepository,
            IEquipmentSizeRepository equipmentSizeRepository,
            IEquipmentStatusRepository equipmentStatusRepository,
            ICostCenterRepository costCenterRepository,
            IServiceTypeRepository serviceTypeRepository,
            ILocationRepository locationRepository,
            IActivityTypeRepository activityTypeRepository,
            IDispatchingPartyRepository dispatchingPartyRepository,
            IProductRepository productRepository,
            IPersistentStorageManager persistentStorageManager,
            IPlatformFactory platformFactory)
        {
            _serviceClient = serviceClient;
            _equipmentTypeRepository = equipmentTypeRepository;
            _equipmentSizeRepository = equipmentSizeRepository;
            _equipmentStatusRepository = equipmentStatusRepository;
            _costCenterRepository = costCenterRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _locationRepository = locationRepository;
            _activityTypeRepository = activityTypeRepository;
            _dispatchingPartyRepository = dispatchingPartyRepository;
            _productRepository = productRepository;
            _persistentStorageManager = persistentStorageManager;
            _platformFactory = platformFactory;
        }

        public override TaskType Type { get; protected set; } = TaskType.MasterDataDownloader;
        public override async Task<ResponseDTO> Execute(MasterDataDownloaderTask data)
        {
            if (data.NeedToClean)
            {
                MessagingCenter.Send(this,StopTaskManagerMessage.Name,new StopTaskManagerMessage());

                CleanLastDateSaved();
            }

            var resp = await DownloadInitialData();

            if (!resp.IsValid) return resp;
            
            _persistentStorageManager.AddValue(AppSettings.MasterDataLastUpdate, DateTimeOffset.Now.ToString(DateFormats.MasterDataFormat));

            return resp;
        }

        private void CleanLastDateSaved()
        {
            var urls = new[]
           {
                EquipmentType.Api,
                EquipmentSize.Api,
                EquipmentStatus.Api,
                CostCenter.Api,
                ServiceType.Api,
                Common.Constants.XPO.ShuttleTracking.Mobile.Common.Constants.Location.Api,
                ActivityType.Api,
                Products.Api,
                Dispatching.Api,
                Parameter.Url
            };

            foreach (var url in urls)
            {
                _persistentStorageManager.RemoveValue(url);
            }
        }
        public async Task<ResponseDTO> DownloadInitialData()
        {
            try
            {
                await Task.WhenAll(GetParameters(), GetEquipmentType());
                await Task.WhenAll(GetEquipmentSize(),GetEquipmentStatus());
                await Task.WhenAll(GetAllCostCenter(),GetAllService());
                await Task.WhenAll(GetDispatching(),GetAllLocation());
                await GetAllActivityType();
                await GetAllProduct();

                if (_equipmentTypes != null) _platformFactory.TruncateCollectionFile("EquipmentType.dat");
                if (_equipmentSizes != null) _platformFactory.TruncateCollectionFile("EquipmentSize.dat");
                if (_equipmentStatuses != null) _platformFactory.TruncateCollectionFile("EquipmentStatus.dat");
                if (_costcenters != null) _platformFactory.TruncateCollectionFile("CostCenter.dat");
                if (_services != null) _platformFactory.TruncateCollectionFile("ServiceType.dat");
                if (_locations != null) _platformFactory.TruncateCollectionFile("Location.dat");
                if (_activityTypes != null) _platformFactory.TruncateCollectionFile("ActivityType.dat");
                if (_products != null) _platformFactory.TruncateCollectionFile("Product.dat");
                if (_dispatchingParties != null) _platformFactory.TruncateCollectionFile("Dispatching.dat");

                PersistActivityTypes();
                PersistEquipTypes();
                PersistEquipmentSizes();
                PersistEquipmentStatus();
                PersistCostCenter();
                PersistServiceDataLocally();
                PersistLocations();
                PersistDispatching();
                PersistDataLocally();

                _productRepository.Flush();

                return new ResponseDTO(true);
            }
            catch (ConnectivityException ex)
            {
                Logger.Current.LogWarning($"MasterDataException: {ex}");
                return new ResponseDTO(false);
            }
        }
        private List<EquipmentTypeDTO> _equipmentTypes;
        private async Task GetEquipmentType()
        {
            var serviceResponse = await _serviceClient.GetAsync<ResponseDTO<List<EquipmentTypeDTO>>>(EquipmentType.Api);
            if (!serviceResponse.IsValid) throw new InvalidOperationException(serviceResponse.GetMessage());

            _equipmentTypes = serviceResponse.Data;
        }
        private List<EquipmentSizeDTO> _equipmentSizes;
        private async Task GetEquipmentSize()
        {
            var serviceResponse = await _serviceClient.GetAsync<ResponseDTO<List<EquipmentSizeDTO>>>(EquipmentSize.Api);
            if (!serviceResponse.IsValid) throw new InvalidOperationException(serviceResponse.GetMessage());
            _equipmentSizes = serviceResponse.Data;
        }

        private void PersistEquipmentSizes()
        {
            if(_equipmentSizes==null)return;
            
            foreach (var equip in _equipmentSizes)
                _equipmentSizeRepository.Update(equip,autoflush:false);

            _equipmentSizes = null;
        }

        private List<EquipmentStatusDTO> _equipmentStatuses;
        private async Task GetEquipmentStatus()
        {
            var serviceResponse = await _serviceClient.GetAsync<ResponseDTO<List<EquipmentStatusDTO>>>(EquipmentStatus.Api);
            if (!serviceResponse.IsValid) throw new InvalidOperationException(serviceResponse.GetMessage());

            _equipmentStatuses=serviceResponse.Data;
        }

        private void PersistEquipmentStatus()
        {
            if(_equipmentStatuses==null)return;

            foreach (var equip in _equipmentStatuses)
                _equipmentStatusRepository.Update(equip, autoflush: false);

            _equipmentStatuses = null;
        }
        private List<CostCenterDTO> _costcenters;
        private async Task GetAllCostCenter()
        {
            var serviceResponse = await _serviceClient.GetAsync<ResponseDTO<List<CostCenterDTO>>>(CostCenter.Api);
            if (!serviceResponse.IsValid) throw new InvalidOperationException(serviceResponse.GetMessage());

            _costcenters = serviceResponse.Data;   
        }

        private void PersistCostCenter()
        {
            if (_costcenters == null)return;

            foreach (var row in _costcenters)
                _costCenterRepository.Update(row, autoflush: false);

            _costcenters = null;
        }
        private List<ServiceDTO> _services;
        private async Task GetAllService()
        {
            var serviceResponse = await _serviceClient.GetAsync<ResponseDTO<List<ServiceDTO>>>(ServiceType.Api);
            if (!serviceResponse.IsValid) throw new InvalidOperationException(serviceResponse.GetMessage());

            _services =serviceResponse.Data;
        }

        private void PersistServiceDataLocally()
        {
            if (_services == null) return;

            foreach (var equip in _services)
                _serviceTypeRepository.Update(equip, autoflush: false);

            _services = null;
        }

        private List<Location> _locations;
        private async Task GetAllLocation()
        {
            var serviceResponse = await _serviceClient.GetAsync<ResponseDTO<List<Location>>>(Common.Constants.XPO.ShuttleTracking.Mobile.Common.Constants.Location.Api);
            if (!serviceResponse.IsValid) throw new InvalidOperationException(serviceResponse.GetMessage());

            _locations = serviceResponse.Data;
        }

        private void PersistLocations()
        {
            if (_locations == null) return;

            foreach (var row in _locations)
                _locationRepository.Update(row, autoflush: false);

            _locations = null;
        }

        private List<ActivityTypeDTO> _activityTypes;
        private async Task GetAllActivityType()
        {
            var serviceResponse = await _serviceClient.GetAsync<ResponseDTO<List<ActivityTypeDTO>>>(ActivityType.Api);
            if (!serviceResponse.IsValid) throw new InvalidOperationException(serviceResponse.GetMessage());

            _activityTypes= serviceResponse.Data;
        }

        private void PersistActivityTypes()
        {
            if (_activityTypes == null) return;

            foreach (var row in _activityTypes)
                _activityTypeRepository.Update(row, autoflush: false);

            _activityTypes = null;
        }

        private List<ProductDTO> _products;
        private async Task GetAllProduct()
        {
            var serviceResponse = await _serviceClient.GetAsync<ResponseDTO<PagedList<ProductDTO>>>(Products.Api);
            if (!serviceResponse.IsValid) throw new InvalidOperationException(serviceResponse.GetMessage());

            _products = serviceResponse.Data?.Items.ToList();
        }

        private void PersistDataLocally()
        {
            if(_products==null)return;

            foreach (var row in _products)
                _productRepository.Update(row, autoflush: false);

            _products = null;
        }

        private List<DispatchingPartyDTO> _dispatchingParties;
        private async Task<ResponseDTO> GetDispatching()
        {
            var serviceResponse = await _serviceClient.GetAsync<ResponseDTO<PagedList<DispatchingPartyDTO>>>(Dispatching.Api);
            if (!serviceResponse.IsValid) throw new InvalidOperationException(serviceResponse.GetMessage());

            _dispatchingParties = serviceResponse.Data?.Items.ToList();

            return serviceResponse;
        }

        private void PersistDispatching()
        {
            if(_dispatchingParties == null)return;

            foreach (var row in _dispatchingParties)
                _dispatchingPartyRepository.Update(row,autoflush:false);

            _dispatchingParties = null;
        }

        private void PersistEquipTypes()
        {
            if(_equipmentTypes == null)return;

            foreach (var equip in _equipmentTypes)
                _equipmentTypeRepository.Update(equip, autoflush: false);

            _equipmentTypes = null;
        }

        public async Task GetParameters()
        {
            var serviceResponse = await _serviceClient.GetAsync<ResponseDTO<List<CustomShuttleParameterDTO>>>(Parameter.Url);
            if (!serviceResponse.IsValid) throw new InvalidOperationException(serviceResponse.GetMessage());

            if (serviceResponse.Data == null) return;
            var parameters = serviceResponse.Data.ToDictionary(x => x.Code, y => y.Value);
            Parameters.All = parameters;
            _persistentStorageManager.RemoveValue(Parameter.Url);
        }
    }
}
