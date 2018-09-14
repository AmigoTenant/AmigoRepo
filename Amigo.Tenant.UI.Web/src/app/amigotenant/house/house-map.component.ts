import { Component, OnInit, Input,Output,EventEmitter } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
//import { AgmCoreModule, SebmGoogleMap, SebmGoogleMapPolygon, LatLngLiteral } from 'angular2-google-maps/core';
import { AgmCoreModule, LatLngLiteral } from '@agm/core';
declare var $: any;

@Component({
    selector: 'house-map-component',
    styles: [`
    .sebm-google-map-container {
       height: 450px;
       width: 800px;
     }
  `],
    template: `
    <div></div>
`})
export class HouseMapComponent implements OnInit {

    @Input() coordinates: Array<LatLngLiteral>;
    @Input() iLatitudeMap: number;
    @Input() iLongitudeMap: number;
    @Output() changeCoordinates =  new EventEmitter<LatLngLiteral>();

    fill: string = "#FE2E2E";
    // google maps zoom level
    zoom: number = 11;
    
    constructor( ) { 
    }

    ngOnInit() {
    }

    clickedMarker(label: string, index: number) {
        console.log(`clicked the marker: ${label || index}`)
    }

    mapClicked = ($event:any)=> {
        this.coordinates = new Array<LatLngLiteral>();
        var coord = {
            lat: $event.coords.lat,
            lng: $event.coords.lng
            //,label: ""
        }
        this.coordinates.push(coord);
        this.changeCoordinates.emit(coord);
    }
}


