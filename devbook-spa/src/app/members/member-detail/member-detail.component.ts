import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.services';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  user: User;
  newRating: number;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];


  constructor(private authService: AuthService ,private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

   ngOnInit() {
    this.route.data.subscribe( data => {
      this.user = data['user'];
  });

  this.route.queryParams.subscribe(params => {
    const selectedTab = params['tab'];
    this.memberTabs.tabs[selectedTab > 0 ? selectedTab : 0].active = true;
  });

    this.galleryOptions = [
    {
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false
    }
  ];
    this.galleryImages = this.getImages();



    

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

  selectTab(tabId: number){
    this.memberTabs.tabs[tabId].active = true;
  }


  sendFavourite(id: number){
    this.userService.sendFavourite(this.authService.decodedToken.nameid, id).subscribe(data => {
      this.alertify.success('You Have Favourited: ' +  this.user.firstName);
    }, error => {
      this.alertify.error("You favourited this user already");
    });
  }



  updateRating(){

    this.user.rating = +this.user.rating + +this.newRating;
    this.user.ratingCount = this.user.ratingCount + 1;

    console.log(this.user.rating);
    console.log(this.user.ratingCount);
 
    this.userService.updateRating(this.user.id, this.user).subscribe(next => {
       this.alertify.success('Rating Updated Successfully');
     }, error => {
       this.alertify.error(error);
     });
 }



}
