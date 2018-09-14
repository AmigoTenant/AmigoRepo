import {OnInit, Directive, ElementRef, Input, Renderer, HostListener} from '@angular/core';

@Directive({
    selector: '[st-blur]',
})

export class BlurForwarder {
    constructor() { }
    @HostListener('focus', ['$event.target'])
    onFocus(target) {
    }
    @HostListener('focusout', ['$event.target'])
    onFocusout(target) {
    }
}