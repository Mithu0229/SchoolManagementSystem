import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface ProcessBillRequest {
  admissionId: string;
  billMonth: number;
  billYear: number;
}

export interface BillDetailResponse {
  id: string;
  billMasterId: string;
  feeTemplateDetailId: string;
  feeHeadId: string;
  feeHeadName?: string;
  amount: number;
}

export interface BillMasterResponse {
  id: string;
  admissionId: string;
  admissionRollNo?: string;
  stdCID?: string;
  billMonth: number;
  billYear: number;
  totalAmount: number;
  isActive: boolean;
  details: BillDetailResponse[];
}

export interface BillDetailRequest {
  id: string;
  feeTemplateDetailId: string;
  feeHeadId: string;
  amount: number;
}

export interface BillMasterRequest {
  id: string;
  admissionId: string;
  billMonth: number;
  billYear: number;
  totalAmount: number;
  stdCID: string;
  isActive: boolean;
  transactionType: number;
  bankName?: string;
  accountNo?: string;
  transactionNo?: string;
  voucherNo?: string;
  particulars?: string;
  details: BillDetailRequest[];
}

export interface BillMasterList {
  items: BillMasterResponse[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root',
})
export class BillMasterService {
  private http = inject(HttpClient);
  private apiUrl = '/BillMaster';

  processBill(request: ProcessBillRequest): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      `${this.apiUrl}/process-bill`,
      request,
    );
  }

  getBillMasters(): Observable<ApiResponse<BillMasterList>> {
    return this.http.post<ApiResponse<BillMasterList>>(
      `${this.apiUrl}/get-bill-master-list`,
      {},
    );
  }

  getBillMasterById(id: string): Observable<ApiResponse<BillMasterResponse>> {
    return this.http.get<ApiResponse<BillMasterResponse>>(
      `${this.apiUrl}/${id}`,
    );
  }

  updateBillMaster(
    billMaster: BillMasterRequest,
  ): Observable<ApiResponse<BillMasterResponse>> {
    return this.http.put<ApiResponse<BillMasterResponse>>(
      `${this.apiUrl}/update-bill-master`,
      billMaster,
    );
  }

  getMoneyReceipt(id: string): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(
      `${this.apiUrl}/get-money-receipt/${id}`,
    );
  }
}
