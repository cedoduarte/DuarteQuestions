import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'DuarteQuestionsWebPanel';
  private router = inject(Router);

  public goLogin(): void {
    console.log("go login!!!");
    this.router.navigate(["/login"]);
  }
}
