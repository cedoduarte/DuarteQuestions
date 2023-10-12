import { AnswerCreatedDTO, AnswerViewModel, CreateAnswerCommand, GetAnswerListQuery, UpdateAnswerCommand } from '../DTOs/models/models';
import { EndPoints } from '../endpoints/endpoints/endpoints';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, share } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AnswerService {
  private baseApiUrl: string = environment.baseApiUrl;
  private http: HttpClient = inject(HttpClient);

  constructor() {}

  public createAnswer(command: CreateAnswerCommand): Observable<AnswerCreatedDTO> {
    return this.http.post<AnswerCreatedDTO>(this.baseApiUrl + "/" + EndPoints.API_ANSWER_CREATE, command).pipe(share());
  }
  
  public updateAnswer(command: UpdateAnswerCommand): Observable<boolean> {
    return this.http.put<boolean>(this.baseApiUrl + "/" + EndPoints.API_ANSWER_UPDATE, command).pipe(share());
  }

  public deleteAnswer(id: number): Observable<boolean> {
    return this.http.delete<boolean>(this.baseApiUrl + "/" + EndPoints.API_ANSWER_DELETE + "/" + id).pipe(share());
  }

  public getAnswerList(query: GetAnswerListQuery): Observable<AnswerViewModel[]> {
    return this.http.get<AnswerViewModel[]>(this.baseApiUrl + "/" + EndPoints.API_ANSWER_GET_LIST + "?Keyword=" + query.keyword + "&GetAll=" + query.getAll).pipe(share());
  }
  
  public getAnswerById(id: number): Observable<AnswerViewModel> {
    return this.http.get<AnswerViewModel>(this.baseApiUrl + "/" + EndPoints.API_ANSWER_GET + "/" + id).pipe(share());
  }

  public restoreAll(): Observable<boolean> {
    return this.http.get<boolean>(this.baseApiUrl + "/" + EndPoints.API_ANSWER_RESTORE_ALL).pipe(share());
  }
}
