import { CommonModule, NgFor } from '@angular/common';
import { Component } from '@angular/core';
import { User } from '../../../core/models/user';
import { UsersService } from '../../services/users.service';
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [CommonModule, HeaderComponent],
  templateUrl: './account.component.html'
})
export class AccountComponent {
  public user: User | undefined = undefined;

  constructor(private usersService: UsersService){}

  ngOnInit(){
    this.usersService.getCurrentUser$()
      .subscribe(user => {
        this.user = user;
      });
  }
}
