import { BrokenRule } from './broken-rules-collection.dto';
import { BrokenRulesCollection } from './broken-rules-collection.dto';
export class OperationResult<T> {

    isValid: boolean;
    brokenRulesCollection: BrokenRulesCollection;
    data: T;
    count: number;
    constructor(data: T, total: number) {
        this.data = data;
        this.count = total;
    }

    toJS(data?: any) {
        data = data === undefined ? {} : data;
        data['data'] = this.data !== undefined ? this.data : null;
        return data;
    }

    toJSON() {
        return JSON.stringify(this.toJS());
    }

    clone() {
        const json = this.toJSON();
        return new OperationResult(JSON.parse(json), 0);
    }

    addErrorBrokenRule(ruleName: string, description: string) {
        this.addBrokenRule(0, ruleName, description);
    }

    addWarningBrokenRule(ruleName: string, description: string) {
        this.addBrokenRule(1, ruleName, description);
    }

    addInfoBrokenRule(ruleName: string, description: string) {
        this.addBrokenRule(2, ruleName, description);
    }

    addSuccessBrokenRule(ruleName: string, description: string) {
        this.addBrokenRule(3, ruleName, description);
    }

    private addBrokenRule(severity: number, ruleName: string, description: string) {
        this.isValid = false;

        if (this.brokenRulesCollection == null) {
            this.brokenRulesCollection = new BrokenRulesCollection();
        }

        const brokenRule = new BrokenRule();

        brokenRule.severity = severity;
        brokenRule.ruleName = ruleName;
        brokenRule.description = description;

        this.brokenRulesCollection.push(brokenRule);
    }
}
