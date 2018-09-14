using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.Services.Interfaces.Security
{
    public interface IDeviceApplicationService
    {
        Task<ResponseDTO<PagedList<DeviceDTO>>> SearchDeviceAsync(DeviceSearchRequest search);

        Task<ResponseDTO> RegisterDeviceAsync(RegisterDeviceRequest newDevice);

        Task<ResponseDTO> UpdateDeviceAsync(UpdateDeviceRequest device);

        Task<ResponseDTO> DeleteDeviceAsync(DeleteDeviceRequest device);

        Task<ResponseDTO<List<ModelDTO>>> GetModelsAsync();

        Task<ResponseDTO<List<OSVersionDTO>>> GetOSVersionsAsync();

        Task<ResponseDTO<List<AppVersionDTO>>> GetAppVersionsAsync();

    }

}
