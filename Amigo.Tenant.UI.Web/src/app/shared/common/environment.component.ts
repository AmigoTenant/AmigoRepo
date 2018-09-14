import { environment } from '../../../environments/environment';
import { Component } from '@angular/core';
export class EnvironmentComponent{

    public config = environment;

    public localization = environment.localization;
}