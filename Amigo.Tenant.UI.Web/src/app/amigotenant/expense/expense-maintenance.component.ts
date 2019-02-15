import {TranslateService} from '@ngx-translate/core';
import {ExpenseDataService} from './expense-data.service';
import { ExpenseRegisterRequest } from './dto/expense-register-request';
import { Component, AfterViewInit, EventEmitter, OnInit, OnDestroy, ElementRef, ViewChildren } from '@angular/core';
import { Http, Jsonp, URLSearchParams} from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule, Form, FormControlName } from '@angular/forms';
import { EntityStatusDTO, HouseDTO } from './../../shared/api/services.client';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { ConfirmationList, Confirmation } from  '../../model/confirmation.dto';
import { ListsService } from '../../shared/constants/lists.service';
import { EntityStatusClient, GeneralTableClient, HouseClient } from '../../shared/api/services.client';
import { EnvironmentComponent } from '../../shared/common/environment.component';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs'
import { ValidationService } from '../../shared/validations/validation.service';
import { ResponseListDTO } from '../../shared/dto/response-list-dto';
import { MasterDataService } from '../../shared/api/master-data-service';
import { GenericValidator } from '../../shared/generic.validator';


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

export class modelHouse {
    houseId: number;
    name: string;
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



    model: ExpenseRegisterRequest;
    public modelExpenseDate: any;
    allowEditing = true;
    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;

    expenseForm: FormGroup;

    //DROPDOWNS
    _listPaymentTypes: any = [];
    _listStatus: any = [];
    _currentHouse: any;
    _currentPeriod: any;
    _listHouses: any = [];
    _listPeriods: any = [];

    flgEdition: string;
    _isDisabled: boolean;

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
          houseId: {
            required: required
          },
          periodId: {
            required: required
          },
          houseTypeId: {
            required: required
          },
          paymentTypeId: {
            required: required
          }
        };
    }


    sub: Subscription;
    contractId: any;

    ngOnInit() {
        debugger
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
            expenseDate: [null, [Validators.required]],
            houseId: [null, [Validators.required]],
            periodId: [null, [Validators.required]],
            paymentTypeId: [null],
            remark: [null],
            subTotalAmount: [null],
            tax: [null],
            totalAmount: [null],
            referenceNo: [null]
         });
    }

    getExpenseById(id): void {

        this.expenseDataService.getById(id).subscribe(
            response => {
                debugger;
                let dataResult: any = new ResponseListDTO(response);
                this.model = dataResult.dat;
                this.expenseForm.patchValue(this.model);
                this.modelExpenseDate = this.getDateFromModel(this.model.expenseDate);
            });
    }

    public getDateFromModel(dateFromModel: Date): modelDate {
        let model = new modelDate();
        if (dateFromModel != null && dateFromModel !== undefined) {
            model.day = dateFromModel.getDate();
            model.month = dateFromModel.getMonth() + 1;
            model.year = dateFromModel.getFullYear();
        } else {
            model = null;
        }
        return model;
    }

    initializeForm(): void {
        this.getPaymentTypes();
        this.getHouseAll();
        this.getPeriodsNumberPeriod(5);
    }


    //=========== 
    //INSERT
    //===========

    onSave(): void {
        if (this.isValidData()) {
            if (this.flgEdition === 'N') {
                //NEW
                //this.model.contractStatusId = 2; //DRAFT
                // this.model.rowStatus = true;
                // this.expenseClient.register(this.model).subscribe(res => {
                //     var dataResult: any = res;
                //     this.successFlag = dataResult.isValid;
                //     this.errorMessages = dataResult.messages;
                //     this.successMessage = 'Rental Expense was created';
                //     setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);
                //     if (this.successFlag) {
                //         this.getExpenseById(dataResult.pk);
                //         this.flgEdition = "E";
                //         this._isDisabled = false;
                //     }
                    
                // });
            }
            else {
                //UPDATE
                // this.expenseClient.update(this.model).subscribe(res => {
                //     var dataResult: any = res;
                //     this.successFlag = dataResult.isValid;
                //     this.errorMessages = dataResult.messages;
                //     this.successMessage = 'Rental Expense was Updated';
                //     //TODO: Permite descargar nuevamente la lista de HouseFeatures Asignados al contrato, 
                //     //debe hacerse la llamada en el servidor al grabar, para evitar grabar informacion Features 
                //     //que ya han sido asignados concurrentemente (por otra persona)
                //     //this.getHouseFeatureDetailContract();
                //     setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);

                // });
            }
            
            
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


    onCancel(): void {
        this.router.navigate(['amigotenant/expense']);
    }

    onExecuteEvent($event) {
        switch ($event) {
            case 's':
                this.onSave();
                break;
            case 'c':
                //this.onClear();
                break;
            case 'k':
                this.onCancel();
                break;
            default:
                confirm('Sorry, that Event is not exists yet!');
        }
    }


    //GETDATA FROM MASTERTABLE
    // getPriority(): void {
    //     this.gnrlTableDataService.getGeneralTableByTableNameAsync("Priority")
    //         .subscribe(res => {
    //             var dataResult: any = res;
    //             this._listPriority = [];
    //             for (var i = 0; i < dataResult.value.data.length; i++) {
    //                 this._listPriority.push({
    //                     "typeId": dataResult.value.data[i].generalTableId,
    //                     "name": dataResult.value.data[i].value
    //                 });
    //             }
    //         });
    // }
    

    isValidData(): boolean {
        let isValid = true;
        //this.resetFormError();
        //if (this.model.tenantId == undefined || this.model.tenantId == 0 || this.model.tenantId == null) {
        //    this._formError.tenantError = true;
        //    isValid = false;
        //}
        return isValid;
    }



    //EXPENSE METHODS

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
            //this.model.houseId = item.houseId;
            debugger;
            this._currentHouse = item;
            this.expenseForm.patchValue({'houseId': item.houseId })
        } else {
            //this.model.periodId = undefined;
            this._currentHouse = undefined;
            this.expenseForm.patchValue({'houseId': null })
            this.showErrors(true);
        }
    };

    // onSelectModelExpenseDate(): void {
    //     if (this.modelExpenseDate != null) {
    //         this.model.expenseDate = new Date(this.modelExpenseDate.year, this.modelExpenseDate.month - 1, this.modelExpenseDate.day, 0, 0, 0, 0);
    //     } else {
    //         this.model.expenseDate = undefined; //new Date();
    //     }
    // }

    //GETTING DATA FOR DROPDOWNLIST

    getPaymentTypes(): void {
        this.masterDataService.getGeneralTableByTableName('PaymentType')
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
                // for (let i = 0; i < dataResult.data.length; i++) {
                //     this._listHouses.push({
                //         'houseId': dataResult.data[i].generalTableId,
                //         'name': dataResult.data[i].value
                //     });
                // }
            });
    }

    getPeriodsNumberPeriod(periodNumber: number): void {
        debugger;
        this.masterDataService.getPeriodLastestNumberPeriods(periodNumber)
            .subscribe(res => {
                let dataResult = new ResponseListDTO(res);
                this._listPeriods = dataResult.data;
            });
    }

    // var resp = this.houseClient.searchForTypeAhead(term)
    //         .map(response => 
    //             response.data
    //         );
    //     return resp;

    
    accept() {
        debugger
        let variable = this.expenseForm.value;
        let expenseDate = new Date(variable.expenseDate.year, variable.expenseDate.month - 1, variable.expenseDate.day, 0, 0, 0, 0);
        variable.expenseDate = expenseDate;
        this.expenseDataService.saveExpense(variable).subscribe(r=> {
            let data = r;
        });
        this.showErrors(true);
    }



    ngAfterViewInit() {
        // Watch for the blur event from any input element on the form.
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


    openDialog: boolean = false;
    selectedDetail: any;

    onAddDetail(): void {
        //this.selectedDetail = data;
        this.openDialog = true;
     }


     close() {
        this.openDialog = false;
     }

     eventoCloseParent() {
        this.openDialog = false;
     }


     onSelectModelExpenseDate(): void {
        if (this.modelExpenseDate != null) {
            let expenseDate = new Date(this.modelExpenseDate.year, this.modelExpenseDate.month - 1, this.modelExpenseDate.day, 0, 0, 0, 0);
            this.expenseForm.patchValue(expenseDate);
            this.expenseForm.get('expenseDate').setValue(expenseDate);
            //this.ExpenseRegisterRequest.expenseDate = new Date(this.modelExpenseDate.year, this.modelExpenseDate.month - 1, this.modelExpenseDate.day, 0, 0, 0, 0);
        } else {
            this.modelExpenseDate = undefined;
        }
    }

}
