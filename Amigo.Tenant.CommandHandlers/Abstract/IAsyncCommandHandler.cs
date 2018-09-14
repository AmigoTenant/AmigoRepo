using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.CommandHandlers.Abstract
{
    public interface IAsyncCommandHandler<in T>: IAsyncRequestHandler<T, CommandResult> where T:class, IAsyncRequest<CommandResult>
    {

    }
    public interface IAsyncCommandHandler<in T,TCommandResult> : 
        IAsyncRequestHandler<T, TCommandResult> where T : class, 
        IAsyncRequest<TCommandResult> where TCommandResult: CommandResult
    {

    }
}
