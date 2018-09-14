using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Amigo.Tenant.Infrastructure.EventSourcing.Abstract
{
    public interface IBus
    {
        TResponse Send<TResponse>(IRequest<TResponse> request);

        Task<TResponse> SendAsync<TResponse>(IAsyncRequest<TResponse> request);

        Task<TResponse> SendAsync<TResponse>(ICancellableAsyncRequest<TResponse> request, CancellationToken cancellationToken);

        void Publish(INotification notification);

        Task PublishAsync(IAsyncNotification notification);

        Task PublishAsync(ICancellableAsyncNotification notification, CancellationToken cancellationToken);
    }
}
