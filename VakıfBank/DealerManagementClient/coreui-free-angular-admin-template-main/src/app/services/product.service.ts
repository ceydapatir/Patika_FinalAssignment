import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
const httpOptions = {
  headers : new HttpHeaders({'Content-Type':'application/json'})
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  
  constructor(private http: HttpClient) { }
  
  GetProducts(): Observable<any>{
    return this.http.get("http://localhost:5248/api/products");
  }

  DeleteProduct(product_id: any): Observable<any>{
    return this.http.delete("http://localhost:5248/api/products/" + product_id);
  }

  CreateProduct(ProductCode: any, ProductName: any, UnitPrice: any, Stock: any, Description: any): Observable<any>{
    return this.http.post("http://localhost:5248/api/products", {ProductCode, ProductName, UnitPrice, Stock, Description}, httpOptions);
  }

  GetSupplierProducts(): Observable<any>{
    return this.http.get("http://localhost:5248/api/supplier/products");
  }
  
  UpdateProduct(product_id: any, ProductName: any, UnitPrice: any, Stock: any, Description: any): Observable<any>{
    let unitPrice = 0;
    let stock = 0;
    let description = "";
    if(UnitPrice != null){ unitPrice = UnitPrice}
    if(Stock != null){ stock = Stock}
    if(Description != description){ stock = Description}
    console.log(ProductName, UnitPrice, Stock, Description)
    return this.http.put("http://localhost:5248/api/products/" + product_id ,{ ProductName, unitPrice, stock, description });
  }
}
