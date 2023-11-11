import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
const httpOptions = {
  headers : new HttpHeaders({'Content-Type':'application/json'})
}
@Injectable({
  providedIn: 'root'
})
export class CardService {

  constructor(private http: HttpClient) { }
  
  GetCards(): Observable<any>{
    return this.http.get<any>("http://localhost:5248/api/cards");
  }

  DeleteCard(cardId: any): Observable<any>{
    return this.http.delete("http://localhost:5248/api/cards/" + cardId);
  }
  
  CreateCard(CardName: any, CardNumber: any, CVV: any, ExpireDate: any): Observable<any>{
    return this.http.post("http://localhost:5248/api/cards" ,{CardName, CardNumber, CVV, ExpireDate},httpOptions);
  }
}
