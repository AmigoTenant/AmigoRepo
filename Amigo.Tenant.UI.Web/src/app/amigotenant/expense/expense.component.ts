import { EntityStatusDTO, HouseDTO } from './../../shared/api/services.client';
import { Component, Input, Injectable, OnChanges, SimpleChange, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { Http, Jsonp, URLSearchParams } from '@angular/http';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { ConfirmationList, Confirmation } from '../../model/confirmation.dto';
import { ListsService } from '../../shared/constants/lists.service';
import { HouseClient, GeneralTableClient } from '../../shared/api/services.client';
//import { ExpenseClient, ExpenseSearchRequest, ExpenseDeleteRequest } from '../../shared/api/rentalapplication.services.client';
//SEARCH CRITERIA:End
import { EnvironmentComponent } from '../../shared/common/environment.component';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs'
import { NotificationService } from '../../shared/service/notification.service';
import { accessSync } from 'fs';
import { DataService } from '../house/dataService';
import { MasterDataService } from '../../shared/api/master-data-service';

declare var $: any;

export class modelDate {
    year: number;
    month: number;
    day: number;
}


@Component({
    selector: 'at-expense',
    templateUrl: './expense.component.html'
})

export class ExpenseComponent extends EnvironmentComponent implements OnInit {
    expenseSearchDTOs: GridDataResult;
    model: any; //ExpenseSearchRequest;
    public modelExpenseDateFrom: any;
    public modelExpenseDateTo: any;

    _currentPeriod: any;

    //GRID SELECT
    isColumnHeaderSelected: boolean = true;
    message: string;
    //isValidToApprove: boolean = false;

    //DROPDOWNS
    _listConfirmation: Confirmation[] = [];
    _listHouseTypes: any = [];
    _listPaymentTypes: any = [];
    _listConcepts: any = [];
    _listStatus: any = [];

    //TOTALS
    totalResultCount: number = 0;

    //MULTISELECT
    //public featureListMultiSelect: IMultiSelectOption[] = [];
    //public selectedOptionsFeature: number[] = [];
    //public selectedOptionsFeatureBackup: number[] = [];

    //public mySettings: IMultiSelectSettings = {
    //    pullRight: false,
    //    enableSearch: true,
    //    checkedStyle: 'checkboxes',
    //    buttonClasses: 'btn btn-default',
    //    selectionLimit: 0,
    //    closeOnSelect: false,
    //    showCheckAll: true,
    //    showUncheckAll: true,
    //    dynamicTitleMaxItems: 1,
    //    maxHeight: '300px',
    //};

    //public myTexts: IMultiSelectTexts = {
    //    checkAll: 'Check all',
    //    uncheckAll: 'Uncheck all',
    //    checked: 'checked',
    //    checkedPlural: 'checked',
    //    searchPlaceholder: 'Search...',
    //    defaultTitle: 'Select',
    //};

    //PAGINATION
    public buttonCount: number = 20;
    public info: boolean = true;
    public type: 'numeric' | 'input' = 'numeric';
    public pageSizes: any = [20, 50, 100, 200];
    public previousNext: boolean = true;
    public currentPage: number = 0;
    public skip: number = 0;

    public pageChange({ skip, take }: PageChangeEvent): void {
        this.currentPage = skip;
        //this.model.pageSize = take;
        let isExport: boolean = false;
        this.getExpense();
        this.deselectColumnAll();
    }


    constructor(
        //private expenseClient: ExpenseClient,
        private listConfirmation: ConfirmationList,
        //private listsService: ListsService,
        private route: ActivatedRoute,
        private router: Router,
        private houseDataService: HouseClient,
        private gnrlTableDataService: GeneralTableClient,
        private notificationService: NotificationService,
        private masterDataService: MasterDataService,
        private formBuilder: FormBuilder
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

    expenseSearchForm: FormGroup;



    initializeForm(): void {
        //this.model = new ExpenseSearchRequest();
        this.setDatesFromTo();
        this.resetGrid();
        this.getHouseTypes();
        this.getPaymentTypes();
        //this.getConceptsByTypeIdList();
        //this.getStatus();
        //this.model.pageSize = 20;
        this.totalResultCount = 0;
    }

    buildForm() {
        if (!this.expenseSearchForm) {
            this.expenseSearchForm = this.formBuilder.group({
                PaymentTypeId: [null],
                HouseTypeId: [null],
                TenantId: [null],
                StatusId: [null],
                ConceptId: [null],
                HouseId: [null],
                PeriodoId: [null],
                FromDate: [null],
                ToDate: [null]
            });
        }
    }

    public setDatesFromTo() {
        var date = new Date();
        this.modelExpenseDateFrom = new modelDate();
        this.modelExpenseDateTo = new modelDate();
        this.onSelectModelApplicationDateFrom();
        this.onSelectModelApplicationDateTo();
    }

    onSelectModelApplicationDateFrom(): void {
        if (this.modelExpenseDateFrom != null)
            this.model.applicationDateFrom = new Date(this.modelExpenseDateFrom.year, this.modelExpenseDateFrom.month - 1, this.modelExpenseDateFrom.day, 0, 0, 0, 0);
    }

    onSelectModelApplicationDateTo(): void {
        if (this.modelExpenseDateTo != null)
            this.model.applicationDateTo = new Date(this.modelExpenseDateTo.year, this.modelExpenseDateTo.month - 1, this.modelExpenseDateTo.day, 0, 0, 0, 0);
    }

    onSelect(): void {
        this.getExpense();
    }

    getExpense(): void {
        this.model.pageSize = +this.model.pageSize;
        this.model.page = (this.currentPage + this.model.pageSize) / this.model.pageSize;
        //debugger;
        //this.expenseClient.search(this.model.periodId, this.model.propertyTypeId,
        //    this.model.applicationDateFrom, this.model.applicationDateTo,
        //    this.model.fullName,
        //    this.model.email,
        //    this.model.checkInFrom, this.model.checkInTo,
        //    this.model.checkOutFrom, this.model.checkOutTo,
        //    this.model.residenseCountryId,
        //    this.model.residenseCountryName,
        //    this.model.budgetId,
        //    undefined,
        //    undefined,
        //    this.model.cityOfInterestId,
        //    this.model.housePartId,
        //    this.model.personNo,
        //    this.model.outInDownId,
        //    this.model.referredById,
        //    this.model.hasNotification,
        //    this.model.page,
        //    this.model.pageSize)
        //    .subscribe(response => {
        //        var datagrid: any = response;
        //        //debugger;
        //        this.expenseSearchDTOs = {
        //            data: datagrid.data.items,
        //            total: datagrid.data.total
        //        };
        //        this.totalResultCount = datagrid.data.total;
        //    });
    }

    getHouseTypes(): void {
        this.houseDataService.getHouseTypes()
            .subscribe(res => {
                var dataResult: any = res;
                this._listHouseTypes = dataResult.data;
            });
    }

    getConceptByTypes(): void {
        this.masterDataService.getConceptsByTypeIdList([31, 29])
            .subscribe(res => {
                var dataResult: any = res;
                this._listHouseTypes = dataResult.data;
            });
    }

    getPaymentTypes(): void {
        this.gnrlTableDataService.getGeneralTableByTableNameAsync("PaymentType")
            .subscribe(res => {
                //debugger;
                var dataResult: any = res;
                this._listStatus = [];
                for (var i = 0; i < dataResult.value.data.length; i++) {
                    this._listPaymentTypes.push({
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
        let c = this.expenseSearchDTOs.data.length;
        let index = this.model.page * this.model.pageSize - this.model.pageSize;
        for (let item in this.expenseSearchDTOs.data) {
            //if (this.expenseSearchDTOs.data[item].serviceStatus === null) {
                $("#" + index)[0].checked = this.isColumnHeaderSelected;
                this.expenseSearchDTOs.data[item].isSelected = this.isColumnHeaderSelected;
            //}
            index++;
        }
        this.isColumnHeaderSelected = !this.isColumnHeaderSelected;
    }

    public resetGrid(): void {
        let grid: GridDataResult[] = [];
        this.expenseSearchDTOs = {
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

    //onEdit(data): void {
    //    //debugger;
    //    this.router.navigate(['/leasing/rentalApp/edit', data.expenseId]); // + data.expenseId);
    //}

    //=========== 
    //INSERT
    //===========

    //onInsert(): void {
    //    this.router.navigateByUrl('leasing/rentalApp/new');
    //}

    //===========
    //DELETE
    //===========

    //public deleteMessage: string = "Are you sure to delete this Rental Application?";
    //expenseToDelete: any;

    //onDelete(entityToDelete) {
    //    this.expenseToDelete = new ExpenseDeleteRequest();
    //    this.expenseToDelete.expenseId = entityToDelete.expenseId;
    //    this.openedDeletionConfimation = true;
    //}

    //yesDelete() {
    //    this.expenseClient.delete(this.expenseToDelete)
    //        .subscribe(response => {
    //            this.onSelect();
    //            this.openedDeletionConfimation = false;
    //        });
    //}

    //public openedDeletionConfimation: boolean = false;

    //public closeDeletionConfirmation() {
    //    this.openedDeletionConfimation = false;
    //}

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
        //this.expenseClient.searchReport(
        //    this.model.periodId,
        //    this.model.expenseCode,
        //    this.model.expenseStatusId,
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


}
