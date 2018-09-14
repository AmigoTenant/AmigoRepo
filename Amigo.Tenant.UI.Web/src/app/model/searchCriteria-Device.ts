import { PageListCommon } from './pageListCommon';

export class SearchCriteriaDevice extends PageListCommon {

    public deviceId: number;
    public identifier: string;
    public wIFIMAC: string;
    public cellphoneNumber: string;
    public oSVersionId: number;
    public platformId: number;
    public appVersionId: number;
    public modelId: number;
    public brandId: number; 
    public isAutoDateTime: boolean;
    public isSpoofingGPS: boolean;
    public isRootedJailbreaked: boolean; 
    public assignedAmigoTenantTUserId: number;
    public rowStatus: boolean;
    public page: number;
    public pageSize: number;

    constructor() {
        super();
    }
}
