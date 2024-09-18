import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Author } from '../../core/models/author';
import { Guid } from 'guid-typescript';

@Injectable({
  providedIn: 'root'
})
export class AuthorsService {
  private apiName: string = "http://localhost:5005/api/v1/authors"

  constructor(private http: HttpClient) { }

  getAll(): Observable<Author[]> {
    return this.http.get<Author[]>(this.apiName);
  }

  add(author: Author): Observable<Guid> {
    var body = {
      name: author.name,
      surname: author.surname,
      birthday: author.birthday.toISOString(),
      country: author.country
    }
    return this.http.post<Guid>(this.apiName, body, { withCredentials: true });
  }
}
