import { EntityStatusDTO, HouseDTO } from './../../shared/api/services.client';
import { Component, Input, Injectable, OnChanges, SimpleChange, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { Http, Jsonp, URLSearchParams } from '@angular/http';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { ConfirmationList, Confirmation } from '../../model/confirmation.dto';
import { ListsService } from '../../shared/constants/lists.service';
import { HouseClient, CountryClient, GeneralTableClient, FeatureClient } from '../../shared/api/services.client';
import { RentalApplicationClient, RentalApplicationSearchRequest, RentalApplicationDeleteRequest } from '../../shared/api/rentalapplication.services.client';
//SEARCH CRITERIA:End
import { EnvironmentComponent } from '../../shared/common/environment.component';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs'
import { NotificationService } from '../../shared/service/notification.service';
import { accessSync } from 'fs';

declare var $: any;

export class modelDate {
    year: number;
    month: number;
    day: number;
}


@Component({
    selector: 'at-rentalApplication',
    templateUrl: './rentalApplication.component.html'
})

export class RentalApplicationComponent extends EnvironmentComponent implements OnInit {
    rentalApplicationSearchDTOs: GridDataResult;
    model: RentalApplicationSearchRequest;
    public modelApplicationDateFrom: any;
    public modelApplicationDateTo: any;
    public modelCheckInFrom: any;
    public modelCheckInTo: any;
    public modelCheckOutFrom: any;
    public modelCheckOutTo: any;
    rentalApplicationModel: any;
    _currentPeriod: any;

    //GRID SELECT
    isColumnHeaderSelected: boolean = true;
    message: string;
    //isValidToApprove: boolean = false;

    //DROPDOWNS
    //_listEntityStatus: any[];
    //_listProperties: any[];
    //_listUnpaidPeriods: Confirmation[] = [];
    _listConfirmation: Confirmation[] = [];
    //_listNextDaysToCollect: any[];
    _listHouseTypes: any = [];
    _listCountries: any = [];
    _listBudgets: any = [];
    _listReferredBy: any = [];

    //TOTALS
    totalResultCount: number = 0;

    //MULTISELECT
    public featureListMultiSelect: IMultiSelectOption[] = [];
    public selectedOptionsFeature: number[] = [];
    public selectedOptionsFeatureBackup: number[] = [];

    public mySettings: IMultiSelectSettings = {
        pullRight: false,
        enableSearch: true,
        checkedStyle: 'checkboxes',
        buttonClasses: 'btn btn-default',
        selectionLimit: 0,
        closeOnSelect: false,
        showCheckAll: true,
        showUncheckAll: true,
        dynamicTitleMaxItems: 1,
        maxHeight: '300px',
    };

    public myTexts: IMultiSelectTexts = {
        checkAll: 'Check all',
        uncheckAll: 'Uncheck all',
        checked: 'checked',
        checkedPlural: 'checked',
        searchPlaceholder: 'Search...',
        defaultTitle: 'Select',
    };

    //PAGINATION
    public buttonCount: number = 200;
    public info: boolean = true;
    public type: 'numeric' | 'input' = 'numeric';
    public pageSizes: any = [100, 200, 300];
    public previousNext: boolean = true;
    public currentPage: number = 0;
    public skip: number = 0;

    public pageChange({ skip, take }: PageChangeEvent): void {
        this.currentPage = skip;
        this.model.pageSize = take;
        let isExport: boolean = false;
        this.getRentalApplication();
        this.deselectColumnAll();
    }


    constructor(
        private rentalApplicationClient: RentalApplicationClient,
        //private entityStatusClient: EntityStatusClient,
        //private houseClient: HouseClient,
        //private featureClient: FeatureClient,
        private listConfirmation: ConfirmationList,
        private listsService: ListsService,
        private route: ActivatedRoute,
        private router: Router,
        private houseDataService: HouseClient,
        private countryDataService: CountryClient,
        private gnrlTableDataService: GeneralTableClient,
        private notificationService: NotificationService
    ) {
        super();
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    sub: Subscription;
    ngOnInit() {

        this.sub = this.route.params.subscribe(params => {

            setTimeout(() => {
                this.onSelect();
            }, 100);

        });

        this.initializeForm();
        this.resetResults();
    }

    public resetResults() {
        $(document).ready(() => {
            this.resizeGrid();
        });

        $(window).bind('load resize scroll', (e) => {
            this.resizeGrid();
        });
    }

    onReset(): void {
        this.initializeForm();
    }

    initializeForm(): void {
        this.model = new RentalApplicationSearchRequest();
        this.setDatesFromTo();
        this.resetGrid();
        this.getHouseTypes();
        this.onGetCountries();
        this.getBudgets();
        this.getReferredBy();
        this.model.fullName = undefined;
        this.model.email = undefined;
        this.model.residenseCountryName = undefined;
        this.model.periodId = undefined;
        this.model.propertyTypeId = undefined;
        this.model.budgetId = undefined;

        this.model.pageSize = 200;
        this.totalResultCount = 0;
    }

    public setDatesFromTo() {
        var date = new Date();
        this.modelApplicationDateFrom = new modelDate();
        this.modelApplicationDateTo = new modelDate();
        this.modelCheckInFrom = new modelDate();
        this.modelCheckInTo = new modelDate();
        this.modelCheckOutFrom = new modelDate();
        this.modelCheckOutTo = new modelDate();

        this.onSelectModelApplicationDateFrom();
        this.onSelectModelApplicationDateTo();

        this.onSelectModelCheckInFrom();
        this.onSelectModelCheckInTo();

        this.onSelectModelCheckOutFrom();
        this.onSelectModelCheckOutTo();
    }

    onSelectModelApplicationDateFrom(): void {
        if (this.modelApplicationDateFrom != null)
            this.model.applicationDateFrom = new Date(this.modelApplicationDateFrom.year, this.modelApplicationDateFrom.month - 1, this.modelApplicationDateFrom.day, 0, 0, 0, 0);
    }

    onSelectModelApplicationDateTo(): void {
        if (this.modelApplicationDateTo != null)
            this.model.applicationDateTo = new Date(this.modelApplicationDateTo.year, this.modelApplicationDateTo.month - 1, this.modelApplicationDateTo.day, 0, 0, 0, 0);
    }

    onSelectModelCheckInFrom(): void {
        if (this.modelCheckInFrom != null)
            this.model.checkInFrom = new Date(this.modelCheckInFrom.year, this.modelCheckInFrom.month - 1, this.modelCheckInFrom.day, 0, 0, 0, 0);
    }

    onSelectModelCheckInTo(): void {
        if (this.modelCheckInTo != null)
            this.model.checkInTo = new Date(this.modelCheckInTo.year, this.modelCheckInTo.month - 1, this.modelCheckInTo.day, 0, 0, 0, 0);
    }

    onSelectModelCheckOutFrom(): void {
        if (this.modelCheckOutFrom != null)
            this.model.checkOutFrom = new Date(this.modelCheckOutFrom.year, this.modelCheckOutFrom.month - 1, this.modelCheckOutFrom.day, 0, 0, 0, 0);
    }

    onSelectModelCheckOutTo(): void {
        if (this.modelCheckOutTo != null)
            this.model.checkOutTo = new Date(this.modelCheckOutTo.year, this.modelCheckOutTo.month - 1, this.modelCheckOutTo.day, 0, 0, 0, 0);
    }

    onSelect(): void {
        this.getRentalApplication();
    }

    getRentalApplication(): void {
        this.model.pageSize = +this.model.pageSize;
        this.model.page = (this.currentPage + this.model.pageSize) / this.model.pageSize;
        //debugger;
        this.rentalApplicationClient.search(this.model.periodId, this.model.propertyTypeId,
            this.model.applicationDateFrom, this.model.applicationDateTo,
            this.model.fullName,
            this.model.email,
            this.model.checkInFrom, this.model.checkInTo,
            this.model.checkOutFrom, this.model.checkOutTo,
            this.model.residenseCountryId,
            this.model.residenseCountryName,
            this.model.budgetId,
            undefined,
            undefined,
            this.model.cityOfInterestId,
            this.model.housePartId,
            this.model.personNo,
            this.model.outInDownId,
            this.model.referredById,
            this.model.hasNotification,
            this.model.page,
            this.model.pageSize)
            .subscribe(response => {
                var datagrid: any = response;
                //debugger;
                this.rentalApplicationSearchDTOs = {
                    data: datagrid.data.items,
                    total: datagrid.data.total
                };
                this.totalResultCount = datagrid.data.total;
            });
    }

    getHouseTypes(): void {
        this.houseDataService.getHouseTypes()
            .subscribe(res => {
                var dataResult: any = res;
                this._listHouseTypes = dataResult.data;
            });
    }

    onGetCountries(): void {
        this.countryDataService.getCountriesAll()
            .subscribe(res => {
                var dataResult: any = res;
                this._listCountries = this.getCountries(dataResult.value.data);
            });
    }

    private getCountries(data: any): any {
        var countries = [];
        var count = data.length;
        for (var i = 0; i < count; i++) {
            if (!this.containsDuplicates(data[i].isoCode, countries)) {
                countries.push({
                    "id": data[i].countryId,
                    "isoCode": data[i].isoCode,
                    "name": data[i].Name,
                });
            }
        }
        return countries;
    };

    private containsDuplicates = function (v, data) {
        var count = data.length;
        for (var i = 0; i < count; i++) {
            if (data[i].isoCode === v) return true;
        }
        return false;
    }

    getBudgets(): void {
        this.gnrlTableDataService.getGeneralTableByTableNameAsync("BudgetRange")
            .subscribe(res => {
                //debugger;
                var dataResult: any = res;
                this._listBudgets = [];
                for (var i = 0; i < dataResult.value.data.length; i++) {
                    this._listBudgets.push({
                        "typeId": dataResult.value.data[i].generalTableId,
                        "name": dataResult.value.data[i].value
                    });
                }
            });
    }

    getReferredBy(): void {
        this.gnrlTableDataService.getGeneralTableByTableNameAsync("ReferredBy")
            .subscribe(res => {
                //debugger;
                var dataResult: any = res;
                this._listReferredBy = [];
                for (var i = 0; i < dataResult.value.data.length; i++) {
                    this._listReferredBy.push({
                        "typeId": dataResult.value.data[i].generalTableId,
                        "name": dataResult.value.data[i].value
                    });
                }
            });
    }

    //=========== 
    //GRID
    //===========
    public changeItemHeader() {
        let c = this.rentalApplicationSearchDTOs.data.length;
        let index = this.model.page * this.model.pageSize - this.model.pageSize;
        for (let item in this.rentalApplicationSearchDTOs.data) {
            //if (this.rentalApplicationSearchDTOs.data[item].serviceStatus === null) {
                $("#" + index)[0].checked = this.isColumnHeaderSelected;
                this.rentalApplicationSearchDTOs.data[item].isSelected = this.isColumnHeaderSelected;
            //}
            index++;
        }
        this.isColumnHeaderSelected = !this.isColumnHeaderSelected;
    }

    public resetGrid(): void {
        let grid: GridDataResult[] = [];
        this.rentalApplicationSearchDTOs = {
            data: grid,
            total: 0
        };
        this.skip = 0;
    }

    private resizeGrid() {
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

    public changeItem(d) {
        d.isSelected = !d.isSelected;
    }

    public deselectColumnAll() {
        this.isColumnHeaderSelected = true;
        $("#HeaderTemplate")[0].checked = !this.isColumnHeaderSelected;
    }

    //=========== 
    //EDIT
    //===========

    onEdit(data): void {
        //debugger;
        this.router.navigate(['/leasing/rentalApp/edit', data.rentalApplicationId]); // + data.rentalApplicationId);
    }

    //=========== 
    //INSERT
    //===========

    onInsert(): void {
        this.router.navigateByUrl('leasing/rentalApp/new');
    }

    //===========
    //DELETE
    //===========

    public deleteMessage: string = "Are you sure to delete this Rental Application?";
    rentalApplicationToDelete: any;

    onDelete(entityToDelete) {
        this.rentalApplicationToDelete = new RentalApplicationDeleteRequest();
        this.rentalApplicationToDelete.rentalApplicationId = entityToDelete.rentalApplicationId;
        this.openedDeletionConfimation = true;
    }

    yesDelete() {
        this.rentalApplicationClient.delete(this.rentalApplicationToDelete)
            .subscribe(response => {
                this.onSelect();
                this.openedDeletionConfimation = false;
            });
    }

    public openedDeletionConfimation: boolean = false;

    public closeDeletionConfirmation() {
        this.openedDeletionConfimation = false;
    }

    //===========
    //EXPORT
    //===========

    getPeriod = (item) => {
        if (item != null && item != undefined && item != "") {
            this.model.periodId = item.periodId;
            this._currentPeriod = item;
        }
        else {
            this.model.periodId = undefined;
            this._currentPeriod = undefined;
        }
    };

    onExport(): void {
        //this.rentalApplicationClient.searchReport(
        //    this.model.periodId,
        //    this.model.rentalApplicationCode,
        //    this.model.rentalApplicationStatusId,
        //    this.model.beginDate,
        //    this.model.endDate,
        //    this.model.tenantFullName,
        //    this.model.houseId,
        //    this.model.unpaidPeriods,
        //    this.model.nextDaysToCollect,
        //    this.selectedOptionsFeature,
        //    this.model.page,
        //    20000);
    }

    //======================
    //SEND WHATSAPP MESSAGE
    //======================

    public whatsAppMessageMessage: string = "Are you sure to Formalize this Lease?";
    contractToWhatsAppMessage: any;

    onOpenWhatsAppMessage() {
        this.openedWhatsAppMessageConfimation = true;
    }

    onSend() {
        //debugger;
        let numbers = this.rentalApplicationSearchDTOs.data.filter((value, index) => {
            return value.isSelected;
        }).map((value, index) => {
            return JSON.stringify(value.cellPhone).replace(/\W/g, '');
        });
        this.notificationService
            .sendNotification(numbers, encodeURIComponent(this.message))
            .subscribe(res => {
                //debugger;
                var dataResult: any = res;
                this.openedWhatsAppMessageConfimation = false;
                this.openedSendConfimationMsg = false;
            });
    }

    public openedWhatsAppMessageConfimation: boolean = false;

    public closeWhatsAppMessageConfirmation() {
        this.openedWhatsAppMessageConfimation = false;
    }

    //===========
    //CONFIRMATION SEND MESSAGE
    //===========

    public sendMessage: string = "Are you sure to Formalize this Lease?";

    SendAppMessage() {
        //debugger;
        //this.yesWhatsAppMessage();
        this.openedSendConfimationMsg = true;
    }

    public openedSendConfimationMsg: boolean = false;

    public closeSendConfirmationMsg() {
        this.openedSendConfimationMsg = false;
    }

}
