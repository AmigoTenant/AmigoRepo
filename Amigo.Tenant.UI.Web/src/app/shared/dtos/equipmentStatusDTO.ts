import {Component} from '@angular/core';

export class EquipmentStatusDTO {

    equipmentStatusId: number;
    name: string;
    code: string;
    rowStatus: string;

    constructor(private equipmentStatusId1: number, name1: string) {
        this.equipmentStatusId = equipmentStatusId1;
        this.name = name1;
    }

}