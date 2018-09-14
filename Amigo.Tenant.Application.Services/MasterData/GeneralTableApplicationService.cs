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
    public class GeneralTableApplicationService : IGeneralTableApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<GeneralTableDTO> _generalTableDataAccess;

        public GeneralTableApplicationService(IBus bus,
            IQueryDataAccess<GeneralTableDTO> generalTableDataAccess,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _generalTableDataAccess = generalTableDataAccess;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<List<GeneralTableDTO>>> GetGeneralTableByTableNameAsync(string tableName)
        {
            Expression<Func<GeneralTableDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(tableName))
                queryFilter = queryFilter.And(p => p.TableName == tableName);

            var generalTable = await _generalTableDataAccess.ListAsync(queryFilter);

            return ResponseBuilder.Correct(generalTable.ToList());
        }

        public async Task<GeneralTableDTO> GetGeneralTableByEntityAndCodeAsync(string entityCode, string generalTableCode)
        {
            Expression<Func<GeneralTableDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(entityCode))
                queryFilter = queryFilter.And(p => p.TableName == entityCode);

            if (!string.IsNullOrEmpty(generalTableCode))
                queryFilter = queryFilter.And(p => p.Code == generalTableCode);

            var generalTable = await _generalTableDataAccess.FirstOrDefaultAsync(queryFilter);

            return generalTable;
        }

    }
}
