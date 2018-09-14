import {Component, Input, OnChanges, Output, EventEmitter} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/debounceTime';
import { PeriodDTO } from '../api/services.client';
import { PeriodClient } from '../api/services.client';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';

@Component({
    selector: 'ngbd-typeahead-period',
    templateUrl: './typeahead-period.html'
})


export class NgbdTypeaheadPeriod implements OnChanges{
    model: any;
    
    @Output() modelOutput = new EventEmitter<any>();
    @Input() currentPeriod: PeriodDTO;
    @Input() searchByPeriodId: number;

    constructor(private periodClient: PeriodClient) {}

    ngOnChanges(changes: any)
    {
        this.model = this.currentPeriod === undefined ? null : this.currentPeriod;
        //debugger;
        if (this.searchByPeriodId != undefined && this.searchByPeriodId > 0) {

            this.periodClient.getPeriodById(this.searchByPeriodId)
                 .subscribe(response => {
                     this.model = new PeriodDTO();
                     this.model.periodId = response.data.periodId;
                     this.model.code = response.data.code;
                 });
        }
        
    }

    getPeriod(term) {
        //debugger;
        var resp = this.periodClient.searchForTypeAhead(term)
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
                :this.getPeriod(term)) ;

    formatter = (x: {code: string}) => x.code;

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


    createModelEmpty()
    {
        this.model = new PeriodDTO();
        this.model.periodId = 0;
        this.model.code = "";
        this.modelOutput.emit(this.model);
    }
}
