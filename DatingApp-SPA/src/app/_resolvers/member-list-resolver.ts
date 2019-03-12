import { Injectable } from '@angular/core';
import { User } from '../_models/User';
import { UserService } from '../_services/User.service';
import { Router, ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable()
export  class MemberListResolver implements Resolve<User[]> {
    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        return this.userService.getUsers().pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data.');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }

}
