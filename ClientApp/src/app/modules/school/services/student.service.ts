import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface Student {
  id?: string;
  studentCode: string;
  studentName: string;
  dateOfBirth?: string | Date;
  gender?: string;
  bloodGroup?: string;
  mobileNo?: string;
  email?: string;
  dobNo?: string;
  guardianNID?: string;
  fatherName?: string;
  motherName?: string;
  guardianMobileNo?: string;
  presentAddress?: string;
  permanentAddress?: string;
  photoPath?: string;
  isActive: boolean;
}

export interface StudentList {
  items: Student[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private http = inject(HttpClient);
  private apiUrl = '/Student';

  getStudents(): Observable<ApiResponse<StudentList>> {
    return this.http.post<ApiResponse<StudentList>>(`${this.apiUrl}/get-student-list`, {});
  }

  getStudent(id: string): Observable<ApiResponse<Student>> {
    return this.http.get<ApiResponse<Student>>(`${this.apiUrl}/${id}`);
  }

  createStudent(student: Student): Observable<ApiResponse<Student>> {
    return this.http.post<ApiResponse<Student>>(`${this.apiUrl}/save-student`, student);
  }

  updateStudent(student: Student): Observable<ApiResponse<Student>> {
    return this.http.put<ApiResponse<Student>>(`${this.apiUrl}/update-student`, student);
  }

  deleteStudent(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-student/${id}`);
  }

  getStudentDropdown(): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${this.apiUrl}/get-student-dropdown`);
  }
}
