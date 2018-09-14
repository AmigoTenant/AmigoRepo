import { ServiceHousePeriodDTO } from "./ServiceHousePeriodDTO";

export interface IServiceHouseDTO {
    // houseServiceId: number | null;
    // houseId: number | null;
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

    serviceHousePeriods: ServiceHousePeriodDTO[];    
}