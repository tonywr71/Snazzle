import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { UserProfileService } from '../../core';

@Component({
    selector: 'logout',
    template: `<button (click)="logout()">Logout</button>`
})
@Injectable()
export class LogoutComponent {

    constructor(private router: Router, private userProfileService: UserProfileService) {
    }

    logout() {
        localStorage.removeItem('access_token');
        this.userProfileService.isLoggedIn = false;
        this.router.navigate(['', '/home']);
    }


}