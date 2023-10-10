import { Injectable, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, share } from 'rxjs';
import { EndPoints } from '../endpoints/endpoints/endpoints';
import { ChangePasswordCommand, CreateUserCommand, GetUserListQuery, LogInCommand, RestoreUserCommand, UpdateUserCommand, UserViewModel } from '../DTOs/models/models';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseApiUrl: string = environment.baseApiUrl;
  private http = inject(HttpClient);

  constructor() {}

  public createUser(command: CreateUserCommand): Observable<boolean> {
    return this.http.post<boolean>(this.baseApiUrl + "/" + EndPoints.API_USER_CREATE, command).pipe(share());
  }

  public updateUser(command: UpdateUserCommand): Observable<boolean> {
    return this.http.put<boolean>(this.baseApiUrl + "/" + EndPoints.API_USER_UPDATE, command).pipe(share());
  }

  public deleteUser(id: number): Observable<boolean> {
    return this.http.delete<boolean>(this.baseApiUrl + "/" + EndPoints.API_USER_DELETE + "/" + id).pipe(share());    
  }

  public getUserList(query: GetUserListQuery): Observable<UserViewModel[]> {
    return this.http.get<UserViewModel[]>(this.baseApiUrl + "/" + EndPoints.API_USER_GET_LIST + "?Keyword=" + query.keyword + "&GetAll=" + query.getAll).pipe(share());
  }

  public getUserById(id: number): Observable<UserViewModel> {
    return this.http.get<UserViewModel>(this.baseApiUrl + "/" + EndPoints.API_USER_GET + "/" + id).pipe(share());
  }

  public login(command: LogInCommand): Observable<boolean> {
    return this.http.post<boolean>(this.baseApiUrl + "/" + EndPoints.API_USER_LOGIN, command).pipe(share());
  }

  public restoreUser(command: RestoreUserCommand): Observable<boolean> {
    return this.http.post<boolean>(this.baseApiUrl + "/" + EndPoints.API_USER_RESTORE, command).pipe(share());
  }

  public changeUserPassword(command: ChangePasswordCommand): Observable<boolean> {
    return this.http.post<boolean>(this.baseApiUrl + "/" + EndPoints.API_USER_CHANGE_PASSWORD, command).pipe(share());
  }
}