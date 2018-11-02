import { ExpenseSearchRequest } from './dto/expense-search-request';
import {tap} from 'rxjs/operators/tap';
import { catchError, map } from 'rxjs/operators';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BaseService } from '../../shared/api/base.service';
import { ExpenseEditRequest } from './dto/expense-edit-request';
import { ExpenseDetailDto } from './dto/expense-detail-dto';
import { ExpenseRegisterRequest } from './dto/expense-register-request';

@Injectable()
export class ExpenseDataService extends BaseService {

    search(expenseSearchRequest: ExpenseSearchRequest): Observable<any[]> {
        const url = `${this.baseUrl}api/expense/searchCriteria`;
        return this.http.post<any>(url, JSON.stringify(expenseSearchRequest),
                { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
            ).pipe(
                catchError(this.handleError)
            );
     }

    getById(id: number): Observable<any | ExpenseEditRequest> {
        const url = `${this.baseUrl}api/expense/getById?id=${id}`;
        return this.http.get<any>(url,
                { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
            ).pipe(
                map(r => r),
                catchError(this.handleError)
            );
    }

    //EXPENSE DETAIL

    getExpenseDetailByExpenseId(id: number): Observable<any[] | ExpenseDetailDto[]> {
        const url = `${this.baseUrl}api/expense/getExpenseDetailByExpenseId?id=${id}`;
        return this.http.get<any>(url,
                { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
            ).pipe(
                map(r => r),
                catchError(this.handleError)
            );
    }

    getExpenseDetailByExpenseDetailId(id: number): Observable<any | ExpenseRegisterRequest> {
        const url = `${this.baseUrl}api/expense/getExpenseDetailByExpenseDetailId?id=${id}`;
        return this.http.get<any>(url,
            { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
        ).pipe(
            map(r => r),
            catchError(this.handleError)
            );
    }


}
