import { MaintenanceModule } from './maintenance/maintenance.module';
import { AnalyticsModule } from './dashboard/analytics/analytics.module';
import { maintenanceRouting } from './maintenance/maintenance.routing';
//import { LeasingRouting } from './leasing/leasing.routing';

import { NgModule, ErrorHandler } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { routing } from './app.routing';
import { SharedModule, API_BASE_URL } from './shared';
import { MainMenuClient } from './shared/api/services.client';

import { InitialScreenModule } from './shared/initial-screen/initial-screen.module';

import { LoginRouteGuard } from './shared/guards/login-route-guard';

import { AppComponent } from './app.component';
import { environment } from '../environments/environment';
//import { RaygunErrorHandler } from './shared/error-handling';
import { SimpleNotificationsModule } from 'angular2-notifications';
//import { AgmCoreModule, MapsAPILoader, NoOpMapsAPILoader } from 'angular2-google-maps/core';
import { AgmCoreModule } from '@agm/core';
import { DashboardRoutingModule } from './dashboard/dashboard.routing';
import { TooltipModule } from "ngx-tooltip";
import { FwModule } from '../fw/fw.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        NgbModule.forRoot(),
        SharedModule.forRoot(),
        AnalyticsModule,
        FwModule,
        routing,
        InitialScreenModule,
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyA6r4xrcWWBceY_1RLVo16sM18iCRrGXlc'
        }),
        SimpleNotificationsModule.forRoot(),
        TooltipModule
    ],
    providers: [
        { provide: API_BASE_URL, useValue: environment.serviceUrl },
        //{ provide: ErrorHandler, useClass: RaygunErrorHandler },
        //{ provide: MapsAPILoader, useClass: NoOpMapsAPILoader},
        LoginRouteGuard,
        MainMenuClient
    ],
    bootstrap: [AppComponent]
})
export class AppModule { } 
