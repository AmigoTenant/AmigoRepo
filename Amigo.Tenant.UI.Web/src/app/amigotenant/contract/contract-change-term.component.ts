import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { ContractDataService } from "./contract-data.service";
import { FormBuilder, FormControl, Validators } from "@angular/forms";

@Component(
{
    selector: 'at-contract-change-term',
    templateUrl: '.\contract-change-term.component.html'
})

export class ContractChangeTermComponent implements OnInit{

    public contractChangeTermForm: any;
    @Output() onAcceptPopupEmitter= new EventEmitter();
    @Output() onCancelPopupEmitter= new EventEmitter();

    constructor(private contractDataService: ContractDataService,
        private fb: FormBuilder){}

    ngOnInit(){
        this.buildForm();
    }

    buildForm() {
        this.contractChangeTermForm = this.fb.group({
            periodIdTo : [''],
            propertyId: [''],
            newDeposit: [null],
            newRent: [null],
            newResponsible: [''],
            periodIdFrom: [''],
            contractId: [''],
            tenantId: ['']
        });
    }


    onAccept(){
        this.onAcceptPopupEmitter.emit();
        //this.contractDataService.ContractChangeTerm()
    }

    onCancelPopup(){
        this.onCancelPopupEmitter.emit();
        //this.contractDataService.ContractChangeTerm()
    }
}