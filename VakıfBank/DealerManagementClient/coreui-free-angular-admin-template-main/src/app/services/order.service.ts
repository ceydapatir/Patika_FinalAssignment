import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
const httpOptions = {
  headers : new HttpHeaders({'Content-Type':'application/json'})
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient) { }
  
  GetOrderById(order_id: any): Observable<any>{
    return this.http.get("http://localhost:5248/api/orders/" + order_id);
  }

  GetOwnCompanyOrders(): Observable<any>{
    return this.http.get("http://localhost:5248/api/orders");
  }

  GetCart(): Observable<any>{
    return this.http.get("http://localhost:5248/api/cart");
  }

  ConfirmOrder(order_id: any): Observable<any>{
    return this.http.put("http://localhost:5248/api/orders/" + order_id, httpOptions);
  }
  
  CancelOrder(order_id: any): Observable<any>{
    return this.http.delete("http://localhost:5248/api/orders/" + order_id);
  }
}
