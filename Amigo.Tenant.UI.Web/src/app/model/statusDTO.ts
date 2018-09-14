import { Component } from '@angular/core';

export class StatusDTO {

    statusId: number;
    name: string;


    constructor(id: number, name: string) {
        this.statusId = id;
        this.name = name;
    }
}