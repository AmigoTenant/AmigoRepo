using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.MasterData
{
    public class EntityStatusDTO
    {
        public int? EntityStatusId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string EntityCode { get; set; }
        public int? Sequence { get; set; }
        //public bool RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}