import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
// import {ListService} from '../../service/list.service';
// import {AppGlobal} from '../../../app.global';

declare var $: any;

@Component({
    selector: 'st-header',
    templateUrl: 'header.component.html',
})
export class HeaderComponent implements OnInit {

    todos;
    constructor(/*listService: ListService, private appGlobal : AppGlobal*/) {
        // this.todos = listService.todos;
        // console.log('header========================>>>>>>>>>>>>>', this.todos);
        //this.routes = this.findNavRoutes(allRoutes);
        //console.log('routessss', this.routes);
    }

    ngOnInit() {
    }
}