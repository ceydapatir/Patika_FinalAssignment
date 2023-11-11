import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
const httpOptions = {
  headers : new HttpHeaders({'Content-Type':'application/json'})
}

@Injectable({
  providedIn: 'root'
})
export class OrderItemService {
  constructor(private http: HttpClient) { }
  
  GetCartItems(): Observable<any>{
    return this.http.get("http://localhost:5248/api/order-items");
  }

  GetOrderItemsByOrderId(order_id: any): Observable<any>{
    return this.http.get("http://localhost:5248/api/orders/" + order_id + "/order-items");
  }

  AddItemToCart(ProductId: any, Amount: any): Observable<any>{
    console.log(ProductId, Amount);
    return this.http.post("http://localhost:5248/api/order-items", {ProductId, Amount}, httpOptions);
  }
  
  DeleteItemFromCart(orderitem_id: any): Observable<any>{
    return this.http.delete("http://localhost:5248/api/order-items/" + orderitem_id);
  }

  DeleteAllItemsFromCart(): Observable<any>{
    return this.http.delete("http://localhost:5248/api/order-items");
  }
  
  UpdateItemAmount(orderitem_id: any, Amount: any): Observable<any>{
    return this.http.put("http://localhost:5248/api/order-items/" + orderitem_id, {Amount}, httpOptions);
  }
}
