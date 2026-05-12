import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface FinancialYear {
  id?: string;
  finYearName: string;
  fromDate: string | Date;
  toDate: string | Date;
  finCode: number;
  remarks: string;
  isCurrent: boolean;
  isActive: boolean;
}

export interface FinancialYearList {
  items: FinancialYear[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class FinancialYearService {
  private http = inject(HttpClient);
  private apiUrl = '/FinancialYear';

  getFinancialYears(): Observable<ApiResponse<FinancialYearList>> {
    return this.http.post<ApiResponse<FinancialYearList>>(`${this.apiUrl}/get-financial-year-list`, {});
  }

  getFinancialYear(id: string): Observable<ApiResponse<FinancialYear>> {
    return this.http.get<ApiResponse<FinancialYear>>(`${this.apiUrl}/${id}`);
  }

  createFinancialYear(financialYear: FinancialYear): Observable<ApiResponse<FinancialYear>> {
    return this.http.post<ApiResponse<FinancialYear>>(`${this.apiUrl}/save-financial-year`, financialYear);
  }

  updateFinancialYear(financialYear: FinancialYear): Observable<ApiResponse<FinancialYear>> {
    return this.http.put<ApiResponse<FinancialYear>>(`${this.apiUrl}/update-financial-year`, financialYear);
  }

  deleteFinancialYear(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-financial-year/${id}`);
  }
}
