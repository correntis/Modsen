import { CanActivateFn, Router } from '@angular/router';
import { UsersService } from '../services/users.service';
import { inject } from '@angular/core';
import { User, UserRole } from '../../core/models/user';

export const roleGuard: CanActivateFn = (route, state) => {

  const usersService : UsersService = inject(UsersService);
  const router : Router = inject(Router);

  const user: User | undefined = usersService.getUser();

  if (user && user.roles.some((role : UserRole) => role.name === 'Admin')) {
    return true;
  }

  router.navigate(['/home']);
  return false;
};
