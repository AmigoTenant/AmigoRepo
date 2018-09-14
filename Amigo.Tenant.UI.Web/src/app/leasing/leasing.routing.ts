import { Routes, RouterModule } from '@angular/router';

import { LoginRouteGuard } from '../shared/guards/login-route-guard';
import { RentalApplicationComponent } from './rentalapplication/rentalapplication.component';
import { RentalApplicationMaintenanceComponent } from './rentalapplication/rentalapplication-maintenance.component';

export const routes: Routes = [
    { path: 'rentalApp', component: RentalApplicationComponent, data: { pageTitle: 'RentalApplication' }, canActivate: [LoginRouteGuard] },
    { path: 'rentalApp/new', component: RentalApplicationMaintenanceComponent, data: { pageTitle: 'RentalApplication-New' }, canActivate: [LoginRouteGuard] },
    { path: 'rentalApp/edit/:rentalApplicationId', component: RentalApplicationMaintenanceComponent, data: { pageTitle: 'RentalApplication-Edit' }, canActivate: [LoginRouteGuard] }
];

export const LeasingRouting = RouterModule.forChild(routes);
