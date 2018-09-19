using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Application.Services.MasterData
{
    public class AppSettingApplicationService : IAppSettingApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<AppSettingDTO> _AppSettingDataAccess;

        public AppSettingApplicationService(IBus bus,
            IQueryDataAccess<AppSettingDTO> AppSettingDataAccess,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _AppSettingDataAccess = AppSettingDataAccess;
            _mapper = mapper;
        }

        public async Task<AppSettingDTO> GetAppSettingByCodeAsync(string AppSettingCode)
        {
            Expression<Func<AppSettingDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(AppSettingCode))
                queryFilter = queryFilter.And(p => p.Code == AppSettingCode);

            var AppSetting = await _AppSettingDataAccess.FirstOrDefaultAsync(queryFilter);

            return AppSetting;
        }

    }
}
