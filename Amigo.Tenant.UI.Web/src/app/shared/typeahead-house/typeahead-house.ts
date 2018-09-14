import {Component, Input, OnChanges, Output, EventEmitter} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/debounceTime';
import { HouseBasicDTO } from '../api/services.client';
import { HouseClient } from '../api/services.client';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';

@Component({
    selector: 'ngbd-typeahead-house',
    templateUrl: './typeahead-house.html'
})


export class NgbdTypeaheadHouse implements OnChanges{
    model: any;
    
    @Output() modelOutput = new EventEmitter<any>();
    @Input() currentHouse: HouseBasicDTO;
    @Input() searchByHouseId: number;
    @Input() typeId: number;
    @Input() _isDisabled;

    constructor(private houseClient: HouseClient) {}

    ngOnChanges(changes: any)
    {
        this.model = this.currentHouse === undefined ? null : this.currentHouse;
        //debugger;
        if (this.searchByHouseId != undefined && this.searchByHouseId > 0) {

             this.houseClient.getById(this.searchByHouseId)
                 .subscribe(response => {
                     this.model = new HouseBasicDTO();
                     this.model.houseId = response.data.houseId;
                     this.model.fullName = response.data.name;
                 });
        }
        
    }

    getHouse(term) {
        var resp = this.houseClient.searchForTypeAhead(term)
            .map(response => 
                response.data
            );
        return resp;
    }

    search = (text$: Observable<string>) =>
        text$
            .debounceTime(300)
            .distinctUntilChanged()
            .switchMap(term => term.length < 2? []
                :this.getHouse(term)) ;

    formatter = (x: {name: string}) => x.name;

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
    //            if (item.length > 5) // Cause 6 this is the longitude of housename, valid only for copy Paste Code
    //            {
    //                this.houseClient.searchCodeName(item, undefined, 1, 10)
    //                    .subscribe(response => {
    //                        if (response.data.items.length>0) {
    //                            this.model = new BasicHouseDTO();
    //                            this.model.houseId = response.data.items[0].houseId;
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
        this.model = new HouseBasicDTO();
        this.model.houseId = 0;
        this.model.name = "";
        this.modelOutput.emit(this.model);
    }
}
