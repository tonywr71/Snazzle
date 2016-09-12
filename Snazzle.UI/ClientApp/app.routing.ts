import { ModuleWithProviders }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Home } from './components/home/home';
import { FetchData } from './components/fetch-data/fetch-data';
import { Counter } from './components/counter/counter';
import { People } from './components/people/people';
import { CatsByOwnerGender } from './components/cats-by-owner-gender/cats-by-owner-gender';
import { LoginComponent } from './components/login/login';
import { LogoutComponent } from './components/login/logout';
import {AuthenticationService } from './services/AuthenticationService';

const appRoutes: Routes = [
    { path: '', redirectTo: 'cats-by-owner-gender', pathMatch: 'full' },
    { path: 'home', component: Home },
    { path: 'counter', component: Counter },
    { path: 'fetch-data', component: FetchData, canActivate: [AuthenticationService] },
    { path: 'people', component: People },
    { path: 'cats-by-owner-gender', component: CatsByOwnerGender },
    { path: 'login', component: LoginComponent },
    { path: 'logout', component: LogoutComponent },
    { path: '**', redirectTo: 'home' }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
