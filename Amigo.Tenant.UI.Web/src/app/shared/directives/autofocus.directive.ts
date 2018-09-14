import {Directive, OnInit, AfterViewInit, ElementRef, DoCheck, Input} from '@angular/core';

@Directive({ selector: '[st-autofocus]' })
export class Autofocus implements OnInit, AfterViewInit {
    private lastVisible: boolean = false;
    private initialised: boolean = false;
    constructor(private el: ElementRef) { }

    ngAfterViewInit() {
        this.initialised = true;
        this.doCheck();
    }

    ngOnInit()
    {
        this.doCheck();
    }

    doCheck() {
        if (!this.initialised) { return; }
        const visible = !!this.el.nativeElement.offsetParent;
        if (visible && !this.lastVisible) {
            if (this.isFocused)
            setTimeout(() => { this.el.nativeElement.focus(); }, 1);
        }
        this.lastVisible = visible;
    }

    @Input() isFocused: boolean= true;

}