import { UtilityServicePeriodDTO } from "./UtilityServicePeriodDTO";
import { UtilityPeriodDTO } from "./UtilityPeriodDTO";
import { UtilityHouseServiceDTO } from "./UtilityHouseServiceDTO";

export class IUtilityHouseServicePeriodDTO {
    public houseServicePeriodId: number;
    public houseServiceId: number;

    public monthId: number | null;
    public dueDateMonth: number;
    public dueDateDay: number;
    public cutOffMonth: number;
    public cutOffDay: number;

    public period: UtilityPeriodDTO;
    public houseService: UtilityHouseServiceDTO;
    public servicePeriod: UtilityServicePeriodDTO;
    
    public rowStatus: boolean | null;
    public createdBy: number | null;
    public creationDate: Date | null;

}