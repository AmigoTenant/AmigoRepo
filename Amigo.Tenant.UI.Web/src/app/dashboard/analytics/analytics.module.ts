import {NgModule} from '@angular/core';
import {AnalyticsComponent} from './analytics.component';
import { analyticsRouting } from './analytics.routing';

@NgModule({
  imports: [    
    analyticsRouting
  ],
  declarations: [    
    AnalyticsComponent
  ],
  providers: [],
})
export class AnalyticsModule {
}