import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

interface AttendanceRow {
  month: string;
  present: number;
  absent: number;
}

@Component({
  selector: 'app-student-dashboard-tab-attendance-summary',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './attendance-summary-tab.component.html',
  styleUrl: './attendance-summary-tab.component.scss',
})
export class AttendanceSummaryTabComponent {
  readonly monthly: AttendanceRow[] = [
    { month: 'January', present: 21, absent: 2 },
    { month: 'February', present: 19, absent: 1 },
    { month: 'March', present: 18, absent: 0 },
  ];
}
