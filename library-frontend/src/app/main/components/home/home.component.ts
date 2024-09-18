import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UsersService } from '../../services/users.service';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { BooksService } from '../../services/books.service';
import { Book } from '../../../core/models/book';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './home.component.html',
})
export class HomeComponent  {

  private pageSize: number = 15;
  private pageIndex: number = 0;

  public books: Book[] = []; 

  constructor(
    private usersService: UsersService,
    private booksService: BooksService) {

  }

  ngOnInit(){
    this.getBooks();
    console.log("init books", this.books );
  }

  isAdmin(): boolean {
    const user = this.usersService.getUser();
    return user?.roles.some(role => role.name === 'Admin') ?? false;
  }

  getBooks(){
    this.booksService.getPage(this.pageIndex, this.pageSize)
      .subscribe(books => {
        this.books = books;
        this.pageIndex++;
      });
  }
}
