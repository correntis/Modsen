import { Routes } from '@angular/router';
import { RegisterComponent } from './auth/components/register/register.component';
import { LoginComponent } from './auth/components/login/login.component';
import { HomeComponent } from './main/components/home/home.component';
import { roleGuard } from './main/guards/role.guard';
import { AdminBooksComponent } from './main/components/admin-books/admin-books.component';

export const routes: Routes = [
    { path: 'register', component: RegisterComponent },
    { path: 'login', component: LoginComponent },
    { path: 'home', component: HomeComponent },
    { path: 'admin/books', component: AdminBooksComponent, canActivate: [roleGuard] },
    { path: '**', redirectTo: '/login'},
];