import { CanActivateFn, Router } from '@angular/router';
import { UsersService } from '../services/users.service';
import { inject } from '@angular/core';
import { User, UserRole } from '../../core/models/user';
import { catchError, map, of, take } from 'rxjs';

export const roleGuard: CanActivateFn = (route, state) => {
  const usersService: UsersService = inject(UsersService);
  const router: Router = inject(Router);

  return usersService.getCurrentUser$().pipe(
    take(1),  
    map(user => {
      if (user && user.roles.some((role: UserRole) => role.name === 'Admin')) {
        return true;
      } else {
        router.navigate(['/home']);
        return false;
      }
    }),
    catchError(() => {
      router.navigate(['/home']); 
      return of(false);
    })
  );
};