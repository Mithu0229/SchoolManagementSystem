import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from './http.service';
import { ApiResponse } from '../models/api-response';

export interface MonthlyRevenueDto {
  month: string;
  revenue: number;
  expenses: number;
}

export interface ClassDemographicsDto {
  className: string;
  studentCount: number;
}

export interface RecentAdmissionDto {
  id: string;
  studentName: string;
  className: string;
  rollNo: string;
  admissionDate: string;
  status: string;
}

export interface AdminDashboardStats {
  totalStudents: number;
  totalRevenue: number;
  totalAdmissions: number;
  totalTeachers: number;
  totalDue: number;
  revenueOverview: MonthlyRevenueDto[];
  studentDemographics: ClassDemographicsDto[];
  recentAdmissions: RecentAdmissionDto[];
}

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  constructor(private readonly http: HttpService) {}

  getAdminDashboardStats(): Observable<ApiResponse<AdminDashboardStats>> {
    return this.http.get<ApiResponse<AdminDashboardStats>>('Dashboard/admin-stats');
  }
}
