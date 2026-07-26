import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface Admission {
  id?: string;
  admissionDate: string | Date;
  studentId: string;
  branchId: string;
  academicSessionId: string;
  classId: string;
  sectionId?: string;
  shiftId?: string;
  groupId?: string;
  teacherId?: string;
  rollNo: string;
  isPassed: boolean;
  isCancelled: boolean;
  isActive: boolean;
}

export interface AdmissionList {
  items: Admission[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class AdmissionService {
  private http = inject(HttpClient);
  private apiUrl = '/Admission';

  getAdmissions(): Observable<ApiResponse<AdmissionList>> {
    return this.http.post<ApiResponse<AdmissionList>>(`${this.apiUrl}/get-admission-list`, {});
  }

  getAdmission(id: string): Observable<ApiResponse<Admission>> {
    return this.http.get<ApiResponse<Admission>>(`${this.apiUrl}/${id}`);
  }

  getAdmissionDropdown(): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${this.apiUrl}/get-admission-dropdown`);
  }

  createAdmission(admission: Admission): Observable<ApiResponse<Admission>> {
    return this.http.post<ApiResponse<Admission>>(`${this.apiUrl}/save-admission`, admission);
  }

  updateAdmission(admission: Admission): Observable<ApiResponse<Admission>> {
    return this.http.put<ApiResponse<Admission>>(`${this.apiUrl}/update-admission`, admission);
  }

  deleteAdmission(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-admission/${id}`);
  }

  getStudentByStdCID(stdcid: string): Observable<ApiResponse<{ id: string; fullName: string }>> {
    return this.http.get<ApiResponse<{ id: string; fullName: string }>>(`${this.apiUrl}/get-student-by-stdcid/${stdcid}`);
  }
}
