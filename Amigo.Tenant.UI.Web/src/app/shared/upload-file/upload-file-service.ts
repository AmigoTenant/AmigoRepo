import { BaseService } from './../api/base.service';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, catchError } from "rxjs/operators";
import { FileRepositorySearchRequest } from './file-repository-search.request';
import { Observable } from 'rxjs/Observable';

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

   getFileRepositoriesByIdList(fileRepositorySearchRequest: FileRepositorySearchRequest): Observable<any[]>
   {

      const url = `${this.baseUrl}api/filerepository/getFileRepositoriesByIdList`;
      return this.http.post<any>(url, JSON.stringify(fileRepositorySearchRequest),
          { headers: this.headers.set('Authorization', 'Bearer ' + this.token) }
      ).pipe(
          catchError(this.handleError)
      );
   }

   deleteFile(id: number)
   {
     const url = `${this.baseUrl}api/filerepository/delete/${id}`;
     return this.http.delete(url, { headers: this.headers.set('Authorization', 'Bearer ' + this.token) });
   }


}


