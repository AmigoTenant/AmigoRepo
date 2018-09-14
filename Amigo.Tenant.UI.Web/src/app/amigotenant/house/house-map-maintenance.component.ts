import { Component, Input, Output, OnInit, EventEmitter, OnChanges, SimpleChange, ViewChild } from '@angular/core';
//import { LatLngLiteral } from "angular2-google-maps/core";
import { environment } from "../../../environments/environment";

@Component({
    selector: 'at-house-map',
    templateUrl: './house-map-maintenance.component.html'
})
export class HouseMapMaintenanceComponent implements OnInit {
    @Output() onClickCloseDialogMap = new EventEmitter<boolean>();
    @Output() changeCoordinates =  new EventEmitter<any>();
    @Input() coordinates = new Array<any>();
    @Input() latitudeMap: number;
    @Input() longitudeMap: number;

    public refreshAfterSaving: boolean;
    public cloneCoordinates = new Array<any>();
    // public origCoordinates = new Array<LatLngLiteral>();
    public latitude: number;
    public longitude: number;

    ngOnInit() {
        if (this.coordinates.length == 0) {
            this.latitudeMap = environment.latitude;
            this.longitudeMap = environment.longitude;
        } else {
            this.cloneCoordinates = this.coordinates.slice(0, 1); 
            this.latitude = this.coordinates[0].lat;
            this.longitude = this.coordinates[0].lng;

            this.latitudeMap = this.coordinates[0].lat == null ? environment.latitude : this.coordinates[0].lat;
            this.longitudeMap = this.coordinates[0].lng == null ? environment.longitude : this.coordinates[0].lng;
        }
    }

    public close() {
        this.onClickCloseDialogMap.emit(this.refreshAfterSaving);
    }

    public onRemove() {
        this.coordinates = [];
        this.latitude = null;
        this.longitude = null;
    }

    public undoPerimeter() {
        this.coordinates = this.cloneCoordinates.slice(0, 1);
        this.latitude = this.coordinates[0].lat;
        this.longitude = this.coordinates[0].lng;
    }

    public onChangeCoordinates(coord: any) {
        this.cloneCoordinates = [];
        this.cloneCoordinates.push({
            lat: this.latitude,
            lng: this.longitude
        });

        this.latitude = coord.lat;
        this.longitude = coord.lng;
    }

    public onSave() {
        this.changeCoordinates.emit({
            lat: this.latitude,
            lng: this.longitude
        });
        this.close();
    }

    public onCancel() {
        this.changeCoordinates.emit(this.coordinates[0]);
        this.close();
    }
}
