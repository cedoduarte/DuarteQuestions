import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-answer-update-editor',
  templateUrl: './answer-update-editor.component.html',
  styleUrls: ['./answer-update-editor.component.css']
})
export class AnswerUpdateEditorComponent {
  @Input() public answerId: number = 0;
  @Input() public answerText: string = "";
  @Output() public editButtonClicked: EventEmitter<any> = new EventEmitter<any>();

  public editClicked(): void {
    this.editButtonClicked.emit({
      id: this.answerId,
      text: this.answerText
    });
  }
}
