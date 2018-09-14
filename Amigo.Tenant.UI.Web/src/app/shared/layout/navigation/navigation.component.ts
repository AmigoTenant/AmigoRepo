import { Component, OnInit } from '@angular/core';
import { SecurityService } from '../../security/security.service';
import { MainMenuClient, MainMenuDTO, UsersClient } from '../../api/services.client';
import { MenuDTO } from './menuDTO';
import { ClientRouterConfig, ClientRoute } from '../../models/client-route.models';
import { environment } from '../../../../environments/environment';
import { Http, Response, Headers } from '@angular/http';

@Component({
    selector: 'st-navigation',
    templateUrl: 'navigation.component.html'
})
export class NavigationComponent implements OnInit {

    private myModules: MainMenuDTO[];
    private routes: MainMenuDTO[];
    private allRoutes: MainMenuDTO[] = [];
    private parentModules: MainMenuDTO[] = [];
    public allMenus: MenuDTO[] = [];

    public version: string = environment.version;
    public currentEnvironment = environment.deploymentEnvironment;
    public fullUserName: string;

    constructor(private securityService: SecurityService, private mainMenuClient: MainMenuClient, private usersClient: UsersClient, private _http: Http) {
        this.serviceMainMenu();
        this.getLoguedUserInfo();
    }

    ngOnInit() {
    }

    private serviceMainMenu(): void {
        this.mainMenuClient.search(0).subscribe(response => {
            this.myModules = response.data;
            this.processModules(response.data);
            var p = response.toJS();
            var permissions = p.Data;
            localStorage.setItem("permissions",JSON.stringify(permissions));
        });
    };
    public getLoguedUserInfo() {
        var authorizationUrl = environment.authenticationUrl + 'connect/userinfo';
        let token = localStorage.getItem('authorizationData');
        if (token == null) return;
        token = token.substr(1);
        token = token.substr(0, (token.length - 1));

        var headers = new Headers();
        headers.set("Authorization", "Bearer " + token);

        this._http.get(authorizationUrl, { headers: headers }).subscribe(response => {
            let body = response.json();
            var firstName = body.given_name != null ? body.given_name : "";
            var lastName = body.family_name != null ? body.family_name : "";
            this.fullUserName = firstName + " " + lastName;
        });
    }

    private inRouteArray(moduleId: number, routes: MainMenuDTO[], filter: string): boolean {
        let inRoute = false;
        for (let _route in routes) {
            if ((filter === 'module' && routes[_route].moduleId === moduleId) || (filter === 'parent' && routes[_route].parentModuleId === moduleId)) {
                inRoute = true;
                break;
            }
        }
        return inRoute;
    };

    private processModules(routes: MainMenuDTO[]): void {
        this.allRoutes = [];
        routes.forEach(route => {
            let inRoute = this.inRouteArray(route.moduleId, this.allRoutes, 'module');
            if (!inRoute)
                this.allRoutes.push(route);
        });
        this.getParentModules();
    };

    private getParentModules(): void {
        this.parentModules = [];
        this.allRoutes.forEach((route: MainMenuDTO) => {
            if (route.parentModuleId > 0) {
                let inRoute = this.inRouteArray(route.parentModuleId, this.parentModules, 'parent');
                if (!inRoute)
                    this.parentModules.push(route);
            }
        });
        this.getAllMenu();
    };

    private fillMenu(module: MainMenuDTO) {

        let menu = new MenuDTO();
        menu.userId = module.userId;
        menu.roleId = module.roleId;
        menu.permissionId = module.permissionId;
        menu.actionId = module.actionId;
        menu.moduleId = module.moduleId;
        menu.moduleName = module.moduleName;
        menu.parentModuleId = module.parentModuleId;
        menu.parentModuleName = module.parentModuleName;
        menu.url = module.url;
        menu.parentSortOrder = module.parentSortOrder;
        menu.parentModuleCode = module.parentModuleCode;
        menu.sortOrder = module.sortOrder;
        menu.moduleCode = module.moduleCode;
        menu.childMenu = [];

        return menu;
    };

    private getAllMenu(): void {
        debugger;
        this.allMenus = [];
        this.parentModules.forEach(module => {
            let menu = this.fillMenu(module);
            menu.show = true;
            this.allRoutes.forEach((route: MainMenuDTO) => {
                if (route.parentModuleId === module.parentModuleId)
                    menu.childMenu.push(route);
            });
            this.allMenus.push(menu);
        });

        this.allRoutes.forEach((route: MainMenuDTO) => {
            if (route.parentModuleId === 0 && route.url != null && route.url != '') {
                let menuNoParent = this.fillMenu(route);
                menuNoParent.show = false;
                this.allMenus.push(menuNoParent);
            }
        });
    };

    logout() {
        this.securityService.Logoff();
    };

}