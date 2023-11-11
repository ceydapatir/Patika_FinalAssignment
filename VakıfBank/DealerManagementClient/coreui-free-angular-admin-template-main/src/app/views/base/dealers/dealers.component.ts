import { Component } from '@angular/core';
import { IconSetService } from '@coreui/icons-angular';
import { freeSet } from '@coreui/icons';
import { CompanyService } from 'src/app/services/company.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-dealers',
  templateUrl: './dealers.component.html',
  styleUrls: ['./dealers.component.scss'],
  providers: [IconSetService],
})
export class DealersComponent {
  DealerRequest = new FormGroup({
    CompanyName: new FormControl(''),
    Mail: new FormControl(''),
    ProfitMargin: new FormControl(''),
    ContractDeadline: new FormControl('')
  })

  company: any;
  dealers: any;
  dealer_id: any;
  create = false;
  errorMessage: any;


  constructor( public iconSet: IconSetService,
    private companyServ: CompanyService,
    private router: Router
  ) {
    iconSet.icons = { ...freeSet};
  }

  async ngOnInit() {
     let filter_icon = await Object.entries(this.iconSet.icons).filter((icon) => {
      return icon[0].startsWith('cil');
    });

    await this.companyServ.GetDealers().subscribe({
      next: data => { 
        if(data.success){
          console.log(data); 
          this.dealers = data.response;
        }else{
          console.log(data.message)
        }
      },
      error: err => {
        console.log(err.error.message);
      }
    });
  }
  
  CreateTemplate(){
    if(this.create){
      this.create = false;
    }else{
      this.create = true;
    }
  }

  GetUserComponent(company_id: any){
    this.router.navigate(['dealers/'+ company_id +'/employees']);
  }

  CreateDealer(){
    const{ CompanyName, Mail, ProfitMargin, ContractDeadline} = this.DealerRequest.value;
    this.companyServ.CreateDealer(CompanyName, Mail, ProfitMargin, ContractDeadline).subscribe({
      next: data => { 
        if(data.success){
          console.log(data); 
          location.reload();
        }else{
          this.errorMessage = data.message 
        }
      },
      error: err => {
        console.log(err.error.message);
      }
    });
  }

  DeleteDealer(dealer_id: any){
    this.companyServ.DeleteDealer(dealer_id).subscribe({
      next: data => { 
        if(data.success){
          console.log(data); 
          location.reload();
        }else{
          this.errorMessage = data.message 
        }
      },
      error: err => {
        console.log(err.error.message);
      }
    });
  }
}
