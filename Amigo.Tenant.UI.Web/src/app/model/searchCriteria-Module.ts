import { PageListCommon } from './pageListCommon';

// Extens Class searchCriteria 
export class SearchCriteriaModule extends PageListCommon {
    public  code:string;
    public name: string;
    public parentName: string;
    public  onlyParents : Boolean;

    constructor() {
        // execute Construtor Father
        super();
    }
}
