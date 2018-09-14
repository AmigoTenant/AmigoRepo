import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { Http, Headers, Response, RequestOptionsArgs } from '@angular/http';
import { AmigoTenantServiceBase } from './amigotenantservicebase';
import { AmigoTenantOffsetBase } from './amigotenantoffsetbase';
import { environment } from '../../../environments/environment';
import { UtilityHouseServicePeriodDTO } from '../dtos/UtilityBill/UtilityHouseServicePeriodDTO';

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

//#region Common

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

//#endregion

//#region UtilityBill Service Client

export interface IUtilityBillServiceClient {
    getHouseServicePeriod(houseId: number, year: number): Observable<ResponseDTOOfListOfHouseServicePeriodDTO>;
}

export interface IResponseDTOOfListOfHouseServiceDTO {
    data: UtilityHouseServicePeriodDTO[] | null;
    isValid: boolean | null;
    messages: ApplicationMessage[] | null;
}

export class ResponseDTOOfListOfHouseServicePeriodDTO implements IResponseDTOOfListOfHouseServiceDTO {
    data: UtilityHouseServicePeriodDTO[] | null;
    isValid: boolean | null;
    messages: ApplicationMessage[] | null;

    constructor(data?: IResponseDTOOfListOfHouseServiceDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            if (data["Data"] && data["Data"].constructor === Array) {
                this.data = [];
                for (let item of data["Data"])
                    this.data.push(UtilityHouseServicePeriodDTO.fromJS(item));
            }
            this.isValid = data["IsValid"] !== undefined ? data["IsValid"] : <any>null;
            if (data["Messages"] && data["Messages"].constructor === Array) {
                this.messages = [];
                for (let item of data["Messages"])
                    this.messages.push(ApplicationMessage.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ResponseDTOOfListOfHouseServicePeriodDTO {
        let result = new ResponseDTOOfListOfHouseServicePeriodDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        if (this.data && this.data.constructor === Array) {
            data["Data"] = [];
            for (let item of this.data)
                data["Data"].push(item.toJSON());
        }
        data["IsValid"] = this.isValid !== undefined ? this.isValid : <any>null;
        if (this.messages && this.messages.constructor === Array) {
            data["Messages"] = [];
            for (let item of this.messages)
                data["Messages"].push(item.toJSON());
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ResponseDTOOfListOfHouseServicePeriodDTO();
        result.init(json);
        return result;
    }
}

@Injectable()
export class UtilityBillServiceClient extends AmigoTenantServiceBase implements IUtilityBillServiceClient {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor( @Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super();
        this.http = http;
        this.baseUrl = environment.serviceUrl;  // baseUrl ? baseUrl : "http://127.0.0.1:7072/";     //
    }

    protected throwException(message: string, status: number, response: string, result?: any): any {
        if (result !== null && result !== undefined)
            throw result;
        else
            throw new SwaggerException(message, status, response);
    }

    /**
    * @return OK
    */
    getHouseServicePeriod(houseId: number, year: number): Observable<ResponseDTOOfListOfHouseServicePeriodDTO | null> {
        let url_ = this.baseUrl + "api/utilitybill/getHouseService?";

        if (houseId !== undefined)
            url_ += "houseId=" + encodeURIComponent("" + houseId) + "&";

        if (year !== undefined)
            url_ += "year=" + encodeURIComponent("" + year);

        const content_ = "";

        return this.http.request(url_, this.transformOptions({
            body: content_,
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8",
                "Accept": "application/json; charset=UTF-8"
            })
        })).map((response) => {
            return this.transformResult(url_, response, (response) => this.processGetHouseServicesByHouse(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processGetHouseServicesByHouse(response)));
                } catch (e) {
                    return <Observable<ResponseDTOOfListOfHouseServicePeriodDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfListOfHouseServicePeriodDTO>><any>Observable.throw(response);
        });
    }

    protected processGetHouseServicesByHouse(response: Response): ResponseDTOOfListOfHouseServicePeriodDTO {
        const responseText = response.text();
        const status = response.status;

        if (status === 200) {
            let result200: ResponseDTOOfListOfHouseServicePeriodDTO = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfListOfHouseServicePeriodDTO.fromJS(resultData200) : new ResponseDTOOfListOfHouseServicePeriodDTO();
            return result200;
        } else if (status !== 200 && status !== 204) {
            this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }

}


//#endregion
