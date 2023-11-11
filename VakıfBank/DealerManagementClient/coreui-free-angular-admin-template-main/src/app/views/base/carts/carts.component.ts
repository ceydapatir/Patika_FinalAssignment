import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { CardService } from 'src/app/services/card.service';
import { CompanyService } from 'src/app/services/company.service';
import { OrderService } from 'src/app/services/order.service';
import { PaymentService } from 'src/app/services/payment.service';
import { OrderItemService } from 'src/app/services/orderitem.service';

@Component({
  selector: 'app-carts',
  templateUrl: './carts.component.html',
  styleUrls: ['./carts.component.scss']
})
export class CartsComponent {
  
  OrderItemRequest = new FormGroup({
    Amount: new FormControl('')
  })
  
  orderItems: any;
  orderId: any;
  order: any;
  errorMessage: any;
  update = false;
  cards: any;
  isCardVisible = false;
  payment_type: any;
  card_id: any;

  constructor(
    private companyServ: CompanyService,
    private orderServ: OrderService,
    private cardServ: CardService, 
    private paymentServ: PaymentService, 
    private orderItemServ: OrderItemService, ) { }

    async ngOnInit() {

      await this.orderItemServ.GetCartItems().subscribe({
        next: data => {
          if(data.success){
            console.log(data.response); 
            this.orderItems = data.response;
            this.orderId = data.response[0].orderId;
          }else{
            this.errorMessage = data.message}
          },
        error: err => {
          console.log(err.error.message);
        }
      });

      await this.orderServ.GetCart().subscribe({
        next: data => {
          if(data.success){this.order = data.response; console.log(data.response) }else{ this.errorMessage = data.message}},
        error: err => {
          console.log(err.error.message);
        }
      });

      this.cardServ.GetCards().subscribe({
        next: data => {
          if(data.success){ this.cards = data.response; console.log(data.response) }else{ this.errorMessage = data.message }},
        error: err => {
          console.log(err.error.message);
        }
      });
    }

    UpdateAmount(orderitem_id: any){
      const{Amount} = this.OrderItemRequest.value;
      this.orderItemServ.UpdateItemAmount(orderitem_id, Amount).subscribe({
        next: data => {
          if(data.success){ 
            console.log(data.response);
            location.reload();
          }else{ this.errorMessage = data.message}},
        error: err => {
          console.log(err.error.message);
        }
      });
    }

    DeleteOrderItem(orderitem_id: any){
      this.orderItemServ.DeleteItemFromCart(orderitem_id).subscribe({
        next: data => {
          if(data.success){ 
            console.log(data.response);
            location.reload();
          }else{ this.errorMessage = data.message}},
        error: err => {
          console.log(err.error.message);
        }
      });
    }

    DeleteAllOrderItem(){
      this.orderItemServ.DeleteAllItemsFromCart().subscribe({
        next: data => {
          if(data.success){ 
            console.log(data.response);
            location.reload(); 
          }else{ this.errorMessage = data.message}},
        error: err => {
          console.log(err.error.message);
        }
      });
    }

    Payment(){
      if((this.payment_type == "open_credit" && !this.card_id) || !this.payment_type){
        this.errorMessage = "Incorrect operation.";
      }else{
        if(this.payment_type == "eft_transfer"){
          this.card_id = 0;
        }
        this.paymentServ.Payment(this.payment_type, this.card_id).subscribe({
          next: data => {
            if(data.success){ 
              console.log(data.response);
              location.reload();
          }else{ this.errorMessage = data.message}},
          error: err => {
            console.log(err.error.message);
          }
        });
      }
    }
    
    UpdateTemplate(){
      if(this.update){
        this.update = false;
      }else{
        this.update = true;
      }
    }

    GetPaymentType(event: any) {
      console.log(event.target.id);
      this.payment_type = event.target.id;
      this.isCardVisible = event.target.id === 'open_credit';
    }

    GetCardId(event: any) {
      console.log(event.target.id);
      this.card_id = event.target.id;
    }
}
