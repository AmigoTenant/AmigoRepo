import { MainMenuDTO } from '../../api/services.client';

export class MenuDTO {
    userId: number;
    roleId: number;
    permissionId: number;
    actionId: number;
    moduleId: number;
    moduleName: string;
    parentModuleId: number;
    parentModuleName: string;
    url: string;
    show: boolean;
    childMenu: MainMenuDTO[];
    parentSortOrder: number;
    parentModuleCode: string;
    sortOrder: number;
    moduleCode: string;

}