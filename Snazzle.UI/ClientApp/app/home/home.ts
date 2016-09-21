import { Component } from '@angular/core';
import { UserProfileService } from '../../core';

@Component({
  selector: 'home',
  template: require('./home.html')
})
export class HomeComponent {
    jwt: string;

    constructor(private userProfileService: UserProfileService) {
        this.jwt = localStorage.getItem("access_token");
        console.log(this.jwt);
    }

    isAuthorized(): boolean {
        this.jwt = localStorage.getItem("access_token");
        if (this.jwt !== null) {
            this.userProfileService.isLoggedIn = true;
        }
        return this.userProfileService.isLoggedIn;
    }



}
