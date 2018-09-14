using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Newtonsoft.Json;

namespace Amigo.Tenant.Application.Services.WebApi.Helpers.Identity
{
    public static class IdentityExtensions
    {
        public static int GetUserId(this IIdentity identity)
        {
            var ide = identity as ClaimsIdentity;
            if(ide == null)throw new NullReferenceException(nameof(identity));

            if (ide.Claims == null || !ide.Claims.Any())
            {
                Trace.WriteLine("User details:" + JsonConvert.SerializeObject(ide));
                return default(int);
            }

            if(ide.Claims.Any(x => x.Type == ClaimTypes.NameIdentifier))
            {
                var idvalue = ide.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                return Int32.Parse(idvalue);
            }
            Trace.WriteLine("User details:" + JsonConvert.SerializeObject(ide));            
            return default(int);
        }

        public static string GetUsername(this IIdentity identity)
        {
            var ide = identity as ClaimsIdentity;
            if (ide == null) throw new NullReferenceException(nameof(identity));

            if (ide.Claims == null || !ide.Claims.Any())
            {
                Trace.WriteLine("User details:" + JsonConvert.SerializeObject(ide));
                return default(string);
            }

            var username = ide.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value;
            return username;
        }
    }
}