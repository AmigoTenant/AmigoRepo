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
    newHouseId: number | null;
    //Additional Data
    contractId: number | null;
}