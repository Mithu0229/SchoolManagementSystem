import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface Branch {
  id?: string;
  branchName: string;
  branchAddress: string;
  contactPerson: string;
  contactNumber: string;
  homeThemeImagePath: string | null;
  instituteId: string;
  isActive: boolean;
}

export interface BranchList {
  items: Branch[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class BranchService {
  private http = inject(HttpClient);
  private apiUrl = '/branch'; // Replace with actual API url if different

  getBranches(): Observable<ApiResponse<BranchList>> {
    return this.http.post<ApiResponse<BranchList>>(`${this.apiUrl}/get-branch-list`, {});
  }

  getBranch(id: string): Observable<ApiResponse<Branch>> {
    return this.http.get<ApiResponse<Branch>>(`${this.apiUrl}/${id}`);
  }

  createBranch(branch: FormData): Observable<ApiResponse<Branch>> {
    return this.http.post<ApiResponse<Branch>>(`${this.apiUrl}/save-branch`, branch);
  }

  updateBranch(branch: FormData): Observable<ApiResponse<Branch>> {
    return this.http.put<ApiResponse<Branch>>(`${this.apiUrl}/update-branch`, branch);
  }

  deleteBranch(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-branch/${id}`);
  }

  getBranchDropdown(): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${this.apiUrl}/get-branch-dropdown`);
  }
}
