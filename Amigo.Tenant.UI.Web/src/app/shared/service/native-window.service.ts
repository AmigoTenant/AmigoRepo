import { Injectable } from "@angular/core";

@Injectable()
export class NativeWindowService {
    constructor() {}

    getNativeWindow() {
        return window;
    }
}