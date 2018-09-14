import { IUtilityHouseServiceDTO } from "./IUtilityHouseServiceDTO";

export class UtilityHouseServiceDTO implements IUtilityHouseServiceDTO {
    public houseServiceId: number;
    public houseId: number;
    public serviceId: number;
    public conceptId: number;
    public conceptCode: string;
    public conceptDescription: string;
    public conceptTypeId: number;
    public businessPartnerId: number;
    public businessPartnerName: string;
    public serviceTypeId: number;
    public serviceTypeCode: string;
    public serviceTypeValue: string;
    public rowStatus: boolean;
    public createdBy: number;
    public creationDate: Date;

    constructor(data?: IUtilityHouseServiceDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }    
    init(data?: any) {
        if (data) {
            this.houseId = data["HouseId"] !== undefined ? data["HouseId"] : <any>null;
            this.houseServiceId = data["HouseServiceId"] !== undefined ? data["HouseServiceId"] : <any>null;
            this.serviceId = data["ServiceId"] !== undefined ? data["ServiceId"] : <any>null;
            this.conceptId = data["ConceptId"] !== undefined ? data["ConceptId"] : <any>null;            
            this.conceptCode = data["ConceptCode"] !== undefined ? data["ConceptCode"] : <any>null;
            this.conceptDescription = data["ConceptDescription"] !== undefined ? data["ConceptDescription"] : <any>null;
            this.conceptTypeId = data["ConceptTypeId"] !== undefined ? data["ConceptTypeId"] : <any>null;
            this.businessPartnerId = data["BusinessPartnerId"] !== undefined ? data["BusinessPartnerId"] : <any>null;
            this.businessPartnerName = data["BusinessPartnerName"] !== undefined ? data["BusinessPartnerName"] : <any>null;
            this.serviceTypeId = data["ServiceTypeId"] !== undefined ? data["ServiceTypeId"] : <any>null;
            this.serviceTypeCode = data["ServiceTypeCode"] !== undefined ? data["ServiceTypeCode"] : <any>null;
            this.serviceTypeValue = data["ServiceTypeValue"] !== undefined ? data["ServiceTypeValue"] : <any>null;

            this.rowStatus = data["RowStatus"] !== undefined ? data["RowStatus"] : <any>null;
            this.createdBy = data["CreatedBy"] !== undefined ? data["CreatedBy"] : <any>null;
            this.creationDate = data["CreationDate"] ? new Date(data["CreationDate"].toString()) : <any>null;
        }
    }

    static fromJS(data: any): UtilityHouseServiceDTO {
        let result = new UtilityHouseServiceDTO();
        result.init(data);
        return result;
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;

        data["HouseId"] = this.houseId !== undefined ? this.houseId : null;
        data["HouseServiceId"] = this.houseServiceId !== undefined ? this.houseServiceId : null;
        data["ServiceId"] = this.serviceId !== undefined ? this.serviceId : null;
        data["ConceptId"] = this.conceptId !== undefined ? this.conceptId : null;
    
        data["ConceptCode"] = this.conceptCode !== undefined ? this.conceptCode : null;
        data["ConceptDescription"] = this.conceptDescription !== undefined ? this.conceptDescription : null;
        data["ConceptTypeId"] = this.conceptTypeId !== undefined ? this.conceptTypeId : null;
        data["BusinessPartnerId"] = this.businessPartnerId !== undefined ? this.businessPartnerId : null;
        data["BusinessPartnerName"] = this.businessPartnerName !== undefined ? this.businessPartnerName : null;
        data["ServiceTypeId"] = this.serviceTypeId !== undefined ? this.serviceTypeId : null;
        data["ServiceTypeCode"] = this.serviceTypeCode !== undefined ? this.serviceTypeCode : null;
        data["ServiceTypeValue"] = this.serviceTypeValue !== undefined ? this.serviceTypeValue : null;

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
        let result = new UtilityHouseServiceDTO();
        result.init(json);
        return result;
    }

}