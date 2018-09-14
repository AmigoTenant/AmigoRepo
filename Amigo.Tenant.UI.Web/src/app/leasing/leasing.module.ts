import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeasingRouting } from './leasing.routing';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { GridModule } from '@progress/kendo-angular-grid';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../shared/shared.module';

import { EntityStatusClient, GeneralTableClient, CountryClient, PeriodClient, FeatureClient } from '../shared/api/services.client';

import { RentalApplicationComponent } from './rentalapplication/rentalapplication.component';
import { RentalApplicationMaintenanceComponent } from './rentalapplication/rentalapplication-maintenance.component';

import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { ConfirmationList, Confirmation } from '../model/confirmation.dto';

import { RentalApplicationClient } from '../shared/api/rentalapplication.services.client';
import { NotificationService } from '../shared/service/notification.service';

//import { DataService } from './house/dataService';

@NgModule({
    imports: [
        CommonModule,
        LeasingRouting,
        FormsModule,
        SharedModule,
        MultiselectDropdownModule
    ],
    declarations: [
        RentalApplicationComponent,
        RentalApplicationMaintenanceComponent
    ],
    providers: [
        EntityStatusClient,
        ConfirmationList,
        Confirmation,
        GeneralTableClient,
        CountryClient,
        PeriodClient,
        RentalApplicationClient,
        FeatureClient,
        NotificationService
    ],
})
export class LeasingModule { }



