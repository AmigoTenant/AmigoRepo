import { Injectable, Inject, Optional } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { BaseService, API_BASE_URL } from './base.service';
import { OperationResult } from "./operation-result.dto";
import { ResponseDTOOfPagedListOfPPSearchDTO } from "./payment.services.client";

@Injectable()
export class PaymentService extends BaseService {

    constructor( @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(http, baseUrl);
    }

    searchServiceOrder(criteria: PaymentPeriodSearchRequest) {
        const url = this.baseUrl + 'api/payment/search';
        return this.http.post(url, JSON.stringify(criteria), { headers: this.headers })
            .map((serviceOrderData => 
                serviceOrderData as ResponseDTOOfPagedListOfPPSearchDTO
            ))
            .catch(this.handleError);
    }

}

export class PaymentPeriodSearchRequest {
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

export class PPSearchDTO {
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
    depositAmountPending: number | null;
    finesAmountPending: number | null;
    servicesAmountPending: number | null;
    lateFeesAmountPending: number | null;
    dueDate: Date | null;

    constructor(data?: PPSearchDTO) {
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
            this.depositAmountPending = data["DepositAmountPending"] !== undefined ? data["DepositAmountPending"] : <any>null;
            this.finesAmountPending = data["FinesAmountPending"] !== undefined ? data["FinesAmountPending"] : <any>null;
            this.servicesAmountPending = data["ServicesAmountPending"] !== undefined ? data["ServicesAmountPending"] : <any>null;
            this.lateFeesAmountPending = data["LateFeesAmountPending"] !== undefined ? data["LateFeesAmountPending"] : <any>null;
            this.contractId = data["ContractId"] !== undefined ? data["ContractId"] : <any>null;
            this.dueDate = data["DueDate"] ? new Date(data["DueDate"].toString()) : <any>null;

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
        data["DepositAmountPending"] = this.depositAmountPending !== undefined ? this.depositAmountPending : <any>null;
        data["FinesAmountPending"] = this.finesAmountPending !== undefined ? this.finesAmountPending : <any>null;
        data["ServicesAmountPending"] = this.servicesAmountPending !== undefined ? this.servicesAmountPending : <any>null;
        data["LateFeesAmountPending"] = this.lateFeesAmountPending !== undefined ? this.lateFeesAmountPending : <any>null;
        data["ContractId"] = this.contractId !== undefined ? this.contractId : <any>null;
        data["DueDate"] = this.dueDate ? this.dueDate.toISOString() : <any>null;

        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PPSearchDTO();
        result.init(json);
        return result;
    }
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