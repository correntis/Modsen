import { AuthService } from './../../services/auth.service';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  public username: string = "";
  public email: string = "";
  public password: string = "";

  public registerErrorMessage = "";

  constructor(private authService: AuthService, private router: Router) {
  }

  public register(){
    this.authService.register(this.email, this.password, this.username)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.router.navigate(['/login'], { 
            queryParams: { 
              successMessage: 'Registration successful! Please login.' 
            }
          });
        },
        error: (error) => {
          console.log(error);
          this.registerErrorMessage = "Registration failed, please try again.";
        }
      })
  }
}
