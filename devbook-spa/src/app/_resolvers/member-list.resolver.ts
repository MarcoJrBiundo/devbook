import {Injectable} from '@angular/core';
import {User} from '../_models/user';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import {UserService} from '../_services/user.service';
import {AlertifyService} from '../_services/alertify.services';
import { of, Observable } from 'rxjs';
import {catchError} from 'rxjs/operators';

@Injectable()
export class MemberListResolver implements Resolve<User[]>{


    constructor(private userService: UserService,
                private router: Router, private alterify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User[]>{
        return this.userService.getUsers().pipe(
            catchError(error => {
                this.alterify.error('Problem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
