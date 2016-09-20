import { NgModule }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './app/home/home';
import { FetchData } from './app/fetch-data/fetch-data';
import { Counter } from './app/counter/counter';
import { People } from './app/people/people';
//import { LoginComponent2 } from './app/login2/login';
//import { LogoutComponent2 } from './app/login2/logout';
//import { AuthenticationService } from './services/AuthenticationService';
import { AuthGuard, CanDeactivateGuard, UserProfileService } from './core';
import { PageNotFoundComponent } from './app/page-not-found.component';
import { AdminComponent } from './app/admin/admin.component';

export const appRoutes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    {
        path: 'admin',
        component: AdminComponent,
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        canLoad: [AuthGuard],
    },
    { path: 'home', component: HomeComponent },
    { path: 'counter', component: Counter },
    { path: 'fetch-data', component: FetchData, canActivate: [AuthGuard] },
    { path: 'people', component: People },
    { path: '**', redirectTo: 'home' }
];

@NgModule({
    imports: [RouterModule.forRoot(appRoutes)],
    exports: [RouterModule],
    providers: [
        AuthGuard,
        CanDeactivateGuard,
        UserProfileService
    ]
})
export class AppRoutingModule { }

