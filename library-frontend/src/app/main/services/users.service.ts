import { HttpClient } from '@angular/common/http';
import { User } from './../../core/models/user';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private currentUser?: User = undefined;

  constructor(private http: HttpClient) { }


  public setUser(user: User) : void{
    console.log("setUser", user);
    this.currentUser = user;
  }

  public getUser() : User | undefined{
    console.log("getUser", this.currentUser);
    return this.currentUser;
  }

  public logout() : void{
    this.currentUser = undefined;
  }

  public isLoggedIn() : boolean{
    return !!this.currentUser;
  }
}
