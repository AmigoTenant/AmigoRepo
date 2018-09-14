import {Component, Input, OnChanges, Output, EventEmitter} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/debounceTime';
import { BasicTenantDTO } from '../api/services.client';
import { TenantClient } from '../api/services.client';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';

@Component({
    selector: 'ngbd-typeahead-tenant',
    templateUrl: './typeahead-tenant.html'
})


export class NgbdTypeaheadTenant implements OnChanges{
    model: any;
    
    @Output() modelOutput = new EventEmitter<any>();
    @Input() currentTenant: BasicTenantDTO;
    @Input() searchByTenantId: number;
    @Input() typeId: number;
    @Input() validateInActiveContract: boolean=false;
    constructor(private tenantClient: TenantClient) {}

    ngOnChanges(changes: any)
    {
        this.model = this.currentTenant === undefined ? null : this.currentTenant;
        //debugger;
        if (this.searchByTenantId != undefined && this.searchByTenantId > 0) {

             this.tenantClient.getById(this.searchByTenantId, this.typeId)
                 .subscribe(response => {
                     this.model = new BasicTenantDTO();
                     this.model.tenantId = response.data.tenantId;
                     this.model.fullName = response.data.fullName;
                 });
        }
        
    }

    getTenant(term) {
        //debugger;
        var resp = this.tenantClient.searchForTypeAhead(term, this.validateInActiveContract)
            .map(response => response.data);
        return resp;

        //.map(response => {
        //    var dataResult: any = response;
        //    dataResult.value.data;
        //});
    }

    search = (text$: Observable<string>) =>
        text$
            .debounceTime(300)
            .distinctUntilChanged()
            .switchMap(term => term.length < 2? []
                :this.getTenant(term)) ;

    formatter = (x: {fullName: string}) => x.fullName;

    selectValue(item) {
        if (item === "" || item === undefined || typeof item != "object")
        {
            console.log("selectValue emit " + item);
            this.modelOutput.emit("");
        }
        else
        {
            if (typeof item.item == "object")
            {
                this.modelOutput.emit(item.item);
            }
            else if (typeof item == "object" )
            this.modelOutput.emit(item);
        }
    }

    ngOnDestroy() {
        this.modelOutput.unsubscribe();
    }

    //findValue(item)
    //{
    //    if (item != "" && item != undefined) {
    //        if (typeof item != "object") {
    //            if (item.length > 5) // Cause 6 this is the longitude of tenantname, valid only for copy Paste Code
    //            {
    //                this.tenantClient.searchCodeName(item, undefined, 1, 10)
    //                    .subscribe(response => {
    //                        if (response.data.items.length>0) {
    //                            this.model = new BasicTenantDTO();
    //                            this.model.tenantId = response.data.items[0].tenantId;
    //                            this.model.fullName = response.data.items[0].fullName;
    //                             //console.log("entro 0");
    //                         }
    //                         else {
    //                             //console.log("entro 1");
    //                             //this.model = null;
    //                             this.createModelEmpty();
    //                         }
    //                         //console.log("entro 2");
    //                         this.modelOutput.emit(this.model);
    //                     });
    //            }
    //            else {
    //                this.createModelEmpty();
    //            }
    //        }
    //        else {
    //            //console.log("entro 3");
    //            this.createModelEmpty();
    //        }
    //    }
    //    else
    //    {
    //        this.createModelEmpty();
    //    }
    //    //console.log("entro 5");
    //}

    createModelEmpty()
    {
        this.model = new BasicTenantDTO();
        this.model.tenantId = 0;
        this.model.fullName = "";
        this.modelOutput.emit(this.model);
    }
}
