import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UserType, UserTypeList } from '../../../model/user-type.dto';
import { UserPaidBy, UserPaidByList } from '../../../model/user-paid-by.dto';
import { UserSearchRequest, AmigoTenantTRoleClient, AmigoTenantTRoleDTO, LocationTypeAheadDTO} from '../../../shared/api/services.client';

@Component({
    selector: 'st-search-user',
    templateUrl: './search-user.component.html',
    providers: [AmigoTenantTRoleClient, UserPaidByList, UserTypeList]
})
export class SearchUserComponent implements OnInit {

    @Output() paramsSearchUsersForm = new EventEmitter<UserSearchRequest>();
    @Output() onAddUser = new EventEmitter<boolean>();
    @Input() isOnlyDriver: boolean = false;

    public typeList: UserType[];
    public paidByList: UserPaidBy[];
    public roleList: AmigoTenantTRoleDTO[];
    public lblAllItems: AmigoTenantTRoleDTO = new AmigoTenantTRoleDTO();
    public user: UserSearchRequest;
    public modelTypeAheadLocation: number = null;
    public currentOriginLocation: any;
    public userRoleId: number = 0;

    ngOnInit() {
        this.user = new UserSearchRequest();
        this.serviceGetRoles();
    }

    constructor(private roleClient: AmigoTenantTRoleClient, private userTypeList: UserTypeList, private userPaidByList: UserPaidByList) {
        let firstEntryDataPaidBy = new UserPaidBy(); //TODO
        firstEntryDataPaidBy.code = ''; firstEntryDataPaidBy.name = 'All';//TODO
        let firstEntryDataUserType = new UserType(); //TODO
        firstEntryDataUserType.code = ''; firstEntryDataUserType.name = 'All';//TODO
        this.typeList = userTypeList.List;
        this.typeList.unshift(firstEntryDataUserType);
        this.paidByList = userPaidByList.List;
        this.paidByList.unshift(firstEntryDataPaidBy);
    }

    public resetForm() {
        this.user.amigoTenantTUserId = 0;
        this.user.userName = '';
        this.user.firstName = '';
        this.user.lastName = '';
        this.user.dedicatedLocationId = 0;
        this.user.userType = '';
        this.user.amigoTenantTRoleId = this.userRoleId;
        this.user.payBy = '';
        this.modelTypeAheadLocation = null;
        this.typeaheadLocationCleanValue();
        this.paramsSearchUsersForm.emit(this.user);
    }

    public serviceGetRoles(): void {
        this.lblAllItems.amigoTenantTRoleId = 0;
        this.lblAllItems.name = 'Loading';
        this.roleList = [];
        this.roleList.unshift(this.lblAllItems);
        this.userRoleId = 0;
        this.roleClient.customRoleSearch(0, '', '', undefined, 1, 1000)
            .subscribe(response => {
                this.lblAllItems.amigoTenantTRoleId = 0;
                this.lblAllItems.name = 'All';
                this.roleList = [];
                if (response.data != undefined && response.data.items != undefined) {
                    var items = response.data.items;
                    if (this.isOnlyDriver) {
                        for (var i = 0, j = items.length; i < j; i++) {
                            if (items[i].code == 'DRIVER') {
                                this.roleList = [items[i]];
                                this.userRoleId = items[i].amigoTenantTRoleId;
                                break;
                            }
                        }
                    } else {
                        this.roleList = items;
                        this.roleList.unshift(this.lblAllItems);
                    }
                } else {
                    this.roleList.unshift(this.lblAllItems);
                }
                //this.roleList.unshift(this.lblAllItems);
                this.resetForm();
            });
    }

    public searchUsers() {
        console.log('acaaa estamos', this.user);
        this.paramsSearchUsersForm.emit(this.user);
    }

    public addUser() {
        this.onAddUser.emit(true);
    }

    public geLocation = (item) => {
        this.user.dedicatedLocationId = undefined;
        if (item != null && item != undefined && item != '') {
            this.user.dedicatedLocationId = item.locationId;
        }
    };

    public typeaheadLocationCleanValue(): void {
        this.currentOriginLocation = new LocationTypeAheadDTO();
        this.currentOriginLocation.locationId = 0;
        this.currentOriginLocation.name = '';
        this.currentOriginLocation.code = '';
    }
}
