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
    }

    isLoggedIn(): boolean {
        return this.userProfileService.isLoggedIn;
    }

}
