import { Injectable } from '@angular/core';
declare var $: any;
@Injectable()
export class ValidationService {

    constructor() { }

    public DisplayFrontEndValidators(formName :string){
       
        console.log('form being validated ' + formName);
        var controls = $('[data-val-form="' + formName + '"] input[type="text"].ng-untouched, [data-val-form="' + formName + '"] input[type="password"].ng-untouched , [data-val-form="' + formName + '"] select.ng-untouched').each(function (index) {
            $(this).addClass('ng-touched').removeClass('ng-untouched');
            $(this).focus();
        });
    }

}