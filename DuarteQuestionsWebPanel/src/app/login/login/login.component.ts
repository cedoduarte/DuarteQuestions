import { Component, OnInit, inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public login: FormGroup;  
  private userService = inject(UserService);

  constructor() {  
    this.login = new FormGroup({
      username: new FormControl(),
      password: new FormControl()
    });
  }

  public ngOnInit(): void { 
  }

  public onSubmit(form: FormGroup): void {
    this.userService.login({
      username: form.controls["username"].value,
      password: form.controls["password"].value
    }).subscribe({
      next: response => {
        console.log(response);
      },
      error: err => {
        console.log("error: " + err.message);
      }
    });
  }
}
