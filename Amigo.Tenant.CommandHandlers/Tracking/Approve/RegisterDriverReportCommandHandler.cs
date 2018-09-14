
using System;
using System.Threading.Tasks;
using MediatR;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.Move;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Events.Tracking;
using Amigo.Tenant.Commands.Tracking.Approve;
using System.Collections.Generic;
using System.Linq;
using Amigo.Tenant.Common;

namespace Amigo.Tenant.CommandHandlers.Tracking.Approve
{
  public  class RegisterDriverReportCommandHandler : IAsyncCommandHandler<RegisterDriverReportCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<DriverReport> _repository;
        private readonly IRepository<AmigoTenantTService> _repositoryAmigoTenantTService;

        private string[] updateFieldsForReject = new string[]
                   {   "ApprovalModified",
                        "ServiceStatus",
                        "UpdatedBy",
                        "UpdatedDate",
                        "ApprovalComments"
                   };

        private string[] updateFieldsForApprove = new string[]
                    {   "ApprovedBy",
                        "ApprovalDate",
                        "ApprovalModified",
                        "ServiceStatus",
                        "UpdatedBy",
                        "UpdatedDate",
                        "ApprovalComments"
                    };


        public RegisterDriverReportCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<DriverReport> repository,
         IRepository<AmigoTenantTService> repositoryAmigoTenantTService,
         IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _repositoryAmigoTenantTService = repositoryAmigoTenantTService;
            _unitOfWork = unitOfWork;
        }


        public async Task<CommandResult> Handle(RegisterDriverReportCommand message)
        {
            try
            {
                UpdateAmigoTenantTServicesAccordApproveOrReject(message.AmigoTenantTServiceCharges);
                RemoveRejectedAmigoTenantTServicesFromCharges(message);
                var entity = _mapper.Map<RegisterDriverReportCommand, DriverReport>(message);
                if (message.AmigoTenantTServiceCharges.Any())
                {
                    _repository.Add(entity);
                    await _unitOfWork.CommitAsync();
                    //Publish bussines Event
                    await SendLogToAmigoTenantTEventLog(message);
                    return entity.ToRegisterdResult().WithId(entity.DriverReportId);
                }

                await _unitOfWork.CommitAsync();
                //Publish bussines Event
                await SendLogToAmigoTenantTEventLog(message);
                return entity.ToRegisterdResult();
            }
            catch (Exception ex)
            {
                await SendLogToAmigoTenantTEventLog(message, ex.Message);
                throw;
            }
        }

        private void RemoveRejectedAmigoTenantTServicesFromCharges(RegisterDriverReportCommand driverReportCommand)
        {
            var chargesDetails = new List<RegisterAmigoTenantTServiceChargeCommand>();
            var serviceCharges = driverReportCommand.AmigoTenantTServiceCharges.Where(q => q.ApproveOrReject.HasValue && q.ApproveOrReject.Value);

            foreach (var serviceCharge in serviceCharges)
            {
                serviceCharge.AmigoTenantTService = null;
                serviceCharge.AmigoTenantTServiceList = null;
                chargesDetails.Add(serviceCharge);
            }
            driverReportCommand.AmigoTenantTServiceCharges = chargesDetails;
        }

        private void UpdateAmigoTenantTServicesAccordApproveOrReject(List<RegisterAmigoTenantTServiceChargeCommand> amigoTenantTServiceCharges)
        {
            var chargesDetails = new List<RegisterAmigoTenantTServiceChargeCommand>();

            foreach (var amigoTenantTServiceCharge in amigoTenantTServiceCharges)
            {
                if (amigoTenantTServiceCharge.AmigoTenantTService != null)
                {
                    if (amigoTenantTServiceCharge.ApproveOrReject.Value)
                    {
                        var entity = _mapper.Map<UpdateAmigoTenantServiceCommand, AmigoTenantTService>(amigoTenantTServiceCharge.AmigoTenantTService);
                        _repositoryAmigoTenantTService.UpdatePartial(entity, updateFieldsForApprove);

                    }
                    else if (!amigoTenantTServiceCharge.ApproveOrReject.Value)
                    {
                        var entity = _mapper.Map<UpdateAmigoTenantServiceCommand, AmigoTenantTService>(amigoTenantTServiceCharge.AmigoTenantTService);
                        _repositoryAmigoTenantTService.UpdatePartial(entity, updateFieldsForReject);
                    }
                }
            }
        }

        private async Task SendLogToAmigoTenantTEventLog(RegisterDriverReportCommand message, string errorMsg = "")
        {
            //Publish bussines Event
            var eventData = _mapper.Map<RegisterDriverReportCommand, RegisterMoveEvent>(message);
            eventData.LogType = string.IsNullOrEmpty(errorMsg) ? Constants.AmigoTenantTEventLogType.In : Constants.AmigoTenantTEventLogType.Err;
            eventData.Parameters = string.IsNullOrEmpty(errorMsg) ? string.Join("|", message.AmigoTenantTServiceIdsListStatus.Select(p => p.AmigoTenantTServiceId).ToArray()) : errorMsg;
            eventData.ReportedActivityDate = DateTime.Now;
            eventData.Username = message.Username;
            await _bus.PublishAsync(eventData);

        }

    }
}
