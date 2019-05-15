import { Routes, RouterModule } from '@angular/router';

export const DashboardRoutingModule = RouterModule.forChild([
    { path: 'analytics', loadChildren: './analytics/analytics.module#AnalyticsModule'}    
]);

