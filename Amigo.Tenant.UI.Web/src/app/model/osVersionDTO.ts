import { Component } from '@angular/core';

export class OSVersionDTO {

    platformId: number;
    platformName: string;
    oSVersionId: number;
    versionNumber: string;
    versionName: string;

    constructor(oSVersionId: number, versionName: string) {
        this.oSVersionId = oSVersionId;
        this.versionName = versionName;
    }
}