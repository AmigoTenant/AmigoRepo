import { PageListCommon } from '../../../model/pageListCommon'

export class SearchCriteriaUser extends PageListCommon {
    public amigoTenantTUserId: number;
    public userName: string;
    public firstName: string;
    public lastName: string;
    public dedicatedLocationId: number;
    public userType: string;
    public amigoTenantTRoleId: number;
    public payBy: string;

    constructor() {
        super();
    }
}
