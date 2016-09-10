import {Component} from '@angular/core';
import {Http, Headers, RequestOptions} from '@angular/http';
import {Router} from '@angular/router';
import { Injectable } from '@angular/core';

@Component({
    selector: 'login',
    template: require('./login.html')
})
@Injectable()
export class LoginComponent {
    private title = 'Login';

    constructor(private _router: Router, private _http: Http) {
    }

    login(event, username, password) {
        event.preventDefault();

        let url = "http://localhost:5100/connect/token";
        let body = "grant_type=password&username=" + username + "&password=" + password ;
        let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });
        let options = new RequestOptions({ headers: headers });

        this._http.post(url, body, options).subscribe(
            response => {
                localStorage.setItem('access_token', response.json().access_token);
                localStorage.setItem('expires_in', response.json().expires_in);
                localStorage.setItem('token_type', response.json().token_type);
                localStorage.setItem('userName', response.json().userName);
                this._router.navigate(['home']);
            },
            error => {
                alert(error.text());
                console.log(error.text());
            }
        );
    }
}