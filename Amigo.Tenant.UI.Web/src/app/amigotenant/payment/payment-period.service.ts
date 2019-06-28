import { tap } from 'rxjs/operators/tap';
import { catchError, map } from 'rxjs/operators';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BaseService } from '../../shared/api/base.service';
import { PaymentPeriodRegisterRequest } from "./dto/payment-period-register-request";


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


}
