using System.Threading.Tasks;

namespace Amigo.Tenant.IdentityServer.Infrastructure.ViewService
{
    /// <summary>
    /// <see cref="IViewLoader"/> decorator implementation that caches HTML templates in-memory.
    /// </summary>
    public class CachingLoader : IViewLoader
    {
        readonly ResourceCache cache;
        readonly IViewLoader inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingLoader" /> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="inner">The inner.</param>
        public CachingLoader(ResourceCache cache, IViewLoader inner)
        {
            this.cache = cache;
            this.inner = inner;
        }

        /// <summary>
        /// Loads the HTML for the named view.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<string> LoadAsync(string name)
        {
            var value = cache.Read(name);
            if (value == null)
            {
                value = await inner.LoadAsync(name);
                cache.Write(name, value);
            }
            return value;
        }
    }
}
