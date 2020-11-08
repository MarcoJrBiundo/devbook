import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Console } from 'console';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { PaginatedResult } from '../_models/Pagination';
import { Skill } from '../_models/Skill';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }



getUsers(page?, itemsPerPage?, userParams?, favouritesParam?): Observable<PaginatedResult<User[]>>{
  const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
  let params = new HttpParams();

  if (page != null && itemsPerPage != null){
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);
  }


  if (userParams != null){
    params = params.append('skill', userParams.skill);
    params = params.append('orderBy', userParams.orderBy);
  }

  if (favouritesParam === 'Favouriters'){
    params = params.append('favouriters', 'true');
   }
  if (favouritesParam === 'Favouritees'){
    params = params.append('favouritees', 'true');
   }

 

  return this.http.get<User[]>(this.baseUrl + 'users', {observe: 'response', params})
    .pipe(
      map( response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );


}
getUser(id): Observable<User>{
  return this.http.get<User>(this.baseUrl + 'users/' + id);
}

updateUser(id: number, user: User){
  return this.http.put(this.baseUrl + 'users/' + id, user);
}

deleteSkill(id: number){
  return this.http.delete(this.baseUrl + 'users/skill/' + id);
}

addSkill(skillObject: Skill){

  return this.http.post(this.baseUrl + 'users/skill/', skillObject);
}

setMainPhoto(userId: number, id: number){
  return this.http.post(this.baseUrl + 'users/' + userId + '/photos/' +  id + '/setMain', {});
}

deletePhoto(userId: number, id: number){
  return this.http.delete(this.baseUrl + 'users/' + userId + '/photos/' + id);
}

sendFavourite(id: number, recipientId: number){
  return this.http.post(this.baseUrl + 'users/' + id + '/favourite/' + recipientId, {});
}


getMessages(id: number, page?, itemsPerPage?, messageContainer?){
  const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();
  let params = new HttpParams();
  params = params.append('MessageContainer', messageContainer);

  if (page != null && itemsPerPage != null){
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);
  }

  return this.http.get<Message[]>(this.baseUrl + 'users/' + id + '/messages', {observe: 'response', params})
  .pipe(
    map( response => {
      paginatedResult.result = response.body;
      if (response.headers.get('Pagination') != null){
        paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
      }
      return paginatedResult;
    })
  );
}
getMessageThread(id: number, recipientId: number){
  return this.http.get<Message[]>(this.baseUrl + 'users/' + id + '/messages/thread/' + recipientId);

}
sendMessage(id: number, message: Message){
  return this.http.post(this.baseUrl + 'users/' + id + '/messages', message);
}
deleteMessage(id: number , userId: number){
  return this.http.post(this.baseUrl + 'users/' + userId + '/messages/' + id, {});
}
markAsRead(userId: number, messageId: number){
  this.http.post(this.baseUrl + 'users/' + userId + '/messages/' + messageId + '/read', {})
    .subscribe();
}


updateRating(id: number, user: User){
  return this.http.put(this.baseUrl + 'users/rating/' + id, user);
}

}

