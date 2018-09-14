import { NgModule } from '@angular/core';
import { InitialScreenComponent } from './initial-screen.component';
import { routing } from './initial-screen.routing';

@NgModule({
  imports: [
    routing
  ],
  declarations: [
    InitialScreenComponent
  ],
  providers: [],
})

export class InitialScreenModule {

}