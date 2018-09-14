import {Routes, RouterModule} from '@angular/router';
import {InitialScreenComponent} from './initial-screen.component'

export const routes: Routes = [
    { path: '', redirectTo: 'initial-screen', pathMatch: 'full' },
    { path: 'initial-screen', component: InitialScreenComponent, data: { pageTitle: 'Initital Screen' } }
];

export const routing = RouterModule.forChild(routes);