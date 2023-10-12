import { Component } from '@angular/core';

@Component({
  selector: 'app-api-tester',
  templateUrl: './api-tester.component.html',
  styleUrls: ['./api-tester.component.css']
})
export class ApiTesterComponent {
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

  public currentValueChanged($event: any): void {
    this.answerValue = $event.value;
  }

  public getAllChanged($event: any): void {
    this.answerGetAll = $event.checked;
    console.log(this.answerGetAll);
  }
}
