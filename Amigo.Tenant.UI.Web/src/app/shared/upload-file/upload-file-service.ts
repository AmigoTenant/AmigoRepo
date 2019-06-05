import { BaseService } from './../api/base.service';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, catchError } from "rxjs/operators";

//export const API_BASE_URL = new InjectionToken('API_BASE_URL');
// @Injectable()
// export class BaseService {

//     public http: HttpClient = null;
//     public baseUrl: string;
//     public jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

//     constructor( @Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
//         this.http = http;
//         this.baseUrl = baseUrl ? baseUrl : '';
//     }

//     get headers() {
//         return new HttpHeaders({ accept: 'application/json', 'Content-Type': 'application/json', 'Accept-Language': 'es-cl' });
//     }
// }


@Injectable()
export class UploadFileService extends BaseService {

   postFile(entityCode:string, parentId: string, additional: string, fileToUpload: File)
   {
       const url = `${this.baseUrl}api/filerepository/upload`;
    //TODO: QUITAR FORMDATA
     const formData: FormData = new FormData();
     formData.append('File', fileToUpload, fileToUpload.name);
     formData.append('EntityCode', entityCode);
     formData.append('ParentId', parentId);
     formData.append('Additional', additional);
     return this.http.post(url, formData);

   }

   downloadFile(id: number) {
    const url = `${this.baseUrl}api/filerepository/download/${id}`;
    //const url = `http://localhost:57584/api/downloadImage/${id}`;
    return this.http
      .get(url, {observe: 'response', responseType: 'blob'});
   }

   getFileRepositories(entityCode: string, parentId: string)
   {
     const url = `${this.baseUrl}api/filerepository/getFileRepositories/${entityCode}/${parentId}`;
     return this.http
        .get(url,  { headers: this.headers.set('Authorization', 'Bearer ' + this.token) })
        .pipe(
              map(r => r),
              catchError(this.handleError)
            );
   }

   deleteFile(id: number)
   {
     const url = `${this.baseUrl}api/filerepository/delete/${id}`;
     return this.http.delete(url, { headers: this.headers.set('Authorization', 'Bearer ' + this.token) });
   }

//      TODO: delete this code
//    search(expenseSearchRequest: ExpenseSearchRequest): Observable<any[]> {
//         const url = `${this.baseUrl}api/expense/searchCriteria`;
//         return this.http.post<any>(url, JSON.stringify(expenseSearchRequest),
//                 { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
//             ).pipe(
//                 catchError(this.handleError)
//             );
//      }

//     getById(id: number): Observable<any | ExpenseEditRequest> {
//         const url = `${this.baseUrl}api/expense/getById?id=${id}`;
//         return this.http.get<any>(url,
//                 { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
//             ).pipe(
//                 map(r => r),
//                 catchError(this.handleError)
//             );
//     }

//     saveExpense(expenseRegisterRequest: ExpenseRegisterRequest): Observable<any> {
//         const url = `${this.baseUrl}api/expense/register`;
//         return this.http.post<any>(url, JSON.stringify(expenseRegisterRequest),
//                 { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
//             ).pipe(
//                 catchError(this.handleError)
//             );
//     }


}


