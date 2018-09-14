import { HouseServicePeriodDTO } from "./HouseServicePeriodDTO";

export interface IHouseServiceDTO {
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

    houseServicePeriods: HouseServicePeriodDTO[];    
}