import { IUtilityPeriodDTO } from "./IUtilityPeriodDTO";

export class UtilityPeriodDTO implements IUtilityPeriodDTO {
    public periodId: number;
    public code: string;
    public beginDate: Date;
    public endDate: Date;
    public dueDate: Date;
    public sequence: number;
    public rowStatus: boolean;

    constructor(data?: IUtilityPeriodDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.periodId = data["PeriodId"] !== undefined ? data["PeriodId"] : <any>null;
            this.code = data["Code"] !== undefined ? data["Code"] : <any>null;
            this.beginDate = data["BeginDate"] ? new Date(data["BeginDate"].toString()) : <any>null;
            this.endDate = data["EndDate"] ? new Date(data["EndDate"].toString()) : <any>null;
            this.dueDate = data["DueDate"] ? new Date(data["DueDate"].toString()) : <any>null;
            this.sequence = data["Sequence"] !== undefined ? data["Sequence"] : <any>null;            
            this.rowStatus = data["RowStatus"] !== undefined ? data["RowStatus"] : <any>null;
        }
    }

    static fromJS(data: any): UtilityPeriodDTO {
        let result = new UtilityPeriodDTO();
        result.init(data);
        return result;
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["PeriodId"] = this.periodId !== undefined ? this.periodId : null;
        data["Code"] = this.code !== undefined ? this.code : null;
        
        data["BeginDate"] = this.beginDate ? this.beginDate.toISOString() : null;
        data["EndDate"] = this.endDate ? this.endDate.toISOString() : null;
        data["DueDate"] = this.dueDate ? this.dueDate.toISOString() : null;
        data["Sequence"] = this.sequence !== undefined ? this.sequence : null;

        data["RowStatus"] = this.rowStatus !== undefined ? this.rowStatus : null;

        return data;
    }

    toJSON(data?: any) {
        return JSON.stringify(this.toJS());
    }

    clone() {
        const json = this.toJSON();
        let result = new UtilityPeriodDTO();
        result.init(json);
        return result;
    }    
}
