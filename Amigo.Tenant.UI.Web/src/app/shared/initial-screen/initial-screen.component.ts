import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SecurityService } from '../security/security.service';
import { environment } from '../../../environments/environment';

declare var $: any;

@Component({
    selector: 'app-initial-screen',
    templateUrl: './initial-screen.component.html'
})
export class InitialScreenComponent implements OnInit {

    private _token = String;

    public version:string = environment.version;

    public year:number = new Date().getFullYear();

    constructor(private _securityService: SecurityService, private _router: Router) {
        this._token = this._securityService.GetToken();
    }

    ngOnInit() {
        this.resizeContainer();

        $(window).resize(() => {
            this.resizeContainer();
        });

        if(this._securityService.IsAuthorized){
            this.redirectToPage();
        }
    }

    private resizeContainer() {
        var viewPort = $(window).height();
        $(".initial-screen").height(viewPort);
    }

    loginCustomerPortal() {
        this.redirectToPage();
    }

    loginInternalDashboard(){
        this._securityService.Authorize("type:windows");
    }

    private redirectToPage() {
        if (this._token != undefined && this._token != null) {
            setTimeout(() => {
                this._router.navigateByUrl("/dashboard");
            }, 100);
        } else {
            this._securityService.Authorize();
        }
    }
}
