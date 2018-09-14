export class BrokenRulesCollection extends Array<BrokenRule> {

    constructor(data?: any) {
        super();
        if (data !== undefined) {
            for (let item of data) {
                this.push(new BrokenRule(item));
            }
        }
    }

    static fromJS(data: any): BrokenRulesCollection {
        return new BrokenRulesCollection(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        return data;
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        const json = this.toJSON();
        return new BrokenRulesCollection(JSON.parse(json));
    }
}

export class BrokenRule {
    ruleName: string;
    description: string;
    property: string;
    severity: RuleSeverity;

    constructor(data?: any) {
        if (data !== undefined) {
            this.ruleName = data["ruleName"] !== undefined ? data["ruleName"] : null;
            this.description = data["description"] !== undefined ? data["description"] : null;
            this.property = data["property"] !== undefined ? data["property"] : null;
            this.severity = data["severity"] !== undefined ? data["severity"] : null;
        }
    }

    static fromJS(data: any): BrokenRule {
        return new BrokenRule(data);
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data["ruleName"] = this.ruleName !== undefined ? this.ruleName : null;
        data["description"] = this.description !== undefined ? this.description : null;
        data["property"] = this.property !== undefined ? this.property : null;
        data["severity"] = this.severity !== undefined ? this.severity : null;
        return data;
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        const json = this.toJSON();
        return new BrokenRule(JSON.parse(json));
    }
}

export enum RuleSeverity {
    Error = 0,
    Warning = 1,
    Information = 2,
    Success = 3,
}