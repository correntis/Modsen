import { HttpClient } from '@angular/common/http';
import { User } from './../../core/models/user';
import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Observable } from 'rxjs';
import * as crypto from "crypto-js";


@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private currentUser?: User = undefined;
  private apiName: string = "http://localhost:5005/api/v1/users"
  private secretKey: string = "secretKey"; // should be in safe place

  constructor(private http: HttpClient) 
  {
    if (!this.currentUser) {
      const id: string | undefined = this.getCookieId();
      if(id){
        this.getUser(this.decryptId(id))
          .subscribe(user => {
            this.currentUser = user;
          });
      }
    }
  }

  public loadFromCookies(){
    if (!this.currentUser) {
      const id: string | undefined = this.getCookieId();
      if(id){
        this.getUser(this.decryptId(id))
          .subscribe(user => {
            this.currentUser = user;
          });
      }
    }
  }

  public getUser(id: Guid): Observable<User>{
    return this.http.get<User>(this.apiName + `/${id}`, {withCredentials: true})
  }

  public setCurrentUser(user: User) : void{
    this.currentUser = user;
    this.setCookieId(this.currentUser.id);
  }

  public getCurrentUser() : User | undefined{
    return this.currentUser;
  }

  public logout() : void{
    this.currentUser = undefined;
    this.deleteCookieId();
  }

  public isLoggedIn() : boolean{
    return !!this.currentUser;
  }

  private setCookieId(userId: Guid){
    const date : Date = new Date();
    const expireDays: number = 30
    date.setTime(date.getTime() + expireDays * 24 * 60 * 60 * 1000);    
  
    document.cookie = `id=${this.encryptId(userId)};expires=${date.toUTCString()};path=/`;
  }

  private getCookieId(): string | undefined {
    const nameEQ = `id=`;
    const cookiesArray = document.cookie.split(';');
    
    for (let cookie of cookiesArray) {
      cookie = cookie.trim();
      if (cookie.indexOf(nameEQ) === 0) {
        return cookie.substring(nameEQ.length, cookie.length);
      }
    }
    return undefined;
  }
  
  private deleteCookieId(){
    document.cookie = `id=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/`;
  }

  private encryptId(id: Guid): string{
    return crypto.AES.encrypt(id.toString(), this.secretKey).toString();
  }

  private decryptId(encryptedId: string): Guid{
    const bytes = crypto.AES.decrypt(encryptedId, this.secretKey);
    return Guid.parse(bytes.toString(crypto.enc.Utf8));
  }
}


