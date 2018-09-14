import {NgModule, ModuleWithProviders, ErrorHandler} from "@angular/core";
import {CommonModule} from '@angular/common';
import {FormsModule,ReactiveFormsModule} from '@angular/forms';
import {RouterModule} from '@angular/router';

import {AmigoTenantLayoutModule} from './layout';
import {GeocodingService} from './api';
import {SecurityService} from './security';
//import {RaygunErrorHandler} from './error-handling';
import {NgbDateParserFormatter, NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { DialogModule } from '@progress/kendo-angular-dialog';

import { BusinessValidationComponent } from './validations/business-validation.component';
import { ValidationService } from './validations/validation.service';
import { ListsService, ConstantsEnvironments } from './constants/lists.service';
import { NgbdTypeaheadUserNew } from './typeahead-usernew/typeahead-usernew';
//import { NgbdTypeaheadLocationNew } from './typeahead-locationnew/typeahead-locationnew';
//import { NgbdTypeaheadProductNew } from './typeahead-productnew/typeahead-productnew';
//import { NgbdTypeaheadCostCenterNew } from './typeahead-costcenternew/typeahead-costcenternew';
import { NgbdTypeaheadTenant } from './typeahead-tenant/typeahead-tenant';
import { NgbdTypeaheadHouse } from './typeahead-house/typeahead-house';
import { NgbdTypeaheadPeriod } from './typeahead-period/typeahead-period';

import { BusyService } from '../shared/layout/spinner/busy-service';
import { HttpModule, Http, XHRBackend, RequestOptions, BaseRequestOptions, Headers } from '@angular/http';
import { SimpleNotificationsModule} from 'angular2-notifications';

import { TitleCasePipe } from './pipes/title-case.pipe';
import { DateFormatPipe } from './pipes/date-format.pipe';
import { TimeFormatPipe } from './pipes/time-format.pipe';
import { SearchPipe } from './pipes/search.pipe';
import { AuthCheckDirective } from './security/auth-check.directive';
import { BlurForwarder } from './directives/blur-forwarder.directive';
import { Autofocus } from './directives/autofocus.directive';
import { Audit } from './common/audit.component';
import { EnvironmentComponent} from './common/environment.component';
import { NgbDateMomentParserFormatter} from './common/NgbDateMomentParserFormatter';
import { OutsideClickDirective} from './common/outsideclick.directive';
import { GridModule } from '@progress/kendo-angular-grid';
//import { UsersClient, ServiceClient, LocationClient } from './api/services.client';
import { UsersClient, LocationClient } from './api/services.client';

import { ExportCsv } from './utils/export-csv';
import { ListService } from './security/shared/list.service';
//import { NgbdTypeaheadProduct } from '../controls/typeahead-product';
import { BotonCrudComponent } from './common/controls/boton-crud.component';
//import { NgbdTypeaheadUser } from './typeahead-user/typeahead-user';
//import { NgbdTypeaheadLocation } from './typeahead-location/typeahead-location';
import { TooltipModule} from "ngx-tooltip";
import { BotonsComponent } from '../controls/common/boton.component';
import { TenantClient, HouseClient, PeriodClient } from './api/services.client';
import { NotificationService } from "./service/notification.service";
import { RentalApplicationClient } from "./api/rentalapplication.services.client";
import { PaymentService } from "./api/payment.service";
import { HttpClientModule, HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { API_BASE_URL } from "./api/base.service";
import { BusinessAppSettingService } from "./constants/business-app-setting.service";

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpModule,
        RouterModule,
        NgbModule,
        SimpleNotificationsModule,        
        GridModule,
        DialogModule,
        TooltipModule,
        HttpClientModule
    ],
    declarations: [
        BusinessValidationComponent,
        NgbdTypeaheadUserNew,
        NgbdTypeaheadTenant,
        NgbdTypeaheadHouse,
        NgbdTypeaheadPeriod,
        TitleCasePipe,
        DateFormatPipe,
        TimeFormatPipe,
        SearchPipe,
        AuthCheckDirective,
        BlurForwarder,
        Autofocus,
        Audit,
        OutsideClickDirective,
        BotonCrudComponent,
        BotonsComponent
    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        BusinessValidationComponent,
        AmigoTenantLayoutModule,
        NgbdTypeaheadUserNew,      
        BotonCrudComponent,
        NgbdTypeaheadTenant,
        NgbdTypeaheadHouse,
        NgbdTypeaheadPeriod,
        TitleCasePipe,
        DateFormatPipe,
        TimeFormatPipe,
        SearchPipe,
        SimpleNotificationsModule,
        AuthCheckDirective,
        BlurForwarder,
        OutsideClickDirective,
        NgbModule,
        TooltipModule,
        GridModule,
        Autofocus,
        Audit,
        DialogModule,
        BotonsComponent
    ],
    providers: [
        BusyService,        
        SecurityService,
        ValidationService,
        UsersClient,        
        ConstantsEnvironments,        
        ExportCsv,
        LocationClient,       
        GeocodingService,
        ListsService,
        TenantClient,
        HouseClient,
        PeriodClient,
        {provide:NgbDateParserFormatter,useClass:NgbDateMomentParserFormatter},
        NotificationService,
        RentalApplicationClient,
        PaymentService,
        { provide: API_BASE_URL, useValue: environment.serviceUrl },
        BusinessAppSettingService
    ]
})
export class SharedModule {

    static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedModule,
            providers: [
                BusyService
            ]
        };
    }
}
