import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { routing } from './settlement.routing';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ApproveReportComponent } from './driver-approve-report/approve-report.component';
import { UsersClient } from '../shared/api/services.client';
//import {
//    AmigoTenanttServiceClient, EquipmentTypeClient, EquipmentSizeClient, UsersClient,
//    EquipmentStatusClient, ServiceClient, ProductClient, DriverReportClient, DispatchingPartyClient
//} from '../shared/api/services.client';
import { GridModule } from '@progress/kendo-angular-grid';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { ExportCsv } from '../shared/utils/export-csv';

import { SharedModule } from '../shared/shared.module';

//import { WeeklyReportComponent } from './driver-weekly-report/weekly-report.component';
import { ApproveServiceMaintenanceComponent } from './driver-approve-report/approve-service-maintenance.component';

//import { DriverPayReportComponent} from './driver-pay-report/driver-pay-report.component';

import { SidePanelModule } from '../controls/sidepanel/sidepanel.module';
import { LocationClient } from '../shared/api/services.client';

@NgModule({
    imports: [        
        routing,        
        FormsModule,
        SidePanelModule,
        SharedModule
    ],
    declarations: [
        //WeeklyReportComponent,
        ApproveReportComponent,
        //DriverPayReportComponent,
        ApproveServiceMaintenanceComponent        
    ],
    exports: [
    ],
    providers: [        
        //AmigoTenanttServiceClient,
        //EquipmentTypeClient,
        //EquipmentSizeClient,
        UsersClient,
        ExportCsv,
        //EquipmentStatusClient,
        //ServiceClient,
        //ProductClient,
        //DriverReportClient,
        //DispatchingPartyClient,
        LocationClient
    ],
})
export class SettlementModule { }
