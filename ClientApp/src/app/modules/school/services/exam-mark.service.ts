import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface ExamComponentDto {
  id: string;
  academicSessionId?: string;
  termName: string;
  componentName: string;
  maxMarks: number;
  orderIndex: number;
}

export interface MarksheetSubjectRowDto {
  subjectId: string;
  subjectName: string;
  ct1?: number;
  ct2?: number;
  halfYearly?: number;
  term1Total: number;
  ct3?: number;
  ct4?: number;
  yearlyExam?: number;
  term2Total: number;
  overallAverage: number;
  grade: string;
  gradePoint: number;
}

export interface MarksheetSummaryRowDto {
  totalCT1: number;
  totalCT2: number;
  totalHalfYearly: number;
  totalTerm1: number;
  totalCT3: number;
  totalCT4: number;
  totalYearlyExam: number;
  totalTerm2: number;
  overallAverage: number;
  finalGrade: string;
  finalGPA: number;
}

export interface GradingScaleLegendItem {
  marksRange: string;
  letterGrade: string;
  gpa: number;
}

export interface StudentMarksheetResponse {
  academicSession: string;
  academicSessionId: string;
  studentName: string;
  studentRoll: string;
  studentId: string;
  className: string;
  classId: string;
  sectionName: string;
  sectionId?: string;
  components: ExamComponentDto[];
  subjects: MarksheetSubjectRowDto[];
  summary: MarksheetSummaryRowDto;
  gradingScaleLegend: GradingScaleLegendItem[];
}

export interface SingleMarkInput {
  examComponentId: string;
  marksObtained: number;
  isAbsent: boolean;
}

export interface SubjectMarkInputRow {
  subjectId: string;
  componentMarks: SingleMarkInput[];
}

export interface SaveStudentMarksRequest {
  academicSessionId: string;
  classId: string;
  sectionId?: string;
  studentId: string;
  subjectMarks: SubjectMarkInputRow[];
}

export interface BatchStudentMarkItem {
  studentId: string;
  studentName: string;
  rollNo: string;
  marksObtained?: number;
  isAbsent: boolean;
}

export interface SaveBatchMarksRequest {
  academicSessionId: string;
  classId: string;
  sectionId?: string;
  subjectId: string;
  examComponentId: string;
  studentMarks: BatchStudentMarkItem[];
}

@Injectable({
  providedIn: 'root'
})
export class ExamMarkService {
  private http = inject(HttpClient);
  private apiUrl = '/ExamMark';

  getExamComponents(sessionId?: string): Observable<ApiResponse<ExamComponentDto[]>> {
    const url = sessionId ? `${this.apiUrl}/get-exam-components?academicSessionId=${sessionId}` : `${this.apiUrl}/get-exam-components`;
    return this.http.get<ApiResponse<ExamComponentDto[]>>(url);
  }

  saveExamComponent(comp: ExamComponentDto): Observable<ApiResponse<boolean>> {
    return this.http.post<ApiResponse<boolean>>(`${this.apiUrl}/save-exam-component`, comp);
  }

  getStudentMarksheet(academicSessionId: string, classId: string, studentId: string, sectionId?: string): Observable<ApiResponse<StudentMarksheetResponse>> {
    return this.http.post<ApiResponse<StudentMarksheetResponse>>(`${this.apiUrl}/get-student-marksheet`, {
      academicSessionId,
      classId,
      sectionId: sectionId || null,
      studentId
    });
  }

  saveStudentMarks(request: SaveStudentMarksRequest): Observable<ApiResponse<boolean>> {
    return this.http.post<ApiResponse<boolean>>(`${this.apiUrl}/save-student-marks`, request);
  }

  getBatchMarks(academicSessionId: string, classId: string, subjectId: string, examComponentId: string, sectionId?: string): Observable<ApiResponse<BatchStudentMarkItem[]>> {
    return this.http.post<ApiResponse<BatchStudentMarkItem[]>>(`${this.apiUrl}/get-batch-marks`, {
      academicSessionId,
      classId,
      sectionId: sectionId || null,
      subjectId,
      examComponentId
    });
  }

  saveBatchMarks(request: SaveBatchMarksRequest): Observable<ApiResponse<boolean>> {
    return this.http.post<ApiResponse<boolean>>(`${this.apiUrl}/save-batch-marks`, request);
  }

  getStudentsByClass(sessionId: string, classId: string, sectionId?: string): Observable<ApiResponse<any[]>> {
    let url = `${this.apiUrl}/get-students-by-class?sessionId=${sessionId}&classId=${classId}`;
    if (sectionId) url += `&sectionId=${sectionId}`;
    return this.http.get<ApiResponse<any[]>>(url);
  }
}
