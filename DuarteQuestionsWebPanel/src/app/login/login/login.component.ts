import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public login: FormGroup;

  constructor() {
    this.login = new FormGroup({
      username: new FormControl(),
      password: new FormControl()
    });
  }

  public ngOnInit(): void { 
  }

  public onSubmit(form: FormGroup): void {
    const username: string = form.controls["username"].value;
    const password: string = form.controls["password"].value;
    const output: string = "your name is " + username + " and your password is " + password;
    console.log(output);
  }
}
