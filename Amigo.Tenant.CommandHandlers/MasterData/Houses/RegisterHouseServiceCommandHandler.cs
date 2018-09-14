using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.MasterData.Houses;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using System;
using Amigo.Tenant.Common;

namespace Amigo.Tenant.CommandHandlers.MasterData.Houses
{
    public class RegisterHouseServiceCommandHandler : IAsyncCommandHandler<RegisterHouseServiceCommand>
    {
        private int currentYear = DateTime.Now.Year;
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<HouseService> _houseServRepository;
        private readonly IRepository<Period> _periodRepository;
        private readonly IRepository<GeneralTable> _generalRepository;
        private readonly IRepository<EntityStatus> _entityRepository;

        public RegisterHouseServiceCommandHandler(IBus bus, IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IRepository<HouseService> 
            houseServRepository, 
            IRepository<Period> periodRepository,
            IRepository<GeneralTable> generalRepository,
            IRepository<EntityStatus> entityRepository)
        {
            _bus = bus;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _houseServRepository = houseServRepository;
            _periodRepository = periodRepository;
            _generalRepository = generalRepository;
            _entityRepository = entityRepository;
        }

        public async Task<CommandResult> Handle(RegisterHouseServiceCommand message)
        {
            HouseService entity = _mapper.Map<RegisterHouseServiceCommand, HouseService>(message);

            entity.RowStatus = true;
            entity.Creation(message.UserId);
            entity.HouseServicePeriods.ToList().ForEach(p => p.Creation(message.UserId));
            if (entity.HasErrors) return entity.ToResult();

            // Initial values
            var statusHouseServicePeriodDraft = 
                await _entityRepository.FirstOrDefaultAsync(p => p.EntityCode == Constants.EntityCode.HouseServicePeriodStatus
                    && p.Code == Constants.EntityStatus.HouseServicePeriodStatus.Draft);

            // Get Period
            var yearPeriods = await _periodRepository.ListAsync(p => p.Code.StartsWith(currentYear.ToString()));
            foreach (var hsp in entity.HouseServicePeriods)
            {
                if (yearPeriods.ToList().Any(p => p.BeginDate.Value.Month == hsp.MonthId))
                {
                    hsp.PeriodId = yearPeriods.ToList().First(p => p.BeginDate.Value.Month == hsp.MonthId).PeriodId;
                    hsp.HouseServicePeriodStatusId = statusHouseServicePeriodDraft.EntityStatusId;
                }
            }
            //Insert
            _houseServRepository.Add(entity);
            await _unitOfWork.CommitAsync();

            //Return result
            HouseService emptyEntity = new HouseService();
            return emptyEntity.ToResult();
        }

        //public async Task<CommandResult> Handle(RegisterHouseServiceCommand message)
        //{
        //    for (int i = 0; i < ((List<RegisterHouseServiceMonthDayCommand>)message.DueDates).Count; i++)
        //    {
        //        HouseService entity = _mapper.Map<RegisterHouseServiceCommand, HouseService>(message);

        //        entity.RowStatus = true;
        //        entity.Creation(message.UserId);

        //        //entity.DueDateMonth = message.DueDates[i].ToString();
        //        //entity.DueDateDay = message.InitialServices[i].ToString();
        //        //entity.CutOffMonth = message.FinalServices[i].ToString();

        //        if (entity.HasErrors) return entity.ToResult();

        //        //Insert
        //        _houseFeatRepository.Add(entity);
        //    }

        //    await _unitOfWork.CommitAsync();

        //    //Publish bussines Event
        //    //await _bus.PublishAsync(new HouseRegistered() { Id = entity.HouseId });

        //    //Return result
        //    HouseService emptyEntity = new HouseService();
        //    return emptyEntity.ToResult();
        //}

    }
}
