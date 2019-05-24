import {tap} from 'rxjs/operators/tap';
import { catchError, map } from 'rxjs/operators';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BaseService } from '../shared/api/base.service';
import { DashboardBalanceRequest } from './dto/dashboard-balance-request';


@Injectable()
export class DashboardDataService extends BaseService {

    getDashboardBalance(dashboardBalanceRequest: DashboardBalanceRequest): Observable<any[]> {
        const url = `${this.baseUrl}api/dashboard/getDashboardBalance`;
        return this.http.post<any>(url, JSON.stringify(dashboardBalanceRequest),
                { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
            ).pipe(
                catchError(this.handleError)
            );
     }

    

}
