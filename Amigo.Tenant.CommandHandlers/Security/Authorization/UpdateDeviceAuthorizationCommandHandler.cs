using System;
using System.Linq;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Response.Security;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.CommandModel.Security;
using Amigo.Tenant.Commands.Common;

using Amigo.Tenant.Events.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Common;
using MediatR;
using Amigo.Tenant.Commands.Security.Authorization;
using System.Text;
using System.Collections.Generic;

namespace Amigo.Tenant.CommandHandlers.Security.Users
{
    public class UpdateDeviceAuthorizationCommandHandler : IAsyncRequestHandler<UpdateDeviceAuthorizationCommand, CommandResult<AmigoTenantTUser>>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Device> _repository;
        private readonly IQueryDataAccess<UserAuthorizationDTO> _userAuthorizationRepository;

        public UpdateDeviceAuthorizationCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<Device> repository,
            IQueryDataAccess<UserAuthorizationDTO> userAuthorizationRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _userAuthorizationRepository = userAuthorizationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult<AmigoTenantTUser>> Handle(UpdateDeviceAuthorizationCommand message)
        {
            try
            {
                var entity = _mapper.Map<UpdateDeviceAuthorizationCommand, AmigoTenantTUser>(message);

                //SEARCH BY USERNAME
                var result = (await _userAuthorizationRepository.ListAsync(p => p.AmigoTenantTUserId == message.AssignedAmigoTenantTUserId)).ToList();
                message.AssignedAmigoTenantTUserName = string.Empty;
                message.LogType = Constants.AmigoTenantTEventLogType.Err;
                if (result.Count == 1)
                {
                    var user = result.FirstOrDefault();
                    var userId = user.AmigoTenantTUserId;
                    var identifierByUser = user.Identifier;
                    var cellPhoneNo = user.CellphoneNumber;
                    var deviceId = user.DeviceId;

                    entity.DedicatedLocationId = user.DedicatedLocationId;
                    entity.AmigoTenantTUserId = user.AmigoTenantTUserId.Value;
                    entity.PayBy = user.PayBy;
                    entity.FirstName = user.FirstName;
                    entity.LastName = user.LastName;
                    entity.Username = user.AssignedAmigoTenantTUserUsername;
                    message.AssignedAmigoTenantTUserName = user.AssignedAmigoTenantTUserUsername;
                    message.CreatedBy = user.AmigoTenantTUserId.Value;

                    var byPassValidation = user.BypassDeviceValidation.HasValue ? user.BypassDeviceValidation.Value : false;

                    if (!byPassValidation)
                    {
                        if (!await IsCellphoneNumberValid(message.Identifier, identifierByUser))
                            entity.AddError(Constants.ErrorMessages.Authorization.UserDeviceCellphoneDifferentToIdentifierCellphone);

                        if (await IsIdentifierAssinedToAnotherUser(message.Identifier, userId.Value))
                            entity.AddError(Constants.ErrorMessages.Authorization.IdentifierWasAssignedMoreThanOneTime);
                        else
                        {
                            if (string.IsNullOrEmpty(identifierByUser))
                            {
                                var cellphones = (await _repository.ListAsync(p => p.CellphoneNumber == cellPhoneNo && p.RowStatus.Value)).ToList();
                                if (cellphones.Count == 1 && cellphones.FirstOrDefault().AssignedAmigoTenantTUserId == userId)
                                {
                                    var lastUpdateDate = !user.UserUpdatedDate.HasValue ? DateTime.Now : user.UserUpdatedDate;
                                    var hoursBetweenNowAndLastUpd = DateTime.Now.Subtract(lastUpdateDate.Value).TotalHours;

                                    if (lastUpdateDate.HasValue && hoursBetweenNowAndLastUpd < 24)
                                    {
                                        var device = (await _repository.FirstOrDefaultAsync(p => p.DeviceId == deviceId));

                                        if (device != null)
                                        {
                                            //UPDATE IDENTIFIER
                                            _repository.UpdatePartial(device, "Identifier", "UpdatedDate", "UpdatedBy");
                                            device.Identifier = message.Identifier;
                                            device.UpdatedDate = DateTime.UtcNow;
                                            device.UpdatedBy = message.AssignedAmigoTenantTUserId;
                                            await _unitOfWork.CommitAsync();
                                            message.LogType = Constants.AmigoTenantTEventLogType.In;
                                        }
                                    }
                                    else
                                    {
                                        entity.AddError(Constants.ErrorMessages.Authorization.ExpiredAccessToLogin);
                                    }

                                }
                                else
                                {
                                    if (cellphones.Any())
                                    {
                                        entity.AddError(Constants.ErrorMessages.Authorization.CellphoneNoAsociatedToOtherUser);
                                    }
                                    else
                                    {
                                        entity.AddError(Constants.ErrorMessages.Authorization.CellphoneNotAsociated);
                                    }
                                }

                            }
                            else if (!string.IsNullOrEmpty(identifierByUser) && identifierByUser != message.Identifier)
                            {
                                entity.AddError(Constants.ErrorMessages.Authorization.CurrentMobileDeviceDoesNotMatch);
                            }
                        }
                    }
                }
                else if (!result.Any())
                {
                    entity.AddError(Constants.ErrorMessages.Authorization.UserIsNotRegisterInDB);
                }
                else
                {
                    entity.AddError(Constants.ErrorMessages.Authorization.MoreThanOneDeviceAsociatedToYou);
                }

                //VERIFYING ERRORS TO SAVE ON SHUTTLETEVENTLOG
                var msgs = new StringBuilder();
                if (entity.Errors.Any())
                {
                    foreach (var item in entity.Errors)
                    {
                        msgs.AppendLine(string.Format("{0}{1}", item, System.Environment.NewLine));
                    }
                }
                await SendLogToAmigoTenantTEventLog(message, msgs.ToString());

                var commandResult = new CommandResult<AmigoTenantTUser>(entity.Errors);
                commandResult.Data = entity;
                
                return commandResult;
            }
            catch (Exception ex)
            {
                await SendLogToAmigoTenantTEventLog(message, ex.Message);
                throw;
            }

        }

        private async Task<bool> IsCellphoneNumberValid(string messageIdentifier, string identifierByUser)
        {
            var cellByIdentifier = (await _repository.ListAsync(p => p.Identifier == messageIdentifier && p.RowStatus.Value && p.Identifier != identifierByUser)).ToList();

            if (cellByIdentifier.Count > 1)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> IsIdentifierAssinedToAnotherUser(string messageIdentifier, int userId)
        {
            var isIdentifierAssinedToAnotherUser = (await _repository.AnyAsync(p => p.Identifier == messageIdentifier && p.RowStatus.Value && p.AssignedAmigoTenantTUserId != userId));
            return isIdentifierAssinedToAnotherUser;
        }

        private async Task SendLogToAmigoTenantTEventLog(UpdateDeviceAuthorizationCommand message, string errorMsg = "")
        {
            //Publish bussines Event
            var eventData = _mapper.Map<UpdateDeviceAuthorizationCommand, RegisterMoveEvent>(message);
            eventData.LogType = string.IsNullOrEmpty(errorMsg) ? Constants.AmigoTenantTEventLogType.In : Constants.AmigoTenantTEventLogType.Err;
            eventData.Parameters = errorMsg;
            eventData.Username = message.AssignedAmigoTenantTUserName.ToString();
            await _bus.PublishAsync(eventData);

        }
    }
}
