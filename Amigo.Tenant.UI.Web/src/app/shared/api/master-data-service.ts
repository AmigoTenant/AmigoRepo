import {tap} from 'rxjs/operators/tap';
import { catchError, map } from 'rxjs/operators';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BaseService } from './base.service';
import { Constants } from '../constants/constants';
import { PeriodDTO, ResponseDTOOfListOfHouseTypeDTO } from './services.client';
import { HouseTypeDTO } from '../dto/house-type-dto';


@Injectable()
export class MasterDataService extends BaseService {

    getConceptsByTypeIdList(typeIdList: number[]): Observable<any[]> {

        const url = `${this.baseUrl}api/${Constants.MASTER_DATA_URL_PATH.getConceptByTypeIdList}`;
        return this.http.post<any>(url, JSON.stringify(typeIdList),  { headers: this.headers.set("Authorization", "Bearer " + this.token) })
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
        const url = `${this.baseUrl}api/${Constants.PERIOD_URL_PATH.getCurrentPeriod}`;
        return this.http.get<PeriodDTO | any>(url, { headers: this.headers.set("Authorization", "Bearer " + this.token) })
            .pipe(
            catchError(this.handleError)
            );
    }

    getPeriodLastestNumberPeriods(periodNumber: number): Observable<PeriodDTO[] | any> {
        const url = `${this.baseUrl}api/${Constants.PERIOD_URL_PATH.getPeriodLastestNumberPeriods}?periodNumbers=${periodNumber}`;
        return this.http.get<PeriodDTO | any>(url, { headers: this.headers.set("Authorization", "Bearer " + this.token) })
            .pipe(
            catchError(this.handleError)
            );
    }

    getYearsFromPeriods(): Observable<any> {
        const url = `${this.baseUrl}api/${Constants.PERIOD_URL_PATH.getYearsFromPeriods}`;
        return this.http.get<PeriodDTO | any>(url, { headers: this.headers.set("Authorization", "Bearer " + this.token) })
            .pipe(
            catchError(this.handleError)
            );
    }

    getPeriodsByYear(year: number): Observable<PeriodDTO[] | any> {
        const url = `${this.baseUrl}api/${Constants.PERIOD_URL_PATH.getPeriodsByYear}?year=${year}`;
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
                map(r => r),
                catchError(this.handleError)
            );
            // return this.http.get<any>(url, { headers: this.headers.set("Authorization", "Bearer " + this.token)})
            // .map(r => r as ResponseDTOOfListOfHouseTypeDTO)
            // .catch(this.handleError);
    }

    getHouseAll(houseName: string): Observable<any> {
        const url = `${this.baseUrl}api/${Constants.MASTER_DATA_URL_PATH.getHouseAll}?search=${houseName}`;
        return this.http.get<any>(url, { headers: this.headers.set("Authorization", "Bearer " + this.token)})
            .pipe(
                map(r => r),
                catchError(this.handleError)
            );
    }

    sendEmailNotification(): Observable<any> {
        const url = `${this.baseUrl}api/${Constants.MASTER_DATA_URL_PATH.getHouseTypes}`;
        return this.http.get<any>(url, { headers: this.headers.set("Authorization", "Bearer " + this.token)})
            .pipe(
            catchError(this.handleError)
            );
    }

    getGeneralTableByTableName(tableName: string): Observable<any | null> {
        const url = `${this.baseUrl}api/${Constants.MASTER_DATA_URL_PATH.getGeneralTableByTableNameAll}?tableName=${tableName}`;
        return this.http.get<any>(url, { headers: this.headers.set("Authorization", "Bearer " + this.token)})
            .pipe(
                map(r => r),
                catchError(this.handleError)
            );
    }

    //BUSINESS PARTNER
    getBusinessPartnerByBPType(bpTypeCode: string): Observable<any | null> {
        const url = `${this.baseUrl}api/${Constants.MASTER_DATA_URL_PATH.getBusinessPartnerByBPTypeCode}?bpTypeCode=${bpTypeCode}`;
        return this.http.get<any>(url, { headers: this.headers.set("Authorization", "Bearer " + this.token)})
            .pipe(
                map(r => r),
                catchError(this.handleError)
            );
    }


}
