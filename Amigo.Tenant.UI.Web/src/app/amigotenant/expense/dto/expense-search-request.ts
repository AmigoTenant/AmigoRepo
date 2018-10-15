export class SearchBase{
        public pageSize: number;
        public page: number;
}

export class ExpenseSearchRequest extends SearchBase
{
        public expenseDateFrom : Date;
        public expenseDateTo : Date;
        public paymentTypeId : number | null;
        public houseId : number | null;
        public houseTypeId : number | null;
        public periodId : number | null;
        public referenceNo : string | null;
        public expenseDetailStatusId : number | null;
        public remark : string | null;
        public conceptId : number | null;
        public tenantId : number | null;
}

