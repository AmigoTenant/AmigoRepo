import { SearchBase } from './expense-search-request';

export class ExpenseDetailDto extends SearchBase {
    expenseDetailId: number;
    conceptId: number;
    remark: string;
    subTotalAmount: number;
    tax: number;
    totalAmount: number;
    applyTo: string;
    tenantId: number;
    expenseId: number;
    expenseDetailStatusId: number;
    conceptName: string;
    applyToName: string;
    tenantFullName: string;
    expenseDetailName: string;
}
