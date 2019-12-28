import 'rxjs/Rx';
import {Observable} from 'rxjs/Observable';
import {Injectable, Inject, Optional, InjectionToken} from '@angular/core';
import {Http, Headers, Response, RequestOptionsArgs} from '@angular/http';
import {AmigoTenantServiceBase} from './amigotenantservicebase';
import {AmigoTenantOffsetBase} from './amigotenantoffsetbase';
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


export interface IPaymentPeriodClient {
    /**
     * @return OK
     */
    search(search_periodId: number, search_houseId: number, search_contractCode: string, search_paymentPeriodStatusId: number, search_tenantId: number, search_hasPendingFines: boolean, search_hasPendingLateFee: boolean, search_hasPendingDeposit: boolean, search_page: number, search_pageSize: number): Observable<ResponseDTOOfPagedListOfPPSearchDTO | null>;
    /**
     * @return OK
     */
    sendPayNotification(search_periodId: number, search_paymentPeriodStatusId: number, search_tenantId: number, search_hasPendingServices: boolean, search_hasPendingFines: boolean, search_hasPendingLateFee: boolean, search_hasPendingDeposit: boolean, search_page: number, search_pageSize: number): Observable<ResponseDTOOfPagedListOfPPSearchDTO | null>;
    /**
     * @return OK
     */
    searchCriteriaByContract(search_periodId: number, search_contractId: number, search_invoiceId: number, search_page: number, search_pageSize: number): Observable<ResponseDTOOfPPHeaderSearchByContractPeriodDTO | null>;
    /**
     * @return OK
     */
    calculateLateFeeByContractAndPeriod(search_periodId: number, search_contractId: number, search_page: number, search_pageSize: number): Observable<ResponseDTOOfPPDetailSearchByContractPeriodDTO | null>;
    /**
     * @return OK
     */
    update(paymentPeriod: PPHeaderSearchByContractPeriodDTO): Observable<ResponseDTO | null>;

    searchInvoiceById(fileRepositoryId: number);
    /**
     * @return OK
     */
    searchForLiquidation(search_periodId: number, search_contractId: number, search_page: number, search_pageSize: number): Observable<ResponseDTOOfPPHeaderSearchByContractPeriodDTO | null>;
    sendEmailAboutLiquidation(search_periodId: number, search_contractId: number): Observable<ResponseDTOOfPPHeaderSearchByContractPeriodDTO | null>;
}

@Injectable()
export class PaymentPeriodClient extends AmigoTenantServiceBase implements IPaymentPeriodClient {
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
    search(search_periodId: number, search_houseId: number, search_contractCode: string, search_paymentPeriodStatusId: number, search_tenantId: number, search_hasPendingFines: boolean, search_hasPendingLateFee: boolean, search_hasPendingDeposit: boolean, search_page: number, search_pageSize: number): Observable<ResponseDTOOfPagedListOfPPSearchDTO | null> {
        let url_ = this.baseUrl + "/api/payment/searchCriteria?";
        if (search_periodId !== undefined)
            url_ += "search.periodId=" + encodeURIComponent("" + search_periodId) + "&";
        if (search_houseId !== undefined)
            url_ += "search.houseId=" + encodeURIComponent("" + search_houseId) + "&";
        if (search_contractCode !== undefined)
            url_ += "search.contractCode=" + encodeURIComponent("" + search_contractCode) + "&";
        if (search_paymentPeriodStatusId !== undefined)
            url_ += "search.paymentPeriodStatusId=" + encodeURIComponent("" + search_paymentPeriodStatusId) + "&";
        if (search_tenantId !== undefined)
            url_ += "search.tenantId=" + encodeURIComponent("" + search_tenantId) + "&";
        // if (search_hasPendingServices !== undefined)
        //     url_ += "search.hasPendingServices=" + encodeURIComponent("" + search_hasPendingServices) + "&";
        if (search_hasPendingFines !== undefined)
            url_ += "search.hasPendingFines=" + encodeURIComponent("" + search_hasPendingFines) + "&";
        if (search_hasPendingLateFee !== undefined)
            url_ += "search.hasPendingLateFee=" + encodeURIComponent("" + search_hasPendingLateFee) + "&";
        if (search_hasPendingDeposit !== undefined)
            url_ += "search.hasPendingDeposit=" + encodeURIComponent("" + search_hasPendingDeposit) + "&";
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
                    return <Observable<ResponseDTOOfPagedListOfPPSearchDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfPagedListOfPPSearchDTO>><any>Observable.throw(response);
        });
        
    }

    protected processSearch(response: Response): ResponseDTOOfPagedListOfPPSearchDTO | null {
        const status = response.status;

        if (status === 200) {
            const responseText = response.text();
            let result200: ResponseDTOOfPagedListOfPPSearchDTO = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfPagedListOfPPSearchDTO.fromJS(resultData200) : new ResponseDTOOfPagedListOfPPSearchDTO();
            return result200;
        } else if (status !== 200 && status !== 204) {
            const responseText = response.text();
            return this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }


    sendPayNotification(search_periodId: number, search_paymentPeriodStatusId: number, search_tenantId: number, search_hasPendingServices: boolean, search_hasPendingFines: boolean, search_hasPendingLateFee: boolean, search_hasPendingDeposit: boolean, search_page: number, search_pageSize: number): Observable<ResponseDTOOfPagedListOfPPSearchDTO | null> {
        let url_ = this.baseUrl + "/api/payment/sendPaymentNotificationEmail?";
        if (search_periodId !== undefined)
            url_ += "search.periodId=" + encodeURIComponent("" + search_periodId) + "&";
        if (search_paymentPeriodStatusId !== undefined)
            url_ += "search.paymentPeriodStatusId=" + encodeURIComponent("" + search_paymentPeriodStatusId) + "&";
        if (search_tenantId !== undefined)
            url_ += "search.tenantId=" + encodeURIComponent("" + search_tenantId) + "&";
        if (search_hasPendingServices !== undefined)
            url_ += "search.hasPendingServices=" + encodeURIComponent("" + search_hasPendingServices) + "&";
        if (search_hasPendingFines !== undefined)
            url_ += "search.hasPendingFines=" + encodeURIComponent("" + search_hasPendingFines) + "&";
        if (search_hasPendingLateFee !== undefined)
            url_ += "search.hasPendingLateFee=" + encodeURIComponent("" + search_hasPendingLateFee) + "&";
        if (search_hasPendingDeposit !== undefined)
            url_ += "search.hasPendingDeposit=" + encodeURIComponent("" + search_hasPendingDeposit) + "&";
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
            return this.transformResult(url_, response, (response) => this.processSendPayNotification(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processSendPayNotification(response)));
                } catch (e) {
                    return <Observable<ResponseDTOOfPagedListOfPPSearchDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfPagedListOfPPSearchDTO>><any>Observable.throw(response);
        });

    }

    protected processSendPayNotification(response: Response): ResponseDTOOfPagedListOfPPSearchDTO | null {
        const status = response.status;

        if (status === 200) {
            const responseText = response.text();
            let result200: ResponseDTOOfPagedListOfPPSearchDTO = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfPagedListOfPPSearchDTO.fromJS(resultData200) : new ResponseDTOOfPagedListOfPPSearchDTO();
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
    searchCriteriaByContract(search_periodId: number, search_contractId: number,  search_invoiceId: number,  search_page: number, search_pageSize: number): Observable<ResponseDTOOfPPHeaderSearchByContractPeriodDTO | null> {
        let url_ = this.baseUrl + "/api/payment/searchCriteriaByContract?";
        if (search_periodId !== undefined)
            url_ += "search.periodId=" + encodeURIComponent("" + search_periodId) + "&";
        if (search_contractId !== undefined)
            url_ += "search.contractId=" + encodeURIComponent("" + search_contractId) + "&";
        if (search_invoiceId !== undefined)
            url_ += "search.invoiceId=" + encodeURIComponent("" + search_invoiceId) + "&";
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
            return this.transformResult(url_, response, (response) => this.processSearchCriteriaByContract(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processSearchCriteriaByContract(response)));
                } catch (e) {
                    return <Observable<ResponseDTOOfPPHeaderSearchByContractPeriodDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfPPHeaderSearchByContractPeriodDTO>><any>Observable.throw(response);
            });

        
    }

    protected processSearchCriteriaByContract(response: Response): ResponseDTOOfPPHeaderSearchByContractPeriodDTO | null {
        const status = response.status;

        if (status === 200) {
            const responseText = response.text();
            let result200: ResponseDTOOfPPHeaderSearchByContractPeriodDTO | null = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfPPHeaderSearchByContractPeriodDTO.fromJS(resultData200) : new ResponseDTOOfPPHeaderSearchByContractPeriodDTO();
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
    searchForLiquidation(search_periodId: number, search_contractId: number, search_page: number, search_pageSize: number): Observable<ResponseDTOOfPPHeaderSearchByContractPeriodDTO | null> {
        let url_ = this.baseUrl + "/api/payment/searchForLiquidation?";
        if (search_periodId !== undefined)
            url_ += "search.periodId=" + encodeURIComponent("" + search_periodId) + "&";
        if (search_contractId !== undefined)
            url_ += "search.contractId=" + encodeURIComponent("" + search_contractId) + "&";
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
            return this.transformResult(url_, response, (response) => this.processSearchForLiquidation(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processSearchCriteriaByContract(response)));
                } catch (e) {
                    return <Observable<ResponseDTOOfPPHeaderSearchByContractPeriodDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfPPHeaderSearchByContractPeriodDTO>><any>Observable.throw(response);
            });

        
    }

    protected processSearchForLiquidation(response: Response): ResponseDTOOfPPHeaderSearchByContractPeriodDTO | null {
        const status = response.status;

        if (status === 200) {
            const responseText = response.text();
            let result200: ResponseDTOOfPPHeaderSearchByContractPeriodDTO | null = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfPPHeaderSearchByContractPeriodDTO.fromJS(resultData200) : new ResponseDTOOfPPHeaderSearchByContractPeriodDTO();
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
    sendEmailAboutLiquidation(search_periodId: number, search_contractId: number): Observable<ResponseDTOOfPPHeaderSearchByContractPeriodDTO | null> {
        let url_ = this.baseUrl + "/api/payment/sendEmailAboutLiquidation?";
        if (search_periodId !== undefined)
            url_ += "search.periodId=" + encodeURIComponent("" + search_periodId) + "&";
        if (search_contractId !== undefined)
            url_ += "search.contractId=" + encodeURIComponent("" + search_contractId) + "&";
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
            return this.transformResult(url_, response, (response) => this.processSendEmailAboutLiquidation(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processSearchCriteriaByContract(response)));
                } catch (e) {
                    return <Observable<ResponseDTOOfPPHeaderSearchByContractPeriodDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfPPHeaderSearchByContractPeriodDTO>><any>Observable.throw(response);
            });

        
    }

    protected processSendEmailAboutLiquidation(response: Response): ResponseDTOOfPPHeaderSearchByContractPeriodDTO | null {
        const status = response.status;

        if (status === 200) {
            const responseText = response.text();
            let result200: ResponseDTOOfPPHeaderSearchByContractPeriodDTO | null = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfPPHeaderSearchByContractPeriodDTO.fromJS(resultData200) : new ResponseDTOOfPPHeaderSearchByContractPeriodDTO();
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
   searchInvoiceById(fileRepositoryId: number) {
        let url_ = this.baseUrl + "/api/payment/searchCriteriaByInvoice?";
        if (fileRepositoryId === undefined || fileRepositoryId === null)
            throw new Error("The parameter 'fileRepositoryId' must be defined and cannot be null.");
        else
            url_ += "fileRepositoryId=" + encodeURIComponent("" + fileRepositoryId) + "&";
        url_ = url_.replace(/[?&]$/, "");

        window.open(url_);

                
    }

    protected processSearchInvoiceById(response: Response): ResponseDTOOfListOfPPHeaderSearchByInvoiceDTO | null {
        const status = response.status;

        if (status === 200) {
            const responseText = response.text();
            let result200: ResponseDTOOfListOfPPHeaderSearchByInvoiceDTO | null = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfListOfPPHeaderSearchByInvoiceDTO.fromJS(resultData200) : new ResponseDTOOfListOfPPHeaderSearchByInvoiceDTO();
            return result200;
        } else if (status !== 200 && status !== 204) {
            const responseText = response.text();
            return this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }

    calculateLateFeeByContractAndPeriod(search_periodId: number, search_contractId: number, search_page: number, search_pageSize: number): Observable<ResponseDTOOfPPDetailSearchByContractPeriodDTO | null> {
        let url_ = this.baseUrl + "/api/payment/calculateLateFeeByContractAndPeriod?";
        if (search_periodId !== undefined)
            url_ += "search.periodId=" + encodeURIComponent("" + search_periodId) + "&";
        if (search_contractId !== undefined)
            url_ += "search.contractId=" + encodeURIComponent("" + search_contractId) + "&";
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
            return this.transformResult(url_, response, (response) => this.processCalculateLateFeeByContractAndPeriod(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processCalculateLateFeeByContractAndPeriod(response)));
                } catch (e) {
                    return <Observable<ResponseDTOOfPPDetailSearchByContractPeriodDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfPPDetailSearchByContractPeriodDTO>><any>Observable.throw(response);
        });

    }

    protected processCalculateOnAccountByContractAndPeriod(response: Response): ResponseDTOOfPPDetailSearchByContractPeriodDTO | null {
        const status = response.status;

        if (status === 200) {
            const responseText = response.text();
            let result200: ResponseDTOOfPPDetailSearchByContractPeriodDTO | null = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfPPDetailSearchByContractPeriodDTO.fromJS(resultData200) : new ResponseDTOOfPPDetailSearchByContractPeriodDTO();
            return result200;
        } else if (status !== 200 && status !== 204) {
            const responseText = response.text();
            return this.throwException("An unexpected server error occurred.", status, responseText);
        }
        return null;
    }

    calculateOnAccountByContractAndPeriod(search_periodId: number, search_contractId: number, search_page: number, search_pageSize: number): Observable<ResponseDTOOfPPDetailSearchByContractPeriodDTO | null> {
        let url_ = this.baseUrl + "/api/payment/calculateOnAccountByContractAndPeriod?";
        if (search_periodId !== undefined)
            url_ += "search.periodId=" + encodeURIComponent("" + search_periodId) + "&";
        if (search_contractId !== undefined)
            url_ += "search.contractId=" + encodeURIComponent("" + search_contractId) + "&";
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
            return this.transformResult(url_, response, (response) => this.processCalculateLateFeeByContractAndPeriod(response));
        }).catch((response: any, caught: any) => {
            if (response instanceof Response) {
                try {
                    return Observable.of(this.transformResult(url_, response, (response) => this.processCalculateLateFeeByContractAndPeriod(response)));
                } catch (e) {
                    return <Observable<ResponseDTOOfPPDetailSearchByContractPeriodDTO>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResponseDTOOfPPDetailSearchByContractPeriodDTO>><any>Observable.throw(response);
        });

    }

    protected processCalculateLateFeeByContractAndPeriod(response: Response): ResponseDTOOfPPDetailSearchByContractPeriodDTO | null {
        const status = response.status;

        if (status === 200) {
            const responseText = response.text();
            let result200: ResponseDTOOfPPDetailSearchByContractPeriodDTO | null = null;
            let resultData200 = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResponseDTOOfPPDetailSearchByContractPeriodDTO.fromJS(resultData200) : new ResponseDTOOfPPDetailSearchByContractPeriodDTO();
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
    update(paymentPeriod: PPHeaderSearchByContractPeriodDTO): Observable<ResponseDTO | null> {
        let url_ = this.baseUrl + "/api/payment/update";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(paymentPeriod ? paymentPeriod.toJSON() : null);

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
            debugger;
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


export class PaymentPeriodSearchRequest implements IPaymentPeriodSearchRequest {
    periodId: number | null;
    houseId: number | null;
    contractCode: string | null;
    paymentPeriodStatusId: number | null;
    tenantId: number | null;
    hasPendingServices: boolean | null;
    hasPendingFines: boolean | null;
    hasPendingLateFee: boolean | null;
    hasPendingDeposit: boolean | null;
    page: number | null;
    pageSize: number | null;


    constructor(data?: IPaymentPeriodSearchRequest) {
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
            this.houseId = data["HouseId"] !== undefined ? data["HouseId"] : <any>null;
            this.contractCode = data["ContractCode"] !== undefined ? data["ContractCode"] : <any>null;
            this.paymentPeriodStatusId = data["PaymentPeriodStatusId"] !== undefined ? data["PaymentPeriodStatusId"] : <any>null;
            this.tenantId = data["TenantId"] !== undefined ? data["TenantId"] : <any>null;
            this.hasPendingServices = data["hasPendingServices"] !== undefined ? data["hasPendingServices"] : <any>null;
            this.hasPendingFines = data["hasPendingFines"] !== undefined ? data["hasPendingFines"] : <any>null;
            this.hasPendingLateFee = data["hasPendingLateFee"] !== undefined ? data["hasPendingLateFee"] : <any>null;
            this.hasPendingDeposit = data["hasPendingDeposit"] !== undefined ? data["hasPendingDeposit"] : <any>null;
            this.page = data["Page"] !== undefined ? data["Page"] : <any>null;
            this.pageSize = data["PageSize"] !== undefined ? data["PageSize"] : <any>null;
        }
    }

    static fromJS(data: any): PaymentPeriodSearchRequest {
        let result = new PaymentPeriodSearchRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["PeriodId"] = this.periodId !== undefined ? this.periodId : <any>null;
        data["HouseId"] = this.houseId !== undefined ? this.houseId : <any>null;
        data["ContractCode"] = this.contractCode !== undefined ? this.contractCode : <any>null;
        data["PaymentPeriodStatusId"] = this.paymentPeriodStatusId !== undefined ? this.paymentPeriodStatusId : <any>null;
        data["TenantId"] = this.tenantId !== undefined ? this.tenantId : <any>null;
        data["hasPendingServices"] = this.hasPendingServices !== undefined ? this.hasPendingServices : <any>null;
        data["hasPendingFines"] = this.hasPendingFines !== undefined ? this.hasPendingFines : <any>null;
        data["hasPendingLateFee"] = this.hasPendingLateFee !== undefined ? this.hasPendingLateFee : <any>null;
        data["hasPendingDeposit"] = this.hasPendingDeposit !== undefined ? this.hasPendingDeposit : <any>null;
        data["Page"] = this.page !== undefined ? this.page : <any>null;
        data["PageSize"] = this.pageSize !== undefined ? this.pageSize : <any>null;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PaymentPeriodSearchRequest();
        result.init(json);
        return result;
    }
}

export interface IPaymentPeriodSearchRequest {
    periodId: number | null;
    houseId: number | null;
    contractCode: string | null;
    paymentPeriodStatusId: number | null;
    tenantId: number | null;
    hasPendingServices: boolean | null;
    hasPendingFines: boolean | null;
    hasPendingLateFee: boolean | null;
    hasPendingDeposit: boolean | null;
    page: number | null;
    pageSize: number | null;
}

export class ResponseDTOOfPagedListOfPPSearchDTO implements IResponseDTOOfPagedListOfPPSearchDTO {
    data: PagedListOfPPSearchDTO | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;

    constructor(data?: IResponseDTOOfPagedListOfPPSearchDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.data = data["Data"] ? PagedListOfPPSearchDTO.fromJS(data["Data"]) : <any>null;
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

    static fromJS(data: any): ResponseDTOOfPagedListOfPPSearchDTO {
        let result = new ResponseDTOOfPagedListOfPPSearchDTO();
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
        let result = new ResponseDTOOfPagedListOfPPSearchDTO();
        result.init(json);
        return result;
    }
}

export interface IResponseDTOOfPagedListOfPPSearchDTO {
    data: PagedListOfPPSearchDTO | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;
}

export class PagedListOfPPSearchDTO implements IPagedListOfPPSearchDTO {
    page: number | null;
    total: number | null;
    pageSize: number | null;
    items: PPSearchDTO[] | null;

    constructor(data?: IPagedListOfPPSearchDTO) {
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
                    this.items.push(PPSearchDTO.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedListOfPPSearchDTO {
        let result = new PagedListOfPPSearchDTO();
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
        let result = new PagedListOfPPSearchDTO();
        result.init(json);
        return result;
    }
}

export interface IPagedListOfPPSearchDTO {
    page: number | null;
    total: number | null;
    pageSize: number | null;
    items: PPSearchDTO[] | null;
}

export class PPSearchDTO implements IPPSearchDTO {
    isSelected: boolean | null;
    periodCode: string | null;
    contractCode: string | null;
    contractId: number | null;
    tenantFullName: string | null;
    houseName: string | null;
    paymentPeriodStatusId: number | null;
    servicesPending: number | null;
    lateFeesPending: number | null;
    finesPending: number | null;
    depositPending: number | null;
    periodId: number | null;
    tenantId: number | null;
    houseId: number | null;
    paymentPeriodStatusCode: string | null;
    paymentPeriodStatusName: string | null;
    paymentAmount: number | null;
    rentAmountPending: number | null;
    depositAmountPending: number | null;
    finesAmountPending: number | null;
    //servicesAmountPending: number | null;
    lateFeesAmountPending: number | null;
    onAccountAmountPending: number | null;
    dueDate: Date | null;
    totalAmountPending: number | null;
    totalIncomeAmountByPeriod: number | null;
    totalIncomePaidAmount: number | null;
    totalIncomePendingAmount: number | null;
    rentAmountPaid: number | null;
    depositAmountPaid: number | null;
    finesAmountPaid: number | null;
    lateFeesAmountPaid: number | null;
    onAccountAmountPaid: number | null;
    totalAmountPaid: number | null;
    contractStatusCode: string | null;
    invoiceId?: number;
    constructor(data?: IPPSearchDTO) {
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
            this.periodCode = data["PeriodCode"] !== undefined ? data["PeriodCode"] : <any>null;
            this.contractCode = data["ContractCode"] !== undefined ? data["ContractCode"] : <any>null;
            this.tenantFullName = data["TenantFullName"] !== undefined ? data["TenantFullName"] : <any>null;
            this.houseName = data["HouseName"] !== undefined ? data["HouseName"] : <any>null;
            this.paymentPeriodStatusId = data["PaymentPeriodStatusId"] !== undefined ? data["PaymentPeriodStatusId"] : <any>null;
            this.servicesPending = data["ServicesPending"] !== undefined ? data["ServicesPending"] : <any>null;
            this.lateFeesPending = data["LateFeesPending"] !== undefined ? data["LateFeesPending"] : <any>null;
            this.finesPending = data["FinesPending"] !== undefined ? data["FinesPending"] : <any>null;
            this.depositPending = data["DepositPending"] !== undefined ? data["DepositPending"] : <any>null;
            this.periodId = data["PeriodId"] !== undefined ? data["PeriodId"] : <any>null;
            this.tenantId = data["TenantId"] !== undefined ? data["TenantId"] : <any>null;
            this.houseId = data["HouseId"] !== undefined ? data["HouseId"] : <any>null;
            this.paymentPeriodStatusCode = data["PaymentPeriodStatusCode"] !== undefined ? data["PaymentPeriodStatusCode"] : <any>null;
            this.paymentPeriodStatusName = data["PaymentPeriodStatusName"] !== undefined ? data["PaymentPeriodStatusName"] : <any>null;
            this.paymentAmount = data["PaymentAmount"] !== undefined ? data["PaymentAmount"] : <any>null;
            this.rentAmountPending = data["RentAmountPending"] !== undefined ? data["RentAmountPending"] : <any>null;
            this.depositAmountPending = data["DepositAmountPending"] !== undefined ? data["DepositAmountPending"] : <any>null;
            this.finesAmountPending = data["FinesAmountPending"] !== undefined ? data["FinesAmountPending"] : <any>null;
            //this.servicesAmountPending = data["ServicesAmountPending"] !== undefined ? data["ServicesAmountPending"] : <any>null;
            this.lateFeesAmountPending = data["LateFeesAmountPending"] !== undefined ? data["LateFeesAmountPending"] : <any>null;
            this.onAccountAmountPending = data["OnAccountAmountPending"] !== undefined ? data["OnAccountAmountPending"] : <any>null;
            this.contractId = data["ContractId"] !== undefined ? data["ContractId"] : <any>null;
            this.dueDate = data["DueDate"] ? new Date(data["DueDate"].toString()) : <any>null;
            this.totalAmountPending = data["TotalAmountPending"] !== undefined ? data["TotalAmountPending"] : <any>null;
            this.totalIncomeAmountByPeriod = data["TotalIncomeAmountByPeriod"] !== undefined ? data["TotalIncomeAmountByPeriod"] : <any>null;
            this.totalIncomePaidAmount = data["TotalIncomePaidAmount"] !== undefined ? data["TotalIncomePaidAmount"] : <any>null;
            this.totalIncomePendingAmount = data["TotalIncomePendingAmount"] !== undefined ? data["TotalIncomePendingAmount"] : <any>null;
            
            this.rentAmountPaid = data["RentAmountPaid"] !== undefined ? data["RentAmountPaid"] : <any>null;
            this.depositAmountPaid = data["DepositAmountPaid"] !== undefined ? data["DepositAmountPaid"] : <any>null;
            this.finesAmountPaid = data["FinesAmountPaid"] !== undefined ? data["FinesAmountPaid"] : <any>null;
            this.lateFeesAmountPaid = data["LateFeesAmountPaid"] !== undefined ? data["LateFeesAmountPaid"] : <any>null;
            this.onAccountAmountPaid = data["OnAccountAmountPaid"] !== undefined ? data["OnAccountAmountPaid"] : <any>null;
            this.totalAmountPaid = data["TotalAmountPaid"] !== undefined ? data["TotalAmountPaid"] : <any>null;
            
            this.contractStatusCode = data["ContractStatusCode"] !== undefined ? data["ContractStatusCode"] : <any>null;
            this.invoiceId = data["InvoiceId"] !== undefined ? data["InvoiceId"] : <any>null;
        }
    }

    static fromJS(data: any): PPSearchDTO {
        let result = new PPSearchDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["IsSelected"] = this.isSelected !== undefined ? this.isSelected : <any>null;
        data["PeriodCode"] = this.periodCode !== undefined ? this.periodCode : <any>null;
        data["ContractCode"] = this.contractCode !== undefined ? this.contractCode : <any>null;
        data["TenantFullName"] = this.tenantFullName !== undefined ? this.tenantFullName : <any>null;
        data["HouseName"] = this.houseName !== undefined ? this.houseName : <any>null;
        data["PaymentPeriodStatusId"] = this.paymentPeriodStatusId !== undefined ? this.paymentPeriodStatusId : <any>null;
        data["ServicesPending"] = this.servicesPending !== undefined ? this.servicesPending : <any>null;
        data["LateFeesPending"] = this.lateFeesPending !== undefined ? this.lateFeesPending : <any>null;
        data["FinesPending"] = this.finesPending !== undefined ? this.finesPending : <any>null;
        data["DepositPending"] = this.depositPending !== undefined ? this.depositPending : <any>null;
        data["PeriodId"] = this.periodId !== undefined ? this.periodId : <any>null;
        data["TenantId"] = this.tenantId !== undefined ? this.tenantId : <any>null;
        data["HouseId"] = this.houseId !== undefined ? this.houseId : <any>null;
        data["PaymentPeriodStatusCode"] = this.paymentPeriodStatusCode !== undefined ? this.paymentPeriodStatusCode : <any>null;
        data["PaymentPeriodStatusName"] = this.paymentPeriodStatusName !== undefined ? this.paymentPeriodStatusName : <any>null;
        data["PaymentAmount"] = this.paymentAmount !== undefined ? this.paymentAmount : <any>null;
        data["RentAmountPending"] = this.rentAmountPending !== undefined ? this.rentAmountPending : <any>null;
        data["DepositAmountPending"] = this.depositAmountPending !== undefined ? this.depositAmountPending : <any>null;
        data["FinesAmountPending"] = this.finesAmountPending !== undefined ? this.finesAmountPending : <any>null;
        //data["ServicesAmountPending"] = this.servicesAmountPending !== undefined ? this.servicesAmountPending : <any>null;
        data["LateFeesAmountPending"] = this.lateFeesAmountPending !== undefined ? this.lateFeesAmountPending : <any>null;
        data["OnAccountAmountPending"] = this.onAccountAmountPending !== undefined ? this.onAccountAmountPending : <any>null;
        data["ContractId"] = this.contractId !== undefined ? this.contractId : <any>null;
        data["DueDate"] = this.dueDate ? this.dueDate.toISOString() : <any>null;
        data["TotalAmountPending"] = this.totalAmountPending !== undefined ? this.totalAmountPending : <any>null;
        data["TotalIncomeAmountByPeriod"] = this.totalIncomeAmountByPeriod !== undefined ? this.totalIncomeAmountByPeriod : <any>null;
        data["TotalIncomePaidAmount"] = this.totalIncomePaidAmount !== undefined ? this.totalIncomePaidAmount : <any>null;
        data["TotalIncomePendingAmount"] = this.totalIncomePendingAmount !== undefined ? this.totalIncomePendingAmount : <any>null;
        data["RentAmountPaid"] = this.rentAmountPaid !== undefined ? this.rentAmountPaid : <any>null;
        data["DepositAmountPaid"] = this.depositAmountPaid !== undefined ? this.depositAmountPaid : <any>null;
        data["FinesAmountPaid"] = this.finesAmountPaid !== undefined ? this.finesAmountPaid : <any>null;
        data["LateFeesAmountPaid"] = this.lateFeesAmountPaid !== undefined ? this.lateFeesAmountPaid : <any>null;
        data["OnAccountAmountPaid"] = this.onAccountAmountPaid !== undefined ? this.onAccountAmountPaid : <any>null;
        data["TotalAmountPaid"] = this.totalAmountPaid !== undefined ? this.totalAmountPaid : <any>null;
        data["ContractStatusCode"] = this.contractStatusCode !== undefined ? this.contractStatusCode : <any>null;
        data["InvoiceId"] = this.invoiceId !== undefined ? this.invoiceId : <any>null;
        
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PPSearchDTO();
        result.init(json);
        return result;
    }
}

export interface IPPSearchDTO {
    isSelected: boolean | null;
    periodCode: string | null;
    contractCode: string | null;
    contractId: number | null;
    tenantFullName: string | null;
    houseName: string | null;
    paymentPeriodStatusId: number | null;
    servicesPending: number | null;
    lateFeesPending: number | null;
    finesPending: number | null;
    depositPending: number | null;
    periodId: number | null;
    tenantId: number | null;
    houseId: number | null;
    paymentPeriodStatusCode: string | null;
    paymentPeriodStatusName: string | null;
    rentAmountPending: number | null;
    depositAmountPending: number | null;
    finesAmountPending: number | null;
    lateFeesAmountPending: number | null;
    onAccountAmountPending: number | null;
    totalAmountPending: number | null;
    totalIncomeAmountByPeriod: number | null;
    totalIncomePaidAmount: number | null;
    totalIncomePendingAmount: number | null;
    rentAmountPaid: number | null;
    depositAmountPaid: number | null;
    finesAmountPaid: number | null;
    lateFeesAmountPaid: number | null;
    onAccountAmountPaid: number | null;
    totalAmountPaid: number | null;
    contractStatusCode: string | null;
    invoiceId?: number ;
}

export class PaymentPeriodSearchByContractPeriodRequest implements IPaymentPeriodSearchByContractPeriodRequest {
    periodId: number | null;
    contractId: number | null;
    invoiceId: number | null;
    page: number | null;
    pageSize: number | null;

    constructor(data?: IPaymentPeriodSearchByContractPeriodRequest) {
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
            this.contractId = data["ContractId"] !== undefined ? data["ContractId"] : <any>null;
            this.invoiceId = data["InvoiceId"] !== undefined ? data["InvoiceId"] : <any>null;
            this.page = data["Page"] !== undefined ? data["Page"] : <any>null;
            this.pageSize = data["PageSize"] !== undefined ? data["PageSize"] : <any>null;
        }
    }

    static fromJS(data: any): PaymentPeriodSearchByContractPeriodRequest {
        let result = new PaymentPeriodSearchByContractPeriodRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["PeriodId"] = this.periodId !== undefined ? this.periodId : <any>null;
        data["ContractId"] = this.contractId !== undefined ? this.contractId : <any>null;
        data["InvoiceId"] = this.invoiceId !== undefined ? this.invoiceId : <any>null;
        data["Page"] = this.page !== undefined ? this.page : <any>null;
        data["PageSize"] = this.pageSize !== undefined ? this.pageSize : <any>null;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PaymentPeriodSearchByContractPeriodRequest();
        result.init(json);
        return result;
    }
}

export interface IPaymentPeriodSearchByContractPeriodRequest {
    periodId: number | null;
    contractId: number | null;
    invoiceId?: number;
    page: number | null;
    pageSize: number | null;
}

export class ResponseDTOOfPPHeaderSearchByContractPeriodDTO implements IResponseDTOOfPPHeaderSearchByContractPeriodDTO {
    data: PPHeaderSearchByContractPeriodDTO | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;

    constructor(data?: IResponseDTOOfPPHeaderSearchByContractPeriodDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.data = data["Data"] ? PPHeaderSearchByContractPeriodDTO.fromJS(data["Data"]) : <any>null;
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

    static fromJS(data: any): ResponseDTOOfPPHeaderSearchByContractPeriodDTO {
        let result = new ResponseDTOOfPPHeaderSearchByContractPeriodDTO();
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
        let result = new ResponseDTOOfPPHeaderSearchByContractPeriodDTO();
        result.init(json);
        return result;
    }
}

export interface IResponseDTOOfPPHeaderSearchByContractPeriodDTO {
    data: PPHeaderSearchByContractPeriodDTO | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;
}

export class PPHeaderSearchByContractPeriodDTO implements IPPHeaderSearchByContractPeriodDTO {
    isSelected: boolean | null;
    paymentPeriodId: number | null;
    periodId: number | null;
    periodCode: string | null;
    houseName: string | null;
    tenantFullName: string | null;
    pPDetail: PPDetailSearchByContractPeriodDTO[] | null;
    comment: string | null;
    referenceNo: string | null;
    paymentDate: Date | null;
    userId: number | null;
    username: string | null;
    paymentType: number | null;
    pendingDeposit: number | null;
    pendingRent: number | null;
    pendingFine: number | null;
    pendingLateFee: number | null;
    pendingService: number | null;
    pendingOnAccount: number | null;
    pendingTotal: number | null;
    contractId: number | null;
    dueDate: Date | null;

    totalAmount: number | null;
    totalRent: number | null;
    totalDeposit: number | null;
    totalLateFee: number | null;
    totalService: number | null;
    totalFine: number | null;
    totalOnAcount: number | null;

    latestInvoiceId: number | null;
    tenantId: number | null;
    paymentTypeId: number | null;
    isPayInFull: boolean;
    totalInvoice: number | null;
    totalIncome: number | null;
    balance: number | null;
    houseId: number | null;

    lateFeeMissing: PPDetailSearchByContractPeriodDTO;
    email?: string;
    isLiquidating?: boolean;
    contractStatusCode?: string;

    constructor(data?: IPPHeaderSearchByContractPeriodDTO) {
        if (data) {
            for (let property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.isSelected = data["IsSelected"] !== undefined ? data["IsSelected"] : <any>null;
            this.paymentPeriodId = data["PaymentPeriodId"] !== undefined ? data["PaymentPeriodId"] : <any>null;
            this.periodId = data["PeriodId"] !== undefined ? data["PeriodId"] : <any>null;
            this.periodCode = data["PeriodCode"] !== undefined ? data["PeriodCode"] : <any>null;
            this.houseName = data["HouseName"] !== undefined ? data["HouseName"] : <any>null;
            this.tenantFullName = data["TenantFullName"] !== undefined ? data["TenantFullName"] : <any>null;
            if (data["PPDetail"] && data["PPDetail"].constructor === Array) {
                this.pPDetail = [];
                for (let item of data["PPDetail"])
                    this.pPDetail.push(PPDetailSearchByContractPeriodDTO.fromJS(item));
            }
            this.comment = data["Comment"] !== undefined ? data["Comment"] : <any>null;
            this.referenceNo = data["ReferenceNo"] !== undefined ? data["ReferenceNo"] : <any>null;
            this.paymentDate = data["PaymentDate"] ? new Date(data["PaymentDate"].toString()) : <any>null;
            this.userId = data["UserId"] !== undefined ? data["UserId"] : <any>null;
            this.username = data["Username"] !== undefined ? data["Username"] : <any>null;
            this.pendingDeposit = data["PendingDeposit"] !== undefined ? data["PendingDeposit"] : <any>null;
            this.pendingRent = data["PendingRent"] !== undefined ? data["PendingRent"] : <any>null;
            this.pendingFine = data["PendingFine"] !== undefined ? data["PendingFine"] : <any>null;
            this.pendingLateFee = data["PendingLateFee"] !== undefined ? data["PendingLateFee"] : <any>null;
            this.pendingService = data["PendingService"] !== undefined ? data["PendingService"] : <any>null;
            this.pendingOnAccount = data["PendingOnAccount"] !== undefined ? data["PendingOnAccount"] : <any>null;
            this.pendingTotal = data["PendingTotal"] !== undefined ? data["PendingTotal"] : <any>null;
            this.paymentType = data["PaymentType"] !== undefined ? data["PaymentType"] : <any>null;
            this.contractId = data["ContractId"] !== undefined ? data["ContractId"] : <any>null;
            this.dueDate = data["DueDate"] ? new Date(data["DueDate"].toString()) : <any>null;

            this.totalAmount = data["TotalAmount"] !== undefined ? data["TotalAmount"] : <any>null;
            this.totalRent = data["TotalRent"] !== undefined ? data["TotalRent"] : <any>null;
            this.totalDeposit = data["TotalDeposit"] !== undefined ? data["TotalDeposit"] : <any>null;
            this.totalLateFee = data["TotalLateFee"] !== undefined ? data["TotalLateFee"] : <any>null;
            this.totalService = data["TotalService"] !== undefined ? data["TotalService"] : <any>null;
            this.totalFine = data["TotalFine"] !== undefined ? data["TotalFine"] : <any>null;
            this.totalOnAcount = data["TotalOnAcount"] !== undefined ? data["TotalOnAcount"] : <any>null;

            this.latestInvoiceId = data["LatestInvoiceId"] !== undefined ? data["LatestInvoiceId"] : <any>null;
            this.tenantId = data["TenantId"] !== undefined ? data["TenantId"] : <any>null;
            this.paymentTypeId = data["PaymentTypeId"] !== undefined ? data["PaymentTypeId"] : <any>null;
            this.isPayInFull = data["IsPayInFull"] !== undefined ? data["IsPayInFull"] : false;
            
            this.totalIncome = data["TotalIncome"] !== undefined ? data["TotalIncome"] : <any>null;
            this.totalInvoice = data["TotalInvoice"] !== undefined ? data["TotalInvoice"] : <any>null;
            this.balance = data["Balance"] !== undefined ? data["Balance"] : <any>null;

            this.lateFeeMissing = data["LateFeeMissing"] !== undefined && data["LateFeeMissing"] !== null? PPDetailSearchByContractPeriodDTO.fromJS(data["LateFeeMissing"]) : <any>null;
            this.houseId = data["HouseId"] !== undefined ? data["HouseId"] : <any>null;
            this.email =  data["Email"] !== undefined ? data["Email"] : <any>null;
            this.isLiquidating =  data["IsLiquidating"] !== undefined ? data["IsLiquidating"] : <any>null;
            this.contractStatusCode =  data["ContractStatusCode"] !== undefined ? data["ContractStatusCode"] : <any>null;
        }
    }

    static fromJS(data: any): PPHeaderSearchByContractPeriodDTO {
        let result = new PPHeaderSearchByContractPeriodDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["IsSelected"] = this.isSelected !== undefined ? this.isSelected : <any>null;
        data["PaymentPeriodId"] = this.paymentPeriodId !== undefined ? this.paymentPeriodId : <any>null;
        data["PeriodId"] = this.periodId !== undefined ? this.periodId : <any>null;
        data["PeriodCode"] = this.periodCode !== undefined ? this.periodCode : <any>null;
        data["HouseName"] = this.houseName !== undefined ? this.houseName : <any>null;
        data["TenantFullName"] = this.tenantFullName !== undefined ? this.tenantFullName : <any>null;
        if (this.pPDetail && this.pPDetail.constructor === Array) {
            data["PPDetail"] = [];
            for (let item of this.pPDetail)
                data["PPDetail"].push(item.toJSON());
        }
        data["Comment"] = this.comment !== undefined ? this.comment : <any>null;
        data["ReferenceNo"] = this.referenceNo !== undefined ? this.referenceNo : <any>null;
        data["PaymentDate"] = this.paymentDate ? this.paymentDate.toISOString() : <any>null;
        data["UserId"] = this.userId !== undefined ? this.userId : <any>null;
        data["Username"] = this.username !== undefined ? this.username : <any>null;
        data["PendingDeposit"] = this.pendingDeposit !== undefined ? this.pendingDeposit : <any>null;
        data["PendingRent"] = this.pendingRent !== undefined ? this.pendingRent : <any>null;
        data["PendingFine"] = this.pendingFine !== undefined ? this.pendingFine : <any>null;
        data["PendingLateFee"] = this.pendingLateFee !== undefined ? this.pendingLateFee : <any>null;
        data["PendingService"] = this.pendingService !== undefined ? this.pendingService : <any>null;
        data["PendingOnAccount"] = this.pendingOnAccount !== undefined ? this.pendingOnAccount : <any>null;
        data["PendingTotal"] = this.pendingTotal !== undefined ? this.pendingTotal : <any>null;
        data["PaymentType"] = this.paymentType !== undefined ? this.paymentType : <any>null;
        data["ContractId"] = this.contractId !== undefined ? this.contractId : <any>null;
        data["DueDate"] = this.dueDate ? this.dueDate.toISOString() : <any>null;
        data["TotalAmount"] = this.totalAmount !== undefined ? this.totalAmount : <any>null;
        data["TotalRent"] = this.totalRent !== undefined ? this.totalRent : <any>null;
        data["TotalDeposit"] = this.totalDeposit !== undefined ? this.totalDeposit : <any>null;
        data["TotalLateFee"] = this.totalLateFee !== undefined ? this.totalLateFee : <any>null;
        data["TotalService"] = this.totalService !== undefined ? this.totalService : <any>null;
        data["TotalFine"] = this.totalFine !== undefined ? this.totalFine : <any>null;
        data["TotalOnAcount"] = this.totalOnAcount !== undefined ? this.totalOnAcount : <any>null;
        data["LatestInvoiceId"] = this.latestInvoiceId !== undefined ? this.latestInvoiceId : <any>null;
        data["TenantId"] = this.tenantId !== undefined ? this.tenantId : <any>null;
        data["PaymentTypeId"] = this.paymentTypeId !== undefined ? this.paymentTypeId : <any>null;
        data["IsPayInFull"] = this.isPayInFull !== undefined ? this.isPayInFull : false;
        data["TotalIncome"] = this.totalIncome !== undefined ? this.totalIncome : <any>null;
        data["TotalInvoice"] = this.totalInvoice !== undefined ? this.totalInvoice : <any>null;
        data["Balance"] = this.balance !== undefined ? this.balance : <any>null;
        data["LateFeeMissing"] = this.lateFeeMissing !== undefined ? this.lateFeeMissing : <any>null;
        data["HouseId"] = this.houseId !== undefined ? this.houseId : <any>null;
        data["Email"] = this.email !== undefined ? this.email : <any>null;
        data["IsLiquidating"] = this.isLiquidating !== undefined ? this.isLiquidating : <any>null;
        data["ContractStatusCode"] = this.contractStatusCode !== undefined ? this.contractStatusCode : <any>null;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PPHeaderSearchByContractPeriodDTO();
        result.init(json);
        return result;
    }
}

export interface IPPHeaderSearchByContractPeriodDTO {
    isSelected: boolean | null;
    paymentPeriodId: number | null;
    periodId: number | null;
    periodCode: string | null;
    houseName: string | null;
    tenantFullName: string | null;
    pPDetail: PPDetailSearchByContractPeriodDTO[] | null;
    comment: string | null;
    referenceNo: string | null;
    paymentDate: Date | null;
    userId: number | null;
    username: string | null;
    pendingDeposit: number | null;
    pendingRent: number | null;
    pendingFine: number | null;
    pendingLateFee: number | null;
    pendingService: number | null;
    pendingOnAccount: number | null;
    paymentType: number | null;
    contractId: number;
    tenantId: number | null;
    paymentTypeId: number | null;
    isPayInFull: boolean;
    totalInvoice: number | null;
    totalIncome: number | null;
    balance: number | null;
    lateFeeMissing: PPDetailSearchByContractPeriodDTO | null;
    isLiquidating?: boolean;
    contractStatusCode?: string;
}

export class ResponseDTOOfPPDetailSearchByContractPeriodDTO implements IResponseDTOOfPPDetailSearchByContractPeriodDTO {
    data: PPDetailSearchByContractPeriodDTO | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;

    constructor(data?: IResponseDTOOfPPDetailSearchByContractPeriodDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.data = data["Data"] ? PPDetailSearchByContractPeriodDTO.fromJS(data["Data"]) : <any>null;
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

    static fromJS(data: any): ResponseDTOOfPPDetailSearchByContractPeriodDTO {
        let result = new ResponseDTOOfPPDetailSearchByContractPeriodDTO();
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
        let result = new ResponseDTOOfPPDetailSearchByContractPeriodDTO();
        result.init(json);
        return result;
    }
}

export interface IResponseDTOOfPPDetailSearchByContractPeriodDTO {
    data: PPDetailSearchByContractPeriodDTO | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;
}

export class PPDetailSearchByContractPeriodDTO implements IPPDetailSearchByContractPeriodDTO {
    isSelected: boolean | null;
    paymentPeriodId: number | null;
    paymentTypeValue: string | null;
    paymentDescription: string | null;
    paymentAmount: number | null;
    paymentPeriodStatusName: string | null;
    paymentTypeName: string | null;
    conceptId: number | null;
    contractId: number | null;
    tenantId: number | null;
    paymentTypeId: string | null;
    dueDate: Date | null;
    comment: string | null;
    paymentDate: Date | null;
    reference: string | null;
    paymentPeriodStatusId: number | null;
    createdBy: number | null;
    creationDate: Date | null;
    updatedBy: number | null;
    updatedDate: Date | null;
    tableStatus: PPDetailSearchByContractPeriodDTOTableStatus | null;
    periodId: number | null;
    userId: number | null;
    username: string | null;
    paymentPeriodStatusCode: string | null;
    isRequired: boolean | null;
    paymentTypeCode: string | null; 
    invoiceNo: string | null;
    invoiceDate: Date | null;
    fileRepositoryId: number | null;
    houseId: number | null;
    periodCode?: string;
    constructor(data?: IPPDetailSearchByContractPeriodDTO) {
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
            this.paymentPeriodId = data["PaymentPeriodId"] !== undefined ? data["PaymentPeriodId"] : <any>null;
            this.paymentTypeValue = data["PaymentTypeValue"] !== undefined ? data["PaymentTypeValue"] : <any>null;
            this.paymentDescription = data["PaymentDescription"] !== undefined ? data["PaymentDescription"] : <any>null;
            this.paymentAmount = data["PaymentAmount"] !== undefined ? data["PaymentAmount"] : <any>null;
            this.paymentPeriodStatusName = data["PaymentPeriodStatusName"] !== undefined ? data["PaymentPeriodStatusName"] : <any>null;
            this.paymentTypeName = data["PaymentTypeName"] !== undefined ? data["PaymentTypeName"] : <any>null;
            this.conceptId = data["ConceptId"] !== undefined ? data["ConceptId"] : <any>null;
            this.contractId = data["ContractId"] !== undefined ? data["ContractId"] : <any>null;
            this.tenantId = data["TenantId"] !== undefined ? data["TenantId"] : <any>null;
            this.paymentTypeId = data["PaymentTypeId"] !== undefined ? data["PaymentTypeId"] : <any>null;
            this.dueDate = data["DueDate"] ? new Date(data["DueDate"].toString()) : <any>null;
            this.comment = data["Comment"] !== undefined ? data["Comment"] : <any>null;
            this.paymentDate = data["PaymentDate"] ? new Date(data["PaymentDate"].toString()) : <any>null;
            this.reference = data["Reference"] !== undefined ? data["Reference"] : <any>null;
            this.paymentPeriodStatusId = data["PaymentPeriodStatusId"] !== undefined ? data["PaymentPeriodStatusId"] : <any>null;
            this.createdBy = data["CreatedBy"] !== undefined ? data["CreatedBy"] : <any>null;
            this.creationDate = data["CreationDate"] ? new Date(data["CreationDate"].toString()) : <any>null;
            this.updatedBy = data["UpdatedBy"] !== undefined ? data["UpdatedBy"] : <any>null;
            this.updatedDate = data["UpdatedDate"] ? new Date(data["UpdatedDate"].toString()) : <any>null;
            this.tableStatus = data["TableStatus"] !== undefined ? data["TableStatus"] : <any>null;
            this.periodId = data["PeriodId"] !== undefined ? data["PeriodId"] : <any>null;
            this.userId = data["UserId"] !== undefined ? data["UserId"] : <any>null;
            this.username = data["Username"] !== undefined ? data["Username"] : <any>null;
            this.paymentPeriodStatusCode = data["PaymentPeriodStatusCode"] !== undefined ? data["PaymentPeriodStatusCode"] : <any>null;
            this.isRequired = data["IsRequired"] !== undefined ? data["IsRequired"] : <any>null;
            this.paymentTypeCode = data["PaymentTypeCode"] !== undefined ? data["PaymentTypeCode"] : <any>null;
            this.invoiceNo = data["InvoiceNo"] !== undefined ? data["InvoiceNo"] : <any>null;
            this.invoiceDate = data["InvoiceDate"] ? new Date(data["InvoiceDate"].toString()) : <any>null;
            this.fileRepositoryId = data["FileRepositoryId"] !== undefined ? data["FileRepositoryId"] : <any>null;
            this.periodCode = data["PeriodCode"] !== undefined ? data["PeriodCode"] : <any>null;
            
            this.houseId = data["HouseId"] !== undefined ? data["HouseId"] : <any>null;
        }
    }

    static fromJS(data: any): PPDetailSearchByContractPeriodDTO {
        let result = new PPDetailSearchByContractPeriodDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["IsSelected"] = this.isSelected !== undefined ? this.isSelected : <any>null;
        data["PaymentPeriodId"] = this.paymentPeriodId !== undefined ? this.paymentPeriodId : <any>null;
        data["PaymentTypeValue"] = this.paymentTypeValue !== undefined ? this.paymentTypeValue : <any>null;
        data["PaymentDescription"] = this.paymentDescription !== undefined ? this.paymentDescription : <any>null;
        data["PaymentAmount"] = this.paymentAmount !== undefined ? this.paymentAmount : <any>null;
        data["PaymentPeriodStatusName"] = this.paymentPeriodStatusName !== undefined ? this.paymentPeriodStatusName : <any>null;
        data["PaymentTypeName"] = this.paymentTypeName !== undefined ? this.paymentTypeName : <any>null;
        data["ConceptId"] = this.conceptId !== undefined ? this.conceptId : <any>null;
        data["ContractId"] = this.contractId !== undefined ? this.contractId : <any>null;
        data["TenantId"] = this.tenantId !== undefined ? this.tenantId : <any>null;
        data["PaymentTypeId"] = this.paymentTypeId !== undefined ? this.paymentTypeId : <any>null;
        data["DueDate"] = this.dueDate ? this.dueDate.toISOString() : <any>null;
        data["Comment"] = this.comment !== undefined ? this.comment : <any>null;
        data["PaymentDate"] = this.paymentDate ? this.paymentDate.toISOString() : <any>null;
        data["Reference"] = this.reference !== undefined ? this.reference : <any>null;
        data["PaymentPeriodStatusId"] = this.paymentPeriodStatusId !== undefined ? this.paymentPeriodStatusId : <any>null;
        data["CreatedBy"] = this.createdBy !== undefined ? this.createdBy : <any>null;
        data["CreationDate"] = this.creationDate ? this.creationDate.toISOString() : <any>null;
        data["UpdatedBy"] = this.updatedBy !== undefined ? this.updatedBy : <any>null;
        data["UpdatedDate"] = this.updatedDate ? this.updatedDate.toISOString() : <any>null;
        data["TableStatus"] = this.tableStatus !== undefined ? this.tableStatus : <any>null;
        data["PeriodId"] = this.periodId !== undefined ? this.periodId : <any>null;
        data["UserId"] = this.userId !== undefined ? this.userId : <any>null;
        data["Username"] = this.username !== undefined ? this.username : <any>null;
        data["PaymentPeriodStatusCode"] = this.paymentPeriodStatusCode !== undefined ? this.paymentPeriodStatusCode : <any>null;
        data["IsRequired"] = this.isRequired !== undefined ? this.isRequired : <any>null;
        data["PaymentTypeCode"] = this.paymentTypeCode !== undefined ? this.paymentTypeCode : <any>null;
        data["InvoiceNo"] = this.invoiceNo !== undefined ? this.invoiceNo : <any>null;
        data["InvoiceDate"] = this.invoiceDate ? this.invoiceDate.toISOString() : <any>null;
        data["FileRepositoryId"] = this.fileRepositoryId !== undefined ? this.fileRepositoryId : <any>null;
        data["HouseId"] = this.houseId !== undefined ? this.houseId : <any>null;
        data["PeriodCode"] = this.periodCode !== undefined ? this.periodCode : <any>null;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PPDetailSearchByContractPeriodDTO();
        result.init(json);
        return result;
    }
}

export interface IPPDetailSearchByContractPeriodDTO {
    isSelected: boolean | null;
    paymentPeriodId: number | null;
    paymentTypeValue: string | null;
    paymentDescription: string | null;
    paymentAmount: number | null;
    paymentPeriodStatusName: string | null;
    paymentTypeName: string | null;
    conceptId: number | null;
    contractId: number | null;
    tenantId: number | null;
    paymentTypeId: string | null;
    dueDate: Date | null;
    comment: string | null;
    paymentDate: Date | null;
    reference: string | null;
    paymentPeriodStatusId: number | null;
    createdBy: number | null;
    creationDate: Date | null;
    updatedBy: number | null;
    updatedDate: Date | null;
    tableStatus: PPDetailSearchByContractPeriodDTOTableStatus | null;
    periodId: number | null;
    userId: number | null;
    username: string | null;
    paymentPeriodStatusCode: string | null;
    isRequired: boolean | null; 
    paymentTypeCode: string | null; 
    invoiceNo: string | null;
    invoiceDate: Date | null;
    periodCode?: string;
}

export class ResponseDTOOfListOfPPHeaderSearchByInvoiceDTO implements IResponseDTOOfListOfPPHeaderSearchByInvoiceDTO {
    data: PPHeaderSearchByInvoiceDTO[] | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;

    constructor(data?: IResponseDTOOfListOfPPHeaderSearchByInvoiceDTO) {
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
                    this.data.push(PPHeaderSearchByInvoiceDTO.fromJS(item));
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

    static fromJS(data: any): ResponseDTOOfListOfPPHeaderSearchByInvoiceDTO {
        let result = new ResponseDTOOfListOfPPHeaderSearchByInvoiceDTO();
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
        let result = new ResponseDTOOfListOfPPHeaderSearchByInvoiceDTO();
        result.init(json);
        return result;
    }
}

export interface IResponseDTOOfListOfPPHeaderSearchByInvoiceDTO {
    data: PPHeaderSearchByInvoiceDTO[] | null;
    isValid: boolean | null;
    pk: number | null;
    code: string | null;
    messages: ApplicationMessage[] | null;
}

export class PPHeaderSearchByInvoiceDTO implements IPPHeaderSearchByInvoiceDTO {
    invoiceNo: string | null;
    invoiceId: number | null;
    periodCode: string | null;
    houseName: string | null;
    tenantFullName: string | null;
    invoiceDate: Date | null;
    customerName: string | null;
    paymentOperationNo: string | null;
    bankName: string | null;
    paymentOperationDate: Date | null;
    comment: string | null;
    contractCode: string | null;
    totalRent: number | null;
    totalDeposit: number | null;
    totalLateFee: number | null;
    totalService: number | null;
    totalFine: number | null;
    totalOnAcount: number | null;
    totalAmount: number | null;
    contractId: number | null;
    paymentPeriodId: number | null;
    paymentTypeValue: string | null;
    paymentTypeCode: string | null;
    paymentDescription: string | null;
    paymentAmount: number | null;
    periodDueDate: Date | null;
    paymentTypeSequence: number | null;

    constructor(data?: IPPHeaderSearchByInvoiceDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.invoiceNo = data["InvoiceNo"] !== undefined ? data["InvoiceNo"] : <any>null;
            this.invoiceId = data["InvoiceId"] !== undefined ? data["InvoiceId"] : <any>null;
            this.periodCode = data["PeriodCode"] !== undefined ? data["PeriodCode"] : <any>null;
            this.houseName = data["HouseName"] !== undefined ? data["HouseName"] : <any>null;
            this.tenantFullName = data["TenantFullName"] !== undefined ? data["TenantFullName"] : <any>null;
            this.invoiceDate = data["InvoiceDate"] ? new Date(data["InvoiceDate"].toString()) : <any>null;
            this.customerName = data["CustomerName"] !== undefined ? data["CustomerName"] : <any>null;
            this.paymentOperationNo = data["PaymentOperationNo"] !== undefined ? data["PaymentOperationNo"] : <any>null;
            this.bankName = data["BankName"] !== undefined ? data["BankName"] : <any>null;
            this.paymentOperationDate = data["PaymentOperationDate"] ? new Date(data["PaymentOperationDate"].toString()) : <any>null;
            this.comment = data["Comment"] !== undefined ? data["Comment"] : <any>null;
            this.contractCode = data["ContractCode"] !== undefined ? data["ContractCode"] : <any>null;
            this.totalRent = data["TotalRent"] !== undefined ? data["TotalRent"] : <any>null;
            this.totalDeposit = data["TotalDeposit"] !== undefined ? data["TotalDeposit"] : <any>null;
            this.totalLateFee = data["TotalLateFee"] !== undefined ? data["TotalLateFee"] : <any>null;
            this.totalService = data["TotalService"] !== undefined ? data["TotalService"] : <any>null;
            this.totalFine = data["TotalFine"] !== undefined ? data["TotalFine"] : <any>null;
            this.totalOnAcount = data["TotalOnAcount"] !== undefined ? data["TotalOnAcount"] : <any>null;
            this.totalAmount = data["TotalAmount"] !== undefined ? data["TotalAmount"] : <any>null;
            this.contractId = data["ContractId"] !== undefined ? data["ContractId"] : <any>null;
            this.paymentPeriodId = data["PaymentPeriodId"] !== undefined ? data["PaymentPeriodId"] : <any>null;
            this.paymentTypeValue = data["PaymentTypeValue"] !== undefined ? data["PaymentTypeValue"] : <any>null;
            this.paymentTypeCode = data["PaymentTypeCode"] !== undefined ? data["PaymentTypeCode"] : <any>null;
            this.paymentDescription = data["PaymentDescription"] !== undefined ? data["PaymentDescription"] : <any>null;
            this.paymentAmount = data["PaymentAmount"] !== undefined ? data["PaymentAmount"] : <any>null;
            this.periodDueDate = data["PeriodDueDate"] ? new Date(data["PeriodDueDate"].toString()) : <any>null;
            this.paymentTypeSequence = data["PaymentTypeSequence"] !== undefined ? data["PaymentTypeSequence"] : <any>null;
        }
    }

    static fromJS(data: any): PPHeaderSearchByInvoiceDTO {
        let result = new PPHeaderSearchByInvoiceDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["InvoiceNo"] = this.invoiceNo !== undefined ? this.invoiceNo : <any>null;
        data["InvoiceId"] = this.invoiceId !== undefined ? this.invoiceId : <any>null;
        data["PeriodCode"] = this.periodCode !== undefined ? this.periodCode : <any>null;
        data["HouseName"] = this.houseName !== undefined ? this.houseName : <any>null;
        data["TenantFullName"] = this.tenantFullName !== undefined ? this.tenantFullName : <any>null;
        data["InvoiceDate"] = this.invoiceDate ? this.invoiceDate.toISOString() : <any>null;
        data["CustomerName"] = this.customerName !== undefined ? this.customerName : <any>null;
        data["PaymentOperationNo"] = this.paymentOperationNo !== undefined ? this.paymentOperationNo : <any>null;
        data["BankName"] = this.bankName !== undefined ? this.bankName : <any>null;
        data["PaymentOperationDate"] = this.paymentOperationDate ? this.paymentOperationDate.toISOString() : <any>null;
        data["Comment"] = this.comment !== undefined ? this.comment : <any>null;
        data["ContractCode"] = this.contractCode !== undefined ? this.contractCode : <any>null;
        data["TotalRent"] = this.totalRent !== undefined ? this.totalRent : <any>null;
        data["TotalDeposit"] = this.totalDeposit !== undefined ? this.totalDeposit : <any>null;
        data["TotalLateFee"] = this.totalLateFee !== undefined ? this.totalLateFee : <any>null;
        data["TotalService"] = this.totalService !== undefined ? this.totalService : <any>null;
        data["TotalFine"] = this.totalFine !== undefined ? this.totalFine : <any>null;
        data["TotalOnAcount"] = this.totalOnAcount !== undefined ? this.totalOnAcount : <any>null;
        data["TotalAmount"] = this.totalAmount !== undefined ? this.totalAmount : <any>null;
        data["ContractId"] = this.contractId !== undefined ? this.contractId : <any>null;
        data["PaymentPeriodId"] = this.paymentPeriodId !== undefined ? this.paymentPeriodId : <any>null;
        data["PaymentTypeValue"] = this.paymentTypeValue !== undefined ? this.paymentTypeValue : <any>null;
        data["PaymentTypeCode"] = this.paymentTypeCode !== undefined ? this.paymentTypeCode : <any>null;
        data["PaymentDescription"] = this.paymentDescription !== undefined ? this.paymentDescription : <any>null;
        data["PaymentAmount"] = this.paymentAmount !== undefined ? this.paymentAmount : <any>null;
        data["PeriodDueDate"] = this.periodDueDate ? this.periodDueDate.toISOString() : <any>null;
        data["PaymentTypeSequence"] = this.paymentTypeSequence !== undefined ? this.paymentTypeSequence : <any>null;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PPHeaderSearchByInvoiceDTO();
        result.init(json);
        return result;
    }
}

export interface IPPHeaderSearchByInvoiceDTO {
    invoiceNo: string | null;
    invoiceId: number | null;
    periodCode: string | null;
    houseName: string | null;
    tenantFullName: string | null;
    invoiceDate: Date | null;
    customerName: string | null;
    paymentOperationNo: string | null;
    bankName: string | null;
    paymentOperationDate: Date | null;
    comment: string | null;
    contractCode: string | null;
    totalRent: number | null;
    totalDeposit: number | null;
    totalLateFee: number | null;
    totalService: number | null;
    totalFine: number | null;
    totalOnAcount: number | null;
    totalAmount: number | null;
    contractId: number | null;
    paymentPeriodId: number | null;
    paymentTypeValue: string | null;
    paymentTypeCode: string | null;
    paymentDescription: string | null;
    paymentAmount: number | null;
    periodDueDate: Date | null;
    paymentTypeSequence: number | null;
}

export enum PPDetailSearchByContractPeriodDTOTableStatus {
    _0 = 0,
    _1 = 1,
    _2 = 2,
    _3 = 3,
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

