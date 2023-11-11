import { Component } from '@angular/core';

import { navItems_admin } from './_navadmin';
import { navItems_dealer } from './_navdealer';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss'],
})
export class DefaultLayoutComponent {

  navItems: any;
  company_type = "supplier";

  constructor(
    private employeeServ: EmployeeService) {}

    async ngOnInit() {

      await this.employeeServ.GetProfile().subscribe({
        next: data => { 
          if(data.response.role == "admin"){this.navItems = navItems_admin; }else{this.navItems = navItems_dealer}},
        error: err => {
          console.log(err.error.message);
        }
      });
    }
}
