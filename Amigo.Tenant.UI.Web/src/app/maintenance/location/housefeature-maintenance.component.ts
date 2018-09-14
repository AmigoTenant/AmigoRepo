import { Component, Input, Output, OnInit, EventEmitter, OnChanges, SimpleChange, ViewChild } from '@angular/core';
import { StatusDTO } from '../../model/statusDTO';
import { FeatureDTO, HouseClient, HouseFeatureRequest, FeatureClient, HouseSearchRequest } from "../../shared/index";

@Component({
    selector: 'at-house-feature',
    templateUrl: './housefeature-maintenance.component.html'
})
export class HouseFeatureMaintenanceComponent implements OnInit {
    @Output() onClickCloseDialog = new EventEmitter<boolean>();

    @Input() inputSelectedFeature: any;
    @Input() entityStatuses: StatusDTO[]; 
    @Input() features: FeatureDTO[];

    public housefeature = new HouseFeatureRequest();
    public auditModel: any;

    public flgEdition: string;

    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;
    public user: any;

    public selectedStatusId: number;
    public selectedFeatureId: number;
    public model = new HouseSearchRequest();
    
    constructor(private houseDataService: HouseClient, private featureDataService: FeatureClient) { }

    refreshAfterSaving: boolean;

    ngOnInit() {
        this.houseDataService.getHouseFeatureStatuses()
            .subscribe(res => {
                var dataResult: any = res;
                this.entityStatuses = dataResult.data;
            });

        this.featureDataService.searchFeatureAll("")
            .subscribe(res => {
                var dataResult: any = res;
                this.features = dataResult.data;
            });
        
        if(this.inputSelectedFeature != undefined && this.inputSelectedFeature != null) {
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
            this.onClearValidation();
            this.cleanFeatureForm();

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
                //break;
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
        if (this.flgEdition === 'N') {
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
                    }
                });
        }
    }

    private onClear(): void {
        this.housefeature = new HouseFeatureRequest();
        this.flgEdition = "N";
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


}