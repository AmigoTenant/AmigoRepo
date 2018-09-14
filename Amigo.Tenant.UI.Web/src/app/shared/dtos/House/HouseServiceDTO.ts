import { IHouseServiceDTO } from "./IHouseServiceDTO";
import { HouseServicePeriodDTO } from "./HouseServicePeriodDTO";


export class HouseServiceDTO implements IHouseServiceDTO {
    houseServiceId: number | null;
    houseId: number | null;
    serviceId: number | null;

    conceptDescription  :string | null; 
    businessPartnerName :string | null; 
    serviceTypeValue: string | null;

    rowStatus: boolean | null;
    createdBy: number | null;
    creationDate: Date | null;
    updatedBy: number | null;
    updatedDate: Date | null;

    checked: boolean | null;
    userId: number | null;
    isNew: boolean | null;

    houseServicePeriods: HouseServicePeriodDTO[];    

    constructor(data?: IHouseServiceDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.houseServiceId = data["HouseServiceId"] !== undefined ? data["HouseServiceId"] : <any>null;
            this.houseId = data["HouseId"] !== undefined ? data["HouseId"] : <any>null;
            this.serviceId = data["ServiceId"] !== undefined ? data["ServiceId"] : <any>null;

            this.serviceTypeValue = data["ServiceTypeValue"] !== undefined ? data["ServiceTypeValue"] : <any>null;
            this.businessPartnerName = data["BusinessPartnerName"] !== undefined ? data["BusinessPartnerName"] : <any>null;
            this.conceptDescription = data["ConceptDescription"] !== undefined ? data["ConceptDescription"] : <any>null;
            
            this.rowStatus = data["RowStatus"] !== undefined ? data["RowStatus"] : <any>null;
            this.createdBy = data["CreatedBy"] !== undefined ? data["CreatedBy"] : <any>null;
            this.creationDate = data["CreationDate"] ? new Date(data["CreationDate"].toString()) : <any>null;
            this.updatedBy = data["UpdatedBy"] !== undefined ? data["UpdatedBy"] : <any>null;
            this.updatedDate = data["UpdatedDate"] ? new Date(data["UpdatedDate"].toString()) : <any>null;

            this.checked = data["Checked"] !== undefined ? data["Checked"] : <any>null;

            this.houseServicePeriods = data["HouseServicePeriods"] !== undefined ? data["HouseServicePeriods"] : new Array<HouseServicePeriodDTO>();
        }
    }

    static fromJS(data: any): HouseServiceDTO {
        let result = new HouseServiceDTO();
        result.init(data);
        return result;
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;

        data["HouseServiceId"] = this.houseServiceId !== undefined ? this.houseServiceId : null;
        data["HouseId"] = this.houseId !== undefined ? this.houseId : null;
        data["ServiceId"] = this.serviceId !== undefined ? this.serviceId : null;
    
        data["ServiceTypeValue"] = this.serviceTypeValue !== undefined ? this.serviceTypeValue : null;
        data["ConceptDescription"] = this.conceptDescription !== undefined ? this.conceptDescription : null;
        data["BusinessPartnerName"] = this.businessPartnerName !== undefined ? this.businessPartnerName : null;
        
        data["RowStatus"] = this.rowStatus !== undefined ? this.rowStatus : null;
        data["CreatedBy"] = this.createdBy !== undefined ? this.createdBy : null;
        data["CreationDate"] = this.creationDate ? this.creationDate.toISOString() : null;
        data["UpdatedBy"] = this.updatedBy !== undefined ? this.updatedBy : null;
        data["UpdatedDate"] = this.updatedDate ? this.updatedDate.toISOString() : null;

        data["Checked"] = this.checked !== undefined ? this.checked : null;

        data["HouseServicePeriods"] = this.houseServicePeriods !== undefined ? this.houseServicePeriods : null;

        return data;
    }

    toJSON(data?: any) {
        return JSON.stringify(this.toJS());
    }

    clone() {
        const json = this.toJSON();
        let result = new HouseServiceDTO();
        result.init(json);
        return result;
    }
}
