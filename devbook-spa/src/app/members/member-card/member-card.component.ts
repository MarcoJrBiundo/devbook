import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.services';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: User;
  skills: string;

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadSkills();
    
  }


  loadSkills(){
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < this.user.skills.length; i++){
      this.skills =  this.skills + this.user.skills[i].skill + ", ";

    }
    this.skills = this.skills.substring(9);
  }


  sendFavourite(id: number){
    this.userService.sendFavourite(this.authService.decodedToken.nameid, id).subscribe(data => {
      this.alertify.success('You Have Favourited: ' +  this.user.firstName);
    }, error => {
      this.alertify.error("You favourited this user already");
    });
  }


}
