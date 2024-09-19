import { Guid } from 'guid-typescript';
import { Book } from '../../../core/models/book';
import { BooksService } from './../../services/books.service';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { AnyCatcher } from 'rxjs/internal/AnyCatcher';

@Component({
  selector: 'app-admin-books',
  standalone: true,
  imports: [CommonModule, MatPaginator],
  templateUrl: './admin-books.component.html',
})
export class AdminBooksComponent {

  public books: Book[] = [];

  public totalPages = 0;
  public pageSize = 15;
  public pageIndex = 0;

  constructor(
    private booksService: BooksService,
    private router: Router,
    private dialog: MatDialog){

  }

  ngOnInit(){
    this.booksService.getAmount()
      .subscribe(amount => {
        this.totalPages = Math.round(amount / this.pageSize);
      });
    this.getBooks();
  }

  getBooks(){
    this.booksService.getPage(this.pageIndex, this.pageSize)
      .subscribe(books => {
        this.books = books;
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
}
