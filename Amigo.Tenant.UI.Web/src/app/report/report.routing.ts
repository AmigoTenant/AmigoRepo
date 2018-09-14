import {Routes, RouterModule} from '@angular/router';
import {ReportComponent} from './report/report.component'
import { LoginRouteGuard } from '../shared/guards/login-route-guard';
export const routes: Routes = [    
    { path: 'report/:target', component: ReportComponent, data: { pageTitle: 'Report' },canActivate: [LoginRouteGuard] },
];

export const routing = RouterModule.forChild(routes);