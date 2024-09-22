import { Component } from '@angular/core';
import { FormsModule, ɵInternalFormsSharedModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { UsersService } from '../../../main/services/users.service';
import { User } from '../../../core/models/user';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  public email: string = "";
  public password: string = "";

  public registerSuccessMessage: string = "";
  public authErrorMessage: string = "";

  constructor(
    private authService: AuthService,
    private usersService: UsersService,
    private router: Router,
    private activateRoute: ActivatedRoute) {

  }

  ngOnInit(){
    this.activateRoute.queryParams.subscribe(params => {
      this.registerSuccessMessage = params['successMessage'] || '';
    });
  }

  public login(){

    this.registerSuccessMessage = "";
    this.authErrorMessage = "";

    this.authService.login(this.email, this.password)
      .subscribe({
        next: (response) => {
          const user: User = JSON.parse(JSON.stringify(response));
          this.usersService.setCurrentUser(user);
          this.router.navigate(['/home']);
        },
        error: (error) => {
          console.log(error);
          this.authErrorMessage = "Wrong email or password, please try again.";
        }
      })
  }
}
