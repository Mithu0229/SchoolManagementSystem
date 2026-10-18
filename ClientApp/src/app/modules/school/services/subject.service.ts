import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface SubjectItem {
  id?: string;
  subjectName: string;
  subjectCode?: string;
  description?: string;
  isActive: boolean;
}

export interface SubjectList {
  items: SubjectItem[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

export interface ClassSubjectDto {
  classId: string;
  subjectIds: string[];
}

export interface AssignedSubjectDto {
  subjectId: string;
  subjectName: string;
  subjectCode?: string;
  orderIndex: number;
}

@Injectable({
  providedIn: 'root'
})
export class SubjectService {
  private http = inject(HttpClient);
  private apiUrl = '/Subject';

  getSubjects(pagedRequest: any = {}): Observable<ApiResponse<SubjectList>> {
    return this.http.post<ApiResponse<SubjectList>>(`${this.apiUrl}/get-subject-list`, pagedRequest);
  }

  getSubject(id: string): Observable<ApiResponse<SubjectItem>> {
    return this.http.get<ApiResponse<SubjectItem>>(`${this.apiUrl}/${id}`);
  }

  createSubject(subject: SubjectItem): Observable<ApiResponse<SubjectItem>> {
    return this.http.post<ApiResponse<SubjectItem>>(`${this.apiUrl}/save-subject`, subject);
  }

  updateSubject(subject: SubjectItem): Observable<ApiResponse<SubjectItem>> {
    return this.http.put<ApiResponse<SubjectItem>>(`${this.apiUrl}/update-subject`, subject);
  }

  deleteSubject(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-subject/${id}`);
  }

  getSubjectDropdown(): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${this.apiUrl}/get-subject-dropdown`);
  }

  getSubjectsByClass(classId: string): Observable<ApiResponse<AssignedSubjectDto[]>> {
    return this.http.get<ApiResponse<AssignedSubjectDto[]>>(`${this.apiUrl}/get-subjects-by-class/${classId}`);
  }

  assignClassSubjects(dto: ClassSubjectDto): Observable<ApiResponse<boolean>> {
    return this.http.post<ApiResponse<boolean>>(`${this.apiUrl}/assign-class-subjects`, dto);
  }
}
