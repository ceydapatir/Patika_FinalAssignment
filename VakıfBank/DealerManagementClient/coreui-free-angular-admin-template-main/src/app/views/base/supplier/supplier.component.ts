import { Component, OnInit } from '@angular/core';
import { IconSetService } from '@coreui/icons-angular';
import { freeSet } from '@coreui/icons';
import { CompanyService } from 'src/app/services/company.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AddressService } from 'src/app/services/address.service';

@Component({
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.scss'],
  providers: [IconSetService],
})
export class SupplierComponent implements OnInit {
  
  supplier: any;
  address: any;
  errorMessage: any;

  constructor( public iconSet: IconSetService,
    private companyServ: CompanyService,
    private addressServ: AddressService,  
    private router: Router
  ) {
    iconSet.icons = { ...freeSet};
  }

  async ngOnInit() {
    let filter_icon = await Object.entries(this.iconSet.icons).filter((icon) => {
      return icon[0].startsWith('cil');
    });

    await this.companyServ.GetCompany("supplier").subscribe({
      next: data => { 
        if(data.success){console.log(data); 
          this.supplier=data.response; 
          this.addressServ.GetAddressByCompanyId(data.response.companyId).subscribe({
            next: data => {if(data.success){
              if(data.response.addressLine2 != null){
                this.address = data.response.addressLine1 + " " + data.response.addressLine2 + " " + data.response.province 
                  + "/" + data.response.city + " " + data.response.country + " " + data.response.postalCode;
                }else{
                  this.address = data.response.addressLine1 + " " + data.response.province 
                    + "/" + data.response.city + " " + data.response.country + " " + data.response.postalCode;
                }
              }else{ this.errorMessage = data.message }
            },
            error: err => {
              console.log(err.error.message);
            }
          });
        }else{console.log(data.message)}},
      error: err => {
        console.log(err.error.message);
      }
    });
  }
  
  GetUserComponent(company_id: any){
    this.router.navigate(['supplier/' + company_id +'/employees']);
  }
}
