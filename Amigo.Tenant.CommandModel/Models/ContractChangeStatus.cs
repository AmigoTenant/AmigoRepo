namespace Amigo.Tenant.CommandModel.Models
{
    using Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ContractChangeStatus")]
    public partial class ContractChangeStatus: EntityBase
    {
     //   [ContractChangeStatusId] [int] IDENTITY(1,1) NOT NULL,
     //   [ContractId] [int] NOT NULL,
     //   [ContractStatusId] [int] NOT NULL,
     //   [TenantId] [int] NULL,
	    //[HouseId] [int] NULL,
	    //[Rent] DECIMAL(8,2) NULL,
	    //[Deposit] DECIMAL(8,2) NULL,
	    //[BeginPeriodId] [INT] NULL,
	    //[EndPeriodId] [INT] NULL,
	    //[ContractTermType] [nvarchar] (20) NOT NULL,
     //   [CreatedBy] [int] NOT NULL,
     //   [CreationDate] [datetime2] (7) NOT NULL,
     //   [UpdatedBy] [int] NULL,
	    //[UpdatedDate] [datetime2] (7) NULL

        public int ContractChangeStatusId { get; set; }
        public int ContractId { get; set; }
        public int ContractStatusId { get; set; }
        public int? TenantId { get; set; }
        public int HouseId { get; set; }
        public decimal? Deposit { get; set; }
        public decimal? Rent { get; set; }
        public int? BeginPeriodId { get; set; }
        public int? EndPeriodId { get; set; }
        public string ContractTermType { get; set; }
        public virtual House House { get; set; }

    }
}
