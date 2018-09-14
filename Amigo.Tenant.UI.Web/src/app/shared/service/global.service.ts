
import {Injectable} from "@angular/core";

@Injectable()
export class GlobalService {
    private myValue;

    constructor() {}

    setValue(val) {
        this.myValue = val;
    }

    getValue() {
        return this.myValue;
    }
}