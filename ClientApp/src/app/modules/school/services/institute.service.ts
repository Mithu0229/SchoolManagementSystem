import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface Institute {
  id?: string;
  instituteName: string;
  address: string;
  contactNo: string;
  email: string;
  logoPath: string | null;
  isActive: boolean;
}

export interface InstituteList {
  items: Institute[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class InstituteService {
  private http = inject(HttpClient);
  private apiUrl = '/Institute'; // Replace with actual API url if different

  getInstitutes(): Observable<ApiResponse<InstituteList>> {
    return this.http.post<ApiResponse<InstituteList>>(`${this.apiUrl}/get-institute-list`, {});
  }

  getInstitute(id: string): Observable<ApiResponse<Institute>> {
    return this.http.get<ApiResponse<Institute>>(`${this.apiUrl}/${id}`);
  }

  createInstitute(institute: FormData): Observable<ApiResponse<Institute>> {
    return this.http.post<ApiResponse<Institute>>(`${this.apiUrl}/save-institute`, institute);
  }

  updateInstitute(institute: FormData): Observable<ApiResponse<Institute>> {
    return this.http.put<ApiResponse<Institute>>(`${this.apiUrl}/update-institute`, institute);
  }

  deleteInstitute(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-institute/${id}`);
  }
}
