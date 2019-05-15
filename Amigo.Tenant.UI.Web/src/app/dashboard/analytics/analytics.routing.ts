import { Routes, RouterModule } from '@angular/router';
import { AnalyticsComponent } from './analytics.component';
import { LoginRouteGuard } from '../../shared/guards/login-route-guard';

// export const analyticsRouting = RouterModule.forChild([    
//     { path: 'analytics', component: AnalyticsComponent, data: { pageTitle: 'Analytics' } }
// ]);

export const routes : Routes = [
    { path: 'analytics', component: AnalyticsComponent, data: { pageTitle: 'Analitics' }, canActivate: [LoginRouteGuard] }
    
    
];


export const analyticsRouting = RouterModule.forChild(routes);
