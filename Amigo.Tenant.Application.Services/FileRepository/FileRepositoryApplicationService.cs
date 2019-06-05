using Amigo.Tenant.Application.DTOs.FileRepository.Requests;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.FileRepository;
using Amigo.Tenant.Application.Services.Interfaces.FileRepository;
using Amigo.Tenant.Commands.FileRepository;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.FileRepository
{
    public class FileRepositoryApplicationService : IFileRepositoryApplicationService
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        //private readonly IQueryDataAccess<ExpenseDetailRegisterRequest> _expenseDetailDataAccess;
        //private readonly IEntityStatusApplicationService _entityStatusApplicationService;
        private readonly IQueryDataAccess<FileRepositoryDTO> _repositoryFileRepository;
        private readonly IQueryDataAccess<FileRepositoryEntityDTO> _repositoryFileRepositoryEntity;

        public FileRepositoryApplicationService(IBus bus,
            //IQueryDataAccess<ExpenseDetailSearchDTO> expenseDetailSearchDataAccess,
            //IEntityStatusApplicationService entityStatusApplicationService,
            IMapper mapper ,
            IQueryDataAccess<FileRepositoryDTO> repositoryFileRepository,
            IQueryDataAccess<FileRepositoryEntityDTO> repositoryFileRepositoryEntity
            )
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _mapper = mapper;
            _repositoryFileRepository = repositoryFileRepository;
            _repositoryFileRepositoryEntity = repositoryFileRepositoryEntity;
            //_generalTableApplicationService = generalTableApplicationService;
            //_contractDtoDataAccess  = contractDtoDataAccess;
            //_repositoryPaymentPeriod = repositoryPaymentPeriod;
        }

        public async Task<bool> DeleteAsync(int fileRepositoryId)
        {
            try
            {
                var command = new FileRepositoryDeleteCommand()
                {
                    FileRepositoryId = fileRepositoryId
                };
                //Execute Command
                var resp = await _bus.SendAsync(command);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
            //Map to Command
            return false;
        }

        public async Task<ResponseDTO<PagedList<FileRepositoryDTO>>> GetFileRepositoriesAsync(string entityCode, int? parentId)
        {
            List<OrderExpression<FileRepositoryDTO>> orderExpressionList = new List<OrderExpression<FileRepositoryDTO>>();
            orderExpressionList.Add(new OrderExpression<FileRepositoryDTO>(OrderType.Desc, p => p.CreationDate));
            Expression<Func<FileRepositoryDTO, bool>> queryFilter = c => true;

            queryFilter = queryFilter.And(p => p.ParentId.Value == parentId && p.EntityCode == entityCode);

            var expense = await _repositoryFileRepository.ListPagedAsync(queryFilter, 1, 20, orderExpressionList.ToArray());

            var pagedResult = new PagedList<FileRepositoryDTO>()
            {
                Items = expense.Items,
                PageSize = expense.PageSize,
                Page = expense.Page,
                Total = expense.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<FileRepositoryEntityDTO> GetFileRepositoryByIdAsync(int fileRepositoryId)
        {
            Expression<Func<FileRepositoryEntityDTO, bool>> queryFilter = c => c.FileRepositoryId == fileRepositoryId;
            var fileRepository = await _repositoryFileRepositoryEntity.FirstOrDefaultAsync(queryFilter);
            return fileRepository;
        }

        public async Task<ResponseDTO> RegisterAsync(FileRepositoryEntityDTO fileRepositoryEntityDtoRequest)
        {
            var command = _mapper.Map<FileRepositoryEntityDTO, FileRepositoryRegisterCommand>(fileRepositoryEntityDtoRequest);
            var resp = await _bus.SendAsync(command);
            return ResponseBuilder.Correct(resp, command.ParentId, "");
        }
    }
}
