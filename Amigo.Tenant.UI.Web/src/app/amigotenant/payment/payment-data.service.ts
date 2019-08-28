import { Injectable } from "@angular/core";
import { BaseService } from "../../shared/api/base.service";
import { Observable } from "rxjs";
import { PaymentPeriodSendNotificationRequest } from "./dto/payment-period-sendnotification-request";
import { catchError } from "rxjs/operators";

@Injectable()
export class PaymentDataService extends BaseService
{
    public SendPaymentNotificationEMail(ppSendNotificationIdList: PaymentPeriodSendNotificationRequest[]): Observable<any>{
        const url = `${this.baseUrl}api/payment/sendPaymentNotificationEmail`;
        return this.http.post<any>(url, JSON.stringify(ppSendNotificationIdList),
                { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
            ).pipe(
                catchError(this.handleError)
            );
    }

}
