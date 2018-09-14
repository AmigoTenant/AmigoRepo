// import { Inject, Injectable, forwardRef, ErrorHandler, isDevMode } from '@angular/core';
// import {Http, Headers} from '@angular/http';
// import {ErrorReport} from './ErrorReport';
// import { HttpClient } from '@angular/common/http';


// @Injectable()
// export class RaygunExceptionHandler extends ErrorHandler {
//     static apiKey: string;
//     static developmentHostnames: Array<string> = [];
//     static version: string;
//     static username = '';
//     static tag:string;

//     errorReport: ErrorReport;

//     constructor(private http: Http) {
//         super();
//     }

//     static setUser(name: string) {
//         this.username = name;
//     }

//     static setApiKey(apiKey:string){
//       this.apiKey = apiKey;
//     }

//     static setDevelopmentHostnames(urls: Array<string>) {
//         this.developmentHostnames = urls;
//     }

//     static setTag(tag:string){
//       this.tag = tag;
//     }
    
//     handleError(error: any) {
//         var originalException = error.originalException || error;
//         this.errorReport = new ErrorReport(error);
//         if(RaygunExceptionHandler.tag != null) this.errorReport.details.tags = [RaygunExceptionHandler.tag];

      
//         if(isDevMode())
//         {
//             console.log(originalException);
//         }
//     }
// }
