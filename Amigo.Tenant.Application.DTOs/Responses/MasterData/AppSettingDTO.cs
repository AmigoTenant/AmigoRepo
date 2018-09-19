using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.MasterData
{
    public class AppSettingDTO
    {
        public int? AppSettingId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string AppSettingValue { get; set; }
        public bool RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}