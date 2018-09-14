import {LoaderService} from '../../api/loader.service';
import {Subscription} from 'rxjs';
import {Component, OnInit, OnDestroy} from '@angular/core';

@Component({
    selector: 'app-main-layout',
    templateUrl: './main-layout.component.html',
    providers: [],
    styles: []
})
export class MainLayoutComponent implements OnInit, OnDestroy {

    constructor() { }

    private subscription:Subscription;

    ngOnInit() {
        this.subscription = LoaderService.IsBusy.subscribe((newvalue)=>{
            this.isBusy = newvalue;
        });
    }

    ngOnDestroy(){
        this.subscription.unsubscribe();
    }

    public isBusy:boolean = false;
}