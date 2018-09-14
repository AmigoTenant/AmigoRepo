import { Component, Input, Output, state, SimpleChange, ViewChild, EventEmitter, OnInit, OnDestroy} from '@angular/core';
import { Http, Jsonp, URLSearchParams} from '@angular/http';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { BotonsComponent } from '../../controls/common/boton.component';
import { RegisterLocationRequest, LocationDTO, LocationClient, ActionDTO, RegisterLocationCoordinateItem, LocationWithCoordinatesDTO } from '../../shared/api/services.client';

import { ValidationService } from '../../shared/validations/validation.service';
import { GeocodingService } from '../../shared/api';
import { DataService } from './dataService';
import {Observable, Subscription } from 'rxjs'
import { Router,ActivatedRoute } from '@angular/router';
import { Autofocus } from  '../../shared/directives/autofocus.directive';
declare var $: any;

@Component({
    selector: 'st-location-maintenance',
    templateUrl: './location-maintenance.component.html'
})

export class LocationMaintenanceComponent implements OnInit, OnDestroy {

    locationMaintenance: any;
    @Output() onCancelEvent = new EventEmitter<any>();
    @Input() SelectedCode: string;

    listState = [];
    listCity = [];
    listCoordinates = [];
    objValidate = [];
    cloneCoordinate = [];

    public listLocationType = [];
    public listParentLocation = [];
    private listStatesCities = [];
    public listCountries = [];
    private flgEdition: string;
    public successFlag: boolean;
    public errorMessages: string[];
    public successMessage: string;
    Location: LocationDTO;

    RegisterLocationCoordinateItem: RegisterLocationCoordinateItem[];

    constructor(private route: ActivatedRoute, private locationDataService: LocationClient,private geocodingService: GeocodingService, private dataservice: DataService,
        private router: Router) {
        this.locationMaintenance = new RegisterLocationRequest();
        this.locationMaintenance.hasGeofence = true;
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    sub: Subscription;

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            let code = params['code'];

            this.Location = DataService.currentlocation;

            if ((code == null || code == undefined) && this.Location != undefined) {
                code = this.Location.code;
            }
            
            if (code != null && typeof (code) != 'undefined') {
                this.getLocationByCode(code);
                this.flgEdition = "E";
            } else {
                this.flgEdition = "N";
            }

        });

        this.locationDataService.getLocationTypes()
            .subscribe(res => {
                var dataResult: any = res;
                this.listLocationType = dataResult.data;
            });

        this.locationDataService.getParentLocations()
            .subscribe(res => {
                var dataResult: any = res;
                this.listParentLocation = dataResult.data;

            });

        this.locationDataService.getCities()
            .subscribe(res => {
                var dataResult: any = res;
                this.listStatesCities = dataResult.data;
                this.listCountries = this.getCountries(dataResult.data);
            });

        $(window).ready((e) => { this.resizeMapContainer(); });

        $(window).bind('load resize scroll', (e) => {
            this.resizeMapContainer();
        });
    }


    containsDuplicates = function (v, data) {
        var count = data.length;
        for (var i = 0; i < count; i++) {
            if (data[i].countryISOCode === v) return true;
        }
        return false;
    }

    getCountries(data: any): any {

        var countries = [];
        var count = data.length;
        for (var i = 0; i < count; i++) {
            if (!this.containsDuplicates(data[i].countryISOCode, countries)) {
                countries.push({
                    "countryISOCode": data[i].countryISOCode,
                    "countryName": data[i].countryName,
                });
            }
        }
        return countries;
    };


    containsDuplicatesState = function (v, data) {
        var count = data.length;
        for (var i = 0; i < count; i++) {
            if (data[i].stateCode === v) return true;
        }
        return false;
    }

    containsDuplicatesCity = function (v, data) {
        var count = data.length;
        for (var i = 0; i < count; i++) {
            if (data[i].stateCode === v) return true;
        }
        return false;
    }

    onChangeCountry(selectedValue) {
        var state = [];
        var count = this.listStatesCities.length;
        for (var i = 0; i < count; i++) {
            if (this.listStatesCities[i].countryISOCode === selectedValue) {
                if (!this.containsDuplicatesState(this.listStatesCities[i].stateCode, state)) {
                    state.push({
                        "stateCode": this.listStatesCities[i].stateCode, "stateName": this.listStatesCities[i].stateName
                    });
                }
            }
        }
        this.listState = state;
        //return state
    }

    onChangeState(selectedValue) {
        var city = [];
        var count = this.listStatesCities.length;
        for (var i = 0; i < count; i++) {
            if (this.listStatesCities[i].stateCode === selectedValue) {
                if (!this.containsDuplicatesCity(this.listStatesCities[i].stateCode, city)) {
                    city.push({
                        "cityCode": this.listStatesCities[i].cityCode, "cityName": this.listStatesCities[i].cityName
                    });
                }
            }
        }
        this.listCity = city;
    }

    onChangeCity(selectedValue) {
    }

    private getLocationByCode(code): void {
        this.locationDataService.get(code)
            .subscribe(res => {
                var dataResult: any = res;
                this.locationMaintenance = dataResult.data;
                this.onChangeCountry(dataResult.data.countryISOCode);
                this.onChangeState(dataResult.data.stateCode);

                var count: number = dataResult.data.coordinates.length;
                this.listCoordinates, this.cloneCoordinate = [];
                for (var i = 0; i < count; i++) {
                    this.listCoordinates.push({ lat: dataResult.data.coordinates[i].latitude, lng: dataResult.data.coordinates[i].longitude })
                    this.cloneCoordinate.push({ lat: dataResult.data.coordinates[i].latitude, lng: dataResult.data.coordinates[i].longitude })
                }

                setTimeout(() => {
                    this.listCoordinates = []
                    this.listCoordinates = this.cloneCoordinate;
                }, 500);

            });
    }

    resetMaintenanceForm(): void {
        this.locationMaintenance = new RegisterLocationRequest();
        this.locationMaintenance.latitude = 28.9474017;
        this.locationMaintenance.longitude = -95.3483186;
        this.locationMaintenance.coordinates = [];
        this.listCoordinates = [];
        this.flgEdition = "N";
    }

    saveLocation = () => {

        if (this.flgEdition === "E") {

            this.locationDataService.update(this.locationMaintenance)
                .subscribe(res => {
                    var dataResult: any = res;
                    this.successFlag = dataResult.isValid;
                    this.errorMessages = dataResult.messages;
                    this.successMessage = 'Location  was Updated';

                });
        }
        else {
            this.locationDataService.register(this.locationMaintenance)
                .subscribe(res => {
                    var dataResult: any = res;
                    this.successFlag = dataResult.isValid;
                    this.errorMessages = dataResult.messages;
                    this.successMessage = 'Location  was created';
                });
        }
    }

    onRemove = () => {
        this.listCoordinates = [];
        this.locationMaintenance.coordinates = [];
    }

    undoPerimeter = () => {
        this.listCoordinates = this.cloneCoordinate;
    }

    onValidateAddress = () => {
        if (this.locationMaintenance.address1 != "" || this.locationMaintenance.address1 == null) {
            // Av. comandante espinar 2245, Miraflores, Lima
            //this.listCoordinates=[];
            this.geocodingService.codeAddress(this.locationMaintenance.address1)
                .subscribe((res) => {

                    var result = res;
                    var lat = result[0].geometry.location.lat();
                    var lng = result[0].geometry.location.lng();
                    this.locationMaintenance.latitude = lat;
                    this.locationMaintenance.longitude = lng;
                    //this.listCoordinates.push({lat:lat,lng:lng })
                });

        }
    }

    changeCoordinates = (module) => {

        this.RegisterLocationCoordinateItem = new Array<RegisterLocationCoordinateItem>();
        var count = module.length;
        for (var i = 0; i < count; i++) {

            var data = new RegisterLocationCoordinateItem({
                "Latitude": module[i].lat,
                "Longitude": module[i].lng,
                "LocationCode": ""
            });
            this.RegisterLocationCoordinateItem.push(data)
        }

        this.locationMaintenance.coordinates = this.RegisterLocationCoordinateItem
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
       this.router.navigateByUrl('maintenance/location');
    }


    //This Event came from BotonComponent
    onExecuteEvent($event) {
        switch ($event) {
            case "s":
                this.saveLocation();
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


}

interface markerCoordinate {
    lat: number;
    lng: number;
}