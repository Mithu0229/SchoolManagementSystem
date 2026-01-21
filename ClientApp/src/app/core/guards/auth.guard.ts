import { inject } from '@angular/core';
import { Router, type CanActivateFn } from '@angular/router';
import { map } from 'rxjs';
import { UserService } from '../services/user.service';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  //const authService = inject(UserService);
  const authService = inject(AuthService);

  return authService.isAuthenticated$.pipe(
    map((isAuthenticated) => {
      if (isAuthenticated) {
        return true;
      }
      debugger;
      const returnUrl = state.url; // Current URL
      return router.createUrlTree(['/login'], { queryParams: { returnUrl } });
    })
  );
};
