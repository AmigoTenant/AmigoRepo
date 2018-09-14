import {Component} from '@angular/core';

export class EquipmentSizeDTO {

    equipmentSizeId: number;
    equipmentTypeId: number;
    equipmentTypeCode: string;
    name: string;
    code: string;
    rowStatus: string;

    constructor(private equipmentSizeId1: number, private name1: string, code1: string) {
        this.equipmentSizeId = equipmentSizeId1;
        this.name = name1;
        this.code = code1;
    }


}