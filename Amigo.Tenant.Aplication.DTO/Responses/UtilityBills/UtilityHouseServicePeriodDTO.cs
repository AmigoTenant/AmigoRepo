using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.UtilityBills
{
    public class UtilityHouseServicePeriodDTO
    {
        #region HouseService
        public int HouseId { get; set; }
        public int HouseServiceId { get; set; }
        public int ServiceId { get; set; }
        public bool RowStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }

        public int ConceptId { get; set; }
        public string ConceptCode { get; set; }
        public string ConceptDescription { get; set; }
        public int ConceptTypeId { get; set; }

        public int BusinessPartnerId { get; set; }
        public string BusinessPartnerName { get; set; }
        public string BusinessPartnerCode { get; set; }

        public int ServiceTypeId { get; set; }
        public string ServiceTypeCode { get; set; }
        public string ServiceTypeValue { get; set; }

        #endregion

        public int MonthId { get; set; }
        public int DueDateMonth { get; set; }
        public int DueDateDay { get; set; }
        public int CutOffMonth { get; set; }
        public int CutOffDay { get; set; }
        public int HouseServicePeriodId { get; set; }

        public int PeriodId { get; set; }

        #region ServicePeriod

        public decimal? Amount { get; set; }
        public decimal? Adjust { get; set; }
        public decimal? Consumption { get; set; }
        public int? ConsumptionUnmId { get; set; }
        public int? HouseServicePeriodStatusId { get; set; }

        public int HouseServicePeriodCreatedBy { get; set; }
        public DateTime HouseServicePeriodCreationDate { get; set; }

        #endregion


        public string PeriodCode { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DueDate { get; set; }
        public int? Sequence { get; set; }

        public int PeriodCreatedBy { get; set; }
        public DateTime PeriodCreationDate { get; set; }
    }
}
