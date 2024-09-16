import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../../core/models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  login(email: string, password: string){
    return this.http.post('http://localhost:5005/api/v1/auth/login', 
      {
        email, 
        password
      })
  }

  register(email: string, password: string, username: string){
    return this.http.post("http://localhost:5005/api/v1/auth/register", 
      {
        email,
        password,
        username
      })
  }
}
