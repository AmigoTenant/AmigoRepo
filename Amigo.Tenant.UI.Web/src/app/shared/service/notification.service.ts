import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { Injectable, Inject, Optional, InjectionToken } from "@angular/core";
import { Http, Headers, Response, RequestOptionsArgs } from '@angular/http';
import { AmigoTenantServiceBase } from "../api/amigotenantservicebase";
import { environment } from "../../../environments/environment";
import { AmigoTenantOffsetBase } from '../api/amigotenantoffsetbase';
import { isContext } from 'vm';

export const API_BASE_URL = new InjectionToken('API_BASE_URL');

//#region SWAGGER
export class SwaggerException extends Error {
    message: string;
    status: number;
    response: string;
    result?: any;

    constructor(message: string, status: number, response: string, result?: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.result = result;
    }
}

function throwException(message: string, status: number, response: string, result?: any): Observable<any> {
    if(result !== null && result !== undefined)
        return Observable.throw(result);
    else
        return Observable.throw(new SwaggerException(message, status, response, null));
}
//#endregion

export class ApplicationMessage {
    key: string;
    message: string;

    constructor(data?: any) {
        if (data !== undefined) {
            this.key = data["Key"] !== undefined ? data["Key"] : null;
            this.message = data["Message"] !== undefined ? data["Message"] : null;
        }
    }

    static fromJS(data: any): ApplicationMessage {
        return new ApplicationMessage(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["Key"] = this.key !== undefined ? this.key : null;
        data["Message"] = this.message !== undefined ? this.message : null;
        return data;
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        const json = this.toJSON();
        return new ApplicationMessage(JSON.parse(json));
    }
}

export class ResponseDTO {
    isValid: boolean;
    pk: number;
    code: string;
    messages: ApplicationMessage[];

    constructor(data?: any) {
        if (data !== undefined) {
            this.isValid = data["IsValid"] !== undefined ? data["IsValid"] : null;
            this.pk = data["Pk"] !== undefined ? data["Pk"] : null;
            this.code = data["Code"] !== undefined ? data["Code"] : null;

            if (data["Messages"] && data["Messages"].constructor === Array) {
                this.messages = [];
                for (let item of data["Messages"])
                    this.messages.push(ApplicationMessage.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ResponseDTO {
        return new ResponseDTO(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["IsValid"] = this.isValid !== undefined ? this.isValid : null;
        data["Pk"] = this.pk !== undefined ? this.pk : null;
        data["Code"] = this.code !== undefined ? this.code : null;

        if (this.messages && this.messages.constructor === Array) {
            data["Messages"] = [];
            for (let item of this.messages)
                data["Messages"].push(item.toJS());
        }
        return data;
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        const json = this.toJSON();
        return new ResponseDTO(JSON.parse(json));
    }
}

export interface INotificationService {
    sendNotification(destinationNumber: Array<string>, textMessage: string): Observable<ResponseDTO>;
}    

@Injectable()
export class NotificationService extends  AmigoTenantServiceBase implements INotificationService {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
    
    constructor( @Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super();
        this.http = http;
        this.baseUrl = environment.serviceUrl;  // baseUrl ? baseUrl : "http://127.0.0.1:7072";
    }

    protected throwException(message: string, status: number, response: string, result?: any): any {
        if (result !== null && result !== undefined)
            throw result;
        else
            throw new SwaggerException(message, status, response);
    }

    sendNotification(destinationNumber: Array<string>, textMessage: string): Observable<ResponseDTO> {
        let url_ = this.baseUrl + "/api/notification";
        let content_ = '{' + '"destinationNumbers": ["' + destinationNumber.join('","') + '"], "textMessage":"' + textMessage + '"}';
        
        return this.http.request(url_, this.transformOptions({
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8",
                "Accept": "application/json; charset=UTF-8"
            })
        })).map((response) => {
            return this.transformResult(url_, response, (response) => this.processSendNotification(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processSendNotification(response)));
                } catch (e) {
                    return <Observable<ResponseDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTO>><any>Observable.throw(response);
        });
    }

    protected processSendNotification(response: Response): ResponseDTO {
        const responseText = response.text();
        const status = response.status;

        if (status === 200) {
            let result200: ResponseDTO = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTO.fromJS(resultData200) : new ResponseDTO();
            let resultNotification = JSON.parse(result200.messages[0].message, this.jsonParseReviver);
            result200.isValid = resultNotification.success;
            return result200;
        } else if (status !== 200 && status !== 204) {
            this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }
}

@Injectable()
export class NotificationServiceUI extends AmigoTenantServiceBase implements INotificationService {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
    
    constructor( @Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super();
        this.http = http;
        this.baseUrl = environment.serviceUrl;      // baseUrl ? baseUrl : "http://127.0.0.1:7072";
    }

    protected throwException(message: string, status: number, response: string, result?: any): any {
        if (result !== null && result !== undefined)
            throw result;
        else
            throw new SwaggerException(message, status, response);
    }

    sendNotification(destinationNumber: Array<string>, textMessage: string): Observable<ResponseDTO> {
        let url_ = environment.waUrlSvcEndPoint;
        let msgId = Guid.newGuid();
        let content_ =  
            "token="+ environment.waApikey + 
            "&uid=" + environment.waUserId + 
            "&to=" + destinationNumber + 
            "&custom_uid=" + msgId + 
            "&text=" + textMessage;
        
        //debugger;
        return this.http.request(url_, this.transformOptions({
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8",
                "Accept": "application/json; charset=UTF-8"
            })
        })).map((response) => {
            //debugger;
            return this.transformResult(url_, response, (response) => this.processSendNotification(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processSendNotification(response)));
                } catch (e) {
                    return <Observable<ResponseDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTO>><any>Observable.throw(response);
        });
    }

    protected processSendNotification(response: Response): ResponseDTO {
        const responseText = response.text();
        const status = response.status;

        if (status === 200) {
            debugger;
            let result200: ResponseDTO = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTO.fromJS(resultData200) : new ResponseDTO();
            result200.isValid = true;
            return result200;
        } else if (status !== 200 && status !== 204) {
            this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }
}

class Guid {
    static newGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            var r = Math.random()*16|0, v = c == 'x' ? r : (r&0x3|0x8);
            return v.toString(16);
        });
    }
}
