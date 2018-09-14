import { Component, OnInit, EventEmitter, Input, Output, OnChanges, SimpleChange } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { UsersClient, AmigoTenantTUserStatusDTO, AmigoTenantTUserDTO, UserSearchRequest } from '../../../shared/api/services.client';
import { UserType, UserTypeList } from '../../../model/user-type.dto';
import { UserPaidBy, UserPaidByList } from '../../../model/user-paid-by.dto';
import { AuthCheckDirective } from  '../../../shared/security/auth-check.directive';
declare var $: any;

@Component({
    selector: 'st-grid-user',
    templateUrl: './grid-user.component.html',
    providers: [UserPaidByList, UserTypeList]
})
export class GridUserComponent implements OnInit {

    @Input() reloadGrid = false;
    @Input() searchUsers = false;
    @Input() paramsSearchUsers = new UserSearchRequest();
    @Output() onClickEditUserGrid = new EventEmitter<boolean>();
    @Output() counterEvent = new EventEmitter<any>();

    ngOnInit() {
        $(document).ready(() => {
            this.resizeGrid();
        });

        $(window).bind('load resize scroll', (e) => {
            this.resizeGrid();
        });
    }

    public buttonCount: number = 20;
    public info: boolean = true;
    public type: 'numeric' | 'input' = 'numeric';
    public pageSizes: any = [20, 50, 100, 200];
    public previousNext: boolean = true;

    public list: GridDataResult;
    public currentPage: number = 0;

    public user = new AmigoTenantTUserStatusDTO();
    public params = new UserSearchRequest();

    public userName: string = '';

    constructor(private userClient: UsersClient, private userTypeList: UserTypeList, private userPaidByList: UserPaidByList) {
        //this.onReloadGrid();
    }

    public pageChange({ skip, take }: PageChangeEvent): void {
        this.currentPage = skip;
        this.params.pageSize = take;
        this.serviceUsers();
    }

    public resizeGrid() {
        var grids = $(".grid-container > .k-grid");
        $.each(grids, (e, grid) => {
            var _combinedPageElementsHeight = 0;
            var _viewportHeight = 0;
            $.each($(grid).parent().parent().siblings().not("kendo-dialog"), (e, v) => {
                _combinedPageElementsHeight += $(v).outerHeight();
            });

            $.each($(grid).find('.k-grid-content').parent().siblings(), (e, v) => {
                _combinedPageElementsHeight += $(v).outerHeight();
            });

            _combinedPageElementsHeight += $(".menu-top").outerHeight();
            _combinedPageElementsHeight += $(".page-header").outerHeight();
            _combinedPageElementsHeight += $(".ro-tab.tabs-top").outerHeight();
            _viewportHeight += $(window).outerHeight() - _combinedPageElementsHeight;
            $(grid).find('.k-grid-content').height(_viewportHeight);
        });
    }

    public serviceUsers(): void {
        this.params.pageSize = +this.params.pageSize;
        this.params.page = (this.currentPage + this.params.pageSize) / this.params.pageSize;
        this.userClient.search(this.params.amigoTenantTUserId, this.params.userName, this.params.firstName, this.params.lastName,
            this.params.dedicatedLocationId, this.params.userType, this.params.amigoTenantTRoleId, this.params.payBy, this.params.page, this.params.pageSize)
            .subscribe(response => {
                let data = response.data;
                //alert(JSON.stringify(data));
                data.items.forEach(user => {
                    for (let _type in this.userTypeList.List) {
                        if (this.userTypeList.List[_type].code === user.userType) {
                            user.userTypeName = this.userTypeList.List[_type].name;
                            break;
                        }
                    }
                    for (let _payBy in this.userPaidByList.List) {
                        if (this.userPaidByList.List[_payBy].code === user.payBy) {
                            user.payByName = this.userPaidByList.List[_payBy].name;
                            break;
                        }
                    }
                });
                this.list = {
                    data: data.items,
                    total: data.total
                };
                this.counterEvent.emit(data.total);
            });
    };

    setSelected(event) {
        this.onClickEditUserGrid.emit(event);
    }

    ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
        if (this.reloadGrid) {
            this.onReloadGrid();
        }
        if (this.searchUsers) {
            this.params = this.paramsSearchUsers;
            this.onReloadGrid();
        }
    }

    onReloadGrid() {
        this.params.pageSize = 20;
        this.currentPage = 0;
        this.serviceUsers();
    }

    deleteUser(user: AmigoTenantTUserDTO) {
        this.user.amigoTenantTUserId = user.amigoTenantTUserId;
        this.user.userName = user.username;
        this.userName = user.username;
        this.opened = true;
    }

    yesDeleteUser() {
        this.user.rowStatus = 0;
        this.userClient.delete(this.user)
            .subscribe(response => {
                this.onReloadGrid();
                this.opened = false;
            });
    }

    public opened: boolean = false;

    public close() {
        this.opened = false;
    }

}
