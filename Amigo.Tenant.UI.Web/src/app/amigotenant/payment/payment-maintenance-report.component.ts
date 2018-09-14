import { Component, Input, Output, state, SimpleChange, ViewChild, EventEmitter, OnInit} from '@angular/core';
import { Http, Jsonp, URLSearchParams } from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { BotonsComponent } from '../../controls/common/boton.component';
import { ValidationService } from '../../shared/validations/validation.service';
import { Observable, Subscription } from 'rxjs'
import { Router, ActivatedRoute } from '@angular/router';

import { PaymentPeriodClient, ApplicationMessage, PPHeaderSearchByInvoiceDTO } from '../../shared/api/payment.services.client';

declare var $: any;


@Component({
    selector: 'at-payment-maintenance-report',
    templateUrl: './payment-maintenance-report.component.html'
})

export class PaymentMaintenanceReportComponent implements OnInit {
    public gridDataDet: GridDataResult;
    @Input() inputInvoiceId: any;

    public flgEdition: string;
    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;
    constructor(private route: ActivatedRoute,
        private paymentDataService: PaymentPeriodClient,
        private router: Router) {
    }

    ngOnInit() {
        var invoiceId = this.inputInvoiceId;
        this.getPaymentDetailByContract(invoiceId);
    }

    public invoiceHeaderDTO: PPHeaderSearchByInvoiceDTO = new PPHeaderSearchByInvoiceDTO();
    public invoiceDetailDTO: PPHeaderSearchByInvoiceDTO[]= [];

    private getPaymentDetailByContract(invoiceId): void {
        this.paymentDataService.searchInvoiceById(invoiceId);
    }

   
}

