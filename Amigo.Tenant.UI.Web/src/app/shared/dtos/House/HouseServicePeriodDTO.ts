export class HouseServicePeriodDTO {
    public houseServicePeriodId: number;
    public houseServiceId: number;

    public monthId: number | null;
    public dueDateMonth: number;
    public dueDateDay: number;
    public cutOffMonth: number;
    public cutOffDay: number;
    
    public month: string;

    public rowStatus: boolean | null;
    public createdBy: number | null;
    public creationDate: Date | null;
    public updatedBy: number | null;
    public updatedDate: Date | null;
}
