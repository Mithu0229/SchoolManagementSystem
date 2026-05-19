import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface FeeHead {
  id?: string;
  feeHeadName: string;
  isMonthly: boolean;
  isActive: boolean;
}

export interface FeeHeadList {
  items: FeeHead[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class FeeHeadService {
  private http = inject(HttpClient);
  private apiUrl = '/FeeHead';

  getFeeHeads(): Observable<ApiResponse<FeeHeadList>> {
    return this.http.post<ApiResponse<FeeHeadList>>(`${this.apiUrl}/get-fee-head-list`, {});
  }

  getFeeHead(id: string): Observable<ApiResponse<FeeHead>> {
    return this.http.get<ApiResponse<FeeHead>>(`${this.apiUrl}/${id}`);
  }

  createFeeHead(feeHead: FeeHead): Observable<ApiResponse<FeeHead>> {
    return this.http.post<ApiResponse<FeeHead>>(`${this.apiUrl}/save-fee-head`, feeHead);
  }

  updateFeeHead(feeHead: FeeHead): Observable<ApiResponse<FeeHead>> {
    return this.http.put<ApiResponse<FeeHead>>(`${this.apiUrl}/update-fee-head`, feeHead);
  }

  deleteFeeHead(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-fee-head/${id}`);
  }

  getFeeHeadDropdown(): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${this.apiUrl}/get-fee-head-dropdown`);
  }
}
