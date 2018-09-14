using System;


namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class ProductDTO: IEntity
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string IsHazardous { get; set; }
        public bool RowStatus { get; set; }
    }
}
