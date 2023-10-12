import { CreateQuestionCommand, GetQuestionListQuery, QuestionViewModel, UpdateQuestionCommand } from '../DTOs/models/models';
import { EndPoints } from '../endpoints/endpoints/endpoints';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, share } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  private baseApiUrl: string = environment.baseApiUrl;
  private http: HttpClient = inject(HttpClient);

  constructor() {}
  
  public createQuestion(command: CreateQuestionCommand): Observable<boolean> {
    return this.http.post<boolean>(this.baseApiUrl + "/" + EndPoints.API_QUESTION_CREATE, command).pipe(share());
  }

  public updateQuestion(command: UpdateQuestionCommand): Observable<boolean> {
    return this.http.put<boolean>(this.baseApiUrl + "/" + EndPoints.API_QUESTION_UPDATE, command).pipe(share());
  }

  public deleteQuestion(id: number): Observable<boolean> {
    return this.http.delete<boolean>(this.baseApiUrl + "/" + EndPoints.API_QUESTION_DELETE + "/" + id).pipe(share());
  }

  public getQuestionList(query: GetQuestionListQuery): Observable<QuestionViewModel[]> {
    return this.http.get<QuestionViewModel[]>(this.baseApiUrl + "/" + EndPoints.API_QUESTION_GET_LIST + "?Keyword=" + query.keyword + "&GetAll=" + query.getAll).pipe(share());
  }

  public getQuestionById(id: number): Observable<QuestionViewModel> {
    return this.http.get<QuestionViewModel>(this.baseApiUrl + "/" + EndPoints.API_QUESTION_GET + "/" + id).pipe(share());
  }

  public restoreAll(): Observable<boolean> {
    return this.http.get<boolean>(this.baseApiUrl + "/" + EndPoints.API_QUESTION_RESTORE_ALL).pipe(share());
  }
}
