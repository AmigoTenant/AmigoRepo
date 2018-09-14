import { IServiceHouseDTO } from "./IServiceHouseDTO";
import { ServiceHousePeriodDTO } from "./ServiceHousePeriodDTO";


export class ServiceHouseDTO implements IServiceHouseDTO {
    rowStatus: boolean | null;
    
    serviceId: number | null;
    conceptId : number | null;
    conceptCode         :string | null; 
    conceptDescription  :string | null; 
    conceptTypeId      :string | null; 

    businessPartnerId : number | null;
    businessPartnerName :string | null; 

    serviceTypeId : number | null;
    serviceTypeCode: string | null;
    serviceTypeValue: string | null;
    
    createdBy: number | null;
    creationDate: Date | null;
    updatedBy: number | null;
    updatedDate: Date | null;

    checked: boolean | null;
    userId: number | null;
    isNew: boolean | null;

    serviceHousePeriods: ServiceHousePeriodDTO[];    

    constructor(data?: IServiceHouseDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.rowStatus = data["RowStatus"] !== undefined ? data["RowStatus"] : <any>null;

            this.serviceId = data["ServiceId"] !== undefined ? data["ServiceId"] : <any>null;
            this.serviceTypeId = data["ServiceTypeId"] !== undefined ? data["ServiceTypeId"] : <any>null;
            this.serviceTypeValue = data["ServiceTypeValue"] !== undefined ? data["ServiceTypeValue"] : <any>null;
            this.serviceTypeCode = data["ServiceTypeCode"] !== undefined ? data["ServiceTypeCode"] : <any>null;
            
            this.conceptId = data["ConceptId"] !== undefined ? data["ConceptId"] : <any>null;
            this.conceptCode = data["ConceptCode"] !== undefined ? data["ConceptCode"] : <any>null;
            this.conceptDescription = data["ConceptDescription"] !== undefined ? data["ConceptDescription"] : <any>null;
            this.conceptTypeId      = data["ConceptTypeId"] !== undefined ? data["ConceptTypeId"] : <any>null;
            this.businessPartnerName = data["BusinessPartnerName"] !== undefined ? data["BusinessPartnerName"] : <any>null;
            this.businessPartnerId = data["BusinessPartnerId"] !== undefined ? data["BusinessPartnerId"] : <any>null;
                    
            this.createdBy = data["CreatedBy"] !== undefined ? data["CreatedBy"] : <any>null;
            this.creationDate = data["CreationDate"] ? new Date(data["CreationDate"].toString()) : <any>null;
            this.updatedBy = data["UpdatedBy"] !== undefined ? data["UpdatedBy"] : <any>null;
            this.updatedDate = data["UpdatedDate"] ? new Date(data["UpdatedDate"].toString()) : <any>null;

            this.checked = data["Checked"] !== undefined ? data["Checked"] : <any>null;

            this.serviceHousePeriods = new Array<ServiceHousePeriodDTO>();
            data["ServiceHousePeriods"].forEach((value, index) => {
                let period = new ServiceHousePeriodDTO();
                if(value) {  // !== undefined && value !== null) {
                    period.init(value);
                    this.serviceHousePeriods.push(period);
                }
            }); 
        }
    }

    static fromJS(data: any): ServiceHouseDTO {
        let result = new ServiceHouseDTO();
        result.init(data);
        return result;
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["RowStatus"] = this.rowStatus !== undefined ? this.rowStatus : null;

        data["ServiceId"] = this.serviceId !== undefined ? this.serviceId : null;
        data["ServiceTypeId"] = this.serviceTypeId !== undefined ? this.serviceTypeId : null;
        data["ServiceTypeValue"] = this.serviceTypeValue !== undefined ? this.serviceTypeValue : null;
        data["ServiceTypeCode"] = this.serviceTypeCode !== undefined ? this.serviceTypeCode : null;
    
        data["ConceptId"] = this.conceptId !== undefined ? this.conceptId : null;
        data["ConceptCode"] = this.conceptCode !== undefined ? this.conceptCode : null;
        data["ConceptDescription"] = this.conceptDescription !== undefined ? this.conceptDescription : null;
        data["ConceptTypeId"] = this.conceptTypeId !== undefined ? this.conceptTypeId : null;
        data["BusinessPartnerName"] = this.businessPartnerName !== undefined ? this.businessPartnerName : null;
        data["BusinessPartnerId"] = this.businessPartnerId !== undefined ? this.businessPartnerId : null;

        data["CreatedBy"] = this.createdBy !== undefined ? this.createdBy : null;
        data["CreationDate"] = this.creationDate ? this.creationDate.toISOString() : null;
        data["UpdatedBy"] = this.updatedBy !== undefined ? this.updatedBy : null;
        data["UpdatedDate"] = this.updatedDate ? this.updatedDate.toISOString() : null;

        data["Checked"] = this.checked !== undefined ? this.checked : null;

        //debugger;
        data["ServiceHousePeriods"] = this.serviceHousePeriods !== undefined ? this.serviceHousePeriods : null;

        return data;
    }

    toJSON(data?: any) {
        return JSON.stringify(this.toJS());
    }

    clone() {
        const json = this.toJSON();
        let result = new ServiceHouseDTO();
        result.init(json);
        return result;
    }
}
