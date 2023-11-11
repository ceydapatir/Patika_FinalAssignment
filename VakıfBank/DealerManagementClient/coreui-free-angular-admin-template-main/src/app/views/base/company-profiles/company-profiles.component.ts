import { Component } from '@angular/core';
import { IconSetService } from '@coreui/icons-angular';
import { freeSet } from '@coreui/icons';
import { EmployeeService } from 'src/app/services/employee.service';
import { CompanyService } from 'src/app/services/company.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AddressService } from 'src/app/services/address.service';
import { CardService } from 'src/app/services/card.service';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-company-profiles',
  templateUrl: './company-profiles.component.html',
  styleUrls: ['./company-profiles.component.scss'],
  providers: [IconSetService]
})
export class CompanyProfilesComponent {

  CompanyUpdateRequest = new FormGroup({
    Mail: new FormControl(''),
    ProfitMargin: new FormControl('')
  })

  CardRequest = new FormGroup({
    CardName: new FormControl(''),
    CardNumber: new FormControl(''),
    CVV: new FormControl(''),
    ExpireDate: new FormControl('')
  })

  errorMessage: any;

  address: any;
  company: any;
  cards: any;

  update = false;
  addcard = false;
  company_type = "supplier";

  constructor( 
    public iconSet: IconSetService, 
    private employeeServ: EmployeeService,
    private companyServ: CompanyService, 
    private addressServ: AddressService,  
    private cardServ: CardService, 
    private router: Router
  ) {
      iconSet.icons = { ...freeSet};
    }
  
    ngOnInit() {
      let filter_icon = Object.entries(this.iconSet.icons).filter((icon) => {
        return icon[0].startsWith('cil');
      });

      this.employeeServ.GetProfile().subscribe({
        next: data => { 
            if(data.response.role == "dealer"){ this.company_type = "dealer"}

            this.companyServ.GetCompany(this.company_type).subscribe({
              next: data => { 
                if(data.success){ this.company=data.response; console.log(data.response) }else{ this.errorMessage = data.message }},
              error: err => {
                console.log(err.error.message);
              }
            });
      
            this.addressServ.GetAddress().subscribe({
              next: data => {if(data.success){console.log(data); 
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
      
            if(this.company_type == "dealer"){
              this.cardServ.GetCards().subscribe({
                next: data => {
                  if(data.success){ this.cards = data.response; console.log(data.response) }else{ this.errorMessage = data.message }},
                error: err => {
                  console.log(err.error.message);
                }
              });
            }
        },
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
    AddCardTemplate(){
      if(this.addcard){
        this.addcard = false;
      }else{
        this.addcard = true;
      }
    }

    async CreateCard(){
      const{ CardName, CardNumber, CVV, ExpireDate} = this.CardRequest.value;
      await this.cardServ.CreateCard(CardName, CardNumber, CVV, ExpireDate).subscribe({
        next: data => { 
          if(data.success){ 
            console.log(data.response);
            location.reload();
          }else{ this.errorMessage = data.message }},
        error: err => {
          console.log(err.error.message);
        }
      }); 
    }

    async DeleteCard(card_id: any){
      await this.cardServ.DeleteCard(card_id).subscribe({
        next: data => { 
          if(data.success){ 
            console.log(data.response);
            location.reload(); 
          }else{ this.errorMessage = data.message }},
        error: err => {
          console.log(err.error.message);
        }
      }); 
    }

    async UpdateCompany(){
      const{ Mail, ProfitMargin } = this.CompanyUpdateRequest.value;
      await this.companyServ.UpdateCompanyProfile(this.company_type, Mail, ProfitMargin).subscribe({
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
    }
    
  GetUserComponent(company_id: any){
    this.router.navigate([this.company_type + '/'+ company_id +'/employees']);
  }
}
