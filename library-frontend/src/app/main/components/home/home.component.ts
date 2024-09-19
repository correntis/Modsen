import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UsersService } from '../../services/users.service';
import { CommonModule } from '@angular/common';
import { BooksService } from '../../services/books.service';
import { Book } from '../../../core/models/book';
import { User } from '../../../core/models/user';

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
  }

  isAdmin(): boolean {
    const user = this.usersService.getCurrentUser();
    return user?.roles.some(role => role.name === 'Admin') ?? false;
  }

  getUser(): User | undefined{
    return this.usersService.getCurrentUser();
  }

  getBooks(){
    this.booksService.getPage(this.pageIndex, this.pageSize)
      .subscribe(books => {
        this.books = books;
        this.pageIndex++;
      });
  }
}
 