import { Component, inject } from '@angular/core';
import { AnswerService } from '../services/answer.service';
import { MessageType, ToasterService } from '../services/toaster.service';
import { AnswerViewModel, UserViewModel } from '../DTOs/models/models';
import { UserService } from '../services/user.service';

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
  public AnswerDelete: number = 6;
  
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
  public answerDeleteEndpoints: any[] = [
    { value: -1, viewValue: "Select an option" },
    { value: this.AnswerDelete, viewValue: "delete-answer" }
  ];

  private answerGetAll: boolean = false;
  public answerKeyword: string = "";
  public answers: AnswerViewModel[] = [];
  public answersForUpdate: AnswerViewModel[] = [];
  public answersForDelete: AnswerViewModel[] = [];
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

  private populateAnswerList(): void {
    this.answerService.getAnswerList({
      keyword: this.answerKeyword,
      getAll: this.answerGetAll
    }).subscribe({
      next: response => this.answers = response,
      error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
    });
  }
  
  public getAnswerListClicked(): void {
    if (this.answerGetAll || this.answerKeyword.length > 0) {    
      this.populateAnswerList();
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
        next: response => this.answerCreatedMessage = "Answer '" + response.text + "' was created successfully!",
        error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
      });
    } else {
      this.toaster.showMessage(MessageType.Warning, "Warning", "Enter an answer text before clicking on 'create'");
    }
  }

  private populateAnswerForUpdate(): void {
    this.answerService.getAnswerList({
      keyword: "",
      getAll: true
    }).subscribe({
      next: response => this.answersForUpdate = response,
      error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
    });
  }

  public answerPutMethodSelected($event: any): void {
    this.answerValue = $event.value;
    if (this.answerValue === this.AnswerUpdate) {      
      this.populateAnswerForUpdate();
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

  private populateAnswerForDelete(): void {
    this.answerService.getAnswerList({
      keyword: "",
      getAll: true
    }).subscribe({
      next: response => this.answersForDelete = response,
      error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
    });
  }

  public answerDeleteMethodSelected($event: any): void {
    this.answerValue = $event.value;
    if (this.answerValue === this.AnswerDelete) {      
      this.populateAnswerForDelete();
    }
  }

  public deleteAnswerClicked($event: any): void {
    this.answerService.deleteAnswer($event.id).subscribe({
      next: response => {
        this.toaster.showMessage(MessageType.Success, "OK", "Answer deleted successfully!");
        this.populateAnswerForDelete();
      },
      error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
    });
  }

  private userService: UserService = inject(UserService);

  public UserGetList: number = 1;
  public UserGet: number = 2;
  public UserCreate: number = 3;

  public userGetEndpoints: any[] = [
    { value: -1, viewValue: "Select an option" },
    { value: this.UserGetList, viewValue: "get-user-list" },
    { value: this.UserGet, viewValue: "get-user" }
  ];
  public userPostEndpoints: any[] = [
    { value: -1, viewValue: "Select an option" },
    { value: this.UserCreate, viewValue: "create-user" }
  ];

  public userValue: number = this.UserGetList;

  private userGetAll: boolean = false;
  public userKeyword: string = "";
  public users: UserViewModel[] = [];
  public userId: number = 1;
  public userText: string = "";

  public username: string = "";
  public userEmail: string = "";
  public userConfirmedEmail: string = "";
  public userPassword: string = "";
  public userConfirmedPassword: string = "";
  public userCreatedMessage: string = "Please, do click on 'create'";
  
  public userGetMethodSelected($event: any): void {
    this.userValue = $event.value;
  }

  public userGetAllChanged($event: any): void {
    this.userGetAll = $event.checked;
  }

  private populateUserList(): void {
    this.userService.getUserList({
      keyword: this.userKeyword,
      getAll: this.userGetAll
    }).subscribe({
      next: response => this.users = response,
      error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
    });
  }

  public getUserListClicked(): void {
    if (this.userGetAll || this.userKeyword.length > 0) {
      this.populateUserList();
    }
  }

  public userIdChanged($event: any): void {
    this.userId = $event;
  }

  public getUserById(): void {
    if (this.userId >= 0) {
      this.userService.getUserById(this.userId).subscribe({
        next: response => this.userText = response.id + " - " + response.name + " - " + response.email,
        error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
      });
    }
  }

  public userPostMethodSelected($event: any): void {
    this.userValue = $event.value;
  }

  public createUserClicked(): void {
    if (this.username.length === 0) {
      this.toaster.showMessage(MessageType.Warning, "Warning", "You need to enter the username!");
      return;
    }
    if (this.userEmail.length === 0) {
      this.toaster.showMessage(MessageType.Warning, "Warning", "You need to enter the email!");
      return;
    }
    if (this.userConfirmedEmail.length === 0) {
      this.toaster.showMessage(MessageType.Warning, "Warning", "You need to enter the confirmed email!");
      return;
    }
    if (this.userPassword.length === 0) {
      this.toaster.showMessage(MessageType.Warning, "Warning", "You need to enter the password!");
      return;
    }
    if (this.userConfirmedPassword.length === 0) {
      this.toaster.showMessage(MessageType.Warning, "Warning", "You need to enter the confirmed password!");
      return;
    }
    if (this.userEmail !== this.userConfirmedEmail) {
      this.toaster.showMessage(MessageType.Warning, "Warning", "Email and confirmed email need to be equals")
      return;
    }
    if (this.userPassword !== this.userConfirmedPassword) {
      this.toaster.showMessage(MessageType.Warning, "Warning", "Password and confirmed password need to be equals");
      return;
    }
    this.userService.createUser({
      name: this.username,
      email: this.userEmail,
      confirmedEmail: this.userConfirmedEmail,
      password: this.userPassword,
      confirmedPassword: this.userConfirmedPassword
    }).subscribe({
      next: response => this.userCreatedMessage = "User '" + this.username + "' was created successfully!",
      error: err => this.toaster.showMessage(MessageType.Critical, "Error", err.message)
    });
  }
}
