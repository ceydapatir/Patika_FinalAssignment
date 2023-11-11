import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
const httpOptions = {
  headers : new HttpHeaders({'Content-Type':'application/json'})
}
@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  
  constructor(private http: HttpClient) { }

  GetCompany(companyType: any): Observable<any>{
    return this.http.get<any>("http://localhost:5248/api/" + companyType);
  }

  CreateDealer(CompanyName: any, Mail: any, ProfitMargin: any, ContractDeadline: any): Observable<any>{
    return this.http.post("http://localhost:5248/api/dealers" ,{ CompanyName, Mail, ProfitMargin, ContractDeadline},httpOptions);
  }

  UpdateCompanyProfile(companyType: any, Mail: any, ProfitMargin: any): Observable<any>{
    return this.http.put("http://localhost:5248/api/" + companyType ,{ Mail, ProfitMargin
    },httpOptions);
  }
  
  GetCompanyById(companyType: any, companyId: any): Observable<any>{
    let url;
    if(companyType == "supplier"){
      url = companyType;
    }else{
      url = companyType + "/" + companyId;
    }
    return this.http.get<any>("http://localhost:5248/api/" + url);
  }

  GetDealers(): Observable<any>{
    return this.http.get<any>("http://localhost:5248/api/dealers");
  }
  
  GetDealerById(compId: any): Observable<any>{
    return this.http.get<any>("http://localhost:5248/api/dealers/" + compId);
  }

  DeleteDealer(compId: any): Observable<any>{
    return this.http.delete("http://localhost:5248/api/dealers/" + compId);
  }
  
}
