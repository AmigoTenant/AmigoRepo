import { Component, OnInit } from '@angular/core';
import { SecurityService } from './shared/security/security.service';
import { FrameworkConfigService, FrameworkConfigSettings } from '../fw/services/framework-config.service';
import { MenuService } from '../fw/services/menu.service';
import { initialMenuItems } from './app.menu';
import { TranslateService } from '@ngx-translate/core';
import { environment } from '../environments/environment';

@Component({
    selector: 'app-root',
    //template: '<div class="ro-modern xpo-dashboard"><router-outlet></router-outlet></div><simple-notifications></simple-notifications>'
    templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {    

    constructor(private securityService: SecurityService,
            private frameworkConfigService: FrameworkConfigService,
            menuService: MenuService,
            private translate: TranslateService) {
        let config: FrameworkConfigSettings = {
            socialIcons: [
              // { imageFile: 'assets/social-fb-bw.png', alt: 'Facebook', link: 'http://www.facebook.com'},
              // { imageFile: 'assets/social-google-bw.png', alt: 'Google +', link: 'http://www.google.com' },
              // { imageFile: 'assets/social-twitter-bw.png', alt: 'Twitter', link: 'http://www.twitter.com' }
            ],
            showLanguageSelector: true,
            showUserControls: true,
            showStatusBar: true,
            showStatusBarBreakpoint: 800
          };
        frameworkConfigService.configure(config);
        menuService.items = initialMenuItems;
        this.translate.setDefaultLang(environment.defaultLang);
        this.translate.use(environment.lang);
    }

    ngOnInit() {
        if (window.location.hash &&
            window.location.hash.indexOf('id_token=') > -1 &&
            window.location.hash.indexOf('token=') > -1) {
            this.securityService.AuthorizedCallback();
        }
    }

    public Login() {
        console.log("Do login logic");
        this.securityService.Authorize();
    }
}
