import { Component, OnInit } from '@angular/core';
import { IconSetService } from '@coreui/icons-angular';
import { freeSet } from '@coreui/icons';
import { EmployeeService } from 'src/app/services/employee.service';
import { CompanyService } from 'src/app/services/company.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.scss'],
  providers: [IconSetService],
})
export class EmployeesComponent implements OnInit {

  employees: any;
  company: any;
  companyId: any
  companyType: any;
  errorMessage: any;
  employeeType: any;

  constructor( public iconSet: IconSetService,
    private employeeServ: EmployeeService,
    private route: ActivatedRoute,
    private router: Router
    ) {
      iconSet.icons = { ...freeSet};
      this.route.params.subscribe(params => {
         this.companyId = params['id']; 
         this.companyType = params['companyType']; 
      });
    }

    async ngOnInit() {
      let filter_icon = Object.entries(this.iconSet.icons).filter((icon) => {
        return icon[0].startsWith('cil');
      });
      this.employeeServ.GetProfile().subscribe({
        next: data => { 
          if(data.success){console.log(data); 
            this.employeeType = data.response.role;
          }else{this.errorMessage = data.message }},
        error: err => {
          console.log(err.error.message);
          alert(err.error.message);
        }
      });
    
      await this.employeeServ.GetEmployeesByCompanyId(this.companyId).subscribe({
        next: data => { 
          if(data.success){ 
            this.employees = data.response; 
            console.log(data.response) }else{ this.errorMessage = data.message }},
        error: err => {
          console.log(err.error.message);
        }
      });
    }
    
  GetMessagesComponent(employee_id: any){
    this.router.navigate(['messages/'+employee_id]);
  }

  DeleteEmployee(employeeId: any){
    this.employeeServ.DeleteEmployee(employeeId).subscribe({
      next: data => { 
        if(data.success){
          console.log(data);
          location.reload();
        }else{this.errorMessage = data.message }},
      error: err => {
        console.log(err.error.message);
      }
    });
  }

}
