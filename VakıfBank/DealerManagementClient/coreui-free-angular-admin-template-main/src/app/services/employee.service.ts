import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
const httpOptions = {
  headers : new HttpHeaders({'Content-Type':'application/json'})
}

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient) { }
  
  GetProfile(): Observable<any>{
    return this.http.get("http://localhost:5248/api/employee-profile");
  }

  GetEmployeesByCompanyId(companyId: any): Observable<any>{
    return this.http.get<any>("http://localhost:5248/api/companies/" + companyId + "/employees");
  }

  UpdateProfile(Mail: any, Password: any): Observable<any>{
    return this.http.put("http://localhost:5248/api/employees",{ Mail, Password},httpOptions);
  }
  
  CreateAccount(CompanyId: any, Name: any, Surname: any, EmployeeNumber: any, Mail: any, Password: any): Observable<any>{
    return this.http.post("http://localhost:5248/api/employees",{ CompanyId, Name, Surname, EmployeeNumber, Mail, Password },httpOptions);
  }

  DeleteEmployee(employee_id: any): Observable<any>{
    return this.http.delete("http://localhost:5248/api/employees/" + employee_id);
  }
}
