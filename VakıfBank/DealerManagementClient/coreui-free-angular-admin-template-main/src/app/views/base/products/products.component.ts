import { Component, OnInit } from '@angular/core';
import { IconSetService } from '@coreui/icons-angular';
import { freeSet } from '@coreui/icons';
import { ProductService } from 'src/app/services/product.service';
import { FormControl, FormGroup } from '@angular/forms';
import { OrderItemService } from 'src/app/services/orderitem.service';

@Component({
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
  providers: [IconSetService],
})
export class ProductsComponent implements OnInit {

  OrderItemRequest = new FormGroup({
    Amount: new FormControl('')
  })

  products: any;
  errorMessage: any;

  constructor( public iconSet: IconSetService,
    private productServ: ProductService,
    private orderItemServ: OrderItemService,
  ) {
    iconSet.icons = { ...freeSet};
  }

  async ngOnInit() {
    let filter_icon = await Object.entries(this.iconSet.icons).filter((icon) => {
      return icon[0].startsWith('cil');
    });

    await this.productServ.GetSupplierProducts().subscribe({
      next: data => { 
        if(data.success){
          console.log(data); 
          this.products = data.response;
        }else{
          console.log(data.message)
        }
      },
      error: err => {
        console.log(err.error.message);
      }
    });

  }
    AddToCart(product_id: any){
      const{Amount} = this.OrderItemRequest.value;
      this.orderItemServ.AddItemToCart(product_id, Amount).subscribe({
        next: data => { 
          if(data.success){
            console.log(data.response);
            location.reload();
          }else{this.errorMessage = data.message;}
        },
        error: err => {
          console.log(err.error.message);
        }
      });  
    }
}
