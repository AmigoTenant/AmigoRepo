import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { Http, Headers, Response, RequestOptionsArgs } from '@angular/http';
import { AmigoTenantServiceBase } from './amigotenantservicebase';
import { AmigoTenantOffsetBase } from './amigotenantoffsetbase';
import { environment } from '../../../environments/environment';
import { IHouseServiceDTO } from '../dtos/House/IHouseServiceDTO';
import { HouseServicePeriodDTO } from '../dtos/House/HouseServicePeriodDTO';
import { ServiceHouseDTO } from '../dtos/House/ServiceHouseDTO';
import { HouseServiceDTO } from '../dtos/House/HouseServiceDTO';

export const API_BASE_URL = new InjectionToken('API_BASE_URL');

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

//#region SERVICE HOUSE

export class ResponseDTOOfListOfServiceHouseDTO implements IResponseDTOOfListOfServiceHouseDTO {
    data: ServiceHouseDTO[] | null;
    isValid: boolean | null;
    messages: ApplicationMessage[] | null;

    constructor(data?: IResponseDTOOfListOfServiceHouseDTO) {
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
                    this.data.push(ServiceHouseDTO.fromJS(item));
            }
            this.isValid = data["IsValid"] !== undefined ? data["IsValid"] : <any>null;
            if (data["Messages"] && data["Messages"].constructor === Array) {
                this.messages = [];
                for (let item of data["Messages"])
                    this.messages.push(ApplicationMessage.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ResponseDTOOfListOfServiceHouseDTO {
        let result = new ResponseDTOOfListOfServiceHouseDTO();
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
        let result = new ResponseDTOOfListOfServiceHouseDTO();
        result.init(json);
        return result;
    }
}

export interface IResponseDTOOfListOfServiceHouseDTO {
    data: ServiceHouseDTO[] | null;
    isValid: boolean | null;
    messages: ApplicationMessage[] | null;
}

//#endregion

//#region HOUSE SERVICE

export class ResponseDTOOfListOfHouseServiceDTO implements IResponseDTOOfListOfHouseServiceDTO {
    data: HouseServiceDTO[] | null;
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
                    this.data.push(HouseServiceDTO.fromJS(item));
            }
            this.isValid = data["IsValid"] !== undefined ? data["IsValid"] : <any>null;
            if (data["Messages"] && data["Messages"].constructor === Array) {
                this.messages = [];
                for (let item of data["Messages"])
                    this.messages.push(ApplicationMessage.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ResponseDTOOfListOfHouseServiceDTO {
        let result = new ResponseDTOOfListOfHouseServiceDTO();
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
        let result = new ResponseDTOOfListOfHouseServiceDTO();
        result.init(json);
        return result;
    }
}

export interface IResponseDTOOfListOfHouseServiceDTO {
    data: HouseServiceDTO[] | null;
    isValid: boolean | null;
    messages: ApplicationMessage[] | null;
}

export class HouseServiceRequest {
    houseServiceId: number;
    houseId: number;
    serviceId: number;

    rowStatus: boolean;
    createdBy: number;
    creationDate: Date;
    userId: number;
    userName: string;

    isNew: boolean;

    houseServicePeriods: HouseServicePeriodDTO[];

    constructor(data?: any) {
        if (data !== undefined) {
            this.houseId = data["HouseId"] !== undefined ? data["HouseId"] : null;
            this.houseServiceId = data["HouseServiceId"] !== undefined ? data["HouseServiceId"] : null;
            this.serviceId = data["ServiceId"] !== undefined ? data["ServiceId"] : null;
            
            this.rowStatus = data["RowStatus"] !== undefined ? data["RowStatus"] : null;
            
            this.createdBy = data["CreatedBy"] !== undefined ? data["CreatedBy"] : null;
            this.creationDate = data["CreationDate"] !== undefined ? data["CreationDate"] : null;
            this.userId = data["UserId"] !== undefined ? data["UserId"] : null;
            this.userName = data["UserName"] !== undefined ? data["UserName"] : null;

            this.houseServicePeriods = data["HouseServicePeriods"] !== undefined ? data["ServiceServicePeriods"] : null;
        }
    }

    static fromJS(data: any): HouseServiceRequest {
        return new HouseServiceRequest(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["HouseId"] = this.houseId !== undefined ? this.houseId : null;
        data["HouseServiceId"] = this.houseServiceId !== undefined ? this.houseServiceId : null;
        data["ServiceId"] = this.serviceId !== undefined ? this.serviceId : null;

        data["RowStatus"] = this.rowStatus !== undefined ? this.rowStatus : null;
        data["CreatedBy"] = this.createdBy !== undefined ? this.createdBy : null;
        data["CreationDate"] = this.creationDate !== undefined ? this.creationDate : null;
        data["UserId"] = this.userId !== undefined ? this.userId : null;
        data["UserName"] = this.userName !== undefined ? this.userName : null;

        data["HouseServicePeriods"] = this.houseServicePeriods !== undefined ? this.houseServicePeriods : null;

        return data;
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        const json = this.toJSON();
        return new HouseServiceRequest(JSON.parse(json));
    }

    static create(houseId: number, houseServiceId: number, userId: number, serviceId: number) : HouseServiceRequest {
        var newObj = new HouseServiceRequest();
        newObj.houseId = houseId;
        newObj.houseServiceId = houseServiceId;
        newObj.isNew = true;
        newObj.userId = userId;
        newObj.createdBy = userId;
        newObj.creationDate = new Date();
        newObj.rowStatus = true;
        newObj.serviceId = serviceId;
        newObj.houseServicePeriods = new Array<HouseServicePeriodDTO>();
        return newObj;
    }
}

export class DeleteHouseServiceRequest {
    houseId: number;
    houseServiceId: number;
    userId: number;
    username: string;

    constructor(data?: any) {
        if (data !== undefined) {
            this.houseId = data["HouseId"] !== undefined ? data["HouseId"] : null;
            this.houseServiceId = data["HouseServiceId"] !== undefined ? data["HouseServiceId"] : null;
            this.userId = data["UserId"] !== undefined ? data["UserId"] : null;
            this.username = data["Username"] !== undefined ? data["Username"] : null;
        }
    }

    static fromJS(data: any): DeleteHouseServiceRequest {
        return new DeleteHouseServiceRequest(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["HouseId"] = this.houseId !== undefined ? this.houseId : null;
        data["HouseServiceId"] = this.houseServiceId !== undefined ? this.houseServiceId : null;
        data["UserId"] = this.userId !== undefined ? this.userId : null;
        data["Username"] = this.username !== undefined ? this.username : null;
        return data;
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        const json = this.toJSON();
        return new DeleteHouseServiceRequest(JSON.parse(json));
    }
}

export class MonthDay {
    public period: number;
    public month: string;
    public day: string;
    constructor(private monthDay: string) {
        this.month = monthDay.substr(1, 2);
        this.day = monthDay.substr(3,2);
        this.period = parseInt(this.month);
    }
}

export interface IHouseServiceClient {
    getHouseServicesByHouse(houseId: number, year: number): Observable<ResponseDTOOfListOfHouseServiceDTO>;

    getHouseService(houseServiceId: number): Observable<ServiceHouseDTO>;

    getServiceHousesAll(): Observable<ResponseDTOOfListOfServiceHouseDTO>;

    deleteHouseService(service: DeleteHouseServiceRequest);
}

@Injectable()
export class HouseServiceClient extends AmigoTenantServiceBase implements IHouseServiceClient {
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
    getHouseServicesByHouse(houseId: number, year: number): Observable<ResponseDTOOfListOfHouseServiceDTO | null> {
        let url_ = this.baseUrl + "api/house/getHouseService?";

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
                    return <Observable<ResponseDTOOfListOfHouseServiceDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfListOfHouseServiceDTO>><any>Observable.throw(response);
        });
    }

    protected processGetHouseServicesByHouse(response: Response): ResponseDTOOfListOfHouseServiceDTO {
        const responseText = response.text();
        const status = response.status;

        if (status === 200) {
            let result200: ResponseDTOOfListOfHouseServiceDTO = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfListOfHouseServiceDTO.fromJS(resultData200) : new ResponseDTOOfListOfHouseServiceDTO();
            return result200;
        } else if (status !== 200 && status !== 204) {
            this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }

    /**
    * @return OK
    */
    getHouseService(houseServiceId: number): Observable<ServiceHouseDTO | null> {
        let url_ = this.baseUrl + "api/house/getHouseService?";

        if (houseServiceId !== undefined)
            url_ += "houseServiceId=" + encodeURIComponent("" + houseServiceId) + "&";

        const content_ = "";

        return this.http.request(url_, this.transformOptions({
            body: content_,
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8",
                "Accept": "application/json; charset=UTF-8"
            })
        })).map((response) => {
            return this.transformResult(url_, response, (response) => this.processGetHouseService(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processGetHouseService(response)));
                } catch (e) {
                    return <Observable<ServiceHouseDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ServiceHouseDTO>><any>Observable.throw(response);
        });
    }

    protected processGetHouseService(response: Response): ServiceHouseDTO {
        const responseText = response.text();
        const status = response.status;

        if (status === 200) {
            let result200: ServiceHouseDTO = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ServiceHouseDTO.fromJS(resultData200) : new ServiceHouseDTO();
            return result200;
        } else if (status !== 200 && status !== 204) {
            this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }

    /**
    * @return OK
    */
    getServiceHousesAll(): Observable<ResponseDTOOfListOfServiceHouseDTO | null> {
        let url_ = this.baseUrl + "api/house/getServicesHouseNew";

        const content_ = "";

        return this.http.request(url_, this.transformOptions({
            body: content_,
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8",
                "Accept": "application/json; charset=UTF-8"
            })
        })).map((response) => {
            return this.transformResult(url_, response, (response) => this.processGetHouseServicesAll(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processGetHouseServicesAll(response)));
                } catch (e) {
                    return <Observable<ResponseDTOOfListOfServiceHouseDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfListOfServiceHouseDTO>><any>Observable.throw(response);
        });
    }

    protected processGetHouseServicesAll(response: Response): ResponseDTOOfListOfServiceHouseDTO {
        const responseText = response.text();
        const status = response.status;

        if (status === 200) {
            let result200: ResponseDTOOfListOfServiceHouseDTO = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfListOfServiceHouseDTO.fromJS(resultData200) : new ResponseDTOOfListOfServiceHouseDTO();
            return result200;
        } else if (status !== 200 && status !== 204) {
            this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }
        /**
     * @return OK
     */
    registerHouseService(houseService: HouseServiceRequest): Observable<ResponseDTO> {
        let url_ = this.baseUrl + "api/house/registerHouseService";
        let content_ = JSON.stringify(houseService ? houseService.toJS() : null) ;
        
        return this.http.request(url_, this.transformOptions({
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8",
                "Accept": "application/json; charset=UTF-8"
            })
        })).map((response) => {
            return this.transformResult(url_, response, (response) => this.processRegisterHouseService(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processRegisterHouseService(response)));
                } catch (e) {
                    return <Observable<ResponseDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTO>><any>Observable.throw(response);
        });
    }

    protected processRegisterHouseService(response: Response): ResponseDTO {
        const responseText = response.text();
        const status = response.status;

        if (status === 200) {
            let result200: ResponseDTO = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTO.fromJS(resultData200) : new ResponseDTO();
            return result200;
        } else if (status !== 200 && status !== 204) {
            this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }

    deleteHouseService(house: DeleteHouseServiceRequest): Observable<ResponseDTO> {
        let url_ = this.baseUrl + "/api/house/deleteHouseService";
    
        const content_ = JSON.stringify(house ? house.toJS() : null);
    
        return this.http.request(url_, this.transformOptions({
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8",
                "Accept": "application/json; charset=UTF-8"
            })
        })).map((response) => {
            return this.transformResult(url_, response, (response) => this.processDeleteHouseService(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processDeleteHouseService(response)));
                } catch (e) {
                    return <Observable<ResponseDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTO>><any>Observable.throw(response);
        });
    }
    
    protected processDeleteHouseService(response: Response): ResponseDTO {
        const responseText = response.text();
        const status = response.status;
    
        if (status === 200) {
            let result200: ResponseDTO = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTO.fromJS(resultData200) : new ResponseDTO();
            return result200;
        } else if (status !== 200 && status !== 204) {
            this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }
    
}

//#edregion

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

//#region CONCEPTS
export interface IConceptClient {
    /**
     * @return OK
     */
    getConceptsByTypeCode(code: string): Observable<ResponseDTOOfListOfConceptDTO | null>;
}

@Injectable()
export class ConceptClient extends AmigoTenantServiceBase implements IConceptClient {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor( @Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super();
        this.http = http;
        this.baseUrl = environment.serviceUrl;  // baseUrl ? baseUrl : "http://127.0.0.1:7072/";  //
    }

    /**
     * @return OK
     */
    getConceptsByTypeCode(code: string): Observable<ResponseDTOOfListOfConceptDTO | null> {
        let url_ = this.baseUrl + "api/concept/getConceptsByTypeCode?";
        if (code === undefined || code === null)
            throw new Error("The parameter 'code' must be defined and cannot be null.");
        else
            url_ += "code=" + encodeURIComponent("" + code) + "&";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = "";

        return this.http.request(url_, this.transformOptions({
            body: content_,
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8",
                "Accept": "application/json; charset=UTF-8"
            })
        })).map((response) => {
            return this.transformResult(url_, response, (response) => this.processGetConceptsByTypeCode(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processGetConceptsByTypeCode(response)));
                } catch (e) {
                    return <Observable<ResponseDTOOfListOfConceptDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfListOfConceptDTO>><any>Observable.throw(response);
            });
    }

    protected processGetConceptsByTypeCode(response: Response): ResponseDTOOfListOfConceptDTO | null {
        const status = response.status;

        if (status === 200) {
            const responseText = response.text();
            let result200: ResponseDTOOfListOfConceptDTO | null = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfListOfConceptDTO.fromJS(resultData200) : new ResponseDTOOfListOfConceptDTO();
            return result200;
        } else if (status !== 200 && status !== 204) {
            const responseText = response.text();
            return this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }

    protected throwException(message: string, status: number, response: string, result?: any): any {
        if (result !== null && result !== undefined)
            throw result;
        else
            throw new SwaggerException(message, status, response);
    }
}


export class ResponseDTOOfListOfConceptDTO implements IResponseDTOOfListOfConceptDTO {
    data: ConceptDTO[] | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;

    constructor(data?: IResponseDTOOfListOfConceptDTO) {
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
                    this.data.push(ConceptDTO.fromJS(item));
            }
            this.isValid = data["IsValid"] !== undefined ? data["IsValid"] : <any>null;
            this.pk = data["Pk"] !== undefined ? data["Pk"] : <any>null;
            this.code = data["Code"] !== undefined ? data["Code"] : <any>null;
            if (data["Messages"] && data["Messages"].constructor === Array) {
                this.messages = [];
                for (let item of data["Messages"])
                    this.messages.push(ApplicationMessage.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ResponseDTOOfListOfConceptDTO {
        let result = new ResponseDTOOfListOfConceptDTO();
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
        data["Pk"] = this.pk !== undefined ? this.pk : <any>null;
        data["Code"] = this.code !== undefined ? this.code : <any>null;
        if (this.messages && this.messages.constructor === Array) {
            data["Messages"] = [];
            for (let item of this.messages)
                data["Messages"].push(item.toJSON());
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ResponseDTOOfListOfConceptDTO();
        result.init(json);
        return result;
    }
}

export interface IResponseDTOOfListOfConceptDTO {
    data: ConceptDTO[] | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;
}

export class ConceptDTO implements IConceptDTO {
    conceptId: number | null;
    code: string | null;
    description: string | null;
    typeId: number | null;
    remark: string | null;
    conceptAmount: number | null;
    payTypeId: number | null;
    rowStatus: boolean | null;
    createdBy: number | null;
    creationDate: Date | null;
    updatedBy: number | null;
    updatedDate: Date | null;
    typeCode: string | null;

    constructor(data?: IConceptDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.conceptId = data["ConceptId"] !== undefined ? data["ConceptId"] : <any>null;
            this.code = data["Code"] !== undefined ? data["Code"] : <any>null;
            this.description = data["Description"] !== undefined ? data["Description"] : <any>null;
            this.typeId = data["TypeId"] !== undefined ? data["TypeId"] : <any>null;
            this.remark = data["Remark"] !== undefined ? data["Remark"] : <any>null;
            this.conceptAmount = data["ConceptAmount"] !== undefined ? data["ConceptAmount"] : <any>null;
            this.payTypeId = data["PayTypeId"] !== undefined ? data["PayTypeId"] : <any>null;
            this.rowStatus = data["RowStatus"] !== undefined ? data["RowStatus"] : <any>null;
            this.createdBy = data["CreatedBy"] !== undefined ? data["CreatedBy"] : <any>null;
            this.creationDate = data["CreationDate"] ? new Date(data["CreationDate"].toString()) : <any>null;
            this.updatedBy = data["UpdatedBy"] !== undefined ? data["UpdatedBy"] : <any>null;
            this.updatedDate = data["UpdatedDate"] ? new Date(data["UpdatedDate"].toString()) : <any>null;
            this.typeCode = data["TypeCode"] !== undefined ? data["TypeCode"] : <any>null;
        }
    }

    static fromJS(data: any): ConceptDTO {
        let result = new ConceptDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["ConceptId"] = this.conceptId !== undefined ? this.conceptId : <any>null;
        data["Code"] = this.code !== undefined ? this.code : <any>null;
        data["Description"] = this.description !== undefined ? this.description : <any>null;
        data["TypeId"] = this.typeId !== undefined ? this.typeId : <any>null;
        data["Remark"] = this.remark !== undefined ? this.remark : <any>null;
        data["ConceptAmount"] = this.conceptAmount !== undefined ? this.conceptAmount : <any>null;
        data["PayTypeId"] = this.payTypeId !== undefined ? this.payTypeId : <any>null;
        data["RowStatus"] = this.rowStatus !== undefined ? this.rowStatus : <any>null;
        data["CreatedBy"] = this.createdBy !== undefined ? this.createdBy : <any>null;
        data["CreationDate"] = this.creationDate ? this.creationDate.toISOString() : <any>null;
        data["UpdatedBy"] = this.updatedBy !== undefined ? this.updatedBy : <any>null;
        data["UpdatedDate"] = this.updatedDate ? this.updatedDate.toISOString() : <any>null;
        data["TypeCode"] = this.typeCode !== undefined ? this.typeCode : <any>null;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ConceptDTO();
        result.init(json);
        return result;
    }
}

export interface IConceptDTO {
    conceptId: number | null;
    code: string | null;
    description: string | null;
    typeId: number | null;
    remark: string | null;
    conceptAmount: number | null;
    payTypeId: number | null;
    rowStatus: boolean | null;
    createdBy: number | null;
    creationDate: Date | null;
    updatedBy: number | null;
    updatedDate: Date | null;
    typeCode: string | null;
}

//#endregion
