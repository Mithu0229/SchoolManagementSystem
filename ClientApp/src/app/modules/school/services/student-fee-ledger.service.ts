import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface StudentFeeLedger {
  id?: string;
  entryDate: string | Date;
  studentId: string;
  admissionId: string;
  branchId: string;
  classId: string;
  financialYearId: string;
  monthNo: number;
  yearNo: number;
  feeAmount: number;
  collectionAmount: number;
  dueAmount: number;
  memoNo?: string;
  voucherCode?: string;
  remarks?: string;
  isCancelled: boolean;
  isActive: boolean;
}

export interface StudentFeeLedgerList {
  items: StudentFeeLedger[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class StudentFeeLedgerService {
  private http = inject(HttpClient);
  private apiUrl = '/StudentFeeLedger';

  getStudentFeeLedgers(): Observable<ApiResponse<StudentFeeLedgerList>> {
    return this.http.post<ApiResponse<StudentFeeLedgerList>>(`${this.apiUrl}/get-student-fee-ledger-list`, {});
  }

  getStudentFeeLedger(id: string): Observable<ApiResponse<StudentFeeLedger>> {
    return this.http.get<ApiResponse<StudentFeeLedger>>(`${this.apiUrl}/${id}`);
  }

  createStudentFeeLedger(studentFeeLedger: StudentFeeLedger): Observable<ApiResponse<StudentFeeLedger>> {
    return this.http.post<ApiResponse<StudentFeeLedger>>(`${this.apiUrl}/save-student-fee-ledger`, studentFeeLedger);
  }

  updateStudentFeeLedger(studentFeeLedger: StudentFeeLedger): Observable<ApiResponse<StudentFeeLedger>> {
    return this.http.put<ApiResponse<StudentFeeLedger>>(`${this.apiUrl}/update-student-fee-ledger`, studentFeeLedger);
  }

  deleteStudentFeeLedger(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-student-fee-ledger/${id}`);
  }
}
