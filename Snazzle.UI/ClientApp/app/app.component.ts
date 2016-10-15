import { Component } from '@angular/core';
import { NavMenu } from './nav-menu/nav-menu';
import { UserProfileService } from '../core';

@Component({
    selector: 'my-app',
    template: require('./app.component.html')
})
export class AppComponent {

    constructor(private userProfileService: UserProfileService) {
        console.log("AppComponent");
        //to do: check the authentication bearer token is set, and if so, set the userProfileService to logged in
    }

    isLoggedIn(): boolean {
        //return this.userProfileService.isLoggedIn;
        return true;
    }

}
