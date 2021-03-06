import { Injectable } from '@angular/core';
import { UserService } from '../_services/User.service';
import { Router, ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { Message } from '../_models/Message';
import { AuthService } from '../_services/auth.service';

@Injectable()
export  class MessagesResolver implements Resolve<Message[]> {
    pageNumber = 1;
    pageSize = 5;
    messageContainer = 'unread';
    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService
        ,       private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
        return this.userService.getMessages(this.authService.decodedToken.nameid
            , this.pageNumber, this.pageSize, this.messageContainer).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data.');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }

}
