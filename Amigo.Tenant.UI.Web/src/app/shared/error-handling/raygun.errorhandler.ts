// import {SwaggerException} from '../api/services.client';
// import { Injectable, ErrorHandler  } from '@angular/core';
// import {RaygunExceptionHandler} from './raygun-angular2';
// import {Http, Response} from "@angular/http";
// import { environment } from '../../../environments/environment';
// import {NotificationsService, SimpleNotificationsComponent} from 'angular2-notifications';
// import { HttpClient } from '@angular/common/http';

// @Injectable()
// export class RaygunErrorHandler extends ErrorHandler {

// private raygun:RaygunExceptionHandler;

//   constructor(http:Http, private _toastService: NotificationsService){
//     super();
//     RaygunExceptionHandler.setApiKey(environment.raygunApikey);
//     RaygunExceptionHandler.setTag(environment.raygunTag);
//     this.raygun = new RaygunExceptionHandler(http);
//   }
  
//   public handleError(error: Error):void
//   {    
//     this.raygun.handleError(error);

//     if(error instanceof SwaggerException){
//       switch(error.status){
//         case 403:
//         case 401:
//           break;
//         case 0:        
//         case 500:
//           this._toastService.error("Error","An error has ocurred calling the web service.");      
//           break;
//       }
//       return;
//     }
//     this._toastService.error("Error","An error has ocurred");      
//   };
// }
