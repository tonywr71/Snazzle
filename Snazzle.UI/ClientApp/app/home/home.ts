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
        var isAuthorized = this.userProfileService.isLoggedIn;
        this.jwt = localStorage.getItem("access_token");
        return isAuthorized;
    }



}
