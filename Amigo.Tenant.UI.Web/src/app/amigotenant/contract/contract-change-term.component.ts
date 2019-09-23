import { Component, OnInit, Output, EventEmitter } from "@angular/core";
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
    @Output() onAcceptPopupEmitter= new EventEmitter();
    @Output() onCancelPopupEmitter= new EventEmitter<ContractChangeTermRequest>();

    constructor(private contractDataService: ContractDataService,
        private fb: FormBuilder,
        private translate: TranslateService){}

    ngOnInit(){
        this.buildForm();
        this.buildValidator();
    }

    buildForm() {
        this.contractChangeTermForm = this.fb.group({
            contractTermType : [''],
            finalPeriodId: [null],
            fromPeriodId: [null],
            newTenantId: [null],
            newDeposit: [null],
            newRent: [null],
            newHouseId: [null],
            contractId: [null]
        });
    }


    onAccept(){
        debugger;
        if (!this.contractChangeTermForm.valid)
        {
            this.showErrors(true);
            return;
        }

        let model = this.contractChangeTermForm.value;
        this.onAcceptPopupEmitter.emit(model);
        //this.contractDataService.ContractChangeTerm()
    }

    onCancel(){
        debugger;
        this.onCancelPopupEmitter.emit();
        //this.contractDataService.ContractChangeTerm()
    }

    
    getHouse = (item) => {
        // if (item != null && item != undefined && item != "") {
        //     this.model.houseId = item.houseId;
        //     this._currentHouse = item;
        // }
        // else {
        //     this.model.houseId = undefined;
        //     this._currentHouse = undefined;
        // }
    };

    getTenant = (item)=> {
        
    }

    getPeriod = (item) => {
        // if (item != null && item != undefined && item != "") {
        //     this.model.periodId = item.periodId;
        //     this._currentPeriod = item;
        // }
        // else {
        //     this.model.periodId = undefined;
        //     this._currentPeriod = undefined;
        // }
    };

    //Shor¡w Errors
    public displayMessage: { [key: string]: string; } = {};
    public validationMessages: { [key: string]: { [key: string]: string } } = {};

    private showErrors(force = false) {
        this.displayMessage = this.genericValidator.processMessages(this.contractChangeTermForm, force);
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

}