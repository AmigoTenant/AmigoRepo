import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
// import { Http } from '@angular/http';
// import { contentHeaders } from '../common/headers';
import {SecurityService} from '../security/security.service';
import {AppGlobal} from '../../app.global';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.less']
})
export class LoginComponent implements OnInit {

    constructor(public router: Router, private securityService: SecurityService, private appGlobal : AppGlobal) { }

    login(event, username, password) {
        event.preventDefault();
        
        // this.securityService.Login('1111', '2222').subscribe(data => {
        //     this.appGlobal.setValue('hamilton');
        //     this.router.navigateByUrl('/home');
        // });
    }

    toggleImage(){
        // debugger;
        this.securityService.Authorize();
    }

    ngOnInit() {
    }

}
