import {Component, OnInit, Input, Output, state, ViewChild, EventEmitter} from '@angular/core';
import {Http, Jsonp, URLSearchParams} from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { AmigoTenantTRoleClient, AmigoTenantTRoleSearchRequest,
    AmigoTenantTRoleStatusDTO } from '../../shared/api/services.client';
import { AuthCheckDirective } from  '../../shared/security/auth-check.directive';
import {RoleMaintenanceComponent } from './role.maintenance.component';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { Autofocus } from  '../../shared/directives/autofocus.directive';

declare var $: any;

@Component({
    selector: 'app-role',
    templateUrl: './role.component.html'
})

export class RoleComponent implements OnInit {

    @ViewChild(RoleMaintenanceComponent) viewRoleMaintenanceComponent: RoleMaintenanceComponent;

    constructor(private roleDataService: AmigoTenantTRoleClient) { }

    countItems: number = 0;
    public gridData: GridDataResult;
    public buttonCount: number = 20;
    public info: boolean = true;
    public type: 'numeric' | 'input' = 'numeric';
    public pageSizes: any = [20, 50, 100, 200];
    public previousNext: boolean = true;
    public currentPage: number = 0;
    public skip: number = 0;
    //public  selectedCode: string;
    @Output() selectedData = new EventEmitter<any>();
    flgEdition: string;

    confirmDeletionVisible: boolean = false;
    confirmDeletionResponse: boolean = false;
    confirmDeletionActionCode: string;

    searchCriteria = new AmigoTenantTRoleSearchRequest();
    deleteRol = new AmigoTenantTRoleStatusDTO();


    ngOnInit() {
        $(document).ready(() => { this.resizeGrid(); });
        $(window).bind('load resize scroll', (e) => { this.resizeGrid(); });

        this.searchCriteria.pageSize = 20;
        this.currentPage = 0;

        this.onSearch();
    }

    typeListAdmin: TypeAdmin[] = [
        { code: null, name: 'All' },
        { code: true, name: 'Yes' },
        { code: false, name: 'No' }
    ];

    onChangeType(selectedValue) {
        this.searchCriteria.isAdmin = selectedValue;
    }

    onSearch(): void {
        this.searchCriteria.pageSize = +this.searchCriteria.pageSize;
        this.searchCriteria.page = (this.currentPage + this.searchCriteria.pageSize) / this.searchCriteria.pageSize;
        this.roleDataService.search(
            this.searchCriteria.amigoTenantTRoleId = 0,
            this.searchCriteria.name,
            this.searchCriteria.code,
            this.searchCriteria.isAdmin,
            this.searchCriteria.page,
            this.searchCriteria.pageSize
        )
            .subscribe(res => {

                var dataResult: any = res;
                this.countItems = dataResult.data.total;
                this.gridData = {
                    data: dataResult.data.items,
                    total: dataResult.data.total
                }

            });

    }

    onNew(): void {
        this.viewRoleMaintenanceComponent.onNew();
    }

    deleteFilters(): void {
        this.searchCriteria = new AmigoTenantTRoleSearchRequest();
        this.searchCriteria.pageSize = 20; this.currentPage = 0;
        $(window).resize();
        this.onSearch();
    }

    // onEdit(data):void{

    //  //   this.selectedCode = data;

    //     this.selectedData.emit(data);
    // }

    onDelete(code): void {
        this.confirmDeletionVisible = true;
        this.deleteRol.amigoTenantTRoleId = code;
        this.deleteRol.rowStatus = 0;
    }

    openConfirmation() {
        this.confirmDeletionVisible = true;
    }

    onConfirmation(status): void {

        this.confirmDeletionResponse = (status === "YES");

        if (this.confirmDeletionResponse) {
            this.roleDataService.delete(this.deleteRol)
                .subscribe(res => {
                    var dataResult: any = res;
                    this.onSearch();
                });


        }

        this.confirmDeletionVisible = false;
    }

    closeConfirmation(status) {
        this.confirmDeletionVisible = false;
    }

    onReloadGrid() {
        this.searchCriteria.pageSize = 20;
        this.currentPage = 0;
        this.onSearch();
    }
    public pageChange({ skip, take }: PageChangeEvent): void {
        this.currentPage = skip;
        this.searchCriteria.pageSize = take;
        this.onSearch();
    }


    public resizeGrid() {
        var grids = $(".grid-container > .k-grid");
        $.each(grids, (e, grid) => {
            var _combinedPageElementsHeight = 0;
            var _viewportHeight = 0;
            $.each($(grid).parent().siblings().not("kendo-dialog"), (e, v) => {
                _combinedPageElementsHeight += $(v).outerHeight();
            });

            $.each($(grid).find('.k-grid-content').parent().siblings(), (e, v) => {
                _combinedPageElementsHeight += $(v).outerHeight();
            });

            _combinedPageElementsHeight += $(".menu-top").outerHeight();
            _combinedPageElementsHeight += $(".page-header").outerHeight();
            _combinedPageElementsHeight += $(".ro-tab.tabs-top").outerHeight();
            _combinedPageElementsHeight += (!$("st-rol-maintenance").length ? 0 : $("st-rol-maintenance").outerHeight());
            _viewportHeight += $(window).outerHeight() - _combinedPageElementsHeight;
            $(grid).find('.k-grid-content').height(_viewportHeight);
        });
    }



    //  public selectionChange(event: any): void {
    //         let dataItem = this.gridData.data[event.index - this.skip];
    //        this.selectedCode = dataItem;
    //     }

}

export class TypeAdmin {
    code: boolean;
    name: string;
}
