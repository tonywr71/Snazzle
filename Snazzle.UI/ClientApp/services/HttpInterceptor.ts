﻿import { Injectable } from '@angular/core';
import { Http, Headers, Request, RequestOptions, RequestOptionsArgs, Response } from '@angular/http';
import { AuthenticationService } from './AuthenticationService';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/Rx';


@Injectable()
export class HttpInterceptor {
    constructor(private authenticationService: AuthenticationService, private http: Http, private router: Router) {
    }

    request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(this.http.request(url, options)); 
    }

    get(url: string, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(this.http.get(url, options));
    }

    post(url: string, body: string, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(this.http.post(url, body, this.getRequestOptionArgs(options)));
    }

    put(url: string, body: string, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(this.http.put(url, body, this.getRequestOptionArgs(options)));
    }

    delete(url: string, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(this.http.delete(url, options));
    }

    getRequestOptionArgs(options?: RequestOptionsArgs): RequestOptionsArgs {
        if (options == null) {
            options = new RequestOptions();
        }
        if (options.headers == null) {
            options.headers = new Headers();
        }
        options.headers.append('Content-Type', 'application/json');
        return options;
    }

    intercept(observable: Observable<Response>): Observable<Response> {
        return observable.catch((err, source) => {
            if (err.status == 401 && !err.url.endsWith('api/connect/token')) {
                localStorage.removeItem("access_token");
                this.router.navigate(['/']);
                return Observable.empty();
            } else {
                return Observable.throw(err);
            }
        });
    }
    public handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}