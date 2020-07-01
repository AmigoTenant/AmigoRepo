import { PaymentService } from './../../shared/api/payment.service';
import { Component, Input, Output, state, SimpleChange, ViewChild, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { Observable, Subscription } from 'rxjs'
import { Router, ActivatedRoute } from '@angular/router';

import { PaymentPeriodClient, PPHeaderSearchByContractPeriodDTO, PPDetailSearchByContractPeriodDTO, ApplicationMessage } from '../../shared/api/payment.services.client';
import { MasterDataService } from '../../shared/api/master-data-service';
import { ResponseListDTO } from '../../shared/dto/response-list-dto';
import { GenericValidator } from '../../shared/generic.validator';
import { TranslateService } from "@ngx-translate/core";
import { PaymentPeriodRegisterRequest } from "./dto/payment-period-register-request";
import { PaymentPeriodService } from "./payment-period.service";
import { PaymentPeriodUpdateRequest } from './dto/payment-period-update-request';
import { PaymentPeriodPopup } from './dto/payment-period-popup';
import { FileRepositorySearchRequest } from '../../shared/upload-file/file-repository-search.request';
import { EntityCode } from '../../shared/constants/enum';

declare var $: any;


@Component({
    selector: 'at-payment-liquitadion',
    templateUrl: './payment-liquidation.component.html'
})

export class PaymentLiquidationComponent implements OnInit, OnDestroy {
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

    totalPending = 0;

    openSendLiquidation= false;

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
            paymentAmount: { required: required },
            email: { required: required }
        };
    }

    private showErrors(force = false) {
        this.displayMessage = this.genericValidator.processMessages(this.paymentForm, force);
    }

    public showDetailMaintenance = false;
    public addDetail() {
        this.showDetailMaintenance = true;
        this.paymentForm.get('paymentTypeId').setValidators(Validators.required);
        this.paymentForm.updateValueAndValidity();
    }

    public saveDetail() {
        if (!this.paymentForm.valid) {
            this.showErrors(true);
            return;
        }

        let paymentType = this._listPaymentTypes.filter(q => q.typeId == this.paymentForm.get('paymentTypeId').value);
        let payment = this.paymentForm.getRawValue();
        let paymentDetail = new PaymentPeriodRegisterRequest();
        paymentDetail.contractId = this.paymentMaintenance.contractId;
        paymentDetail.periodId = this.paymentMaintenance.periodId;
        paymentDetail.paymentAmount = payment.paymentAmount;
        paymentDetail.paymentTypeId = payment.paymentTypeId;
        paymentDetail.referenceNo = payment.referenceNo;
        paymentDetail.comment = payment.comment;
        paymentDetail.tenantId = this.paymentMaintenance.tenantId;
        paymentDetail.houseId = this.paymentMaintenance.houseId;
        this.paymentPeriodService.registerPaymentDetail(paymentDetail)
            .subscribe(
                r => {
                    var dataResult: any = r;
                    this.successFlag = dataResult.IsValid;
                    if (!dataResult.IsValid) {
                        this.errorMessages = dataResult.Messages.length > 0 ? [{ message: dataResult.Messages[0].Message }] : dataResult.Messages;
                    }
                    else {
                        this.successMessage = 'Payment detail was inserted successfully!';;
                        this.errorMessages = null;
                    }
                }
            )
            .add(
                r => {
                    this.getPaymentDetailForLiquidation(this.paymentMaintenance.contractId, this.paymentMaintenance.periodId);
                }
            );
        this.showDetailMaintenance = this.paymentForm.get('continueRegistration').value;
    }

    closeDetail() {
        this.showDetailMaintenance = false;
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    sub: Subscription;
    fileRepositorySearchRequest = new FileRepositorySearchRequest();
    parentId: string;

    ngOnInit() {
        this.paymentPeriodPopup = new PaymentPeriodPopup();
        this.fileRepositorySearchRequest.entityCode = 'PY';
        this.buildForm();
        this.initialize();
        this.sub = this.route.params.subscribe(params => {
            let periodId = params['periodId'];
            let contractId = params['contractId'];
            let invoiceId = params['invoiceId'];
            if (invoiceId != null && invoiceId != 0 && typeof (invoiceId) != 'undefined') {
                this.paymentDataService.searchCriteriaByContract(null, null, invoiceId, 1, 20)
                    .subscribe(res => {
                        let dataResult: any = res;
                        this.paymentMaintenance = dataResult.data;
                        this.fileRepositorySearchRequest.parentIds = this.paymentMaintenance.pPDetail.map(q => q.paymentPeriodId);
                        this.parentId = null;
                        this.countItemsDet = dataResult.data.pPDetail.length;
                        this.gridDataDet = {
                            data: dataResult.data.pPDetail,
                            total: dataResult.data.pPDetail.length
                        }
                    })
                    .add(r => {
                        this.calculatePendingToPay();
                    });
                this.flgEdition = "E";
            }
            else {
                if (periodId != null && typeof (periodId) != 'undefined' &&
                contractId != null && typeof (contractId) != 'undefined') {
                this.paymentDataService.searchForLiquidation(periodId, contractId, 1, 20)
                    .subscribe(res => {
                        let dataResult: any = res;
                        this.paymentMaintenance = dataResult.data;
                        this.fileRepositorySearchRequest.parentIds = this.paymentMaintenance.pPDetail.map(q => q.paymentPeriodId);
                        this.parentId = null;
                        this.countItemsDet = dataResult.data.pPDetail.length;
                        this.gridDataDet = {
                            data: dataResult.data.pPDetail,
                            total: dataResult.data.pPDetail.length
                        }
                    })
                    .add(r => {
                        this.calculatePendingToPay();
                        //this.verifyLateFeeMissing();
                    });
                this.flgEdition = "E";
                }
                else {
                    this.flgEdition = "N";
                }
            }
        });

    }

    public buildForm() {
        this.paymentForm = this.formBuilder.group({
            paymentTypeId: [null, [Validators.required]],
            paymentAmount: [null, [Validators.required]],
            referenceNo: [null],
            comment: [null],
            continueRegistration: [true],
            email: [null, [Validators.required]]
        });
    }

    initialize() {
        this.getPaymentTypes();
    }

    getPaymentTypes(): void {
        this.masterDataService.getGeneralTableByTableName('PaymentType')
            .subscribe(res => {
                let dataResult = new ResponseListDTO(res);
                let paymentTypeFiltered = dataResult.data.filter(q => q.code !== 'RENT' && q.code !== 'DEPOSIT');
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

    private getPaymentDetailForLiquidation(contractId, periodId): void {
        this.paymentDataService.searchForLiquidation(periodId, contractId, 1, 20)
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

    private getPaymentDetailByInvoiceId(invoiceId: number): void {
        this.paymentDataService.searchCriteriaByContract(null, null, invoiceId, 1, 20)
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

    public dataToPrint: any;

    public savePaymentDetailonHeader() {

        this.paymentMaintenance.totalAmount = this.paymentMaintenance.pendingTotal;
        this.paymentMaintenance.totalDeposit = this.paymentMaintenance.pendingDeposit;
        this.paymentMaintenance.totalRent = this.paymentMaintenance.pendingRent;
        this.paymentMaintenance.totalFine = this.paymentMaintenance.pendingFine;
        this.paymentMaintenance.totalLateFee = this.paymentMaintenance.pendingLateFee;
        this.paymentMaintenance.totalOnAcount = this.paymentMaintenance.pendingOnAccount;
        this.paymentMaintenance.totalService = this.paymentMaintenance.pendingService;
        this.paymentMaintenance.isLiquidating = true;
        this.calculatePendingToPay();

        this.paymentDataService.update(this.paymentMaintenance)
            .subscribe(res => {
                var dataResult: any = res;
                this.successFlag = dataResult.isValid;
                this.errorMessages = dataResult.messages;
                this.successMessage = 'Payment Detail was Updated';

                if (this.successFlag) {
                    this.dataToPrint = this.paymentMaintenance;
                    this.getPaymentDetailByInvoiceId(dataResult.pk);
                }
            });
    }


    onCancel() {
        this.router.navigateByUrl('amigotenant/payment');
    }

    public savePaymentStatus() {
        this.paymentForm.get('paymentTypeId').clearValidators();
        this.paymentForm.updateValueAndValidity();
        this.openedConfimationPopup = true;
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


    public calculatePendingToPay() {
        let pendingRows = this.gridDataDet.data.filter(q => q.paymentPeriodStatusCode === 'PPPAYED').length;
        let paymentAmountTotal = 0;

        for (let item in this.gridDataDet.data) {
            paymentAmountTotal += this.gridDataDet.data[item].paymentAmount;
        }
        this.totalPending = paymentAmountTotal
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
                    this.getPaymentDetailForLiquidation(this.paymentMaintenance.contractId, this.paymentMaintenance.periodId);
                }
            );
        this.showDetailMaintenance = this.paymentForm.get('continueRegistration').value;
        this.closeDetailPopup();
        this.calculatePendingToPay();
    }

    // addLatefee(): void {
    //     this.errorMessages = null;
    //     var existLateFee = this.paymentMaintenance.pPDetail.filter(q => q.paymentTypeCode == 'LATEFEE');
    //     if (existLateFee.length == 0) {

    //         this.paymentDataService.calculateLateFeeByContractAndPeriod(this.paymentMaintenance.periodId, this.paymentMaintenance.contractId, 1, 20)
    //             .subscribe(res => {
    //                 var dataResult: any = res;
    //                 if (dataResult.data != undefined) {
    //                     var id = this.paymentMaintenance.pPDetail.length * -1;
    //                     dataResult.data.paymentPeriodId = id;
    //                     this.paymentMaintenance.pPDetail.push(dataResult.data);
    //                 }
    //                 else
    //                 {
    //                     this.writeMessage(false, 'There is no Late fee to calculate');
    //                 }
    //             });
    //     } else {
    //         this.writeMessage(false, 'Late fee already exist');
    //     }
    // }

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

    // public isReportVisible: boolean = false;

    // closeReportPopup(): void {
    //     this.isReportVisible = false;
    // }

    // public dataLatestInvoiceId: any;

    onPrint(fileRepositoryId) {
        if (fileRepositoryId === '' || fileRepositoryId == undefined || fileRepositoryId == null) {
            this.writeMessage(false, 'There is no invoice to print');
            return;
        }
        this.paymentDataService.searchInvoiceById(fileRepositoryId);
    }


    //===========
    //SAVE HEADER PAYMENT
    //===========

    public confirmMessage: string = "Are you sure to save this Payment?";
    public openedConfimationPopup = false;

    public yesConfirm() {
        this.savePaymentDetailonHeader();
        this.openedConfimationPopup = false;
    }

    public closeConfirmation() {
        this.openedConfimationPopup = false;
    }

    //===========
    /*SEND EMAIL INFORMACION DE LIQUIDATION */
    //===========

    public onSendLiquidation(){
        this.openSendLiquidation = true;
    }

    public yesConfirmSendLiquidation(){

        this.paymentDataService.sendEmailAboutLiquidation(this.paymentMaintenance.periodId, this.paymentMaintenance.contractId)
        .subscribe(res => {
        });
    }

    public closeSendLiquidation(){
        this.openSendLiquidation = false;
    }


}
