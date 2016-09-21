import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http, Headers, RequestOptions } from '@angular/http';
import { CanActivate, Router } from '@angular/router';

import { SpinnerService, UserProfileService } from '../../core';


@Injectable()
export class LoginService {
    constructor(
        private http: Http,
        private router: Router,
        private spinnerService: SpinnerService,
        private userProfileService: UserProfileService) { }

    login(username: string, password: string) {

        this.spinnerService.show();

        return Observable.create(observer => {
            let url = "http://localhost:5100/connect/token";
            let body = "grant_type=password&client_id=snazzleClient&username=" + username + "&password=" + password + "&scope=profile roles email";
            let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });
            let options = new RequestOptions({ headers: headers });

            return this.http.post(url, body, options).subscribe(
                response => {
                    localStorage.setItem('access_token', response.json().access_token);
                    localStorage.setItem('expires_in', response.json().expires_in);
                    localStorage.setItem('token_type', response.json().token_type);
                    localStorage.setItem('userName', username);
                    this.userProfileService.isLoggedIn = true;
                    observer.next(true);
                    observer.complete();
                },
                error => {
                    observer.next(false);
                    console.log(error.text());
                    observer.complete();
                }
            );

        });

        //return Observable.of(true)
        //    .do(_ => this.spinnerService.show())
        //    .delay(1000)
        //    .do(this.toggleLogState.bind(this));
    }

    logout() {
        this.toggleLogState(false);
    }

    private toggleLogState(val: boolean) {
        this.userProfileService.isLoggedIn = val;
        this.spinnerService.hide();
    }
}
