import { Guid } from 'guid-typescript';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Book } from '../../core/models/book';
import { Observable } from 'rxjs';
import { Author } from '../../core/models/author';

@Injectable({
  providedIn: 'root'
})
export class BooksService {

  apiName: string = "http://localhost:5005/api/v1/books" 
  resoursecUrl: string = "http://localhost:5005/Resources"

  constructor(private http: HttpClient) { }

  add(book: Book, image: File | null): Observable<Guid>{
    const formData = new FormData();
    
    formData.append('Name', book.name);
    formData.append('ISBN', book.isbn);
    formData.append('Genre', book.genre);
    formData.append('Description', book.description);
    formData.append('TakenAt', new Date().toISOString()); 
    formData.append('ReturnBy', new Date().toISOString()); 

    if (image) {
      formData.append('ImageFile', image, image.name);
    }
    
    return this.http.post<Guid>(this.apiName, formData, { withCredentials: true });
  }

  addAuthor(bookId: Guid, authorId: Guid): Observable<Guid>{
    return this.http.put<Guid>(this.apiName + `/${bookId}/authors/${authorId}`, null, {withCredentials: true});
  }

  update(book: Book, image: File | null): Observable<Guid>{
    const formData = new FormData();

    formData.append('Name', book.name);
    formData.append('ISBN', book.isbn);
    formData.append('Genre', book.genre);
    formData.append('Description', book.description);
    formData.append('TakenAt', book.takenAt!.toString()); 
    formData.append('ReturnBy', book.returnBy!.toString()); 

    if(image) {
      formData.append('ImageFile', image, image.name);
    }

    return this.http.put<Guid>(this.apiName + `/${book.id}`, formData, {withCredentials: true});
  }

  delete(id: Guid): Observable<Guid>{
    return this.http.delete<Guid>(this.apiName + `/${id}`, 
      {
        withCredentials: true
      });
  }

  deleteAuthor(bookId: Guid, authorId: Guid): Observable<Guid>{
    return this.http.delete<Guid>(this.apiName + `/${bookId}/authors/${authorId}`, {withCredentials: true});
  }

  get(id: Guid): Observable<Book>{
    return this.http.get<Book>(this.apiName + `/${id}`,)
  }

  getAll(): Observable<Book[]>{
    return this.http.get<Book[]>(this.apiName)
  }

  getPage(pageIndex: number, pageSize: number): Observable<Book[]>{
    return this.http.get<Book[]>(this.apiName, { 
      headers: {
        pageSize: pageSize.toString(),
        pageIndex: pageIndex.toString()
      }
    });
  }

  getImageFile(imageName: string): Observable<any>{
    return this.http.get(this.resoursecUrl + `/${imageName}`,
    {
      observe: 'response',
      responseType: "blob",
      withCredentials: true
    });
  }
}
