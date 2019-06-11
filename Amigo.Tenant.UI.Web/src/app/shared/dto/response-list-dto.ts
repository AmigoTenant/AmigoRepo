import { ApplicationMessage } from './application-message';
import * as _ from 'lodash';

export class ResponseListDTO {
    data: any[];
    isValid: boolean;
    messages: ApplicationMessage[];
    pk: string | null;
    code: string | null;

    page: number;
    total: number;
    pageSize: number;
    items: any[];

    dat: any;

    constructor(data?: any) {
        if (data !== undefined) {
            if (data['Data'] && data['Data'].constructor === Array) {
                this.data = [];
                for (let item of data['Data']) {
                    let camel = _.mapKeys(item, (v, k) => _.camelCase(k));
                    this.data.push(camel);
                }
            } else {
                let dat = data['Data'];
                if (dat !== undefined && dat['Items'] !== undefined && dat['Items'].constructor === Array) {
                    this.page = dat['Page'] !== undefined ? dat['Page'] : null;
                    this.total = dat['Total'] !== undefined ? dat['Total'] : null;
                    this.pageSize = dat['PageSize'] !== undefined ? dat['PageSize'] : null;
                    this.items = [];
                    for (let item of dat['Items']) {
                        let camel = _.mapKeys(item, (v, k) => _.camelCase(k));
                        this.items.push(camel);
                    }
                } else {
                    if (dat !== undefined && dat.constructor === Object) {
                        let camel = _.mapKeys(dat, (v, k) => _.camelCase(k));
                        this.dat = camel;
                    }
                }
            }

            this.isValid = data['IsValid'] !== undefined ? data['IsValid'] : null;
            if (data['Messages'] && data['Messages'].constructor === Array) {
                this.messages = [];
                for (let item of data['Messages']) {
                    this.messages.push(ApplicationMessage.fromJS(item));
                }
            }
        }
    }
}

export class PagedListOfResponseDTO {
    page: number;
    total: number;
    pageSize: number;
    items: any[];

    constructor(data?: any) {
        if (data !== undefined) {
            this.page = data['Page'] !== undefined ? data['Page'] : null;
            this.total = data['Total'] !== undefined ? data['Total'] : null;
            this.pageSize = data['PageSize'] !== undefined ? data['PageSize'] : null;
            if (data['Items'] && data['Items'].constructor === Array) {
                this.items = [];
                for (let item of data['Items']) {
                    let camel = _.mapKeys(item, (v, k) => _.camelCase(k));
                    this.items.push(camel);
                }
            }
        }
    }
}
