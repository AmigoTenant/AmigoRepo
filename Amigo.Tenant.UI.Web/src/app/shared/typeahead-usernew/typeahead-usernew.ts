import {Component, Input, OnChanges, Output, EventEmitter} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/debounceTime';
import { AmigoTenantUserBasicDTO } from '../api/services.client';
import { UsersClient } from '../api/services.client';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';

@Component({
    selector: 'ngbd-typeahead-usernew',
    templateUrl: './typeahead-usernew.html'
})


export class NgbdTypeaheadUserNew implements OnChanges{
    model: any;
    
    @Output() modelOutput = new EventEmitter<any>();
    @Input() currentUser :AmigoTenantUserBasicDTO;
    @Input() searchByUserId: number;
    constructor(private userClient: UsersClient) {}

    ngOnChanges(changes: any)
    {
        this.model = this.currentUser === undefined ? null : this.currentUser;
        
        if (this.searchByUserId != undefined && this.searchByUserId > 0) {

            this.userClient.searchById(this.searchByUserId, '', '', '', null, '', null, '', 1, 10)
                .subscribe(response => {
                    this.model = new AmigoTenantUserBasicDTO();
                    this.model.amigoTenantTUserId = Number(response.amigoTenantTUserId);
                    this.model.username = response.username;
                });
        }
        
    }

    getAmigoTenantTUsers(term) {
        var resp =  this.userClient.searchForAutocomplete("", "", "", term)
            .map(response => response.data);
        return resp;
    }

    search = (text$: Observable<string>) =>
        text$
            .debounceTime(300)
            .distinctUntilChanged()
            .switchMap(term => term.length < 2? []
                :this.getAmigoTenantTUsers(term)) ;

    formatter = (x: {username: string}) => x.username;

    selectValue(item) {
        if (item === "" || item === undefined || typeof item != "object")
        {
            //console.log("selectValue emit " + item);
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

    findValue(item)
    {
        if (item != "" && item != undefined) {
            if (typeof item != "object") {
                if (item.length > 5) // Cause 6 this is the longitude of username, valid only for copy Paste Code
                {
                    this.userClient.searchById(0, item, '', '', null, '', null, '', 1, 10)
                        .subscribe(response => {
                            if (response.amigoTenantTUserId !== undefined) {
                                this.model = new AmigoTenantUserBasicDTO();
                                this.model.amigoTenantTUserId = Number(response.amigoTenantTUserId);
                                this.model.username = response.username;
                                //console.log("entro 0");
                            }
                            else {
                                //console.log("entro 1");
                                //this.model = null;
                                this.createModelEmpty();
                            }
                            //console.log("entro 2");
                            this.modelOutput.emit(this.model);
                        });
                }
                else {
                    this.createModelEmpty();
                }
            }
            else {
                //console.log("entro 3");
                this.createModelEmpty();
            }
        }
        else
        {
            //console.log("entro 4");
            this.createModelEmpty();
            //this.model = new AmigoTenantTUserBasicDTO();
            //this.model.amigoTenantTUserId = 0;
            //this.model.username = "";
            //this.modelOutput.emit(this.model);
        }
        //console.log("entro 5");
    }

    createModelEmpty()
    {
        this.model = new AmigoTenantUserBasicDTO();
        this.model.amigoTenantTUserId = 0;
        this.model.username = "";
        this.modelOutput.emit(this.model);
    }
}
