import {Component} from '@angular/core';

export class ServiceDTO {

    serviceId: number;
    name: string;
    code: string;
    rowStatus: string;

    constructor(private serviceId1: number, code1: string , name1: string) {
        this.serviceId = serviceId1;
        this.code = code1;
        this.name = name1;
    }

}