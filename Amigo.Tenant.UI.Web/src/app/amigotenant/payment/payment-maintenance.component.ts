import { Component, Input, Output, state, SimpleChange, ViewChild, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { Http, Jsonp, URLSearchParams } from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { BotonsComponent } from '../../controls/common/boton.component';
import { ValidationService } from '../../shared/validations/validation.service';
import { Observable, Subscription } from 'rxjs'
import { Router, ActivatedRoute } from '@angular/router';

import { PaymentPeriodClient, PPHeaderSearchByContractPeriodDTO, PPDetailSearchByContractPeriodDTO, ApplicationMessage } from '../../shared/api/payment.services.client';
import { MasterDataService } from '../../shared/api/master-data-service';
import { ResponseListDTO } from '../../shared/dto/response-list-dto';
import { GenericValidator } from '../../shared/generic.validator';
import { TranslateService } from "@ngx-translate/core";
import { PaymentPeriodRegisterRequest } from "./dto/payment-period-register-request";
import { PaymentPeriodService } from "./payment-period.service";
import { PPDetailSearchByContractPeriodDTOTableStatus } from "../../shared/api/rentalapplication.services.client";
import { PaymentPeriodUpdateRequest } from './dto/payment-period-update-request';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { PaymentPeriodPopup } from './dto/payment-period-popup';

declare var $: any;


@Component({
    selector: 'at-payment-maintenance',
    templateUrl: './payment-maintenance.component.html'
})

export class PaymentMaintenanceComponent implements OnInit, OnDestroy {
    public paymentPeriodPopup: PaymentPeriodPopup;
    public gridDataDet: GridDataResult;
    public skipDet: number = 0;
    public buttonCount: number = 20;
    public info: boolean = true;
    public type: 'numeric' | 'input' = 'numeric';
    public pageSizes: any = [20, 50, 100, 200];
    public previousNext: boolean = true;
    public currentPage: number = 0;
    public countItemsDet: number = 0;
    isColumnHeaderSelected: boolean = true;

    paymentMaintenance: PPHeaderSearchByContractPeriodDTO;

    @Output() onCancelEvent = new EventEmitter<any>();
    @Output() onEditItem: EventEmitter<any> = new EventEmitter<any>();
    @Output() onAddItem: EventEmitter<any> = new EventEmitter<any>();


    public flgEdition: string;
    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;
    public openDialog: boolean = false;
    public openDialogMap: boolean = false;
    public openDialogHouseService: boolean = false;

    private genericValidator: GenericValidator;
    public displayMessage: { [key: string]: string; } = {};
    public validationMessages: { [key: string]: { [key: string]: string } } = {};

    paymentForm: FormGroup;
    _listPaymentTypes: any[];

    public paymentPeriodRegisterRequest: PaymentPeriodRegisterRequest;

    constructor(private route: ActivatedRoute,
        private paymentDataService: PaymentPeriodClient,
        private router: Router,
        private formBuilder: FormBuilder,
        private masterDataService: MasterDataService,
        private translate: TranslateService,
        private paymentPeriodService: PaymentPeriodService
        ) {
        this.paymentMaintenance = new PPHeaderSearchByContractPeriodDTO();


        Observable.forkJoin([
            this.translate.get('common.requiredField')
            
        ]).subscribe((messages: string[]) => this.buildMessages(...messages));
        this.genericValidator = new GenericValidator(this.validationMessages);
    }

    

    buildMessages(required?: string) {
        this.validationMessages = {
            paymentTypeId: {
                required: required
            },
            paymentAmount: { required: required}
        };
    }

    private showErrors(force = false) {
        this.displayMessage = this.genericValidator.processMessages(this.paymentForm, force);
    }

    public showDetailMaintenance= false;
    public addDetail() {
        this.showDetailMaintenance= true;
        this.paymentForm.get('paymentTypeId').setValidators(Validators.required);
        this.paymentForm.updateValueAndValidity();
    }

    public saveDetail() {
        if (!this.paymentForm.valid) {
            this.showErrors(true);
            return;
        }

        let paymentType = this._listPaymentTypes.filter(q=> q.typeId == this.paymentForm.get('paymentTypeId').value);
        let payment = this.paymentForm.getRawValue();
        let paymentDetail = new PaymentPeriodRegisterRequest();
        paymentDetail.contractId = this.paymentMaintenance.contractId;
        paymentDetail.periodId = this.paymentMaintenance.periodId;
        paymentDetail.paymentAmount = payment.paymentAmount;
        paymentDetail.paymentTypeId = payment.paymentTypeId;
        paymentDetail.referenceNo = payment.referenceNo;
        paymentDetail.comment = payment.comment;
        paymentDetail.tenantId = this.paymentMaintenance.tenantId;
        this.paymentPeriodService.registerPaymentDetail(paymentDetail)
        .subscribe(
            r=> {
                var dataResult: any = r;
                this.successFlag = dataResult.IsValid;
                if (!dataResult.IsValid)
                {
                    this.errorMessages = dataResult.Messages.length > 0 ? [{ message: dataResult.Messages[0].Message }] : dataResult.Messages;
                }
                else
                {
                    this.successMessage = 'Payment detail was inserted successfully!';;
                    this.errorMessages = null;
                }
            }
        )
        .add(
            r => {
                    this.getPaymentDetailByContract(this.paymentMaintenance.contractId, this.paymentMaintenance.periodId);
            }
        );
        this.showDetailMaintenance= this.paymentForm.get('continueRegistration').value;
    }

    closeDetail()
    {
        this.showDetailMaintenance = false;
    }


    addAccount(): void {
        this.paymentDataService.calculateOnAccountByContractAndPeriod(this.paymentMaintenance.periodId, this.paymentMaintenance.contractId, 1, 20)
            .subscribe(res => {
                var dataResult: any = res;
                if (dataResult.data != undefined) {
                    var id = this.paymentMaintenance.pPDetail.length * -1;
                    dataResult.data.paymentPeriodId = id;
                    this.paymentMaintenance.pPDetail.push(dataResult.data);
                }
            });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    sub: Subscription;

    ngOnInit() {
        this.paymentPeriodPopup = new PaymentPeriodPopup();
        this.buildForm();
        this.initialize();
        this.sub = this.route.params.subscribe(params => {
            let periodId= params['periodId'];
            let contractId = params['contractId'];

            if (periodId != null && typeof (periodId) != 'undefined' &&
                contractId != null && typeof (contractId) != 'undefined' ) {
                this.paymentDataService.searchCriteriaByContract(periodId, contractId, 1, 20)
                .subscribe(res => {
                    
                    let dataResult: any = res;
                    this.paymentMaintenance = dataResult.data;
                    this.countItemsDet = dataResult.data.pPDetail.length;
                    this.gridDataDet = {
                        data: dataResult.data.pPDetail,
                        total: dataResult.data.pPDetail.length
                    }
                })
                .add(r=> {
                    this.calculatePendingToPay();
                    this.verifyLateFeeMissing();
                });
                this.flgEdition = "E";
            } else {
                this.flgEdition = "N";
            }
        });

    }

    public buildForm()
    {
        this.paymentForm = this.formBuilder.group({
            paymentTypeId: [null, [Validators.required]],
            paymentAmount: [null, [Validators.required]],
            referenceNo: [null],
            comment: [null],
            continueRegistration: [true]
        });
    }

    initialize(){
        this.getPaymentTypes();
    }

    getPaymentTypes(): void {
        this.masterDataService.getGeneralTableByTableName('PaymentType')
            .subscribe(res => {
                let dataResult = new ResponseListDTO(res);
                let paymentTypeFiltered = dataResult.data.filter(q=> q.code !== 'RENT' && q.code !== 'DEPOSIT');
                this._listPaymentTypes = [];
                for (let i = 0; i < paymentTypeFiltered.length; i++) {
                    this._listPaymentTypes.push({
                        'typeId': paymentTypeFiltered[i].generalTableId,
                        'name': paymentTypeFiltered[i].value,
                        'code': paymentTypeFiltered[i].code
                    });
                }
            });
    }

    private getPaymentDetailByContract(contractId, periodId): void {
        this.paymentDataService.searchCriteriaByContract(periodId, contractId, 1, 20)
            .subscribe(res => {
                let dataResult: any = res;
                
                this.paymentMaintenance = dataResult.data;
                this.countItemsDet = dataResult.data.pPDetail.length;
                this.gridDataDet = {
                    data: dataResult.data.pPDetail,
                    total: dataResult.data.pPDetail.length
                }
                this.calculatePendingToPay();
            });
    }


    public verifyLateFeeMissing()
    {
        if (this.paymentMaintenance.lateFeeMissing!==null)
        {
            this.openedLateFeeConfimationPopup = true;
            this.confirmLateFeeMessage = "Se ha identificado pago de renta tardía por " + this.paymentMaintenance.lateFeeMissing.paymentAmount + ", deseas agregarlo al periodo?";
        }
        else {
            this.openedLateFeeConfimationPopup = false;
        }
    }

    public dataToPrint: any;

    public savePaymentDetail() {

        this.paymentMaintenance.totalAmount = this.paymentMaintenance.pendingTotal;
        this.paymentMaintenance.totalDeposit = this.paymentMaintenance.pendingDeposit;
        this.paymentMaintenance.totalRent = this.paymentMaintenance.pendingRent;
        this.paymentMaintenance.totalFine = this.paymentMaintenance.pendingFine;
        this.paymentMaintenance.totalLateFee = this.paymentMaintenance.pendingLateFee;
        this.paymentMaintenance.totalOnAcount = this.paymentMaintenance.pendingOnAccount;
        this.paymentMaintenance.totalService = this.paymentMaintenance.pendingService;
        this.calculatePendingToPay();

        this.paymentDataService.update(this.paymentMaintenance)
            .subscribe(res => {

                    var dataResult: any = res;
                    this.successFlag = dataResult.isValid;
                    this.errorMessages = dataResult.messages;
                    this.successMessage = 'Payment Detail was Updated';

                    if (this.successFlag) {
                        this.dataToPrint = this.paymentMaintenance;
                        this.getPaymentDetailByContract(this.paymentMaintenance.contractId, this.paymentMaintenance.periodId);
                    }
            });



    }


    onCancel() {
        this.router.navigateByUrl('amigotenant/payment');
    }

    public savePaymentStatus()
    {
        this.paymentForm.get('paymentTypeId').clearValidators();
        this.paymentForm.updateValueAndValidity();
        this.openedConfimationPopup = true;
    }

    onReset(): void {
    }

    //-------------------------------------------------------------------------
    //--------------------------    Maintenance     ---------------------------
    //-------------------------------------------------------------------------

    public isDetailVisible: boolean = false;

    public onEdit(data): void {
        this.isDetailVisible = true;
        this.setPaymentPeriodPopup(data);
        
    }

    public onClickCloseDialog(refreshGridAfterSaving: boolean) {
        this.openDialog = false;

        if (refreshGridAfterSaving) {
            //this.getFeaturesByHouse(this.paymentMaintenance.houseId); TODO: Create method to bring detail elements
        }
    }

    public onClickCloseDialogMap(refreshGridAfterSaving: boolean) {
        this.openDialogMap = false;
    }

    public onClickCloseHouseService(refreshGridAfterSaving: boolean) {
        this.openDialogHouseService = false;
    }


    //===========
    //DELETE
    //===========

    public deleteMessage: string = "Are you sure to delete this Payment?";
    paymentPeriodIdToDelete: any; 

    onDelete(entityToDelete) {
        this.paymentPeriodIdToDelete = entityToDelete.paymentPeriodId;
        this.openedDeletionConfimation = true;
    }

    yesDelete() {
        for (let i in this.paymentMaintenance.pPDetail) {
            if (this.paymentMaintenance.pPDetail[i].paymentPeriodId == this.paymentPeriodIdToDelete) {
                this.paymentMaintenance.pPDetail.splice(parseInt(i), 1);
                break;
            }
        }
        this.openedDeletionConfimation = false;
    }

    public openedDeletionConfimation: boolean = false;

    public closeDeletionConfirmation() {
        this.openedDeletionConfimation = false;
    }

    //PAGINATION
    public skip: number = 0;

    public pageChange1({ skip, take }: PageChangeEvent): void {
        this.currentPage = skip;
        let isExport: boolean = false;
    }

    public changeItemHeader() {
        let c = this.gridDataDet.data.length;
        let index = 0; 
        for (let item in this.gridDataDet.data) {

            if ($("#" + index)[0].disabled == false) {
                $("#" + index)[0].checked = this.isColumnHeaderSelected;
                this.gridDataDet.data[item].isSelected = this.isColumnHeaderSelected;
            }
            index++;
        }
        this.isColumnHeaderSelected = !this.isColumnHeaderSelected;
    }

    public changeItem(d) {
        d.isSelected = !d.isSelected;
        this.calculatePendingToPay();
    }

    public deselectColumnAll() {
        this.isColumnHeaderSelected = true;
        $("#HeaderTemplate")[0].checked = !this.isColumnHeaderSelected;
    }

    public calculatePendingToPay() {
        let pendingRows = this.gridDataDet.data.filter(q => q.paymentPeriodStatusCode === 'PPPENDING').length;
        let rowsSelected = 0;
        let totalDeposit = 0;
        let totalRent = 0;
        let totalFine = 0;
        let totalLateFee = 0;
        let totalService = 0;
        let totalOnAccount = 0;
        let total = 0;

        for (let item in this.gridDataDet.data) {
            if (this.gridDataDet.data[item].isSelected) {
                rowsSelected++;
            }

            if ((this.gridDataDet.data[item].isSelected == null && this.gridDataDet.data[item].isRequired && this.gridDataDet.data[item].paymentPeriodStatusCode == 'PPPENDING') ||
                (this.gridDataDet.data[item].isSelected && this.gridDataDet.data[item].paymentPeriodStatusCode == 'PPPENDING')) {
                if (this.gridDataDet.data[item].paymentTypeCode == 'DEPOSIT')
                    totalDeposit += this.gridDataDet.data[item].paymentAmount;
                if (this.gridDataDet.data[item].paymentTypeCode == 'RENT')
                    totalRent += this.gridDataDet.data[item].paymentAmount;
                if (this.gridDataDet.data[item].paymentTypeCode == 'FINE')
                    totalFine += this.gridDataDet.data[item].paymentAmount;
                if (this.gridDataDet.data[item].paymentTypeCode == 'SERVICE')
                    totalService += this.gridDataDet.data[item].paymentAmount;
                if (this.gridDataDet.data[item].paymentTypeCode == 'LATEFEE')
                    totalLateFee += this.gridDataDet.data[item].paymentAmount;
                if (this.gridDataDet.data[item].paymentTypeCode == 'ONACCOUNT')
                    totalOnAccount += this.gridDataDet.data[item].paymentAmount;
                total += this.gridDataDet.data[item].paymentAmount;
            }
        }
        this.paymentMaintenance.pendingDeposit = totalDeposit;
        this.paymentMaintenance.pendingRent = totalRent;
        this.paymentMaintenance.pendingFine = totalFine;
        this.paymentMaintenance.pendingLateFee = totalLateFee;
        this.paymentMaintenance.pendingService = totalService;
        this.paymentMaintenance.pendingOnAccount = totalOnAccount;
        this.paymentMaintenance.pendingTotal = total;
        this.paymentMaintenance.isPayInFull = rowsSelected === pendingRows;

    }

    setPaymentPeriodPopup(dataItem: any): void {
        this.paymentPeriodPopup = new PaymentPeriodPopup();
        this.paymentPeriodPopup.paymentPeriodId = dataItem.paymentPeriodId;
        this.paymentPeriodPopup.paymentAmount = dataItem.paymentAmount;
        this.paymentPeriodPopup.comment = dataItem.comment;
        this.paymentPeriodPopup.paymentTypeCode = dataItem.paymentTypeCode;
        this.paymentPeriodPopup.referenceNo = dataItem.reference;
        this.isDetailVisible = true;

    };

    closeDetailPopup(): void {
        this.isDetailVisible = false;
    }

    saveDetailPopup(data): void {
        let paymentDetail = new PaymentPeriodUpdateRequest();
        paymentDetail.paymentPeriodId = data.paymentPeriodId;
        paymentDetail.paymentAmount = data.paymentAmount;
        paymentDetail.referenceNo = data.reference;
        paymentDetail.comment = data.comment;
        this.paymentPeriodService.updatePaymentDetail(paymentDetail)
        .subscribe()
        .add(
            r => {
                    this.getPaymentDetailByContract(this.paymentMaintenance.contractId, this.paymentMaintenance.periodId);
            }
        );
        this.showDetailMaintenance= this.paymentForm.get('continueRegistration').value;
        this.closeDetailPopup();
        this.calculatePendingToPay();
    }

    addLatefee(): void {
        this.errorMessages = null;
        var existLateFee = this.paymentMaintenance.pPDetail.filter(q => q.paymentTypeCode == 'LATEFEE');
        if (existLateFee.length == 0) {

            this.paymentDataService.calculateLateFeeByContractAndPeriod(this.paymentMaintenance.periodId, this.paymentMaintenance.contractId, 1, 20)
                .subscribe(res => {
                    var dataResult: any = res;
                    if (dataResult.data != undefined) {
                        var id = this.paymentMaintenance.pPDetail.length * -1;
                        dataResult.data.paymentPeriodId = id;
                        this.paymentMaintenance.pPDetail.push(dataResult.data);
                    }
                    else
                    {
                        this.writeMessage(false, 'There is no Late fee to calculate');
                    }
                });
        } else {
            this.writeMessage(false, 'Late fee already exist');
        }
    }

    


    writeMessage(isValid, message): void {
        this.successFlag = isValid;
        if (isValid) {
            this.successMessage = message;
        } else {
            var arrMessages = [];
            var appMessage = new ApplicationMessage();
            appMessage.key = 'Error';
            appMessage.message = message;
            arrMessages.push(appMessage);
            this.errorMessages = arrMessages;
        }
        
    }


    //-------------------------------------------------------------------------
    //--------------------------    REPORT     ---------------------------
    //-------------------------------------------------------------------------

    public isReportVisible: boolean = false;

    closeReportPopup(): void {
        this.isReportVisible = false;
    }

    public dataLatestInvoiceId: any;

    onPrint(fileRepositoryId) {
        if (fileRepositoryId === '' || fileRepositoryId == undefined || fileRepositoryId == null) {
            this.writeMessage(false, 'There is no invoice to print');
            return;
        }
        this.paymentDataService.searchInvoiceById(fileRepositoryId);
    }




    //===========
    //SAVE
    //===========

    public confirmMessage: string = "Are you sure to save this Payment?";
    public openedConfimationPopup = false;

    public yesConfirm() {
        this.savePaymentDetail();
        this.openedConfimationPopup = false;
    }

    public closeConfirmation() {
        this.openedConfimationPopup = false;
    }

    //===========
    //ADDING LATEFEE
    //===========

    public confirmLateFeeMessage: string;
    public openedLateFeeConfimationPopup = false;

    public yesConfirmLateFee() {
        
        let payment = this.paymentMaintenance.lateFeeMissing;
        let paymentDetail = new PaymentPeriodRegisterRequest();
        paymentDetail.contractId = this.paymentMaintenance.contractId;
        paymentDetail.periodId = this.paymentMaintenance.periodId;
        paymentDetail.paymentAmount = payment.paymentAmount;
        paymentDetail.paymentTypeId = parseInt(payment.paymentTypeId);
        paymentDetail.referenceNo = payment.reference;
        paymentDetail.comment = payment.comment;
        paymentDetail.tenantId = this.paymentMaintenance.tenantId;
        paymentDetail.houseId = payment.houseId;
        this.paymentPeriodService.registerPaymentDetail(paymentDetail)
        .subscribe()
        .add(
            r => {
                    this.getPaymentDetailByContract(this.paymentMaintenance.contractId, this.paymentMaintenance.periodId);
            }
        );
        this.openedLateFeeConfimationPopup = false;
    }

    public closeConfirmationLateFee() {
        this.openedLateFeeConfimationPopup = false;
    }

}

