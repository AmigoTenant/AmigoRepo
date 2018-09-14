import {Routes, RouterModule} from '@angular/router';
//import { WeeklyReportComponent } from './driver-weekly-report/weekly-report.component';
import { ApproveReportComponent } from './driver-approve-report/approve-report.component';
//import {DriverPayReportComponent} from './driver-pay-report/driver-pay-report.component';
import { LoginRouteGuard } from '../shared/guards/login-route-guard';

export const routes: Routes = [    
    { path: 'driver-report-approval', component: ApproveReportComponent, data: { pageTitle: 'Driver Report Approval' } ,canActivate: [LoginRouteGuard] }
    //{ path: 'driver-weekly-report', component: WeeklyReportComponent, data: { pageTitle: 'Driver Weekly Report' },canActivate: [LoginRouteGuard]  },
    //{ path: 'driver-pay-report', component: DriverPayReportComponent, data: { pageTitle: 'Driver Pay Report' },canActivate: [LoginRouteGuard]  }
];

export const routing = RouterModule.forChild(routes);
