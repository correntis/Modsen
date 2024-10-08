import { Routes } from '@angular/router';
import { RegisterComponent } from './auth/components/register/register.component';
import { LoginComponent } from './auth/components/login/login.component';
import { HomeComponent } from './main/components/home/home.component';
import { AdminBooksComponent } from './main/components/admin-books/admin-books.component';
import { EditBooksComponent } from './main/components/edit-books/edit-books.component';
import { AccountComponent } from './main/components/account/account.component';
import { BookComponent } from './main/components/book/book.component';
import { roleGuard } from './main/guards/role.guard';

export const routes: Routes = [
    { path: 'register', component: RegisterComponent },
    { path: 'login', component: LoginComponent },
    { path: 'home', component: HomeComponent },
    { path: 'account/:id', component: AccountComponent},
    { path: 'books/:id', component: BookComponent},
    { path: 'admin/books', component: AdminBooksComponent, canActivate: [roleGuard]}, 
    { path: 'admin/books/edit/:id', component: EditBooksComponent, canActivate: [roleGuard]}, 
    { path: 'admin/books/new', component: EditBooksComponent, canActivate: [roleGuard]}, 
    { path: '**', redirectTo: '/login'},
];