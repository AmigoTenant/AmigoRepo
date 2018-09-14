import { Routes, RouterModule } from '@angular/router';

//import { DeviceComponent } from './device/device.component';
import { LocationComponent } from './location/location.component';
import { ModuleComponent } from './module/module.component';
import { RoleComponent } from './role/role.component';
import { AppUserComponent } from './user/app-user/app-user.component';
import { LocationMaintenanceComponent} from './location/location-maintenance.component';
//import { ProductComponent } from './product/product.component';
//import { AppCostCenterComponent} from './cost-center/app-cost-center/app-cost-center.component';
import { LoginRouteGuard } from '../shared/guards/login-route-guard';

export const routes: Routes = [    
    //{ path: 'device', component: DeviceComponent, data: { pageTitle: 'Device' },canActivate: [LoginRouteGuard]},
   
    { path: 'module', component: ModuleComponent, data: { pageTitle: 'Module' },canActivate: [LoginRouteGuard] },
    { path: 'role', component: RoleComponent, data: { pageTitle: 'Role' },canActivate: [LoginRouteGuard] },
    { path: 'user', component: AppUserComponent, data: { pageTitle: 'User' },canActivate: [LoginRouteGuard]},
    
    { path: 'location', component: LocationComponent, data: { pageTitle: 'Location' },canActivate: [LoginRouteGuard]} ,    
    { path: 'location/new', component: LocationMaintenanceComponent , data: { pageTitle: 'House-New' },canActivate: [LoginRouteGuard]},
    { path: 'location/edit/:id', component: LocationMaintenanceComponent, data: { pageTitle: 'House-Edit' },canActivate: [LoginRouteGuard] },  
    //{ path: 'device/getDevice/:userId', component: DeviceComponent, data: { pageTitle: 'Device' }, canActivate: [LoginRouteGuard] }
    //{ path: 'product', component: ProductComponent, data: { pageTitle: 'Product' }, canActivate: [LoginRouteGuard] },
    //{ path: 'cost-center', component: AppCostCenterComponent, data: { pageTitle: 'Cost Center' }, canActivate: [LoginRouteGuard] }
];

export const maintenanceRouting = RouterModule.forChild(routes);