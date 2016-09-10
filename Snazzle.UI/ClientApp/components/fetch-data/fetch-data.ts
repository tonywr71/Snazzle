import * as ng from '@angular/core';
import { Http, Headers } from '@angular/http';

@ng.Component({
  selector: 'fetch-data',
  template: require('./fetch-data.html')
})
export class FetchData {
    public forecasts: WeatherForecast[];

    constructor(http: Http) {

        var jwt = localStorage.getItem("access_token");
        console.log('has jwt:');
        console.log(jwt);
        var headers = new Headers();
        headers.append('Authorization', 'Bearer ' + jwt );

        http.get('http://localhost:5100/api/SampleData/WeatherForecasts', { headers: headers }).subscribe(result => {
            this.forecasts = result.json();
        });
    }
}

interface WeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
