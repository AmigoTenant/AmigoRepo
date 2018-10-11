export class SearchBase{
        public pageSize: number;
        public page: number;
}

export class ExpenseSearchRequest extends SearchBase
{
        public propertyTypeId : number | null;
        public periodId : number | null;
        public expenseDateFrom : Date;
        public expenseDateTo : Date;
        public houseId : number | null;
        public paymentTypeId : number | null;
        public referenceNo : string | null;
        public statusId : number | null;
        public conceptId : number | null;
        public description : string | null;
        public tenantId : number | null;
}

