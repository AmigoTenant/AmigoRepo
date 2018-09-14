import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { Http, Jsonp, URLSearchParams } from '@angular/http';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';

import { EntityStatusClient, EntityStatusDTO } from '../../shared/api/services.client';
import { PaymentPeriodClient /*, PaymentPeriodSearchRequest, PaymentDTO, DeletePaymentRequest*/ } from '../../shared/api/payment.services.client';
import { PaymentMaintenanceComponent } from './payment-maintenance.component';
import { AuthCheckDirective } from  '../../shared/security/auth-check.directive';
import { Autofocus } from  '../../shared/directives/autofocus.directive';

import { Router,ActivatedRoute } from '@angular/router';
//import { DataService } from './dataService';
import { Observable, Subscription } from 'rxjs'
import { ListsService } from '../../shared/constants/lists.service';

import { ConfirmationList, ConfirmationIntResult } from '../../model/confirmation.dto';
import { PaymentService, PaymentPeriodSearchRequest } from "../../shared/api/payment.service";

declare var $: any;

@Component({
    selector: 'at-payment',
    templateUrl: './payment.component.html'
})
export class PaymentComponent implements OnInit {
    //TYPEAHEAD
    _currentHouse: any;
    _currentPeriod: any;
    _currentTenant: any;

    //DROPDOWNS
    _listEntityStatus: any[];
    _listYesNoBool: ConfirmationIntResult[] = [];

    public confirmationFilter(): void {
        var confirmation = this.listConfirmation.ListIntResult;
        confirmation.forEach(obj => {
            this._listYesNoBool.push(obj);
        });
    };


    getEntityStatus(): void {
        this.entityStatusClient.getEntityStatusByEntityCodeAsync("PP")
            .subscribe(response => {
                //debugger;
                var dataResult: any = response;
                this._listEntityStatus = dataResult.data;
                let entity = new EntityStatusDTO();
                entity.entityStatusId = null;
                entity.name = 'All';
                this._listEntityStatus.unshift(entity);
            })
    }

    constructor(private router: Router, private route: ActivatedRoute,
        private paymentDataService: PaymentPeriodClient,
        private _listsService: ListsService,
        private listConfirmation: ConfirmationList,
        private entityStatusClient: EntityStatusClient,
        public serviceOrderService: PaymentService) { }

    //@ViewChild(PaymentMaintenanceComponent) viewPaymentComponent: PaymentMaintenanceComponent;

    public gridData: GridDataResult;
    public skip: number = 0;
    public listPaymentTypes = [];
    public listPaymentStatuses = [];
    public SelectedCode: string;
    public buttonCount: number = 20;
    public info: boolean = true;
    public type: 'numeric' | 'input' = 'numeric';
    public pageSizes: any = [20, 50, 100, 200];
    public previousNext: boolean = true;
    public currentPage: number = 0;
    //public flgMantenance: boolean = true;

    //confirmDeletionVisible: boolean = false;
    //confirmDeletionResponse: boolean = false;
    //confirmDeletionActionCode: string;
    countItems: number = 0;
    visible: boolean = true;

    //public activeInactiveStatus: any[] = this._listsService.ActiveInactiveStatus();

    @Output() open: EventEmitter<any> = new EventEmitter();
    @Output() close: EventEmitter<any> = new EventEmitter();

    searchCriteria = new PaymentPeriodSearchRequest();
    //deletePayment = new DeletePaymentRequest();
    sub: Subscription;

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    ngOnInit() {
        
        this.searchCriteria.pageSize = 40;
        this.currentPage = 0;
        this.sub = this.route.params.subscribe(params => {
            setTimeout(() => {
                    this.onSearch();
                }, 500);
        });

        this.getEntityStatus();

        this.confirmationFilter();

        $(document).ready(() => { this.resizeGrid(); });
        $(window).bind('load resize scroll', (e) => { this.resizeGrid(); });
    }

    onSearch() {
        this.searchCriteria.pageSize = +this.searchCriteria.pageSize;
        this.searchCriteria.page = (this.currentPage + this.searchCriteria.pageSize) / this.searchCriteria.pageSize;

        this.paymentDataService.search(
            this.searchCriteria.periodId,
            this.searchCriteria.houseId,
            this.searchCriteria.contractCode,
            this.searchCriteria.paymentPeriodStatusId,
            this.searchCriteria.tenantId,
            this.searchCriteria.hasPendingServices,
            this.searchCriteria.hasPendingFines,
            this.searchCriteria.hasPendingLateFee,
            this.searchCriteria.hasPendingDeposit,
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
        this.searchCriteria = new PaymentPeriodSearchRequest();
        this.searchCriteria.pageSize = 40; this.currentPage = 0;
        this.errorMessages= [];
        setTimeout(() => {
            $(window).resize();
        }, 300);
        this.onSearch();
        this._currentTenant = null;
        this._currentPeriod = null;
        this._currentHouse = null;
    }

    public cancel(): void {
        this.deleteFilters();
        $(window).resize();
    }

    onEdit(dataItem): void {
        this.router.navigateByUrl('amigotenant/payment/edit/' + dataItem.contractId + '/' + dataItem.periodId);
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

    public onExport() {
        
    }

    getPeriod = (item) => {
        if (item != null && item != undefined && item != "") {
            this.searchCriteria.periodId = item.periodId;
            this._currentPeriod = item;
        }
        else {
            this.searchCriteria.periodId = undefined;
            this._currentPeriod = undefined;
        }
    };

    getHouse = (item) => {
        if (item != null && item != undefined && item != "") {
            this.searchCriteria.houseId = item.houseId;
            this._currentHouse = item;
        }
        else {
            this.searchCriteria.houseId = undefined;
            this._currentHouse = undefined;
        }
    };

    getTenant = (item) => {
        if (item != null && item != undefined) {
            this.searchCriteria.tenantId = item.tenantId;
            this._currentTenant = item;
        }
        else {
            this.searchCriteria.tenantId = 0;
            this._currentTenant = undefined;
        }
    };


    public successFlag: boolean;
    public errorMessages: any[];
    public successMessage: string;

    public onSendPayNotification(){
        this.searchCriteria.pageSize = +this.searchCriteria.pageSize;
        this.searchCriteria.page = (this.currentPage + this.searchCriteria.pageSize) / this.searchCriteria.pageSize;
        
        if (!this.isValidateToSendEmailNotification())
            return;

        this.paymentDataService.sendPayNotification(
            this.searchCriteria.periodId,
            11, //PPPENDING
            this.searchCriteria.tenantId,
            this.searchCriteria.hasPendingServices,
            this.searchCriteria.hasPendingFines,
            this.searchCriteria.hasPendingLateFee,
            this.searchCriteria.hasPendingDeposit,
            1,
            100
        )
            .subscribe(res => {
                var dataResult: any = res;
                this.successFlag = dataResult.isValid;
                this.errorMessages = dataResult.messages;
                this.successMessage = 'Emails has been sent Successfully';
            });
    }

    isValidateToSendEmailNotification(){
        if (this._currentPeriod === null || this._currentPeriod === undefined){
            this.successFlag = false;
            this.errorMessages = [{message: 'Period is required to send Notification'}];
            this.successMessage = null;

            setTimeout(() => { 
                this.successFlag = null;
                this.errorMessages = null;
                this.successMessage = null; }, 5000);

            return false;

        }
        return true;
    }
}



//Select * from Tenant where FullName like '%Luis Gonzales%' 
//Select * from Tenant where FullName like '%Medina Robles%' 
//Select * from Tenant where FullName like '%David%' 


//Update Tenant Set email = NULL
//where tenantId = 166

//Update Tenant Set email = 'james.romero@tss.com.pe'
//where tenantId = 164

//Update Tenant Set email = 'jamromguz@outlook.com'
//where tenantId = 167

//Update Tenant Set email = null
//where tenantId = 166

//Update Tenant Set email = null
//where tenantId = 164

//Update Tenant Set email = null
//where tenantId = 167