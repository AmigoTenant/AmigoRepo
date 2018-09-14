import { Routes, RouterModule } from '@angular/router';

import { LoginRouteGuard } from '../shared/guards/login-route-guard';
import { ContractComponent } from './contract/contract.component';
import { ContractMaintenanceComponent } from './contract/contract-maintenance.component';
import { TenantComponent } from './tenant/tenant.component';

import { HouseComponent } from './house/house.component';
import { HouseMaintenanceComponent} from './house/house-maintenance.component';

import { PaymentComponent } from './payment/payment.component';
import { PaymentMaintenanceComponent } from './payment/payment-maintenance.component';
import { UtilityBillComponent } from './utilitybill/utilitybill.component';
import { UtilityBillMaintenanceComponent } from './utilitybill/utilitybill-maintenance.component';
import { ExpenseComponent } from './expense/expense.component';


export const routes: Routes = [
  { path: 'contract', component: ContractComponent, data: { pageTitle: 'Contract' }, canActivate: [LoginRouteGuard] },
  { path: 'contract/new', component: ContractMaintenanceComponent, data: { pageTitle: 'Contract-New' }, canActivate: [LoginRouteGuard] },
  { path: 'contract/edit/:code', component: ContractMaintenanceComponent, data: { pageTitle: 'Contract-Edit' }, canActivate: [LoginRouteGuard] },  
  { path: 'tenant', component: TenantComponent, data: { pageTitle: 'Tenant' }, canActivate: [LoginRouteGuard]},
  { path: 'house', component: HouseComponent, data: { pageTitle: 'House' }, canActivate: [LoginRouteGuard]} ,    
  { path: 'house/new', component: HouseMaintenanceComponent , data: { pageTitle: 'House-New' },canActivate: [LoginRouteGuard]},
  { path: 'house/edit/:id', component: HouseMaintenanceComponent, data: { pageTitle: 'House-Edit' }, canActivate: [LoginRouteGuard] },  
  { path: 'payment', component: PaymentComponent, data: { pageTitle: 'Payment' }, canActivate: [LoginRouteGuard] },    
  { path: 'payment/edit/:contractId/:periodId', component: PaymentMaintenanceComponent, data: { pageTitle: 'Massive Payment Detail' }, canActivate: [LoginRouteGuard] },  
  { path: 'utilitybill', component: UtilityBillComponent, data: { pageTitle: 'Utility Bills'}, canActivate : [LoginRouteGuard] },  
  { path: 'utilitybill/edit/:houseId', component: UtilityBillMaintenanceComponent, data: { pageTitle: 'Register Utility Bills' }, canActivate: [LoginRouteGuard] },
  { path: 'expense', component: PaymentComponent, data: { pageTitle: 'Payment' }, canActivate: [LoginRouteGuard] },
  { path: 'expense/edit/:expenseId', component: ExpenseComponent, data: { pageTitle: 'Expense' }, canActivate: [LoginRouteGuard] }

];

export const AmigotenantRouting = RouterModule.forChild(routes);
