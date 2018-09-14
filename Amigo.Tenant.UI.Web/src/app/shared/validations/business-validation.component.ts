import {SimpleChanges, Component, Input, OnChanges} from '@angular/core';

@Component({
    selector: 'biz-val',
    templateUrl: './business-validation.component.html'
})
export class BusinessValidationComponent implements OnChanges {
    @Input() errorMessages: any[];
    @Input() successFlag: boolean;
    @Input() successMessage: string;

    messages: any[];

    ngOnChanges(changes: SimpleChanges) {
        // debugger
        if (this.successFlag) {
            this.messages = [{ message: this.successMessage }];
        }
        else {
            this.messages = this.errorMessages;
        }

    }

}

