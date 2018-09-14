import {LoaderService} from './loader.service';
import {RequestOptionsArgs, Headers, Response} from '@angular/http';

export class AmigoTenantServiceBase {        

    protected transformOptions(options: RequestOptionsArgs): any {
        //console.log(options);
        LoaderService.startLoading();

        let token = localStorage.getItem('authorizationData');
        
        if(token == null)return options;
        
        token = token.substr(1);
        token = token.substr(0, (token.length - 1));
        if (!options) {
            options = {};
        }

        if (!options.headers) {
            options.headers = new Headers();
        }
        options.headers.set("Authorization", "Bearer " + token);
        return options;
    }

    protected transformResult(url: string, response: Response, callback: any): any {        
        //console.log(response);  
        switch (response.status) {
            case 401:               
                localStorage.clear();
                window.location.reload(true);
                break;
            case 403:
                window.location.href = "/#/unauthorized";                                
            case 500:
            case 404:
                console.log(response);
                break;
        }
        LoaderService.stopLoading();
        return callback(response);
    }
}