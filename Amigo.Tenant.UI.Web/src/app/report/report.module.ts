import { NgModule} from '@angular/core';
import { routing} from './report.routing';
import { ReportComponent} from './report/report.component';
import { SharedModule } from '../shared/shared.module';
//import { ReportClient, ProductClient, EquipmentStatusClient } from '../shared/api/services.client';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { NgbModule} from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [
      routing,      
      SharedModule,
      DialogModule
  ],
  declarations: [
      ReportComponent
  ]//,
  //providers: [ReportClient,
  //    EquipmentStatusClient, ProductClient]
})
export class ReportModule {

}