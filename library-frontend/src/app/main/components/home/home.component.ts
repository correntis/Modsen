import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UsersService } from '../../services/users.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private usersService: UsersService) {

  }

  isAdmin(): boolean {
    const user = this.usersService.getUser();
    return user?.roles.some(role => role.name === 'Admin') ?? false;
  }
}
