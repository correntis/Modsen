import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../../core/models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiName: string = "http://localhost:5000/api/v1/auth";

  constructor(private http: HttpClient) { }

  login(email: string, password: string){
    return this.http.post(this.apiName + '/login', 
      {
        email, 
        password
      }, { withCredentials: true })
  }

  register(email: string, password: string, username: string){
    return this.http.post(this.apiName + "/register", 
      {
        email,
        password,
        username
      }, { withCredentials: true })
  }
}
