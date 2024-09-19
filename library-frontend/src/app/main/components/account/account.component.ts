import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { User } from '../../../core/models/user';
import { UsersService } from '../../services/users.service';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account.component.html'
})
export class AccountComponent {
  public user: User | undefined = undefined;

  constructor(private usersService: UsersService){}

  ngOnInit(){
    this.user = this.usersService.getCurrentUser();
    console.log(this.user);
  }
}
