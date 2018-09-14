import { Injectable } from '@angular/core';
declare var $: any;

@Injectable()
export class BusinessAppSettingService {

    constructor() { }

    public GetDepositPercentage():number {
        return 0.5;
    }

}