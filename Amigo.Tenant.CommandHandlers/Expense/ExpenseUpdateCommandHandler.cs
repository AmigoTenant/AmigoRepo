
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Expense;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Threading.Tasks;
using model = Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandHandlers.Expense
{
    public class ExpenseUpdateCommandHandler : IAsyncCommandHandler<ExpenseUpdateCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<model.Expense> _repository;
        //private readonly IRepository<ExpenseFeature> _repositoryFeatures;
        //private readonly IRepository<ExpenseCity> _repositoryCities;

        public ExpenseUpdateCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<model.Expense> repository,
         IUnitOfWork unitOfWork
         //IRepository<ExpenseCity> repositoryCities ,
         //IRepository<ExpenseFeature> repositoryFeatures
            )
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            //_repositoryCities = repositoryCities;
            //_repositoryFeatures = repositoryFeatures;
        }


        public async Task<CommandResult> Handle(ExpenseUpdateCommand message)
        {
            try
            {
                var entity = _mapper.Map<ExpenseUpdateCommand, model.Expense>(message);
                entity.Update(message.UserId);

                //=================================================
                //Contract
                //=================================================

                _repository.UpdatePartial(entity, new string[] {
                    "ExpenseId",
                    "ExpenseDate",
                    "PaymentTypeId",
                    "HouseId",
                    "PeriodId",
                    "ReferenceNo",
                    "Remark",
                    "SubTotalAmount",
                    "Tax",
                    "TotalAmount",
                    "UpdatedBy",
                    "UpdatedDate",
                });


                ////=================================================
                //// DETAILS
                ////=================================================
                //var existCityInDB = await _repositoryCities.ListAsync(w => w.ExpenseId == message.ExpenseId);
                //existCityInDB.ToList().ForEach(p => _repositoryCities.Delete(p));

                //foreach (var item in message.ExpenseCities)
                //{
                //    var expenseCity = _mapper.Map<ExpenseCityCommand, ExpenseCity>(item);
                //    _repositoryCities.Add(expenseCity);
                //}

                ////=================================================
                //// Features
                ////=================================================
                //var existFeatureInDB = await _repositoryFeatures.ListAsync(w => w.ExpenseId == message.ExpenseId);
                //existFeatureInDB.ToList().ForEach(p => _repositoryFeatures.Delete(p));

                //foreach (var item in message.ExpenseFeatures)
                //{
                //    var expenseFeature = _mapper.Map<ExpenseFeatureCommand, ExpenseFeature>(item);
                //    _repositoryFeatures.Add(expenseFeature);
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
