import { NgModule }       from '@angular/core';
import { BrowserModule }  from '@angular/platform-browser';
import { HttpModule }  from '@angular/http';

import { FetchData }         from './fetch-data/fetch-data';
import { Counter }         from './counter/counter';
import { HomeComponent }         from './home/home';
import { NavMenu }         from './nav-menu/nav-menu';
import { AppComponent }         from './app.component';
import { AppRoutingModule } from '../app.routing';
import { People }         from './people/people';
import { AuthenticationService } from '../services/AuthenticationService';
import { HttpInterceptor } from '../services/HttpInterceptor';
import { CoreModule } from '../core/core.module';
import { LoginModule } from './login/login.module';
import { AdminModule } from './admin/admin.module';
import { MainLoginComponent } from './login/main-login.component';
import { LogoutComponent } from './login/logout';

@NgModule({
    imports: [
        BrowserModule,
        HttpModule,
        LoginModule,
        // Routes get loaded in order. It is important that login
        // come before AppRoutingModule, as
        // AppRoutingModule defines the catch-all ** route
        AppRoutingModule,
        CoreModule,
        AdminModule
    ],
    declarations: [
        NavMenu,
        AppComponent,
        HomeComponent,
        Counter,
        MainLoginComponent,
        FetchData,
        People,
        LogoutComponent
    ],
    providers: [
        AuthenticationService,
        HttpInterceptor
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
    constructor() {
        console.log("startup!");
    }

}
