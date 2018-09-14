using System.Collections.Generic;
using System.Linq;

namespace Amigo.Tenant.Application.DTOs.Responses.Common
{
    public class ResponseDTO
    {
        public ResponseDTO(bool isValid)
        {
            IsValid = isValid;
        }
        public ResponseDTO(bool isValid, int? pk, string code)
        {
            IsValid = isValid;

        }
        public ResponseDTO()
        {
            this.Messages = new List<ApplicationMessage>();
        }
        public virtual bool IsValid { get; set; }
        public virtual int? Pk { get; set; }
        public virtual string Code { get; set; }

        public IList<ApplicationMessage> Messages { get; set; }

        public virtual ResponseDTO WithMessage(string message, string key = null)
        {
            this.Messages.Add(new ApplicationMessage(key, message));
            return this;
        }

        public virtual ResponseDTO WithMessages(params ApplicationMessage[] messages)
        {
            this.Messages = messages.ToList();
            return this;
        }
    }

    public class ResponseDTO<T> : ResponseDTO
    {
        public ResponseDTO()
        {            
        }

        public ResponseDTO(bool isValid):base(isValid)
        {            
        }
        public virtual T Data { get; set; }

        public new ResponseDTO<T> WithMessage(string message,string key=null)
        {
            this.Messages.Add(new ApplicationMessage(key,message));
            return this;
        }

        public new ResponseDTO<T> WithMessages(params ApplicationMessage[] messages)
        {
            this.Messages = messages.ToList();
            return this;
        }
    }
}
