using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Common.Extensions;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Commands.Security.Module;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;

namespace Amigo.Tenant.Application.Services.Security
{
    public class MainMenuApplicationService : IMainMenuApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<MainMenuDTO> _mainMenuDataAcces;

        public MainMenuApplicationService(IBus bus,
            IQueryDataAccess<MainMenuDTO> mainMenuDataAcces,
            IMapper mapper)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _mainMenuDataAcces = mainMenuDataAcces;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<IEnumerable<MainMenuDTO>>> SearchMainMenuAsync(MainMenuSearchRequest search)
        {
            List<OrderExpression<MainMenuDTO>> orderExpressionList = new List<OrderExpression<MainMenuDTO>>();
            orderExpressionList.Add(new OrderExpression<MainMenuDTO>(OrderType.Asc, p => p.ParentSortOrder));
            orderExpressionList.Add(new OrderExpression<MainMenuDTO>(OrderType.Asc, p => p.ParentModuleCode));
            orderExpressionList.Add(new OrderExpression<MainMenuDTO>(OrderType.Asc, p => p.SortOrder));
            orderExpressionList.Add(new OrderExpression<MainMenuDTO>(OrderType.Asc, p => p.ModuleCode));

            Expression<Func<MainMenuDTO, bool>> queryFilter = c => true;

            if (search.UserId > 0)
                queryFilter = queryFilter.And(p => p.UserId == search.UserId);

            IEnumerable<MainMenuDTO> mainMenu = await _mainMenuDataAcces.ListAsync(queryFilter, orderExpressionList.ToArray());

            return ResponseBuilder.Correct(mainMenu);
        }
    }
}
