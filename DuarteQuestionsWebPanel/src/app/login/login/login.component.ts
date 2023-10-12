import { Component, OnInit, inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageType, ToasterService } from 'src/app/services/toaster.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public login: FormGroup;
  private router: Router = inject(Router);
  private userService: UserService = inject(UserService);
  private toaster: ToasterService = inject(ToasterService);

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
        this.toaster.showMessage(MessageType.Success, "OK", "¡Ha ingresado correctamente!");
        this.router.navigate(["/api-tester"]);
      },
      error: err => {
        this.toaster.showMessage(MessageType.Critical, "Error", err.message);
      }
    });
  }
}
