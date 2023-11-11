import { Component } from '@angular/core';
import { IconSetService } from '@coreui/icons-angular';
import { freeSet } from '@coreui/icons';
import { EmployeeService } from 'src/app/services/employee.service';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-profiles',
  templateUrl: './profiles.component.html',
  styleUrls: ['./profiles.component.scss'],
  providers: [IconSetService]
})
export class ProfilesComponent {

  EmployeeUpdateRequest = new FormGroup({
    Mail: new FormControl(''),
    Password: new FormControl(''),
    SecondPassword: new FormControl('')
  })

  errorMessage: any;
  employee: any;
  update: any;

  constructor( 
    public iconSet: IconSetService, 
    private employeeServ: EmployeeService
  ) {
      iconSet.icons = { ...freeSet};
    }
  
    ngOnInit() {
      let filter_icon = Object.entries(this.iconSet.icons).filter((icon) => {
        return icon[0].startsWith('cil');
      });

      this.employeeServ.GetProfile().subscribe({
        next: data => { 
          if(data.success){console.log(data.response); 
          this.employee = data.response;
        }else{
          this.errorMessage = data.message;}},
        error: err => {
          console.log(err.error.message);
        }
      });
      
    }

    UpdateTemplate(){
      if(this.update){
        this.update = false;
      }else{
        this.update = true;
      }
    }

    async UpdateProfile(){
      const{ Mail,  Password, SecondPassword} = this.EmployeeUpdateRequest.value;
      if(SecondPassword === Password){
        await this.employeeServ.UpdateProfile(Mail, Password).subscribe({
        next: data => { 
          if(data.success){ 
            this.update = false; 
            console.log(data.response);
            location.reload();
          }else{ this.errorMessage = data.message }},
        error: err => {
          console.log(err.error.message);
        }
      });  
      }else{
        this.errorMessage = "The entered passwords do not match.";
      }
    }
    
}
