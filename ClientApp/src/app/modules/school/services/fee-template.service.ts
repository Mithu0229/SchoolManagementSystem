import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface FeeTemplateDetail {
  id?: string;
  feeHeadId: string;
  amount: number;
}

export interface FeeTemplate {
  id?: string;
  templateName: string;
  classId: string;
  groupId?: string;
  shiftId?: string;
  isActive: boolean;
  details: FeeTemplateDetail[];
}

export interface FeeTemplateList {
  items: FeeTemplate[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class FeeTemplateService {
  private http = inject(HttpClient);
  private apiUrl = '/FeeTemplate';

  getFeeTemplates(): Observable<ApiResponse<FeeTemplateList>> {
    return this.http.post<ApiResponse<FeeTemplateList>>(`${this.apiUrl}/get-fee-template-list`, {});
  }

  getFeeTemplate(id: string): Observable<ApiResponse<FeeTemplate>> {
    return this.http.get<ApiResponse<FeeTemplate>>(`${this.apiUrl}/${id}`);
  }

  createFeeTemplate(feeTemplate: FeeTemplate): Observable<ApiResponse<FeeTemplate>> {
    return this.http.post<ApiResponse<FeeTemplate>>(`${this.apiUrl}/save-fee-template`, feeTemplate);
  }

  updateFeeTemplate(feeTemplate: FeeTemplate): Observable<ApiResponse<FeeTemplate>> {
    return this.http.put<ApiResponse<FeeTemplate>>(`${this.apiUrl}/update-fee-template`, feeTemplate);
  }

  deleteFeeTemplate(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-fee-template/${id}`);
  }
}
