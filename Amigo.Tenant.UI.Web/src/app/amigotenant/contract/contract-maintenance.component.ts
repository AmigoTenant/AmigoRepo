import {Component, Input, Output, state, SimpleChange, ViewChild, EventEmitter, OnInit, OnDestroy} from '@angular/core';
import {Http, Jsonp, URLSearchParams} from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";

//import { GridActivityLogComponent } from './../../shipment-tracking/activity-log/grid-activity-log/grid-activity-log.component';
import { EntityStatusDTO, HouseDTO } from './../../shared/api/services.client';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { ConfirmationList, Confirmation } from  '../../model/confirmation.dto';
import { ListsService } from '../../shared/constants/lists.service';
import { ContractClient, ContractRegisterRequest, EntityStatusClient, HouseClient, FeatureClient, GeneralTableClient } from '../../shared/api/services.client';
import { EnvironmentComponent } from '../../shared/common/environment.component';
import { ContractHouseFeatureComponent } from './contract-house-feature.component';
import { NgbdTypeaheadTenant } from '../../shared/typeahead-tenant/typeahead-tenant';
import { NgbdTypeaheadHouse } from '../../shared/typeahead-house/typeahead-house';
import { OtherTenantRegisterRequest } from '../../shared/api/services.client';
import { HouseFeatureAndDetailDTO } from '../../shared/api/services.client';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs'
import { ContractHouseDetailRegisterRequest } from '../../shared/api/services.client';
import { ValidationService } from '../../shared/validations/validation.service';
import { BusinessAppSettingService } from "../../shared/constants/business-app-setting.service";


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

export class FormError {
    tenantError: boolean;
    beginDateError: boolean;
    endDateError: boolean;
    houseError: boolean;
    depositError: boolean;
    rentError: boolean;
    contractHouseDetailError: boolean;
    paymentModeError: boolean;
}


@Component({
  selector: 'at-contract-maintenance',
  templateUrl: './contract-maintenance.component.html'
})

export class ContractMaintenanceComponent extends EnvironmentComponent implements OnInit, OnDestroy {

    model: ContractRegisterRequest;
    otherTenantRequestList: OtherTenantRegisterRequest[]=[];
    otherTenantRequest: OtherTenantRegisterRequest;
    houseFeatureRequest: ContractHouseDetailRegisterRequest;

    isVisibleHouseFeature: boolean = false;
    public modelFrom: any;
    public modelTo: any;
    hasFeatures: boolean = false;
    allowEditing: boolean= true;
    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;
    _listContractHouseDetail: any[]=[];

    _formError: FormError;

    //DROPDOWNS
    _listHouseAndDetails: HouseFeatureAndDetailDTO[]=[];
    _listPaymentMode: any[];
    _listProperties: any[];
    _currentTenant: any;
    _currentOtherTenant: any;
    _currentHouse: any;
    selectedOtherTenantList: any[] = [];
    flgEdition: string;
    _otherTenantDeleted: any[] = [];

    //To manage the status if it is Editing or Adding
    _isDisabled: boolean;

    constructor(
            private route: ActivatedRoute, 
            private router: Router,
            private contractClient: ContractClient,
            private entityStatusClient: EntityStatusClient,
            private houseClient: HouseClient,
            private featureClient: FeatureClient,
            private listConfirmation: ConfirmationList,
            private listsService: ListsService,
            private generalTableClient: GeneralTableClient,
            private businessAppSettingService: BusinessAppSettingService
        ) {
        super();
    }

    sub: Subscription;

    contractId: any;

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    ngOnInit() {
        this.model = new ContractRegisterRequest();
        this.initializeForm();
        this.sub = this.route.params.subscribe(params => {

            let code = params['code'];
            if (code != null && typeof (code) != 'undefined') {
                this.getContractById(code);
                this.flgEdition = "E";
                this.allowEditing = this.model.contractStatusCode == "DRAFT" ? true : false;
                this._isDisabled = true;

            } else {
                this.flgEdition = "N";
                this.model.contractId = -1;
                this.isVisibleHouseFeature = false;
                this.model.contractCode = "";
                this.model.otherTenants = [];
                this.model.contractStatusCode = "DRAFT";
                this._isDisabled = false;
            }

        });

    }

    getContractById(id):void
    {
        this.contractClient.getById(id).subscribe(
            response => {
                var dataResult: any = response;
                this.model = dataResult.data;
                this.modelFrom = this.getDateFromModel(this.model.beginDate);
                this.modelTo = this.getDateFromModel(this.model.endDate);
                this._currentTenant = this.getCurrentTenant(this.model.tenantId, this.model.fullName);
                this._currentHouse = this.getCurrentHouse(this.model.houseId, this.model.houseName);
                this.isVisibleHouseFeature = true;
                this.getHouseFeatureDetailContract();
                this.getHouseFeatureAndDetail();
                this.setOtherTenants(this.model.otherTenants);
            });
    }

    public setOtherTenants(otherTenants: any[])
    {
        for (let i in otherTenants) {
            var tenant = new modelTenant();
            tenant.tenantId = otherTenants[i].tenantId;
            tenant.fullName = otherTenants[i].fullName;
            this.getOtherTenant(tenant);
        }

    }

    public getCurrentTenant(id, name): modelTenant {
        var currentTenant = new modelTenant();
        currentTenant.tenantId = id;
        currentTenant.fullName = name;
        return currentTenant;
    }

    public getCurrentHouse(id, name): modelHouse {
        var currentHouse = new modelHouse();
        currentHouse.houseId = id;
        currentHouse.name = name;
        return currentHouse;
    }

    public getDateFromModel(serviceStartDate: Date): modelDate {
        var model = new modelDate();
        if (serviceStartDate != null && serviceStartDate != undefined) {
            model.day = serviceStartDate.getDate();
            model.month = serviceStartDate.getMonth() + 1;
            model.year = serviceStartDate.getFullYear();
        }
        else
            model = null;

        return model;
    }

    initializeForm(): void {
        this.resetFormError();
        this.getPaymentMode();
    }

  onSelectModelFrom(): void {
      if (this.modelFrom != null && this.modelFrom != undefined ) {
          this.model.beginDate = new Date(this.modelFrom.year, this.modelFrom.month - 1, this.modelFrom.day, 0, 0, 0, 0);
          let monthsNumber = this.model.monthsNumber === undefined || this.model.monthsNumber == null ? 0 : this.model.monthsNumber;
          this.calculateEndDate(monthsNumber);
          this._formError.beginDateError = false;

      }
      else {
          this.model.beginDate = undefined; //new Date();
          this._formError.beginDateError = true;
      }
  }

  onSelectModelTo(): void {
      if (this.modelTo != null && this.modelTo != undefined) {
          this.model.endDate = new Date(this.modelTo.year, this.modelTo.month - 1, this.modelTo.day, 0, 0, 0, 0);
          this.getHouseFeatureAndDetail();
          this._formError.endDateError = false;
      }
      else {
          this.model.endDate = undefined; //new Date();
          this._formError.endDateError = true;
      }

  }

    //=========== 
    //PRINT
    //===========

    onPrint(data): void {
        // this.itemSelectedEdit.emit({
        //     'device': data,
        //     'platformsRawData': this.platformsRawData,
        //     'platforms': this.platforms,
        //     'modelsRawData': this.modelsRawData,
        //     'brands': this.brands
        // });
    }


    //=========== 
    //INSERT
    //===========

    onSave(): void {

        this.setHouseFeatureToDB();
        if (this.isValidData()) {
            if (this.flgEdition == "N") {
                //NEW
                this.model.contractStatusId = 2; //DRAFT
                this.model.rowStatus = true;
                this.contractClient.register(this.model).subscribe(res => {
                    var dataResult: any = res;
                    this.successFlag = dataResult.isValid;
                    this.errorMessages = dataResult.messages;
                    this.successMessage = 'Contract was created';
                    //TODO: Permite descargar nuevamente la lista de HouseFeatures Asignados al contrato, 
                    //debe hacerse la llamada en el servidor al grabar, para evitar grabar informacion Features 
                    //que ya han sido asignados concurrentemente (por otra persona)
                    this.getHouseFeatureDetailContract();
                    setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);
                    if (this.successFlag) {
                        this.getContractById(dataResult.pk);
                        this.flgEdition = "E";
                        this._isDisabled = true;
                    }
                    
                });
            }
            else {
                //UPDATE
                this.contractClient.update(this.model).subscribe(res => {
                    var dataResult: any = res;
                    this.successFlag = dataResult.isValid;
                    this.errorMessages = dataResult.messages;
                    this.successMessage = 'Contract was Updated';
                    //TODO: Permite descargar nuevamente la lista de HouseFeatures Asignados al contrato, 
                    //debe hacerse la llamada en el servidor al grabar, para evitar grabar informacion Features 
                    //que ya han sido asignados concurrentemente (por otra persona)
                    this.getHouseFeatureDetailContract();
                    setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);

                });
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

    //===========
    //EXPORT
    //===========

    onExport():void
    {

    }

    onCancel():void{
        this.router.navigate(['amigotenant/contract']);
    }

    //This Event came from BotonComponent
    onExecuteEvent($event) {
        switch ($event) {
            case "s":
                this.onSave();
                break;
            case "c":
                //this.onClear();
                break;
            case "k":
                this.onCancel();
                break;
            default:
                confirm("Sorry, that Event is not exists yet!");
        }
    }

    getTenant = (item) => {
        if (item != null && item != undefined) {
            this.model.tenantId = item.tenantId;
            //this.model.fullName = item.fullName;
            this._currentTenant = item;
            this._formError.tenantError = false;
        }
        else {
            this.model.tenantId = 0;
            //this.model.fullName = item.username;
            this._currentTenant = undefined;
            this._formError.tenantError = true;
        }
    };

    getHouse = (item) => {
        if (item != null && item != undefined && item!= "") {
            this.model.houseId = item.houseId;
            this._currentHouse = item;
            this.isVisibleHouseFeature = true;
            this.getHouseFeatureDetailContract();
            this.getHouseFeatureAndDetail();
            this._formError.houseError = false;
        }
        else {
            this.model.houseId = 0;
            this._currentHouse = undefined;
            this.isVisibleHouseFeature = false;
            this.hasFeatures = false;
            this._formError.houseError = true;
        }
    };

    getHouseFeatureDetailContract(): void {
        this.contractClient.searchHouseFeatureDetailContract(this.model.houseId).subscribe
            (response => {
                var dataResult: any = response;
                this._listContractHouseDetail = dataResult.value.data;
            });
    }

    getHouseFeatureAndDetail():void
    {
        if (this.model.houseId !== undefined && this.model.houseId !== null && this.model.houseId > 0) {
            this.houseClient.searchHouseFeatureAndDetail(this.model.houseId, this.model.contractId)
                .subscribe(response => {
                    var dataResult: any = response;
                    this._listHouseAndDetails = dataResult.value.data;
                    this.hasFeatures = this._listHouseAndDetails.length > 0;
                    this.setHouseAndDetails();
                });
        }
    }

    setHouseAndDetails(): void {
        let inputBeginDate: Date = this.model.beginDate;
        let inputEndDate: Date = this.model.endDate;

        if (inputBeginDate !== undefined && inputEndDate !== undefined) {
            for (let i in this._listHouseAndDetails) {
                var houseFeature = this._listHouseAndDetails[i];
                var featureExisting = this._listContractHouseDetail.filter(q =>
                    q.houseFeatureId == houseFeature.houseFeatureId
                    && (
                        (inputBeginDate >= q.beginDate && inputBeginDate <= q.endDate) ||
                        (inputEndDate >= q.beginDate && inputEndDate <= q.endDate) ||
                        (inputBeginDate <= q.beginDate && inputEndDate >= q.endDate)
                    )
                    && q.contractId != this.model.contractId
                );

                if (featureExisting.length > 0)
                    houseFeature.isDisabled = true;
                else {
                    //Busqueda de los features existentes para el contrato EDITADO

                    var existForCheck = this._listContractHouseDetail.filter(q =>
                        q.houseFeatureId == houseFeature.houseFeatureId
                        && q.contractId == this.model.contractId);

                    if (existForCheck.length > 0)
                        houseFeature.marked = true;
                    houseFeature.isDisabled = false;
                }
            }
        }

        this.setAllHouseItem();
    }

    setAllHouseItem()
    {
        if (this._listHouseAndDetails[0] != undefined) {
            var exist = this._listHouseAndDetails.filter(r => r.isAllHouse == false && r.isDisabled == true).length;
            if (exist > 0)
                this._listHouseAndDetails[0].isDisabled = true;
            else
                this._listHouseAndDetails[0].isDisabled = false;
        }
    }

    getPaymentMode():void
    {
        this.generalTableClient.getGeneralTableByTableNameAsync("PaymentMode")
            .subscribe(response => {
                var dataResult: any = response;
                this._listPaymentMode = dataResult.value.data;

            });

    }

    //////////////////////////////////////////////////////////////
    ///////// OTHER TENANTS
    //////////////////////////////////////////////////////////////

    getOtherTenant = (item) => {
        if (item != undefined && item != null && item != '') {

            //Ingresar a la Lista de elementos de UI si no existe
            //verifica si ya existe uno ya ingresado
            var exists = this.selectedOtherTenantList.filter(r => r.tenantId == item.tenantId).length;
            if (exists == 0) {
                this.selectedOtherTenantList.push(item);
                this.typeaheadOtherTenantCleanValue();
            }

            //Logica para Insertar en Lista que grabara en BD
            var otherTenant = this.model.otherTenants.filter(r => r.tenantId == item.tenantId);
            if (otherTenant[0] != undefined && otherTenant[0] != null) {
                if (otherTenant[0].tableStatus == 0) //unchanged
                    otherTenant[0].tableStatus = 0;
                if (otherTenant[0].tableStatus == 1) //inserted
                    otherTenant[0].tableStatus = 1;
                if (otherTenant[0].tableStatus == 3) //deleted
                    otherTenant[0].tableStatus = 0;
            }
            else
            {
                this.otherTenantRequest = new OtherTenantRegisterRequest();
                this.otherTenantRequest.contractId = this.model.contractId;
                this.otherTenantRequest.tenantId = item.tenantId;
                this.otherTenantRequest.tableStatus = 1; //inserted
                this.otherTenantRequest.rowStatus = true; //inserted
                this.model.otherTenants.push(this.otherTenantRequest);
            }
        }
    };

    public typeaheadOtherTenantCleanValue(): void {
        this._currentOtherTenant = [];
        this._currentOtherTenant.tenantId = 0;
        this._currentOtherTenant.fullName = '';
    }

    public quitSelectedOtherTenant(item: any) {
        for (let i in this.selectedOtherTenantList) {
            if (this.selectedOtherTenantList[i].tenantId == item.tenantId) {
                this.selectedOtherTenantList.splice(parseInt(i), 1);
                break;
            }
        }

        var otherTenant = this.model.otherTenants.filter(r => r.tenantId == item.tenantId);
        if (otherTenant[0] != undefined && otherTenant[0] != null) {
            if (otherTenant[0].tableStatus == 0) //unchanged
                otherTenant[0].tableStatus = 3;
            if (otherTenant[0].tableStatus == 1) //inserted
            {
                for (let i in this.model.otherTenants) {
                    if (this.model.otherTenants[i].tenantId == item.tenantId) {
                        this.model.otherTenants.splice(parseInt(i), 1);
                        break;
                    }
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////
    ///////// PRICES
    //////////////////////////////////////////////////////////////

    getPrices= (items) => 
    {
        var rentPrice = 0;
        var rentDeposit = 0;
        for (let i in items) {
            var houseFeature = items[i];
            if (!houseFeature.isAllHouse) {
                if (!houseFeature.isDisabled && houseFeature.marked) {
                    rentPrice += houseFeature.rentPrice;
                    rentDeposit += houseFeature.rentDeposit;
                }
            }
        }
        
        this.model.rentPrice = rentPrice;
        this.model.rentDeposit = rentPrice * this.businessAppSettingService.GetDepositPercentage();
        if (this.model.rentPrice > 0)
            this._formError.contractHouseDetailError = false;
    }

    calculateEndDate(months): void
    {

        if ( (this.modelFrom == null || this.modelFrom == undefined))
             return;

        months = months == "" || months == undefined || months == "0" ? 1 : months;
        var dateFrom = new Date(this.modelFrom.year, this.modelFrom.month - 1, this.modelFrom.day, 0, 0, 0, 0);
        dateFrom.setMonth(dateFrom.getMonth() + parseInt(months));
        dateFrom.setDate(dateFrom.getDate() - 1)
        this.modelTo = new modelDate();
        this.modelTo.day = dateFrom.getDate();
        this.modelTo.month = dateFrom.getMonth()+1;
        this.modelTo.year = dateFrom.getFullYear();
        this.model.endDate = new Date(this.modelTo.year, this.modelTo.month - 1, this.modelTo.day, 0, 0, 0, 0);
        this.getHouseFeatureAndDetail();
        this._formError.endDateError = false;
    }

    setHouseFeatureToDB(): void
    {
        this.model.contractHouseDetails = [];
        let c: number = 0;
        for (let i in this._listHouseAndDetails)
        {
            c--;
            let houseFeature = this._listHouseAndDetails[i];
            if (!houseFeature.isDisabled) {

                this.houseFeatureRequest = new ContractHouseDetailRegisterRequest();
                this.houseFeatureRequest.contractId = this.model.contractId;
                this.houseFeatureRequest.houseFeatureId = houseFeature.houseFeatureId;
                this.houseFeatureRequest.rowStatus = true;

                var contractHouseDetail = this._listContractHouseDetail.filter(q => q.houseFeatureId == houseFeature.houseFeatureId && q.contractId == this.model.contractId);


                if (contractHouseDetail.length > 0) {
                    //Contract ID es el mismo que el de la Cabecera
                    if (!houseFeature.marked) {

                        houseFeature.tableStatus = 3; //Delete
                        this.houseFeatureRequest.tableStatus = 3;
                        this.houseFeatureRequest.contractHouseDetailId = contractHouseDetail[0].contractHouseDetailId;
                        this.model.contractHouseDetails.push(this.houseFeatureRequest);
                    }
                    else
                    {
                        houseFeature.tableStatus = 0; //Unchanged
                        this.houseFeatureRequest.tableStatus = 0;
                    }
                }
                else {

                    //Contract ID es diferente al de la Cabecera
                    if (!houseFeature.marked) {
                        houseFeature.tableStatus = 0; //Unchanged
                        this.houseFeatureRequest.tableStatus = 0;
                    }
                    else {
                        houseFeature.tableStatus = 1; //Inserted
                        this.houseFeatureRequest.tableStatus = 1;
                        this.houseFeatureRequest.contractHouseDetailId = c;
                        this.model.contractHouseDetails.push(this.houseFeatureRequest);
                    }
                }
            }
        }
    }

    onPaymentModeChange(): void
    {
        if (this.model.paymentModeId !== undefined && this.model.paymentModeId !== null && this.model.paymentModeId > 0)
        this._formError.paymentModeError = false;
    }

    isValidData(): boolean
    {
        var isValid = true;
        this.resetFormError();

        if (this.model.tenantId == undefined || this.model.tenantId == 0 || this.model.tenantId == null) {
            this._formError.tenantError = true;
            isValid = false;
        }
        if (this.modelFrom == undefined && this.modelFrom == null) {
            this._formError.beginDateError = true;
            isValid = false;
        }
        if (this.modelTo == undefined && this.modelTo == null) {
            this._formError.endDateError = true;
            isValid = false;
        }
        if (this.model.houseId == undefined && this.model.houseId == null) {
            this._formError.houseError = true;
            isValid = false;
        }
        if (this.model.rentDeposit == undefined && this.model.rentDeposit == null) {
            this._formError.depositError = true;
            isValid = false;
        }
        if (this.model.rentPrice == undefined && this.model.rentPrice == null) {
            this._formError.rentError = true;
            isValid = false;
        }
        if (this.flgEdition == "N" && (this.model.contractHouseDetails == undefined || this.model.contractHouseDetails.length == 0 || this.model.contractHouseDetails == null)) {
            this._formError.contractHouseDetailError = true;
            isValid = false;
        }
        if (this.model.paymentModeId == undefined || this.model.paymentModeId == 0 || this.model.paymentModeId == null) {
            this._formError.paymentModeError = true;
            isValid = false;
        }
        return isValid;
    }

    private resetFormError = () => {
        if (this._formError == undefined)
            this._formError = new FormError();

        this._formError.tenantError = false;
        this._formError.beginDateError = false;
        this._formError.endDateError = false;
        this._formError.houseError = false;
        this._formError.depositError = false;
        this._formError.rentError = false;
        this._formError.contractHouseDetailError = false;
        this._formError.paymentModeError = false;
    }


}
