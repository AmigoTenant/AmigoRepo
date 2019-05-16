import {NgModule} from '@angular/core';
import {AnalyticsComponent} from './analytics.component';
import { analyticsRouting } from './analytics.routing';
import { ChartsModule } from '@progress/kendo-angular-charts';
import { CommonModule } from "@angular/common";
import { DashboardDataService } from '../dasboard-data.service';

import 'hammerjs';
import { MasterDataService } from '../../shared/api/master-data-service';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [    
    CommonModule, analyticsRouting, ChartsModule, FormsModule 
  ],
  declarations: [    
    AnalyticsComponent
  ],
  providers: [
    DashboardDataService,
    MasterDataService
  ],
})
export class AnalyticsModule {
}