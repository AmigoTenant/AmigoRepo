namespace Amigo.Tenant.Application.DTOs.Requests.Common
{
 
    public enum ObjectStatus:byte
    {
        Unchanged = 0,
        Added = 1,
        Modified = 2,
        Deleted = 3
    }

    public class BaseStatusRequest
    {

        public BaseStatusRequest()
        {
            TableStatus = ObjectStatus.Unchanged;
        }

        public ObjectStatus TableStatus { get; set; }

    }
}
