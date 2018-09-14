import 'rxjs/Rx';
import {Observable} from 'rxjs/Observable';
import {Injectable, Inject, Optional, InjectionToken} from '@angular/core';
import {Http, Headers, Response, RequestOptionsArgs} from '@angular/http';
import {AmigoTenantServiceBase} from './amigotenantservicebase';
import { AmigoTenantOffsetBase } from './amigotenantoffsetbase';
import { environment } from '../../../environments/environment';

export const API_BASE_URL = new InjectionToken('API_BASE_URL');

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

export interface IRentalApplicationClient {
    /**
     * @return OK
     */
    search(search_periodId: number, search_propertyTypeId: number, search_applicationDateFrom: Date, search_applicationDateTo: Date, search_fullName: string, search_email: string, search_checkInFrom: Date, search_checkInTo: Date, search_checkOutFrom: Date, search_checkOutTo: Date, search_residenseCountryId: number, search_residenseCountryName: string, search_budgetId: number, search_featureIds: number[], search_citiesIds: number[],
        search_cityOfInterestId: number,
        search_housePartId: number,
        search_personNo: number,
        search_outInDownId: number,
        search_referredById: number,
        search_hasNotification: boolean, 
        search_page: number,
        search_pageSize: number): Observable<ResponseDTOOfPagedListOfRentalApplicationSearchDTO | null>;

     /**
     * @return OK
     */
    getById(id: number): Observable<ResponseDTOOfRentalApplicationRegisterRequest | null>;

    /**
     * @return OK
     */
    register(rentalApplication: RentalApplicationRegisterRequest): Observable<ResponseDTO | null>;
    /**
     * @return OK
     */
    update(rentalApplication: RentalApplicationUpdateRequest): Observable<ResponseDTO | null>;
    /**
     * @return OK
     */
    delete(rentalApplication: RentalApplicationDeleteRequest): Observable<ResponseDTO | null>;
}

@Injectable()
export class RentalApplicationClient extends AmigoTenantServiceBase implements IRentalApplicationClient {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor( @Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super();
        this.http = http;
        this.baseUrl = environment.serviceUrl;  // baseUrl ? baseUrl : "http://127.0.0.1:7072";
    }

    /**
     * @return OK
     */
    search(search_periodId: number, search_propertyTypeId: number, search_applicationDateFrom: Date,
        search_applicationDateTo: Date, search_fullName: string, search_email: string,
        search_checkInFrom: Date, search_checkInTo: Date, search_checkOutFrom: Date,
        search_checkOutTo: Date, search_residenseCountryId: number, search_residenseCountryName: string,
        search_budgetId: number, search_featureIds: number[], search_citiesIds: number[],
        search_cityOfInterestId: number,
        search_housePartId: number , 
        search_personNo: number,
        search_outInDownId: number,
        search_referredById: number,
        search_hasNotification: boolean, 
        search_page: number, search_pageSize: number

    ): Observable<ResponseDTOOfPagedListOfRentalApplicationSearchDTO | null> {
        let url_ = this.baseUrl + "/api/rentalApplication/searchCriteria?";
        if (search_periodId !== undefined)
            url_ += "search.periodId=" + encodeURIComponent("" + search_periodId) + "&";
        if (search_propertyTypeId !== undefined)
            url_ += "search.propertyTypeId=" + encodeURIComponent("" + search_propertyTypeId) + "&";
        if (search_applicationDateFrom !== undefined)
            url_ += "search.applicationDateFrom=" + encodeURIComponent("" + search_applicationDateFrom.toJSON()) + "&";
        if (search_applicationDateTo !== undefined)
            url_ += "search.applicationDateTo=" + encodeURIComponent("" + search_applicationDateTo.toJSON()) + "&";
        if (search_fullName !== undefined)
            url_ += "search.fullName=" + encodeURIComponent("" + search_fullName) + "&";
        if (search_email !== undefined)
            url_ += "search.email=" + encodeURIComponent("" + search_email) + "&";
        if (search_checkInFrom !== undefined)
            url_ += "search.checkInFrom=" + encodeURIComponent("" + search_checkInFrom.toJSON()) + "&";
        if (search_checkInTo !== undefined)
            url_ += "search.checkInTo=" + encodeURIComponent("" + search_checkInTo.toJSON()) + "&";
        if (search_checkOutFrom !== undefined)
            url_ += "search.checkOutFrom=" + encodeURIComponent("" + search_checkOutFrom.toJSON()) + "&";
        if (search_checkOutTo !== undefined)
            url_ += "search.checkOutTo=" + encodeURIComponent("" + search_checkOutTo.toJSON()) + "&";
        if (search_residenseCountryId !== undefined)
            url_ += "search.residenseCountryId=" + encodeURIComponent("" + search_residenseCountryId) + "&";
        if (search_residenseCountryName !== undefined)
            url_ += "search.residenseCountryName=" + encodeURIComponent("" + search_residenseCountryName) + "&";
        if (search_budgetId !== undefined)
            url_ += "search.budgetId=" + encodeURIComponent("" + search_budgetId) + "&";
        //if (search_featureIds !== undefined)
        //    search_featureIds.forEach(item => { url_ += "search.featureIds=" + encodeURIComponent("" + item) + "&"; });
        //if (search_citiesIds !== undefined)
        //    search_citiesIds.forEach(item => { url_ += "search.citiesIds=" + encodeURIComponent("" + item) + "&"; });
        if (search_cityOfInterestId !== undefined)
            url_ += "search.cityOfInterestId=" + encodeURIComponent("" + search_cityOfInterestId) + "&";
        if (search_housePartId !== undefined)
            url_ += "search.housePartId=" + encodeURIComponent("" + search_housePartId) + "&";
        if (search_personNo !== undefined)
            url_ += "search.personNo=" + encodeURIComponent("" + search_personNo) + "&";
        if (search_outInDownId !== undefined)
            url_ += "search.outInDownId=" + encodeURIComponent("" + search_outInDownId) + "&";
        if (search_referredById !== undefined)
            url_ += "search.referredById=" + encodeURIComponent("" + search_referredById) + "&";
        if (search_hasNotification !== undefined)
            url_ += "search.hasNotification=" + encodeURIComponent("" + search_hasNotification) + "&";

        if (search_page !== undefined)
            url_ += "search.page=" + encodeURIComponent("" + search_page) + "&";
        if (search_pageSize !== undefined)
            url_ += "search.pageSize=" + encodeURIComponent("" + search_pageSize) + "&";
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
            return this.transformResult(url_, response, (response) => this.processSearch(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processSearch(response)));
                } catch (e) {
                    return <Observable<ResponseDTOOfPagedListOfRentalApplicationSearchDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfPagedListOfRentalApplicationSearchDTO>><any>Observable.throw(response);
            });
        
    }

    protected processSearch(response: Response): ResponseDTOOfPagedListOfRentalApplicationSearchDTO | null {
        const status = response.status;

        if (status === 200) {
            const responseText = response.text();
            let result200: ResponseDTOOfPagedListOfRentalApplicationSearchDTO = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfPagedListOfRentalApplicationSearchDTO.fromJS(resultData200) : new ResponseDTOOfPagedListOfRentalApplicationSearchDTO();
            return result200;
        } else if (status !== 200 && status !== 204) {
            const responseText = response.text();
            return this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }

    /**
         * @return OK
         */
    getById(id: number): Observable<ResponseDTOOfRentalApplicationRegisterRequest | null> {
        let url_ = this.baseUrl + "/api/rentalApplication/getById?";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined and cannot be null.");
        else
            url_ += "id=" + encodeURIComponent("" + id) + "&";
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
            return this.transformResult(url_, response, (response) => this.processGetById(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processGetById(response)));
                } catch (e) {
                    return <Observable<ResponseDTOOfRentalApplicationRegisterRequest>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfRentalApplicationRegisterRequest>><any>Observable.throw(response);
        });
    }

    protected processGetById(response: Response): ResponseDTOOfRentalApplicationRegisterRequest {
        const status = response.status;

        if (status === 200) {
            const responseText = response.text();
            let result200: ResponseDTOOfRentalApplicationRegisterRequest = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfRentalApplicationRegisterRequest.fromJS(resultData200) : new ResponseDTOOfRentalApplicationRegisterRequest();
            return result200;
        } else if (status !== 200 && status !== 204) {
            const responseText = response.text();
            return this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }

    /**
     * @return OK
     */
    register(rentalApplication: RentalApplicationRegisterRequest): Observable<ResponseDTO | null> {
        let url_ = this.baseUrl + "/api/rentalApplication/register";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(rentalApplication ? rentalApplication.toJSON() : null);

        return this.http.request(url_, this.transformOptions({
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8",
                "Accept": "application/json; charset=UTF-8"
            })
        })).map((response) => {
            return this.transformResult(url_, response, (response) => this.processRegister(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processRegister(response)));
                } catch (e) {
                    return <Observable<ResponseDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTO>><any>Observable.throw(response);
        });
    }

    protected processRegister(response: Response): ResponseDTO {
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

    /**
     * @return OK
     */
    update(rentalApplication: RentalApplicationUpdateRequest): Observable<ResponseDTO | null> {
        let url_ = this.baseUrl + "/api/rentalApplication/update";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(rentalApplication ? rentalApplication.toJSON() : null);

        return this.http.request(url_, this.transformOptions({
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8",
                "Accept": "application/json; charset=UTF-8"
            })
        })).map((response) => {
            return this.transformResult(url_, response, (response) => this.processUpdate(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processUpdate(response)));
                } catch (e) {
                    return <Observable<ResponseDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTO>><any>Observable.throw(response);
        });
    }

    protected processUpdate(response: Response): ResponseDTO {
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

    /**
     * @return OK
     */
    delete(rentalApplication: RentalApplicationDeleteRequest): Observable<ResponseDTO | null> {
        let url_ = this.baseUrl + "/api/rentalApplication/delete";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(rentalApplication ? rentalApplication.toJSON() : null);

        return this.http.request(url_, this.transformOptions({
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json; charset=UTF-8",
                "Accept": "application/json; charset=UTF-8"
            })
        })).map((response) => {
            return this.transformResult(url_, response, (response) => this.processDelete(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processDelete(response)));
                } catch (e) {
                    return <Observable<ResponseDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTO>><any>Observable.throw(response);
        });
    }

    protected processDelete(response: Response): ResponseDTO {
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

    protected throwException(message: string, status: number, response: string, result?: any): any {
        if (result !== null && result !== undefined)
            throw result;
        else
            throw new SwaggerException(message, status, response);
    }
}


export class RentalApplicationSearchRequest implements IRentalApplicationSearchRequest {
    periodId: number | null;
    propertyTypeId: number | null;
    applicationDateFrom: Date | null;
    applicationDateTo: Date | null;
    fullName: string | null;
    email: string | null;
    checkInFrom: Date | null;
    checkInTo: Date | null;
    checkOutFrom: Date | null;
    checkOutTo: Date | null;
    residenseCountryId: number | null;
    residenseCountryName: string | null;
    budgetId: number | null;
    //featureIds: number[] | null;
    //citiesIds: number[] | null;
    page: number | null;
    pageSize: number | null;
    cityOfInterestId: number | null;
    housePartId: number | null;
    personNo: number | null;
    outInDownId: number | null; 
    referredById: number | null;
    hasNotification: boolean | null; 


    constructor(data?: IRentalApplicationSearchRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.periodId = data["PeriodId"] !== undefined ? data["PeriodId"] : <any>null;
            this.propertyTypeId = data["PropertyTypeId"] !== undefined ? data["PropertyTypeId"] : <any>null;
            this.applicationDateFrom = data["ApplicationDateFrom"] ? new Date(data["ApplicationDateFrom"].toString()) : <any>null;
            this.applicationDateTo = data["ApplicationDateTo"] ? new Date(data["ApplicationDateTo"].toString()) : <any>null;
            this.fullName = data["FullName"] !== undefined ? data["FullName"] : <any>null;
            this.email = data["Email"] !== undefined ? data["Email"] : <any>null;
            this.checkInFrom = data["CheckInFrom"] ? new Date(data["CheckInFrom"].toString()) : <any>null;
            this.checkInTo = data["CheckInTo"] ? new Date(data["CheckInTo"].toString()) : <any>null;
            this.checkOutFrom = data["CheckOutFrom"] ? new Date(data["CheckOutFrom"].toString()) : <any>null;
            this.checkOutTo = data["CheckOutTo"] ? new Date(data["CheckOutTo"].toString()) : <any>null;
            this.residenseCountryId = data["ResidenseCountryId"] !== undefined ? data["ResidenseCountryId"] : <any>null;
            this.residenseCountryName = data["ResidenseCountryName"] !== undefined ? data["ResidenseCountryName"] : <any>null;
            this.budgetId = data["BudgetId"] !== undefined ? data["BudgetId"] : <any>null;
            //if (data["FeatureIds"] && data["FeatureIds"].constructor === Array) {
            //    this.featureIds = [];
            //    for (let item of data["FeatureIds"])
            //        this.featureIds.push(item);
            //}
            //if (data["CitiesIds"] && data["CitiesIds"].constructor === Array) {
            //    this.citiesIds = [];
            //    for (let item of data["CitiesIds"])
            //        this.citiesIds.push(item);
            //}
            this.page = data["Page"] !== undefined ? data["Page"] : <any>null;
            this.pageSize = data["PageSize"] !== undefined ? data["PageSize"] : <any>null;
            this.cityOfInterestId = data["CityOfInterestId"] !== undefined ? data["CityOfInterestId"] : <any>null;
            this.housePartId = data["HousePartId"] !== undefined ? data["HousePartId"] : <any>null;
            this.personNo = data["PersonNo"] !== undefined ? data["PersonNo"] : <any>null;
            this.outInDownId = data["OutInDownId"] !== undefined ? data["OutInDownId"] : <any>null;
            this.referredById = data["ReferredById"] !== undefined ? data["ReferredById"] : <any>null;
            this.hasNotification = data["HasNotification"] !== undefined ? data["HasNotification"] : <any>null;

        }
    }

    static fromJS(data: any): RentalApplicationSearchRequest {
        let result = new RentalApplicationSearchRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["PeriodId"] = this.periodId !== undefined ? this.periodId : <any>null;
        data["PropertyTypeId"] = this.propertyTypeId !== undefined ? this.propertyTypeId : <any>null;
        data["ApplicationDateFrom"] = this.applicationDateFrom ? this.applicationDateFrom.toISOString() : <any>null;
        data["ApplicationDateTo"] = this.applicationDateTo ? this.applicationDateTo.toISOString() : <any>null;
        data["FullName"] = this.fullName !== undefined ? this.fullName : <any>null;
        data["Email"] = this.email !== undefined ? this.email : <any>null;
        data["CheckInFrom"] = this.checkInFrom ? this.checkInFrom.toISOString() : <any>null;
        data["CheckInTo"] = this.checkInTo ? this.checkInTo.toISOString() : <any>null;
        data["CheckOutFrom"] = this.checkOutFrom ? this.checkOutFrom.toISOString() : <any>null;
        data["CheckOutTo"] = this.checkOutTo ? this.checkOutTo.toISOString() : <any>null;
        data["ResidenseCountryId"] = this.residenseCountryId !== undefined ? this.residenseCountryId : <any>null;
        data["ResidenseCountryName"] = this.residenseCountryName !== undefined ? this.residenseCountryName : <any>null;
        data["BudgetId"] = this.budgetId !== undefined ? this.budgetId : <any>null;
        //if (this.featureIds && this.featureIds.constructor === Array) {
        //    data["FeatureIds"] = [];
        //    for (let item of this.featureIds)
        //        data["FeatureIds"].push(item);
        //}
        //if (this.citiesIds && this.citiesIds.constructor === Array) {
        //    data["CitiesIds"] = [];
        //    for (let item of this.citiesIds)
        //        data["CitiesIds"].push(item);
        //}
        data["Page"] = this.page !== undefined ? this.page : <any>null;
        data["PageSize"] = this.pageSize !== undefined ? this.pageSize : <any>null;
        data["CityOfInterestId"] = this.cityOfInterestId !== undefined ? this.cityOfInterestId : <any>null;
        data["HousePartId"] = this.housePartId !== undefined ? this.housePartId : <any>null;
        data["PersonNo"] = this.personNo !== undefined ? this.personNo : <any>null;
        data["OutInDownId"] = this.outInDownId !== undefined ? this.outInDownId : <any>null;
        data["ReferredById"] = this.referredById !== undefined ? this.referredById : <any>null;
        data["HasNotification"] = this.hasNotification !== undefined ? this.hasNotification : <any>null;

        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new RentalApplicationSearchRequest();
        result.init(json);
        return result;
    }
}

export interface IRentalApplicationSearchRequest {
    periodId: number | null;
    propertyTypeId: number | null;
    applicationDateFrom: Date | null;
    applicationDateTo: Date | null;
    fullName: string | null;
    email: string | null;
    checkInFrom: Date | null;
    checkInTo: Date | null;
    checkOutFrom: Date | null;
    checkOutTo: Date | null;
    residenseCountryId: number | null;
    residenseCountryName: string | null;
    budgetId: number | null;
    //featureIds: number[] | null;
    //citiesIds: number[] | null;
    page: number | null;
    pageSize: number | null;
    cityOfInterestId: number | null;
    housePartId: number | null;
    personNo: number | null;
    outInDownId: number | null; 
    referredById: number | null;
    hasNotification: boolean | null; 

}

export class ResponseDTOOfPagedListOfRentalApplicationSearchDTO implements IResponseDTOOfPagedListOfRentalApplicationSearchDTO {
    data: PagedListOfRentalApplicationSearchDTO | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;

    constructor(data?: IResponseDTOOfPagedListOfRentalApplicationSearchDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.data = data["Data"] ? PagedListOfRentalApplicationSearchDTO.fromJS(data["Data"]) : <any>null;
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

    static fromJS(data: any): ResponseDTOOfPagedListOfRentalApplicationSearchDTO {
        let result = new ResponseDTOOfPagedListOfRentalApplicationSearchDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Data"] = this.data ? this.data.toJSON() : <any>null;
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
        let result = new ResponseDTOOfPagedListOfRentalApplicationSearchDTO();
        result.init(json);
        return result;
    }
}

export interface IResponseDTOOfPagedListOfRentalApplicationSearchDTO {
    data: PagedListOfRentalApplicationSearchDTO | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;
}

export class PagedListOfRentalApplicationSearchDTO implements IPagedListOfRentalApplicationSearchDTO {
    page: number | null;
    total: number | null;
    pageSize: number | null;
    items: RentalApplicationSearchDTO[] | null;

    constructor(data?: IPagedListOfRentalApplicationSearchDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.page = data["Page"] !== undefined ? data["Page"] : <any>null;
            this.total = data["Total"] !== undefined ? data["Total"] : <any>null;
            this.pageSize = data["PageSize"] !== undefined ? data["PageSize"] : <any>null;
            if (data["Items"] && data["Items"].constructor === Array) {
                this.items = [];
                for (let item of data["Items"])
                    this.items.push(RentalApplicationSearchDTO.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedListOfRentalApplicationSearchDTO {
        let result = new PagedListOfRentalApplicationSearchDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Page"] = this.page !== undefined ? this.page : <any>null;
        data["Total"] = this.total !== undefined ? this.total : <any>null;
        data["PageSize"] = this.pageSize !== undefined ? this.pageSize : <any>null;
        if (this.items && this.items.constructor === Array) {
            data["Items"] = [];
            for (let item of this.items)
                data["Items"].push(item.toJSON());
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedListOfRentalApplicationSearchDTO();
        result.init(json);
        return result;
    }
}

export interface IPagedListOfRentalApplicationSearchDTO {
    page: number | null;
    total: number | null;
    pageSize: number | null;
    items: RentalApplicationSearchDTO[] | null;
}

export class RentalApplicationSearchDTO implements IRentalApplicationSearchDTO {
    isSelected: boolean | null;
    rentalApplicationId: number | null;
    periodCode: string | null;
    periodId: number | null;
    propertyTypeId: number | null;
    propertyTypeName: string | null;
    applicationDate: Date | null;
    featuresCode: string | null;
    fullName: string | null;
    email: string | null;
    cellPhone: string | null;
    checkIn: Date | null;
    checkOut: Date | null;
    cities: string | null;
    residenseCountryName: string | null;
    budgetName: string | null;
    budgetId: number | null;
    availableProperties: number | null;
    rentedProperties: number | null;
    housePartName: string | null;
    outInDownName: string | null; 
    referredById: number | null;
    referredByName: number | null; 
    hasNotification: boolean | null;
    referredByOther: string | null;
    //alertBeforeThat: number | null; 
    personNo: number | null; 
    priorityId: number | null;
    //alertDate: Date | null;
    alertMessage: string | null;
    priorityName: string | null; 



    constructor(data?: IRentalApplicationSearchDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.isSelected = data["IsSelected"] !== undefined ? data["IsSelected"] : <any>null;
            this.rentalApplicationId = data["RentalApplicationId"] !== undefined ? data["RentalApplicationId"] : <any>null;
            this.periodCode = data["PeriodCode"] !== undefined ? data["PeriodCode"] : <any>null;
            this.periodId = data["PeriodId"] !== undefined ? data["PeriodId"] : <any>null;
            this.propertyTypeId = data["PropertyTypeId"] !== undefined ? data["PropertyTypeId"] : <any>null;
            this.propertyTypeName = data["PropertyTypeName"] !== undefined ? data["PropertyTypeName"] : <any>null;
            this.applicationDate = data["ApplicationDate"] ? new Date(data["ApplicationDate"].toString()) : <any>null;
            this.featuresCode = data["FeaturesCode"] !== undefined ? data["FeaturesCode"] : <any>null;
            this.fullName = data["FullName"] !== undefined ? data["FullName"] : <any>null;
            this.email = data["Email"] !== undefined ? data["Email"] : <any>null;
            this.cellPhone = data["CellPhone"] !== undefined ? data["CellPhone"] : <any>null;
            this.checkIn = data["CheckIn"] ? new Date(data["CheckIn"].toString()) : <any>null;
            this.checkOut = data["CheckOut"] ? new Date(data["CheckOut"].toString()) : <any>null;
            this.cities = data["Cities"] !== undefined ? data["Cities"] : <any>null;
            this.residenseCountryName = data["ResidenseCountryName"] !== undefined ? data["ResidenseCountryName"] : <any>null;
            this.budgetName = data["BudgetName"] !== undefined ? data["BudgetName"] : <any>null;
            this.budgetId = data["BudgetId"] !== undefined ? data["BudgetId"] : <any>null;
            this.availableProperties = data["AvailableProperties"] !== undefined ? data["AvailableProperties"] : <any>null;
            this.rentedProperties = data["RentedProperties"] !== undefined ? data["RentedProperties"] : <any>null;
            this.housePartName = data["HousePartName"] !== undefined ? data["HousePartName"] : <any>null;
            this.outInDownName = data["OutInDownName"] !== undefined ? data["OutInDownName"] : <any>null;
            this.referredById = data["ReferredById"] !== undefined ? data["ReferredById"] : <any>null;
            this.hasNotification = data["HasNotification"] !== undefined ? data["HasNotification"] : <any>null;
            this.referredByOther = data["ReferredByOther"] !== undefined ? data["ReferredByOther"] : <any>null;
            //this.alertBeforeThat = data["AlertBeforeThat"] !== undefined ? data["AlertBeforeThat"] : <any>null;
            this.personNo = data["PersonNo"] !== undefined ? data["PersonNo"] : <any>null;
            this.referredByName = data["ReferredByName"] !== undefined ? data["ReferredByName"] : <any>null;
            this.alertMessage = data["AlertMessage"] !== undefined ? data["AlertMessage"] : <any>null;
            this.priorityName = data["PriorityName"] !== undefined ? data["PriorityName"] : <any>null;


        }
    }

    static fromJS(data: any): RentalApplicationSearchDTO {
        let result = new RentalApplicationSearchDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["IsSelected"] = this.isSelected !== undefined ? this.isSelected : <any>null;
        data["RentalApplicationId"] = this.rentalApplicationId !== undefined ? this.rentalApplicationId : <any>null;
        data["PeriodCode"] = this.periodCode !== undefined ? this.periodCode : <any>null;
        data["PeriodId"] = this.periodId !== undefined ? this.periodId : <any>null;
        data["PropertyTypeId"] = this.propertyTypeId !== undefined ? this.propertyTypeId : <any>null;
        data["PropertyTypeName"] = this.propertyTypeName !== undefined ? this.propertyTypeName : <any>null;
        data["ApplicationDate"] = this.applicationDate ? this.applicationDate.toISOString() : <any>null;
        data["FeaturesCode"] = this.featuresCode !== undefined ? this.featuresCode : <any>null;
        data["FullName"] = this.fullName !== undefined ? this.fullName : <any>null;
        data["Email"] = this.email !== undefined ? this.email : <any>null;
        data["CellPhone"] = this.cellPhone !== undefined ? this.cellPhone : <any>null;
        data["CheckIn"] = this.checkIn ? this.checkIn.toISOString() : <any>null;
        data["CheckOut"] = this.checkOut ? this.checkOut.toISOString() : <any>null;
        data["Cities"] = this.cities !== undefined ? this.cities : <any>null;
        data["ResidenseCountryName"] = this.residenseCountryName !== undefined ? this.residenseCountryName : <any>null;
        data["BudgetName"] = this.budgetName !== undefined ? this.budgetName : <any>null;
        data["BudgetId"] = this.budgetId !== undefined ? this.budgetId : <any>null;
        data["AvailableProperties"] = this.availableProperties !== undefined ? this.availableProperties : <any>null;
        data["RentedProperties"] = this.rentedProperties !== undefined ? this.rentedProperties : <any>null;
        data["HousePartName"] = this.housePartName !== undefined ? this.housePartName : <any>null;
        data["OutInDownName"] = this.outInDownName !== undefined ? this.outInDownName : <any>null;
        data["ReferredById"] = this.referredById !== undefined ? this.referredById : <any>null;
        data["HasNotification"] = this.hasNotification !== undefined ? this.hasNotification : <any>null;
        data["ReferredByOther"] = this.referredByOther !== undefined ? this.referredByOther : <any>null;
        //data["AlertBeforeThat"] = this.alertBeforeThat !== undefined ? this.alertBeforeThat : <any>null;
        data["PersonNo"] = this.personNo !== undefined ? this.personNo : <any>null;
        data["ReferredByName"] = this.referredByName !== undefined ? this.referredByName : <any>null;
        data["AlertMessage"] = this.alertMessage !== undefined ? this.alertMessage : <any>null;
        data["PriorityName"] = this.priorityName !== undefined ? this.priorityName : <any>null;

        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new RentalApplicationSearchDTO();
        result.init(json);
        return result;
    }
}

export interface IRentalApplicationSearchDTO {
    isSelected: boolean | null;
    rentalApplicationId: number | null;
    periodCode: string | null;
    periodId: number | null;
    propertyTypeId: number | null;
    propertyTypeName: string | null;
    applicationDate: Date | null;
    featuresCode: string | null;
    fullName: string | null;
    email: string | null;
    cellPhone: string | null;
    checkIn: Date | null;
    checkOut: Date | null;
    cities: string | null;
    residenseCountryName: string | null;
    budgetName: string | null;
    budgetId: number | null;
    availableProperties: number | null;
    rentedProperties: number | null;
    referredById: number | null;
    referredByName: number | null; 
    hasNotification: boolean | null;
    referredByOther: string | null;
    //alertBeforeThat: number | null;
    personNo: number | null; 
    alertMessage: string | null;
    priorityName: string | null; 
}

export class RentalApplicationRegisterRequest implements IRentalApplicationRegisterRequest {
    rentalApplicationId: number | null;
    periodId: number | null;
    propertyTypeId: number | null;
    applicationDate: Date | null;
    fullName: string | null;
    email: string | null;
    housePhone: string | null;
    cellPhone: string | null;
    checkIn: Date | null;
    checkOut: Date | null;
    residenseCountryId: number | null;
    budgetId: number | null;
    comment: string | null;
    rowStatus: boolean | null;
    createdBy: string | null;
    creationDate: Date | null;
    updatedBy: string | null;
    cityOfInterestId: number | null;
    housePartId: number | null;
    personNo: number | null; 
    outInDownId: number | null; 
    referredById: number | null;
    referredByOther: string | null;
    //alertBeforeThat: number | null;
    priorityId: number | null;
    alertDate: Date | null;
    alertMessage: string | null; 


    //features: RentalApplicationFeatureRequest[] | null;
    //cities: RentalApplicationCityRequest[] | null;

    constructor(data?: IRentalApplicationRegisterRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.rentalApplicationId = data["RentalApplicationId"] !== undefined ? data["RentalApplicationId"] : <any>null;
            this.periodId = data["PeriodId"] !== undefined ? data["PeriodId"] : <any>null;
            this.propertyTypeId = data["PropertyTypeId"] !== undefined ? data["PropertyTypeId"] : <any>null;
            this.applicationDate = data["ApplicationDate"] ? new Date(data["ApplicationDate"].toString()) : <any>null;
            this.fullName = data["FullName"] !== undefined ? data["FullName"] : <any>null;
            this.email = data["Email"] !== undefined ? data["Email"] : <any>null;
            this.housePhone = data["HousePhone"] !== undefined ? data["HousePhone"] : <any>null;
            this.cellPhone = data["CellPhone"] !== undefined ? data["CellPhone"] : <any>null;
            this.checkIn = data["CheckIn"] ? new Date(data["CheckIn"].toString()) : <any>null;
            this.checkOut = data["CheckOut"] ? new Date(data["CheckOut"].toString()) : <any>null;
            this.residenseCountryId = data["ResidenseCountryId"] !== undefined ? data["ResidenseCountryId"] : <any>null;
            this.budgetId = data["BudgetId"] !== undefined ? data["BudgetId"] : <any>null;
            this.comment = data["Comment"] !== undefined ? data["Comment"] : <any>null;
            this.rowStatus = data["RowStatus"] !== undefined ? data["RowStatus"] : <any>null;
            this.createdBy = data["CreatedBy"] !== undefined ? data["CreatedBy"] : <any>null;
            this.creationDate = data["CreationDate"] ? new Date(data["CreationDate"].toString()) : <any>null;
            this.updatedBy = data["UpdatedBy"] !== undefined ? data["UpdatedBy"] : <any>null;
            this.cityOfInterestId = data["CityOfInterestId"] !== undefined ? data["CityOfInterestId"] : <any>null;
            this.housePartId = data["HousePartId"] !== undefined ? data["HousePartId"] : <any>null;
            this.personNo = data["PersonNo"] !== undefined ? data["PersonNo"] : <any>null;
            this.outInDownId = data["OutInDownId"] !== undefined ? data["OutInDownId"] : <any>null;
            this.referredById = data["ReferredById"] !== undefined ? data["ReferredById"] : <any>null;
            this.referredByOther = data["ReferredByOther"] !== undefined ? data["ReferredByOther"] : <any>null;
            //this.alertBeforeThat = data["AlertBeforeThat"] !== undefined ? data["AlertBeforeThat"] : <any>null;
            this.priorityId = data["PriorityId"] !== undefined ? data["PriorityId"] : <any>null;
            this.alertDate = data["AlertDate"] ? new Date(data["AlertDate"].toString()) : <any>null;
            this.alertMessage = data["AlertMessage"] !== undefined ? data["AlertMessage"] : <any>null;


            //if (data["Features"] && data["Features"].constructor === Array) {
            //    this.features = [];
            //    for (let item of data["Features"])
            //        this.features.push(RentalApplicationFeatureRequest.fromJS(item));
            //}
            //if (data["Cities"] && data["Cities"].constructor === Array) {
            //    this.cities = [];
            //    for (let item of data["Cities"])
            //        this.cities.push(RentalApplicationCityRequest.fromJS(item));
            //}
        }
    }

    static fromJS(data: any): RentalApplicationRegisterRequest {
        let result = new RentalApplicationRegisterRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["RentalApplicationId"] = this.rentalApplicationId !== undefined ? this.rentalApplicationId : <any>null;
        data["PeriodId"] = this.periodId !== undefined ? this.periodId : <any>null;
        data["PropertyTypeId"] = this.propertyTypeId !== undefined ? this.propertyTypeId : <any>null;
        data["ApplicationDate"] = this.applicationDate ? this.applicationDate.toISOString() : <any>null;
        data["FullName"] = this.fullName !== undefined ? this.fullName : <any>null;
        data["Email"] = this.email !== undefined ? this.email : <any>null;
        data["HousePhone"] = this.housePhone !== undefined ? this.housePhone : <any>null;
        data["CellPhone"] = this.cellPhone !== undefined ? this.cellPhone : <any>null;
        data["CheckIn"] = this.checkIn ? this.checkIn.toISOString() : <any>null;
        data["CheckOut"] = this.checkOut ? this.checkOut.toISOString() : <any>null;
        data["ResidenseCountryId"] = this.residenseCountryId !== undefined ? this.residenseCountryId : <any>null;
        data["BudgetId"] = this.budgetId !== undefined ? this.budgetId : <any>null;
        data["Comment"] = this.comment !== undefined ? this.comment : <any>null;
        data["RowStatus"] = this.rowStatus !== undefined ? this.rowStatus : <any>null;
        data["CreatedBy"] = this.createdBy !== undefined ? this.createdBy : <any>null;
        data["CreationDate"] = this.creationDate ? this.creationDate.toISOString() : <any>null;
        data["UpdatedBy"] = this.updatedBy !== undefined ? this.updatedBy : <any>null;
        data["CityOfInterestId"] = this.cityOfInterestId !== undefined ? this.cityOfInterestId : <any>null;
        data["HousePartId"] = this.housePartId !== undefined ? this.housePartId : <any>null;
        data["PersonNo"] = this.personNo !== undefined ? this.personNo : <any>null;
        data["OutInDownId"] = this.outInDownId !== undefined ? this.outInDownId : <any>null;
        data["ReferredById"] = this.referredById !== undefined ? this.referredById : <any>null;
        data["ReferredByOther"] = this.referredByOther !== undefined ? this.referredByOther : <any>null;
        //data["AlertBeforeThat"] = this.alertBeforeThat !== undefined ? this.alertBeforeThat : <any>null;
        data["PriorityId"] = this.priorityId !== undefined ? this.priorityId : <any>null;
        data["AlertDate"] = this.alertDate ? this.alertDate.toISOString() : <any>null;
        data["AlertMessage"] = this.alertMessage !== undefined ? this.alertMessage : <any>null;

        //if (this.features && this.features.constructor === Array) {
        //    data["Features"] = [];
        //    for (let item of this.features)
        //        data["Features"].push(item.toJSON());
        //}
        //if (this.cities && this.cities.constructor === Array) {
        //    data["Cities"] = [];
        //    for (let item of this.cities)
        //        data["Cities"].push(item.toJSON());
        //}
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new RentalApplicationRegisterRequest();
        result.init(json);
        return result;
    }
}

export interface IRentalApplicationRegisterRequest {
    rentalApplicationId: number | null;
    periodId: number | null;
    propertyTypeId: number | null;
    applicationDate: Date | null;
    fullName: string | null;
    email: string | null;
    housePhone: string | null;
    cellPhone: string | null;
    checkIn: Date | null;
    checkOut: Date | null;
    residenseCountryId: number | null;
    budgetId: number | null;
    comment: string | null;
    rowStatus: boolean | null;
    createdBy: string | null;
    creationDate: Date | null;
    updatedBy: string | null;
    cityOfInterestId: number | null;
    housePartId: number | null;
    personNo: number | null;
    outInDownId: number | null; 
    referredById: number | null;
    referredByOther: string | null;
    //alertBeforeThat: number | null;
    priorityId: number | null;
    alertDate: Date | null;
    alertMessage: string | null; 

    //features: RentalApplicationFeatureRequest[] | null;
    //cities: RentalApplicationCityRequest[] | null;
}

export class RentalApplicationUpdateRequest implements IRentalApplicationUpdateRequest {
    rentalApplicationId: number | null;
    periodId: number | null;
    propertyTypeId: number | null;
    applicationDate: Date | null;
    fullName: string | null;
    email: string | null;
    housePhone: string | null;
    cellPhone: string | null;
    checkIn: Date | null;
    checkOut: Date | null;
    residenseCountryId: number | null;
    budgetId: number | null;
    comment: string | null;
    rowStatus: boolean | null;
    createdBy: string | null;
    creationDate: Date | null;
    updatedBy: string | null;
    cityOfInterestId: number | null;
    housePartId: number | null;
    personNo: number | null;
    outInDownId: number | null; 
    referredById: number | null;
    referredByOther: string | null;
    //alertBeforeThat: number | null;
    priorityId: number | null;
    alertDate: Date | null;
    alertMessage: string | null; 

    constructor(data?: IRentalApplicationUpdateRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.rentalApplicationId = data["RentalApplicationId"] !== undefined ? data["RentalApplicationId"] : <any>null;
            this.periodId = data["PeriodId"] !== undefined ? data["PeriodId"] : <any>null;
            this.propertyTypeId = data["PropertyTypeId"] !== undefined ? data["PropertyTypeId"] : <any>null;
            this.applicationDate = data["ApplicationDate"] ? new Date(data["ApplicationDate"].toString()) : <any>null;
            this.fullName = data["FullName"] !== undefined ? data["FullName"] : <any>null;
            this.email = data["Email"] !== undefined ? data["Email"] : <any>null;
            this.housePhone = data["HousePhone"] !== undefined ? data["HousePhone"] : <any>null;
            this.cellPhone = data["CellPhone"] !== undefined ? data["CellPhone"] : <any>null;
            this.checkIn = data["CheckIn"] ? new Date(data["CheckIn"].toString()) : <any>null;
            this.checkOut = data["CheckOut"] ? new Date(data["CheckOut"].toString()) : <any>null;
            this.residenseCountryId = data["ResidenseCountryId"] !== undefined ? data["ResidenseCountryId"] : <any>null;
            this.budgetId = data["BudgetId"] !== undefined ? data["BudgetId"] : <any>null;
            this.comment = data["Comment"] !== undefined ? data["Comment"] : <any>null;
            this.rowStatus = data["RowStatus"] !== undefined ? data["RowStatus"] : <any>null;
            this.createdBy = data["CreatedBy"] !== undefined ? data["CreatedBy"] : <any>null;
            this.creationDate = data["CreationDate"] ? new Date(data["CreationDate"].toString()) : <any>null;
            this.updatedBy = data["UpdatedBy"] !== undefined ? data["UpdatedBy"] : <any>null;
            this.cityOfInterestId = data["CityOfInterestId"] !== undefined ? data["CityOfInterestId"] : <any>null;
            this.housePartId = data["HousePartId"] !== undefined ? data["HousePartId"] : <any>null;
            this.personNo = data["PersonNo"] !== undefined ? data["PersonNo"] : <any>null;
            this.outInDownId = data["OutInDownId"] !== undefined ? data["OutInDownId"] : <any>null;
            this.referredById = data["ReferredById"] !== undefined ? data["ReferredById"] : <any>null;
            this.referredByOther = data["ReferredByOther"] !== undefined ? data["ReferredByOther"] : <any>null;
            //this.alertBeforeThat = data["AlertBeforeThat"] !== undefined ? data["AlertBeforeThat"] : <any>null;
            this.priorityId = data["PriorityId"] !== undefined ? data["PriorityId"] : <any>null;
            this.alertDate = data["AlertDate"] ? new Date(data["AlertDate"].toString()) : <any>null;
            this.alertMessage = data["AlertMessage"] !== undefined ? data["AlertMessage"] : <any>null;

        }
    }

    static fromJS(data: any): RentalApplicationUpdateRequest {
        let result = new RentalApplicationUpdateRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["RentalApplicationId"] = this.rentalApplicationId !== undefined ? this.rentalApplicationId : <any>null;
        data["PeriodId"] = this.periodId !== undefined ? this.periodId : <any>null;
        data["PropertyTypeId"] = this.propertyTypeId !== undefined ? this.propertyTypeId : <any>null;
        data["ApplicationDate"] = this.applicationDate ? this.applicationDate.toISOString() : <any>null;
        data["FullName"] = this.fullName !== undefined ? this.fullName : <any>null;
        data["Email"] = this.email !== undefined ? this.email : <any>null;
        data["HousePhone"] = this.housePhone !== undefined ? this.housePhone : <any>null;
        data["CellPhone"] = this.cellPhone !== undefined ? this.cellPhone : <any>null;
        data["CheckIn"] = this.checkIn ? this.checkIn.toISOString() : <any>null;
        data["CheckOut"] = this.checkOut ? this.checkOut.toISOString() : <any>null;
        data["ResidenseCountryId"] = this.residenseCountryId !== undefined ? this.residenseCountryId : <any>null;
        data["BudgetId"] = this.budgetId !== undefined ? this.budgetId : <any>null;
        data["Comment"] = this.comment !== undefined ? this.comment : <any>null;
        data["RowStatus"] = this.rowStatus !== undefined ? this.rowStatus : <any>null;
        data["CreatedBy"] = this.createdBy !== undefined ? this.createdBy : <any>null;
        data["CreationDate"] = this.creationDate ? this.creationDate.toISOString() : <any>null;
        data["UpdatedBy"] = this.updatedBy !== undefined ? this.updatedBy : <any>null;
        data["CityOfInterestId"] = this.cityOfInterestId !== undefined ? this.cityOfInterestId : <any>null;
        data["HousePartId"] = this.housePartId !== undefined ? this.housePartId : <any>null;
        data["PersonNo"] = this.personNo !== undefined ? this.personNo : <any>null;
        data["OutInDownId"] = this.outInDownId !== undefined ? this.outInDownId : <any>null;
        data["ReferredById"] = this.referredById !== undefined ? this.referredById : <any>null;
        data["ReferredByOther"] = this.referredByOther !== undefined ? this.referredByOther : <any>null;
        //data["AlertBeforeThat"] = this.alertBeforeThat !== undefined ? this.alertBeforeThat : <any>null;
        data["PriorityId"] = this.priorityId !== undefined ? this.priorityId : <any>null;
        data["AlertDate"] = this.alertDate? this.alertDate.toISOString() : <any>null;
        data["AlertMessage"] = this.alertMessage !== undefined ? this.alertMessage : <any>null;

        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new RentalApplicationUpdateRequest();
        result.init(json);
        return result;
    }
}

export interface IRentalApplicationUpdateRequest {
    rentalApplicationId: number | null;
    periodId: number | null;
    propertyTypeId: number | null;
    applicationDate: Date | null;
    fullName: string | null;
    email: string | null;
    housePhone: string | null;
    cellPhone: string | null;
    checkIn: Date | null;
    checkOut: Date | null;
    residenseCountryId: number | null;
    budgetId: number | null;
    comment: string | null;
    rowStatus: boolean | null;
    createdBy: string | null;
    creationDate: Date | null;
    updatedBy: string | null;
    cityOfInterestId: number | null;
    housePartId: number | null;
    personNo: number | null;
    outInDownId: number | null; 
    referredById: number | null;
    referredByOther: string | null;
    //alertBeforeThat: number | null;
    priorityId: number | null;
    alertDate: Date | null;
    alertMessage: string | null; 

}

export class RentalApplicationDeleteRequest implements IRentalApplicationDeleteRequest {
    rentalApplicationId: number | null;
    userId: number | null;
    username: string | null;

    constructor(data?: IRentalApplicationDeleteRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.rentalApplicationId = data["RentalApplicationId"] !== undefined ? data["RentalApplicationId"] : <any>null;
            this.userId = data["UserId"] !== undefined ? data["UserId"] : <any>null;
            this.username = data["Username"] !== undefined ? data["Username"] : <any>null;
        }
    }

    static fromJS(data: any): RentalApplicationDeleteRequest {
        let result = new RentalApplicationDeleteRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["RentalApplicationId"] = this.rentalApplicationId !== undefined ? this.rentalApplicationId : <any>null;
        data["UserId"] = this.userId !== undefined ? this.userId : <any>null;
        data["Username"] = this.username !== undefined ? this.username : <any>null;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new RentalApplicationDeleteRequest();
        result.init(json);
        return result;
    }
}

export interface IRentalApplicationDeleteRequest {
    rentalApplicationId: number | null;
    userId: number | null;
    username: string | null;
}

export enum PPDetailSearchByContractPeriodDTOTableStatus {
    _0 = 0,
    _1 = 1,
    _2 = 2,
    _3 = 3,
}

export interface IRentalApplicationCityRequest {
    rentalApplicationCityId: number | null;
    rentalApplicationId: number | null;
    cityId: number | null;
    tableStatus: OtherTenantRegisterRequestTableStatus | null;
}


export class ResponseDTOOfRentalApplicationRegisterRequest implements IResponseDTOOfRentalApplicationRegisterRequest {
    data: RentalApplicationRegisterRequest | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;

    constructor(data?: IResponseDTOOfRentalApplicationRegisterRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.data = data["Data"] ? RentalApplicationRegisterRequest.fromJS(data["Data"]) : <any>null;
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

    static fromJS(data: any): ResponseDTOOfRentalApplicationRegisterRequest {
        let result = new ResponseDTOOfRentalApplicationRegisterRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Data"] = this.data ? this.data.toJSON() : <any>null;
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
        let result = new ResponseDTOOfRentalApplicationRegisterRequest();
        result.init(json);
        return result;
    }
}

export interface IResponseDTOOfRentalApplicationRegisterRequest {
    data: RentalApplicationRegisterRequest | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;
}


export class RentalApplicationCityRequest implements IRentalApplicationCityRequest {
    rentalApplicationCityId: number | null;
    rentalApplicationId: number | null;
    cityId: number | null;
    tableStatus: OtherTenantRegisterRequestTableStatus | null;

    constructor(data?: IRentalApplicationCityRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.rentalApplicationCityId = data["RentalApplicationCityId"] !== undefined ? data["RentalApplicationCityId"] : <any>null;
            this.rentalApplicationId = data["RentalApplicationId"] !== undefined ? data["RentalApplicationId"] : <any>null;
            this.cityId = data["CityId"] !== undefined ? data["CityId"] : <any>null;
        }
    }

    static fromJS(data: any): RentalApplicationCityRequest {
        let result = new RentalApplicationCityRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["RentalApplicationCityId"] = this.rentalApplicationCityId !== undefined ? this.rentalApplicationCityId : <any>null;
        data["RentalApplicationId"] = this.rentalApplicationId !== undefined ? this.rentalApplicationId : <any>null;
        data["CityId"] = this.cityId !== undefined ? this.cityId : <any>null;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new RentalApplicationCityRequest();
        result.init(json);
        return result;
    }
}

export interface IRentalApplicationFeatureRequest {
    RentalApplicationFeatureId: number | null;
    rentalApplicationId: number | null;
    featureId: number | null;
    tableStatus: OtherTenantRegisterRequestTableStatus | null;
}

export class RentalApplicationFeatureRequest implements IRentalApplicationFeatureRequest {
    RentalApplicationFeatureId: number | null;
    rentalApplicationId: number | null;
    featureId: number | null;
    tableStatus: OtherTenantRegisterRequestTableStatus | null;

    constructor(data?: IRentalApplicationFeatureRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.RentalApplicationFeatureId = data["RentalApplicationFeatureId"] !== undefined ? data["RentalApplicationFeatureId"] : <any>null;
            this.rentalApplicationId = data["RentalApplicationId"] !== undefined ? data["RentalApplicationId"] : <any>null;
            this.featureId = data["featureId"] !== undefined ? data["featureId"] : <any>null;
        }
    }

    static fromJS(data: any): RentalApplicationFeatureRequest {
        let result = new RentalApplicationFeatureRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["RentalApplicationFeatureId"] = this.RentalApplicationFeatureId !== undefined ? this.RentalApplicationFeatureId : <any>null;
        data["RentalApplicationId"] = this.rentalApplicationId !== undefined ? this.rentalApplicationId : <any>null;
        data["featureId"] = this.featureId !== undefined ? this.featureId : <any>null;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new RentalApplicationFeatureRequest();
        result.init(json);
        return result;
    }
}

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

export enum OtherTenantRegisterRequestTableStatus {
    _0 = 0,
    _1 = 1,
    _2 = 2,
    _3 = 3,
}