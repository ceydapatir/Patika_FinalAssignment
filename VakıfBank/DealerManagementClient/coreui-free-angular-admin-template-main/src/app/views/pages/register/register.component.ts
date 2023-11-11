import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { freeSet } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})

export class RegisterComponent{

  EmployeeRequest = new FormGroup({
    CompanyId: new FormControl(''), 
    EmployeeName: new FormControl(''),
    EmployeeSurname: new FormControl(''),
    EmployeeNumber: new FormControl(''),
    Mail: new FormControl(''),
    Password: new FormControl(''),
    SecondPassword: new FormControl('')
  })

  errorMessage: any;

  constructor(
    public iconSet: IconSetService, 
      private employeeServ: EmployeeService, 
      private router: Router
  ) { 
    iconSet.icons = { ...freeSet};}

  ngOnInit(): void { 
    let filter_icon = Object.entries(this.iconSet.icons).filter((icon) => {
      return icon[0].startsWith('cil');
    });}

  OnSubmit(){
  }

  CreateAccount(){
    const{ CompanyId, EmployeeName, EmployeeSurname, EmployeeNumber, Mail, Password, SecondPassword } = this.EmployeeRequest.value;
    if(SecondPassword === Password){
      this.employeeServ.CreateAccount( CompanyId, EmployeeName, EmployeeSurname, EmployeeNumber, Mail, Password).subscribe({
        next: data => { 
          console.log(data);  
          if (data.success) {
            this.router.navigate(['/login']);
          } else {
            this.errorMessage = data.message;}},
        error: err => {
          console.log(err.error.message);
          alert(err.error.message);
        }
      });
    }else{
      this.errorMessage = "Passwords do not match.";
    }
  }
}
