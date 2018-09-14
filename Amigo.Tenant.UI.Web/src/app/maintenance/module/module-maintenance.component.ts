
import { Component, Input,Output, OnInit,EventEmitter,OnChanges, SimpleChange} from '@angular/core';
import {Http,Jsonp, URLSearchParams} from '@angular/http';
import {ModuleDto } from '../../model/moduledto';
import {ActionDtoModule} from '../../model/actiondto';
import { ModuleClient,ModuleDTO, ActionDTO,RegisterModuleRequest } from '../../shared/api/services.client';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { BotonsComponent } from '../../controls/common/boton.component';
import { ValidationService } from '../../shared/validations/validation.service';
import { Autofocus } from  '../../shared/directives/autofocus.directive';
declare var $: any;

export class Type {
    code: string;
    name: string;
}

@Component({
    selector: 'st-module-maintenance',
    templateUrl: './module-maintenance.component.html'
})

export class ModuleMaintenanceComponent implements OnInit{
    constructor(private moduleDataService: ModuleClient,
                private _validationService: ValidationService) { }

    private gridAction: GridDataResult;
    private actionModel = new ActionDtoModule();

    public mainPopupOpened: boolean = false;
    public mainPopupClose() {
        this.mainPopupOpened = false;
    }
    public mainPopupOpen() {

        this.clearValidation()
        this.mainPopupOpened = true;
    }


    confirmDeletionVisible: boolean = false;
    confirmDeletionResponse: boolean = false;
    confirmDeletionActionCode: string;

    confirmActionVisible: boolean = false;


    @Input() eventNew : string;
    @Input() listParents =[];
    @Output() searchModule = new EventEmitter<any>();
    
    showAction:boolean=false;
    ActionDTO : any[];
    flgEdition: string;
    isNew = true;
    isOn:boolean = true;
    maintenance = new ModuleDto(); 

    typeList : Type[]= [
     { code: "-1", name: 'Select' },
     { code: "Button", name: 'Button' },
     {  code: "Link", name: 'Link' },
     {  code: "Tab",  name: 'Tab' }
  ]; 

    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;

    clearValidation() {
        this.successFlag = null;
        this.errorMessages = null;
        this.successMessage = null;
    }

// Verify the Changes Events of @Input
    ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
        
        if (this.eventNew != null) {
            this.onClear();
        }
    }

    ngOnInit() { }

    selectModule = (module)=>{
        this.onGetModuleByCode(module.code);
        this.flgEdition = "E";
        this.isNew = false;
        this.showAction = true;
        this.mainPopupOpen();
    };

    newModule = (module) => {
        this.flgEdition = "A";
        this.isNew = true;
        this.showAction = false;
        this.onClear();
        this.mainPopupOpen();

    };

    private onGetModuleByCode(code): void {

                this.moduleDataService.get(code)
                .subscribe(res => {
                        var dataResult: any = res;
                        this.maintenance = dataResult.data;
                        
                        this.gridAction = {
                            data: dataResult.data.actions,
                            total: 50
                        }
                    }
                );
      }

    onChange(selectedValue) {
     
        //console.log(selectedValue);
    }

    // Event Click from Pop up Action
    AddItemAction(actionFormValid: boolean) {
       
        if (actionFormValid) {
            //---------------------------  Save ------------------------------
            this.ActionDTO = [
                ActionDTO.fromJS({
                    "Code": this.actionModel.code
                    , "Name": this.actionModel.name
                    , "Description": this.actionModel.description
                    , "Type": this.actionModel.type
                    , "EntityStatus": 1
                })
            ];

            this.maintenance.actions = this.ActionDTO;
            this.onUpdate(this.maintenance);
            this.actionModel = new ActionDtoModule();

            $('.xico-action-close').each(function (index) {
                $(this).click();
            });
        }
        else {

            //---------------------------------------------------------------------------
            //      "touch" input controls (to display failed validation messages)
            //---------------------------------------------------------------------------

            this._validationService.DisplayFrontEndValidators('actionForm');

        }

    }
    
    restrictNumeric(e) {
        var input;
        if (e.metaKey || e.ctrlKey) {
            return true;
        }
        if (e.which === 32) {
            return false;
        }
        if (e.which === 0) {
            return true;
        }
        if (e.which < 33) {
            return true;
        }
        input = String.fromCharCode(e.which);
        return !!/[\d\s]/.test(input);
    }

    private onUpdate(module): void {

        this.moduleDataService.update(module)
                                .subscribe(res => {
                                        var dataResult: any = res; 
                                        
                                        this.successFlag = dataResult.isValid;
                                        this.errorMessages = dataResult.messages;
                                        this.successMessage = 'Action Saved';

                                        this.onGetModuleByCode(module.code);
                                });

    }




 private skip: number = 0;

 setSelected(code) {

     this.confirmDeletionActionCode = code;
     this.openConfirmation();
  }

 closeConfirmation(status) {
     this.confirmDeletionResponse = (status == "yes");
     
    // console.log(this.confirmDeletionResponse);

     if (this.confirmDeletionResponse)
     {
         this.maintenance;
         var counter = this.maintenance.actions.length;
         for (var i = 0; i < counter; i++) {
             if (this.maintenance.actions[i].code === this.confirmDeletionActionCode) {
                 this.maintenance.actions[i].entityStatus = 3
             }
         }
         // Send To Delete by row entityStatus
         this.onUpdate(this.maintenance);
     }

     this.confirmDeletionVisible = false;
     this.confirmActionVisible = false;
 }

 openConfirmation() {
     this.confirmDeletionVisible = true;
 }

closeActionConfirmation(status){
 this.confirmDeletionResponse = (status == "yes");

 if (this.confirmDeletionResponse)
     {
         this.showAction = true;
         this.confirmActionVisible = false;
         return;
     }

     this.confirmDeletionVisible = false;
     this.confirmActionVisible = false;
    this.mainPopupClose();

}


 onChangeType(selectedValue) {
     this.actionModel.type = selectedValue;
 }

 private onClear(): void {
     
     this.maintenance = new ModuleDto(); 
     this.resetGrid();
 }

private resetGrid():void {
     
        let actionList: ActionDtoModule[] = [];
        this.gridAction = {
            data: actionList,
            total: 0
        };
        //this.totalRows = 0;
        this.skip = 0;
    }

    private onSave(): void {
        
        let module = new RegisterModuleRequest();
        module.code = this.maintenance.code;
        module.name = this.maintenance.name;
        module.uRL = this.maintenance.url;
        module.parentModuleCode = this.maintenance.parentModuleCode;
        module.sortOrder = this.maintenance.sortOrder;
        if (this.flgEdition === "E") {  
        
            //------------------------------
            //      process EDIT form
            //------------------------------
            this.moduleDataService.update(module)
                    .subscribe(res => {
                        //debugger;
                var dataResult: any = res; 

                    this.successFlag = dataResult.isValid;
                    this.errorMessages = dataResult.messages;
                    this.successMessage = 'Module Updated';

                    //this.onGetModuleByCode(module.code);
                    });


        }
        else if (this.flgEdition === "A") {
            //------------------------------
            //      process ADD form
            //------------------------------
            this.moduleDataService.register(module).subscribe(res => {

                var dataResult: any = res; 

                this.successFlag = dataResult.isValid;
                this.errorMessages = dataResult.messages;
                this.successMessage = 'Module Created';

                if (this.successFlag)
                {
                    // prepare the EDIT Form to allow user register Actions
                    this.flgEdition = "E";
                    this.isNew = false;
                    //this.showAction  = this.successFlag;
                    this.confirmActionVisible = true;
                }



            });

        }
    }

//This Event cames from Boton.Component
onExecuteEvent($event){

 switch ($event) {
    case "s":
      this.onSave();
      break;
    case "c":
      this.onClear();        
      break;
    case "k":
         this.mainPopupClose();
         this.searchModule.emit();
      break;
    default:
      confirm("Sorry, that Event doesn't exist yet!");
  }
}

}