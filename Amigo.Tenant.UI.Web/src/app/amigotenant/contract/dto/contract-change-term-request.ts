export class ContractChangeTermRequest{
    contractTermType: string | null;
    //Extension
    finalPeriodId: number | null;
    //Modify
    fromPeriodId: number | null;
    //Common
    newTenantId: number | null;
    newDeposit: number;
    newRent: number;
    //Additional Data
    contractId: number | null;
    tenantId: number | null;
    houseId: number | null;
}