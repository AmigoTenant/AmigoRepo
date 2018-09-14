using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;

namespace Amigo.Tenant.Infrastructure.EventSourcing
{
    public class MediatRBus: IBus
    {
        private readonly IMediator _mediator;

        public MediatRBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public TResponse Send<TResponse>(IRequest<TResponse> request)
        {
            return _mediator.Send(request);
        }

        public Task<TResponse> SendAsync<TResponse>(IAsyncRequest<TResponse> request)
        {
            return _mediator.SendAsync(request);
        }

        public Task<TResponse> SendAsync<TResponse>(ICancellableAsyncRequest<TResponse> request, CancellationToken cancellationToken)
        {
            return _mediator.SendAsync(request,cancellationToken);
        }

        public void Publish(INotification notification)
        {
            _mediator.Publish(notification);
        }

        public Task PublishAsync(IAsyncNotification notification)
        {
           return _mediator.PublishAsync(notification);
        }

        public Task PublishAsync(ICancellableAsyncNotification notification, CancellationToken cancellationToken)
        {
            return _mediator.PublishAsync(notification,cancellationToken);
        }
    }
}
