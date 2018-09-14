using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Events.Tracking;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.EventHandlers.Tracking
{
    public class RequestLogEventHandler : IAsyncNotificationHandler<RequestLogEvent>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<RequestLog> _repository;

        public RequestLogEventHandler(IMapper mapper, IRepository<RequestLog> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public Task Handle(RequestLogEvent notification)
        {
            return Task.Run(() =>
            {
                try
                {
                    //Validate using domain models
                    var entity = _mapper.Map<RequestLogEvent, RequestLog>(notification);

                    //Insert
                    _repository.Add(entity);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            });
        }
    }
}
