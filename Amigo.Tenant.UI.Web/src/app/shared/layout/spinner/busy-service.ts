import {Subject} from 'rxjs';
import {Injectable} from '@angular/core';
@Injectable()
export class BusyService
{
    IsBusy : Subject<boolean> = new Subject<boolean>();    

    startLoading(): void {
        this.IsBusy.next(true);
    }

    stopLoading(): void {
        this.IsBusy.next(false);
    }
}