import { Component, Input, Output, state, SimpleChange, ViewChild, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { Http, URLSearchParams } from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { BotonsComponent } from '../../controls/common/boton.component';
import { RegisterHouseRequest, UpdateHouseRequest, HouseDTO, HouseClient, ActionDTO, RegisterLocationCoordinateItem, LocationWithCoordinatesDTO } from '../../shared/api/services.client';

import { ValidationService } from '../../shared/validations/validation.service';
import { GeocodingService, HouseSearchRequest, FeatureDTO, HouseFeatureDTO, DeleteHouseFeatureRequest, LocationClient } from '../../shared/api';
import { DataService } from './dataService';
import { Observable, Subscription } from 'rxjs'
import { Router, ActivatedRoute } from '@angular/router';
import { Autofocus } from '../../shared/directives/autofocus.directive';
//import { LatLngLiteral } from "angular2-google-maps/core";
import { HouseServiceRequest, HouseServiceClient, DeleteHouseServiceRequest } from '../../shared/api/amigotenant.service';
import { HouseServiceDTO } from '../../shared/dtos/House/HouseServiceDTO';

declare var $: any;

@Component({
    selector: 'at-house-maintenance',
    templateUrl: './house-maintenance.component.html'
})

export class HouseMaintenanceComponent implements OnInit, OnDestroy {

    houseMaintenance: any;
    selectedFeature: HouseFeatureDTO;
    selectedHouseService: HouseServiceRequest;

    @Output() onCancelEvent = new EventEmitter<any>();
    @Output() onEditItem: EventEmitter<any> = new EventEmitter<any>();
    @Output() onAddItem: EventEmitter<any> = new EventEmitter<any>();

    coordinates = [];
    cloneCoordinates = [];
    public houseFeatures: HouseFeatureDTO[];
    public houseServices: HouseServiceDTO[];
    public houseTypes = [];
    public houseStatuses = [];
    public cities = [];

    public flgEdition: string;
    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;
    public House: HouseDTO;
    private initialAddress: string;

    public RegisterLocationCoordinateItem: RegisterLocationCoordinateItem[];
    public openDialog: boolean = false;
    public openDialogMap: boolean = false;
    public openDialogHouseService: boolean = false;

    public model = new HouseSearchRequest();

    constructor(private route: ActivatedRoute, 
        private houseDataService: HouseClient, 
        private houseServiceDataService: HouseServiceClient, 
        private geocodingService: GeocodingService, 
        private dataservice: DataService,
        private locationService: LocationClient,
        private router: Router) {
            this.houseMaintenance = new RegisterHouseRequest();
            this.houseMaintenance.hasGeofence = false;
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    sub: Subscription;

    ngOnInit() {
        this.locationService.getCities()
            .subscribe(res => {
                var dataResult: any = res;
                this.cities = dataResult.data;

                this.houseDataService.getHouseTypes()
                    .subscribe(res => {
                        var dataResult: any = res;
                        this.houseTypes = dataResult.data;

                        this.houseDataService.getHouseStatuses()
                            .subscribe(res => {
                                var dataResult: any = res;
                                this.houseStatuses = dataResult.data;
                                // this.listHouseStatus = this.getCountries(dataResult.data);

                                this.locationService.getCities()
                                .subscribe(res => {
                                    var dataResult: any = res;
                                    this.cities = dataResult.data;
                                    
                                    this.sub = this.route.params.subscribe(params => {
                                        let id = params['id'];
    
                                        this.House = DataService.currentHouse;
    
                                        if ((id == null || id == undefined) && this.House != undefined) {
                                            id = this.House.houseId;
                                        }
    
                                        if (id != null && typeof (id) != 'undefined') {
                                            this.getHouseByCode(id);
                                            this.flgEdition = "E";
                                        } else {
                                            this.flgEdition = "N";
                                        }
    
                                    });
                                });
                        });
                });

        });

        $(window).ready((e) => { this.resizeMapContainer(); });

        $(window).bind('load resize scroll', (e) => {
            this.resizeMapContainer();
        });
    }

    private getHouseByCode(code): void {
        this.houseDataService.get(code)
            .subscribe(res => {
                var dataResult: any = res;
                var request = new UpdateHouseRequest();
                this.houseMaintenance = request.createFromDTO(dataResult.data);
                this.houseMaintenance.houseTypeCode = dataResult.data.houseTypeCode;
                this.coordinates, this.cloneCoordinates = [];
                this.coordinates.push({
                    lat: dataResult.data.latitude,
                    lng: dataResult.data.longitude
                });
                this.cloneCoordinates.push({
                    lat: dataResult.data.latitude,
                    lng: dataResult.data.longitude
                });
                this.initialAddress = this.houseMaintenance.address;
                this.getFeaturesByHouse(this.houseMaintenance.houseId);
                this.getServicesByHouse(this.houseMaintenance.houseId);
            });
    }

    private getFeaturesByHouse(houseId: number) {
        this.houseDataService.getFeaturesByHouse(houseId)
            .subscribe(resFeat => {
                var dataFeatures: any = resFeat;
                this.houseFeatures = dataFeatures.data;
            });
    }

    private getServicesByHouse(houseId: number) {
        var today = new Date();
        this.houseServiceDataService.getHouseServicesByHouse(houseId, today.getFullYear())
        .subscribe(response => {
            var dataHouseServices: any = response;
            this.houseServices = dataHouseServices.data;
        });
    }

    resetMaintenanceForm(): void {
        this.houseMaintenance = new RegisterHouseRequest();
        // this.houseMaintenance.latitude = 28.9474017;
        // this.houseMaintenance.longitude = -95.3483186;
        this.houseMaintenance.coordinates = [];
        this.coordinates = [];
        this.flgEdition = "N";
    }

    public saveHouse() {

        if (this.flgEdition === "E") {
            this.houseDataService.update(this.houseMaintenance)
                .subscribe(res => {
                    var dataResult: any = res;
                    this.successFlag = dataResult.isValid;
                    this.errorMessages = dataResult.messages;
                    this.successMessage = 'House was Updated';
                    if (this.successFlag) {
                        this.getHouseByCode(dataResult.pk);
                    }
                });
        }
        else {
            this.houseDataService.register(this.houseMaintenance)
                .subscribe(res => {
                    var dataResult: any = res;
                    this.successFlag = dataResult.isValid;
                    this.errorMessages = dataResult.messages;
                    this.successMessage = 'House was created';
                    //setTimeout(() => { this.successFlag = null; this.errorMessages = null; this.successMessage = null; }, 5000);
                    if (this.successFlag) {
                        this.getHouseByCode(dataResult.pk);
                        this.flgEdition = "E";
                    }
                });
        }
    }

    onRemove = () => {
        this.coordinates = [];
        this.houseMaintenance.coordinates = [];
    }

    undoPerimeter = () => {
        this.coordinates = this.cloneCoordinates;
    }

    onValidateAddress = () => {
        if (this.houseMaintenance.address1 != "" || this.houseMaintenance.address1 == null) {
            this.geocodingService.codeAddress(this.houseMaintenance.address1)
                .subscribe((res) => {

                    var result = res;
                    var lat = result[0].geometry.location.lat();
                    var lng = result[0].geometry.location.lng();
                    this.houseMaintenance.latitude = lat;
                    this.houseMaintenance.longitude = lng;
                });
        }
    }

    public onChangeCoordinates(coord: number) {
        this.houseMaintenance.latitude = 51.678418; //coord.lat;
        this.houseMaintenance.longitude = 7.809007; //coord.lng;
    }

    private resizeMapContainer() {
        var _combinedPageElementsHeight = 0;
        var _viewportHeight = 0;
        var mapContainer = $(".sebm-google-map-container");
        $.each($(mapContainer).parent().parent().siblings().not("kendo-dialog"), (e, v) => {
            _combinedPageElementsHeight += $(v).outerHeight(true);
        });
        _combinedPageElementsHeight += $(".menu-top").outerHeight();
        _combinedPageElementsHeight += $(".page-header").outerHeight();
        _viewportHeight += $(window).outerHeight() - _combinedPageElementsHeight;
        $(mapContainer).height(_viewportHeight);
    }

    checked() { }

    onCancel() {
        this.router.navigateByUrl('amigotenant/house');
    }

    //This Event came from BotonComponent
    onExecuteEvent($event) {
        switch ($event) {
            case "s":
                this.saveHouse();
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

    onReset(): void {
        //   this.model = new SearchCriteriaTenant();
        //   // this.typeaheadUserCleanValue();
        //   this.getTenants();
        //   //this.currentUser = null;
    }

    //-------------------------------------------------------------------------
    //--------------------------    Maintenance     ---------------------------
    //-------------------------------------------------------------------------

    //#region House Feature
    public editItem(data): void {
        this.selectedFeature = data;
        this.selectedFeature.isNew = false;
        this.selectedFeature.houseTypeCode = this.houseMaintenance.houseTypeCode;
        this.openDialog = true;
    }

    public addFeature() {
    }

    public addItem() {
        this.selectedFeature = new HouseFeatureDTO();
        this.selectedFeature.houseId = this.houseMaintenance.houseId;
        this.selectedFeature.rowStatus = true;
        this.selectedFeature.isRentable = true;
        this.selectedFeature.isNew = true;
        this.selectedFeature.houseTypeCode = this.houseMaintenance.houseTypeCode;
        this.openDialog = true;
    }

    public housefeatureToDelete: DeleteHouseFeatureRequest;

    public deleteMessage: string;
    
    public deleteItem(housefeatureToDelete) {
        this.housefeatureToDelete = new DeleteHouseFeatureRequest();
        this.housefeatureToDelete.houseFeatureId = housefeatureToDelete.houseFeatureId;
        this.housefeatureToDelete.houseId = housefeatureToDelete.houseId;
        this.deleteMessage = "Delete HouseFeature?";
        this.openedDeletionConfimation = true;
    }

    public onConfirmation(statusCode: string) {
        var confirmDeletionResponse = (statusCode === "YES");

        if (confirmDeletionResponse) {
            this.houseDataService.deleteFeature(this.housefeatureToDelete)
                .subscribe(res => {
                    var dataResult: any = res;
                    if (dataResult.isValid) {
                        this.getFeaturesByHouse(this.houseMaintenance.houseId);
                        this.openedDeletionConfimation = false;
                    }
                });
        }
    }

    public openedDeletionConfimation: boolean = false;
    
    public closeDeletionConfirmation(actionCode: string) {
        this.openedDeletionConfimation = false;
    }
        
    public onClickCloseDialog(refreshGridAfterSaving: boolean) {
        this.openDialog = false;

        if (refreshGridAfterSaving) {
            this.getFeaturesByHouse(this.houseMaintenance.houseId);
        }
    }
    //#endregion

    //#region House Service
    public addHouseService() {
        this.selectedHouseService = HouseServiceRequest.create(this.houseMaintenance.houseId, 0, this.houseMaintenance.createdBy, 0);        
        this.openDialogHouseService = true;        
    }
               
    public onClickCloseHouseService(refreshGridAfterSaving: boolean) {
        this.openDialogHouseService = false;
    }

    editService(data) : void {
        this.selectedHouseService = data;
        this.selectedHouseService.isNew = false;
        this.openDialogHouseService = true;
    }

    public houseServiceToDelete: DeleteHouseServiceRequest;

    deleteService(data) : void {
        this.houseServiceToDelete = new DeleteHouseServiceRequest();
        this.houseServiceToDelete.houseServiceId = data.houseServiceId;
        this.houseServiceToDelete.houseId = data.houseId;
        this.deleteMessage = "Delete House Service?";
        this.openedDeletionConfimationService = true;
    }

    public openedDeletionConfimationService: boolean = false;
    
    public closeDeletionConfirmationService(actionCode: string) {
        this.openedDeletionConfimationService = false;
    }

    public onConfirmationService(statusCode: string) {
        var confirmDeletionResponse = (statusCode === "YES");

        if (confirmDeletionResponse) {
            this.houseServiceDataService.deleteHouseService(this.houseServiceToDelete)
                .subscribe(res => {
                    var dataResult: any = res;
                    if (dataResult.isValid) {
                        this.getServicesByHouse(this.houseMaintenance.houseId);
                        this.openedDeletionConfimationService = false;
                    }
                });
        }
    }
        
    //#endregion

    //#region Map Info
    public setMapInfo() {
        this.coordinates = [];

        this.coordinates.push({
            lat: this.houseMaintenance.latitude,
            lng: this.houseMaintenance.longitude
        });
        
        this.openDialogMap = true;
    }

    public searchMapInfo() {
        this.coordinates = [];                    
        if (this.houseMaintenance.address != "" || this.houseMaintenance.address == null) {
            this.geocodingService.codeAddress(this.houseMaintenance.address)
                .subscribe((res) => {
                    //debugger;
                    var result = res;
                    var lat = result[0].geometry.location.lat();
                    var lng = result[0].geometry.location.lng();

                    // this.locationMaintenance.latitude = lat;
                    // this.locationMaintenance.longitude = lng;
                    this.coordinates = [];                    
                    this.coordinates.push({
                        lat: lat,
                        lng: lng
                    });                    
                    this.openDialogMap = true;
                });
       }    
    }

    public onClickCloseDialogMap(refreshGridAfterSaving: boolean) {
        this.openDialogMap = false;
    }
    //#endregion

    //#region Pagination
    public buttonCount: number = 20;
    public info: boolean = true;
    public type: 'numeric' | 'input' = 'numeric';
    public pageSizes: any = [20, 50, 100, 200];
    public previousNext: boolean = true;
    public currentPage: number = 0;
    public skip: number = 0;

    public pageChange1({ skip, take }: PageChangeEvent): void {
        this.currentPage = skip;
        this.model.pageSize = take;
        let isExport: boolean = false;
        this.getFeaturesByHouse(this.houseMaintenance.houseId);
    }
    //#endregion
    
    onTypeChange(item) {
        this.houseMaintenance.houseTypeCode = item.code;
    }

}

interface markerCoordinate {
    lat: number;
    lng: number;
}