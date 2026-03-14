import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

interface AdmitCardRow {
  exam: string;
  date: string;
  status: string;
}

@Component({
  selector: 'app-student-dashboard-tab-admit-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admit-card-tab.component.html',
  styleUrl: './admit-card-tab.component.scss',
})
export class AdmitCardTabComponent {
  readonly rows: AdmitCardRow[] = [
    { exam: 'Half Yearly Exam', date: '10 Apr 2026', status: 'Available' },
    { exam: 'Final Exam', date: '10 Nov 2026', status: 'Upcoming' },
  ];
}
