export class ExpenseRegisterRequest
{
        public ExpenseId : number;
        public ExpenseDate : Date;
        public PaymentTypeId : number;
        public HouseId : number;
        public PeriodId : number;
        public ReferenceNo : string;
        public Remark : string;
        public SubTotalAmount : number;
        public Tax : number;
        public TotalAmount : number;
}