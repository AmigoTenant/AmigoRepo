declare var $:any;
import {OnDestroy, OnInit,  Directive,   ElementRef,   Input} from '@angular/core';

@Directive({
    selector: '[outside-click]' 
})
export class OutsideClickDirective implements OnInit,OnDestroy
{        
    @Input("outside-click") targetElement: any;

    constructor(private el: ElementRef) {     
        console.log(el);  
    }
    
    private onmouseupHandler = (e):void =>
    {        
        if(this.el == null)return;                 
         if (!
         (
             e.target.className == "ro-calendar-btn"||
             (e.target.nodeName as string).toLowerCase() == "input" ||
             e.target.className.startsWith("ngb") || 
             (e.target.parentElement.nodeName as string).toLowerCase().startsWith("ngb") ||
             e.target.parentElement.className.startsWith("ngb")
         ))
         {         
             (this.targetElement as any).close();
         }else{
             e.stopPropagation();
         }
    }

    ngOnInit(): void 
    {
        document.body.addEventListener('click',this.onmouseupHandler);        
    }

    ngOnDestroy(): void 
    {
        document.body.removeEventListener('click',this.onmouseupHandler,false);
    }
}