import { Routes, RouterModule } from '@angular/router';
import { MainLayoutComponent } from './shared/layout/app-layouts/main-layout.component';
import { LoginRouteGuard } from './shared/guards/login-route-guard';
import { InitialScreenComponent } from './shared/initial-screen/initial-screen.component';
import { LoginPostbackComponent } from './shared/login-postback/login-postback.component';
import { NotFoundComponent } from './shared/layout/error-pages/not-found.component';
import { UnauthorizedComponent } from './shared/layout/error-pages/unauthorized.component';

export const routes: Routes = [    
    { path: 'id_token', component: LoginPostbackComponent, pathMatch: 'full' },
    { path: 'login', component: InitialScreenComponent, data: { pageTitle: 'Initial Screen' } },    
    {
        path: '', component: MainLayoutComponent, data: { pageTitle: 'Home' }, children: [
            { path: '', redirectTo: 'dashboard', pathMatch: "full"},
            { path: 'dashboard', loadChildren: './dashboard/analytics/analytics.module#AnalyticsModule', data: { pageTitle: 'Dashboard' }, canActivate: [LoginRouteGuard], canActivateChild: [LoginRouteGuard] },
            { path: 'maintenance', loadChildren: './maintenance/maintenance.module#MaintenanceModule', data: { pageTitle: 'Maintenances', canActivate: [LoginRouteGuard], canActivateChild: [LoginRouteGuard] } },
            { path: 'report', loadChildren: './report/report.module#ReportModule', data: { pageTitle: 'Report' }, canActivate: [LoginRouteGuard], canActivateChild: [LoginRouteGuard] },
            //{ path: 'shipment-tracking', loadChildren: './shipment-tracking/shipment-tracking.module#ShipmentTrackingModule', data: { pageTitle: 'Shipment Tracking' }, canActivate: [LoginRouteGuard], canActivateChild: [LoginRouteGuard] },
            { path: 'settlement', loadChildren: './settlement/settlement.module#SettlementModule', data: { pageTitle: 'Settlement' }, canActivate: [LoginRouteGuard], canActivateChild: [LoginRouteGuard] },
            { path: 'amigotenant', loadChildren: './amigotenant/amigotenant.module#AmigotenantModule', data: { pageTitle: 'AmigoTenant' }, canActivate: [LoginRouteGuard], canActivateChild: [LoginRouteGuard] },
            { path: 'leasing', loadChildren: './leasing/leasing.module#LeasingModule', data: { pageTitle: 'Leasing' }, canActivate: [LoginRouteGuard], canActivateChild: [LoginRouteGuard] },
            { path: 'notfound', component: NotFoundComponent, canActivate: [LoginRouteGuard], canActivateChild: [LoginRouteGuard] },
            { path: 'unauthorized', component: UnauthorizedComponent },
            { path: '**', redirectTo: 'notfound', pathMatch: 'full' },
        ]
    }    
];

export const routing = RouterModule.forRoot(routes, { useHash: true }); 