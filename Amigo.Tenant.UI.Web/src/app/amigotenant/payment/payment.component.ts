import { PPSearchDTO } from './../../shared/api/payment.service';

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
import { PaymentServiceNew } from './payment.service';
import { MasterDataService } from '../../shared/api/master-data-service';
import { PaymentPeriodSendNotificationRequest } from './dto/payment-period-sendnotification-request';
import { PaymentDataService } from './payment-data.service';

declare var $: any;

@Component({
    selector: 'at-payment',
    templateUrl: './payment.component.html',
    styles:[
        `.pendingLabel { color:red}
         .paidLabel { color:green}
         .pendingTotalLabel { color:red; font-weight: bold}
         .paidTotalLabel { color:green; font-weight: bold}
        `
      ],
})
export class PaymentComponent implements OnInit {
    //TYPEAHEAD
    _currentHouse: any;
    _currentPeriod: any;
    _currentTenant: any;

    //DROPDOWNS
    _listEntityStatus: any[];
    _listYesNoBool: ConfirmationIntResult[] = [];

    //GRID
    isColumnHeaderSelected: boolean = true;

    public confirmationFilter(): void {
        var confirmation = this.listConfirmation.ListIntResult;
        confirmation.forEach(obj => {
            this._listYesNoBool.push(obj);
        });
    };


    getEntityStatus(): void {
        this.entityStatusClient.getEntityStatusByEntityCodeAsync("PP")
            .subscribe(response => {
                var dataResult: any = response;
                this._listEntityStatus = dataResult.data;
                let entity = new EntityStatusDTO();
                entity.entityStatusId = null;
                entity.name = 'All';
                this._listEntityStatus.unshift(entity);
            })
    }

    constructor(private router: Router, private route: ActivatedRoute,
        private paymentPeriodDataService: PaymentPeriodClient,
        private _listsService: ListsService,
        private listConfirmation: ConfirmationList,
        private entityStatusClient: EntityStatusClient,
        public serviceOrderService: PaymentService,
        public paymentServiceNew: PaymentServiceNew,
        public masterDataService: MasterDataService,
        public paymentService: PaymentDataService
    ) { }

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
    countItems: number = 0;
    visible: boolean = true;
    @Output() open: EventEmitter<any> = new EventEmitter();
    @Output() close: EventEmitter<any> = new EventEmitter();

    public TotalIncomeAmountByPeriod:number=0;
    public TotalIncomePaidAmount:number=0;
    public TotalIncomePendingAmount:number=0;

    searchCriteria = new PaymentPeriodSearchRequest();
    ngOnInit() {
        this.initializeForm(true);
        $(document).ready(() => { this.resizeGrid(); });
        $(window).bind('load resize scroll', (e) => { this.resizeGrid(); });
    }

    initializeForm(isCurrentPeriod: boolean): void {
        this.searchCriteria = new PaymentPeriodSearchRequest();
        this.searchCriteria.pageSize = 40;
        this.currentPage = 0;
        this.errorMessages = [];
        this._currentTenant = null;
        this._currentPeriod = null;
        this._currentHouse = null;
        this.getEntityStatus();
        this.confirmationFilter();
        this.setCurrentPeriod(isCurrentPeriod);
      }

    onSearch() {
        this.searchCriteria.pageSize = +this.searchCriteria.pageSize;
        this.searchCriteria.page = (this.currentPage + this.searchCriteria.pageSize) / this.searchCriteria.pageSize;
        this.paymentPeriodDataService.search(
            this.searchCriteria.periodId,
            this.searchCriteria.houseId,
            this.searchCriteria.contractCode,
            this.searchCriteria.paymentPeriodStatusId,
            this.searchCriteria.tenantId,
            this.searchCriteria.hasPendingFines,
            this.searchCriteria.hasPendingLateFee,
            this.searchCriteria.hasPendingDeposit,
            this.searchCriteria.page,
            this.searchCriteria.pageSize
        )
            .subscribe(res => {
                let dataResult: any = res;
                this.countItems = dataResult.data.total;
                this.setTotalPendingAmount(dataResult.data.items);
                this.setTotalDetailByPeriod(dataResult.data.items);
                this.gridData = {
                    data: dataResult.data.items,
                    total: dataResult.data.total,
                }
            });
    };

    onReset(): void {
        this.initializeForm(false);
    }

    public cancel(): void {
        this.onReset();
        $(window).resize();
    }

    onEdit(dataItem): void {
        this.router.navigateByUrl('amigotenant/payment/edit/' + dataItem.contractId + '/' + dataItem.periodId);
    }

    onReloadGrid(): void {
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

    public confirmSendPayNotification() {
        if (!this.isValidateToSendEmailNotification()){
            return;
        }

        this.openedConfimationPopup = true;
        this.confirmMessage = 'Are you sure to send email notification. Some emails will not be sent, so you must to verify which emails were not sent in the send email folder';
    }

    public onSendPayNotification(){

        let lista: PaymentPeriodSendNotificationRequest[]= [];
        
        this.gridData.data.filter(q => q.isSelected).forEach(element => {
            lista.push(new PaymentPeriodSendNotificationRequest(element.contractId, element.periodId, element.periodCode));
        });

        this.searchCriteria.pageSize = +this.searchCriteria.pageSize;
        this.searchCriteria.page = (this.currentPage + this.searchCriteria.pageSize) / this.searchCriteria.pageSize;

        this.paymentService.SendPaymentNotificationEMail(
            lista
        ).subscribe(res => {
            let dataResult: any = res;
            this.successFlag = dataResult.isValid;
            this.errorMessages = dataResult.messages;
            this.successMessage = 'Emails has been sent Successfully';
        });
    }

    isValidateToSendEmailNotification(){
        this.errorMessages= [];
        
        if (this.searchCriteria.periodId === null || this.searchCriteria.periodId === undefined || this.searchCriteria.periodId === 0){
            this.errorMessages.push({message: 'Period is required to send Notification'});
        }
        if (this.gridData.data.filter(q => q.isSelected).length===0){
            this.errorMessages.push({message: 'You must to select at least one record'});
        }

        if (this.errorMessages.length>0)
        {
            this.successFlag = false;
            this.successMessage = null;

            setTimeout(() => {
                this.successFlag = null;
                this.errorMessages = null;
                this.successMessage = null; }, 5000);

            return false;

        }
        

        return true;
    }


    exportToExcel() {
        let period = this.paymentServiceNew.exportToExcel(
            this.searchCriteria.periodId,
            this.searchCriteria.houseId,
            this.searchCriteria.contractCode,
            this.searchCriteria.paymentPeriodStatusId,
            this.searchCriteria.tenantId,
            this.searchCriteria.hasPendingFines,
            this.searchCriteria.hasPendingLateFee,
            this.searchCriteria.hasPendingDeposit,
            this.searchCriteria.page,
            this.searchCriteria.pageSize
        );
    }

    setCurrentPeriod(currentPeriod) {
        let period = this.masterDataService.getCurrentPeriod().subscribe(
            res => {
                this._currentPeriod = res.Data;
            })
        .add(x => {
            if (currentPeriod) {
                this.searchCriteria.periodId = this.searchCriteria.periodId === null || this.searchCriteria.periodId === undefined?
                this._currentPeriod.PeriodId : this.searchCriteria.periodId;
            }
            this.onSearch();

        });
    }

    //===========
    //SEND EMAIL
    //===========
    public confirmMessage: string;
    public openedConfimationPopup = false;

    public yesConfirm() {
        this.onSendPayNotification();
        this.openedConfimationPopup = false;
    }

    public closeConfirmation() {
        this.openedConfimationPopup = false;
    }



    public changeItemHeader() {
        let c = this.gridData.data.length;
        let index = this.searchCriteria.page * this.searchCriteria.pageSize - this.searchCriteria.pageSize;
        for (let item in this.gridData.data) {
            $("#" + index)[0].checked = this.isColumnHeaderSelected;
            this.gridData.data[item].isSelected = this.isColumnHeaderSelected;
            index++;
        }
        this.isColumnHeaderSelected = !this.isColumnHeaderSelected;
    }

    public changeItem(d) {
        d.isSelected = !d.isSelected;
    }

    public deselectColumnAll() {
        this.isColumnHeaderSelected = true;
        $("#HeaderTemplate")[0].checked = !this.isColumnHeaderSelected;
    }

    setTotalPendingAmount(data: any[]){
        data.forEach(q=> {
            q.totalAmountPending = q.rentAmountPending+q.depositAmountPending+q.finesAmountPending+q.onAccountAmountPending+q.lateFeesAmountPending;
            q.totalAmountPaid = q.rentAmountPaid+q.depositAmountPaid+q.finesAmountPaid+q.onAccountAmountPaid+q.lateFeesAmountPaid;
        });
    }

    setTotalDetailByPeriod(data: any[]){
        if (data.length>0) 
        {
            this.TotalIncomeAmountByPeriod = data[0].totalIncomeAmountByPeriod;
            this.TotalIncomePaidAmount = data[0].totalIncomePaidAmount;
            this.TotalIncomePendingAmount = data[0].totalIncomePendingAmount;
        }
    }

    onLiquidate(data: any){
        this.router.navigate(['/amigotenant/payment/liquidate', data.contractId, data.periodId]);
    }

    onShowLiquidation(data: any){
        this.router.navigate(['/amigotenant/payment/liquidate', 0, 0, data.invoiceId]);
    }
}
