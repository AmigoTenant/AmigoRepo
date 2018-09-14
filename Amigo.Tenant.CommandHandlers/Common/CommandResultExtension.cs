using Amigo.Tenant.CommandModel.Abstract;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;
using System.Linq;

namespace Amigo.Tenant.CommandHandlers.Common
{
    public static class CommandResultExtension
    {
        public static CommandResult ToResult(this IValidatable entity)
        {
            return new CommandResult(entity.Errors);
        }
        public static RegisteredCommandResult ToRegisterdResult(this IValidatable entity)
        {
            return new RegisteredCommandResult(entity.Errors);
        }
        public static CommandResult ToResult(this List<IValidatable> entities)
        {
            var summarizedErrors = new List<string>();
            entities.ForEach(p => summarizedErrors.AddRange(p.Errors));
            return new CommandResult(summarizedErrors);
        }
    }
}