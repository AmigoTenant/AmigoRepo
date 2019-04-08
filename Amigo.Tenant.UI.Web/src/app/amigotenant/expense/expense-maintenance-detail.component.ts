import { TranslateService } from '@ngx-translate/core';
import { ExpenseDataService } from './expense-data.service';
import { ExpenseRegisterRequest } from './dto/expense-register-request';
import { Component, EventEmitter, OnInit, OnDestroy, ElementRef, ViewChildren, Input, Output } from '@angular/core';
import { Http, Jsonp, URLSearchParams } from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule, Form, FormControlName } from '@angular/forms';
import { EntityStatusDTO, HouseDTO } from './../../shared/api/services.client';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { ConfirmationList, Confirmation } from '../../model/confirmation.dto';
import { ListsService } from '../../shared/constants/lists.service';
import { EntityStatusClient, GeneralTableClient } from '../../shared/api/services.client';
import { EnvironmentComponent } from '../../shared/common/environment.component';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs'
import { ValidationService } from '../../shared/validations/validation.service';
import { ResponseListDTO } from '../../shared/dto/response-list-dto';
import { MasterDataService } from '../../shared/api/master-data-service';
import { GenericValidator } from '../../shared/generic.validator';
import { ExpenseDetailRegisterRequest } from './dto/expense-detail-register-request';

declare var $: any;

@Component({
    selector: 'at-expense-maintenance-detail',
    templateUrl: './expense-maintenance-detail.component.html'
})

export class ExpenseMaintenanceDetailComponent extends EnvironmentComponent implements OnInit {
    private genericValidator: GenericValidator;
    private validationSubscription: Subscription;
    public validationMessages: { [key: string]: { [key: string]: string } } = {};
    public displayMessage: { [key: string]: string; } = {};
    @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];
    @Input() inputSelectedExpenseDetail: ExpenseDetailRegisterRequest;
    @Output() eventoClose = new EventEmitter<any>();
    @Input() inputPaymentTypeId: number;

    _listConcepts: any[];
    isColumnHeaderSelected = true;
    message: string;

    model: ExpenseDetailRegisterRequest;
    public modelExpenseDate: any;
    allowEditing = true;
    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;

    expenseDetailForm: FormGroup;

    flgEdition: string;
    sub: Subscription;
    contractId: any;

    public showTenant = false;


    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private gnrlTableDataService: GeneralTableClient,
        private expenseDataService: ExpenseDataService,
        private formBuilder: FormBuilder,
        private translate: TranslateService,
        private masterDataService: MasterDataService
    ) {
        super();
    }

    ngOnInit() {
        this.buildForm();
        this.buildValidator();
        this.initializeForm();
        this.expenseDetailForm.patchValue(this.inputSelectedExpenseDetail);

        if (this.inputSelectedExpenseDetail === undefined || this.inputSelectedExpenseDetail === null ||
            this.inputSelectedExpenseDetail.expenseDetailId === undefined) {
            this.flgEdition = 'N';
        } else {
            this.flgEdition = 'E';
            this._currentTenant =   new CurrentTenant();
            this._currentTenant.tenantId = this.inputSelectedExpenseDetail.tenantId;
            this._currentTenant.fullName = this.inputSelectedExpenseDetail.tenantFullName;
            this.onApplyValidator()
        }
    }

    buildValidator() {
        Observable.forkJoin([
            this.translate.get('common.requiredField'),
            this.translate.get('common.maxLength', { value: 500 }),
            this.translate.get('common.notValidFormat')
        ]).subscribe((messages: string[]) => this.buildMessages(...messages));
        this.genericValidator = new GenericValidator(this.validationMessages);
    }

    buildMessages(required?: string, maxlength?: string, notvalid?: string) {
        this.validationMessages = {
            conceptId: {
                required: required
            },
            applyTo: {
                required: required
            },
            subTotalAmount: {
                required: required
            },
            tax: {
                required: required
            },
            totalAmount: {
                required: required
            },
            tenantId: {
                required: required
            }
        };
    }


    buildForm() {
        this.expenseDetailForm = this.formBuilder.group({
            expenseDetailId: [null],
            conceptId: [null, [Validators.required]],
            quantity: [1, [Validators.required]],
            subTotalAmount: [null, [Validators.required]],
            applyTo: [null, [Validators.required]],
            tenantId: [null, [Validators.required]],
            remark: [null],
            tax: [null, [Validators.required]],
            totalAmount: [null, [Validators.required]],
            expenseId: [null],
            expenseDetailStatusId: [null]
        });
    }

    getExpenseDetailByExpenseDetailId(id): void {
        this.expenseDataService.getExpenseDetailByExpenseDetailId(id).subscribe(
            response => {
                debugger;
                let dataResult: any = new ResponseListDTO(response);
                this.model = dataResult.data;
                this.expenseDetailForm.patchValue(this.model);
            });
    }

    initializeForm(): void {
        this.getConceptByTypes();
        this.getApplyTo();
    }


    //=========== 
    //INSERT
    //===========

    acceptDetail(): void {
        debugger;
        //this.setRutValidators();
        if (!this.expenseDetailForm.valid) {
            this.showErrors(true);
            return;
        }
        this.displayMessage = {};

        const model = new ExpenseDetailRegisterRequest();
        Object.assign(model, this.expenseDetailForm.value);

        if (this.flgEdition === 'N') {
            this.expenseDataService.saveExpenseDetail(model)
                .subscribe(r => {
                    let result = new ResponseListDTO(r);
                }).add(r => {
                    this.onApplyValidator();
                    this.onCancelDetail();
                });

        } else {
            //UPDATE
            this.expenseDataService.updateExpenseDetail(model)
                .subscribe(r => {
                    let result = new ResponseListDTO(r);
                }).add(r => {
                    this.onCancelDetail();
                });
        }

    }

    ////===========
    ////DELETE
    ////===========

    //public deleteMessage: string = "Are you sure to delete this Lease?";
    //deviceToDelete: any; //DeleteDeviceRequest;

    //onDelete(deviceToDelete) {

    //    // this.deviceToDelete = new DeleteDeviceRequest();
    //    // this.deviceToDelete.deviceId = deviceToDelete.deviceId;
    //    // if (deviceToDelete.assignedAmigoTenantTUserId != null && deviceToDelete.assignedAmigoTenantTUserId > 0)
    //    //     this.deleteMessage = "There is an assigned driver, delete Device?";
    //    // else
    //        //this.deleteMessage = "Delete Device?";
    //    this.openedDeletionConfimation = true;
    //}

    //yesDelete() {
    //    // this.deviceDataService.delete(this.deviceToDelete)
    //    //     .subscribe(response => {
    //    //         this.onReset()
    //    //         this.openedDeletionConfimation = false;
    //    //     });
    //}

    //public openedDeletionConfimation: boolean = false;

    //public closeDeletionConfirmation() {
    //    this.openedDeletionConfimation = false;
    //}

    onExecuteEvent($event) {
        switch ($event) {
            case 's':
                this.acceptDetail();
                break;
            case 'c':
                break;
            case 'k':
                this.onCancelDetail();
                break;
            default:
                confirm('Sorry, that Event is not exists yet!');
        }
    }

    private showErrors(force = false) {
        this.displayMessage = this.genericValidator.processMessages(this.expenseDetailForm, force);
    }

    _currentTenant: any;

    getTenant = (item) => {
        if (item != null && item != undefined) {
            //this.model.tenantId = item.tenantId;
            //this.model.fullName = item.fullName;
            this._currentTenant = item;
            this.expenseDetailForm.get('tenantId').setValue(item.tenantId);
        }
        else {
            //this.model.tenantId = 0;
            //this.model.fullName = item.username;
            this._currentTenant = undefined;
            this.expenseDetailForm.get('tenantId').setValue(null);
        }
    };

    _listApplyTo: any[];
    getApplyTo(): void {
        this.gnrlTableDataService.getGeneralTableByTableNameAsync("ApplyTo")
            .subscribe(res => {

                var dataResult: any = res;
                this._listApplyTo = [];
                for (var i = 0; i < dataResult.value.data.length; i++) {
                    this._listApplyTo.push({
                        "applyToId": dataResult.value.data[i].generalTableId,
                        "name": dataResult.value.data[i].value
                    });
                }
            });
    }

    getConceptByTypes(): void {
        this.masterDataService.getConceptsByTypeIdList([this.inputPaymentTypeId])
            .subscribe(res => {
                let dataResult = new ResponseListDTO(res);
                this._listConcepts = dataResult.data;
            });
    }

    onCancelDetail() {
        this.eventoClose.emit(this.expenseDetailForm.get('expenseId').value);
    }

    onApplyValidator() {
        // Aplicar Validator solo para Tenant
        // 66: All Tenant
        // 64: Period
        if (this.expenseDetailForm.get('applyTo').value === 66 || this.expenseDetailForm.get('applyTo').value === 64) {
            this.showTenant = false;
            this.clearTenantIdValidator();
        } else {
            this.showTenant = true;
            this.assignTenantIdValidator();
        }
    }

    assignTenantIdValidator() {
        const tenantId = this.expenseDetailForm.get('tenantId');
        tenantId.setValidators(Validators.required);
        tenantId.updateValueAndValidity();
    }

    clearTenantIdValidator() {
        const tenantId = this.expenseDetailForm.get('tenantId');
        tenantId.clearValidators();
        tenantId.updateValueAndValidity();
    }

}

export class CurrentTenant {
    tenantId: number;
    fullName: string;
}
