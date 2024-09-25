import { User } from './../../../core/models/user';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UsersService } from '../../services/users.service';
import { CommonModule } from '@angular/common';
import { BooksService } from '../../services/books.service';
import { Book } from '../../../core/models/book';
import { MatPaginatorModule } from '@angular/material/paginator';
import { Filter } from '../../../core/models/fitler';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from "../header/header.component";
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, CommonModule, MatPaginatorModule, FormsModule, HeaderComponent],
  templateUrl: './home.component.html',
})
export class HomeComponent  {
  public apiResources: string = environment.API_RESOURCCES;

  public user: User | undefined;
  public totalPages: number = 0
  public pageSize: number = 6;
  public pageIndex: number = 0;
  public filter: Filter = {
    name: "",
    author: "",
    genre: ""
  }
  public books: Book[] = []; 


  constructor(
    private usersService: UsersService,
    private booksService: BooksService) {
  }

  ngOnInit(){
    this.booksService.getAmount(this.filter)
      .subscribe(amount => {
        this.totalPages = Math.round(amount)
      })

    this.booksService.getPage(this.pageIndex, this.pageSize, this.filter)
    .subscribe(books => {
      this.books = books;
    });

    this.usersService.getCurrentUser$()
      .subscribe(user => {
        this.user = user;
      })
  }

  onPageChange(event: any){
    this.pageIndex = event.pageIndex;
    this.getBooks();
  }

  isAdmin(): boolean {
    return this.user?.roles.some(role => role.name === 'Admin') ?? false;
  }

  getBooks(){
    this.booksService.getPage(this.pageIndex, this.pageSize, this.filter)
      .subscribe(books => {
        this.books = books;
      });
  }

  getAmount(){
    this.booksService.getAmount(this.filter)
    .subscribe(amount => {
      this.totalPages = Math.round(amount / this.pageSize)
    }) 
  }

  onSearch(){
    this.booksService.getAmount(this.filter)
      .subscribe(amount => {
        this.totalPages = Math.round(amount)
      })

    this.booksService.getPage(this.pageIndex, this.pageSize, this.filter)
    .subscribe(books => {
      this.books = books;
    });  
  }

  onReset(){
    this.filter.author = "";
    this.filter.name = "";
    this.filter.genre = "";
    this.onSearch();
  }

  onInputKeyDown(event: KeyboardEvent){
    if (event.key === 'Enter') {
      this.onSearch();
    }
  }

  isMinDate(date: Date | string | undefined) {
    if (typeof date === 'string') {
      date = new Date(date);
    }

    let minDate: Date = new Date('0001-01-01T00:00:00');

    if (date instanceof Date) {
      return date.toISOString() === minDate.toISOString();
    }
    return false;
  }
}
 