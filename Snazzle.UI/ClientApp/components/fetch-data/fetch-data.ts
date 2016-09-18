import * as ng from '@angular/core';
import { Headers } from '@angular/http';
import { HttpInterceptor } from '../../services/HttpInterceptor'
import { AuthenticationService } from '../../services/AuthenticationService';
import { Router } from '@angular/router';

@ng.Component({
  selector: 'fetch-data',
  template: require('./fetch-data.html') 
})
export class FetchData {
    public forecasts: WeatherForecast[];

    constructor(private authenticationService: AuthenticationService, private http: HttpInterceptor, private router: Router) {

        var jwt = localStorage.getItem("access_token");
        console.log('has jwt:');
        console.log(jwt);
        var headers = new Headers();
        headers.append('Authorization', 'Bearer ' + jwt );

        http.get('http://localhost:5100/api/SampleData/WeatherForecasts', { headers: headers })
            .subscribe(result =>
            {
            this.forecasts = result.json();
            }
            //,error =>
            //{
            //    if (error.status === 401) {
            //        authenticationService.logout();
            //        this.router.navigate(['', '/home']);
            //    }
            //    console.log("An error occured!" + error.statusText);
            //}
        );
    }
}

interface WeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
