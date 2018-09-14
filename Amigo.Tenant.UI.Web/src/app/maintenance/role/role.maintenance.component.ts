import {Component,OnInit, Input, state, OnChanges, SimpleChange,ViewChild} from '@angular/core';
import {Http, Jsonp, URLSearchParams} from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { AmigoTenantTRoleClient,AmigoTenantTRoleSearchRequest ,AmigoTenantTRoleDTO} from '../../shared/api/services.client';
import { BotonsComponent } from '../../controls/common/boton.component';
import { RolPermissionComponent } from './role-permission.component';
import { Autofocus } from  '../../shared/directives/autofocus.directive';

@Component({
  selector: 'st-rol-maintenance',
  templateUrl: './role.maintenance.component.html'
})

export class RoleMaintenanceComponent implements OnInit {


    constructor(private roleDataService: AmigoTenantTRoleClient )  { }

//@Input() inputSelectedCode : any;
@Input() inputTypeListAdmin : any;

public successFlag: boolean=false;
public showTree : boolean= false;
public errorMessages: string[];
public successMessage: string;
public mainPopupOpened: boolean = false;

maintenance = new AmigoTenantTRoleDTO(); 
ModuleTree : any[];
flgEdition :string;
isOn:boolean = true;


 ngOnInit() {
        this.onGetRolPermissionTree();
}

// Verify the Changes Events of @Input
//  ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
//         debugger
//         if (this.inputSelectedCode) {
//         this.onClearValidation();
//         this.onGetRolTree(this.inputSelectedCode.code );
//         this.maintenance.code = this.inputSelectedCode.code;
//         this.maintenance.name = this.inputSelectedCode.name;
//         this.maintenance.isAdmin = this.inputSelectedCode.isAdmin;
//         this.maintenance.rowStatus = this.inputSelectedCode.rowStatus;
//         this.maintenance.entityStatus = 2;
//         this.maintenance.amigoTenantTRoleId = this.inputSelectedCode.amigoTenantTRoleId;
//         this.flgEdition = "E";
//         this.mainPopupOpen();
//         }
//     }
lo=()=>{
this.maintenance
}
onEdit=(inputSelectedCode)=>{

 this.onClearValidation();
 
        this.onGetRolTree(inputSelectedCode.code );
        this.maintenance.code = inputSelectedCode.code;
        this.maintenance.name = inputSelectedCode.name;
        this.maintenance.isAdmin = inputSelectedCode.isAdmin;
        this.maintenance.rowStatus = inputSelectedCode.rowStatus;
        this.maintenance.entityStatus = 2;
        this.maintenance.amigoTenantTRoleId = inputSelectedCode.amigoTenantTRoleId;
        this.flgEdition = "E";
        this.mainPopupOpen();

}

    onNew():void{
        this.onClear();
        this.onClearValidation();
        this.mainPopupOpen();
    }

   private onClear():void{
       this.maintenance = new AmigoTenantTRoleDTO(); 
       this.maintenance.amigoTenantTRoleId=null;
       this.maintenance.rowStatus =true; 
       this.maintenance.entityStatus=1;
       this.maintenance.isAdmin = false;
       this.showTree = false;
       this.flgEdition = "N";
        
    }

   private  onClearValidation():void {
        this.successFlag = null;
        this.errorMessages = null;
        this.successMessage = null;
    }

   private onGetRolPermissionTree():void{
        this.roleDataService.getModuleAction()
                .subscribe(res => {
                        var dataResult: any = res;
                        this.ModuleTree = dataResult.data;
                      //  console.warn(this.ModuleTree);
                    });
    }

    private onGetRolTree(code):void{
        this.roleDataService.getRol(code)
        .subscribe(res => {
                        var dataResult: any = res;
                        this.ModuleTree = dataResult.data;
                        this.showTree = true;
                    });

    }

    private mainPopupClose():void {
        this.mainPopupOpened = false;
    }
    private mainPopupOpen():void{
        this.clearValidation()
        this.mainPopupOpened = true;
    }

     clearValidation():void {
        this.successFlag , this.errorMessages,this.successMessage = null;
        
    }

    onSave=()=>{
        
   if(this.flgEdition ==='N'){
       this.roleDataService.register(this.maintenance)
                .subscribe(res => {
            
                var dataResult: any = res;
                
                this.successFlag = dataResult.isValid;
                this.errorMessages = dataResult.messages;
                this.successMessage = 'Role added';
                if (this.successFlag)
                {
                    // prepare the EDIT Form  register
                    this.flgEdition = "E";
                    this.showTree = true;
                }
                
        });

   }else{
       this.roleDataService.update(this.maintenance)
                .subscribe(res => {
                var dataResult: any = res;
                        
                this.successFlag = dataResult.isValid;
                this.errorMessages = dataResult.messages;
                this.successMessage = 'Role updated';
                if (this.successFlag)
                {
                    // prepare the EDIT Form  register
                    this.flgEdition = "E";
                    this.showTree = true;
                }
                
        });

   }

    }


    //This Event came from BotonComponent
    onExecuteEvent($event) {debugger
        switch ($event) {
            case "s":
                this.onSave();
                //break;
            case "c":
                //this.onClear();
                break;
            case "k":
                this.onClear();
                this.mainPopupClose();
                break;
            default:
                confirm("Sorry, that Event is not exists yet!");
        }
    }  


}