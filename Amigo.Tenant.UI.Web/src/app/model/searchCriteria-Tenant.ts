import { PageListCommon } from './pageListCommon';

export class SearchCriteriaTenant extends PageListCommon {

    public tenantId: number;

    public statusId: number;
    public countryId: number;
    public typeId: number;

    public code: string;
    public fullName: string;
    public phoneN01: string;

    public rowStatus: boolean;
    public page: number;
    public pageSize: number;
    
    public phoneNumber: string;

    constructor() {
        super();
    }
}