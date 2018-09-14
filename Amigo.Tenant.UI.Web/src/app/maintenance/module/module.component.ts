import { Component, OnInit } from '@angular/core';
import { AuthCheckDirective } from  '../../shared/security/auth-check.directive';

@Component({
    selector: 'st-module',
    templateUrl: './module.component.html'
    //providers: []
})
export class ModuleComponent implements OnInit {

    constructor() { }

    countItems : number=0;

    ngOnInit() {
    }

    counterSearchEvent(counter) {
        this.countItems = counter;
    }

}
