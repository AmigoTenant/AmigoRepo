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

declare var $: any;

export class dataDetailClass {
    public paymentPeriodId: string;
    public paymentTypeCode: string;
    public comment: string;
    public paymentAmount: number;
}

@Component({
    selector: 'at-payment-maintenance',
    templateUrl: './payment-maintenance.component.html'
})

export class PaymentMaintenanceComponent implements OnInit, OnDestroy {
    public dataDetail: dataDetailClass;
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
    }

    public saveDetail() {
        if (!this.paymentForm.valid) {
            this.showErrors(true);
            return;
        }

        let paymentType = this._listPaymentTypes.filter(q=> q.typeId == this.paymentForm.get('paymentTypeId').value);
        let payment = this.paymentForm.getRawValue();
        let paymentDetail = new PPDetailSearchByContractPeriodDTO();
        paymentDetail.paymentPeriodId = -2;
        paymentDetail.contractId = this.paymentMaintenance.contractId;
        paymentDetail.periodId = this.paymentMaintenance.periodId;
        paymentDetail.paymentAmount = payment.paymentAmount;
        paymentDetail.paymentTypeId = payment.paymentTypeId;
        paymentDetail.paymentTypeValue = paymentType.length>0? paymentType[0].name: "";
        paymentDetail.paymentTypeCode = paymentType.length>0? paymentType[0].code: "";
        paymentDetail.paymentTypeName = paymentType.length>0? paymentType[0].name: "";
        paymentDetail.paymentPeriodStatusCode = 'PPPENDING'; //TODO: Cambiar
        paymentDetail.paymentPeriodStatusId = 1; //TODO: Cambiar
        paymentDetail.isRequired = false;
        paymentDetail.isSelected = false;
        paymentDetail.tableStatus = PPDetailSearchByContractPeriodDTOTableStatus._0; //aDDING
        paymentDetail.paymentDescription = payment.comment;
        //paymentDetail.conceptId = concept.Data.ConceptId;
        paymentDetail.tenantId = this.paymentMaintenance.tenantId;
        paymentDetail.paymentPeriodStatusName = 'PENDING'; //TODO: Cambiar

        var id = this.paymentMaintenance.pPDetail.length * -1;
        paymentDetail.paymentPeriodId = id;
        this.paymentMaintenance.pPDetail.push(paymentDetail);

        //if (!this.paymentForm.get('continueRegistration').value)
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
        this.dataDetail = new dataDetailClass();
        this.buildForm();
        this.initialize();
        this.sub = this.route.params.subscribe(params => {
            let periodId= params['periodId'];
            let contractId = params['contractId'];

            if (periodId != null && typeof (periodId) != 'undefined' &&
                contractId != null && typeof (contractId) != 'undefined' ) {
                this.getPaymentDetailByContract(contractId, periodId);
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
                this._listPaymentTypes = [];
                for (let i = 0; i < dataResult.data.length; i++) {
                    this._listPaymentTypes.push({
                        'typeId': dataResult.data[i].generalTableId,
                        'name': dataResult.data[i].value,
                        'code': dataResult.data[i].code
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

    //This Event came from BotonComponent
    onExecuteEvent($event) {
        switch ($event) {
            case "s":
                //this.savePaymentDetail();
                this.openedConfimationPopup = true;
                break;
            case "c":
                //this.onClear();
                break;
            case "k":
                this.onCancel();
                break;
            default:
                confirm("Sorry, that Event does not exists yet!");
        }
    }

    onReset(): void {
    }

    //-------------------------------------------------------------------------
    //--------------------------    Maintenance     ---------------------------
    //-------------------------------------------------------------------------

    public isDetailVisible: boolean = false;

    public onEdit(data): void {
        this.isDetailVisible = true;
        this.getCostCenterToUpdate(data);
        
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

    getCostCenterToUpdate(costCenterDTO: any): void {
        this.dataDetail = new dataDetailClass();
        this.dataDetail.paymentPeriodId = costCenterDTO.paymentPeriodId;
        this.dataDetail.paymentAmount = costCenterDTO.paymentAmount;
        this.dataDetail.comment = costCenterDTO.comment;
        this.dataDetail.paymentTypeCode = costCenterDTO.paymentTypeCode;
        this.isDetailVisible = true;

    };

    closeDetailPopup(): void {
        this.isDetailVisible = false;
    }

    saveDetailPopup(data): void {
        let paymentperiod: PPDetailSearchByContractPeriodDTO[];
        paymentperiod = this.gridDataDet.data.filter(q => q.paymentPeriodId === data.paymentPeriodId);
        if (paymentperiod.length > 0) {
            if (paymentperiod[0].paymentAmount != Number(data.paymentAmount)
                || paymentperiod[0].comment != data.comment) {
                paymentperiod[0].paymentAmount = Number(data.paymentAmount);
                paymentperiod[0].comment = data.comment;
                if (paymentperiod[0].paymentPeriodId > 0) {
                    paymentperiod[0].tableStatus = 2; //Modified
                }
            }
        }
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

    onPrint(invoiceNo) {
        if (invoiceNo === '' || invoiceNo == undefined || invoiceNo == null) {
            this.writeMessage(false, 'There is no invoice to print');
            return;
        }
        this.paymentDataService.searchInvoiceById(invoiceNo);
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

}

