import { Component } from '@angular/core';

export class TypeDTO {

    typeId: number;
    name: string;


    constructor(id: number, name: string) {
        this.typeId = id;
        this.name = name;
    }
}