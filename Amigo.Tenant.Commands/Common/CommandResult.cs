using System.Collections.Generic;
using System.Linq;

namespace Amigo.Tenant.Commands.Common
{
    public class CommandResult
    {
        public CommandResult(IEnumerable<string> errors )
        {
            Errors = errors.ToList();            
        }
        public List<string> Errors { get; }
        public bool IsCorrect => !Errors.Any();
    }

    public class RegisteredCommandResult : CommandResult
    {
        public int Id { get; set; } 
        public RegisteredCommandResult(IEnumerable<string> errors) : base(errors)
        {
        }
    }

    public class CommandResult<T> : CommandResult
    {
        public CommandResult(IEnumerable<string> errors) : base(errors)
        {
        }

        public T Data { get; set; }
    }

    public static class CommandExtensions
    {
        public static RegisteredCommandResult WithId(this RegisteredCommandResult result, int id)
        {
            result.Id = id;
            return result;
        }   
    }
}
