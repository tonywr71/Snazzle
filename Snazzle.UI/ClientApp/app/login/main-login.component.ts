import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import { LoginService } from './login.service';
import { ToastService, UserProfileService } from '../../core';

@Component({
    selector: 'main-login',
    template: require('./main-login.component.html'),
    providers: [LoginService]
})
export class MainLoginComponent implements OnDestroy {
    private loginSub: Subscription;
    public logo = require('../../assets/tribe200x60.png');
    public mainBackground = require('../../assets/tribal-background-dark.png');

    constructor(
        private loginService: LoginService,
        private route: ActivatedRoute,
        private router: Router,
        private toastService: ToastService,
        private userProfileService: UserProfileService) {

        console.log(this.mainBackground);
    }

    public get isLoggedIn(): boolean {
        return this.userProfileService.isLoggedIn;
    }

    public hasData(username: string, password: string): boolean {
        return (username.length > 0 && password.length > 0);
    }

    login(event, username, password) {
        event.preventDefault();
        this.loginSub = this.loginService
            .login(username, password)
            .subscribe(result => {
                if (result === true) {
                    this.toastService.activate(`Successfully logged in`);
                    if (this.userProfileService.isLoggedIn) {
                        console.log('you are logged in');
                    }
                }
                else {
                    console.log('failed to log in');
                }
            });
    }

    register(event, firstName, lastName, email, password) {
        event.preventDefault();

        this.loginSub = this.loginService
            .register(firstName, lastName, email, password)
            .subscribe(result => {
                if (result === true) {
                    console.log("registered");
                }
                else {
                    console.log("failed to register");
                }
            });

    }

    logout() {
        this.loginService.logout();
        this.toastService.activate(`Successfully logged out`);
    }

    ngOnDestroy() {
        if (this.loginSub) {
            this.loginSub.unsubscribe();
        }
    }
}
