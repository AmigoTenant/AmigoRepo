using System;
using XPO.ShuttleTracking.Application.DTOs.Responses.Move;

namespace XPO.ShuttleTracking.Mobile.Infrastructure.State
{
    public class State
    {
        public State()
        {
        }

        public State(Guid id, ServiceTypeEnum type, ShuttletServiceDTO request)
        {
            Id = id;
            Type = type;
            Request = request;
        }
        public Guid Id { get; set; }
        public ServiceTypeEnum Type { get; set; }
        public ShuttletServiceDTO Request { get; set; }

        public static State Move(Guid id, ShuttletServiceDTO request)
        {
            return new State(id,ServiceTypeEnum.Move,request);
        }
        public static State Detention(Guid id, ShuttletServiceDTO request)
        {
            return new State(id, ServiceTypeEnum.Detention,request);
        }
        public static State Taylor(Guid id, ShuttletServiceDTO request)
        {
            return new State(id, ServiceTypeEnum.Taylor, request);
        }
        public static State Service(Guid id, ShuttletServiceDTO request)
        {
            return new State(id, ServiceTypeEnum.Service, request);
        }
    }

    public enum ServiceTypeEnum:byte
    {
        Move=0,
        Taylor=1,
        Detention = 2,
        Service = 3
    }
}
