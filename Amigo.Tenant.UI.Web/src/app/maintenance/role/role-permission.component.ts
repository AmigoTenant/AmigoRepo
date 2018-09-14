import { Component, Input } from '@angular/core';
import { TreeModule } from 'angular-tree-component';
import { AmigoTenantTRoleClient, AmigoTenanttRolPermissionRequest, ResponseDTO } from '../../shared/api/services.client';


@Component({
    selector: 'st-action-permission',
    templateUrl: './role-permission.component.html'
})

export class RolPermissionComponent {

    constructor(private roleDataService: AmigoTenantTRoleClient) { }

    @Input() inputMuduleTree = [];
    @Input() inputCodeRol: any;

    private handleResult = (res: ResponseDTO) => {
        if (res.isValid == false) {
            alert('A error ocurred trying to update permissions.');
        }
    };

    clicChecked = (event, item) => {
        console.log('permissions:');
        console.log(item);
        let state = item.enabled === true;
        let action = new AmigoTenanttRolPermissionRequest();
        action.actionId = item.actionId;
        action.codeAction = item.code;
        action.codeRol = this.inputCodeRol;
        if (state === true) {
            action.entityStatus = 1;
        } else {
            action.entityStatus = 3;
        }

        this.roleDataService.updateActions(action).subscribe(this.handleResult);
    }
}
