import { Component, Input, Output, state, SimpleChange, ViewChild, EventEmitter, OnInit, OnDestroy} from '@angular/core';
import { Http, Jsonp, URLSearchParams} from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { EntityStatusDTO, HouseDTO } from './../../shared/api/services.client';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { ConfirmationList, Confirmation } from  '../../model/confirmation.dto';
import { ListsService } from '../../shared/constants/lists.service';
import { CountryClient, ContractClient, ContractRegisterRequest, EntityStatusClient, HouseClient, FeatureClient, GeneralTableClient } from '../../shared/api/services.client';
import { EnvironmentComponent } from '../../shared/common/environment.component';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs'
import { ValidationService } from '../../shared/validations/validation.service';
import { RentalApplicationClient, RentalApplicationRegisterRequest, RentalApplicationCityRequest, RentalApplicationFeatureRequest } from '../../shared/api/rentalapplication.services.client';


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
    fullNameError: boolean;
    applicationDateError: boolean;
}


@Component({
  selector: 'at-rentalapplication-maintenance',
  templateUrl: './rentalapplication-maintenance.component.html'
})

export class RentalApplicationMaintenanceComponent extends EnvironmentComponent implements OnInit, OnDestroy {

    model: RentalApplicationRegisterRequest;
    rentalApplicationFeatureRequestList: RentalApplicationFeatureRequest[]=[];
    rentalApplicationFeatureRequest: RentalApplicationFeatureRequest;
    rentalApplicationCityRequestList: RentalApplicationCityRequest[] = [];
    rentalApplicationCityRequest: RentalApplicationCityRequest;
    public modelApplicationDate: any;
    public modelCheckIn: any;
    public modelCheckOut: any;
    public modelAlertDate: any;

    allowEditing: boolean= true;
    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;

    _formError: FormError;

    //DROPDOWNS
    _listHouseTypes: any = [];
    _listCountries: any = [];
    _listBudgets: any = [];
    _listCompleteShared: any = [];
    _listOutInDowntown: any = [];
    _listReferredBy: any = [];
    _listPriority: any = [];

    _listPaymentMode: any[];
    _listProperties: any[];
    _currentTenant: any;
    _currentRentalApplicationFeature: any;
    _currentHouse: any;
    selectedRentalApplicationFeatureList: any[] = [];
    flgEdition: string;
    _featureDeleted: any[] = [];

    //To manage the status if it is Editing or Adding
    _isDisabled: boolean;

    constructor(
            private route: ActivatedRoute, 
            private router: Router,
            private rentalApplicationClient: RentalApplicationClient, 
            private featureClient: FeatureClient,
            private listConfirmation: ConfirmationList,
            private listsService: ListsService,
            private houseDataService: HouseClient,
            private countryDataService: CountryClient,
            private gnrlTableDataService: GeneralTableClient
        ) {
        super();
    }

    sub: Subscription;

    contractId: any;

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    ngOnInit() {
        this.model = new RentalApplicationRegisterRequest();
        this.initializeForm();
        this.sub = this.route.params.subscribe(params => {

            //TODO:
            let id = params['rentalApplicationId'];
            if (id != null && typeof (id) != 'undefined') {
                //this.getBudgets();
                this.getRentalApplicationById(id); //TODO:
                this.flgEdition = "E";
                //this.allowEditing = this.model.contractStatusCode == "DRAFT" ? true : false;
                this._isDisabled = false;

            } else {
                this.flgEdition = "N";
                //this.model.contractId = -1;
                //this.isVisibleHouseFeature = false;
                //this.model.contractCode = "";
                //this.model.features = [];
                //this.model.contractStatusCode = "DRAFT";
                this._isDisabled = false;
            }

        });

    }

    //TODO: PARA LA EDICION
    getRentalApplicationById(id):void
    {
        //TODO: Hacer este servicio getById
        this.rentalApplicationClient.getById(id).subscribe(
            response => {
                var dataResult: any = response;
                this.model = dataResult.data;
                this.modelApplicationDate = this.getDateFromModel(this.model.applicationDate);
                this.modelCheckIn = this.getDateFromModel(this.model.checkIn);
                this.modelCheckOut = this.getDateFromModel(this.model.checkOut);
                this.modelAlertDate = this.getDateFromModel(this.model.alertDate);
            });
    }

    public getDateFromModel(dateFromModel: Date): modelDate {
        var model = new modelDate();
        if (dateFromModel != null && dateFromModel != undefined) {
            model.day = dateFromModel.getDate();
            model.month = dateFromModel.getMonth() + 1;
            model.year = dateFromModel.getFullYear();
        }
        else
            model = null;

        return model;
    }

    initializeForm(): void {
        this.resetFormError();
        //this.getPaymentMode();
        
        this.onGetCountries();
        this.getHouseTypes();
        this.getCompleteShared();
        this.getOutInDowntown();
        this.getBudgets();
        this.getReferredBy();
        this.getPriority();
    }

  onSelectModelApplicationDate(): void {
      if (this.modelApplicationDate != null) {
          this.model.applicationDate = new Date(this.modelApplicationDate.year, this.modelApplicationDate.month - 1, this.modelApplicationDate.day, 0, 0, 0, 0);
          this._formError.applicationDateError = false;

      }
      else {
          this.model.applicationDate = undefined; 
          this._formError.applicationDateError = true;
      }
  }

  onSelectModelCheckIn(): void {
      if (this.modelCheckIn != null) {
          this.model.checkIn = new Date(this.modelCheckIn.year, this.modelCheckIn.month - 1, this.modelCheckIn.day, 0, 0, 0, 0);
      }
      else {
          this.model.checkIn = undefined; 
      }

  }

  onSelectModelCheckOut(): void {
      if (this.modelCheckOut != null) {
          this.model.checkOut = new Date(this.modelCheckOut.year, this.modelCheckOut.month - 1, this.modelCheckOut.day, 0, 0, 0, 0);
      }
      else {
          this.model.checkOut = undefined; 
      }

  }

    onSelectModelAlertDate(): void {
        if (this.modelAlertDate != null) {
            this.model.alertDate = new Date(this.modelAlertDate.year, this.modelAlertDate.month - 1, this.modelAlertDate.day, 0, 0, 0, 0);
        }
        else {
            this.model.alertDate = undefined; 

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

        //this.setHouseFeatureToDB();
        if (this.isValidData()) {
            if (this.flgEdition == "N") {
                //NEW
                //this.model.contractStatusId = 2; //DRAFT
                this.model.rowStatus = true;
                this.rentalApplicationClient.register(this.model).subscribe(res => {
                    var dataResult: any = res;
                    this.successFlag = dataResult.isValid;
                    this.errorMessages = dataResult.messages;
                    this.successMessage = 'Rental Application was created';
                    //TODO: Permite descargar nuevamente la lista de HouseFeatures Asignados al contrato, 
                    //debe hacerse la llamada en el servidor al grabar, para evitar grabar informacion Features 
                    //que ya han sido asignados concurrentemente (por otra persona)
                    //this.getHouseFeatureDetailContract();
                    setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);
                    if (this.successFlag) {
                        
                        this.getRentalApplicationById(dataResult.pk);
                        this.flgEdition = "E";
                        this._isDisabled = false;
                    }
                    
                });
            }
            else {
                //UPDATE
                this.rentalApplicationClient.update(this.model).subscribe(res => {
                    var dataResult: any = res;
                    this.successFlag = dataResult.isValid;
                    this.errorMessages = dataResult.messages;
                    this.successMessage = 'Rental Application was Updated';
                    //TODO: Permite descargar nuevamente la lista de HouseFeatures Asignados al contrato, 
                    //debe hacerse la llamada en el servidor al grabar, para evitar grabar informacion Features 
                    //que ya han sido asignados concurrentemente (por otra persona)
                    //this.getHouseFeatureDetailContract();
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
        this.router.navigate(['leasing/rentalApp']);
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

    //GETTING DATA FOR DROPDOWNLIST

    getHouseTypes(): void {
        this.houseDataService.getHouseTypes()
            .subscribe(res => {
                var dataResult: any = res;
                this._listHouseTypes = dataResult.data;
            });
    }

    onGetCountries(): void {
        this.countryDataService.getCountriesAll()
            .subscribe(res => {
                var dataResult: any = res;
                this._listCountries = this.getCountries(dataResult.value.data);
            });
    }

    getCountries(data: any): any {
        var countries = [];
        var count = data.length;
        for (var i = 0; i < count; i++) {
            if (!this.containsDuplicates(data[i].isoCode, countries)) {
                countries.push({
                    "id": data[i].countryId,
                    "isoCode": data[i].isoCode,
                    "name": data[i].Name,
                });
            }
        }
        return countries;
    };

    containsDuplicates = function (v, data) {
        var count = data.length;
        for (var i = 0; i < count; i++) {
            if (data[i].isoCode === v) return true;
        }
        return false;
    }

    getBudgets(): void {
        this.gnrlTableDataService.getGeneralTableByTableNameAsync("BudgetRange")
            .subscribe(res => {
                
                let dataResult: any = res;
                this._listBudgets = [];
                for (let i = 0; i < dataResult.value.data.length; i++) {
                    this._listBudgets.push({
                        "typeId": dataResult.value.data[i].generalTableId,
                        "name": dataResult.value.data[i].value
                    });
                }
            });
    }

    getReferredBy(): void {
        this.gnrlTableDataService.getGeneralTableByTableNameAsync("ReferredBy")
            .subscribe(res => {
                
                var dataResult: any = res;
                this._listReferredBy = [];
                for (var i = 0; i < dataResult.value.data.length; i++) {
                    this._listReferredBy.push({
                        "typeId": dataResult.value.data[i].generalTableId,
                        "name": dataResult.value.data[i].value
                    });
                }
            });
    }

    getCompleteShared(): void {
        
        this.gnrlTableDataService.getGeneralTableByTableNameAsync("CompleteShared")
            .subscribe(res => {
                
                var dataResult: any = res;
                this._listCompleteShared = [];
                for (var i = 0; i < dataResult.value.data.length; i++) {
                    this._listCompleteShared.push({
                        "typeId": dataResult.value.data[i].generalTableId,
                        "name": dataResult.value.data[i].value
                    });
                }
            });
    }

    getOutInDowntown(): void {
        this.gnrlTableDataService.getGeneralTableByTableNameAsync("OutInDowntown")
            .subscribe(res => {
                
                var dataResult: any = res;
                this._listOutInDowntown = [];
                for (var i = 0; i < dataResult.value.data.length; i++) {
                    this._listOutInDowntown.push({
                        "typeId": dataResult.value.data[i].generalTableId,
                        "name": dataResult.value.data[i].value
                    });
                }
            });
    }

    getPriority(): void {
        this.gnrlTableDataService.getGeneralTableByTableNameAsync("Priority")
            .subscribe(res => {
                var dataResult: any = res;
                this._listPriority = [];
                for (var i = 0; i < dataResult.value.data.length; i++) {
                    this._listPriority.push({
                        "typeId": dataResult.value.data[i].generalTableId,
                        "name": dataResult.value.data[i].value
                    });
                }
            });
    }
    

    //TODO: Para validaciones
    isValidData(): boolean
    {
        var isValid = true;
        this.resetFormError();

        //if (this.model.tenantId == undefined || this.model.tenantId == 0 || this.model.tenantId == null) {
        //    this._formError.tenantError = true;
        //    isValid = false;
        //}
        if (this.modelApplicationDate == undefined && this.modelApplicationDate == null) {
            this._formError.applicationDateError = true;
            isValid = false;
        }
        if (this.model.fullName == undefined || this.model.fullName == null || this.model.fullName == '') {
            this._formError.fullNameError = true;
            isValid = false;
        }
        //if (this.modelCheckIn == undefined && this.modelCheckIn == null) {
        //    this._formError.endDateError = true;
        //    isValid = false;
        //}
        //if (this.model.houseId == undefined && this.model.houseId == null) {
        //    this._formError.houseError = true;
        //    isValid = false;
        //}
        //if (this.model.rentDeposit == undefined && this.model.rentDeposit == null) {
        //    this._formError.depositError = true;
        //    isValid = false;
        //}
        //if (this.model.rentPrice == undefined && this.model.rentPrice == null) {
        //    this._formError.rentError = true;
        //    isValid = false;
        //}
        //if (this.flgEdition == "N" && (this.model.contractHouseDetails == undefined || this.model.contractHouseDetails.length == 0 || this.model.contractHouseDetails == null)) {
        //    this._formError.contractHouseDetailError = true;
        //    isValid = false;
        //}
        //if (this.model.paymentModeId == undefined || this.model.paymentModeId == 0 || this.model.paymentModeId == null) {
        //    this._formError.paymentModeError = true;
        //    isValid = false;
        //}
        return isValid;
    }

    private resetFormError = () => {
        if (this._formError == undefined)
            this._formError = new FormError();

        this._formError.fullNameError = false;
        this._formError.applicationDateError = false;
        //this._formError.endDateError = false;
        //this._formError.houseError = false;
        //this._formError.depositError = false;
        //this._formError.rentError = false;
        //this._formError.contractHouseDetailError = false;
        //this._formError.paymentModeError = false;
    }


}
