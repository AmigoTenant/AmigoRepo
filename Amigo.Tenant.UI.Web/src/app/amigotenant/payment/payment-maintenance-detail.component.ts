import { Component, Input, Output, state, SimpleChange, EventEmitter, OnInit} from '@angular/core';
import { Http, Jsonp, URLSearchParams } from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { BotonsComponent } from '../../controls/common/boton.component';

import { ValidationService } from '../../shared/validations/validation.service';
import { Observable, Subscription } from 'rxjs'
import { Router, ActivatedRoute } from '@angular/router';
import { PaymentPeriodClient, PPDetailSearchByContractPeriodDTO } from '../../shared/api/payment.services.client';

declare var $: any;

@Component({
    selector: 'at-payment-maintenance-detail',
    templateUrl: './payment-maintenance-detail.component.html'
})

export class PaymentMaintenanceDetailComponent implements OnInit{

    paymentMaintenanceDetail: PPDetailSearchByContractPeriodDTO;

    constructor(private route: ActivatedRoute,
        private paymentDataService: PaymentPeriodClient,
        private router: Router) {
        this.paymentMaintenanceDetail = new PPDetailSearchByContractPeriodDTO();
    }

    @Input() inputDataDetail: any;

    ngOnInit() {
        if (this.inputDataDetail != null && this.inputDataDetail.paymentTypeCode != undefined) {
            this.paymentMaintenanceDetail.paymentPeriodId = this.inputDataDetail.paymentPeriodId;
            this.paymentMaintenanceDetail.comment = this.inputDataDetail.comment;
            this.paymentMaintenanceDetail.paymentTypeCode = this.inputDataDetail.paymentTypeCode;
            this.paymentMaintenanceDetail.paymentAmount = this.inputDataDetail.paymentAmount;
        }
    }

    public savePaymentDetail() {
        //this.paymentDataService.update(this.paymentMaintenance)
        //        .subscribe(res => {
        //            var dataResult: any = res;
        //            this.successFlag = dataResult.isValid;
        //            this.errorMessages = dataResult.messages;
        //            this.successMessage = 'Payment Detail was Updated';
        //            if (this.successFlag) {
        //                this.getPaymentDetailByContract(this.paymentMaintenance.contractId, this.paymentMaintenance.periodId);
        //            }
        //        });
    }

    //This Event came from BotonComponent
    onExecuteEvent($event) {
        switch ($event) {
            case "s":
                this.onSave();
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

    @Output() saveEvent = new EventEmitter<any>();

    onSave(): void {
        this.saveEvent.emit(this.paymentMaintenanceDetail);
    }

    @Output() cancelEvent = new EventEmitter<any>();

    onCancel(): void {
        this.cancelEvent.emit(false);
    }
}

