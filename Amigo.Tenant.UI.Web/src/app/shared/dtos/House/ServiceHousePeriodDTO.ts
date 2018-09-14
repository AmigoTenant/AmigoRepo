export class ServiceHousePeriodDTO {
    public serviceHousePeriodId: number;
    public serviceId: number;

    public monthId: number | null;
    public dueDateMonth: number;
    public dueDateDay: number;
    public cutOffMonth: number;
    public cutOffDay: number;
    
    public month: string;
    public periodId: number;

    public rowStatus: boolean | null;
    public createdBy: number | null;
    public creationDate: Date | null;
    public updatedBy: number | null;
    public updatedDate: Date | null;


    init(data?: any) {
        if (data) {
            this.rowStatus = data["RowStatus"] !== undefined ? data["RowStatus"] : <any>null;

            this.serviceId = data["ServiceId"] !== undefined ? data["ServiceId"] : <any>null;
            this.serviceHousePeriodId = data["ServiceHousePeriodId"] !== undefined ? data["ServiceHousePeriodId"] : <any>null;
            this.monthId = data["MonthId"] !== undefined ? data["MonthId"] : <any>null;
            this.dueDateMonth = data["DueDateMonth"] !== undefined ? data["DueDateMonth"] : <any>null;
            
            this.dueDateDay = data["DueDateDay"] !== undefined ? data["DueDateDay"] : <any>null;
            this.cutOffMonth = data["CutOffMonth"] !== undefined ? data["CutOffMonth"] : <any>null;
            this.cutOffDay = data["CutOffDay"] !== undefined ? data["CutOffDay"] : <any>null;
            this.rowStatus = data["RowStatus"] !== undefined ? data["RowStatus"] : <any>null;
                    
            this.createdBy = data["CreatedBy"] !== undefined ? data["CreatedBy"] : <any>null;
            this.creationDate = data["CreationDate"] ? new Date(data["CreationDate"].toString()) : <any>null;
            this.updatedBy = data["UpdatedBy"] !== undefined ? data["UpdatedBy"] : <any>null;
            this.updatedDate = data["UpdatedDate"] ? new Date(data["UpdatedDate"].toString()) : <any>null;
        }
    }
}
