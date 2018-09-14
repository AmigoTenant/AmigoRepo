import {environment} from '../../../environments/environment';
import {Subject} from 'rxjs';

export class LoaderService {        

    private id:number;
    
    constructor() {        
        this.id = new Date().getMilliseconds();
        
    }

    static IsBusy = new Subject<boolean>();

    static startLoading(){
        LoaderService.currentRequest++;
        
        console.log("Start - current request count: " + LoaderService.currentRequest);
        
        setTimeout(()=>{
            if(LoaderService.currentRequest>0){
                console.log("Show loader...");
                LoaderService.IsBusy.next(true);
            }
        },environment.service.Loader.delay);

        setTimeout(()=>{
            if(LoaderService.currentRequest>0){
                console.log("Hide Loader by Timeout...");
                LoaderService.currentRequest = 0;
                LoaderService.IsBusy.next(false);
            }
        },environment.service.Loader.timeout);
    }

    static stopLoading(){
        if(LoaderService.currentRequest > 0)
        LoaderService.currentRequest--;

        console.log("Stop - current request count: " + LoaderService.currentRequest);

        if(LoaderService.currentRequest == 0){
            LoaderService.IsBusy.next(false);
            console.log("Hiding loader...")
        }
    }

    private static currentRequest = 0;
}