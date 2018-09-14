
import {Injectable, Input} from "@angular/core";

@Injectable()
export class AppGlobal {    

    constructor() {}

    private _userId;
    setValue(id) {this._userId = id;}
    getValue() {return this._userId;}

}