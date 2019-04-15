export class ExpenseRegisterRequest {
        public expenseId: number;
        public expenseDate: Date;
        public paymentTypeId: number;
        public houseId: number;
        public periodId: number;
        public referenceNo: string;
        public remark: string;
        public subTotalAmount: number;
        public tax: number;
        public totalAmount: number;
        public conceptId: number;
}
