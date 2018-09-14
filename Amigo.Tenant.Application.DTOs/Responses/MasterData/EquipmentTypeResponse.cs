using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.MasterData
{
    public class EquipmentTypeResponse: IDetailedResponse<EquipmentTypeDetails>
    {        
        public string Name { get; set; }
        public string Code { get; set; }        
        public bool IncludeDetails { get; set; }
        public EquipmentTypeDetails Details { get; set; }
    }

    public class EquipmentTypeDetails: IAuditableDetails
    {
        public int EquipmentTypeId { get; set; }
        public bool RowStatus { get; set; }        
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public interface IDetailedResponse<T>
    {
        bool IncludeDetails { get; set; }
        T Details { get; set; }
    }

    public interface IAuditableDetails
    {
        bool RowStatus { get; set; }
        int CreatedBy { get; set; }
        DateTime CreationDate { get; set; }
        int? UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
    }

    public class ServiceResponse<T>
    {
        public bool IsCorrect { get; set; }
        public T Data { get; set; }
        public List<String> Messages { get; set; }
        public bool IsPaged { get; set; }
        public PagingDetails PagingDetails { get; set; }
    }

    public class PagingDetails
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
    }
}
