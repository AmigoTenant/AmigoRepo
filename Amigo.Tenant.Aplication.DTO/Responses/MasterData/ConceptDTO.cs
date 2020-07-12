using System;

namespace Amigo.Tenant.Application.DTOs.Responses.MasterData
{
    public class ConceptDTO: IEntity
    {
        public int? ConceptId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? TypeId { get; set; }
        public string Remark { get; set; }
        public decimal? ConceptAmount { get; set; }
        public int? PayTypeId { get; set; }
        public bool RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TypeCode { get; set; }
    }
}