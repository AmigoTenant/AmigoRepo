import {NgModule} from '@angular/core';
import {AnalyticsComponent} from './analytics.component';
import { analyticsRouting } from './analytics.routing';
import { ChartsModule } from '@progress/kendo-angular-charts';
import { CommonModule } from "@angular/common";
import { DashboardDataService } from '../dasboard-data.service';


@NgModule({
  imports: [    
    CommonModule, analyticsRouting, ChartsModule
  ],
  declarations: [    
    AnalyticsComponent
  ],
  providers: [
    DashboardDataService
  ],
})
export class AnalyticsModule {
}