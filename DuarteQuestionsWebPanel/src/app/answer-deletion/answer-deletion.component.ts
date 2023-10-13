import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-answer-deletion',
  templateUrl: './answer-deletion.component.html',
  styleUrls: ['./answer-deletion.component.css']
})
export class AnswerDeletionComponent {
  @Input() public answerId: number = 0;
  @Input() public answerText: string = "";
  @Output() public deleteButtonClicked: EventEmitter<any> = new EventEmitter<any>();

  public deleteClicked(): void {
    this.deleteButtonClicked.emit({
      id: this.answerId,
      text: this.answerText
    });
  }
}
