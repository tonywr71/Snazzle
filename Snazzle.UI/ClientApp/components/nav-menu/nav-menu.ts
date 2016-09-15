import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/AuthenticationService';

@Component({
  selector: 'nav-menu',
  template: require('./nav-menu.html')
})
export class NavMenu {

    constructor(private authenticationService: AuthenticationService) {
    }

    isAuthorized(): boolean {
        return this.authenticationService.isAuthorized();
    }
}
