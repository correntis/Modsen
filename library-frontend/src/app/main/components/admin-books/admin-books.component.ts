import { Guid } from 'guid-typescript';
import { Book } from '../../../core/models/book';
import { BooksService } from './../../services/books.service';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-admin-books',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-books.component.html',
})
export class AdminBooksComponent {

  public books: Book[] = [];

  public pageSize = 15;
  public pageIndex = 0;

  constructor(
    private booksService: BooksService,
    private router: Router,
    private dialog: MatDialog){

  }

  ngOnInit(){
    this.getBooks();
  }

  getBooks(){
    this.booksService.getPage(this.pageIndex, this.pageSize)
      .subscribe(books => {
        this.books = books;
        this.pageIndex++;
      })
  }

  deleteBook(id: Guid){
    const dialogRef = this.dialog.open(ConfirmDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.booksService.delete(id).subscribe(() => {
          console.log("book deleted", id);
          this.getBooks();
        });
      }
    });
  }

  redirectToEdit(book: Book | null){
    if (book) {
      this.router.navigate([`/admin/books/edit/${book.id}`])
    } else {
      this.router.navigate(["/admin/books/new"])
    }
  }
}
