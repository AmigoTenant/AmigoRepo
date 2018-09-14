using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExpressMapper;
using Microsoft.AspNet.Identity;
using Amigo.Tenant.IdentityServer.ApplicationServices.Extensions;
using Amigo.Tenant.IdentityServer.ApplicationServices.Helpers.Extensions;
using Amigo.Tenant.IdentityServer.DTOs.Requests.Users;
using Amigo.Tenant.IdentityServer.DTOs.Responses.Common;
using Amigo.Tenant.IdentityServer.DTOs.Responses.Users;
using Amigo.Tenant.IdentityServer.Infrastructure.ExternalAuthentication.Windows;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Manager;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model;
using Constants = Amigo.Tenant.Common.Constants;

namespace Amigo.Tenant.IdentityServer.ApplicationServices.Services
{
    public class UserAdministrationService : IUserAdministrationService
    {
        private readonly UserManager _userManager;
        private readonly UserStore _userStore;
        private readonly WindowsUserValidator _windowsUserValidator;

        public UserAdministrationService(UserManager userManager, UserStore userStore,
            WindowsUserValidator windowsUserValidator)
        {
            _userManager = userManager;
            _userStore = userStore;
            _windowsUserValidator = windowsUserValidator;
        }

        public async Task<ResponseDTO<UserResponse>> GetDetails(string username)
        {
            var userdb = await _userStore.FindByNameAsync(username);
            var user = Mapper.Map<User, UserResponse>(userdb);
            return ResponseBuilder.Correct(user);
        }

        public async Task<ResponseDTO<List<UserResponse>>> GetUsersDetails(List<string> usernames)
        {
            Expression<Func<User, bool>> filter = x => usernames.Contains(x.UserName);

            var userdb = await _userStore.FindUsersAsync(filter);
            var users = Mapper.Map<List<User>, List<UserResponse>>(userdb);
            return ResponseBuilder.Correct(users);
        }

        public async Task<ResponseDTO<PagedList<UserResponse>>> Search(SearchUserRequest search)
        {
            if (search == null) throw new ArgumentNullException(nameof(search));

            if (search.IsSearchingByUserNameOnly() && _windowsUserValidator.Exists(search.UserName))
            {                                
                var r = await CreateWindowsUserIfNotExist(search.UserName);

                if (!r.IsValid) return ResponseBuilder.InCorrect<PagedList<UserResponse>>().WithMessage("The user could not be created.");

                var resp = new PagedList<UserResponse>
                {
                    PageSize = 1,
                    Total = 1,
                    Page = 1,
                    Items = new List<UserResponse> {r.Data}
                };
                return ResponseBuilder.Correct(resp);
            }

            Expression<Func<User, bool>> filter = x => x.LockoutEndDateUtc == null;

            if (search.Id.IsNotEmpty())
                filter = filter.And(x => x.Id == search.Id);

            if (search.UserName.IsNotEmpty())
                filter = filter.And(x => x.UserName.Contains(search.UserName));

            if (search.FirstName.IsNotEmpty())
                filter = filter.And(x => x.FirstName.Contains(search.FirstName));

            if (search.LastName.IsNotEmpty())
                filter = filter.And(x => x.LastName.Contains(search.LastName));

            if (search.Email.IsNotEmpty())
                filter = filter.And(x => x.Email.Contains(search.Email));

            if (search.Claim.IsNotEmpty())
                filter =
                    filter.And(
                        x =>
                            x.Claims.Any(
                                c => c.ClaimType == search.Claim.ClaimType && c.ClaimValue == search.Claim.ClaimValue));

            var users = await _userStore.FindUsersAsync(filter, search.Page, search.PageSize);
            var total = await _userStore.CountUsersAsync(filter);

            var usersMapped = Mapper.Map<List<User>, List<UserResponse>>(users);

            var list = new PagedList<UserResponse>
            {
                PageSize = search.PageSize,
                Page = search.Page,
                Total = total,
                Items = usersMapped
            };

            return ResponseBuilder.Correct(list);
        }

        private async Task<ResponseDTO<UserResponse>> CreateWindowsUserIfNotExist(string userName)
        {
            var existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser != null)
            {
                var respExisting = new UserResponse()
                {
                    UserName = existingUser.UserName,
                    Email = existingUser.Email,
                    Id = existingUser.Id,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName
                };
                return ResponseBuilder.Correct(respExisting);
            }

            var details = _windowsUserValidator.GetProfileInfo(userName);
            var user = new User(userName)
            {
                Email = details.Email,
                EmailConfirmed = true,
                FirstName = details.FirstName,
                LastName = details.LastName
            };
            user.RowStatus = true;
            var createdResult = await _userManager.CreateAsync(user);
            if (!createdResult.Succeeded)
            {
                return ResponseBuilder.InCorrect<UserResponse>().WithMessage(createdResult.Errors.First());
            }

            var externalLogin = new UserLoginInfo("Windows", user.UserName);

            var addExternalResult = await _userManager.AddLoginAsync(user.Id, externalLogin);
            if (!addExternalResult.Succeeded)
            {
                return ResponseBuilder.InCorrect<UserResponse>().WithMessage(addExternalResult.Errors.First());
            }
            var us = new UserResponse()
            {
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return ResponseBuilder.Correct(us);
        }

        public async Task<ResponseDTO<UserResponse>> Register(RegisterUserRequest user)
        {
            var userdb = Mapper.Map<RegisterUserRequest, User>(user);
            var msg = new StringBuilder();
            var response = new UserResponse();
            var userDto = await GetDetails(user.UserName);
            IdentityResult result;
            userdb.RowStatus = true;

            if (userDto.Data != null)
            {
                if (userDto.Data.RowStatus.HasValue && userDto.Data.RowStatus.Value)
                {
                    msg.AppendLine(
                        $"{Constants.Entity.AmigoTenantTUser.ErrorMessage.UserExist} on {Constants.Services.IdentityService}");
                    return
                        ResponseBuilder.InCorrect(userDto.Data)
                            .WithMessages(new ApplicationMessage {Message = msg.ToString()});
                }
                else
                {
                    userdb.FirstName = userDto.Data.FirstName;
                    userdb.LastName = userDto.Data.LastName;
                    userdb.Email = userDto.Data.Email;
                    userdb.PhoneNumber = userDto.Data.PhoneNumber;
                    userdb.Id = userDto.Data.Id;
                    result = await _userManager.UpdateAsync(userdb);
                    //_userManager.ResetPassword(userdb.Id,)
                }
                
            }
            else
                result = await _userManager.CreateAsync(userdb, user.Password);

            if (!result.Succeeded)
            {
                msg.AppendLine(
                    $"{Constants.Entity.Common.ErrorMessage.NonRegister} on {Constants.Services.IdentityService}");
                response = Mapper.Map<User, UserResponse>(userdb);
                return
                    ResponseBuilder.InCorrect(response).WithMessages(new ApplicationMessage {Message = msg.ToString()});
            }
            response = Mapper.Map<User, UserResponse>(userdb);
            return ResponseBuilder.Correct(response).WithMessages(new ApplicationMessage {Message = msg.ToString()});
        }

        public async Task<ResponseDTO<UserResponse>> RegisterExternalUser(RegisterExternalUserRequest request)
        {
            var userExists = _windowsUserValidator.Exists(request.UserName);
            if (!userExists)
                return ResponseBuilder.InCorrect<UserResponse>().WithMessage("The user does not exist.");

            var details = _windowsUserValidator.GetProfileInfo(request.UserName);
            var user = Mapper.Map<WindowsUserInfo, User>(details);

            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                var respExisting = Mapper.Map<User, UserResponse>(existingUser);
                return ResponseBuilder.Correct(respExisting);
            }

            user.RowStatus = true;
            var createdResult = await _userManager.CreateAsync(user);
            if (!createdResult.Succeeded) {
                return ResponseBuilder.InCorrect<UserResponse>().WithMessage(createdResult.Errors.First());
            }

            var externalLogin = new UserLoginInfo("Windows", user.UserName);

            var addExternalResult = await _userManager.AddLoginAsync(user.Id, externalLogin);
            if (!addExternalResult.Succeeded)
            {
                return ResponseBuilder.InCorrect<UserResponse>().WithMessage(addExternalResult.Errors.First());                
            }

            var resp = Mapper.Map<WindowsUserInfo, UserResponse>(details);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO> Update(UpdateUserRequest user)
        {
            var msg = new StringBuilder();
            var userdb = _userManager.FindById(user.Id);
            if (userdb == null)
            {
                msg.AppendLine(
                    $"{Constants.Entity.AmigoTenantTUser.ErrorMessage.UserNoExist} on {Constants.Services.IdentityService}");
                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage {Message = msg.ToString()});
            }

            if (user.FirstName.IsNotEmpty())
                userdb.FirstName = user.FirstName;

            if (user.LastName.IsNotEmpty())
                userdb.LastName = user.LastName;

            if (user.Email.IsNotEmpty())
                userdb.Email = user.Email;

            if (user.PhoneNumber.IsNotEmpty())
                userdb.PhoneNumber = user.PhoneNumber;

            if (user.RowStatus.HasValue)
                userdb.RowStatus = user.RowStatus;

            var result = await _userManager.UpdateAsync(userdb);

            if (!result.Succeeded)
            {
                msg.AppendLine(
                    $"{Constants.Entity.Common.ErrorMessage.NonRegister} on {Constants.Services.IdentityService}");
                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage {Message = msg.ToString()});
            }

            var response = Mapper.Map<User, UserResponse>(userdb);
            return ResponseBuilder.Correct(response).WithMessages(new ApplicationMessage {Message = msg.ToString()});
        }

        public async Task<ResponseDTO> ResetPassword(ResetUserPasswordRequest resetPasswordRequest)
        {
            var msg = new StringBuilder();
            var userdb = _userManager.FindById(resetPasswordRequest.Id);
            if (userdb == null)
            {
                msg.AppendLine(
                    $"{Constants.Entity.AmigoTenantTUser.ErrorMessage.UserNoExist} on {Constants.Services.IdentityService}");
                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage {Message = msg.ToString()});
            }

            if (ValidatePassword(resetPasswordRequest.Password))
            {
                msg.AppendLine("Password must contain letters a-zA-Z and at least one digit 0-9");
                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage {Message = msg.ToString()});
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(resetPasswordRequest.Id);
            var result = await _userManager.ResetPasswordAsync(userdb.Id, token, resetPasswordRequest.Password);

            if (!result.Succeeded)
            {
                msg.AppendLine(
                    $"{Constants.Entity.Common.ErrorMessage.NonRegister} on {Constants.Services.IdentityService}");
                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage {Message = msg.ToString()});
            }
            return ResponseBuilder.Correct().WithMessages(new ApplicationMessage {Message = msg.ToString()});
        }

        public async Task<ResponseDTO> ChangePassword(ChangeUserPasswordRequest changePasswordRequest)
        {
            var msg = new StringBuilder();
            var userdb = _userManager.FindById(changePasswordRequest.Id);
            if (userdb == null)
            {
                msg.AppendLine(
                    $"{Constants.Entity.AmigoTenantTUser.ErrorMessage.UserNoExist} on {Constants.Services.IdentityService}");
                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage {Message = msg.ToString()});
            }

            if (string.IsNullOrWhiteSpace(changePasswordRequest.OldPassword))
            {
                msg.AppendLine("Old password must not be empty.");
                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage {Message = msg.ToString()});
            }

            if (ValidatePassword(changePasswordRequest.NewPassword))
            {
                msg.AppendLine("Password must contain letters a-zA-Z and at least one digit 0-9");
                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage { Message = msg.ToString() });
            }

            var result =
                await
                    _userManager.ChangePasswordAsync(userdb.Id, changePasswordRequest.OldPassword,
                        changePasswordRequest.NewPassword);

            if (!result.Succeeded)
            {
                msg.AppendLine(string.Join(",", result.Errors));
                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage {Message = msg.ToString()});
            }
            return ResponseBuilder.Correct().WithMessages(new ApplicationMessage {Message = msg.ToString()});
        }

        public async Task<ResponseDTO> AddClaim(AddClaimRequest claim)
        {
            var claimdb = Mapper.Map<AddClaimRequest, Claim>(claim);

            var result = await _userManager.AddClaimAsync(claim.UserId, claimdb);

            if (!result.Succeeded)
                return ResponseBuilder.InCorrect<UserResponse>()
                    .WithMessages(result.Errors.Select(x => new ApplicationMessage(null, x)).ToArray());

            return ResponseBuilder.Correct();
        }

        public async Task<ResponseDTO> RemoveClaim(RemoveClaimRequest claim)
        {
            var claimdb = Mapper.Map<RemoveClaimRequest, Claim>(claim);

            var result = await _userManager.RemoveClaimAsync(claim.UserId, claimdb);

            if (!result.Succeeded)
                return ResponseBuilder.InCorrect<UserResponse>()
                    .WithMessages(result.Errors.Select(x => new ApplicationMessage(null, x)).ToArray());

            return ResponseBuilder.Correct();
        }

        private static bool ValidatePassword(string password)
        {
            return string.IsNullOrEmpty(password) || !Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-zA-Z]).{6,12}$");
        }

        public async Task<ResponseDTO> ChangeStatus(UpdateStatusUserRequest user)
        {
            var msg = new StringBuilder();
            User userdb = _userManager.FindById(user.Id);
            if (userdb == null)
            {
                msg.AppendLine(
                    $"{Constants.Entity.AmigoTenantTUser.ErrorMessage.UserNoExist} on {Constants.Services.IdentityService}");
                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage { Message = msg.ToString() });
            }

            if (user.RowStatus.HasValue)
                userdb.RowStatus = user.RowStatus;
            
            var result = await _userManager.UpdateAsync(userdb);

            if (!result.Succeeded)
            {
                msg.AppendLine(
                    $"{Constants.Entity.Common.ErrorMessage.NonRegister} on {Constants.Services.IdentityService}");
                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage { Message = msg.ToString() });
            }

            var response = Mapper.Map<User, UserResponse>(userdb);
            return ResponseBuilder.Correct(response).WithMessages(new ApplicationMessage { Message = msg.ToString() });
        }
    }
}