import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";

import {MainLayoutComponent } from './app-layouts/main-layout.component';
import {SpinnerComponent} from './spinner/spinner-component';
import {FooterComponent} from "./footer/footer.component";
import {HeaderComponent} from "./header/header.component";
import {NavigationComponent} from "./navigation/navigation.component";
import {NotFoundComponent} from "./error-pages/not-found.component";
import {UnauthorizedComponent} from "./error-pages/unauthorized.component";

import {HeaderModule} from "./header/header.module";
import {NavigationModule} from "./navigation/navigation.module";
import {BusyService} from './spinner/busy-service';

import {RouterModule} from "@angular/router";
import {LoginPostbackComponent } from '../login-postback/login-postback.component';

@NgModule({
  imports: [
    CommonModule,
    HeaderModule,
    NavigationModule,
    FormsModule,
    RouterModule
  ],
  declarations: [    
    FooterComponent,
    SpinnerComponent,
    MainLayoutComponent,
    LoginPostbackComponent,
    UnauthorizedComponent,
    NotFoundComponent
  ],
  exports:[
    HeaderModule,
    NavigationModule,    
    FooterComponent,
    NotFoundComponent,
    UnauthorizedComponent
  ],
  providers:[
    BusyService
  ]
})
export class AmigoTenantLayoutModule{

}