export class ExpenseEditRequest
{
        public expenseId : number;
        public expenseDate : Date;
        public paymentTypeId : number;
        public conceptId: number;
        public houseId : number;
        public periodId : number;
        public referenceNo : string;
        public remark : string;
        public subTotalAmount : number;
        public tax : number;
        public totalAmount : number;
}