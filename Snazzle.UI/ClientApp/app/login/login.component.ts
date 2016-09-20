import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import { LoginService } from './login.service';
import { ToastService, UserProfileService } from '../../core';

@Component({
  template: require('./login.component.html'),
  providers: [LoginService]
})
export class LoginComponent implements OnDestroy {
  private loginSub: Subscription;

  constructor(
    private loginService: LoginService,
    private route: ActivatedRoute,
    private router: Router,
    private toastService: ToastService,
    private userProfileService: UserProfileService) {
  }

  public get isLoggedIn() : boolean {
    return this.userProfileService.isLoggedIn;
  }

  login(event, username, password) {
    event.preventDefault();
    this.loginSub = this.loginService
        .login(username, password)
        .subscribe(result => {
            if (result === true) {
                this.toastService.activate(`Successfully logged in`);
                console.log('yay');
                if (this.userProfileService.isLoggedIn) {
                    console.log('you are logged in');
                }
            }
            else {
                console.log('nay');
            }
        });
      //.mergeMap(loginResult => this.route.queryParams)
      //.map(qp => qp['redirectTo'])
      //.subscribe(redirectTo => {
      //  this.toastService.activate(`Successfully logged in`);
      //  if (this.userProfileService.isLoggedIn) {
      //    let url = redirectTo ? [redirectTo] : [ '/home' ];
      //    this.router.navigate(url);
      //  }
      //});
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
