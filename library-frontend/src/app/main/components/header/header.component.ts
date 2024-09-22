import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { User } from '../../../core/models/user';
import { UsersService } from '../../services/users.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
})
export class HeaderComponent {
  public user : User | undefined = undefined;

  constructor(
    private usersService: UsersService,
    private router: Router){
    usersService.getCurrentUser$()
      .subscribe(user => {
        this.user = user;
      })
  }

  logout(){
    this.usersService.logout();
    this.user = undefined;
    this.router.navigate(['home']);
  }

  isAdmin(){
    let result = this.user?.roles.some(role => role.name === 'Admin');
    return result;
  }
}
