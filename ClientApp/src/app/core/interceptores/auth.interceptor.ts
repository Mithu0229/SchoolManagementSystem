import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';
import { ToastService } from '../services/toast.service';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const authService = inject(AuthService);
  const toastService = inject(ToastService);
  const router = inject(Router);
  const token = localStorage.getItem('token');
  if (token) {
    try {
      const decoded: any = jwtDecode(token);
      const currentTime = Math.floor(Date.now() / 1000); // in seconds

      if (decoded.exp && decoded.exp > currentTime) {
        // Token is valid
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`,
          },
        });
      } else {
        // Token is expired
        authService.logout();
        toastService.error(
          'Your session has expired. Please log in again to continue.',
          'Session Expired'
        );
        router.navigate(['/login']);
      }
    } catch (error) {
      // Token is invalid or can't be decoded
      authService.logout();
      router.navigate(['/login']);
    }
  }

  return next(request);
};
