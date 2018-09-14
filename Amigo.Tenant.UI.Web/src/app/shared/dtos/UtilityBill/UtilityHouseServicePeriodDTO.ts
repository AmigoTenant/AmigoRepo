import { UtilityServicePeriodDTO } from "./UtilityServicePeriodDTO";
import { IUtilityHouseServicePeriodDTO } from "./IUtilityHouseServicePeriodDTO";
import { UtilityPeriodDTO } from "./UtilityPeriodDTO";
import { UtilityHouseServiceDTO } from "./UtilityHouseServiceDTO";

export class UtilityHouseServicePeriodDTO implements IUtilityHouseServicePeriodDTO {
    public houseServicePeriodId: number;
    public houseServiceId: number;
    public monthId: number | null;
    public dueDateMonth: number;
    public dueDateDay: number;
    public cutOffMonth: number;
    public cutOffDay: number;
    public periodId: number;

    public period: UtilityPeriodDTO;
    public houseService: UtilityHouseServiceDTO;
    public servicePeriod: UtilityServicePeriodDTO;

    public rowStatus: boolean | null;
    public createdBy: number | null;
    public creationDate: Date | null;

    constructor(data?: IUtilityHouseServicePeriodDTO) {
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
            this.houseServiceId = data["HouseServiceId"] !== undefined ? data["HouseServiceId"] : <any>null;
            this.monthId = data["MonthId"] !== undefined ? data["MonthId"] : <any>null;
            this.dueDateMonth = data["DueDateMonth"] !== undefined ? data["DueDateMonth"] : <any>null;            
            this.dueDateDay = data["DueDateDay"] !== undefined ? data["DueDateDay"] : <any>null;
            this.cutOffMonth = data["CutOffMonth"] !== undefined ? data["CutOffMonth"] : <any>null;
            this.cutOffDay = data["CutOffDay"] !== undefined ? data["CutOffDay"] : <any>null;
            this.periodId = data["PeriodId"] !== undefined ? data["PeriodId"] : <any>null;

            //this.period = data["Period"] !== undefined ? data["Period"] : <any>null;
            this.period = UtilityPeriodDTO.fromJS(data["Period"]);

            //this.houseService = data["HouseService"] !== undefined ? data["HouseService"] : <any>null;
            this.houseService = UtilityHouseServiceDTO.fromJS(data["HouseService"]);

            // this.servicePeriod = data["ServicePeriod"] !== undefined ? data["ServicePeriod"] : <any>null;
            this.servicePeriod = UtilityServicePeriodDTO.fromJS(data["ServicePeriod"]);

            this.rowStatus = data["RowStatus"] !== undefined ? data["RowStatus"] : <any>null;
            this.createdBy = data["CreatedBy"] !== undefined ? data["CreatedBy"] : <any>null;
            this.creationDate = data["CreationDate"] ? new Date(data["CreationDate"].toString()) : <any>null;
        }
    }

    static fromJS(data: any): UtilityHouseServicePeriodDTO {
        let result = new UtilityHouseServicePeriodDTO();
        result.init(data);
        return result;
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;

        data["HouseServicePeriodId"] = this.houseServicePeriodId !== undefined ? this.houseServicePeriodId : null;
        data["HouseServiceId"] = this.houseServiceId !== undefined ? this.houseServiceId : null;
        data["MonthId"] = this.monthId !== undefined ? this.monthId : null;
        data["DueDateDay"] = this.dueDateDay !== undefined ? this.dueDateDay : null;
        data["DueDateMonth"] = this.dueDateMonth !== undefined ? this.dueDateMonth : null;
        data["CutOffDay"] = this.cutOffDay !== undefined ? this.cutOffDay : null;
        data["CutOffMonth"] = this.cutOffMonth !== undefined ? this.cutOffMonth : null;
    
        data["RowStatus"] = this.rowStatus !== undefined ? this.rowStatus : null;
        data["CreatedBy"] = this.createdBy !== undefined ? this.createdBy : null;
        data["CreationDate"] = this.creationDate ? this.creationDate.toISOString() : null;

        data["ServicePeriod"] = this.servicePeriod !== undefined ? this.servicePeriod : null;
        data["Period"] = this.period !== undefined ? this.period : null;
        data["HouseService"] = this.houseService !== undefined ? this.houseService : null;

        return data;
    }

    toJSON(data?: any) {
        return JSON.stringify(this.toJS());
    }

    clone() {
        const json = this.toJSON();
        let result = new UtilityHouseServicePeriodDTO();
        result.init(json);
        return result;
    }
}
