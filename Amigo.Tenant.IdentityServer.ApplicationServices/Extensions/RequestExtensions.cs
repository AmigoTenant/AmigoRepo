using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.IdentityServer.ApplicationServices.Extensions
{
    public static class RequestExtensions
    {
        public static bool IsEmpty(this int value)
        {
            return value == default(int);
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsEmpty<T>(this T value) where T:class
        {
            return value == default(T);
        }

        public static bool IsNotEmpty(this int value)
        {
            return !value.IsEmpty();
        }

        public static bool IsNotEmpty(this string value)
        {
            return !value.IsEmpty();
        }

        public static bool IsNotEmpty<T>(this T value) where T : class
        {
            return !value.IsEmpty();
        }        

        public static bool IsNotEmpty<T>(this T[] value)
        {
            return value != null && value.Length > 0;
        }
    }
}
