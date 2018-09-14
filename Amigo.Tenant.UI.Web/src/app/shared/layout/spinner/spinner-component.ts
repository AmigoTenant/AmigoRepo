import {Input, Component, OnInit} from '@angular/core';

@Component({
    selector: 'st-spinner',
    template: '<div class="loader" [hidden]="!isLoading"><div class="spinner"><div class="box"></div><div class="box"></div><div class="box"></div><div class="box"></div></div></div>',
    providers: [],
    styles: []
})
export class SpinnerComponent implements OnInit {

    @Input()isLoading:boolean = false;    

    ngOnInit() {

    }
}