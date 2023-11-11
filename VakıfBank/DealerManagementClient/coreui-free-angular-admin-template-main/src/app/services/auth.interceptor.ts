
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem("authToken");
    if(token){
      req = req.clone({
        setHeaders: { Authorization: `Bearer ${token}`}
      });
    }
    return next.handle(req);
  }
} 
