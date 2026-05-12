import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface StudentGroup {
  id?: string;
  groupName: string;
  groupDetails?: string;
  isActive: boolean;
}

export interface StudentGroupList {
  items: StudentGroup[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class StudentGroupService {
  private http = inject(HttpClient);
  private apiUrl = '/StudentGroup';

  getStudentGroups(): Observable<ApiResponse<StudentGroupList>> {
    return this.http.post<ApiResponse<StudentGroupList>>(`${this.apiUrl}/get-student-group-list`, {});
  }

  getStudentGroup(id: string): Observable<ApiResponse<StudentGroup>> {
    return this.http.get<ApiResponse<StudentGroup>>(`${this.apiUrl}/${id}`);
  }

  createStudentGroup(studentGroup: StudentGroup): Observable<ApiResponse<StudentGroup>> {
    return this.http.post<ApiResponse<StudentGroup>>(`${this.apiUrl}/save-student-group`, studentGroup);
  }

  updateStudentGroup(studentGroup: StudentGroup): Observable<ApiResponse<StudentGroup>> {
    return this.http.put<ApiResponse<StudentGroup>>(`${this.apiUrl}/update-student-group`, studentGroup);
  }

  deleteStudentGroup(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-student-group/${id}`);
  }
}
