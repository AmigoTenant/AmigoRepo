import {TranslateService} from '@ngx-translate/core';
import {ExpenseDataService} from './expense-data.service';
import { ExpenseRegisterRequest } from './dto/expense-register-request';
import { Component, AfterViewInit, EventEmitter, OnInit, OnDestroy, ElementRef, ViewChildren, Input, Output } from '@angular/core';
import { Http, Jsonp, URLSearchParams} from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule, Form, FormControlName } from '@angular/forms';
import { EntityStatusDTO, HouseDTO } from './../../shared/api/services.client';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { ConfirmationList, Confirmation } from  '../../model/confirmation.dto';
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

export class ExpenseMaintenanceDetailComponent extends EnvironmentComponent implements OnInit, OnDestroy, AfterViewInit {
    private genericValidator: GenericValidator;
    private validationSubscription: Subscription;
    public validationMessages: { [key: string]: { [key: string]: string } } = {};
    public displayMessage: { [key: string]: string; } = {};
    @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];
    @Input() inputSelectedExpenseDetail: any;
    @Output() eventoClose = new EventEmitter();

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

        // Observable.forkJoin([
        //     this.translate.get('common.requiredField'),
        //     this.translate.get('common.alphadashmax12'),
        //     this.translate.get('common.maxLength', { value: 50 })
        //   ]).subscribe((messages: string[]) => this.buildMessages(...messages));
        //   this.genericValidator = new GenericValidator(this.validationMessages);
    }

    // buildMessages(required?: string, notvalid?: string, maxlength?: string) {
    //     this.validationMessages = {
    //       conceptId: {
    //         required: required
    //       },
    //       applyTo: {
    //         required: required
    //       },
    //       subTotalAmount: {
    //         required: required
    //       }
    //     };
    // }

    

    ngOnInit() {
         debugger
         this.model = new ExpenseDetailRegisterRequest();

        this.buildForm();
        this.initializeForm();
        // this.sub = this.route.params.subscribe(params => {

        //     //TODO:
        //     let id = params['expenseDetailId'];
        //     let idDetail = this.inputSelectedExpenseDetail;
        //     if (id != null && typeof (id) !== 'undefined') {
        //         this.getExpenseDetailByExpenseDetailId(id);
        //         this.flgEdition = 'E';
        //         //this._isDisabled = false;

        //     } else {
        //         this.flgEdition = 'N';
        //         //this._isDisabled = false;
        //     }

        // });


    }

    buildForm() {
        this.expenseDetailForm = this.formBuilder.group({
            conceptId: [null, [Validators.required]],
            subTotalAmount: [null, [Validators.required]],
            applyTo: [null, [Validators.required]],
            tenantId: [null],
            remark: [null],
            tax: [null],
            totalAmount: [null]
         });
    }

    getExpenseDetailByExpenseDetailId(id): void {

        this.expenseDataService.getExpenseDetailByExpenseDetailId(id).subscribe(
            response => {
                debugger;
                let dataResult: any = new ResponseListDTO(response);
                this.model = dataResult.dat;
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


    cancel(): void {

        //this.router.navigate(['expense/edit/1']);
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



    //EXPENSE DETAIL METHODS

    //GETTING DATA FOR DROPDOWNLIST
    accept() {
        //let variable = this.expenseDetailForm.value;
        Object.assign(this.model, this.expenseDetailForm.value);
        this.expenseDataService.save(this.model)
        .subscribe(r=>
            {
                let result = new ResponseListDTO(r);
            }
        )
    }

    ngAfterViewInit() {
        // Watch for the blur event from any input element on the form.
        // const controlBlurs: Observable<any>[] = this.formInputElements
        //   .map((formControl: ElementRef) => Observable.fromEvent(formControl.nativeElement, 'blur'));
    
        // // Merge the blur event observable with the valueChanges observable
        // this.validationSubscription = Observable.merge(this.expenseDetailForm.valueChanges, ...controlBlurs)
        // .debounceTime(800).subscribe(value => this.showErrors());
      }
    
      private showErrors(force = false) {
        this.displayMessage = this.genericValidator.processMessages(this.expenseDetailForm, force);
      }
    
      ngOnDestroy() {
        this.sub.unsubscribe();
        if (this.validationSubscription) {
          this.validationSubscription.unsubscribe();
        }
      }

    //   _currentTenant: any;

    //   getTenant = (item) => {
    //       if (item != null && item != undefined) {
    //           //this.model.tenantId = item.tenantId;
    //           //this.model.fullName = item.fullName;
    //           this._currentTenant = item;
    //       }
    //       else {
    //           //this.model.tenantId = 0;
    //           //this.model.fullName = item.username;
    //           this._currentTenant = undefined;
    //       }
    //   };

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
          this.masterDataService.getConceptsByTypeIdList([31, 29])
              .subscribe(res => {
                  let dataResult = new ResponseListDTO(res);
                  this._listConcepts = dataResult.data;
              });
      }

    onCancel() {
        this.eventoClose.emit();
    }
}
