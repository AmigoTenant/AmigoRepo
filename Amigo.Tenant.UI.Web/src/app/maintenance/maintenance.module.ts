import { NgModule} from '@angular/core';
import { maintenanceRouting } from './maintenance.routing';
import { ModuleSearchComponent } from './module/module-search.component';
import { ModuleGridComponent } from './module/module-grid.component';
import { UsersClient, ModuleClient, LocationClient, AmigoTenantTRoleClient } from '../shared/api/services.client';
import { ModuleMaintenanceComponent } from './module/module-maintenance.component'


/*User*/
import { AppUserComponent } from './user/app-user/app-user.component'
import { DialogUserComponent } from './user/dialog-user/dialog-user.component'
import { GridUserComponent } from './user/grid-user/grid-user.component'
import { SearchUserComponent } from './user/search-user/search-user.component'
import { SharedModule } from '../shared/shared.module';
import { LocationComponent } from './location/location.component';
import { ModuleComponent } from './module/module.component';
import { RoleComponent } from './role/role.component';
import { LocationMaintenanceComponent } from './location/location-maintenance.component';
import { MarkerMapComponent } from './location/marker-map.component';
//import { AgmCoreModule, LatLngLiteral } from '@agm/core';
import { RoleMaintenanceComponent } from './role/role.maintenance.component';
import { RolPermissionComponent } from './role/role-permission.component';
//import { TreeModule } from 'angular-tree-component';
import { DataService } from './location/dataService';
import { HouseFeatureMaintenanceComponent } from './location/housefeature-maintenance.component';

@NgModule({
    imports: [
        maintenanceRouting,
        SharedModule,
        //TreeModule,
        //AgmCoreModule.forRoot({ apiKey: 'AIzaSyA6r4xrcWWBceY_1RLVo16sM18iCRrGXlc' })
    ],
    declarations: [
        ModuleSearchComponent,
        ModuleMaintenanceComponent,
        ModuleGridComponent,
        LocationMaintenanceComponent,
        MarkerMapComponent,
        LocationComponent,
        ModuleComponent,
        RoleComponent,
        RoleMaintenanceComponent,
        RolPermissionComponent,
        AppUserComponent,
        DialogUserComponent,
        GridUserComponent,
        SearchUserComponent,
        HouseFeatureMaintenanceComponent
    ],
    providers: [
        ModuleClient,
        LocationClient,
        UsersClient,
        AmigoTenantTRoleClient,
        DataService,
    ]
})
export class MaintenanceModule {
}