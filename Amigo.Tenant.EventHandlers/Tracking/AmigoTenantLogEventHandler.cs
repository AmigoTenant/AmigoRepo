using System;
using System.Threading.Tasks;
using MediatR;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Events.Tracking;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Common;

namespace Amigo.Tenant.EventHandlers.Tracking
{
    public class AmigoTenantLogEventHandler:IAsyncNotificationHandler<RegisterMoveEvent>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork  _unitOfWork;
        private readonly IRepository<AmigoTenantTEventLog> _repository;

        public AmigoTenantLogEventHandler(IMapper mapper,IUnitOfWork unitOfWork, IRepository<AmigoTenantTEventLog> repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task Handle(RegisterMoveEvent notification)
        {

            //Validate using domain models
            var entity = _mapper.Map<RegisterMoveEvent, Amigo.Tenant.CommandModel.Models.AmigoTenantTEventLog>(notification);
            entity.RowStatus = true;
            entity.CreationDate = DateTime.UtcNow;
            entity.ConvertedActivityUTC = DatetimeToDateUTC(entity.ReportedActivityDate);
            
            //Insert
            _repository.Add(entity);

            await _unitOfWork.CommitAsync();            
        }

        public DateTime DatetimeToDateUTC(DateTimeOffset? date)
        {
            string datetime = date.ToString();
            var dtOffset = DateTimeOffset.Parse(datetime);

            return dtOffset.UtcDateTime;
        }


    }
}
