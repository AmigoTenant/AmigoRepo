using System.Collections.Generic;
using ExpressMapper;

namespace Amigo.Tenant.Infrastructure.Mapping.Abstract
{
    public static class MapperConfig
    {
        private static readonly List<Profile> Profiles = new List<Profile>();

        public static void Register<T>() where T : Profile, new()
        {
            Profiles.Add(new T());
        }

        public static void Init()
        {            
            foreach (var profile in Profiles)
                profile.Register();            

            Mapper.Compile();
        }
    }
}
