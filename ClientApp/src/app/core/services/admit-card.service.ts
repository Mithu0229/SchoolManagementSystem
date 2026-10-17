import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/api-response';

export interface StudentAdmitCardDto {
  examType: string;
  examName: string;
  session: string;
  examStartDate: string;
  cutoffMonth: number;
  cutoffYear: number;
  dueAmount: number;
  dueAmountCurrentDate: number;
  dueAmountExamDate: number;
  isEligible: boolean;
  status: string;
  badgeColor: string;
  reason?: string;
}

export interface StudentAdmitCardListResponse {
  studentId: string;
  studentName: string;
  studentCode: string;
  className: string;
  sectionName: string;
  rollNo: string;
  academicYear: string;
  totalUnpaidDueOverall: number;
  admitCards: StudentAdmitCardDto[];
}

@Injectable({
  providedIn: 'root',
})
export class AdmitCardService {
  private http = inject(HttpClient);

  getStudentAdmitCards(
    studentId: string,
    cutoffType: string = 'exam_date'
  ): Observable<ApiResponse<StudentAdmitCardListResponse>> {
    return this.http.get<ApiResponse<StudentAdmitCardListResponse>>(
      `/AdmitCard/get-student-admit-cards/${studentId}?cutoffType=${cutoffType}`
    );
  }

  downloadAdmitCard(
    studentId: string,
    examType: string
  ): Observable<HttpResponse<Blob>> {
    return this.http.get(
      `/AdmitCard/download/${studentId}/${examType}`,
      {
        responseType: 'blob',
        observe: 'response',
      }
    );
  }

  previewAdmitCard(
    studentId: string,
    examType: string
  ): Observable<HttpResponse<Blob>> {
    return this.http.get(
      `/AdmitCard/preview/${studentId}/${examType}`,
      {
        responseType: 'blob',
        observe: 'response',
      }
    );
  }
}
