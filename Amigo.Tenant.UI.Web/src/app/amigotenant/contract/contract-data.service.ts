import { Injectable } from "@angular/core";
import { BaseService } from "../../shared/api/base.service";
import { Observable } from "rxjs";
import { catchError } from "rxjs/operators";
import { ContractChangeTermRequest } from "./dto/contract-change-term-request";

@Injectable()
export class ContractDataService extends BaseService
{
    public ContractChangeTerm(contractChangeRequest: ContractChangeTermRequest): Observable<any>{
        const url = `${this.baseUrl}api/contract/changeTerm`;
        return this.http.post<any>(url, JSON.stringify(contractChangeRequest),
                { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
            ).pipe(
                catchError(this.handleError)
            );
    }

}
