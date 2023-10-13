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
  public AnswerCreate: number = 4;
  public AnswerUpdate: number = 5;

  public answerValue: number = this.AnswerGetList;

  public answerGetEndpoints: any[] = [
    { value: -1, viewValue: "Select an option" },
    { value: this.AnswerGetList, viewValue: "get-answer-list" },
    { value: this.AnswerGet, viewValue: "get-answer" },
    { value: this.AnswerRestoreAll, viewValue: "restore-all" }
  ];

  public answerPostEndpoints: any[] = [
    { value: -1, viewValue: "Select an option" },
    { value: this.AnswerCreate, viewValue: "create-answer" }
  ];

  public answerPutEndpoints: any[] = [
    { value: -1, viewValue: "Select an option" },
    { value: this.AnswerUpdate, viewValue: "update-answer" }
  ];
  
  private answerGetAll: boolean = false;
  public answerKeyword: string = "";
  public answers: AnswerViewModel[] = [];
  public answerId: number = 1;
  public answerText: string = "";
  public createAnswerText: string = "";
  public answersRestoredMessage: string = "Please, do click on 'restore all'";
  public answerCreatedMessage: string = "Please, do click on 'create'";

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

  public answerIdChanged($event: any): void {
    this.answerId = $event;
  }
  
  public getAnswerById(): void {
    if (this.answerId >= 0) {
      this.answerService.getAnswerById(this.answerId).subscribe({
        next: response => this.answerText = response.text,
        error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
      });
    }
  }

  public restoreAllAnswerClicked(): void {
    this.answerService.restoreAll().subscribe({
      next: response => this.answersRestoredMessage = "All answers were restored successfully!",
      error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
    });
  }

  public answerPostMethodSelected($event: any): void {
    this.answerValue = $event.value;
  }

  public createAnswerClicked(): void {
    if (this.createAnswerText.length > 0) {
      this.answerService.createAnswer({
        text: this.createAnswerText
      }).subscribe({
        next: response => this.answerCreatedMessage = "Answer '" + response.text + "' was created successfully",
        error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
      });
    } else {
      this.toaster.showMessage(MessageType.Warning, "Warning", "Enter an answer text before clicking on 'create'");
    }
  }

  public answerPutMethodSelected($event: any): void {
    this.answerValue = $event.value;
    if (this.answerValue === this.AnswerUpdate) {      
      this.answerService.getAnswerList({
        keyword: "",
        getAll: true
      }).subscribe({
        next: response => this.answers = response,
        error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
      });
    }
  }

  public editAnswerClicked($event: any): void {
    this.answerService.updateAnswer({
      id: $event.id,
      text: $event.text
    }).subscribe({
      next: response => this.toaster.showMessage(MessageType.Success, "OK", "Answer updated successfully!"),
      error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
    });
  }
}
