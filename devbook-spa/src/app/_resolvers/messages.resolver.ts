import {Injectable} from '@angular/core';
import {Message} from '../_models/message';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import {UserService} from '../_services/user.service';
import {AlertifyService} from '../_services/alertify.services';
import { of, Observable } from 'rxjs';
import {catchError} from 'rxjs/operators';
import {AuthService} from '../_services/auth.service';

@Injectable()
export class MessagesResolver implements Resolve<Message[]>{
    pageNumber = 1;
    pageSize = 5;
    messageContainer = 'Unread';

    constructor(private userService: UserService, private authService: AuthService,
                private router: Router, private alterify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Message[]>{
        return this.userService.getMessages(this.authService.decodedToken.nameid,
             this.pageNumber, this.pageSize, this.messageContainer).pipe(
            catchError(error => {
                this.alterify.error('Problem retrieving messages');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
