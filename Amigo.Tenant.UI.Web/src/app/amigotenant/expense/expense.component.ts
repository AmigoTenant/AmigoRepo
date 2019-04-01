import { EntityStatusDTO, HouseDTO } from './../../shared/api/services.client';
import { Component, Input, Injectable, OnChanges, SimpleChange, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { Http, Jsonp, URLSearchParams } from '@angular/http';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { ConfirmationList, Confirmation } from '../../model/confirmation.dto';
import { ListsService } from '../../shared/constants/lists.service';
import { HouseClient,    GeneralTableClient,    ResponseDTOOfListOfHouseTypeDTO} from '../../shared/api/services.client';
import { EnvironmentComponent } from '../../shared/common/environment.component';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { Observable, Subscription } from 'rxjs'
import { NotificationService } from '../../shared/service/notification.service';
import { accessSync } from 'fs';
import { DataService } from '../house/dataService';
import { MasterDataService } from '../../shared/api/master-data-service';
import { ResponseListDTO, PagedListOfResponseDTO } from '../../shared/dto/response-list-dto';
import { ExpenseDeleteRequest } from "./dto/expense-delete-request";
import { ExpenseDataService } from "./expense-data.service";
import { ExpenseSearchRequest } from "./dto/expense-search-request";

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
    model: ExpenseSearchRequest;
    public modelExpenseDateFrom: any;
    public modelExpenseDateTo: any;
    public expenseSearchForm: FormGroup;
    _currentPeriod: any;
    //GRID SELECT
    isColumnHeaderSelected = true;
    message: string;

    //DROPDOWNS
    _listConfirmation: Confirmation[] = [];
    _listHouseTypes: any = [];
    _listPaymentTypes: any = [];
    _listConcepts: any = [];
    _listStatus: any = [];

    //TOTALS
    totalResultCount = 0;

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
    public buttonCount = 20;
    public info = true;
    public type: 'numeric' | 'input' = 'numeric';
    public pageSizes: any = [20, 50, 100, 200];
    public previousNext = true;
    public currentPage = 0;
    public skip = 0;

    public pageChange({ skip, take }: PageChangeEvent): void {
        this.currentPage = skip;
        this.model.pageSize = take;
        let isExport = false;
        this.getExpense();
        this.deselectColumnAll();
    }

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private masterDataService: MasterDataService,
        private formBuilder: FormBuilder,
        private expenseDataService: ExpenseDataService
    ) {
        super();
    }

    ngOnInit() {
        this.buildForm();
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
        this.model = new ExpenseSearchRequest();
        this.setDatesFromTo();
        this.resetGrid();
        this.getHouseTypes();
        this.getPaymentTypes();
        this.getConceptByTypes();
        this.model.pageSize = 20;
        this.totalResultCount = 0;
    }

    buildForm() {
        this.expenseSearchForm = this.formBuilder.group({
            paymentTypeId: null,
            houseTypeId: null,
            tenantId: null,
            statusId: null,
            conceptId: null,
            houseId: null,
            periodoId: null,
            expenseDateFrom: null,
            expenseDateTo: null
         });
    }

    public setDatesFromTo() {
        let date = new Date();
        this.modelExpenseDateFrom = new modelDate();
        this.modelExpenseDateTo = new modelDate();
        this.onSelectModelApplicationDateFrom();
        this.onSelectModelApplicationDateTo();
    }

    onSelectModelApplicationDateFrom(): void {
        if (this.modelExpenseDateFrom != null) {
            this.model.expenseDateFrom = new Date(
                this.modelExpenseDateFrom.year,
                this.modelExpenseDateFrom.month - 1,
                this.modelExpenseDateFrom.day, 0, 0, 0, 0);
        }
    }

    onSelectModelApplicationDateTo(): void {
        if (this.modelExpenseDateTo != null) {
            this.model.expenseDateTo = new Date(
                this.modelExpenseDateTo.year,
                this.modelExpenseDateTo.month - 1,
                this.modelExpenseDateTo.day, 0, 0, 0, 0);
        }
    }

    onSelect(): void {
        this.getExpense();
    }

    getExpense(): void {
        Object.assign(this.model, this.expenseSearchForm.value);
        this.onSelectModelApplicationDateFrom();
        this.onSelectModelApplicationDateTo();
        this.model.pageSize = +this.model.pageSize;
        this.model.page = (this.currentPage + this.model.pageSize) / this.model.pageSize;
        this.expenseDataService.search(this.model)
           .subscribe(response => {
               let datagrid: any = new ResponseListDTO(response);
               this.expenseSearchDTOs = {
                   data: datagrid.items,
                   total: datagrid.total
               };
               this.totalResultCount = datagrid.total;
           });
    }

    getHouseTypes(): void {
        this.masterDataService.getHouseTypes()
            .subscribe(res => {
                this._listHouseTypes = [];
                let dataResult = new ResponseListDTO(res);
                this._listHouseTypes = dataResult.data;
            });
    }

    getConceptByTypes(): void {
        this.masterDataService.getConceptsByTypeIdList([13]) //Cambiar para que se trate con Constantes
            .subscribe(res => {
                this._listHouseTypes = [];
                let dataResult = new ResponseListDTO(res);
                this._listConcepts = dataResult.data;
            });
    }

    getPaymentTypes(): void {
        this.masterDataService.getGeneralTableByTableName('ConceptType')
            .subscribe(res => {
                let dataResult = new ResponseListDTO(res);
                this._listPaymentTypes = [];
                for (let i = 0; i < dataResult.data.length; i++) {
                    this._listPaymentTypes.push({
                        'typeId': dataResult.data[i].generalTableId,
                        'name': dataResult.data[i].value
                    });
                }
            });
    }

    public changeItemHeader() {
        let c = this.expenseSearchDTOs.data.length;
        let index = this.model.page * this.model.pageSize - this.model.pageSize;
        for (let item in this.expenseSearchDTOs.data) {
                $("#" + index)[0].checked = this.isColumnHeaderSelected;
                this.expenseSearchDTOs.data[item].isSelected = this.isColumnHeaderSelected;
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

    onEdit(data): void {
       this.router.navigate(['/amigotenant/expense/edit', data.expenseId , data.periodId]); // + data.expenseId);
    }

    //=========== 
    //INSERT
    //===========

    //onInsert(): void {
    //    this.router.navigateByUrl('leasing/rentalApp/new');
    //}

    //===========
    //DELETE
    //===========

    public deleteMessage: string = "Are you sure to delete this Rental Application?";
    expenseToDelete: any;

    onDelete(entityToDelete) {
       this.expenseToDelete = new ExpenseDeleteRequest();
       this.expenseToDelete.expenseId = entityToDelete.expenseId;
       this.openedDeletionConfimation = true;
    }

    yesDelete() {
    //    this.expenseDataService.delete(this.expenseToDelete)
    //        .subscribe(response => {
    //            this.onSelect();
    //            this.openedDeletionConfimation = false;
    //        });
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

    onAddExpense(): void {
        debugger
        //this.router.navigate['/amigotenant/expense/new'];
        this.router.navigateByUrl('amigotenant/expense/new');
    }


}
