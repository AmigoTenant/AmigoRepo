import { ApplicationMessage } from './application-message';
import * as _ from 'lodash';

export class ResponseListDTO {
    data: any[];
    isValid: boolean;
    messages: ApplicationMessage[];
    pk: string | null;
    code: string | null;

    constructor(data?: any) {
        if (data !== undefined) {
            if (data['Data'] && data['Data'].constructor === Array) {
                this.data = [];
                for (let item of data['Data']) {
                    let camel = _.mapKeys(item, (v, k) => _.camelCase(k));
                    this.data.push(camel);
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
