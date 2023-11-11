import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
const httpOptions = {
  headers : new HttpHeaders({'Content-Type':'application/json'})
}

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  constructor(private http: HttpClient) { }

  Payment(PaymentType: any, CardId: any): Observable<any>{
    return this.http.post("http://localhost:5248/api/payment" ,{ PaymentType, CardId
    }, httpOptions);
  }
  GetPayment(order_id: any): Observable<any>{
    return this.http.get("http://localhost:5248/api/orders/" + order_id + "/payment" );
  }
}
