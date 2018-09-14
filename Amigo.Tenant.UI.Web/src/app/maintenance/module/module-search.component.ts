/// <reference path="../../shared/security/auth-check.directive.ts" />
/// <reference path="../../shared/shared.module.ts" />

import {Component, Injectable, OnInit, EventEmitter, Output } from '@angular/core';
import {Http,Jsonp, URLSearchParams} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import { FormGroup, FormBuilder, Validators, FormControl, ReactiveFormsModule } from "@angular/forms";
import { GridDataResult, PageChangeEvent, SelectionEvent } from '@progress/kendo-angular-grid';
import { SearchCriteriaModule } from  '../../model/searchCriteria-Module';
import { ModuleClient } from '../../shared/api/services.client';
import { ModuleGridComponent } from './module-grid.component';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { AuthCheckDirective } from  '../../shared/security/auth-check.directive';
import { Autofocus } from  '../../shared/directives/autofocus.directive';
@Component({
    selector: 'st-module-search',
    templateUrl: './module-search.component.html',
})

export class ModuleSearchComponent implements OnInit {

    constructor(private moduleDataService: ModuleClient) {}

    public flgParents: boolean = false;
    public modulo: number=0;
    public eventNew: number=0;
    public listParents = [];
    countItems: number = 0;

    @Output() counterEvent = new EventEmitter<any>();

    model = new SearchCriteriaModule();

    onSearch() {
        this.modulo++;
    }
    deleteFilters(){
         this.model = new SearchCriteriaModule();
    }
    onNew(){
        
        this.eventNew++;
    }
    ngOnInit():void{
        this.onGetParentModule();
    }

    //GEt Select Option for PARENTS
    public onGetParentModule():void{

        this.moduleDataService.search("","","",true,1,50)
        .subscribe(res => {
            var dataResult: any = res;
                        this.listParents = dataResult.data.items;
                    }
        );

    }

    counterGridEvent(counter) {
        this.countItems = counter;
        this.counterEvent.emit(this.countItems);
    }

}