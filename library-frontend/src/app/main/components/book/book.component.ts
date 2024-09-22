import { Component } from '@angular/core';
import { Book } from '../../../core/models/book';
import { BooksService } from '../../services/books.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { CommonModule } from '@angular/common';
import { User } from '../../../core/models/user';
import { UsersService } from '../../services/users.service';
import { HeaderComponent } from "../header/header.component";

@Component({
  selector: 'app-book',
  standalone: true,
  imports: [CommonModule, HeaderComponent],
  templateUrl: './book.component.html',
})
export class BookComponent {
  public book: Book | undefined = undefined;
  public user: User | undefined = undefined;

  constructor(
    private bookService: BooksService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private usersService: UsersService,
  ) {}

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe((params) => {
      const bookId = params.get('id');

      if (bookId && Guid.isGuid(bookId)) {
        this.bookService.get(Guid.parse(bookId)).subscribe((book) => {
          this.book = book;
        });
      }
    });

    this.usersService.getCurrentUser$()
      .subscribe(user => {
        this.user = user;
      })
  }

  issueBook(){
    this.usersService.addBook(this.user?.id!, this.book?.id!)
      .subscribe(id => {
        this.usersService.loadFromCookies()
        this.router.navigate(['home']);    
      })
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
