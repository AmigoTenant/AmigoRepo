using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.Authorization;
using Amigo.Tenant.Commands.Security.AmigoTenantTUsers;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using Amigo.Tenant.ServiceAgent.IdentityServer;
//using static Amigo.Tenant.ServiceAgent.IdentityServer.ISHttpClientAgent;

namespace Amigo.Tenant.Application.Services.Security
{
    public class AmigoTenantTUserApplicationService : IAmigoTenantTUserApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<AmigoTenantTUserDTO> _userDataAccess;
        private readonly IQueryDataAccess<AmigoTenantTUserBasicDTO> _userBasicDataAccess;
        private readonly IAmigoTenantTRoleApplicationService _AmigoTenantTRoleApplicationService;
        private readonly IActivityTypeApplicationService _activityTypeService;
        private readonly IIdentitySeverAgent _identityAgent;

        public ISClientSettings IdentityServerClientSettings
        {
            get; set;
        }

        public AmigoTenantTUserApplicationService(IBus bus,
            IQueryDataAccess<AmigoTenantTUserDTO> userDataAccess,
            IQueryDataAccess<AmigoTenantTUserBasicDTO> userBasicDataAccess,
            IMapper mapper,
            IAmigoTenantTRoleApplicationService amigoTenantTRoleApplicationService,
            IActivityTypeApplicationService activityTypeService,
            IIdentitySeverAgent identityAgent
            )
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _userDataAccess = userDataAccess;
            _userBasicDataAccess = userBasicDataAccess;
            _mapper = mapper;
            _AmigoTenantTRoleApplicationService = amigoTenantTRoleApplicationService;
            _activityTypeService = activityTypeService;
            _identityAgent = identityAgent;
        }




        private async Task<bool> SetUsersAdditionalInformation(List<AmigoTenantTUserDTO> userListDTO)
        {
            if (userListDTO.Any())
            {
                var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings);
                var rspUsersDetails = await httpClient.GetAsync("api/Users/GetUsersDetails" + GetUrlParameterValues(userListDTO.Select(q => q.Username).ToList()));
                if (rspUsersDetails.IsSuccessStatusCode)
                {
                    var userDetailsJson = await rspUsersDetails.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var users = JsonConvert.DeserializeObject<ResponseDTO<List<UserResponse>>>(userDetailsJson);

                    foreach (var item in userListDTO)
                    {
                        var user = users.Data.Where(q => q.UserName == item.Username).FirstOrDefault();
                        if (user != null)
                        {
                            item.FirstName = user.FirstName;
                            item.LastName = user.LastName;
                            item.Email = user.Email;
                            item.CustomUsername = user.UserName + " - " + (user.FirstName == null ? string.Empty : user.FirstName) + " " + (user.LastName == null ? string.Empty : user.LastName);
                        }
                        else
                        {
                            item.CustomUsername = item.Username + " - " + (item.FirstName == null ? string.Empty : item.FirstName) + " " + (item.LastName == null ? string.Empty : item.LastName);
                        }
                    }
                }
                else
                {
                    throw new Exception("Amigo.Tenant.Application.Services.Security - AmigoTenantTUserApplicationService - SearchUsersByCriteriaAsync - call to IdentityServerHttpClient api/Users/GetUsersDetails was not successful");
                }
            }
            return true;
        }


        //to Do
        private async Task SetUserAdditionalInformation(AmigoTenantTUserDTO userDTO)
        {
            if (!string.IsNullOrEmpty(userDTO.Username))
            {
                var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings);
                var rspUsersDetails = await httpClient.GetAsync("api/Users/GetUsersDetails?Usernames=" + HttpUtility.UrlEncode(userDTO.Username));
                if (rspUsersDetails.IsSuccessStatusCode)
                {
                    var userDetailsJson = await rspUsersDetails.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var users = JsonConvert.DeserializeObject<ResponseDTO<List<UserResponse>>>(userDetailsJson);
                    var user = users.Data.Where(q => q.UserName == userDTO.Username).FirstOrDefault();
                    if (user != null)
                    {
                        userDTO.FirstName = user.FirstName;
                        userDTO.LastName = user.LastName;
                        userDTO.Email = user.Email;
                    }
                }
                else
                {
                    throw new Exception("Amigo.Tenant.Application.Services.Security - AmigoTenantTUserApplicationService - SearchUsersByCriteriaAsync - call to IdentityServerHttpClient api/Users/GetUsersDetails was not successful");
                }
            }
        }

        public async Task<ResponseDTO<PagedList<AmigoTenantTUserDTO>>> SearchUsersByCriteriaAsync(UserSearchRequest search)
        {

            var queryFilter = GetQueryFilter(search);
            var result = await _userDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);
            await SetUsersAdditionalInformation(result.Items.ToList());
            var pagedResult = new PagedList<AmigoTenantTUserDTO>()
            {
                Items = result.Items,
                PageSize = result.PageSize,
                Page = result.Page,
                Total = result.Total
            };
            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<List<AmigoTenantTUserBasicDTO>>> SearchUsersByCriteriaBasicAsync(UserSearchBasicRequest search)
        {

            Expression<Func<AmigoTenantTUserBasicDTO, bool>> queryFilter = p => true;

            queryFilter = queryFilter.And(p => p.RowStatus == true);

            //if (search!= null && !string.IsNullOrEmpty(search.CustomUsername))
            //    queryFilter = queryFilter.And(p => p.CustomUsername.Contains(search.CustomUsername));

            var result = (await _userBasicDataAccess.ListAsync(queryFilter)).ToList();

            await SetUsersAdditionalInformationBasic(result);

            Expression<Func<AmigoTenantTUserBasicDTO, bool>> queryOtherFilter = p => true;

            if (search != null && !string.IsNullOrEmpty(search.CustomUsername))
                queryOtherFilter = queryOtherFilter.And(p => p.CustomUsername.ToLower().Contains(search.CustomUsername.ToLower()));

            var applyOtherFilters = result.AsQueryable().Where(queryOtherFilter).ToList();

            return ResponseBuilder.Correct(applyOtherFilters);
        }

        public async Task<AmigoTenantTUserDTO> SearchUsersByIdAsync(UserSearchRequest search)
        {
            var queryFilter = GetQueryFilter(search);
            var result = await _userDataAccess.FirstOrDefaultAsync(queryFilter);
            if (result != null)
                await SetUserAdditionalInformation(result);

            return result;
        }

        public async Task<AmigoTenantTUserDTO> GetUsersByIdAsync(int amigoTenantTUserId)
        {
            Expression<Func<AmigoTenantTUserDTO, bool>> queryFilter = p => p.RowStatus.Value == true;

            if (amigoTenantTUserId > 0)
                queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == amigoTenantTUserId);

            var result = await _userDataAccess.FirstOrDefaultAsync(queryFilter);

            if (result != null)
                await SetUserAdditionalInformation(result);

            return result;
        }

        public async Task<AmigoTenantTUserDTO> GetUsersByUserNameAsync(string userName)
        {
            Expression<Func<AmigoTenantTUserDTO, bool>> queryFilter = p => true;

            queryFilter = queryFilter.And(p => p.Username.Contains(userName));

            var result = await _userDataAccess.FirstOrDefaultAsync(queryFilter);

            if (result != null)
                await SetUserAdditionalInformation(result);

            return result;
        }

        public async Task<bool> IsAdmin(int amigoTenantTUserId)
        {
            var isAdmin = false;
            Expression<Func<AmigoTenantTUserDTO, bool>> queryFilter = p => p.RowStatus.Value == true;

            if (amigoTenantTUserId > 0)
                queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == amigoTenantTUserId);

            var result = await _userDataAccess.FirstOrDefaultAsync(queryFilter);

            if (result != null)
                isAdmin = result.IsAdmin;

            return isAdmin;
        }

        public async Task<bool> Exists(UserSearchRequest search)
        {
            var queryFilter = GetQueryFilter(search);
            var result = await _userDataAccess.AnyAsync(queryFilter);
            return result;
        }

        public async Task<UserResponse> ValidateUserName(UserSearchRequest search)
        {
            var queryFilter = GetQueryFilter(search);
            var exist = await _userDataAccess.AnyAsync(queryFilter);
            UserResponse userAD = new UserResponse();

            using (var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings))
            {
                var resetUserPasswordRequest = new AmigoTenantTUserDTO() { Username = search.UserName };
                var resetUserPasswordResponse = await httpClient.PostAsJsonAsync("api/Users/search", resetUserPasswordRequest);

                if (resetUserPasswordResponse.IsSuccessStatusCode)
                {
                    var userDetailsJson = await resetUserPasswordResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var users = JsonConvert.DeserializeObject<ResponseDTO<PagedList<UserResponse>>>(userDetailsJson);

                    userAD = users.Data.Items.FirstOrDefault();

                    var content = await resetUserPasswordResponse.Content.ReadAsAsync<ResponseDTO>();
                    if (!content.IsValid)
                    {
                        //return ResponseBuilder.InCorrect().WithMessages(content.Messages.ToArray());
                    }
                }
                else
                {
                    //return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage() { Message = Constants.Entity.AmigoTenantTUser.ErrorMessage.PasswordNotUpdated });
                }

            }
            return userAD;
        }

        private async Task<bool> ExistsActiveToSave(UserSearchRequest search)
        {
            var queryFilter = GetQueryFilterForUserName(search);
            var result = false;
            var user = await _userDataAccess.FirstOrDefaultAsync(queryFilter);
            if (user != null)
            {
                result = user.RowStatus.HasValue && user.RowStatus.Value;
            }

            return result;
        }

        private async Task<bool> ExistsToUpdate(UserSearchRequest search)
        {
            var queryFilter = GetQueryFilterForId(search);
            var result = await _userDataAccess.AnyAsync(queryFilter);
            return result;
        }

        private async Task<bool> ExistsInactiveUser(UserSearchRequest search)
        {
            Expression<Func<AmigoTenantTUserDTO, bool>> queryFilter = p => true;

            if (!string.IsNullOrEmpty(search.UserName))
                queryFilter = queryFilter.And(p => p.Username.Contains(search.UserName));

            var result = false;
            var user = await _userDataAccess.FirstOrDefaultAsync(queryFilter);
            if (user != null)
                result = user.RowStatus.HasValue && !user.RowStatus.Value;
            return result;
        }


        public async Task<ResponseDTO> Register(AmigoTenantTUserDTO dto)
        {
            if (dto == null)
                return ResponseBuilder.InCorrect();

            var msg = new StringBuilder();
            var roleRequest = new AmigoTenantTRoleSearchRequest() { AmigoTenantTRoleId = dto.AmigoTenantTRoleId ?? 0 };
            var roleDto = await _AmigoTenantTRoleApplicationService.GetAmigoTenantTRoleBasicByIdAsync(roleRequest);

            var isUserModifiedAdmin = await IsAdmin(dto.CreatedBy.Value);

            if (roleDto.Data.IsAdmin && !isUserModifiedAdmin)
            {
                msg.AppendLine("User does not have permission to create root users");

                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage() { Message = msg.ToString() });
            }

            var userSearchRequest = new UserSearchRequest() { AmigoTenantTUserId = dto.AmigoTenantTUserId, UserName = dto.Username };
            var existsInactive = await ExistsInactiveUser(userSearchRequest);

            //  var agent = new ISHttpClientAgent();
            _identityAgent.IdentityServerClientSettings = this.IdentityServerClientSettings;
            if (!existsInactive)
            {
                /*******************************************
              1. INSERTING IN IDENTITY SERVER
                {
                      "AmigoTenantTUserId": 0,
                      "Username": "qqstring",
                      "PayBy": "1",
                      "UserType": "1",
                      "DedicatedLocationId": 810,
                      "BypassDeviceValidation": "1",
                      "UnitNumber": "1",
                      "TractorNumber": "1",
                      "FirstName": "12",
                      "LastName": "132",
                      "Email": "string@com.pe",
                      "Password": "123string",
                      "Id": 0,
                      "PhoneNumber": "122131",
                      "AmigoTenantTRoleId": 1,
                      "RowStatus": true,
                      "EntityStatus": 0
                }AnyAsync
            *******************************************/
                /*
             using (var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings))
             {
                 var response = await httpClient.PostAsJsonAsync("api/Users", dto);
                 await ProcessResponse(response, httpClient, dto, msg);

                 var claimRequest = new AddClaimRequest() { UserId = dto.AmigoTenantTUserId, ClaimType = "role", ClaimValue = roleDto.Data.Code };
                 var claimResponseAdd = await httpClient.PostAsJsonAsync("api/Users/addClaim", claimRequest);
                 if (!claimResponseAdd.IsSuccessStatusCode)
                     msg.AppendLine(Constants.Entity.AmigoTenantTUser.ErrorMessage.ClaimNoCreated);

             }
             */
                var responseAgent = await _identityAgent.Save_AmigoTenantTUserDTO_IdentityServer(dto);
                await ProcessResponseAgent(responseAgent, dto, msg);

                if (responseAgent.IsSuccessStatusCode)
                {
                    var claimRequest = new IdentitySeverAgent.AddClaimRequest()
                    {
                        UserId = dto.AmigoTenantTUserId,
                        ClaimType = "role",
                        ClaimValue = roleDto.Data.Code
                    };
                    var claimAgent = await _identityAgent.Save_ClaimRequest_IdentityServer(claimRequest);
                    if (!claimAgent.IsSuccessStatusCode)
                        msg.AppendLine(Constants.Entity.AmigoTenantTUser.ErrorMessage.ClaimNoCreated);
                }
            }
            else
            {
                /*******************************************
              1. UPDATING IN IDENTITY SERVER
            *******************************************/

                var amigoTenantTUser = await GetUsersByUserNameAsync(dto.Username);
                if (amigoTenantTUser != null)
                {
                    var roleRequestToAdd = new AmigoTenantTRoleSearchRequest() { AmigoTenantTRoleId = dto.AmigoTenantTRoleId.HasValue ? dto.AmigoTenantTRoleId.Value : 0 };
                    var roleToAdd = await _AmigoTenantTRoleApplicationService.GetAmigoTenantTRoleBasicByIdAsync(roleRequestToAdd);
                    
                    roleRequest = new AmigoTenantTRoleSearchRequest() { AmigoTenantTRoleId = amigoTenantTUser.AmigoTenantTRoleId.HasValue ? amigoTenantTUser.AmigoTenantTRoleId.Value : 0 };
                    var roleToRemove = await _AmigoTenantTRoleApplicationService.GetAmigoTenantTRoleBasicByIdAsync(roleRequest);

                    var identityUserDto = new IdentityUserDTO()
                    {
                        Id = amigoTenantTUser.AmigoTenantTUserId,
                        UserName = dto.Username,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Email = dto.Email,
                        PhoneNumber = dto.PhoneNumber,
                        RowStatus = dto.RowStatus
                    };
                    _identityAgent.IdentityServerClientSettings = this.IdentityServerClientSettings;
                    var responseAgent = await _identityAgent.Put_AmigoTenantTUserDTO_IdentityServer(identityUserDto);
                    await ProcessResponseAgent(responseAgent, dto, msg);

                    if (responseAgent.IsSuccessStatusCode)
                    {
                        //--- AddClaim
                        var claimRequest = new IdentitySeverAgent.RemoveClaimRequest() { UserId = amigoTenantTUser.AmigoTenantTUserId, ClaimType = "role", ClaimValue = roleToRemove.Data.Name };
                        var claimAgent = await _identityAgent.Remove_ClaimRequest_IdentityServer(claimRequest);
                        if (!claimAgent.IsSuccessStatusCode)
                            msg.AppendLine(Constants.Entity.AmigoTenantTUser.ErrorMessage.ClaimNoCreated);

                        //--- RemoveClaim
                        var claimAddRequest = new IdentitySeverAgent.AddClaimRequest() { UserId = amigoTenantTUser.AmigoTenantTUserId, ClaimType = "role", ClaimValue = roleToAdd.Data.Code };
                        var claimAddAgent = await _identityAgent.Save_ClaimRequest_IdentityServer(claimAddRequest);
                        if (!claimAddAgent.IsSuccessStatusCode)
                            msg.AppendLine(Constants.Entity.AmigoTenantTUser.ErrorMessage.ClaimNoCreated);

                    }


                    //-------------------------------------
                    //      Change Password ( "reset")
                    //-------------------------------------
                    if (!string.IsNullOrEmpty(dto.Password))
                    {
                        var resetUserPasswordRequest = new IdentitySeverAgent.ResetUserPasswordRequest() { Id = amigoTenantTUser.AmigoTenantTUserId, Password = dto.Password };
                        //var resetUserPasswordResponse = await httpClient.PostAsJsonAsync("api/Users/resetPassword", resetUserPasswordRequest);
                        var resetUserPasswordResponse = await _identityAgent.Reset_AmigoTenantTUser_Password_IdentityServer(resetUserPasswordRequest);

                        if (resetUserPasswordResponse.IsSuccessStatusCode)
                        {
                            var content = await resetUserPasswordResponse.Content.ReadAsAsync<ResponseDTO>();
                            if (!content.IsValid)
                            {
                                return ResponseBuilder.InCorrect().WithMessages(content.Messages.ToArray());
                            }
                        }
                        else
                        {
                            return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage() { Message = Constants.Entity.AmigoTenantTUser.ErrorMessage.PasswordNotUpdated });
                        }
                    }

                    // } 
                }

            }


            /*****************************************
              2. INSERTING IN SERVICE
            ******************************************/

            var existsToSave = await ExistsActiveToSave(userSearchRequest);

            if (existsToSave)
                msg.AppendLine(string.Format("{0} on {1}", Constants.Entity.AmigoTenantTUser.ErrorMessage.UserExist, Constants.Services.AmigoTenantService));
            else
            {
                if (!existsInactive)
                {
                    if (dto.AmigoTenantTUserId > 0)
                    {
                        var command = _mapper.Map<AmigoTenantTUserDTO, RegisterAmigoTenantTUserCommand>(dto);
                        var resp = await _bus.SendAsync(command);
                        return ResponseBuilder.Correct(resp).WithMessages(new ApplicationMessage() { Message = msg.ToString() });
                    }
                    else
                    {
                        msg.AppendLine(Constants.Entity.AmigoTenantTUser.ErrorMessage.UserNoCreatedInIdentity);
                    }
                }
                else
                {
                    var amigoTenantTUser = await GetUsersByUserNameAsync(dto.Username);
                    if (amigoTenantTUser != null)
                    {
                        dto.AmigoTenantTUserId = amigoTenantTUser.AmigoTenantTUserId;
                        var command = _mapper.Map<AmigoTenantTUserDTO, UpdateAmigoTenantTUserCommand>(dto);
                        var userSearchRequestToUpdate = new UserSearchRequest() { AmigoTenantTUserId = dto.AmigoTenantTUserId, UserName = dto.Username };
                        var existsToUpdate = await ExistsToUpdate(userSearchRequestToUpdate);

                        if (!existsToUpdate)
                            msg.AppendLine(string.Format("{0} on {1}", Constants.Entity.AmigoTenantTUser.ErrorMessage.UserNoExist, Constants.Services.AmigoTenantService));
                        else
                        {
                            var resp = await _bus.SendAsync(command);
                            return ResponseBuilder.Correct(resp);
                        }
                    }
                    
                }
                
            }

            return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage() { Message = msg.ToString() });

        }

        private async Task ProcessResponseAgent(HttpResponseMessage  response, AmigoTenantTUserDTO dto, StringBuilder msg)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsAsync<ResponseDTO<UserResponse>>();
                if (content != null && content.Data != null && content.Data.Id != 0 && content.IsValid)
                {
                    dto.AmigoTenantTUserId = content.Data.Id;

                }
                else if (content != null && content.Data != null && content.Data.Id != 0 && !content.IsValid)
                {
                    dto.AmigoTenantTUserId = content.Data.Id;
                    msg.AppendLine(content.Messages.Any() ? content.Messages.First().Message : string.Empty);
                }
                else if (content != null)
                    msg.AppendLine(content.Messages.Any() ? content.Messages.First().Message : string.Empty);
            }
            else
            {
                msg.AppendLine("Amigo.Tenant.Application.Services.Security - AmigoTenantTUserApplicationService - SearchUsersByCriteriaAsync - call to IdentityServerHttpClient api/Users/GetUsersDetails was not successful");
            }
        }
        private async Task ProcessResponseAgent(HttpResponseMessage response, IdentityUserDTO dto, StringBuilder msg)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsAsync<ResponseDTO<UserResponse>>();
                if (content != null && content.Data != null && content.Data.Id != 0 && content.IsValid)
                {
                    dto.Id = content.Data.Id;

                }
                else if (content != null && content.Data != null && content.Data.Id != 0 && !content.IsValid)
                {
                    dto.Id = content.Data.Id;
                    msg.AppendLine(content.Messages.Any() ? content.Messages.First().Message : string.Empty);
                }
                else if (content != null)
                    msg.AppendLine(content.Messages.Any() ? content.Messages.First().Message : string.Empty);
            }
            else
            {
                msg.AppendLine("Amigo.Tenant.Application.Services.Security - AmigoTenantTUserApplicationService - SearchUsersByCriteriaAsync - call to IdentityServerHttpClient api/Users/GetUsersDetails was not successful");
            }
        }        

        public async Task<ResponseDTO> Update(AmigoTenantTUserDTO dto)
        {
            StringBuilder msg = new StringBuilder();
            var roleRequest = new AmigoTenantTRoleSearchRequest() { AmigoTenantTRoleId = dto.AmigoTenantTRoleId.HasValue ? dto.AmigoTenantTRoleId.Value : 0 };
            var roleToAdd = await _AmigoTenantTRoleApplicationService.GetAmigoTenantTRoleBasicByIdAsync(roleRequest);

            var amigoTenantTUser = await GetUsersByIdAsync(dto.AmigoTenantTUserId);
            roleRequest = new AmigoTenantTRoleSearchRequest() { AmigoTenantTRoleId = amigoTenantTUser.AmigoTenantTRoleId.HasValue ? amigoTenantTUser.AmigoTenantTRoleId.Value : 0 };
            var roleToRemove = await _AmigoTenantTRoleApplicationService.GetAmigoTenantTRoleBasicByIdAsync(roleRequest);
            
            var isUserModifiedAdmin = await IsAdmin(dto.UpdatedBy.Value);

            if (roleToAdd.Data.IsAdmin && !isUserModifiedAdmin)
            {
                msg.AppendLine("User does not have permission to update root users");

                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage() { Message = msg.ToString() });
            }

            /*******************************************
              1. UPDATING IN IDENTITY SERVER
            *******************************************/

            // var agent = new ISHttpClientAgent();
            var identityUserDto = new IdentityUserDTO()
            {
                Id = dto.AmigoTenantTUserId,
                UserName = dto.Username,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                RowStatus = dto.RowStatus
            };
            _identityAgent.IdentityServerClientSettings = this.IdentityServerClientSettings;
            var responseAgent = await _identityAgent.Put_AmigoTenantTUserDTO_IdentityServer(identityUserDto);
            await ProcessResponseAgent(responseAgent, dto, msg);

            if (responseAgent.IsSuccessStatusCode)
            {
                //--- AddClaim
                var claimRequest = new IdentitySeverAgent.RemoveClaimRequest() { UserId = amigoTenantTUser.AmigoTenantTUserId, ClaimType = "role", ClaimValue = roleToRemove.Data.Name };
                var claimAgent = await _identityAgent.Remove_ClaimRequest_IdentityServer(claimRequest);
                if (!claimAgent.IsSuccessStatusCode)
                    msg.AppendLine(Constants.Entity.AmigoTenantTUser.ErrorMessage.ClaimNoCreated);

                //--- RemoveClaim
                var claimAddRequest = new IdentitySeverAgent.AddClaimRequest() { UserId = dto.AmigoTenantTUserId, ClaimType = "role", ClaimValue = roleToAdd.Data.Code };
                var claimAddAgent = await _identityAgent.Save_ClaimRequest_IdentityServer(claimAddRequest);
                if (!claimAddAgent.IsSuccessStatusCode)
                    msg.AppendLine(Constants.Entity.AmigoTenantTUser.ErrorMessage.ClaimNoCreated);

            }
            //-------------------------------------------------------------------------------------------------------
            //using (var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings))
            //{
            //    dto.Id = dto.AmigoTenantTUserId;
            //    var response = await httpClient.PutAsJsonAsync("api/Users", dto);
            //    await ProcessResponse(response, httpClient, dto, msg);

            //    var claimRequestRemove = new RemoveClaimRequest() { UserId = amigoTenantTUser.AmigoTenantTUserId, ClaimType = "role", ClaimValue = roleToRemove.Data.Name };
            //    var claimResponseRemove = await httpClient.PostAsJsonAsync("api/Users/removeClaim", claimRequestRemove);
            //    if (!claimResponseRemove.IsSuccessStatusCode)
            //        msg.AppendLine(Constants.Entity.AmigoTenantTUser.ErrorMessage.ClaimNoRemoved);

            //    var claimRequestAdd = new AddClaimRequest() { UserId = dto.AmigoTenantTUserId, ClaimType = "role", ClaimValue = roleToAdd.Data.Name };
            //    var claimResponseAdd = await httpClient.PostAsJsonAsync("api/Users/addClaim", claimRequestAdd);
            //    if (!claimResponseAdd.IsSuccessStatusCode)
            //        msg.AppendLine(Constants.Entity.AmigoTenantTUser.ErrorMessage.ClaimNoCreated);


            //-------------------------------------
            //      Change Password ( "reset")
            //-------------------------------------
            if (!string.IsNullOrEmpty(dto.Password))
                {
                    var resetUserPasswordRequest = new IdentitySeverAgent.ResetUserPasswordRequest() { Id = dto.AmigoTenantTUserId, Password = dto.Password };
                 //var resetUserPasswordResponse = await httpClient.PostAsJsonAsync("api/Users/resetPassword", resetUserPasswordRequest);
                   var resetUserPasswordResponse = await _identityAgent.Reset_AmigoTenantTUser_Password_IdentityServer(resetUserPasswordRequest);

                if (resetUserPasswordResponse.IsSuccessStatusCode)
                    {
                        var content = await resetUserPasswordResponse.Content.ReadAsAsync<ResponseDTO>();
                        if (!content.IsValid)
                        {
                            return ResponseBuilder.InCorrect().WithMessages(content.Messages.ToArray());
                        }
                    }
                    else
                    {
                        return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage() { Message = Constants.Entity.AmigoTenantTUser.ErrorMessage.PasswordNotUpdated });
                    }
                }

          // }

            /****************************************************
              2. UPDATING IN SERVICE
            *****************************************************/

            var command = _mapper.Map<AmigoTenantTUserDTO, UpdateAmigoTenantTUserCommand>(dto);
            var userSearchRequest = new UserSearchRequest() { AmigoTenantTUserId = dto.AmigoTenantTUserId, UserName = dto.Username };
            var exists = await ExistsToUpdate(userSearchRequest);

            if (!exists)
                msg.AppendLine(string.Format("{0} on {1}", Constants.Entity.AmigoTenantTUser.ErrorMessage.UserNoExist, Constants.Services.AmigoTenantService));
            else
            {
                var resp = await _bus.SendAsync(command);
                return ResponseBuilder.Correct(resp);
            }

            return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage() { Message = msg.ToString() });
        }


        private Expression<Func<AmigoTenantTUserDTO, bool>> GetQueryFilter(UserSearchRequest search)
        {
            Expression<Func<AmigoTenantTUserDTO, bool>> queryFilter = p => true;

            queryFilter = queryFilter.And(p => p.RowStatus == true);

            if (search.AmigoTenantTUserId > 0)
                queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == search.AmigoTenantTUserId);

            if (search.DedicatedLocationId > 0)
                queryFilter = queryFilter.And(p => p.DedicatedLocationId == search.DedicatedLocationId);

            if (!string.IsNullOrEmpty(search.UserName))
                queryFilter = queryFilter.And(p => p.Username.Contains(search.UserName));

            if (!string.IsNullOrEmpty(search.FirstName))
                queryFilter = queryFilter.And(p => p.FirstName.Contains(search.FirstName));

            if (!string.IsNullOrEmpty(search.LastName))
                queryFilter = queryFilter.And(p => p.LastName.Contains(search.LastName));

            if (!string.IsNullOrEmpty(search.UserType))
                queryFilter = queryFilter.And(p => p.UserType.Contains(search.UserType));

            if (search.AmigoTenantTRoleId > 0)
                queryFilter = queryFilter.And(p => p.AmigoTenantTRoleId == search.AmigoTenantTRoleId);

            if (!string.IsNullOrEmpty(search.PayBy))
                queryFilter = queryFilter.And(p => p.PayBy.Contains(search.PayBy));

            if (search.DedicatedLocationId != null && search.DedicatedLocationId > 0)
                queryFilter = queryFilter.And(p => p.DedicatedLocationId == search.DedicatedLocationId);


            return queryFilter;
        }

        private Expression<Func<AmigoTenantTUserDTO, bool>> GetQueryFilterForExists(UserSearchRequest search)
        {
            Expression<Func<AmigoTenantTUserDTO, bool>> queryFilter = p => true;

            if (search.AmigoTenantTUserId > 0)
                queryFilter = queryFilter.And(p => p.AmigoTenantTUserId != search.AmigoTenantTUserId);

            if (!string.IsNullOrEmpty(search.UserName))
                queryFilter = queryFilter.And(p => p.Username.Contains(search.UserName));

            return queryFilter;
        }

        private Expression<Func<AmigoTenantTUserDTO, bool>> GetQueryFilterForUserName(UserSearchRequest search)
        {
            Expression<Func<AmigoTenantTUserDTO, bool>> queryFilter = p => true;

            if (!string.IsNullOrEmpty(search.UserName))
                queryFilter = queryFilter.And(p => p.Username.Contains(search.UserName));

            return queryFilter;
        }

        private Expression<Func<AmigoTenantTUserDTO, bool>> GetQueryFilterForId(UserSearchRequest search)
        {
            Expression<Func<AmigoTenantTUserDTO, bool>> queryFilter = p => true;

            if (search.AmigoTenantTUserId > 0)
                queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == search.AmigoTenantTUserId);

            return queryFilter;
        }

        public async Task<ResponseDTO> Delete(AmigoTenantTUserStatusDTO dto)
        {
            var command = _mapper.Map<AmigoTenantTUserStatusDTO, DeleteAmigoTenantTUserCommand>(dto);

            var userDto = new IdentityUserDTO()
            {
                Id = dto.AmigoTenantTUserId,
                RowStatus = false,
                UserName = dto.UserName
                
            };
            var msg = new StringBuilder();
            _identityAgent.IdentityServerClientSettings = this.IdentityServerClientSettings;
            var responseAgent = await _identityAgent.ChangeStatus_AmigoTenantTUserDTO_IdentityServer(userDto);
            await ProcessResponseAgent(responseAgent, userDto, msg);

            var resp = await _bus.SendAsync(command);
            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO> ValidateAuthorizationAsync(AuthorizationRequest search, int? amigoTenantTUserId)
        {
            var activityType = await _activityTypeService.SearchActivityByCodeAsync(Constants.ActivityTypeCode.Authentication);
            var command = _mapper.Map<AuthorizationRequest, UpdateDeviceAuthorizationCommand>(search);
            command.AssignedAmigoTenantTUserId = amigoTenantTUserId;
            command.ActivityTypeId = activityType.ActivityTypeId;

            //Execute Command
            
            var commandResult = await _bus.SendAsync(command);

            var reponseDTO = new ResponseDTO<AmigoTenantTUserDTO>();
            reponseDTO.IsValid = commandResult.IsCorrect;
            foreach (var item in commandResult.Errors)
            {
                var appMsg = new ApplicationMessage() { Key = null, Message = item };
                reponseDTO.Messages.Add(appMsg);
            }
            reponseDTO.Data = _mapper.Map<AmigoTenantTUser, AmigoTenantTUserDTO>(commandResult.Data);

            return reponseDTO;
        }

        private async Task SetUsersAdditionalInformationBasic(List<AmigoTenantTUserBasicDTO> userListDTO)
        {
            if (userListDTO.Any())
            {
                //var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings);
                //var rspUsersDetails = await httpClient.GetAsync("api/Users/GetUsersDetails" + GetUrlParameterValues(userListDTO.Select(q => q.Username).ToList()));

                //
                //var agent = new ISHttpClientAgent();
                _identityAgent.IdentityServerClientSettings = this.IdentityServerClientSettings;
                var rspUsersDetails = await _identityAgent.AmigoTenantTUser_Details_IdentityServer(userListDTO.Select(q => q.Username).ToList());


                if (rspUsersDetails.IsSuccessStatusCode)
                {
                    var userDetailsJson = await rspUsersDetails.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var users = JsonConvert.DeserializeObject<ResponseDTO<List<UserResponse>>>(userDetailsJson);

                    foreach (var item in userListDTO)
                    {
                        var user = users.Data.Where(q => q.UserName.ToLower() == item.Username.ToLower()).FirstOrDefault();
                        if (user != null)
                        {
                            item.FirstName = user.FirstName;
                            item.LastName = user.LastName;
                            item.CustomUsername = user.UserName + " - " + (user.FirstName == null ? string.Empty : user.FirstName) + " " + (user.LastName == null ? string.Empty : user.LastName);
                        }
                        else
                        {
                            item.CustomUsername = item.Username + " - " + (item.FirstName == null ? string.Empty : item.FirstName) + " " + (item.LastName == null ? string.Empty : item.LastName);
                        }
                    }
                }
                else
                {
                    throw new Exception("Amigo.Tenant.Application.Services.Security - AmigoTenantTUserApplicationService - SearchUsersByCriteriaAsync - call to IdentityServerHttpClient api/Users/GetUsersDetails was not successful");
                }

            }
        }

        public async Task<AmigoTenantTUserBasicDTO> GetBaseUserInfoById(int amigoTenantTUserId)
        {
            Expression<Func<AmigoTenantTUserBasicDTO, bool>> queryFilter = p => true;

            queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == amigoTenantTUserId);

            var result = await _userBasicDataAccess.FirstOrDefaultAsync(queryFilter);

            return result;
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

        public async Task<AmigoTenantTUserAuditDTO> SearchByIdForAudit(int? createdBy, int? updatedBy)
        {
            var amigoTenantTUserAuditDTO = new AmigoTenantTUserAuditDTO();
            var user = await GetUserById(createdBy);
            if (user != null)
            {
                amigoTenantTUserAuditDTO.CreatedBy = createdBy;
                amigoTenantTUserAuditDTO.CreatedByCode = user.Username;
                //amigoTenantTUserAuditDTO.CreationDate = user.CreationDate;
            }

            user = await GetUserById(updatedBy);
            if (user != null)
            {
                amigoTenantTUserAuditDTO.UpdatedBy = updatedBy;
                amigoTenantTUserAuditDTO.UpdatedByCode = user.Username;
                //amigoTenantTUserAuditDTO.UpdatedDate = user.UpdatedDate;
            }

            return amigoTenantTUserAuditDTO;
        }

        private async Task<AmigoTenantTUserDTO> GetUserById(int? userId)
        {
            Expression<Func<AmigoTenantTUserDTO, bool>> queryFilter = p => true;
            queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == userId);

            var user = await _userDataAccess.FirstOrDefaultAsync(queryFilter);
            return user;
        }
    }

    //public abstract class ClaimRequestBase
    //{
    //    public int UserId
    //    {
    //        get; set;
    //    }
    //    public string ClaimType
    //    {
    //        get; set;
    //    }
    //    public string ClaimValue
    //    {
    //        get; set;
    //    }
    //}

    //public class AddClaimRequest : ClaimRequestBase
    //{
    //}

    //public class RemoveClaimRequest : ClaimRequestBase
    //{
    //}
    //public class ResetUserPasswordRequest
    //{
    //    public int Id
    //    {
    //        get; set;
    //    }
    //    public string Password
    //    {
    //        get; set;
    //    }
    //}
}
