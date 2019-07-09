export class SearchBase{
        public pageSize: number;
        public page: number;
}

export class ExpenseSearchRequest extends SearchBase
{
        public houseTypeId : number | null;
        public propertyName : string | null;
        public expenseDateFrom : Date;
        public expenseDateTo : Date;
        public periodId : number | null;
        public referenceNo : string | null;
        public totalAmountFrom : number | null;
        public totalAmountTo : number | null;
        public paymentTypeId : number | null;
        public conceptName : string | null;
        public businessPartnerId : number | null;
        public fileName : string | null;
        public tenantFullName: string | null;
}

