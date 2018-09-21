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

export class NgbdTypeaheadPeriod {
    model: any;
    @Output() modelOutput = new EventEmitter<any>();
    @Input() currentPeriod: PeriodDTO;
    @Input() searchByPeriodId: number;
    @Input() isDisabled: boolean;

    constructor(private periodClient: PeriodClient) {}

    getPeriod(term) {
        let resp = this.periodClient.searchForTypeAhead(term)
            .map(response =>
                response.data
            );
        return resp;
    }

    search = (text$: Observable<string>) =>
        text$
            .debounceTime(300)
            .distinctUntilChanged()
            .switchMap(term => term.length < 2 ? [] : this.getPeriod(term)) ;

    formatter = (x: {code: string}) => x.code;

    lookup(item) {
        if (item === '' || item === undefined || typeof item !== 'object') {
            this.modelOutput.emit('');
        }
    }

    selectValue(item) {
        this.modelOutput.emit(item);
    }

    createModelEmpty() {
        this.model = new PeriodDTO();
        this.model.periodId = 0;
        this.model.code = '';
    }
}
