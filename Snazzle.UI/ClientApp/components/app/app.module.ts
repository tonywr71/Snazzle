import { NgModule }       from '@angular/core';
import { BrowserModule }  from '@angular/platform-browser';
import { HttpModule }  from '@angular/http';

import { FetchData }         from '../fetch-data/fetch-data';
import { Counter }         from '../counter/counter';
import { Home }         from '../home/home';
import { NavMenu }         from '../nav-menu/nav-menu';
import { AppComponent }         from './app.component';
import { routing } from '../../app.routing';
import { People }         from '../people/people';
import { CatsByOwnerGender }         from '../cats-by-owner-gender/cats-by-owner-gender';
import { LoginComponent }         from '../login/login';
import { LogoutComponent }         from '../login/logout';
import { AuthenticationService } from '../../services/AuthenticationService';

@NgModule({
    imports: [
        BrowserModule,
        HttpModule,
        routing
    ],
    declarations: [
        NavMenu,
        AppComponent,
        Home,
        Counter,
        FetchData,
        People,
        CatsByOwnerGender,
        LoginComponent,
        LogoutComponent
    ],
    providers: [
        AuthenticationService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
