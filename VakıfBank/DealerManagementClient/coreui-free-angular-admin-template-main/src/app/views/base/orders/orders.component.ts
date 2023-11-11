import { Component } from '@angular/core';
import { IconSetService } from '@coreui/icons-angular';
import { freeSet } from '@coreui/icons';
import { OrderService } from 'src/app/services/order.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
  providers: [IconSetService],
})
export class OrdersComponent {
  orders: any;

  errorMessage: any;
  employeeType: any;

  constructor( public iconSet: IconSetService,
    private orderServ: OrderService,  
    private router: Router
    ) {
      iconSet.icons = { ...freeSet};
    }
  
    async ngOnInit() {
      let filter_icon = Object.entries(this.iconSet.icons).filter((icon) => {
        return icon[0].startsWith('cil');
      });

      await this.orderServ.GetOwnCompanyOrders().subscribe({
        next: data => {
          if(data.success){
            console.log(data.response); 
            this.orders = data.response;
          }else{
            this.errorMessage = data.message;}
          },
        error: err => {
          console.log(err.error.message);
        }
      });
    }

    GetDetails(order_id: any){
      this.router.navigate(['orders/'+ order_id +'/orderdetails']);
    }
}
