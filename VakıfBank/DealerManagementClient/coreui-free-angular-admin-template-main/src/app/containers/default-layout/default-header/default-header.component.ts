import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { HeaderComponent } from '@coreui/angular';

import { freeSet } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
  styleUrls: ['./default-header.component.scss'],

})
export class DefaultHeaderComponent extends HeaderComponent {

  employee: any;

  @Input() sidebarId: string = "sidebar";

  public newMessages = new Array(4)
  public newTasks = new Array(5)
  public newNotifications = new Array(5)

  constructor(public iconSet: IconSetService,
    private employeeServ: EmployeeService,
    private router: Router) {
      iconSet.icons = { ...freeSet};
    super();
  }
  ngOnInit() {
    this.employeeServ.GetProfile().subscribe({
      next: data => { 
        if(data.success){
          console.log(data); 
          this.employee = data.response;
        }else{
          console.log(data.message)
        }
      },
      error: err => {
        console.log(err.error.message);
        alert(err.error.message);
      }
    });
  }
  
  GetCartComponent(){
    this.router.navigate(['cart']);
  }

  GetLoginComponent(){
    this.router.navigate(['login']);
  }

  Logout(){
    localStorage.setItem('authToken', "");
    this.router.navigate(['']);
  }

  GetMessagesComponent(){
    this.router.navigate(['messages']);
  }
  GetProfile(){
    this.router.navigate(['profile']);
  }
}

