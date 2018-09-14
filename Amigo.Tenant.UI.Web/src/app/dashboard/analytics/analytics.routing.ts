import { Routes, RouterModule } from '@angular/router';
import { AnalyticsComponent } from './analytics.component';

export const analyticsRouting = RouterModule.forChild([    
    { path: 'analytics', component: AnalyticsComponent, data: { pageTitle: 'Analytics' } }
]);