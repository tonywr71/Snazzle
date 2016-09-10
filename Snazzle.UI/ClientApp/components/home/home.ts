import * as ng from '@angular/core';

@ng.Component({
  selector: 'home',
  template: require('./home.html')
})
export class Home {
    jwt: string;
     
    constructor() {
        this.jwt = localStorage.getItem("access_token");
        console.log(this.jwt);
    }

    callApi() {

    }
}
