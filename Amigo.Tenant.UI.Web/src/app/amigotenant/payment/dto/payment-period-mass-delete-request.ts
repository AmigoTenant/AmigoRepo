export class PaymentPeriodMassDeleteRequest
{
    paymentPeriodId: number;
    periodId: number;
    contractId: number;

    constructor(contractId: number, periodId: number){
        this.contractId = contractId;
        this.periodId = periodId;
    }
}