import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { Observable } from 'rxjs/Observable';
export const API_BASE_URL = new InjectionToken('API_BASE_URL');

@Injectable()
export class BaseService {

    public http: HttpClient = null;
    public baseUrl: string;
    public jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor( @Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : '';
    }

    handleError(error: HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
            // A client-side or network error occurred. Handle it accordingly.
            console.error('An error occurred:', error.error.message);
        } else {
            // The backend returned an unsuccessful response code.
            // The response body may contain clues as to what went wrong,
            console.error(
                `Backend returned code ${error.status}, ` +
                `body was: ${error.error}`);
        }
        // return an observable with a user-facing error message
        //return throwError('Something bad happened; please try again later.');
        return Observable.throw('Something bad happened; please try again later.');
    }

    get headers() {
        return new HttpHeaders({ 'Content-Type': 'application/json', 'Accept-Language': 'es-cl' });
    }
}
