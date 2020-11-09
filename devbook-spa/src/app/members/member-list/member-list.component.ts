import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaginatedResult, Pagination } from 'src/app/_models/Pagination';
import { User } from '../../_models/user';
import { AlertifyService } from '../../_services/alertify.services';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  userParams: any = {};
  pagination: Pagination;

  constructor( private userService: UserService,
               private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe( data => {
      this.users = data['user'].result;
      this.pagination = data['user'].pagination;
    });

    this.userParams.skill = "";
    this.userParams.orderBy = 'lastActive';

  }


  resetFilters(){
    this.userParams.skill = "";
    this.loadUsers();
  }


  pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }



  loadUsers(){
    this.userService
      .getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe(
        (res: PaginatedResult<User[]>) => {
          this.users = res.result;
          this.pagination = res.pagination;
        },
        error => {
          this.alertify.error(error);
       });
    
  }




}
