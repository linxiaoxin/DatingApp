import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { PaginatedResult } from '../_models/Pagination';
import { map } from 'rxjs/operators';
import { Message } from '../_models/Message';
import { resetCompiledComponents } from '@angular/core/src/render3/jit/module';

// const httpOptions = {
//   headers: new HttpHeaders({
//      Authorization : 'Bearer ' + localStorage.getItem('token')
//   })
// };
@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl + '';

  constructor(private http: HttpClient) { }

  getUsers(page?, itemsPerPage?, userParams?, likeParams?): Observable<PaginatedResult<User[]>> {
    const paginationResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
    // return this.http.get<User[]>(this.baseUrl + 'users', httpOptions);
    let params = new HttpParams();
    if (page != null && itemsPerPage  != null) {
      params = params.append('pageNumber',  page).append('pageSize', itemsPerPage);
    }
    if (userParams != null) {
      params = params.append('minAge', userParams.minAge)
        .append('maxAge', userParams.maxAge)
        .append('gender', userParams.gender)
        .append('orderBy', userParams.orderBy);
    }
    if(likeParams === 'Likers') {
      params = params.append('isLiker', 'true')
    }
    if(likeParams === 'Likees') {
      params = params.append('isLikee', 'true')
    }
    return this.http.get<User[]>(this.baseUrl + 'users', { observe: 'response', params}).pipe(
      map(response => {
        paginationResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginationResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginationResult;
      })
    );
  }

  getUser(id: number): Observable<User> {
    // return this.http.get<User>(this.baseUrl + 'users/' + id, httpOptions);
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }

  updateUser(id: string, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user);
  }

  setMainPhoto(userId: number, id: number) {
    return this.http.post(this.baseUrl + 'users/' + userId + '/photos/' + id + '/setMain', {});
  }

  deletePhoto(userId: number, id: number) {
    return this.http.delete(this.baseUrl + 'users/' + userId + '/photos/' + id );
  }

  likeUser(userId: number, recipientId: number) {
    return this.http.post(this.baseUrl + 'users/' + userId + '/like/' + recipientId , {});
  }

  getMessages(userId: number, page?, itemPerPage?, messageContainer?) {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();

    let params = new HttpParams();

    if (page != null && itemPerPage != null) {
      params = params.append('PageNumber', page)
                    .append('PageSize', itemPerPage);
    }
    if (messageContainer != null) {
      params = params.append('MessageContainer', messageContainer);
    }

    return this.http.get<Message[]>(this.baseUrl + 'users/' + userId + '/messages', {observe: 'response', params})
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  getMessageThread(userId: number, recipientId: number) {
    return this.http.get<Message[]>(this.baseUrl + 'users/' + userId + '/messages/thread/' + recipientId);
  }

  sendMessage(userId: number, message: Message) {
    return this.http.post(this.baseUrl + 'users/' + userId + '/messages/', message);
  }

  deleteMessage(msgId: number, userid: number) {
    return this.http.post(this.baseUrl + 'users/' + userid + '/messages/' + msgId, {});
  }

  markAsRead(msgId: number, userId: number) {
    this.http.post(this.baseUrl + 'users/' + userId + '/messages/' + msgId + '/read', {}).subscribe();
  }
}
