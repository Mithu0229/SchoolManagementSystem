import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface AcademicClass {
  id?: string;
  className: string;
  classDetails: string;
  isActive: boolean;
}

export interface AcademicClassList {
  items: AcademicClass[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class AcademicClassService {
  private http = inject(HttpClient);
  private apiUrl = '/AcademicClass';

  getAcademicClasses(): Observable<ApiResponse<AcademicClassList>> {
    return this.http.post<ApiResponse<AcademicClassList>>(`${this.apiUrl}/get-academic-class-list`, {});
  }

  getAcademicClass(id: string): Observable<ApiResponse<AcademicClass>> {
    return this.http.get<ApiResponse<AcademicClass>>(`${this.apiUrl}/${id}`);
  }

  createAcademicClass(academicClass: AcademicClass): Observable<ApiResponse<AcademicClass>> {
    return this.http.post<ApiResponse<AcademicClass>>(`${this.apiUrl}/save-academic-class`, academicClass);
  }

  updateAcademicClass(academicClass: AcademicClass): Observable<ApiResponse<AcademicClass>> {
    return this.http.put<ApiResponse<AcademicClass>>(`${this.apiUrl}/update-academic-class`, academicClass);
  }

  deleteAcademicClass(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-academic-class/${id}`);
  }

  getAcademicClassDropdown(): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${this.apiUrl}/get-academic-class-dropdown`);
  }
}
