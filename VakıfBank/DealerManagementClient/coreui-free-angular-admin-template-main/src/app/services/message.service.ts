import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  http: any;

  constructor() { }
  GetMessages(): Observable<any>{
    return this.http.get("http://localhost:5248/api/messages");
  }
}
