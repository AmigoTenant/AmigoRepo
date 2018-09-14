using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.ServiceAgent.IdentityServer
{
    public class IdentitySeverAgent : IIdentitySeverAgent
    {
        public ISClientSettings IdentityServerClientSettings
        {
            get; set;
        }
        public async Task<HttpResponseMessage>  Save_AmigoTenantTUserDTO_IdentityServer(AmigoTenantTUserDTO dto )
        {
            
            using (var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings))
            {
                var response = await httpClient.PostAsJsonAsync("api/Users", dto);
                return response;

            }
        }

        public async Task<HttpResponseMessage> Save_ClaimRequest_IdentityServer(AddClaimRequest claimRequest)
        {
            using (var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings))
            {
                var response = await httpClient.PostAsJsonAsync("api/Users/addClaim", claimRequest);
                return response;

            }
        }

        public async Task<HttpResponseMessage>Remove_ClaimRequest_IdentityServer(RemoveClaimRequest claimRequest)
        {
            using (var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings))
            {
                var response = await httpClient.PostAsJsonAsync("api/Users/removeClaim", claimRequest);
                return response;

            }
        }        

        public async Task<HttpResponseMessage> Put_AmigoTenantTUserDTO_IdentityServer(IdentityUserDTO dto)
        {
            using (var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings))
            {
                var response = await httpClient.PutAsJsonAsync("api/Users", dto);
                return response;

            }
        }

        public async Task<HttpResponseMessage> Reset_AmigoTenantTUser_Password_IdentityServer(ResetUserPasswordRequest resetUserPasswordRequest)
        {
            using (var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings))
            {

                var response = await httpClient.PostAsJsonAsync( "api/Users/resetPassword", resetUserPasswordRequest);
                return response;

            }
        }

        public async Task<HttpResponseMessage> AmigoTenantTUser_Details_IdentityServer(List<string> users)
        {
            using (var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings))
            {

                var response = await httpClient.GetAsync("api/Users/GetUsersDetails" + GetUrlParameterValues(users));
                return response;
                //HttpContext.Current.Server.UrlEncode()
            }
        }
        public async Task<HttpResponseMessage> ChangeStatus_AmigoTenantTUserDTO_IdentityServer(IdentityUserDTO dto)
        {

            using (var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings))
            {
                var response = await httpClient.PostAsJsonAsync("api/Users/changeStatus", dto);
                return response;

            }
        }

        private string GetUrlParameterValues(List<string> userNames)
        {
            if (!userNames.Any())
                return "";

            var builder = new StringBuilder("?");
            var separator = string.Empty;
            foreach (var userName in userNames.Where(kvp => kvp != null))
            {
                builder.AppendFormat("{0}{1}={2}", separator, HttpUtility.UrlEncode("Usernames"), HttpUtility.UrlEncode(userName.ToString()));
                separator = "&";
            }
            return builder.ToString();
        }
        public abstract class ClaimRequestBase
        {
            public int UserId
            {
                get; set;
            }
            public string ClaimType
            {
                get; set;
            }
            public string ClaimValue
            {
                get; set;
            }
        }

        public class AddClaimRequest : ClaimRequestBase
        {
        }

        public class RemoveClaimRequest : ClaimRequestBase
        {
        }

        public class ResetUserPasswordRequest
        {
            public int Id
            {
                get; set;
            }
            public string Password
            {
                get; set;
            }
        }

    }
}
