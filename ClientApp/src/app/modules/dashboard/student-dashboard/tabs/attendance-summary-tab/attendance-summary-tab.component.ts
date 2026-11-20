import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StudentService } from '../../../../../core/services/student.service';

interface AttendanceDay {
  date: string;
  firstPunch: string;
  lastPunch: string;
  punchCount: number;
}

interface AttendanceMonth {
  key: string;
  label: string;
  presentDays: number;
  days: AttendanceDay[];
}

@Component({
  selector: 'app-student-dashboard-tab-attendance-summary',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './attendance-summary-tab.component.html',
  styleUrl: './attendance-summary-tab.component.scss',
})
export class AttendanceSummaryTabComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private studentService = inject(StudentService);

  loading = false;
  errorMessage = '';
  months: AttendanceMonth[] = [];
  selectedMonthKey = '';

  get totalPresentDays(): number {
    return this.months.reduce((sum, m) => sum + m.presentDays, 0);
  }

  get selectedMonth(): AttendanceMonth | undefined {
    return this.months.find((m) => m.key === this.selectedMonthKey);
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const studentId = params['studentId'];
      if (studentId) {
        this.load(studentId);
      }
    });
  }

  private load(studentId: string): void {
    this.loading = true;
    this.errorMessage = '';
    this.studentService.getAttendanceSummaryByStudent(studentId).subscribe({
      next: (res) => {
        this.loading = false;
        if (res.isSuccess && Array.isArray(res.data)) {
          this.months = this.groupByMonth(res.data);
          this.selectedMonthKey = this.months[0]?.key ?? '';
        } else {
          this.errorMessage = res.errors?.[0] || 'Failed to load attendance.';
        }
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Failed to load attendance.';
      },
    });
  }

  private groupByMonth(rows: any[]): AttendanceMonth[] {
    const map = new Map<string, AttendanceMonth>();
    for (const r of rows) {
      const d = new Date(r.date);
      const key = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}`;
      let month = map.get(key);
      if (!month) {
        month = {
          key,
          label: d.toLocaleString('en-US', { month: 'long', year: 'numeric' }),
          presentDays: 0,
          days: [],
        };
        map.set(key, month);
      }
      month.presentDays++;
      month.days.push({
        date: r.date,
        firstPunch: r.firstPunch,
        lastPunch: r.lastPunch,
        punchCount: r.punchCount,
      });
    }
    return Array.from(map.values()).sort((a, b) => b.key.localeCompare(a.key));
  }
}
