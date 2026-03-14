import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/api-response';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  constructor(private readonly http: HttpService) {}

  createStudent(payload: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      'StudentInfo/save-student',
      payload,
    );
  }
  editSitemap(payload: any): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>('sitemap', payload);
  }
  getParentMenuList(): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>('sitemap/get-parent-menu-list');
  }

  getFeatureList(): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>('Sitemap/get-feature-list');
  }

  getStudentById(id: string): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(`StudentInfo/${id}`);
  }

  getRoleById(roleId: string): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(`/Role/${roleId}`);
  }

  updateStudent1(id: string, payload: FormData): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`StudentInfo/${id}`, payload);
  }

  updateStudent(payload: any): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(
      'StudentInfo/update-student',
      payload,
    );
  }
}
