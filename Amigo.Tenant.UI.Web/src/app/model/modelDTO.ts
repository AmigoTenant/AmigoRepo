import { Component } from '@angular/core';

export class ModelDTO {

    brandId: number;
    brandName: string;
    modelId: number;
    modelName: string;

    constructor(modelId: number, modelName: string) {
        this.modelId = modelId;
        this.modelName = modelName;
    }
}