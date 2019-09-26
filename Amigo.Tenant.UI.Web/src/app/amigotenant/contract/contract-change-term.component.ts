import { Component, OnInit, Output, EventEmitter, Input } from "@angular/core";
import { ContractDataService } from "./contract-data.service";
import { FormBuilder, FormControl, Validators, FormGroup } from "@angular/forms";
import { ContractChangeTermRequest } from "./dto/contract-change-term-request";
import { GenericValidator } from "../../shared/generic.validator";
import { Observable } from "rxjs/Observable";
import { TranslateService } from "@ngx-translate/core";

@Component(
{
    selector: 'at-contract-change-term',
    templateUrl: './contract-change-term.component.html'
})

export class ContractChangeTermComponent implements OnInit{
    private genericValidator: GenericValidator;
    public contractChangeTermForm: FormGroup;
    @Output() onAcceptPopupEmitter= new EventEmitter<ContractChangeTermRequest>();
    @Output() onCancelPopupEmitter= new EventEmitter<ContractChangeTermRequest>();
    @Input() contract: ContractChangeTermRequest;

    constructor(private contractDataService: ContractDataService,
        private fb: FormBuilder,
        private translate: TranslateService){}

    ngOnInit(){
        this.buildForm();
        this.buildValidator();
        this.setValues();
    }

    buildForm() {
        this.contractChangeTermForm = this.fb.group({
            contractTermType : ['EXTENSION', Validators.required],
            finalPeriodId: [null, Validators.required],
            fromPeriodId: [null],
            newTenantId: [null],
            newDeposit: [null],
            newRent: [null],
            newHouseId: [null],
            contractId: [null]
        });
    }

    setValues(){
        this.contractChangeTermForm.get('contractId').setValue(this.contract.contractId);
    }

    onAccept(){
        if (!this.contractChangeTermForm.valid)
        {
            this.showErrors(true);
            return;
        }

        this.showPopuChangeTermConfirm = true;
    }

    onCancel(){
        this.onCancelPopupEmitter.emit();
    }

    
    getHouse = (item) => {
        if (item != null && item != undefined && item != "") {
            this.contractChangeTermForm.get('newHouseId').setValue(item.houseId);
        }
        else {
            this.contractChangeTermForm.get('newHouseId').setValue(null);
        }
    };

    getTenant = (item)=> {
        if (item != null && item != undefined && item != "") {
            this.contractChangeTermForm.get('newTenantId').setValue(item.tenantId);
        }
        else {
            this.contractChangeTermForm.get('newTenantId').setValue(null);
        }
    }

    getFinalPeriod = (item) => {
        if (item != null && item != undefined && item != "") {
            this.contractChangeTermForm.get('finalPeriodId').setValue(item.periodId);
            this.showErrors(true);
        }
        else {
            this.contractChangeTermForm.get('finalPeriodId').setValue(null);
        }
    };

    getFromPeriod = (item) => {
        if (item != null && item != undefined && item != "") {
            this.contractChangeTermForm.get('fromPeriodId').setValue(item.periodId);
            this.showErrors(true);
        }
        else {
            this.contractChangeTermForm.get('fromPeriodId').setValue(null);
        }
    };

    //Show Errors
    public displayMessage: { [key: string]: string; } = {};
    public validationMessages: { [key: string]: { [key: string]: string } } = {};

    private showErrors(force = false) {
        this.displayMessage = this.genericValidator.processMessages(this.contractChangeTermForm, force);
    }

    buildValidator() {
        Observable.forkJoin([
            this.translate.get('common.requiredField'),
            this.translate.get('common.notValidFormat')
        ]).subscribe((messages: string[]) => this.buildMessages(...messages));
        this.genericValidator = new GenericValidator(this.validationMessages);
    }

    buildMessages(required?: string, notvalid?: string) {
        this.validationMessages = {
            finalPeriodId: {
                required: required
            },
            fromPeriodId: {
                required: required
            }
        };
    }

    onChangeTermTypeClick(){
        if (this.contractChangeTermForm.get('contractTermType').value == 'EXTENSION')
        {
            this.contractChangeTermForm.get('fromPeriodId').clearValidators();
            this.contractChangeTermForm.get('fromPeriodId').updateValueAndValidity();

            this.contractChangeTermForm.get('finalPeriodId').setValidators(Validators.required);
            this.contractChangeTermForm.get('finalPeriodId').updateValueAndValidity();
            
        }else{
            this.contractChangeTermForm.get('finalPeriodId').clearValidators();
            this.contractChangeTermForm.get('finalPeriodId').updateValueAndValidity();
            this.contractChangeTermForm.get('fromPeriodId').setValidators(Validators.required);
            this.contractChangeTermForm.get('fromPeriodId').updateValueAndValidity();
        }
        
    }

    //=========== 
    //CHANGE TERM
    //===========

    public changeTermMessage: string = "Are you sure to Change the Terms of this Lease?";
    public showPopuChangeTermConfirm: boolean = false;

    public yesChangeTerm() {
        let model = this.contractChangeTermForm.value;
        this.onAcceptPopupEmitter.emit(model);
    }

    public noChangeTerm() {
        this.showPopuChangeTermConfirm= false;
    }

}