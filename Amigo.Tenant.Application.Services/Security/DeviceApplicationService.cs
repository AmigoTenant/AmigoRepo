using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using System.Linq;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Commands.Security.Device;

namespace Amigo.Tenant.Application.Services.Security
{


    public class DeviceApplicationService : IDeviceApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<DeviceDTO> _deviceDataAccess;
        private readonly IQueryDataAccess<ModelDTO> _modelDataAccess;
        private readonly IQueryDataAccess<OSVersionDTO> _osVersionDataAccess;
        private readonly IQueryDataAccess<AppVersionDTO> _appVersionDataAccess;



        public DeviceApplicationService(IBus bus,
            IQueryDataAccess<DeviceDTO> deviceDataAccess,
            IQueryDataAccess<ModelDTO> modelDataAccess,
            IQueryDataAccess<OSVersionDTO> osVersionDataAccess,
            IQueryDataAccess<AppVersionDTO> appVersionDataAccess,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _deviceDataAccess = deviceDataAccess;
            _modelDataAccess = modelDataAccess;
            _osVersionDataAccess = osVersionDataAccess;
            _appVersionDataAccess = appVersionDataAccess;
            _mapper = mapper;
        }

      

        public async Task<ResponseDTO<PagedList<DeviceDTO>>> SearchDeviceAsync(DeviceSearchRequest search)
        {

            var queryFilter = GetQueryFilter(search);

            var device = await _deviceDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);

            var pagedResult = new PagedList<DeviceDTO>()
            {

                Items = device.Items,
                PageSize = device.PageSize,
                Page = device.Page,
                Total = device.Total

            };

            return ResponseBuilder.Correct(pagedResult);
        }

   
    

        public async Task<ResponseDTO> RegisterDeviceAsync(RegisterDeviceRequest newDevice)
        {

            //Map to Command
            var command = _mapper.Map<RegisterDeviceRequest, RegisterDeviceCommand>(newDevice);
            
            var response = await ValidateCellphoneAndUser(newDevice.CellphoneNumber, newDevice.AssignedAmigoTenantTUserId);

            if (response.IsValid)
            {
                //Execute Command
                var resp = await _bus.SendAsync(command);
                ResponseBuilder.Correct(resp);
            }

            return response;
        }

        public async Task<ResponseDTO> UpdateDeviceAsync(UpdateDeviceRequest device)
        {
            //Map to Command
            var command = _mapper.Map<UpdateDeviceRequest, UpdateDeviceCommand>(device);

            var response = await ValidateCellphoneAndUser(device.CellphoneNumber, device.AssignedAmigoTenantTUserId);
            
            if (response.IsValid)
            {
                //Execute Command
                var resp = await _bus.SendAsync(command);
                ResponseBuilder.Correct(resp);
            }

            return response;
        }

        public async Task<ResponseDTO> ValidateCellphoneAndUser(string cellphoneNumber, int? userid)
        {
            Expression<Func<DeviceDTO, bool>> queryFilter = p => true;
            var result = "";
            if (userid.HasValue)
            {
                queryFilter = queryFilter.And(p => p.CellphoneNumber != cellphoneNumber.Trim());
                queryFilter = queryFilter.And(p => p.AssignedAmigoTenantTUserId == userid);
                var device = await _deviceDataAccess.FirstOrDefaultAsync(queryFilter);

                if (device != null)
                    result = "The cellphone has another user associated or the user is assigned to another device";
                else
                {
                    Expression<Func<DeviceDTO, bool>> queryFilter1 = p => true;
                    queryFilter1 = queryFilter1.And(p => p.CellphoneNumber != cellphoneNumber.Trim());
                    queryFilter1 = queryFilter1.And(p => p.AssignedAmigoTenantTUserId == userid);
                    device = await _deviceDataAccess.FirstOrDefaultAsync(queryFilter1);

                    if (device != null)
                        result = "The user has another device associated";
                }
            }
           
            var response = new ResponseDTO()
            {
                IsValid = string.IsNullOrEmpty(result),
                Messages = new List<ApplicationMessage>()
            };

            response.Messages.Add(new ApplicationMessage()
            {
                Key = string.IsNullOrEmpty(result)? "Ok":"Error",
                Message = result
            });

            return response;
        }

        public async Task<ResponseDTO> DeleteDeviceAsync(DeleteDeviceRequest device)
        {
            //Map to Command
            var command = _mapper.Map<DeleteDeviceRequest, DeleteDeviceCommand>(device);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }




        public async Task<ResponseDTO<List<ModelDTO>>> GetModelsAsync()
        {
            var models = await _modelDataAccess.ListAsync(null);
            var result = models.ToList();
            return ResponseBuilder.Correct(result);
        }


        public async Task<ResponseDTO<List<OSVersionDTO>>> GetOSVersionsAsync()
        {
            var osVersions = await _osVersionDataAccess.ListAsync(null);
            var result = osVersions.ToList();
            return ResponseBuilder.Correct(result);
        }

        public async Task<ResponseDTO<List<AppVersionDTO>>> GetAppVersionsAsync()
        {
            var appVersions = await _appVersionDataAccess.ListAsync(null);
            var result = appVersions.ToList();
            return ResponseBuilder.Correct(result);
        }

 


        #region Helpers

        private Expression<Func<DeviceDTO, bool>> GetQueryFilter(DeviceSearchRequest search)
        {
            Expression<Func<DeviceDTO, bool>> queryFilter = p => true;

            if (search.DeviceId != null)
            {
                queryFilter = queryFilter.And(p => p.DeviceId == search.DeviceId);
            }

            if (!string.IsNullOrEmpty(search.Identifier))
            {
                queryFilter = queryFilter.And(p =>  p.Identifier == search.Identifier );
            }

            if (!string.IsNullOrEmpty(search.WIFIMAC))
            {
                queryFilter = queryFilter.And(p => p.WIFIMAC == search.WIFIMAC);
            }

            if (!string.IsNullOrEmpty(search.CellphoneNumber))
            {
                queryFilter = queryFilter.And(p => p.CellphoneNumber == search.CellphoneNumber);
            }

            if (search.OSVersionId != null)
            {
                queryFilter = queryFilter.And(p => p.OSVersionId == search.OSVersionId);
            }

            if (search.PlatformId != null)
            {
                queryFilter = queryFilter.And(p => p.PlatformId == search.PlatformId);
            }

            if (search.AppVersionId != null)
            {
                queryFilter = queryFilter.And(p => p.AppVersionId == search.AppVersionId);
            }

            if (search.ModelId != null)
            {
                queryFilter = queryFilter.And(p => p.ModelId == search.ModelId);
            }

            if (search.BrandId != null)
            {
                queryFilter = queryFilter.And(p => p.BrandId == search.BrandId);
            }

            if (search.IsAutoDateTime != null)
            {
                queryFilter = queryFilter.And(p => p.IsAutoDateTime == search.IsAutoDateTime);
            }

            if (search.IsSpoofingGPS != null)
            {
                queryFilter = queryFilter.And(p => p.IsSpoofingGPS == search.IsSpoofingGPS);
            }

            if (search.IsRootedJailbreaked != null)
            {
                queryFilter = queryFilter.And(p => p.IsRootedJailbreaked == search.IsRootedJailbreaked);
            }

            if (search.AssignedAmigoTenantTUserId != null)
            {
                queryFilter = queryFilter.And(p => p.AssignedAmigoTenantTUserId == search.AssignedAmigoTenantTUserId);
            }

            if (search.RowStatus != null)
            {
                queryFilter = queryFilter.And(p => p.RowStatus == search.RowStatus);
            }

            return queryFilter;
        }


        #endregion
    }



}
