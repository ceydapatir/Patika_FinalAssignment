import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent{

  TokenRequest = new FormGroup({
    EmployeeNumber: new FormControl(''),
    Password: new FormControl('')
  })
  errorMessage: any;

  constructor(
      private authService: AuthService, 
      private router: Router
  ) { }

  ngOnInit(): void { }

  OnSubmit(){
    const{ EmployeeNumber, Password } = this.TokenRequest.value;

    this.authService.register(EmployeeNumber , Password).subscribe({
      next: data => { 
        console.log(data);  
        if (data.success) {
          localStorage.setItem('authToken', data.response.token)
          this.router.navigate(['/dashboard']);
        } else {
          this.errorMessage = data.message;
        }
      },
      error: err => {
        console.log(err.error.message);
        alert(err.error.message);
      }
    });

    
  }
}
