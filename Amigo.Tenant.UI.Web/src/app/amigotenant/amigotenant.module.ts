import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AmigotenantRouting } from './amigotenant.routing';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { GridModule } from '@progress/kendo-angular-grid';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../shared/shared.module';

import { ContractClient, EntityStatusClient, HouseClient, FeatureClient, TenantClient, GeneralTableClient, CountryClient, LocationClient, PeriodClient } from '../shared/api/services.client';
import { SidePanelModule } from '../controls/sidepanel/sidepanel.module';

import { ContractComponent } from './contract/contract.component';
import { ContractMaintenanceComponent } from './contract/contract-maintenance.component';
import { ContractHouseFeatureComponent } from './contract/contract-house-feature.component';

import { TenantComponent } from './tenant/tenant.component';
import { TenantSearchComponent } from './tenant/tenant-search.component';
import { TenantMaintenanceComponent } from './tenant/tenant-maintenance.component';

import { ConfirmationList, Confirmation } from  '../model/confirmation.dto';

import { HouseComponent } from './house/house.component';
import { HouseMaintenanceComponent } from './house/house-maintenance.component';
import { HouseFeatureMaintenanceComponent } from './house/housefeature-maintenance.component';
import { HouseMapComponent } from './house/house-map.component';
import { HouseMapMaintenanceComponent } from './house/house-map-maintenance.component';

import { PaymentComponent } from './payment/payment.component';
import { PaymentPeriodClient } from '../shared/api/payment.services.client';

import { AgmCoreModule } from '@agm/core';

import { DataService } from './house/dataService';
import { HouseServiceComponent } from './house/house-service.component';
import { PaymentMaintenanceComponent } from './payment/payment-maintenance.component';

import { HouseServiceClient } from '../shared/api/amigotenant.service';
import { UtilityBillComponent } from '../amigotenant/utilitybill/utilitybill.component';
import { EditService } from './house/edit.service';
import { NativeWindowService } from '../shared/service/native-window.service';
import { NotificationService } from '../shared/service/notification.service';

import { DataTableModule, DropdownModule, DialogModule as PrimeDialogModule } from 'primeng/primeng';

import { PaymentMaintenanceDetailComponent } from './payment/payment-maintenance-detail.component';
import { PaymentMaintenanceReportComponent } from './payment/payment-maintenance-report.component';
import { PaymentService } from "../shared/api/payment.service";
import { UtilityBillMaintenanceComponent } from './utilitybill/utilitybill-maintenance.component';
import { UtilityBillServiceClient } from '../shared/api/utilityBill.services.client';
import { ExpenseComponent } from './expense/expense.component';
import { MasterDataService } from '../shared/api/master-data-service';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { ExpenseMaintenanceComponent } from './expense/expense-maintenance.component';




@NgModule({
  imports: [
    CommonModule,
    AmigotenantRouting,
    FormsModule,
    SidePanelModule,
    SharedModule,
    MultiselectDropdownModule,
    AgmCoreModule.forRoot({ apiKey: 'AIzaSyA6r4xrcWWBceY_1RLVo16sM18iCRrGXlc' }),
    GridModule,
    DropdownModule,
    DataTableModule,
    PrimeDialogModule
  ],
  declarations: [
    ContractComponent,
    TenantComponent,
    TenantSearchComponent,
    TenantMaintenanceComponent,
    ContractMaintenanceComponent,
    ContractHouseFeatureComponent,
    HouseMaintenanceComponent,
    HouseComponent,
    HouseFeatureMaintenanceComponent,
    HouseMapComponent,
    HouseMapMaintenanceComponent,
    HouseServiceComponent,
    PaymentComponent,
    PaymentMaintenanceComponent,
    PaymentMaintenanceDetailComponent,
    UtilityBillComponent,
    UtilityBillMaintenanceComponent,
    PaymentMaintenanceReportComponent,
    ExpenseComponent,
    ExpenseMaintenanceComponent
  ],
  providers: [
      TenantClient,
      ContractClient,
      EntityStatusClient,
      HouseClient,
      FeatureClient,
      ConfirmationList,
      Confirmation,
      GeneralTableClient,
      CountryClient,
      PeriodClient,
      LocationClient,
      //{ provide: MapsAPILoader, useClass: NoOpMapsAPILoader },
      DataService,
      PaymentPeriodClient,
      HouseServiceClient,
      EditService,
      NativeWindowService,
      NotificationService,
      PaymentService,
      UtilityBillServiceClient,
      MasterDataService
    ],
})
export class AmigotenantModule { }



