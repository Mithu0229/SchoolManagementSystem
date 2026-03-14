import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HttpService } from './http.service';
import { ApiResponse } from '../models/api-response';
import { Router } from '@angular/router';

export interface IUser {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  password: string;
  userType: string;
  address: string;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  constructor(
    private readonly http: HttpService,
    private readonly router: Router,
  ) {}

  createUser(payload: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>('user/save-user', payload);
  }
  editUser(payload: any): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>('user/update-user', payload);
  }
  getUserById(id: string): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(`user/${id}`);
  }

  login(credentials: { email: string; password: string }): Observable<any> {
    return this.http.post(`user/user-login`, credentials).pipe(
      tap((response: any) => {
        if (response.isSuccess) {
          if (response.data.userType == 2) {
            this.isAuthenticatedSubject.next(true);
            //localStorage.setItem('adminLayout', JSON.stringify(response.data));
            this.router.navigate(['/register']);
          } else {
            this.router.navigate(['/user']);
            this.isAuthenticatedSubject.next(true);
          }
        }
      }),
    );
  }
  logout(): void {
    this.isAuthenticatedSubject.next(false);
    //localStorage.removeItem('adminLayout');
    this.router.navigate(['/login']);
  }
}
