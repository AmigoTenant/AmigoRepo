import { catchError } from 'rxjs/operators';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BaseService } from '../../shared/api/base.service';
import { Constants } from '../../shared/constants/constants';

@Injectable()
export class PaymentServiceNew extends BaseService {

    exportToExcel(
        periodId: number|null,
        houseId: number|null,
        contractCode: string,
        paymentPeriodStatusId: number|null,
        tenantId: number|null,
        hasPendingFines: boolean|null,
        hasPendingLateFee: boolean|null,
        hasPendingDeposit: boolean|null,
        page: number|null,
        pageSize: number|null
    ) {
        if (periodId === undefined) {
            periodId = null;
        }
        if (houseId === undefined) {
        houseId = null;
        }
        if (contractCode === undefined) {
        contractCode = 'xxxx';
        }
        if (paymentPeriodStatusId === undefined) {
        paymentPeriodStatusId = null;
        }
        if (tenantId === undefined) {
        tenantId = null;
        }
        if (hasPendingFines === undefined) {
        hasPendingFines = null;
        }
        if (hasPendingLateFee === undefined) {
        hasPendingLateFee = null;
        }
        if (hasPendingDeposit === undefined) {
        hasPendingDeposit = null;
        }

        let url = `${this.baseUrl}${Constants.PAYMENTPERIOD_URL_PATH.exportToExcel}/${periodId}/${houseId}/${contractCode}/${paymentPeriodStatusId}/${tenantId}/${hasPendingFines}/${hasPendingLateFee}/1/20000`;

        window.open(url);
    }
}