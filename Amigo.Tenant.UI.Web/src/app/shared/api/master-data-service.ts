import { catchError } from 'rxjs/operators';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BaseService } from './base.service';
import { Constants } from '../constants/constants';
import { PeriodDTO } from './services.client';
import { HouseTypeDTO } from '../dto/house-type-dto';


@Injectable()
export class MasterDataService extends BaseService {

    getConceptsByTypeIdList(typeIdList: number[]): Observable<any[]> {

        const url = `${this.baseUrl}${Constants.MASTER_DATA_URL_PATH.getLocations}/${typeIdList}`;
        return this.http.get<any>(url, { headers: this.headers })
            .pipe(
            catchError(this.handleError)
            );
    }


    getTypes(): Observable<any[]> {
        const url = `${this.baseUrl}${Constants.MASTER_DATA_URL_PATH.getTypes}`;
        return this.http.get<any>(url, { headers: this.headers }) //.map(r => <any[]>r)
        .pipe(
            catchError(this.handleError)
          );
    }

    getCurrentPeriod(): Observable<PeriodDTO[] | any> {
        // let token = localStorage.getItem('authorizationData');
        // token = token.substr(1);
        // token = token.substr(0, (token.length - 1));

        const url = `${this.baseUrl}api/${Constants.PERIOD_URL_PATH.getCurrentPeriod}`;
        return this.http.get<PeriodDTO | any>(url, { headers: this.headers.set("Authorization", "Bearer " + this.token) })
            .pipe(
            catchError(this.handleError)
            );
    }

    //HOUSE
    getHouseTypes(): Observable<any> {
        const url = `${this.baseUrl}api/${Constants.MASTER_DATA_URL_PATH.getHouseTypes}`;
        return this.http.get<any>(url, { headers: this.headers.set("Authorization", "Bearer " + this.token)})
            .pipe(
            catchError(this.handleError)
            );
    }
}
