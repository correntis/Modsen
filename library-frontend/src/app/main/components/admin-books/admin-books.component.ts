import { Author } from './../../../core/models/author';
import { Guid } from 'guid-typescript';
import { Book } from '../../../core/models/book';
import { BooksService } from './../../services/books.service';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { Filter } from '../../../core/models/fitler';
import { HeaderComponent } from '../header/header.component';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-admin-books',
  standalone: true,
  imports: [CommonModule, MatPaginator, HeaderComponent],
  templateUrl: './admin-books.component.html',
})
export class AdminBooksComponent {
  public apiResources: string = environment.API_RESOURCCES;
 
  public books: Book[] = [];

  public totalPages = 0;
  public pageSize = 6;
  public pageIndex = 0;
  public filter: Filter =  {
    name: "",
    genre: "",
    author: ""
  }

  constructor(
    private booksService: BooksService,
    private router: Router,
    private dialog: MatDialog){

  }

  ngOnInit(){
    this.booksService.getAmount(this.filter)
      .subscribe(amount => {
        this.totalPages = Math.round(amount );
      });
    this.getBooks();
  }

  getBooks(){
    this.booksService.getPage(this.pageIndex, this.pageSize, this.filter)
      .subscribe(books => {
        this.books = books;
        console.log("books:", this.books);
        
      })
  }

  deleteBook(id: Guid){
    const dialogRef = this.dialog.open(ConfirmDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.booksService.delete(id).subscribe(() => {
          this.getBooks();
        });
      }
    });
  }

  onPageChange(event: any){
    this.pageIndex = event.pageIndex;
    this.getBooks();
  }

  redirectToEdit(book: Book | null){
    if (book) {
      this.router.navigate([`/admin/books/edit/${book.id}`])
    } else {
      this.router.navigate(["/admin/books/new"])
    }
  }

  isMinDate(date: Date | string | undefined){
    if (typeof date === 'string') {
      date = new Date(date);
    }
    
    let minDate: Date = new Date("0001-01-01T00:00:00"); 
    
    if (date instanceof Date) {
      return date.toISOString() === minDate.toISOString();
    }
    return false; 
  }
}
