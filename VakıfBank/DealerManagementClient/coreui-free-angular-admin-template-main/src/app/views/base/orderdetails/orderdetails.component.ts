import { Component, OnInit } from '@angular/core';
import { IconSetService } from '@coreui/icons-angular';
import { freeSet } from '@coreui/icons';
import { ActivatedRoute, Router } from '@angular/router';
import { PaymentService } from 'src/app/services/payment.service';
import { OrderItemService } from 'src/app/services/orderitem.service';
import { OrderService } from 'src/app/services/order.service';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  templateUrl: './orderdetails.component.html',
  styleUrls: ['./orderdetails.component.scss'],
  providers: [IconSetService],
})
export class OrderdetailsComponent implements OnInit {

  orderItems: any;
  payment: any;
  order_id: any;
  errorMessage: any;
  order: any;
  employeeType: any;
  
  constructor( public iconSet: IconSetService,
    private paymentServ: PaymentService,  
    private orderItemServ: OrderItemService,  
    private employeeServ: EmployeeService,  
    private orderServ: OrderService,  
    private route: ActivatedRoute
  ) {
    iconSet.icons = { ...freeSet};
    this.route.params.subscribe(params => {
      this.order_id = params['id']; 
   });
  }

  async ngOnInit() {
    let filter_icon = await Object.entries(this.iconSet.icons).filter((icon) => {
      return icon[0].startsWith('cil');
    });
    
    this.employeeServ.GetProfile().subscribe({
      next: data => {
        if(data.success){
          this.employeeType = data.response.role;
        }else{
          this.errorMessage = data.message;}
        },
      error: err => {
        console.log(err.error.message);
      }
    });

    this.orderServ.GetOrderById(this.order_id).subscribe({
      next: data => {
        if(data.success){
          console.log(data.response); 
          this.order = data.response;
        }else{
          this.errorMessage = data.message;}
        },
      error: err => {
        console.log(err.error.message);
      }
    });

      this.paymentServ.GetPayment(this.order_id).subscribe({
        next: data => {
          if(data.success){
            console.log(data.response); 
            this.payment = data.response;
          }else{
            this.errorMessage = data.message;}
          },
        error: err => {
          console.log(err.error.message);
        }
      });
      
      this.orderItemServ.GetOrderItemsByOrderId(this.order_id).subscribe({
        next: data => {
          if(data.success){
            console.log(data.response); 
            this.orderItems = data.response;
          }else{
            this.errorMessage = data.message;}
          },
        error: err => {
          console.log(err.error.message);
        }
      });   
  }

  CancelOrder(order_id: any){
    this.orderServ.CancelOrder(order_id).subscribe({
      next: data => {
        if(data.success){
          console.log(data.response);
          location.reload();
        }else{
          this.errorMessage = data.message;}
        },
      error: err => {
        console.log(err.error.message);
      }
    });
  }

  ConfirmOrder(order_id: any){
    this.orderServ.ConfirmOrder(order_id).subscribe({
      next: data => {
        if(data.success){
          console.log(data.response); 
          location.reload();
        }else{
          this.errorMessage = data.message;}
        },
      error: err => {
        console.log(err.error.message);
      }
    });
  }
}
