import { TranslateService } from '@ngx-translate/core';
import { ExpenseDataService } from './expense-data.service';
import { ExpenseRegisterRequest } from './dto/expense-register-request';
import { Component, AfterViewInit, EventEmitter, OnInit, OnDestroy, ElementRef, ViewChildren } from '@angular/core';
import { Http, Jsonp, URLSearchParams } from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule, Form, FormControlName } from '@angular/forms';
import { EntityStatusDTO, HouseDTO } from './../../shared/api/services.client';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { ConfirmationList, Confirmation } from '../../model/confirmation.dto';
import { ListsService } from '../../shared/constants/lists.service';
import { EntityStatusClient, GeneralTableClient, HouseClient } from '../../shared/api/services.client';
import { EnvironmentComponent } from '../../shared/common/environment.component';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs'
import { ValidationService } from '../../shared/validations/validation.service';
import { ResponseListDTO } from '../../shared/dto/response-list-dto';
import { MasterDataService } from '../../shared/api/master-data-service';
import { GenericValidator } from '../../shared/generic.validator';
import { ExpenseDetailRegisterRequest } from './dto/expense-detail-register-request';
import { DetailAmountsDto } from './dto/detail-amounts-dto';


declare var $: any;

export class modelDate {
    year: number;
    month: number;
    day: number;
}

export class modelTenant {
    tenantId: number;
    fullName: string;
}

@Component({
    selector: 'at-expense-maintenance',
    templateUrl: './expense-maintenance.component.html'
})

export class ExpenseMaintenanceComponent extends EnvironmentComponent implements OnInit, OnDestroy, AfterViewInit {
    private genericValidator: GenericValidator;
    private validationSubscription: Subscription;
    public validationMessages: { [key: string]: { [key: string]: string } } = {};
    public displayMessage: { [key: string]: string; } = {};
    @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];
    public difTotalAmount?: number = null;
    public difSubTotalAmount?: number = null;
    public difTax?: number = null;


    model: ExpenseRegisterRequest;
    expenseDetailData: any[];

    allowEditing = true;
    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;

    expenseForm: FormGroup;

    _listPaymentTypes: any = [];
    _listStatus: any = [];
    _currentHouse: any;
    _currentPeriod: any;
    _listHouses: any = [];
    _listPeriods: any = [];

    flgEdition: string;
    _isDisabled: boolean;

    public isPeriodDisabled = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private listConfirmation: ConfirmationList,
        private listsService: ListsService,
        private gnrlTableDataService: GeneralTableClient,
        private expenseDataService: ExpenseDataService,
        private masterDataService: MasterDataService,
        private formBuilder: FormBuilder,
        private translate: TranslateService,
        private houseClient: HouseClient
    ) {
        super();

        Observable.forkJoin([
            this.translate.get('common.requiredField'),
            this.translate.get('common.alphadashmax12'),
            this.translate.get('common.maxLength', { value: 50 })
        ]).subscribe((messages: string[]) => this.buildMessages(...messages));
        this.genericValidator = new GenericValidator(this.validationMessages);
    }

    buildMessages(required?: string, notvalid?: string, maxlength?: string) {
        this.validationMessages = {
            expenseDate: {
                required: required
            },
            // houseId: {
            //     required: required
            // },
            periodId: {
                required: required
            },
            houseTypeId: {
                required: required
            },
            paymentTypeId: {
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
            }
        };
    }


    sub: Subscription;
    contractId: any;

    ngOnInit() {
        debugger; //Inicializacion del Maintenance header
        this.model = new ExpenseRegisterRequest();
        this.buildForm();
        this.initializeForm();
        this.sub = this.route.params.subscribe(params => {

            //TODO:
            let id = params['expenseId'];
            if (id != null && typeof (id) !== 'undefined') {
                this.getExpenseById(id);
                this.flgEdition = 'E';
                this._isDisabled = false;

            } else {
                this.flgEdition = 'N';
                this._isDisabled = false;
            }

        });


    }

    buildForm() {
        this.expenseForm = this.formBuilder.group({
            expenseId: [null],
            expenseDate: [null, [Validators.required]],
            houseId: [null],
            periodId: [null, [Validators.required]],
            paymentTypeId: [null, [Validators.required]],
            remark: [null],
            subTotalAmount: [null, [Validators.required]],
            tax: [null, [Validators.required]],
            totalAmount: [null, [Validators.required]],
            referenceNo: [null]
        });
    }

    getExpenseById(id): void {
        this.expenseDataService.getById(id).subscribe(
            response => {
                let dataResult: any = new ResponseListDTO(response);
                this.model = dataResult.dat;
                this.expenseForm.patchValue(this.model);
                this.getDateFromModel(this.model.expenseDate);
            }).add(
                r   => {
                    this.getDetail()
                }
            );
    }

    public getDateFromModel(dateFromModel: Date): void {
        let model = new modelDate();
        if (dateFromModel != null && dateFromModel !== undefined) {
            const expenseDate = new Date(dateFromModel);
            model.day = expenseDate.getDate();
            model.month = expenseDate.getMonth() + 1;
            model.year = expenseDate.getFullYear();
        } else {
            model = null;
        }
        this.expenseForm.get('expenseDate').setValue(model);
    }

    initializeForm(): void {
        this.getPaymentTypes();
        this.getHouseAll();
        this.getPeriodsNumberPeriod(10);
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


    onCancel(): void {
        this.router.navigate(['amigotenant/expense']);
    }

    // onExecuteEvent($event) {
    //     switch ($event) {
    //         case 's':
    //             this.onSave();
    //             break;
    //         case 'c':
    //             //this.onClear();
    //             break;
    //         case 'k':
    //             this.onCancel();
    //             break;
    //         default:
    //             confirm('Sorry, that Event is not exists yet!');
    //     }
    // }

    // isValidData(): boolean {
    //     let isValid = true;
    //     //this.resetFormError();
    //     //if (this.model.tenantId == undefined || this.model.tenantId == 0 || this.model.tenantId == null) {
    //     //    this._formError.tenantError = true;
    //     //    isValid = false;
    //     //}
    //     return isValid;
    // }


    getPeriod = (item) => {
        if (item !== null && item !== undefined && item !== '') {
            this.model.periodId = item.periodId;
            this._currentPeriod = item;
        } else {
            this.model.periodId = undefined;
            this._currentPeriod = undefined;
        }
    };

    getHouse = (item) => {
        if (item !== null && item !== undefined && item !== '') {
            this._currentHouse = item;
            this.expenseForm.patchValue({ 'houseId': item.houseId })
        } else {
            this._currentHouse = undefined;
            this.expenseForm.patchValue({ 'houseId': null })
            this.showErrors(true);
        }
    };


    getPaymentTypes(): void {
        this.masterDataService.getGeneralTableByTableName('ConceptType')
            .subscribe(res => {
                let dataResult = new ResponseListDTO(res);
                this._listStatus = [];
                for (let i = 0; i < dataResult.data.length; i++) {
                    this._listPaymentTypes.push({
                        'typeId': dataResult.data[i].generalTableId,
                        'name': dataResult.data[i].value
                    });
                }
            });
    }

    getHouseAll(): void {
        this.masterDataService.getHouseAll('')
            .subscribe(res => {
                let dataResult = new ResponseListDTO(res);
                this._listHouses = dataResult.data;
            });
    }

    getPeriodsNumberPeriod(periodNumber: number): void {
        this.masterDataService.getPeriodLastestNumberPeriods(periodNumber)
            .subscribe(res => {
                let dataResult = new ResponseListDTO(res);
                this._listPeriods = dataResult.data;
            });
    }

    //=========== 
    //INSERT
    //===========
    public expenseIdAfterNewOnHeader: number;
    public periodIdAfterNewOnHeader: number;
    public paymentTypeIdAfterNewOnHeader: number;

    onAccept() {
        let expense = this.expenseForm.getRawValue();

        if (!this.expenseForm.valid) {
            this.showErrors(true);
            return;
        }
        let expenseDate = new Date(expense.expenseDate.year, expense.expenseDate.month - 1, expense.expenseDate.day, 0, 0, 0, 0);
        expense.expenseDate = expenseDate;

        if (this.flgEdition === 'N') {
            // NEW EXPENSE
            this.expenseDataService.saveExpense(expense).subscribe(res => {
                let dataResult: any = res;
                this.successFlag = dataResult.IsValid;
                this.errorMessages = dataResult.Messages;
                this.successMessage = 'Expense was created';
                this.expenseIdAfterNewOnHeader = dataResult.Pk;
                setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);
            })
                .add(
                    r => {
                        this.flgEdition = 'E';
                        this.periodIdAfterNewOnHeader = this.expenseForm.get('periodId').value;
                        this.paymentTypeIdAfterNewOnHeader = this.expenseForm.get('paymentTypeId').value;
                    }
                );
            this.showErrors(true);
        } else {
            // UPDATE EXPENSE
            this.expenseDataService.updateExpense(expense).subscribe(res => {
                let dataResult: any = res;
                this.successFlag = dataResult.IsValid;
                this.errorMessages = dataResult.Messages;
                this.successMessage = 'Expense was updated';

                setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);
            })
                .add(
                    r => {
                        this.periodIdAfterNewOnHeader = this.expenseForm.get('periodId').value;
                        this.paymentTypeIdAfterNewOnHeader = this.expenseForm.get('paymentTypeId').value;
                        this.getDetail();
                    }
                );
        }
    }

    getDetailAmounts() {
        let totalAmount = 0;
        let subTotalAmount = 0;
        let tax = 0;
        for (let i = 0; i < this.expenseDetailData.length; i++) {
            totalAmount += this.expenseDetailData[i].totalAmount;
            subTotalAmount += this.expenseDetailData[i].subTotalAmount;
            tax += this.expenseDetailData[i].tax;
        }
        let detailAmounts = new DetailAmountsDto();
        detailAmounts.totalAmount = totalAmount;
        detailAmounts.subTotalAmount = subTotalAmount;
        detailAmounts.tax = tax;
        return detailAmounts;
    }

    ngAfterViewInit() {
        const controlBlurs: Observable<any>[] = this.formInputElements
            .map((formControl: ElementRef) => Observable.fromEvent(formControl.nativeElement, 'blur'));

        // Merge the blur event observable with the valueChanges observable
        this.validationSubscription = Observable.merge(this.expenseForm.valueChanges, ...controlBlurs)
            .debounceTime(800).subscribe(value => this.showErrors());
    }

    private showErrors(force = false) {
        this.displayMessage = this.genericValidator.processMessages(this.expenseForm, force);
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
        if (this.validationSubscription) {
            this.validationSubscription.unsubscribe();
        }
    }

    verifyDifferences(detailAmounts: DetailAmountsDto) {
        this.difTotalAmount = this.expenseForm.get('totalAmount').value - detailAmounts.totalAmount;
        this.difSubTotalAmount = this.expenseForm.get('subTotalAmount').value - detailAmounts.subTotalAmount;
        this.difTax = this.expenseForm.get('tax').value - detailAmounts.tax;

        if (this.difTotalAmount === 0) {
            this.difTotalAmount = null;
        }
        if (this.difTax === 0) {
            this.difTax = null;
        }
        if (this.difSubTotalAmount === 0) {
            this.difSubTotalAmount = null;
        }

        this.setDisablePayment(detailAmounts)
        this.setDisablePeriodAndProperty();
    }

    public existsAnyDetail = false;
    setDisablePayment(detailAmounts: DetailAmountsDto) {
        // Deshabilita PaymentType si ya hay detalles, para evitar inconsistencia de conceptos
        this.existsAnyDetail = detailAmounts.totalAmount > 0 ||
                                detailAmounts.subTotalAmount > 0 ||
                                detailAmounts.tax > 0;
        if (this.existsAnyDetail) {
            this.expenseForm.get('periodId').disable();
            this.expenseForm.get('paymentTypeId').disable();
        } else {
            this.expenseForm.get('periodId').enable();
            this.expenseForm.get('paymentTypeId').enable();
        }

    }

    public existsDetailMigratedOnDetail = false;
    setDisablePeriodAndProperty() {
        // Deshabilita Period y Properties si ya han migrado al menos un detalle
        this.existsDetailMigratedOnDetail =  this.expenseDetailData !== undefined &&
                                    this.expenseDetailData.filter(q => q.expenseDetailStatusId === 23).length > 0; // 23 Migrated
        if (this.existsDetailMigratedOnDetail) {
            this.expenseForm.get('periodId').disable();
            this.expenseForm.get('houseId').disable();
        } else {
            this.expenseForm.get('periodId').enable();
            this.expenseForm.get('houseId').enable();
        }
    }

    getDetail() {
        this.expenseDataService.getExpenseDetailByExpenseId(this.expenseForm.get('expenseId').value)
        .subscribe(
            resp => {
                let datagrid = new ResponseListDTO(resp);
                this.expenseDetailData = datagrid.items;
            }
        )
        .add(
            q => {
                this.verifyDifferences(this.getDetailAmounts());
                this.setDisablePeriodAndProperty();
            }
        );
    }

}
