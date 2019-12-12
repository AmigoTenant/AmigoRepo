import { tap } from 'rxjs/operators/tap';
import { catchError, map } from 'rxjs/operators';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BaseService } from '../../shared/api/base.service';
import { PaymentPeriodRegisterRequest } from "./dto/payment-period-register-request";
import { PaymentPeriodUpdateRequest } from './dto/payment-period-update-request';


@Injectable()
export class PaymentPeriodService extends BaseService {

    registerPaymentDetail(paymentPeriodRegisterRequest: PaymentPeriodRegisterRequest): Observable<any> {
        const url = `${this.baseUrl}api/payment/registerPaymentDetail`;
        return this.http.post<any>(url, JSON.stringify(paymentPeriodRegisterRequest),
                { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
            ).pipe(
                catchError(this.handleError)
            );
    }
    updatePaymentDetail(paymentPeriodUpdateRequest: PaymentPeriodUpdateRequest): Observable<any> {
        const url = `${this.baseUrl}api/payment/updatePaymentDetail`;
        return this.http.post<any>(url, JSON.stringify(paymentPeriodUpdateRequest),
                { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
            ).pipe(
                catchError(this.handleError)
            );
    }
    deletePaymentDetail(paymentPeriodUpdateRequest: PaymentPeriodUpdateRequest): Observable<any> {
        const url = `${this.baseUrl}api/payment/deletePaymentDetail`;
        return this.http.post<any>(url, JSON.stringify(paymentPeriodUpdateRequest),
                { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
            ).pipe(
                catchError(this.handleError)
            );
    }
    searchForLiquitadion(paymentPeriodRegisterRequest: PaymentPeriodRegisterRequest): Observable<any> {
        const url = `${this.baseUrl}api/payment/searchForLiquidation`;
        return this.http.post<any>(url, JSON.stringify(paymentPeriodRegisterRequest),
                { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
            ).pipe(
                catchError(this.handleError)
            );
    }
}
