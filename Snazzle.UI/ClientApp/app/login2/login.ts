import {Component} from '@angular/core';
import {Http, Headers, RequestOptions} from '@angular/http';
import {Router} from '@angular/router';
import { Injectable } from '@angular/core';
import { AuthenticationService } from '../../services/AuthenticationService';

@Component({
    selector: 'login',
    template: require('./login.html')
})
@Injectable()
export class LoginComponent2 {
    private title = 'Login';

    constructor(private _router: Router, private _http: Http, private authenticationService: AuthenticationService) {
    }

    login(event, username, password) {
        event.preventDefault();
        this.authenticationService.login(username, password)
    }
}