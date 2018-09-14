import { Component, OnInit, ViewChild } from '@angular/core';
import { UserSearchRequest } from '../../../shared/api/services.client';
import { SearchUserComponent } from '../search-user/search-user.component';
import { Audit, AuditModel } from '../../../shared/common/audit.component';

@Component({
    selector: 'app-user',
    templateUrl: './app-user.component.html'
})
export class AppUserComponent implements OnInit {

    public isReloadGrid: boolean = false;
    public isSearchUsers: boolean = false;
    public openDialog: boolean = false;
    public user = new UserSearchRequest();
    public isOnlyDriver: boolean = false;
    public countItems: number = 0;
    public userId: number = 0;
    public userSelected:  any;
    @ViewChild(SearchUserComponent) searchUserComponent: SearchUserComponent;

    ngOnInit() {
        var permissions = JSON.parse(localStorage.getItem("permissions"));
        var action = 'User.Only.Driver';
        var permision = permissions.filter(p => p.ActionCode == action);
        if (permision.length > 0) {
            this.isOnlyDriver = true;
        }
    }

    addUser(event) {
        this.userId = 0;
        this.openDialog = true;
    }

    onClickCloseDialog(refreshGridAfterSaving: boolean) {
        this.openDialog = false;
        if (refreshGridAfterSaving)
            this.searchUserComponent.resetForm();
    }

    onClickEditUserGrid(dataItem) {
        this.userId = dataItem.amigoTenantTUserId;
        this.userSelected = new AuditModel();
        this.userSelected.userId = dataItem.amigoTenantTUserId;
        this.userSelected.createdBy = dataItem.createdBy;
        this.userSelected.creationDate = dataItem.creationDate;
        this.userSelected.updatedBy = dataItem.updatedBy;
        this.userSelected.updatedDate = dataItem.updatedDate;
        this.openDialog = true;
    }

    paramsSearchUsersForm(user: UserSearchRequest) {
        this.user = user;
        this.isSearchUsers = true;
        setTimeout(() => { this.isSearchUsers = false; }, 100);
    }

    counterSearchEvent(counter) {
        this.countItems = counter;
    }
}
