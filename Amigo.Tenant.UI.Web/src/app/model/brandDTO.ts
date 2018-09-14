import { Component } from '@angular/core';

export class BrandDTO {

    brandId: number;
    brandName: string;

    constructor(brandId: number, brandName: string) {
        this.brandId = brandId;
        this.brandName = brandName;
    }
}