using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{

    public interface ILocationApplicationService
    {
        Task<ResponseDTO<PagedList<LocationDTO>>> SearchLocationAsync(LocationSearchRequest search);
        Task<ResponseDTO<List<LocationCoordinateDTO>>> ListCoordinatesAsync(LocationCoordinatesListRequest search);

        Task<ResponseDTO<List<CityDTO>>> GetCitiesAsync();

        Task<ResponseDTO<List<ParentLocationDTO>>> GetParentLocationsAsync();

        Task<ResponseDTO<List<LocationTypeDTO>>> GetLocationTypesAsync();

        Task<ResponseDTO<List<LocationDTO>>> SearchLocationAll();

        Task<ResponseDTO<List<LocationTypeAheadDTO>>> SearchLocationAllTypeAhead();

        Task<ResponseDTO<LocationWithCoordinatesDTO>> GetLocationAsync(GetLocationRequest getRequest);

        Task<ResponseDTO> RegisterLocationAsync(RegisterLocationRequest newLocation);

        Task<ResponseDTO> UpdateLocationAsync(UpdateLocationRequest location);


        Task<ResponseDTO> DeleteLocationAsync(DeleteLocationRequest location);

        Task<ResponseDTO> RegisterLocationCoordinatesAsync(RegisterLocationCoordinatesRequest newCoordinates);

        Task<ResponseDTO> DeleteLocationCoordinatesAsync(DeleteLocationCoordinatesRequest location);

        Task<ResponseDTO<List<LocationTypeAheadDTO>>> SearchLocationAllTypeAhead(string name);
        Task<LocationTypeAheadDTO> SearchLocationByNameAsync(string name);
    }
}
