using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.ServiceAgent.IdentityServer
{
    public interface IIdentitySeverAgent
    {

        Task<HttpResponseMessage> Save_AmigoTenantTUserDTO_IdentityServer(AmigoTenantTUserDTO dto);
        Task<HttpResponseMessage> Save_ClaimRequest_IdentityServer(IdentitySeverAgent.AddClaimRequest claimRequest);
        Task<HttpResponseMessage> Remove_ClaimRequest_IdentityServer(IdentitySeverAgent.RemoveClaimRequest claimRequest);
        Task<HttpResponseMessage> Put_AmigoTenantTUserDTO_IdentityServer(IdentityUserDTO dto);
        Task<HttpResponseMessage> Reset_AmigoTenantTUser_Password_IdentityServer(IdentitySeverAgent.ResetUserPasswordRequest resetUserPasswordRequest);
        Task<HttpResponseMessage> AmigoTenantTUser_Details_IdentityServer(List<string> users);

        ISClientSettings IdentityServerClientSettings{get; set;}
        Task<HttpResponseMessage> ChangeStatus_AmigoTenantTUserDTO_IdentityServer(IdentityUserDTO dto);
    }
}
