import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-integer-spinner',
  templateUrl: './integer-spinner.component.html',
  styleUrls: ['./integer-spinner.component.css']
})
export class IntegerSpinnerComponent {
  @Input() public currentValue: number = 1;
  @Output() public currentValueChange: EventEmitter<number> = new EventEmitter<number>();

  public minusButtonClicked(): void {
    this.currentValue--;
    this.currentValueChange.emit(this.currentValue);
  }

  public plusButtonClicked(): void {
    this.currentValue++;
    this.currentValueChange.emit(this.currentValue);
  }
}
