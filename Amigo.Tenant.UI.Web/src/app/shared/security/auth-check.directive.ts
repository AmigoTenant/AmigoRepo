//import {IUsersClient} from '../api/services.client';
import {OnInit,Directive, ElementRef, Input} from '@angular/core';
import { MainMenuClient, MainMenuDTO } from '../api/services.client';

@Directive({
    selector: '[st-authCheck]' // <button st-authCheck [actionCode]="15215" ></button>
})

export class AuthCheckDirective implements OnInit {
   
    constructor(private el: ElementRef, private mainMenuClient: MainMenuClient) {    
        
     }

     ngOnInit():void {
         
         var permissions = JSON.parse(localStorage.getItem("permissions"));
         
         if (permissions == null) {
             this.mainMenuClient.search(0)
                 .subscribe(response => {
                     var action = response.data.filter(p => p.actionCode == this.actionCode || this.actionCode == undefined);
                     if (action.length <= 0)
                         this.DisableControl();
                 });
         } else {
             var action = permissions.filter(p => p.ActionCode == this.actionCode || this.actionCode == undefined);
             if (action.length <= 0)
                 this.DisableControl();
         }
     }

     @Input() actionCode: string;

    private DisableControl() {
        this.el.nativeElement.style.visibility = "hidden";
    }
}