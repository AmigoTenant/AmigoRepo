using Amigo.Tenant.Application.Services.Interfaces.UtilityBills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using System.Linq.Expressions;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Application.DTOs.Responses.UtilityBills;

namespace Amigo.Tenant.Application.Services.UtilityBills
{
    public class UtilityBillApplicationService : IUtilityBillApplicationService
    {
        private readonly IQueryDataAccess<UtilityHouseServicePeriodDTO> _houseServiceDataAccess;

        public UtilityBillApplicationService(IBus bus, IQueryDataAccess<UtilityHouseServicePeriodDTO> houseServiceDataAccess,
            IMapper mapper)
        {
            _houseServiceDataAccess = houseServiceDataAccess;
        }

        public async Task<ResponseDTO<List<UtilityBillDTO>>> GetHouseServices(int houseId, int year)
        {
            Expression<Func<UtilityHouseServicePeriodDTO, bool>> queryFilter =
                c => c.RowStatus == true 
                    && c.HouseId == houseId
                    && c.PeriodCode.StartsWith(year.ToString());

            var servicePeriods = (await _houseServiceDataAccess.ListAsync(queryFilter)).ToList();

            var list = GroupByHouseService(servicePeriods);

            return ResponseBuilder.Correct(list);
        }

        private List<UtilityBillDTO> GroupByHouseService(List<UtilityHouseServicePeriodDTO> list)
        {
            return list
                .GroupBy(p => new
                {
                    p.HouseServicePeriodId,
                    p.HouseServiceId,
                    p.MonthId,
                    p.DueDateMonth,
                    p.DueDateDay,
                    p.CutOffMonth,
                    p.CutOffDay,
                    p.RowStatus,
                    p.HouseServicePeriodCreatedBy,
                    p.HouseServicePeriodCreationDate,
                    p.PeriodId
                })
                .Select(p => new UtilityBillDTO()
                {
                    ServicePeriod = list
                                    .Where(q => q.HouseServicePeriodId == p.Key.HouseServicePeriodId)
                                    .Select(q => new UtilityServicePeriodDTO()
                                    {
                                        HouseServicePeriodId = p.Key.HouseServicePeriodId,
                                        PeriodId = q.PeriodId,
                                        Amount = q.Amount,
                                        Adjust = q.Adjust,
                                        Consumption = q.Consumption,
                                        ConsumptionUnmId = q.ConsumptionUnmId,
                                        HouseServicePeriodStatusId = q.HouseServicePeriodStatusId,
                                        RowStatus = true,
                                        CreatedBy = q.CreatedBy,
                                        CreationDate = q.CreationDate
                                    }).FirstOrDefault(),

                    HouseService = list
                                    .Where(q => q.HouseServiceId == p.Key.HouseServiceId)
                                    .Select(q => new UtilityHouseServiceDTO()
                                    {
                                        HouseServiceId = q.HouseServiceId,
                                        HouseId = q.HouseId,
                                        ServiceId = q.ServiceId,

                                        ConceptId = q.ConceptId,
                                        ConceptCode = q.ConceptCode,
                                        ConceptDescription = q.ConceptDescription,
                                        ConceptTypeId = q.ConceptTypeId,

                                        BusinessPartnerId = q.BusinessPartnerId,
                                        BusinessPartnerName = q.BusinessPartnerName,

                                        ServiceTypeId = q.ServiceTypeId,
                                        ServiceTypeCode = q.ServiceTypeCode,
                                        ServiceTypeValue = q.ServiceTypeValue,
                                        RowStatus = true
                                    }).FirstOrDefault(),

                    HouseServicePeriodId = p.Key.HouseServicePeriodId,
                    HouseServiceId = p.Key.HouseServiceId,
                    MonthId = p.Key.MonthId,
                    DueDateMonth = p.Key.DueDateMonth,
                    DueDateDay = p.Key.DueDateDay,
                    CutOffMonth = p.Key.CutOffMonth,
                    CutOffDay = p.Key.CutOffDay,
                    RowStatus = p.Key.RowStatus,
                    CreatedBy = p.Key.HouseServicePeriodCreatedBy,
                    CreationDate = p.Key.HouseServicePeriodCreationDate,
                    PeriodId = p.Key.PeriodId,

                    Period = list
                            .Where(q => q.PeriodId == p.Key.PeriodId)
                            .Select(q => new UtilityPeriodDTO()
                            {
                                PeriodId = p.Key.PeriodId,
                                Code = q.PeriodCode,
                                BeginDate = q.BeginDate,
                                EndDate = q.EndDate,
                                Sequence = q.Sequence,
                                RowStatus = true,
                                CreatedBy = q.PeriodCreatedBy,
                                CreationDate = q.CreationDate
                            }).FirstOrDefault()
                }).ToList();
        }
    }
}
