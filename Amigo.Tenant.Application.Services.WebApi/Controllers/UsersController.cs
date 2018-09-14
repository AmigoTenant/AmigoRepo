using System;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Configuration;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using Amigo.Tenant.ServiceAgent.IdentityServer;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Identity;
using System.Collections.Generic;
using System.Linq;
using Amigo.Tenant.Application.Services.WebApi.Filters;
using Amigo.Tenant.Common;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Net.Http.Headers;
using System.Net;
using Amigo.Tenant.Web.Logging;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController: ApiController
    {
        private readonly IAmigoTenantTUserApplicationService _usersApplicationService;

        public UsersController(IAmigoTenantTUserApplicationService usersApplicationService)
        {
            _usersApplicationService = usersApplicationService;
            _usersApplicationService.IdentityServerClientSettings = new ISClientSettings
            {
                SecurityAuthority = Settings.SecurityAuthority,
                ClientId = Settings.IdentityServerClientId,
                ClientSecret = Settings.IdentityServerClientSecret,
                ClientScope = Settings.IdentityServerClientScope
            };
        }


        [HttpGet, Route("search"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.UserSearch)]
        public async Task<ResponseDTO<PagedList<AmigoTenantTUserDTO>>> Search([FromUri]UserSearchRequest search)
        {
           var resp = await _usersApplicationService.SearchUsersByCriteriaAsync(search);
            var isAdmin = await _usersApplicationService.IsAdmin(User.Identity.GetUserId());
            resp.Data.Items.ToList().ForEach(p => p.IsAdminModifiedUser = isAdmin);
            return resp;
        }

        [HttpGet, Route("searchForAutocomplete")]
        public async Task<ResponseDTO<List<AmigoTenantTUserBasicDTO>>> SearchForAutocomplete([FromUri]UserSearchBasicRequest search)
        {
            var resp = await _usersApplicationService.SearchUsersByCriteriaBasicAsync(search);
            return resp;
        }

        [HttpGet, Route("searchCode")]
        public async Task<AmigoTenantTUserDTO> SearchById([FromUri]UserSearchRequest search)
        {
            var resp = await _usersApplicationService.SearchUsersByIdAsync(search);
            var isAdmin = await _usersApplicationService.IsAdmin(User.Identity.GetUserId());
            resp.IsAdminModifiedUser = isAdmin;
            return resp;
        }

        [HttpGet, Route("searchBaseInfoById")]
        public async Task<AmigoTenantTUserBasicDTO> SearchBaseInfoById(int shutttleUserId)
        {
            if(shutttleUserId<=0)
                shutttleUserId = User.Identity.GetUserId();
            var resp = await _usersApplicationService.GetBaseUserInfoById(shutttleUserId);
            return resp;
        }


        [HttpGet, Route("exists")]
        public async Task<bool> Exists([FromUri]UserSearchRequest search)
        {
            var resp = await _usersApplicationService.Exists(search);
            return resp;
        }

        [HttpGet, Route("validate")]
        public async Task<UserResponse> ValidateUserName([FromUri]UserSearchRequest search)
        {
            var resp = await _usersApplicationService.ValidateUserName(search);
            return resp;
        }

        [HttpPost, Route("register"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.UserCreate)]
        public async Task<ResponseDTO> Register(AmigoTenantTUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                dto.CreatedBy = User.Identity.GetUserId();
                dto.CreationDate = DateTime.UtcNow;
                return await _usersApplicationService.Register(dto);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("update"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.UserUpdate)]
        public async Task<ResponseDTO> Update(AmigoTenantTUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                dto.UpdatedBy = User.Identity.GetUserId();
                dto.UpdatedDate = DateTime.UtcNow;
                return await _usersApplicationService.Update(dto);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("delete"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.UserDelete)]
        public async Task<ResponseDTO> Delete(AmigoTenantTUserStatusDTO dto)
        {
            if (ModelState.IsValid)
            {
                return await _usersApplicationService.Delete(dto);
            }
            return ModelState.ToResponse();
        }


        ///VALIDATE USER
        ///VALIDATE USER
        ///VALIDATE USER

        [HttpPost, Route("validateAuthorization"), Authorize(Roles = "DRIVER"), LogRequestActionFilter]
        public async Task<ResponseDTO> ValidateAuthorization(AuthorizationRequest search)
        {
            var userId = User.Identity.GetUserId();
            search.IncludeRequestLog = Settings.RequestLogEnabled;
            var resp = await _usersApplicationService.ValidateAuthorizationAsync(search, userId);
            return resp;
        }

        [HttpGet, Route("searchByIdForAudit")]
        public async Task<AmigoTenantTUserAuditDTO> SearchByIdForAudit(int? createdBy, int? updatedBy )
        {
            var resp = await _usersApplicationService.SearchByIdForAudit(createdBy, updatedBy);
            return resp;
        }
    }
 
}