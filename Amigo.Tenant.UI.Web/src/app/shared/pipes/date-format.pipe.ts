import {Pipe, PipeTransform} from '@angular/core';
declare var moment: any;
import { environment } from '../../../environments/environment';

@Pipe({ name: 'dateFormat' })
export class DateFormatPipe implements PipeTransform {
    transform(date: any, args?: any): any {
        //debugger;
        
        var userTimezoneOffset = date.getTimezoneOffset() * 60000;
        date = new Date(date.getTime() + userTimezoneOffset);
        var parsed = moment(date);
        //var parsed = date;
        return parsed.format(environment.localization.dateFormat);
        

    }

}