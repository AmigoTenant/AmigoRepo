import { Component, Input, Output, OnInit, EventEmitter, OnChanges, SimpleChange, ViewChild } from '@angular/core';
import { StatusDTO } from '../../model/statusDTO';
import { FeatureDTO, HouseClient, HouseFeatureRequest, FeatureClient, HouseSearchRequest, HouseFeatureDTO, EntityStatusClient } from "../../shared/index";
import { TypeDTO } from "../../model/typeDTO";
import { AuditModel } from "../../shared/common/audit.component";

export class FormError {
    featureError: boolean;
    statusError: boolean;
    rentError: boolean;
}

@Component({
    selector: 'at-house-feature',
    templateUrl: './housefeature-maintenance.component.html'
})
export class HouseFeatureMaintenanceComponent implements OnInit {
    @Output() onClickCloseDialog = new EventEmitter<boolean>();

    @Input() inputSelectedFeature: any;

    public houseStatuses: StatusDTO[];
    public features: FeatureDTO[];

    public housefeature = new HouseFeatureRequest();
    public auditModel: any;

    public flgEdition: string;
    _formError: FormError;

    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;
    public user: any;

    public selectedStatusId: number;
    public selectedFeatureId: number;
    public model = new HouseSearchRequest();
    
    constructor(private houseDataService: HouseClient, private featureDataService: FeatureClient, 
                private entityStatusDataService: EntityStatusClient) { }

    refreshAfterSaving: boolean;

    ngOnInit() {
        this.auditModel = new AuditModel();

        this.entityStatusDataService.getEntityStatusByEntityCodeAsync("HO")
            .subscribe(res => {
                var dataResult: any = res;
                this.houseStatuses = dataResult.data;

                this.featureDataService.searchFeatureAll(this.inputSelectedFeature.houseTypeCode)
                .subscribe(resx => {
                    var dataResultx: any = resx;
                    var result = dataResultx.value.data.filter(r => r.isAllHouse == false);
                    this.features = result;
                });
    
            });

        this.initializeForm();

        if(!this.inputSelectedFeature.isNew) {
            this.housefeature.houseFeatureId = this.inputSelectedFeature.houseFeatureId;
            this.housefeature.houseId = this.inputSelectedFeature.houseId;
            this.housefeature.featureId = this.inputSelectedFeature.featureId;
            this.housefeature.houseFeatureStatusId = this.inputSelectedFeature.houseFeatureStatusId;
            this.housefeature.isRentable = this.inputSelectedFeature.isRentable;
            this.housefeature.addressInfo = this.inputSelectedFeature.addressInfo;
            this.housefeature.userId = this.inputSelectedFeature.userId;
            this.housefeature.rentPrice = this.inputSelectedFeature.rentPrice;
            
            this.housefeature.rowStatus = this.inputSelectedFeature.rowStatus;
            this.housefeature.createdBy = this.inputSelectedFeature.createdBy;
            this.housefeature.creationDate = this.inputSelectedFeature.creationDate;

            this.auditModel.createdBy = this.inputSelectedFeature.createdBy;
            this.auditModel.creationDate = this.inputSelectedFeature.creationDate;
            this.auditModel.updatedBy = this.inputSelectedFeature.updatedBy;
            this.auditModel.updatedDate = this.inputSelectedFeature.updatedDate;

            this.flgEdition = "E";
        } else {
            this.cleanFeatureForm();
            this.housefeature.isRentable = this.inputSelectedFeature.isRentable;
            this.housefeature.houseId = this.inputSelectedFeature.houseId;
            this.housefeature.userId = this.inputSelectedFeature.userId;
            this.housefeature.rowStatus = this.inputSelectedFeature.rowStatus;
            this.housefeature.houseFeatureId = 0;

            this.onClearValidation();

            this.flgEdition = "N";
        }
    }

    public close() {
        this.onClickCloseDialog.emit(this.refreshAfterSaving);
    }
  
    onNew(): void {
        this.onClear();
        this.onClearValidation();
        //this.mainPopupOpen();
    }

    onExecuteEvent($event) {
        switch ($event) {
            case "s":
                this.onSave();
                break;
            case "c":
                //this.onClear();
                break;
            case "k":
                this.onClear();
                this.close();
                break;
            default:
                confirm("Sorry, that Event is not exists yet!");
        }
    }

    onSave = () => {

        if (this.isValidData()) {
            if (this.flgEdition === 'N') {
                this.housefeature.creationDate = new Date();
                this.housefeature.createdBy = this.inputSelectedFeature.userId;

                this.houseDataService.registerFeature(this.housefeature)
                    .subscribe(res => {
                        var dataResult: any = res;

                        this.successFlag = dataResult.isValid;
                        this.errorMessages = dataResult.messages;
                        this.successMessage = 'Feature added';
                        if (this.successFlag) {
                            this.refreshAfterSaving = true;

                            //---------------------------------------------
                            //      prepare the EDIT Form  register
                            //---------------------------------------------
                            this.flgEdition = "E";
                            this.close();
                        }
                    });
                console.log('INSERT');

            } else {
                this.houseDataService.updateFeature(this.housefeature)
                    .subscribe(res => {
                        var dataResult: any = res;

                        this.successFlag = dataResult.isValid;
                        this.errorMessages = dataResult.messages;
                        this.successMessage = 'Feature updated';
                        if (this.successFlag) {
                            this.refreshAfterSaving = true;

                            // prepare the EDIT Form  register
                            this.flgEdition = "E";
                            this.close();
                        }
                    });
            }
        }
    }

    private onClear(): void {
        this.housefeature = new HouseFeatureRequest();
        this.flgEdition = "N";
        this.refreshAfterSaving = false;
    }

    onClearValidation() {
        this.successFlag = null;
        this.errorMessages = null;
        this.successMessage = null;
    }

    cleanFeatureForm() {
        this.housefeature = new HouseFeatureRequest();
        this.housefeature.rowStatus = true;
    }

    initializeForm(): void {
        this.resetFormError();
    }

    isValidData(): boolean {
        var isValid = true;
        this.resetFormError();

        if (this.housefeature.featureId == undefined || this.housefeature.featureId == 0 ||  this.housefeature.featureId == null) {
            this._formError.featureError = true;
            isValid = false;
        }

        if (this.housefeature.houseFeatureStatusId == undefined || this.housefeature.houseFeatureStatusId == 0 || this.housefeature.houseFeatureStatusId == null) {
            this._formError.statusError = true;
            isValid = false;
        }

        if (this.housefeature.rentPrice == undefined || this.housefeature.rentPrice == null || this.housefeature.rentPrice == 0) {
            this._formError.rentError = true;
            isValid = false;
        }
        return isValid;
    }

    private resetFormError = () => {
        if (this._formError == undefined)
            this._formError = new FormError();

        this._formError.featureError = false;
        this._formError.statusError = false;
        this._formError.rentError = false;
    }

    onFeatureChange() {
        if (this.housefeature.featureId == undefined || this.housefeature.featureId == 0 || this.housefeature.featureId == null) {
            this._formError.featureError = true;
        } else
            this._formError.featureError = false;
    }

    onStatusChange() {
        if (this.housefeature.houseFeatureStatusId == undefined || this.housefeature.houseFeatureStatusId == 0 || this.housefeature.houseFeatureStatusId == null) {
            this._formError.statusError = true;
        } else
            this._formError.statusError = false;
    }

    onRentChange() {
        if (this.housefeature.rentPrice == undefined || this.housefeature.rentPrice == null || this.housefeature.rentPrice == 0) {
            this._formError.rentError = true;
        } else
            this._formError.rentError = false;
    }
}