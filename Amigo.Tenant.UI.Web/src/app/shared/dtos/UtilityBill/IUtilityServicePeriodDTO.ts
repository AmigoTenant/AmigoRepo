export class IUtilityServicePeriodDTO {
    // public servicePeriodId: number;
    public houseServicePeriodId: number;
    public periodId: number;

    public amount: number;
    public adjust: number;
    public consumptionUnmId: number;
    public consumption: number;

    public houseServicePeriodStatusId: number;

    public createdBy: number | null;
    public creationDate: Date | null;
}