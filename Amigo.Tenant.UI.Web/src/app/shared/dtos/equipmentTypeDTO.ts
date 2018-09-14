import {Component} from '@angular/core';

export class EquipmentTypeDTO {

    equipmentTypeId: number;
    name: string;
    code: string;
    rowStatus: string;

    constructor(private equipmentTypeId1: number, name1: string) {
        this.equipmentTypeId = equipmentTypeId1;
        this.name = name1;
    }

}