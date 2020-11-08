import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { SIGKILL } from 'constants';
import { constants } from 'crypto';
import { Skill } from 'src/app/_models/Skill';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.services';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: User;

  newSkill: string;
  newSkillObject: any = {};
  @ViewChild('editForm', {static: true}) editForm: NgForm;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any){
    if (this.editForm.dirty){
      $event.returnValue = true;
    }
  }


  // tslint:disable-next-line: max-line-length
  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute, private authService: AuthService) { }

   ngOnInit() {
    this.route.data.subscribe( data => {
      this.user = data['user'];
  });
  }

  getImages(){
    const imageUrls = [];
    for(const photo of this.user.photos){
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
        description: photo.description,
      });
    }
    return imageUrls;
  }
  
  updateUser(){
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
        this.alertify.success('Profile Updated Successfully');
        this.editForm.reset(this.user);
      }, error => {
        this.alertify.error(error);
      });
  }






  deleteSkill(skill){
    this.userService.deleteSkill(skill.id).subscribe(next => {
      this.alertify.success('Skill Deleted Successfully');
    }, error =>{
      this.alertify.error(error);
    })
    const index = this.user.skills.indexOf(skill);
    this.user.skills.splice(index, 1);
  }

  addSkill(){
    this.newSkillObject.userId = this.user.id;
    this.newSkillObject.skill = this.newSkill;

    let localSkillObject = {
      userId : this.user.id,
      id: 0,
      skill: this.newSkill
    };
    let index = this.user.skills.length;
    this.user.skills[index] = localSkillObject;
    this.userService.addSkill(this.newSkillObject).subscribe(next => {
      this.alertify.success('Skill Added Successfully');
    }, error =>{
      this.alertify.error(error);
    })

  }

  updateMainPhoto(photoUrl){
    this.user.photoUrl = photoUrl;
  }


}
