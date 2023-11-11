import { Component, OnInit } from '@angular/core';
import { IconSetService } from '@coreui/icons-angular';
import { freeSet } from '@coreui/icons';
import { ProductService } from 'src/app/services/product.service';
import { CompanyService } from 'src/app/services/company.service';
import { EmployeeService } from 'src/app/services/employee.service';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  templateUrl: './stocks.component.html',
  styleUrls: ['./stocks.component.scss'],
  providers: [IconSetService],
})
export class StocksComponent implements OnInit {

  
  ProductRequest = new FormGroup({
    ProductCode: new FormControl(''),
    ProductName: new FormControl(''),
    UnitPrice: new FormControl(''),
    Stock: new FormControl(''),
    Description: new FormControl('')
  })

  company: any;
  products: any;
  company_id: any;
  create = false;
  update = false;
  errorMessage: any;
  employeeType: any;

  constructor( public iconSet: IconSetService,
    private productServ: ProductService,
    private employeeServ: EmployeeService
  ) {
    iconSet.icons = { ...freeSet};
  }

  async ngOnInit() {
    let filter_icon = await Object.entries(this.iconSet.icons).filter((icon) => {
      return icon[0].startsWith('cil');
    });

    this.employeeServ.GetProfile().subscribe({
      next: data => { 
          if(data.success){ this.employeeType = data.response.role }else{ this.errorMessage = data.message }},
      error: err => {
        console.log(err.error.message);
      }
    });

    await this.productServ.GetProducts().subscribe({
      next: data => { 
        if(data.success){
          console.log(data); 
          this.products = data.response;
          this.company_id = data.response.companyId;
        }else{
          console.log(data.message)
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
  CreateTemplate(){
    if(this.create){
      this.create = false;
    }else{
      this.create = true;
    }
  }

  CreateProduct(){
    const{ ProductCode, ProductName, UnitPrice, Stock, Description} = this.ProductRequest.value;
    this.productServ.CreateProduct(ProductCode, ProductName, UnitPrice, Stock, Description).subscribe({
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

  UpdateProduct(product_id: any){
    const{ ProductName, UnitPrice, Stock, Description} = this.ProductRequest.value;
    this.productServ.UpdateProduct(product_id,  ProductName, UnitPrice, Stock, Description).subscribe({
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
  DeleteProduct(product_id: any){
    this.productServ.DeleteProduct(product_id).subscribe({
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
}
