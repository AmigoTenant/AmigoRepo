export class ApplicationMessage {
    key: string;
    message: string;

    constructor(data?: any) {
        if (data !== undefined) {
            this.key = data['Key'] !== undefined ? data['Key'] : null;
            this.message = data['Message'] !== undefined ? data['Message'] : null;
        }
    }

    // tslint:disable-next-line:member-ordering
    static fromJS(data: any): ApplicationMessage {
        return new ApplicationMessage(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data['Key'] = this.key !== undefined ? this.key : null;
        data['Message'] = this.message !== undefined ? this.message : null;
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