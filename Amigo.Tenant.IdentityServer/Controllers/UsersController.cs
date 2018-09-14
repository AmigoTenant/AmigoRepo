using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.IdentityServer.ApplicationServices.Services;
using Amigo.Tenant.IdentityServer.DTOs.Requests.Users;
using Amigo.Tenant.IdentityServer.DTOs.Responses.Common;
using Amigo.Tenant.IdentityServer.DTOs.Responses.Users;

namespace Amigo.Tenant.IdentityServer.Controllers
{    
     [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private readonly IUserAdministrationService _administrationService;

        public UsersController(IUserAdministrationService administrationService)
        {
            _administrationService = administrationService;            
        }

        [Route("GetUser")]
        public async Task<ResponseDTO<UserResponse>> GetUser([FromUri]string username)
        {
            return await _administrationService.GetDetails(username);
        }


        /// <summary>
        ///     
        /// </summary>
        /// <param name="usernames">array of usernames</param>
        /// <returns>Array of users'details</returns>
        /// <example>
        ///     Call this method from a browser as follows
        ///     http://localhost:7071/api/Users/GetUsersDetails?usernames=johnconnor&usernames=juniohamano
        /// </example>  
        /// 
        [Route("GetUsersDetails")]
        public async Task<ResponseDTO<List<UserResponse>>> GetUsersDetails([FromUri]List<string> usernames)
        {
            return await _administrationService.GetUsersDetails(usernames);
        }

        [HttpPost]
        [Route("Search")]
        public async Task<ResponseDTO<PagedList<UserResponse>>> Search(SearchUserRequest search)
        {
            return await _administrationService.Search(search);
        }

        [HttpPost]
        [Route()]
        public async Task<ResponseDTO> Post(RegisterUserRequest user)
        {
            return await _administrationService.Register(user);
        }

        [HttpPost]
        [Route("registerExternal")]
        public async Task<ResponseDTO> RegisterInternal(RegisterExternalUserRequest user)
        {
            return await _administrationService.RegisterExternalUser(user);
        }

        [HttpPut]
        [Route()]
        public async Task<ResponseDTO> Put(UpdateUserRequest user)
        {
            return await _administrationService.Update(user);
        }

        [HttpPost]
        [Route("addClaim")]
        public async Task<ResponseDTO> AddClaim(AddClaimRequest claim)
        {
            return await _administrationService.AddClaim(claim);
        }

        [HttpPost]
        [Route("removeClaim")]
        public Task<ResponseDTO> RemoveClaim(RemoveClaimRequest claim)
        {
            return _administrationService.RemoveClaim(claim);
        }

        [HttpPost]
        [Route("changePassword")]
        public Task<ResponseDTO> ChangePassword(ChangeUserPasswordRequest request)
        {
            return _administrationService.ChangePassword(request);
        }

        [HttpPost]
        [Route("resetPassword")]
        public Task<ResponseDTO> ResetPassword(ResetUserPasswordRequest request)
        {
            var res = _administrationService.ResetPassword(request);
            return res;
        }

        [HttpPost]
        [Route("changeStatus")]
        public Task<ResponseDTO> ChangeStatus(UpdateStatusUserRequest request)
        {
            var res = _administrationService.ChangeStatus(request);
            return res;
        }
    }
}
