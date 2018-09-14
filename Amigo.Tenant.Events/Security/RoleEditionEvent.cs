using MediatR;

namespace Amigo.Tenant.Events.Security
{
    public class RoleEditionEvent : IAsyncNotification
    {
        public RoleEditionEvent()
        {
            Type = RoleEditionEventType.Register;
        }
        public RoleEditionEvent(string roleCode,RoleEditionEventType type)
        {
            RoleCode = roleCode;
            Type = type;
        }

        public string RoleCode { get; set; }
        public RoleEditionEventType Type { get; set; }
    }
}