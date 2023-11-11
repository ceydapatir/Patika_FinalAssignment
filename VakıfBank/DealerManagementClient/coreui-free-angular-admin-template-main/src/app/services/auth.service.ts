import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

const AUTH_API = 'http://localhost:5248/api/';
const httpOptions = {
  headers : new HttpHeaders({'Content-Type':'application/json'})
}

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  token: string | undefined;
  url = "Tokens"

  constructor(private http: HttpClient) { }

  register(EmployeeNumber: any, Password: any): Observable<any>{
    return this.http.post(AUTH_API + this.url ,{ EmployeeNumber, Password
    },httpOptions);
  }

  login(EmployeeNumber: any, Password: any): Observable<string>{
    return this.http.post(AUTH_API + this.url ,{ EmployeeNumber, Password
    }, { responseType: "text"});
  }
  
}
