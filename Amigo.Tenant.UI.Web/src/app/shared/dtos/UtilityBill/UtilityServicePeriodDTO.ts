import { IUtilityServicePeriodDTO } from "./IUtilityServicePeriodDTO";

export class UtilityServicePeriodDTO implements IUtilityServicePeriodDTO {
    public houseServicePeriodId: number;
    public periodId: number;

    public amount: number;
    public adjust: number;
    public consumptionUnmId: number;
    public consumption: number;

    public houseServicePeriodStatusId: number;

    public rowStatus: boolean | null;
    public createdBy: number | null;
    public creationDate: Date | null;

    constructor(data?: IUtilityServicePeriodDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.houseServicePeriodId = data["HouseServicePeriodId"] !== undefined ? data["HouseServicePeriodId"] : <any>null;
            this.periodId = data["PeriodId"] !== undefined ? data["PeriodId"] : <any>null;

            this.amount = data["Amount"] !== undefined ? data["Amount"] : <any>null;
            this.adjust = data["Adjust"] !== undefined ? data["Adjust"] : <any>null;
            this.consumptionUnmId = data["ConsumptionUnmId"] !== undefined ? data["ConsumptionUnmId"] : <any>null;
            this.consumption = data["Consumption"] !== undefined ? data["Consumption"] : <any>null;
            this.houseServicePeriodStatusId = data["HouseServicePeriodStatusId"] !== undefined ? data["HouseServicePeriodStatusId"] : <any>null;
            
            this.rowStatus = data["RowStatus"] !== undefined ? data["RowStatus"] : <any>null;
            this.createdBy = data["CreatedBy"] !== undefined ? data["CreatedBy"] : <any>null;
            this.creationDate = data["CreationDate"] ? new Date(data["CreationDate"].toString()) : <any>null;
        }
    }

    static fromJS(data: any): UtilityServicePeriodDTO {
        let result = new UtilityServicePeriodDTO();
        result.init(data);
        return result;
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["PeriodId"] = this.periodId !== undefined ? this.periodId : null;
        data["HouseServicePeriodId"] = this.houseServicePeriodId !== undefined ? this.houseServicePeriodId : null;
        
        data["Amount"] = this.amount !== undefined ? this.amount : null;
        data["Adjust"] = this.adjust !== undefined ? this.adjust : null;
        data["ConsumptionUnmId"] = this.consumptionUnmId !== undefined ? this.consumptionUnmId : null;
        data["Consumption"] = this.consumption !== undefined ? this.consumption : null;

        data["HouseServicePeriodStatusId"] = this.houseServicePeriodStatusId !== undefined ? this.houseServicePeriodStatusId : null;
        
        data["RowStatus"] = this.rowStatus !== undefined ? this.rowStatus : null;
        data["CreatedBy"] = this.createdBy !== undefined ? this.createdBy : null;
        data["CreationDate"] = this.creationDate ? this.creationDate.toISOString() : null;

        return data;
    }

    toJSON(data?: any) {
        return JSON.stringify(this.toJS());
    }

    clone() {
        const json = this.toJSON();
        let result = new UtilityServicePeriodDTO();
        result.init(json);
        return result;
    }
}