import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';

@Injectable()
export class ListService {
    todos: any[];

    constructor() {
        this.todos = [
            { title: "Read the todo list 33", completed: true },
            { title: "Look at the code 33", completed: false }
        ];
    }
}