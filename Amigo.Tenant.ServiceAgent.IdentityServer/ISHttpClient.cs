using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.ServiceAgent.IdentityServer
{
    /// <summary>
    /// Identity Server Http Client
    /// </summary>
    public static class ISHttpClient
    {
        

        public static HttpClient GetClient(ISClientSettings clientSettings)
        {

            HttpClient client = new HttpClient();
            client.SetBearerToken(GetAccessToken(clientSettings));
            client.BaseAddress = new Uri(clientSettings.SecurityAuthority);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private static string GetAccessToken(ISClientSettings clientSettings)
        {
            var client = new TokenClient(
                                           clientSettings.SecurityAuthority + "/connect/token",
                                           clientSettings.ClientId,
                                           clientSettings.ClientSecret);
            var resp = client.RequestClientCredentialsAsync(clientSettings.ClientScope).Result;
            return resp.AccessToken;
        }
    }
}
