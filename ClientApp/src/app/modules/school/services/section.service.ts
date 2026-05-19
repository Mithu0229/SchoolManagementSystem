import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface Section {
  id?: string;
  sectionName: string;
  remarks: string;
  isActive: boolean;
}

export interface SectionList {
  items: Section[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class SectionService {
  private http = inject(HttpClient);
  private apiUrl = '/Section';

  getSections(): Observable<ApiResponse<SectionList>> {
    return this.http.post<ApiResponse<SectionList>>(`${this.apiUrl}/get-section-list`, {});
  }

  getSection(id: string): Observable<ApiResponse<Section>> {
    return this.http.get<ApiResponse<Section>>(`${this.apiUrl}/${id}`);
  }

  createSection(section: Section): Observable<ApiResponse<Section>> {
    return this.http.post<ApiResponse<Section>>(`${this.apiUrl}/save-section`, section);
  }

  updateSection(section: Section): Observable<ApiResponse<Section>> {
    return this.http.put<ApiResponse<Section>>(`${this.apiUrl}/update-section`, section);
  }

  deleteSection(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-section/${id}`);
  }

  getSectionDropdown(): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${this.apiUrl}/get-section-dropdown`);
  }
}
