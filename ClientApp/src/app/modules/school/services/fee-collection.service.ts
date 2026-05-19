import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface FeeCollectionDetail {
  id?: string;
  feeCollectionId: string;
  feeHeadId: string;
  monthNo: string; // Following C# Guid
  yearNo: string;  // Following C# Guid
  feeAmount: number;
  discountAmount: number;
  paidAmount: number;
  dueAmount: number;
}

export interface FeeCollection {
  id?: string;
  collectionDate: string | Date;
  memoNo?: string;
  studentId: string;
  admissionId: string;
  branchId: string;
  financialYearId: string;
  totalAmount: number;
  discountAmount: number;
  paidAmount: number;
  dueAmount: number;
  paymentMode?: string;
  remarks?: string;
  isCancelled: boolean;
  isActive: boolean;
  details: FeeCollectionDetail[];
}

export interface FeeCollectionList {
  items: FeeCollection[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class FeeCollectionService {
  private http = inject(HttpClient);
  private apiUrl = '/FeeCollection';

  getFeeCollections(): Observable<ApiResponse<FeeCollectionList>> {
    return this.http.post<ApiResponse<FeeCollectionList>>(`${this.apiUrl}/get-fee-collection-list`, {});
  }

  getFeeCollection(id: string): Observable<ApiResponse<FeeCollection>> {
    return this.http.get<ApiResponse<FeeCollection>>(`${this.apiUrl}/${id}`);
  }

  getFeeCollectionDropdown(): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${this.apiUrl}/get-fee-collection-dropdown`);
  }

  createFeeCollection(feeCollection: FeeCollection): Observable<ApiResponse<FeeCollection>> {
    return this.http.post<ApiResponse<FeeCollection>>(`${this.apiUrl}/save-fee-collection`, feeCollection);
  }

  updateFeeCollection(feeCollection: FeeCollection): Observable<ApiResponse<FeeCollection>> {
    return this.http.put<ApiResponse<FeeCollection>>(`${this.apiUrl}/update-fee-collection`, feeCollection);
  }

  deleteFeeCollection(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-fee-collection/${id}`);
  }
}
