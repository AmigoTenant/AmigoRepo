import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { Http, Jsonp, URLSearchParams } from '@angular/http';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { HouseClient, HouseSearchRequest, HouseDTO, DeleteHouseRequest } from '../../shared/api/services.client';
import { HouseMaintenanceComponent } from './house-maintenance.component';
import { AuthCheckDirective } from  '../../shared/security/auth-check.directive';
import { Autofocus } from  '../../shared/directives/autofocus.directive';

import { Router,ActivatedRoute } from '@angular/router';
import { DataService } from './dataService';
import { Observable, Subscription } from 'rxjs'
import { ListsService } from '../../shared/constants/lists.service';

declare var $: any;

@Component({
    selector: 'st-house',
    templateUrl: './house.component.html'
})
export class HouseComponent implements OnInit {

    constructor(private router: Router, private route: ActivatedRoute,
        private houseDataService: HouseClient, public dataservice: DataService,
        private _listsService: ListsService) { }

    //@ViewChild(HouseMaintenanceComponent) viewHouseComponent: HouseMaintenanceComponent;

    public gridData: GridDataResult;
    public skip: number = 0;
    public listHouseTypes = [];
    public listHouseStatuses = [];
    public SelectedCode: string;
    public buttonCount: number = 20;
    public info: boolean = true;
    public type: 'numeric' | 'input' = 'numeric';
    public pageSizes: any = [20, 50, 100, 200];
    public previousNext: boolean = true;
    public currentPage: number = 0;
    public flgMantenance: boolean = true;

    confirmDeletionVisible: boolean = false;
    confirmDeletionResponse: boolean = false;
    confirmDeletionActionCode: string;
    countItems: number = 0;
    visible: boolean = true;

    public activeInactiveStatus: any[] = this._listsService.ActiveInactiveStatus();

    @Output() open: EventEmitter<any> = new EventEmitter();
    @Output() close: EventEmitter<any> = new EventEmitter();

    searchCriteria = new HouseSearchRequest();
    deleteHouse = new DeleteHouseRequest();
    sub: Subscription;

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    ngOnInit() {
        this.searchCriteria.pageSize = 40;
        this.currentPage = 0;

        // this.sub = this.route.params.subscribe(params => {
        //     setTimeout(() => {
        //             this.onSearch();
        //         }, 500);
        // });

        this.houseDataService.getHouseTypes()
            .subscribe(res => {
                var dataResult: any = res;
                this.listHouseTypes = dataResult.data;

                this.houseDataService.getHouseStatuses()
                    .subscribe(res => {
                        var dataResult: any = res;
                        this.listHouseStatuses = dataResult.data;

                        this.sub = this.route.params.subscribe(params => {
                            this.onSearch();
                        });
                    });
            });

        // this.houseDataService.getCities()
        //     .subscribe(res => {
        //         var dataResult: any = res;
        //         this.listStatesCities = dataResult.data;
        //         this.listCountries = this.getCountries(dataResult.data);
        //     });

        $(document).ready(() => { this.resizeGrid(); });
        $(window).bind('load resize scroll', (e) => { this.resizeGrid(); });
    }

    containsDuplicates = function (v, data) {
        var count = data.length;
        for (var i = 0; i < count; i++) {
            if (data[i].countryISOCode === v) return true;
        }
        return false;
    }

    onSearch() {
        this.searchCriteria.pageSize = +this.searchCriteria.pageSize;
        this.searchCriteria.page = (this.currentPage + this.searchCriteria.pageSize) / this.searchCriteria.pageSize;

        this.houseDataService.search(
            this.searchCriteria.address,
            this.searchCriteria.houseTypeId,
            this.searchCriteria.phoneNumber,
            this.searchCriteria.name,
            this.searchCriteria.code,
            this.searchCriteria.houseStatusId,
            this.searchCriteria.page,
            this.searchCriteria.pageSize
        )
            .subscribe(res => {
                var dataResult: any = res;
                this.countItems = dataResult.data.total;
                this.gridData = {
                    data: dataResult.data.items,
                    total: dataResult.data.total,
                }
            });
    };

    deleteFilters(): void {
        this.searchCriteria = new HouseSearchRequest();
        //this.viewHouseComponent.resetMaintenanceForm();
        this.searchCriteria.pageSize = 40; this.currentPage = 0;
        setTimeout(() => {
            $(window).resize();
        }, 300);
        this.onSearch();
    }

    onNew(): void {
        //this.viewHouseComponent.resetMaintenanceForm();
        DataService.currentHouse = new HouseDTO();
        this.router.navigateByUrl('amigotenant/house/new');
    }

    public cancel(): void {
        this.deleteFilters();
        //this.viewHouseComponent.resetMaintenanceForm();
        $(window).resize();
        this.visible = true;
        this.flgMantenance = true;
    }

    onEdit(dataItem): void {
        DataService.currentHouse = dataItem;
        this.router.navigateByUrl('amigotenant/house/edit/' + dataItem.houseId);
    }


    onDelete(dataItem): void {
        this.deleteHouse.houseId = dataItem.houseId;
        this.confirmDeletionVisible = true;
    }

    onConfirmation = (status) => {
        this.confirmDeletionResponse = (status === "YES");

        if (this.confirmDeletionResponse) {
            this.houseDataService.delete(this.deleteHouse)
                .subscribe(res => {
                    var dataResult: any = res;
                    if (dataResult.isValid) this.onSearch();
                });
        }

        this.confirmDeletionVisible = false;
    }

    closeConfirmation(status):void {
        this.confirmDeletionVisible = false;
    }

    onReloadGrid():void {
        this.searchCriteria.pageSize = 40;
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

            if($(".menu-top").length)
                _combinedPageElementsHeight += $(".menu-top").outerHeight();
            if($(".page-header").length)
                _combinedPageElementsHeight += $(".page-header").outerHeight();
            if($(".ro-tab.tabs-top").length)
                _combinedPageElementsHeight += $(".ro-tab.tabs-top").outerHeight();

            _viewportHeight += $(window).outerHeight() - _combinedPageElementsHeight;
            $(grid).find('.k-grid-content').height(_viewportHeight);
        });
    }

    public onExport() {
        
    }
}
