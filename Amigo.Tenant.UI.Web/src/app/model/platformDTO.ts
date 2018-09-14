import { Component } from '@angular/core';

export class PlatformDTO {

    platformId: number;
    platformName: string;


    constructor(id: number, name: string) {
        this.platformId = id;
        this.platformName = name;
    }
}