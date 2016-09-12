import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { AuthenticationService } from '../../services/AuthenticationService';

@Component({
    selector: 'logout',
    template: `<button (click)="logout()">Logout</button>`
})
@Injectable()
export class LogoutComponent {

    constructor(private router: Router, private authenticationService: AuthenticationService) {
    }

    logout() {
        this.authenticationService.logout();
        this.router.navigate(['', '/home']);
    }


}