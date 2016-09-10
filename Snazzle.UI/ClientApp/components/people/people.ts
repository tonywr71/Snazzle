import * as ng from '@angular/core';
import { Http } from '@angular/http';

@ng.Component({
    selector: 'people',
    template: require('./people.html')
})
export class People {
    public people: Person[];

    constructor(http: Http) {
        http.get('http://localhost:5100/api/PeopleData/GetPeople').subscribe(result => {
            this.people = result.json();
        });
    }
}

interface Pet {
    name: string;
    type: string;
}

interface Person {
    name: string;
    gender: number;
    age: number;
    pets: Pet[];
}
