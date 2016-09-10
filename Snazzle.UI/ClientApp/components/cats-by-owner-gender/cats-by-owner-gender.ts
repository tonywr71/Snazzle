import * as ng from '@angular/core';
import { Http } from '@angular/http';
import * as _ from 'lodash';

@ng.Component({
    selector: 'cats-by-owner-gender',
    template: require('./cats-by-owner-gender.html')
})
export class CatsByOwnerGender {

    public people: Person[];
    public catsByGender: CatsByGender[];

    constructor(http: Http) {
        http.get('http://localhost:5100/api/PeopleData/GetPeople').subscribe(result => {
            this.people = result.json();

            //            var catsByGender = [];
            this.catsByGender = [];

            //give me a unique list of genders
            var genders = _(this.people.map((p) => p.gender)).uniq().value();
            for (var i = 0; i < genders.length; i++) {
                console.log("Gender:" + genders[i]);
                var ownersForThisGender = _(this.people).filter({ gender: genders[i] }).value();
                var petsForThisGender = [];
                ownersForThisGender.forEach((p) => {
                    if (p.pets != null) {
                        p.pets.filter((pet) => pet.type === "Cat").forEach((pet) => petsForThisGender.push(pet.name));
                    }
                });
                this.catsByGender.push({ gender: genders[i], pets: petsForThisGender.sort() });
            }

        });
    }
}

interface CatsByGender {
    gender: string;
    pets: string[];
}

interface Pet {
    name: string;
    type: string;
}

interface Person {
    name: string;
    gender: string;
    age: number;
    pets: Pet[];
}
