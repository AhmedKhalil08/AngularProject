import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const adminGuard: CanActivateFn = (route, state) => {
const authService = inject(AuthService);
const router = inject(Router);

  console.log('user:', authService.currentUser());
  console.log('isAdmin:', authService.isAdmin());
if (authService.isAdmin()) {
  return true;
}
return router.createUrlTree(['/unauthorized']);
};
