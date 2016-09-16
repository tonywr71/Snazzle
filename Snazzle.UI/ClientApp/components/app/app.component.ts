import { Component } from '@angular/core';
import { NavMenu } from '../nav-menu/nav-menu';

@Component({
    selector: 'my-app',
    template: require('./app.component.html')
})
export class AppComponent {
    constructor() {
        console.log("AppComponent");
    }

}
