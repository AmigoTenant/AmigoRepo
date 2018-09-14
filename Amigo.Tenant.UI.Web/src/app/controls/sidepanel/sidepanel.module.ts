import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from "@angular/forms";
import { SidePanelComponent } from './sidepanel.component';

const SIDEPANEL_COMPONENTS = [SidePanelComponent];

@NgModule({
    imports: [CommonModule, FormsModule],
    declarations: SIDEPANEL_COMPONENTS,
    exports: SIDEPANEL_COMPONENTS

})
export class SidePanelModule { }