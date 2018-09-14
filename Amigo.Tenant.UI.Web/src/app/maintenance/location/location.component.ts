import {Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import {Http, Jsonp, URLSearchParams} from '@angular/http';
import {GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import {LocationClient, LocationSearchRequest, LocationDTO, DeleteLocationRequest} from '../../shared/api/services.client';
import {LocationMaintenanceComponent } from './location-maintenance.component';
import { AuthCheckDirective } from  '../../shared/security/auth-check.directive';
import { Autofocus } from  '../../shared/directives/autofocus.directive';

import {  Router,ActivatedRoute } from '@angular/router';
import { DataService } from './dataService';
import {Observable, Subscription} from 'rxjs'

declare var $: any;

@Component({
    selector: 'st-location',
    templateUrl: './location.component.html'
})
export class LocationComponent implements OnInit {

    constructor(private router: Router,private route: ActivatedRoute, private locationDataService: LocationClient,public dataservice: DataService) { }

    @ViewChild(LocationMaintenanceComponent) viewLocationComponent: LocationMaintenanceComponent;

    public gridData: GridDataResult;
    public skip: number = 0;
    public listLocationType = [];
    public listParentLocation = [];
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

    @Output() open: EventEmitter<any> = new EventEmitter();
    @Output() close: EventEmitter<any> = new EventEmitter();


    searchCriteria = new LocationSearchRequest();
    deleteLocation = new DeleteLocationRequest();

ngOnDestroy() {
        this.sub.unsubscribe();
    }

sub: Subscription;
    ngOnInit() {

        this.sub = this.route.params.subscribe(params => {

            setTimeout(() => {
                    this.onSearch();
                }, 500);

        });

        this.locationDataService.getLocationTypes()
            .subscribe(res => {
                var dataResult: any = res;
                this.listLocationType = dataResult.data;
            });

        this.locationDataService.getParentLocations()
            .subscribe(res => {
                var dataResult: any = res;
                this.listParentLocation = dataResult.data;

            });

        // this.locationDataService.getCities()
        //     .subscribe(res => {
        //         var dataResult: any = res;
        //         this.listStatesCities = dataResult.data;
        //         this.listCountries = this.getCountries(dataResult.data);
        //     });

        $(document).ready(() => { this.resizeGrid(); });
        $(window).bind('load resize scroll', (e) => { this.resizeGrid(); });

        this.searchCriteria.pageSize = 40;
        this.currentPage = 0;

    }


    containsDuplicates = function (v, data) {
        var count = data.length;
        for (var i = 0; i < count; i++) {
            if (data[i].countryISOCode === v) return true;
        }
        return false;
    }

    getCountries(data: any): any {
        var countries = [];
        var count = data.length;
        for (var i = 0; i < count; i++) {
            if (!this.containsDuplicates(data[i].countryISOCode, countries)) {
                countries.push({
                    "countryISOCode": data[i].countryISOCode,
                    "countryName": data[i].countryName,
                });
            }
        }
        return countries;
    };

    onSearch = () => {
        
        this.searchCriteria.pageSize = +this.searchCriteria.pageSize;
        this.searchCriteria.page = (this.currentPage + this.searchCriteria.pageSize) / this.searchCriteria.pageSize;

        this.locationDataService.search(
            this.searchCriteria.name,
            this.searchCriteria.code,
            this.searchCriteria.zipCode,
            this.searchCriteria.hasGeofence,
            this.searchCriteria.countryISOCode,
            this.searchCriteria.stateCode,
            this.searchCriteria.cityCode,
            this.searchCriteria.locationTypeCode,
            this.searchCriteria.parentLocationCode,
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
        this.searchCriteria = new LocationSearchRequest();
        this.viewLocationComponent.resetMaintenanceForm();
        this.searchCriteria.pageSize = 40; this.currentPage = 0;
        setTimeout(() => {
            $(window).resize();
        }, 300);
        this.onSearch();
    }

    onNew(): void {
        this.viewLocationComponent.resetMaintenanceForm();
        DataService.currentlocation = new LocationDTO();
        this.router.navigateByUrl('maintenance/location/new');
    }

    public cancel(): void {
        this.deleteFilters();
        this.viewLocationComponent.resetMaintenanceForm();
        $(window).resize();
        this.visible = true;
        this.flgMantenance = true;
    }

    onEdit(dataItem): void {
        DataService.currentlocation = dataItem;
        this.router.navigateByUrl('maintenance/location/edit/' + dataItem.code);
    }


    onDelete(code): void {
        this.deleteLocation.code = code;
        this.confirmDeletionVisible = true;

    }

    onConfirmation = (status) => {
        this.confirmDeletionResponse = (status === "YES");

        if (this.confirmDeletionResponse) {
            this.locationDataService.delete(this.deleteLocation)
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

            _combinedPageElementsHeight += $(".menu-top").outerHeight();
            _combinedPageElementsHeight += $(".page-header").outerHeight();
            _combinedPageElementsHeight += $(".ro-tab.tabs-top").outerHeight();
            _viewportHeight += $(window).outerHeight() - _combinedPageElementsHeight;
            $(grid).find('.k-grid-content').height(_viewportHeight);
        });
    }
}
