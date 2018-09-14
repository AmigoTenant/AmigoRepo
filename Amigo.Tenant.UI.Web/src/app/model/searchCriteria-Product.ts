import { PageListCommon } from './pageListCommon';

export class SearchCriteriaProduct extends PageListCommon {

    public productId: number;
    public code: string;
    public name: string;
    public shortName: string;
    public isHazardous: string;
    public rowStatus: boolean;
    public page: number;
    public pageSize: number;

    constructor() {
        super();
    }
}
