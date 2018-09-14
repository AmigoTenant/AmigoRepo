
import {Component, OnInit, Input,Output,EventEmitter  } from '@angular/core';
import {BrowserModule } from '@angular/platform-browser';
// import {AgmCoreModule,
//     SebmGoogleMap, SebmGoogleMapPolygon, LatLngLiteral } from 'angular2-google-maps/core';
import {AgmCoreModule, LatLngLiteral } from '@agm/core';

declare var $: any;



//import { SebmGoogleMap, SebmGooglePolygon, LatLngLiteral } from 'angular2-maps/core';
// import {Component, OnInit, Input,Output,EventEmitter } from '@angular/core';
// import {BrowserModule } from '@angular/platform-browser';
// import {AgmCoreModule,SebmGoogleMap, SebmGoogleMapPolygon, LatLngLiteral
    /*,GoogleMapsAPIWrapper */ //} from 'angular2-google-maps/core';

//import { GoogleMap, Marker } from 'angular2-google-maps/core/services/google-maps-types';

//declare var $: any;
//import { SebmGoogleMap, SebmGooglePolygon, LatLngLiteral } from 'angular2-maps/core';

@Component({
    selector: 'marker-map-component',
    styles: [`
    .sebm-google-map-container {
       height: 350px;
     }
  `],
    template: `
    <div>TODO</div>
`})
export class MarkerMapComponent implements OnInit {

    @Input() iListCoordinate :Array<LatLngLiteral>;
    @Input() iLatitude: number;
    @Input() ilongitude: number;
    @Output() changeCoordinates =  new EventEmitter<any>();

    fill:string = "#FE2E2E";
    // google maps zoom level
    zoom: number = 6;
    
  //  private map: GoogleMap;

    constructor( ) { 
    }

    ngOnInit() {}

    clickedMarker(label: string, index: number) {
        console.log(`clicked the marker: ${label || index}`)
    }

    mapClicked = ($event:any)=> {
        this.iListCoordinate = [...this.iListCoordinate, ({ lat: $event.coords.lat,  lng: $event.coords.lng })];
        this.changeCoordinates.emit(this.iListCoordinate); 
    }

    // markerDragEnd(m: marker, $event: MouseEvent) {
    //     console.log('dragEnd', m, $event);
    // }

 
}


//  <sebm-google-map
    //   [latitude]="iLatitude"
    //   [longitude]="ilongitude"
    //   [zoom]="zoom"
    //   [disableDefaultUI]="false"
    //   [zoomControl]="true"
    //   (mapClick)="mapClicked($event)">
    //   <sebm-google-map-marker 
    //       *ngFor="let m of iListCoordinate; let i = index"
    //       (markerClick)="clickedMarker(m)"
    //       [latitude]="m.lat"
    //       [longitude]="m.lng"
    //       (centerChange)= "true"
    //       [markerDraggable]="false">

    //        <sebm-google-map-info-window>
    //           <p>Latitude : {{m.lat}} - Longitude: {{m.lng}}</p>
    //           </sebm-google-map-info-window>
    //   </sebm-google-map-marker>
    //   <sebm-map-polygon [paths]="iListCoordinate" [fillColor]="fill" >
    //   </sebm-map-polygon>  
    //   <sebm-google-map-marker 
    //       [latitude]="iLatitude"
    //       [longitude]="ilongitude"
    //       [iconUrl]="'./assets/images/icon_real_estate.png'">          
    //   </sebm-google-map-marker>
    // </sebm-google-map>