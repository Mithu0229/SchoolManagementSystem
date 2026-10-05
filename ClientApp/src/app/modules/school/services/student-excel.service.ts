import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface StudentExcelRowError {
  rowNumber: number;
  fullName?: string | null;
  messages: string[];
}

export interface StudentExcelImportedRow {
  rowNumber: number;
  studentId: string;
  stdCID?: string | null;
  fullName?: string | null;
}

export interface StudentExcelImportResponse {
  totalRows: number;
  importedCount: number;
  failedCount: number;
  validationFailed: boolean;
  errors: StudentExcelRowError[];
  imported: StudentExcelImportedRow[];
}

@Injectable({
  providedIn: 'root',
})
export class StudentExcelService {
  private http = inject(HttpClient);
  private apiUrl = '/StudentInfo';

  /** Downloads the sample .xlsx built from the exact save-student fields. */
  downloadSample(): Observable<HttpResponse<Blob>> {
    return this.http.get(`${this.apiUrl}/download-student-sample-excel`, {
      responseType: 'blob',
      observe: 'response',
    });
  }

  /** Uploads a filled student Excel; rows are saved through the standard save-student logic. */
  uploadStudents(
    file: File,
  ): Observable<ApiResponse<StudentExcelImportResponse>> {
    const formData = new FormData();
    formData.append('File', file, file.name);
    return this.http.post<ApiResponse<StudentExcelImportResponse>>(
      `${this.apiUrl}/upload-student-excel`,
      formData,
    );
  }
}
