import { Injectable } from '@angular/core';
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

    // Note: the catch method did not work out of the box. I had to change the typescriptServices.js file, found
    // in C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TypeScript
    // as per an issue found on github: https://github.com/Microsoft/TypeScript/issues/8518
    // specifically a comment made by mhegacy. It suggested that I should not need to do this if I have Update 3 RTM 
    // installed, but that was not the case for me.

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