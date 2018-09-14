namespace Amigo.Tenant.IdentityServer.DTOs.Responses.Common
{
    public class ApplicationMessage
    {
        public ApplicationMessage()
        {            
        }
        public ApplicationMessage(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; set; }
        public string Message { get; set; }        
    }
}