import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from './http.service';
import { ApiResponse } from '../models/api-response';
import { TenantService } from './tenant.service';
import { EMPTY_GUID } from '../constents';

export interface IRole {
  id: string;
  description: string;
  name: string;
}

export interface IRoleList {
  id: string;
  name: string;
}

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  // constructor(private readonly http: HttpService) {}
  tenantId: string | null = '';
  constructor(
    private readonly http: HttpService,
    private readonly tenantService: TenantService
  ) {
    this.tenantId =
      this.tenantService.getCurrentTenantId() == null
        ? EMPTY_GUID
        : this.tenantService.getCurrentTenantId();
  }

  deleteRole(id: string): Observable<ApiResponse<any>> {
    return this.http.delete(`role/delete-role/${id}`);
  }

  createRole(payload: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>('role/save-role', payload);
  }

  editRole(payload: any): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>('role/update-role', payload);
  }

  getRolesByTenant(): Observable<
    ApiResponse<{ id: string; roleName: string }[]>
  > {
    return this.http.get(`role/get-role-by-teanantId/${this.tenantId}`);
  }
}
