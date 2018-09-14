import {Pipe, PipeTransform} from '@angular/core';
declare var moment: any;
import { environment } from '../../../environments/environment';


export class AmigoTenantOffsetBase {


    public static format(date: string): Date {
        if (date != undefined && date != null && date != "") {
            var momentDate = moment(date);
            var newDate = new Date(date);
            var userTimezoneOffset = newDate.getTimezoneOffset() * 60000;
            var momentUserTimezoneOffset = 0;
            if (momentDate._tzm != undefined)
                momentUserTimezoneOffset = momentDate._tzm * 60000;
            newDate = new Date(newDate.getTime() + userTimezoneOffset + momentUserTimezoneOffset);
            //var parsed = moment(newDate);
            return newDate;    
        }
        return null;
    }

    public static parse(date: Date): string {
        var userTimezoneOffset = date.getTimezoneOffset() * 60000;
        date = new Date(date.getTime() - userTimezoneOffset);
        return date.toISOString();
    }
}