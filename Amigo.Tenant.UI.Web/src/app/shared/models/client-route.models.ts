import { Route } from '@angular/router';

export interface ClientRoute extends Route {
    name: string,
    mainNav?: boolean,
    children?: ClientRoute[]
}

export declare type ClientRouterConfig = ClientRoute[];
