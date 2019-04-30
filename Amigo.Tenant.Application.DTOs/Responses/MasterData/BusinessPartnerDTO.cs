using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.MasterData
{
    public class BusinessPartnerDTO
    {
        public int? BusinessPartnerId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int BPTypeId { get; set; }
        public string SIN { get; set; }
        public bool RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string BPTypeCode { get; set; }
    }
}