import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/AuthenticationService';

@Component({
  selector: 'home',
  template: require('./home.html')
})
export class Home {
    jwt: string;
     
    constructor(private authenticationService: AuthenticationService) {
        this.jwt = localStorage.getItem("access_token");
        console.log(this.jwt);
    }

    isAuthorized(): boolean {
        var isAuthorized = this.authenticationService.isAuthorized();
        this.jwt = localStorage.getItem("access_token");
        return isAuthorized;
    }



}
