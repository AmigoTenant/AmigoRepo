using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Houses;
using Amigo.Tenant.Application.DTOs.Responses.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.Interfaces.Houses
{
    public interface IHouseApplicationService
    {
        Task<ResponseDTO<List<HouseDTO>>> SearchHouseAll();
        Task<ResponseDTO<List<HouseFeatureAndDetailDTO>>> SearchHouseFeatureAndDetailAsync(int? houseId, int? contractId);
        Task<ResponseDTO<List<HouseBasicDTO>>> SearchForTypeAhead(string search);
        Task<ResponseDTO<HouseDTO>> GetById(int? id);
        Task<ResponseDTO<PagedList<HouseDTO>>> Search(HouseSearchRequest search);
        Task<ResponseDTO<List<HouseStatusDTO>>> GetHouseStatusesAsync();
        Task<ResponseDTO<List<HouseTypeDTO>>> GetHouseTypesAsync();

        Task<ResponseDTO> RegisterHouseAsync(RegisterHouseRequest newHouse);
        Task<ResponseDTO> UpdateHouseAsync(UpdateHouseRequest house);
        Task<ResponseDTO> DeleteHouseAsync(DeleteHouseRequest house);

        Task<ResponseDTO<HouseDTO>> GetHouseAsync(GetHouseRequest request);
        Task<ResponseDTO<List<HouseFeatureDTO>>> GetFeaturesByHouseAsync(int houseId);

        Task<ResponseDTO> RegisterHouseFeatureAsync(HouseFeatureRequest newHouseFeature);
        Task<ResponseDTO> UpdateHouseFeatureAsync(HouseFeatureRequest houseFeature);
        Task<ResponseDTO> DeleteHouseFeatureAsync(DeleteHouseFeatureRequest houseFeature);

        Task<ResponseDTO<List<HouseServiceDTO>>> GetHouseServiceByHouseAsync(int houseId);
        Task<ResponseDTO<List<ServiceHouseDTO>>> GetServicesHouseAllAsync();
        Task<ResponseDTO<List<ServiceHouseDTO>>> GetHouseServicesNewAsync();

        Task<ResponseDTO> RegisterHouseServiceAsync(HouseServiceRequest newHouseService);
        Task<ResponseDTO> UpdateHouseServiceAsync(HouseServiceRequest houseService);
        Task<ResponseDTO> DeleteHouseServiceAsync(DeleteHouseServiceRequest houseService);
    }
}
