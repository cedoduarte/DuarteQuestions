import { Component, inject } from '@angular/core';
import { AnswerService } from '../services/answer.service';
import { MessageType, ToasterService } from '../services/toaster.service';
import { AnswerViewModel } from '../DTOs/models/models';

@Component({
  selector: 'app-api-tester',
  templateUrl: './api-tester.component.html',
  styleUrls: ['./api-tester.component.css']
})
export class ApiTesterComponent {
  private toaster: ToasterService = inject(ToasterService);

  private answerService: AnswerService = inject(AnswerService);

  public AnswerGetList: number = 1;
  public AnswerGet: number = 2;
  public AnswerRestoreAll: number = 3;
  public answerValue: number = this.AnswerGetList;

  public answerGetEndpoints: any[] = [
    { value: this.AnswerGetList, viewValue: "get-answer-list" },
    { value: this.AnswerGet, viewValue: "get-answer" },
    { value: this.AnswerRestoreAll, viewValue: "restore-all" }
  ];

  private answerGetAll: boolean = false;
  public answerKeyword: string = "";
  public answers: AnswerViewModel[] = [];

  public answerGetMethodSelected($event: any): void {
    this.answerValue = $event.value;
  }

  public answerGetAllChanged($event: any): void {
    this.answerGetAll = $event.checked;
  }
  
  public getAnswerListClicked(): void {
    if (this.answerGetAll || this.answerKeyword.length > 0) {    
      this.answerService.getAnswerList({
        keyword: this.answerKeyword,
        getAll: this.answerGetAll
      }).subscribe({
        next: response => this.answers = response,
        error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
      });
    }
  }
}
