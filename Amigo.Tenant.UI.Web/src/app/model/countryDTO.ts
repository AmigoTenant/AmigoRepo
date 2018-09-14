import { Component } from '@angular/core';

export class CountryDTO {

    id: number;
    name: string;
    isoCode: string;

    constructor(id: number, name: string) {
        this.id = id;
        this.name = name;
    }
}