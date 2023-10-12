import { Component } from '@angular/core';

@Component({
  selector: 'app-integer-spinner',
  templateUrl: './integer-spinner.component.html',
  styleUrls: ['./integer-spinner.component.css']
})
export class IntegerSpinnerComponent {
  public currentValue: number = 1;

  public minusButtonClicked(): void {
    this.currentValue--;
  }

  public plusButtonClicked(): void {
    this.currentValue++;
  }
}
