import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface AcademicSession {
  id?: string;
  sessionName: string;
  fromDate: string | Date;
  toDate: string | Date;
  isCurrent: boolean;
  isActive: boolean;
}

export interface AcademicSessionList {
  items: AcademicSession[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class AcademicSessionService {
  private http = inject(HttpClient);
  private apiUrl = '/AcademicSession';

  getAcademicSessions(): Observable<ApiResponse<AcademicSessionList>> {
    return this.http.post<ApiResponse<AcademicSessionList>>(`${this.apiUrl}/get-academic-session-list`, {});
  }

  getAcademicSession(id: string): Observable<ApiResponse<AcademicSession>> {
    return this.http.get<ApiResponse<AcademicSession>>(`${this.apiUrl}/${id}`);
  }

  createAcademicSession(academicSession: AcademicSession): Observable<ApiResponse<AcademicSession>> {
    return this.http.post<ApiResponse<AcademicSession>>(`${this.apiUrl}/save-academic-session`, academicSession);
  }

  updateAcademicSession(academicSession: AcademicSession): Observable<ApiResponse<AcademicSession>> {
    return this.http.put<ApiResponse<AcademicSession>>(`${this.apiUrl}/update-academic-session`, academicSession);
  }

  deleteAcademicSession(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-academic-session/${id}`);
  }
}
