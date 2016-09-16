import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { CanActivate, Router } from '@angular/router';

@Injectable()
export class AuthenticationService implements CanActivate {
    private loggedIn = false;

    constructor(private http: Http, private router: Router) {
        this.loggedIn = this.isAuthorized();
    }

    isAuthorized(): boolean {
        return !!localStorage.getItem('access_token');
    }

    login(username, password) {

        let url = "http://localhost:5100/connect/token";
        let body = "grant_type=password&client_id=snazzleClient&username=" + username + "&password=" + password + "&scope=profile roles";
        let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });
        let options = new RequestOptions({ headers: headers });

        return this.http.post(url, body, options).subscribe(
            response => {
                localStorage.setItem('access_token', response.json().access_token);
                localStorage.setItem('expires_in', response.json().expires_in);
                localStorage.setItem('token_type', response.json().token_type);
                localStorage.setItem('userName', username);

                this.router.navigate(['home']);
            },
            error => {
                alert(error.text());
                console.log(error.text());
            }
        );

    }

    canActivate(): boolean {
        const isAuth = this.isAuthorized();

        if (!isAuth) {
            this.router.navigate(['', 'login']);
        }

        return isAuth;

    }

    logout() {
        localStorage.removeItem('access_token');
        this.loggedIn = false;
    }

}