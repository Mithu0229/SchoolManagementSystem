import { CommonModule } from '@angular/common';
import { Component, Type, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AcademicCalendarTabComponent } from './tabs/academic-calendar-tab/academic-calendar-tab.component';
import { AdmitCardTabComponent } from './tabs/admit-card-tab/admit-card-tab.component';
import { AttendanceSummaryTabComponent } from './tabs/attendance-summary-tab/attendance-summary-tab.component';
import { ClassRoutineTabComponent } from './tabs/class-routine-tab/class-routine-tab.component';
import { DashboardTabComponent } from './tabs/dashboard-tab/dashboard-tab.component';
import { DisciplinaryTabComponent } from './tabs/disciplinary-tab/disciplinary-tab.component';
import { FeesInfoTabComponent } from './tabs/fees-info-tab/fees-info-tab.component';
import { ResultTabComponent } from './tabs/result-tab/result-tab.component';
import { SmsHistoryTabComponent } from './tabs/sms-history-tab/sms-history-tab.component';
import { AuthService } from '../../../core/services/auth.service';

type TabKey =
  | 'dashboard'
  | 'classRoutine'
  | 'feesInfo'
  | 'result'
  | 'academicCalendar'
  | 'attendanceSummary'
  | 'admitCard'
  | 'disciplinary'
  | 'smsHistory';

interface StudentTab {
  key: TabKey;
  label: string;
}

@Component({
  selector: 'app-student-dashboard',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './student-dashboard.component.html',
  styleUrl: './student-dashboard.component.scss',
})
export class StudentDashboardComponent implements OnInit {
  private authService = inject(AuthService);
  private router = inject(Router);

  studentName: string = '';

  readonly menuItems = ['Dashboard'];

  readonly tabs: StudentTab[] = [
    { key: 'dashboard', label: 'Dashboard' },
    { key: 'classRoutine', label: 'Class Routine' },
    { key: 'feesInfo', label: 'Fees Info' },
    { key: 'result', label: 'Result' },
    { key: 'academicCalendar', label: 'Academic Calendar' },
    { key: 'attendanceSummary', label: 'Attendance Summary' },
    { key: 'admitCard', label: 'Admit Card' },
    { key: 'disciplinary', label: 'Disciplinary' },
    { key: 'smsHistory', label: 'SMS History' },
  ];

  activeTab: TabKey = 'dashboard';

  readonly tabComponentMap: Record<TabKey, Type<unknown>> = {
    dashboard: DashboardTabComponent,
    classRoutine: ClassRoutineTabComponent,
    feesInfo: FeesInfoTabComponent,
    result: ResultTabComponent,
    academicCalendar: AcademicCalendarTabComponent,
    attendanceSummary: AttendanceSummaryTabComponent,
    admitCard: AdmitCardTabComponent,
    disciplinary: DisciplinaryTabComponent,
    smsHistory: SmsHistoryTabComponent,
  };

  setActiveTab(tab: TabKey): void {
    this.activeTab = tab;
  }

  ngOnInit(): void {
    const currentUser = this.authService.getCurrentUser();
    this.studentName = currentUser?.userLoginInfo?.userName;
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login'], { replaceUrl: true });
  }
}
