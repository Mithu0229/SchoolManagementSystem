import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { TenantService } from './tenant.service';

export interface ApiResponse<T = any> {
  isSuccess: boolean;
  statusCode: number;
  data: T | null;
  errors: string[];
  notificationMessage: string;
}

interface ChangePasswordRequest {
  userId: string;
  oldPassword: string;
  newPassword: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly currentUserSubject = new BehaviorSubject<any | null>(null);
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private tenantService: TenantService,
    private readonly router: Router,
  ) {
    this.verifyToken();
  }

  private logoutAndRedirect() {
    localStorage.removeItem('token');
    this.isAuthenticatedSubject.next(false);
    this.router.navigate(['/login']);
  }

  verifyToken() {
    const token = localStorage.getItem('token');

    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        const currentTime = Math.floor(Date.now() / 1000); // in seconds

        if (decoded.exp && decoded.exp > currentTime) {
          // Token is valid
          this.isAuthenticatedSubject.next(true);
          this.tenantService.loadTenantProfile().then();
        } else {
          // Token is expired
          this.logoutAndRedirect();
        }
      } catch (error) {
        // Token is invalid or can't be decoded
        this.logoutAndRedirect();
      }
    }
  }

  setCurrentUser(user: any) {
    this.currentUserSubject.next(user);
  }

  getCurrentUser() {
    // return this.currentUserSubject.value ?? null;
    const currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
    return currentUser || null;
  }

  async loadCurrentUserDetails() {
    const currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
    this.setCurrentUser(currentUser);
  }

  login(credentials: {
    email: string;
    password: string;
    rememberMe: boolean;
  }): Observable<any> {
    return this.http.post(`/user/user-login`, credentials).pipe(
      //
      tap((response: any) => {
        if (response.isSuccess && response.data.token) {
          this.storeAuthData(response.data.token, response.data);
        }
      }),
    );
  }

  storeAuthData(token: string, userData: any) {
    userData.userLoginInfo.userId = userData.id;
    if (token) {
      this.getMenuList(userData.id).subscribe({
        next: (response: any) => {
          if (response.isSuccess) {
            localStorage.setItem(
              'menuList',
              JSON.stringify(response.data ?? []),
            );
          }
        },
      });
    }

    localStorage.setItem('token', token);
    localStorage.setItem('userLoginInfo', JSON.stringify(userData));
    localStorage.setItem('userId', userData.id);
    localStorage.setItem('studentId', userData.studentId);
    localStorage.setItem('currentUser', JSON.stringify(userData));
    this.setCurrentUser(userData);
    this.isAuthenticatedSubject.next(true);
    //this.tenantService.loadTenantProfile();
  }
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userLoginInfo');
    localStorage.removeItem('userId');
    localStorage.removeItem('studentId');
    localStorage.clear();
    this.tenantService.clearCurrentTenant();
    this.isAuthenticatedSubject.next(false);
  }

  sendOtp(email: string) {
    return this.http.post<ApiResponse<null>>('/Account/forgot-password', {
      email,
    });
  }

  verifyOtp(email: string, verificationCode: string) {
    return this.http.post<ApiResponse<{ hashToken: string; key: string }>>(
      '/Account/verify-reset-token',
      {
        email,
        verificationCode,
      },
    );
  }

  verifyTwoFactor(email: string, verificationCode: string) {
    return this.http.post<ApiResponse<any>>('/Account/verify-two-factor', {
      email,
      verificationCode,
    });
  }

  resetPassword(key: string, hashToken: string, password: string) {
    return this.http.post<ApiResponse<null>>('/Account/password-reset', {
      key,
      hashToken,
      password,
    });
  }

  changePassword(
    currentPassword: string,
    newPassword: string,
  ): Observable<ApiResponse<null>> {
    const userId = localStorage.getItem('userId');

    const request: ChangePasswordRequest = {
      userId: userId ?? '',
      oldPassword: currentPassword,
      newPassword: newPassword,
    };

    return this.http.post<ApiResponse<null>>(
      '/Account/change-password',
      request,
    );
  }

  setPassword(key: string, password: string) {
    return this.http.post<ApiResponse<null>>('/Account/set-password', {
      key,
      password,
    });
  }

  getMenuList(id: string | null = null) {
    const userId = id ?? localStorage.getItem('userId');
    return this.http.get<ApiResponse<null>>(
      '/Sitemap/get-all-menu-list/' + userId,
    );
  }

  getPermissionList() {
    return this.http.get<ApiResponse<null>>('/Sitemap/get-all-menu-list/');
  }

  createRole(roleData: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>('/Role/save-role', roleData);
  }

  getRoleList(payload: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>('/Role/get-role-list', payload);
  }

  updateRole(roleData: any): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>('/Role/update-role', roleData);
  }

  getRoleById(roleId: string): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(`/Role/${roleId}`);
  }

  deleteRole(deleteData: any): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`/Role/delete-role/`, {
      body: deleteData,
    });
  }

  getAssignedUserByRoleId(roleId: string): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(
      `/Role/get-user-count-by-roleId?id=${roleId}`,
    );
  }

  cancelRoleDeletion(cancelRole: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`/Role/cancel-delete-request`, {
      body: cancelRole,
    });
  }

  getDecodedToken(): any {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        return jwtDecode(token);
      } catch (error) {
        console.error('Error decoding token:', error);
        return null;
      }
    }
    return null;
  }
}
