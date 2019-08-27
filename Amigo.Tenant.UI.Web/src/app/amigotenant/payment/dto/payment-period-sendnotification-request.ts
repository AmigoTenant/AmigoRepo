export class PaymentPeriodSendNotificationRequest
{
    contractId: number | null;
    periodId: number | null;
    periodCode: string | null;

    constructor(contractId: number, periodId: number, periodCode: string){
        this.periodCode = periodCode;
        this.contractId = contractId;
        this.periodId = periodId;
    }
}