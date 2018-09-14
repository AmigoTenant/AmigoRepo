import { Component, Output, Input, EventEmitter } from '@angular/core';
import { ValidationService } from '../../shared/validations/validation.service';

@Component({
    selector: 'st-boton-maintenance',
    // templateUrl: './boton.component.html'
    template: `
    <div>
        <input type="button" class="ro-button btn-default" value="Cancel" (click)="onCancel()">
        <input type="button" class="ro-button btn-submit" value="Save" (click)="onSave();">
    </div>
    `
})

export class BotonsComponent {

    constructor(private _validationService: ValidationService) { }

    @Input() formValid: boolean;
    @Input() formName: string;
    @Output() onEventSave = new EventEmitter();

    onSave() {
        if (this.formValid) {
            //  Save
            this.onEventSave.emit("s");
        }
        else {
            //-----------------------------------------------
            //      "touch" input controls
            //-----------------------------------------------
            this._validationService.DisplayFrontEndValidators(this.formName);
        }

    }

    onClear() {
        this.onEventSave.emit("c");
    }
    onCancel() {
        this.onEventSave.emit("k");
    }

}
