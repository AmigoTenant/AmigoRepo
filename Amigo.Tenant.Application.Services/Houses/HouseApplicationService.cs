using Amigo.Tenant.Application.DTOs.Responses.Houses;
using Amigo.Tenant.Application.Services.Interfaces.Houses;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using System.Linq.Expressions;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Leasing;
using Amigo.Tenant.Commands.MasterData.Houses;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Services;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Application.Services.Houses
{
    public class HouseApplicationService : IHouseApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<HouseDTO> _houseDataAccess;
        private readonly IQueryDataAccess<HouseFeatureAndDetailDTO> _houseFeatureAndDetailDataAccess;
        private readonly IQueryDataAccess<HouseBasicDTO> _houseBasicDataAccess;
        private readonly IQueryDataAccess<HouseStatusDTO> _houseStatusDataAccess;
        private readonly IQueryDataAccess<HouseTypeDTO> _houseTypeDataAccess;
        private readonly IQueryDataAccess<HouseFeatureDetailContractDTO> _houseFeatureDetailContractDataAccess;
        private readonly IQueryDataAccess<HouseFeatureDTO> _houseFeatureHouseDataAccess;
        private readonly IQueryDataAccess<HouseServiceDTO> _houseServiceDataAccess;
        private readonly IQueryDataAccess<ServiceHouseDTO> _serviceHouseDataAccess;

        public HouseApplicationService(IBus bus, IQueryDataAccess<HouseDTO> houseDataAccess,
            IMapper mapper,
            IQueryDataAccess<HouseFeatureAndDetailDTO> houseFeatureAndDetailDataAccess,
            IQueryDataAccess<HouseBasicDTO> houseBasicDataAccess,
            IQueryDataAccess<HouseTypeDTO> houseTypeDataAccess,
            IQueryDataAccess<HouseStatusDTO> houseStatusDataAccess,
            IQueryDataAccess<HouseFeatureDetailContractDTO> houseFeatureDetailContractDataAccess,
            IQueryDataAccess<HouseFeatureDTO> houseFeatureHouseDataAccess,
            IQueryDataAccess<HouseServiceDTO> houseServiceDataAccess,
            IQueryDataAccess<ServiceHouseDTO> serviceHouseDataAccess)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _houseDataAccess = houseDataAccess;
            _houseFeatureAndDetailDataAccess = houseFeatureAndDetailDataAccess;
            _mapper = mapper;
            _houseBasicDataAccess = houseBasicDataAccess;
            _houseTypeDataAccess = houseTypeDataAccess;
            _houseStatusDataAccess = houseStatusDataAccess;
            _houseFeatureDetailContractDataAccess = houseFeatureDetailContractDataAccess;
            _houseFeatureHouseDataAccess = houseFeatureHouseDataAccess;
            _houseServiceDataAccess = houseServiceDataAccess;
            _serviceHouseDataAccess = serviceHouseDataAccess;
        }

        public async Task<ResponseDTO<List<HouseDTO>>> SearchHouseAll()
        {
            Expression<Func<HouseDTO, bool>> queryFilter = c => c.RowStatus;
            var list = (await _houseDataAccess.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list);
        }

        public async Task<ResponseDTO<List<HouseFeatureAndDetailDTO>>> SearchHouseFeatureAndDetailAsync(int? houseId, int? contractId)
        {
            List<OrderExpression<HouseFeatureAndDetailDTO>> orderExpressionList = new List<OrderExpression<HouseFeatureAndDetailDTO>>();
            orderExpressionList.Add(new OrderExpression<HouseFeatureAndDetailDTO>(OrderType.Desc, p => p.IsAllHouse));
            orderExpressionList.Add(new OrderExpression<HouseFeatureAndDetailDTO>(OrderType.Asc, p => p.Sequence));
            Expression<Func<HouseFeatureAndDetailDTO, bool>> queryFilter = c => c.HouseId == houseId;
            var list = (await _houseFeatureAndDetailDataAccess.ListAsync(queryFilter, orderExpressionList.ToArray())).ToList();

            list.ForEach(r =>
            {
                r.IsDisabled = r.ContractId.HasValue && contractId != r.ContractId;
            });


            Expression<Func<HouseFeatureDetailContractDTO, bool>> queryFilterDetailContract = c => c.HouseId == houseId;
            var listDetailContract = await _houseFeatureDetailContractDataAccess.ListAsync(queryFilterDetailContract);




            //Deshabilitar Complete "IsAllHouse" porque ha sido alquilada por partes
            var existDisable = list.Any(r => r.IsDisabled.Value);
            if (existDisable)
                list.Where(r => r.IsAllHouse.Value).ToList().ForEach(r => r.IsDisabled = true);

            return ResponseBuilder.Correct(list);
        }

        public async Task<ResponseDTO<List<HouseBasicDTO>>> SearchForTypeAhead(string search)
        {
            Expression<Func<HouseBasicDTO, bool>> queryFilter = c => c.Name.Contains(search);

            var list = (await _houseBasicDataAccess.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list);
        }

        public async Task<ResponseDTO<HouseDTO>> GetById(int? id)
        {
            var house = await GetHouseById(id.GetValueOrDefault());
            return ResponseBuilder.Correct(house);
        }

        private async Task<HouseDTO> GetHouseById(int id)
        {
            Expression<Func<HouseDTO, bool>> queryFilter = c => true;
            queryFilter = queryFilter.And(p => p.HouseId == id);

            var house = await _houseDataAccess.FirstOrDefaultAsync(queryFilter);
            return house;
        }

        public async Task<ResponseDTO<PagedList<HouseDTO>>> Search(HouseSearchRequest search)
        {
            List<OrderExpression<HouseDTO>> orderExpressionList = new List<OrderExpression<HouseDTO>>();
            orderExpressionList.Add(new OrderExpression<HouseDTO>(OrderType.Asc, p => p.Address));

            Expression<Func<HouseDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(search.Code))
                queryFilter = queryFilter.And(p => p.Code.Contains(search.Code));

            if (!string.IsNullOrEmpty(search.Name))
                queryFilter = queryFilter.And(p => p.Name.Contains(search.Name));

            if (!string.IsNullOrEmpty(search.PhoneNumber))
                queryFilter = queryFilter.And(p => p.PhoneNumber.Contains(search.PhoneNumber));

            if (search.HouseTypeId.HasValue)
                queryFilter = queryFilter.And(p => p.HouseTypeId == search.HouseTypeId.Value);

            if (!string.IsNullOrEmpty(search.Address))
                queryFilter = queryFilter.And(p => p.Address.Contains(search.Address));

            if (search.RowStatus.HasValue)
                queryFilter = queryFilter.And(p => p.RowStatus == search.RowStatus);

            if (search.HouseStatusId.HasValue)
                queryFilter = queryFilter.And(p => p.HouseStatusId == search.HouseStatusId.Value);

            var house = await _houseDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());

            var pagedResult = new PagedList<HouseDTO>()
            {
                Items = house.Items,
                PageSize = house.PageSize,
                Page = house.Page,
                Total = house.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<List<HouseStatusDTO>>> GetHouseStatusesAsync()
        {
            var parentHouses = await _houseStatusDataAccess.ListAsync(null);
            var result = parentHouses.ToList();
            return ResponseBuilder.Correct(result);
        }

        public async Task<ResponseDTO<List<HouseTypeDTO>>> GetHouseTypesAsync()
        {
            var houseTypes = await _houseTypeDataAccess.ListAsync(null);
            var result = houseTypes.ToList();
            return ResponseBuilder.Correct(result);
        }

        public async Task<ResponseDTO> RegisterHouseAsync(RegisterHouseRequest newHouse)
        {
            //Map to Command
            var command = _mapper.Map<RegisterHouseRequest, RegisterHouseCommand>(newHouse);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp, command.HouseId, command.Code);
            //return resp.ToResponse();
        }

        public async Task<ResponseDTO> UpdateHouseAsync(UpdateHouseRequest house)
        {
            //Map to Command
            var command = _mapper.Map<UpdateHouseRequest, UpdateHouseCommand>(house);

            //Execute Command
            var resp = await _bus.SendAsync(command);
            return ResponseBuilder.Correct(resp, command.HouseId, command.Code);

            //return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO> DeleteHouseAsync(DeleteHouseRequest house)
        {
            //Map to Command
            var command = _mapper.Map<DeleteHouseRequest, DeleteHouseCommand>(house);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO<HouseDTO>> GetHouseAsync(GetHouseRequest request)
        {
            var house = await GetHouseById(request.Id);
            return ResponseBuilder.Correct(house);

        }

        public async Task<ResponseDTO<List<HouseFeatureDTO>>> GetFeaturesByHouseAsync(int houseId)
        {
            Expression<Func<HouseFeatureDTO, bool>> queryFilter = c => c.RowStatus == true && c.HouseId == houseId;

            var list = (await _houseFeatureHouseDataAccess.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list);
        }

        public async Task<ResponseDTO> RegisterHouseFeatureAsync(HouseFeatureRequest newHouseFeature)
        {
            //Map to Command
            var command = _mapper.Map<HouseFeatureRequest, RegisterHouseFeatureCommand>(newHouseFeature);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
            //.ToResponse();
        }

        public async Task<ResponseDTO> UpdateHouseFeatureAsync(HouseFeatureRequest houseFeature)
        {
            //Map to Command
            var command = _mapper.Map<HouseFeatureRequest, UpdateHouseFeatureCommand>(houseFeature);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO> DeleteHouseFeatureAsync(DeleteHouseFeatureRequest houseFeature)
        {
            //Map to Command
            var command = _mapper.Map<DeleteHouseFeatureRequest, DeleteHouseFeatureCommand>(houseFeature);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO<List<HouseServiceDTO>>> GetHouseServiceByHouseAsync(int houseId)
        {
            Expression<Func<HouseServiceDTO, bool>> queryFilter = c => c.HouseId == houseId;
            var houseServices = (await _houseServiceDataAccess.ListAsync(queryFilter)).ToList();

            var list = GroupByHouseService(houseServices);
            return ResponseBuilder.Correct(list);
        }

        private List<HouseServiceDTO> GroupByHouseService(List<HouseServiceDTO> houseServices)
        {
            return houseServices
                .GroupBy(p => new
                {
                    p.HouseId,
                    p.HouseServiceId,
                    p.ServiceId,
                    p.ConceptId,
                    p.ConceptCode,
                    p.ConceptDescription,
                    p.ConceptTypeId,
                    p.BusinessPartnerId,
                    p.BusinessPartnerName,
                    p.ServiceTypeCode,
                    p.ServiceTypeId,
                    p.ServiceTypeValue,
                    p.CreatedBy,
                    p.UpdatedBy,
                    p.RowStatus
                })
                .Select(p => new HouseServiceDTO()
                {
                    HouseId = p.Key.HouseId,
                    HouseServiceId = p.Key.HouseServiceId,
                    ServiceId = p.Key.ServiceId,
                    BusinessPartnerId = p.Key.BusinessPartnerId,
                    BusinessPartnerName = p.Key.BusinessPartnerName,
                    ConceptId = p.Key.ConceptId,
                    ConceptCode = p.Key.ConceptCode,
                    ConceptDescription = p.Key.ConceptDescription,
                    ConceptTypeId = p.Key.ConceptTypeId,
                    ServiceTypeId = p.Key.ServiceTypeId,
                    ServiceTypeCode = p.Key.ServiceTypeCode,
                    ServiceTypeValue = p.Key.ServiceTypeValue,
                    RowStatus = p.Key.RowStatus,
                    CreatedBy = p.Key.CreatedBy,
                    CreationDate = p.Max(q => q.CreationDate),
                    UpdatedBy = p.Key.UpdatedBy,
                    UpdatedDate = p.Max(q => q.UpdatedDate),
                    HouseServicePeriods = houseServices
                                        .Where(q => q.HouseId == p.Key.HouseId 
                                                && q.HouseServiceId == p.Key.HouseServiceId 
                                                && q.ServiceId == p.Key.ServiceId)
                                        .Select(q => new HouseServicePeriodDTO()
                                        {
                                            HouseServiceId = q.HouseServiceId,
                                            HouseServicePeriodId = q.HouseServicePeriodId,
                                            MonthId = q.MonthId,
                                            DueDateMonth = q.DueDateMonth,
                                            CutOffMonth = q.CutOffMonth,
                                            DueDateDay = q.DueDateDay,
                                            CutOffDay = q.CutOffDay,
                                            RowStatus = q.RowStatus,
                                            CreatedBy = q.CreatedBy,
                                            CreationDate = q.CreationDate,
                                            UpdatedBy = q.UpdatedBy,
                                            UpdatedDate = q.UpdatedDate
                                        }).ToList()
                }).ToList();
        }

        public async Task<ResponseDTO<List<ServiceHouseDTO>>> GetServicesHouseAllAsync()
        {
            Expression<Func<ServiceHouseDTO, bool>> masterQueryFilter = c => c.RowStatus;

            var list = (await _serviceHouseDataAccess.ListAsync(masterQueryFilter)).ToList();

            return ResponseBuilder.Correct(list);
        }

        public async Task<ResponseDTO> RegisterHouseServiceAsync(HouseServiceRequest newHouseService)
        {
            //Map to Command
            var command = _mapper.Map<HouseServiceRequest, RegisterHouseServiceCommand>(newHouseService);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
            //.ToResponse();
        }

        public async Task<ResponseDTO> UpdateHouseServiceAsync(HouseServiceRequest houseService)
        {
            //Map to Command
            var command = _mapper.Map<HouseServiceRequest, UpdateHouseServiceCommand>(houseService);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO> DeleteHouseServiceAsync(DeleteHouseServiceRequest houseService)
        {
            //Map to Command
            var command = _mapper.Map<DeleteHouseServiceRequest, DeleteHouseServiceCommand>(houseService);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO<List<ServiceHouseDTO>>> GetHouseServicesNewAsync()
        {
            Expression<Func<ServiceHouseDTO, bool>> serviceHouseFilter = c => c.RowStatus;

            //Expression<Func<ServiceHouseDTO, ServiceHouseDTO>> proj = 
            //    p => new ServiceHouseDTO() { BusinessPartnerId = p.BusinessPartnerId , BusinessPartnerName = p.BusinessPartnerName ,
            //        ConceptCode = p.ConceptCode , ConceptDescription = p.ConceptDescription , ConceptId = p.ConceptId , ConceptTypeId =  p.ConceptTypeId,
            //     CreatedBy = p.CreatedBy , CreationDate = p.CreationDate , RowStatus = p.RowStatus, ServiceId = p.ServiceId ,
            //        ServiceTypeCode = p.ServiceTypeCode , ServiceTypeId = p.ServiceTypeId , ServiceTypeValue = p.ServiceTypeValue ,
            //        UpdatedBy = p.UpdatedBy  , UpdatedDate = p.UpdatedDate ,
            //     ServiceHousePeriods = p. };

            var serviceHousePeriods = (await _serviceHouseDataAccess.ListAsync(serviceHouseFilter)).ToList();

            // Group Periods by Services
            var services = serviceHousePeriods
                .GroupBy(p => new
                {
                    p.ServiceId,
                    p.ConceptId,
                    p.ConceptCode,
                    p.ConceptDescription,
                    p.ConceptTypeId,
                    p.BusinessPartnerId,
                    p.BusinessPartnerName,
                    p.ServiceTypeCode,
                    p.ServiceTypeId,
                    p.ServiceTypeValue,
                    p.CreatedBy,
                    p.UpdatedBy,
                    p.RowStatus
                })
                .Select(p => new ServiceHouseDTO()
                {
                    ServiceId = p.Key.ServiceId,
                    BusinessPartnerId = p.Key.BusinessPartnerId,
                    BusinessPartnerName = p.Key.BusinessPartnerName,
                    ConceptId = p.Key.ConceptId,
                    ConceptCode = p.Key.ConceptCode,
                    ConceptDescription = p.Key.ConceptDescription,
                    ConceptTypeId = p.Key.ConceptTypeId,
                    ServiceTypeId = p.Key.ServiceTypeId,
                    ServiceTypeCode = p.Key.ServiceTypeCode,
                    ServiceTypeValue = p.Key.ServiceTypeValue,
                    RowStatus = p.Key.RowStatus,
                    CreatedBy = p.Key.CreatedBy,
                    CreationDate = p.Max(q => q.CreationDate),
                    UpdatedBy = p.Key.UpdatedBy,
                    UpdatedDate = p.Max(q => q.UpdatedDate),
                    ServiceHousePeriods = serviceHousePeriods
                                        .Where(q => q.ServiceId == p.Key.ServiceId)
                                        .Select(q => new ServiceHousePeriodDTO()
                                        {
                                            ServiceId = q.ServiceId,
                                            MonthId = q.MonthId,
                                            DueDateMonth = q.DueDateMonth,
                                            CutOffMonth = q.CutOffMonth,
                                            DueDateDay = q.DueDateDay,
                                            CutOffDay = q.CutOffDay,
                                            RowStatus = q.RowStatus,
                                            CreatedBy = q.CreatedBy,
                                            CreationDate = q.CreationDate,
                                            UpdatedBy = q.UpdatedBy,
                                            UpdatedDate = q.UpdatedDate
                                        }).ToList()
                });

            //var list = new List<HouseServiceDTO>();
            //foreach(var servHouse in serviceHouses)
            //{
            //    var periods = HouseService.CreatePeriods(servHouse.ServiceId);
            //    list = _mapper.Map<List<HouseService>, List<HouseServiceDTO>>(periods);
            //    foreach(var h in list)
            //    {
            //        var houseServ = h;
            //        houseServ.BusinessPartnerId = servHouse.BusinessPartnerId;
            //        houseServ.BusinessPartnerName = servHouse.BusinessPartnerName;
            //        houseServ.ConceptCode = servHouse.ConceptCode;
            //        houseServ.ConceptDescription = servHouse.ConceptDescription;
            //        houseServ.ConceptId = servHouse.ConceptId;
            //        houseServ.ConceptTypeId = servHouse.ConceptTypeId;
            //        houseServ.ServiceTypeCode = servHouse.ServiceTypeCode;
            //        houseServ.ServiceTypeId = servHouse.ServiceTypeId;
            //        houseServ.ServiceTypeValue = servHouse.ServiceTypeValue;
            //    }
            //}

            return ResponseBuilder.Correct(services.ToList());
        }
    }
}
