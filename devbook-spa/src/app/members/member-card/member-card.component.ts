import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: User;
  skills: string;

  constructor() { }

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


}
