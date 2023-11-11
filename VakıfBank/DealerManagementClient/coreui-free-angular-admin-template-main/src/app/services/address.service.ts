import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
const httpOptions = {
  headers : new HttpHeaders({'Content-Type':'application/json'})
}

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  
  constructor(private http: HttpClient) { }


  GetAddress(): Observable<any>{
    return this.http.get<any>("http://localhost:5248/api/address");
  }
  
  GetAddressByCompanyId(company_id: any): Observable<any>{
    return this.http.get<any>("http://localhost:5248/api/companies/" + company_id + "/address");
  }
  
  CreateAddress(Country: any, City: any, Province: any, AddressLine1: any, AddressLinee: any, PostalCode: any): Observable<any>{
    return this.http.post("http://localhost:5248/api/address" ,{ Country, City, Province, AddressLine1, AddressLinee, PostalCode }, httpOptions);
  }
}
