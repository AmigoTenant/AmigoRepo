import { Component, Input, Output, OnInit, EventEmitter, OnChanges, SimpleChange, ViewChild} from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import {Http, Jsonp, URLSearchParams} from '@angular/http';
//import { AmigoTenantTServiceRequest } from '../../shared/api/services.client';
////import { AmigoTenanttServiceClient, AmigoTenantTServiceRequest } from '../../shared/api/services.client';
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
//import { NgbdTypeaheadLocationNew } from '../../shared/typeahead-locationnew/typeahead-locationnew';
import { AuthCheckDirective } from  '../../shared/security/auth-check.directive';
//import { LocationTypeAheadDTO, ProductDTO, CostCenterTypeAheadDTO } from '../../shared/api/services.client';
import { LocationTypeAheadDTO } from '../../shared/api/services.client';
import { EnvironmentComponent } from  '../../shared/common/environment.component';
import { Audit, AuditModel } from '../../shared/common/audit.component';
declare var moment: any;

export class Type {
    code: string;
    name: string;
}


export class modelServiceDate {
    year: number;
    month: number;
    day: number;
}

export class modelServiceTime {
    hour: number;
    minute: number;
    second: number;
}


@Component({
    selector: 'st-approve-service-maintenance',
    templateUrl: './approve-service-maintenance.component.html'
})

export class ApproveServiceMaintenanceComponent extends EnvironmentComponent implements OnInit {

    //constructor(private serviceClient: AmigoTenanttServiceClient) { super(); }

    public showModal = false;
    public modelServiceDateStart: modelServiceDate; //any;
    public modelServiceDateFinish: modelServiceDate; //any;
    public modelServiceTimeStart: modelServiceTime;
    public modelServiceTimeFinish: modelServiceTime;
    public currentOriginLocation: any;
    public currentDestinationLocation: any;
    public currentProduct: any;
    public currentCostCenter: any;
    //@ViewChild(NgbdTypeaheadLocation) ngbdTypeaheadLocation: NgbdTypeaheadLocation;

    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;

    public validationSuccessFlag: boolean;
    public validationErrorMessages: any[];
    public validationSuccessMessage: string;

    public mainPopupOpened: boolean = false;
    public mainPopupClose() {
        this.mainPopupOpened = false;
    }
    public mainPopupOpen() {
        this.mainPopupOpened = true;
    }


    @Input() eventNew: string;
    @Input() listEquipmentStatus = [];
    @Input() listMoveTypes = [];
    @Input() isValidToApprove: boolean;

    @Output() addItem = new EventEmitter<any>();
    @Output() itemSelected = new EventEmitter<any>();
    @Output() itemSaved = new EventEmitter<any>();

    ActionDTO: any[];
    flgEdition: string;
    isOn: boolean = true;
    maintenance: any; // = new AmigoTenantTServiceRequest();
    auditModel: any;

    listChargeTypes: Type[] = [
        { code: "C", name: 'Cost Center' },
        { code: "S", name: 'Shipment ID' },
    ];

    public transactionSuccess: boolean;
    public validationMessages: string[];

    showEquipmentNumber: boolean;
    showChassisNumber: boolean;
    showEquipmentStatus: boolean;
    showMoveType: boolean;
    showToBlock: boolean;
    showProduct: boolean;
    showHasH34: boolean;
    hasAudit: boolean = false;
    public chargeNumberDisabled: boolean = false;

    showCostCenterCode: boolean;

    // Verify the Changes Events of @Input
    ngOnChanges(changes: { [propKey: string]: SimpleChange }) {

        if (this.eventNew != null) {
            //this.onClear();
        }
    }

    ngOnInit() {

    }

    selectService = (amigoTenantTservice) => {
        this.getServiceById(amigoTenantTservice.amigoTenantTServiceId);
        this.flgEdition = "E";
        this.mainPopupOpen();
    };

    // newModule = (module) => {
    //     this.flgEdition = "A";
    //     this.mainPopupOpen();

    // };


    // TODO
    // TODO
    // TODO
    public getServiceById(id): void {
        this.onGeneralMessageClear();
        this.onValidationMessageClear();
        this.hasAudit = false;
        //this.serviceClient.searchById(id)
        //    .subscribe(res => {
        //        var dataResult: any = res;                
        //        this.maintenance = dataResult.data;
        //        this.hasAudit = true;
        //        this.auditModel = new AuditModel();
        //        this.auditModel.createdBy = dataResult.data.createdBy;
        //        this.auditModel.creationDate = dataResult.data.creationDate;
        //        this.auditModel.updatedBy = dataResult.data.updatedBy;
        //        this.auditModel.updatedDate = dataResult.data.updatedDate;
        //    }
        //    );
    }

    public getServiceByIdAfterSave(id): void {
        //this.serviceClient.searchById(id)
        //    .subscribe(res => {
        //        var dataResult: any = res;
        //        this.maintenance = dataResult.data;
        //    }
        //    );
    }

    onChangeChargeType(selectedValue) {
        this.onShowCostCenterCode(selectedValue);
        this.typeaheadCostCenterCleanValue();
        this.maintenance.costCenterCode = undefined;
    }

    // public onUpdate(service): void {

    //     this.serviceClient.update(service)
    //         .subscribe(res => {
    //             var dataResult: any = res;
    //             this.getServiceById(service.amigoTenantTserviceId);
    //         });

    // }

    //public skip: number = 0;

    public onClear(): void {

        this.maintenance = [];//new AmigoTenantTServiceRequest();
        //this.resetGrid();
    }

    public onValidationMessageClear(): void {

        this.validationSuccessFlag = null;
        this.validationErrorMessages = null;
        this.validationSuccessMessage = null;
    }
    public onGeneralMessageClear(): void {

        this.successFlag = null;
        this.errorMessages = null;
        this.successMessage = null;
    }

    writeMessages(type, msg) {
        if (type === 'E') {
            this.validationSuccessFlag = false;
            this.validationErrorMessages = [{ key: null, message: msg }];
        } else if (type === 'I') {
            this.validationSuccessMessage = msg;
        }
    }
    public onSave(): void {
        let service = this.maintenance;
        if (service.chargeType == "C" && (service.costCenterCode == undefined || service.costCenterCode == null || service.costCenterCode == "")) {
            this.onGeneralMessageClear();
            this.writeMessages("E", "Charge Number is required.");
            return;
        }
        if (service.chargeType == "S" && service.shipmentID == "") {
            this.onGeneralMessageClear();
            this.writeMessages("E", "ShipmentID is required.");
            return;
        }
        if (this.modelServiceTimeStart.hour > this.modelServiceTimeFinish.hour ||
            (this.modelServiceTimeStart.hour == this.modelServiceTimeFinish.hour &&
                this.modelServiceTimeStart.minute > this.modelServiceTimeFinish.minute) ||
            (this.modelServiceTimeStart.hour == this.modelServiceTimeFinish.hour &&
                this.modelServiceTimeStart.minute == this.modelServiceTimeFinish.minute &&
                this.modelServiceTimeStart.second > this.modelServiceTimeFinish.second)) {
            this.onGeneralMessageClear();
            this.writeMessages("E", "Start Time should be lower than Finish Time.");
            return;
        }
        if (this.flgEdition === "E") {
            //------------------------------
            //      process EDIT form
            //------------------------------
            this.onCleanCostCenterOrShipment(this.maintenance.chargeType);
            //this.serviceClient.updateAmigoTenantTServiceForApprove(service)
            //    .subscribe(res => {
            //        var dataResult: any = res;
            //        this.onValidationMessageClear();
            //        this.successFlag = dataResult.isValid;
            //        this.errorMessages = dataResult.messages;
            //        this.successMessage = "The record was saved successfully!.";
            //        this.getServiceByIdAfterSave(service.amigoTenantTServiceId);
            //        this.itemSaved.emit();
            //    });

        }

    }

    sendDataToChild(data) {
        //debugger;
        this.getServiceById(data.amigoTenantTServiceId);
        this.flgEdition = "E";
        this.mainPopupOpen();
        this.onShowFields(data.serviceTypeCode);
        this.modelServiceDateStart = this.GetDateFromModel(data.localServiceStartDate);
        this.modelServiceDateFinish = this.GetDateFromModel(data.localServiceFinishDate);
        this.onShowCostCenterCode(data.chargeType);
        this.modelServiceTimeStart = this.GetTimeFromModel(data.localServiceStartDate);
        this.modelServiceTimeFinish = this.GetTimeFromModel(data.localServiceFinishDate);
        this.isValidToApprove = data.serviceStatus === null ? false : true;
        this.onSetCurrentOriginLocation(data.originLocationId, data.originLocationName);
        this.onSetCurrentCostCenter(0, data.chargeNo, data.chargeNo);
        this.onSetCurrentDestinationLocation(data.destinationLocationId, data.destinationLocationName);
        this.onSetCurrentProduct(data.productId, data.productName);
    }

    public GetTimeFromModel(serviceTime: Date): modelServiceTime {
        var model = new modelServiceTime();
        if (serviceTime != null && serviceTime != undefined) {
            model.second = serviceTime.getSeconds();
            model.minute = serviceTime.getMinutes();
            model.hour = serviceTime.getHours();
        }
        else {
            model = null;
        }
        return model;
    }

    public GetDateFromModel(serviceStartDate: Date): modelServiceDate {
        var model = new modelServiceDate();
        if (serviceStartDate != null && serviceStartDate != undefined) {
            model.day = serviceStartDate.getDate();
            model.month = serviceStartDate.getMonth() + 1;
            model.year = serviceStartDate.getFullYear();
        }
        else
            model = null;

        return model;
    }

    getProduct = (item) => {
        if (item != null && item != undefined)
            this.maintenance.productId = item.productId;
        else
            this.maintenance.productId = null;
    };


    public onShowFields(serviceTypeCode) {
        this.showEquipmentNumber = true;
        this.showChassisNumber = true;
        this.showEquipmentStatus = true;
        this.showMoveType = true;
        this.showToBlock = true;
        this.showProduct = true;
        this.showHasH34 = true;

        if (serviceTypeCode === "MOV") {
            this.showEquipmentNumber = true;
            this.showChassisNumber = true;
            this.showEquipmentStatus = true;
            this.showMoveType = true;
            this.showToBlock = true;
            this.showProduct = true;
            this.showHasH34 = false;
        }

        if (serviceTypeCode === "ADS") {
            this.showEquipmentNumber = true;
            this.showChassisNumber = true;
            this.showEquipmentStatus = true;
            this.showMoveType = true;
            this.showToBlock = false;
            this.showProduct = true;
            this.showHasH34 = true;
        }

        if (serviceTypeCode === "DET") {
            this.showEquipmentNumber = true;
            this.showChassisNumber = true;
            this.showEquipmentStatus = true;
            this.showMoveType = false;
            this.showToBlock = false;
            this.showProduct = true;
            this.showHasH34 = false;
        }

        if (serviceTypeCode === "LFT") {
            this.showEquipmentNumber = true;
            this.showChassisNumber = true;
            this.showEquipmentStatus = true;
            this.showMoveType = false;
            this.showToBlock = false;
            this.showProduct = true;
            this.showHasH34 = false;
        }


    }

    public onShowCostCenterCode(chargeType) {
        this.showCostCenterCode = true;
        this.chargeNumberDisabled = true;
        if (chargeType === "S") {
            this.showCostCenterCode = false;
            this.chargeNumberDisabled = false;
        }

    }

    public onCleanCostCenterOrShipment(chargeType) {
        if (chargeType === "S") {
            this.maintenance.costCenterCode = "";
            this.chargeNumberDisabled = false;
        }
        else
            this.maintenance.shipmentID = null;
    }

    onSelectModelServiceTimeStart(): void {
        var tmpDate = this.maintenance.localServiceStartDate;

        if (this.modelServiceTimeStart != null) {
            var hours = this.modelServiceTimeStart.hour - tmpDate.getHours();
            var minutes = this.modelServiceTimeStart.minute - tmpDate.getMinutes();
            var seconds = this.modelServiceTimeStart.second - tmpDate.getSeconds();
            tmpDate.setHours(this.maintenance.localServiceStartDate.getHours() + hours);
            tmpDate.setMinutes(this.maintenance.localServiceStartDate.getMinutes() + minutes);
            tmpDate.setSeconds(this.maintenance.localServiceStartDate.getSeconds() + seconds);
            this.maintenance.localServiceStartDate = tmpDate;

        } else {
            var model = new modelServiceTime();
            var date = new Date();
            model.second = date.getSeconds();
            model.minute = date.getMinutes();
            model.hour = date.getHours();
            this.modelServiceTimeStart = model;
            this.maintenance.localServiceStartDate = date;
            this.modelServiceDateStart = this.GetDateFromModel(date);
        }
    }


    onSelectModelServiceTimeFinish(): void {
        if (this.maintenance != null && this.maintenance.localServiceFinishDate != null) {
            var tmpDate = this.maintenance.localServiceFinishDate;

            if (this.modelServiceTimeFinish != null) {

                var hours = this.modelServiceTimeFinish.hour - tmpDate.getHours();
                var minutes = this.modelServiceTimeFinish.minute - tmpDate.getMinutes();
                var seconds = this.modelServiceTimeFinish.second - tmpDate.getSeconds();
                tmpDate.setHours(this.maintenance.localServiceFinishDate.getHours() + hours);
                tmpDate.setMinutes(this.maintenance.localServiceFinishDate.getMinutes() + minutes);
                tmpDate.setSeconds(this.maintenance.localServiceFinishDate.getSeconds() + seconds);
                this.maintenance.localServiceFinishDate = tmpDate;

            } else {
                var model = new modelServiceTime();
                var date = new Date();
                model.second = date.getSeconds();
                model.minute = date.getMinutes();
                model.hour = date.getHours();
                this.modelServiceTimeFinish = model;
                this.maintenance.localServiceFinishDate = date;

                this.modelServiceDateFinish = this.GetDateFromModel(date);
            }
        } else {
            var model = new modelServiceTime();
            var date = new Date();
            model.second = date.getSeconds();
            model.minute = date.getMinutes();
            model.hour = date.getHours();
            this.modelServiceTimeFinish = model;
            this.maintenance.localServiceFinishDate = date;

            this.modelServiceDateFinish = this.GetDateFromModel(date);
        }
    }

    getOriginLocation = (item) => {
        if (item != null && item != undefined)
            this.maintenance.originLocationId = item.locationId;
        else
            this.maintenance.originLocationId = 0;
    };

    getDestinationLocation = (item) => {
        if (item != null && item != undefined)
            this.maintenance.destinationLocationId = item.locationId;
        else
            this.maintenance.destinationLocationId = 0;
    };

    public onSetCurrentOriginLocation(id, name) {
        this.currentOriginLocation = new LocationTypeAheadDTO();
        this.currentOriginLocation.name = name;
        this.currentOriginLocation.locationId = id;
        this.currentOriginLocation.code = '';
    }

    public onSetCurrentDestinationLocation(id, name) {
        this.currentDestinationLocation = new LocationTypeAheadDTO();
        this.currentDestinationLocation.name = name;
        this.currentDestinationLocation.locationId = id;
        this.currentDestinationLocation.code = '';
    }

    public onSetCurrentProduct(id, name) {
        //this.currentProduct = new ProductDTO();
        //this.currentProduct.name = name;
        //this.currentProduct.productId = id;
        //this.currentProduct.code = '';
    }

    public getCostCenter = (item) => {
        this.maintenance.costCenterCode = undefined;
        if (item != null && item != undefined && item != '') {
            this.maintenance.costCenterCode = item.code;
        }
    };

    public typeaheadCostCenterCleanValue(): void {
        //this.currentCostCenter = new CostCenterTypeAheadDTO();
        //this.currentCostCenter.costCenterIdId = 0;
        //this.currentCostCenter.name = '';
        //this.currentCostCenter.code = '';
    }

    public onSetCurrentCostCenter(id, name, code) {
        //this.currentCostCenter = new CostCenterTypeAheadDTO();
        //this.currentCostCenter.costCenterIdId = id;
        //this.currentCostCenter.name = name;
        //this.currentCostCenter.code = code;
    }
}