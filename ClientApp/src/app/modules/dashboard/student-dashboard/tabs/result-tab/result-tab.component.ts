import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../../../core/services/auth.service';
import { ExamMarkService, StudentMarksheetResponse } from '../../../../school/services/exam-mark.service';
import { AcademicSessionService } from '../../../../school/services/academic-session.service';
import { AcademicClassService } from '../../../../school/services/academic-class.service';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-student-dashboard-tab-result',
  standalone: true,
  imports: [CommonModule, ButtonModule],
  templateUrl: './result-tab.component.html',
  styleUrl: './result-tab.component.scss',
})
export class ResultTabComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private authService = inject(AuthService);
  private markService = inject(ExamMarkService);
  private sessionService = inject(AcademicSessionService);
  private classService = inject(AcademicClassService);

  studentId: string = '';
  marksheetData: StudentMarksheetResponse | null = null;
  loading: boolean = false;

  ngOnInit(): void {
    const qId = this.route.snapshot.queryParams['studentId'];
    const currentAuthUser = this.authService.getCurrentUser();
    const localId = localStorage.getItem('studentId');

    this.studentId =
      qId ||
      localId ||
      currentAuthUser?.userLoginInfo?.studentId ||
      currentAuthUser?.studentId ||
      currentAuthUser?.id ||
      '';

    if (this.studentId) {
      this.loadStudentMarksheet();
    }
  }

  loadStudentMarksheet(): void {
    this.loading = true;
    const currentAuthUser = this.authService.getCurrentUser();
    const defaultSessionId = currentAuthUser?.userLoginInfo?.academicSessionId || currentAuthUser?.academicSessionId;
    const defaultClassId = currentAuthUser?.userLoginInfo?.classId || currentAuthUser?.classId;

    if (defaultSessionId && defaultClassId && this.studentId) {
      this.fetchMarksheet(defaultSessionId, defaultClassId);
      return;
    }

    this.sessionService.getAcademicSessionDropdown().subscribe(sessionRes => {
      const sessionId = defaultSessionId || sessionRes.data?.[0]?.id || sessionRes.data?.[0]?.value;
      this.classService.getAcademicClassDropdown().subscribe(classRes => {
        const classId = defaultClassId || classRes.data?.[0]?.id || classRes.data?.[0]?.value;
        if (this.studentId) {
          this.fetchMarksheet(sessionId || '', classId || '');
        } else {
          this.loading = false;
        }
      });
    });
  }

  private fetchMarksheet(sessionId: string, classId: string): void {
    this.markService.getStudentMarksheet(sessionId, classId, this.studentId).subscribe({
      next: (res) => {
        this.loading = false;
        if (res.isSuccess && res.data) {
          this.marksheetData = res.data;
          if (this.marksheetData.subjects) {
            this.marksheetData.subjects.forEach(row => {
              const anyRow = row as any;
              row.ct1 = row.ct1 ?? anyRow.cT1 ?? anyRow.CT1;
              row.ct2 = row.ct2 ?? anyRow.cT2 ?? anyRow.CT2;
              row.halfYearly = row.halfYearly ?? anyRow.HalfYearly;
              row.ct3 = row.ct3 ?? anyRow.cT3 ?? anyRow.CT3;
              row.ct4 = row.ct4 ?? anyRow.cT4 ?? anyRow.CT4;
              row.yearlyExam = row.yearlyExam ?? anyRow.YearlyExam;
            });
          }
          if (this.marksheetData.summary) {
            const s = this.marksheetData.summary as any;
            s.totalCT1 = s.totalCT1 ?? s.totalCt1 ?? s.TotalCT1;
            s.totalCT2 = s.totalCT2 ?? s.totalCt2 ?? s.TotalCT2;
            s.totalHalfYearly = s.totalHalfYearly ?? s.TotalHalfYearly;
            s.totalCT3 = s.totalCT3 ?? s.totalCt3 ?? s.TotalCT3;
            s.totalCT4 = s.totalCT4 ?? s.totalCt4 ?? s.TotalCT4;
            s.totalYearlyExam = s.totalYearlyExam ?? s.TotalYearlyExam;
          }
        }
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  printReport(): void {
    window.print();
  }
}
