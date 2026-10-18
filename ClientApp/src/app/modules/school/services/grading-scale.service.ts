import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface GradingScaleDetailDto {
  id?: string;
  minMark: number;
  maxMark: number;
  letterGrade: string;
  gradePoint: number;
  remarks?: string;
  orderIndex: number;
}

export interface GradingScaleRequest {
  id?: string;
  scaleName: string;
  description?: string;
  isDefault: boolean;
  details: GradingScaleDetailDto[];
}

export interface GradingScaleResponse {
  id: string;
  scaleName: string;
  description?: string;
  isDefault: boolean;
  details: GradingScaleDetailDto[];
}

@Injectable({
  providedIn: 'root'
})
export class GradingScaleService {
  private http = inject(HttpClient);
  private apiUrl = '/GradingScale';

  getGradingScales(): Observable<ApiResponse<GradingScaleResponse[]>> {
    return this.http.post<ApiResponse<GradingScaleResponse[]>>(`${this.apiUrl}/get-grading-scale-list`, {});
  }

  getGradingScale(id: string): Observable<ApiResponse<GradingScaleResponse>> {
    return this.http.get<ApiResponse<GradingScaleResponse>>(`${this.apiUrl}/${id}`);
  }

  getDefaultGradingScale(): Observable<ApiResponse<GradingScaleResponse>> {
    return this.http.get<ApiResponse<GradingScaleResponse>>(`${this.apiUrl}/get-default`);
  }

  saveGradingScale(scale: GradingScaleRequest): Observable<ApiResponse<GradingScaleResponse>> {
    return this.http.post<ApiResponse<GradingScaleResponse>>(`${this.apiUrl}/save-grading-scale`, scale);
  }

  deleteGradingScale(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-grading-scale/${id}`);
  }
}
