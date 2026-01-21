import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HttpService } from './http.service';
import { ApiResponse } from './auth.service';
import { HttpUrlEncodingCodec } from '@angular/common/http';

export interface TenantProfile {
  id: string;
  name: string;
  logoUrl: string;
  domain: string;
}
export interface TenantRegistrationRequest {
  id: string;
  tenantName: string;
  binNo: string;
  tenantEmail: string;
  phoneNumber: string;
  domain: string;
  tenantPath: string;
  currentPlanId: string;
  street: string;
  city: string;
  province: string;
  postCode: string;
  tenantDomainList?: any[];
  tenantAttachmentList?: any;
  tenantUserList: any[];
}

@Injectable({
  providedIn: 'root',
})
export class TenantService {
  private readonly currentTenantSubject =
    new BehaviorSubject<TenantProfile | null>(null);
  public currentTenant$ = this.currentTenantSubject.asObservable();
  codec = new HttpUrlEncodingCodec();

  constructor(private readonly http: HttpService) {}

  setCurrentTenant(tenant: TenantProfile) {
    this.currentTenantSubject.next(tenant);
  }

  getCurrentTenant(): Observable<TenantProfile | null> {
    return this.currentTenant$;
  }

  getCurrentTenantId(): string | null {
    return this.currentTenantSubject.value?.id || null;
  }

  getCurrentTenantDomain(): string | null {
    return this.currentTenantSubject.value?.domain ?? null;
  }

  // Add method to load tenant from API
  async loadTenantProfile() {
    // Get tenant info from localStorage
    const currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');

    if (currentUser && currentUser.userLoginInfo.tenantId) {
      const tenantProfile: TenantProfile = {
        id: currentUser.userLoginInfo.tenantId,
        name: currentUser.userLoginInfo.tenantName || 'Default Tenant',
        logoUrl: currentUser.userLoginInfo.logo || '',
        domain: currentUser?.userLoginInfo?.userEmail?.split('@')?.[1] ?? '',
      };
      this.setCurrentTenant(tenantProfile);
    }
  }

  clearCurrentTenant() {
    this.currentTenantSubject.next(null);
  }

  submitTenantRegistration(
    payload: TenantRegistrationRequest
  ): Observable<ApiResponse<any>> {
    return this.http.post('tenant/save-tenant', payload);
  }

  updateTenantRegistration(
    payload: TenantRegistrationRequest
  ): Observable<ApiResponse<any>> {
    return this.http.put('tenant/update-tenant', payload);
  }

  getTenantById(
    tenantId: string
  ): Observable<ApiResponse<TenantRegistrationRequest | any>> {
    return this.http.get(`tenant/get-tenant/${tenantId}`);
  }

  deactivateTenant(payload: any): Observable<ApiResponse<any>> {
    return this.http.deactivate<ApiResponse<any>>('Tenant/deactivate-tenant', {
      body: payload,
    });
  }

  transferTenantOwnership(payload: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>('role/transfer-role', payload);
  }
  verifyTransferRoleToken(userId: string): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(
      `role/verify-role-transfer/${userId}`
    );
  }

  cancelTransferRole(payload: {
    userId: string;
    password: string;
  }): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      'role/cancel-role-transfer',
      payload
    );
  }
  roleTransferActivate(key: string): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(
      `role/role-transfer-activation?key=${this.codec.encodeValue(key)}`
    );
  }
  deleteAttachment(attachmentId: string): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(
      `tenant/delete-tenant-attachment/${attachmentId}`
    );
  }
  getTenantDropdown(): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(`tenant/get-tenant-dropdown`);
  }

  saveTenantStandards(payload: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      'tenant/save-tenant-standard',
      payload
    );
  }

  getTenantStandards(tenantId: string): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(
      `tenant/get-tenant-standards?id=${tenantId}`
    );
  }
}
