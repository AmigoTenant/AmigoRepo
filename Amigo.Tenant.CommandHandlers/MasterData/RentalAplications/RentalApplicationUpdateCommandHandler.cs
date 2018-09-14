
using System;
using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Leasing.Contracts;
using Amigo.Tenant.Application.DTOs.Requests.Common;
using System.Linq;
using Amigo.Tenant.Commands.MasterData.RentalApplication;

namespace Amigo.Tenant.CommandHandlers.MasterData.RentalApplications
{
    public class RentalApplicationUpdateCommandHandler : IAsyncCommandHandler<RentalApplicationUpdateCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<RentalApplication> _repository;
        private readonly IRepository<RentalApplicationFeature> _repositoryFeatures;
        private readonly IRepository<RentalApplicationCity> _repositoryCities;

        public RentalApplicationUpdateCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<RentalApplication> repository,
         IUnitOfWork unitOfWork,
         IRepository<RentalApplicationCity> repositoryCities ,
         IRepository<RentalApplicationFeature> repositoryFeatures
            )
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _repositoryCities = repositoryCities;
            _repositoryFeatures = repositoryFeatures;
        }


        public async Task<CommandResult> Handle(RentalApplicationUpdateCommand message)
        {
            try
            {
                var entity = _mapper.Map<RentalApplicationUpdateCommand, RentalApplication>(message);
                entity.Update(message.UserId);

                //=================================================
                //Contract
                //=================================================

                _repository.UpdatePartial(entity, new string[] { 
                    "RentalApplicationId",
                    "PeriodId",
                    "PropertyTypeId",
                    "ApplicationDate",
                    "FullName",
                    "Email",
                    "HousePhone",
                    "CellPhone",
                    "CheckIn",
                    "CheckOut",
                    "ResidenseCountryId",
                    "BudgetId",
                    "Comment",
                    "RowStatus",
                    "HousePartId",
                    "PersonNo",
                    "OutInDownId",
                    "UpdatedBy",
                    "UpdatedDate",
                    "ReferredById",
                    "ReferredByOther",
                    "PriorityId",
                    "AlertDate",
                    "AlertMessage"
                });


                ////=================================================
                //// Cities
                ////=================================================
                //var existCityInDB = await _repositoryCities.ListAsync(w => w.RentalApplicationId == message.RentalApplicationId);
                //existCityInDB.ToList().ForEach(p => _repositoryCities.Delete(p));

                //foreach (var item in message.RentalApplicationCities)
                //{
                //    var rentalApplicationCity = _mapper.Map<RentalApplicationCityCommand, RentalApplicationCity>(item);
                //    _repositoryCities.Add(rentalApplicationCity);
                //}

                ////=================================================
                //// Features
                ////=================================================
                //var existFeatureInDB = await _repositoryFeatures.ListAsync(w => w.RentalApplicationId == message.RentalApplicationId);
                //existFeatureInDB.ToList().ForEach(p => _repositoryFeatures.Delete(p));

                //foreach (var item in message.RentalApplicationFeatures)
                //{
                //    var rentalApplicationFeature = _mapper.Map<RentalApplicationFeatureCommand, RentalApplicationFeature>(item);
                //    _repositoryFeatures.Add(rentalApplicationFeature);
                //}


                await _unitOfWork.CommitAsync();
                return entity.ToResult();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //private async Task SendLogToAmigoTenantTEventLog(RegisterAmigoTenanttServiceCommand message, string errorMsg = "")
        //{
        //    //Publish bussines Event
        //    var eventData = _mapper.Map<RegisterAmigoTenanttServiceCommand, RegisterMoveEvent>(message);
        //    eventData.LogType = string.IsNullOrEmpty(errorMsg)? Constants.AmigoTenantTEventLogType.In:Constants.AmigoTenantTEventLogType.Err;
        //    eventData.Parameters = errorMsg;
        //    eventData.Username = message.Username;
        //    await _bus.PublishAsync(eventData);
        //}
    }
}
