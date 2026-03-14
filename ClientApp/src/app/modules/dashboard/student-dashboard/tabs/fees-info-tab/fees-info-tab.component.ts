import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

interface FeeSummary {
  title: string;
  amount: string;
}

interface FeeHistory {
  month: string;
  amount: string;
  status: string;
}

@Component({
  selector: 'app-student-dashboard-tab-fees-info',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './fees-info-tab.component.html',
  styleUrl: './fees-info-tab.component.scss',
})
export class FeesInfoTabComponent {
  readonly summary: FeeSummary[] = [
    { title: 'Monthly Fee', amount: 'Tk 3,500' },
    { title: 'Exam Fee', amount: 'Tk 1,000' },
    { title: 'Transport', amount: 'Tk 1,200' },
  ];

  readonly history: FeeHistory[] = [
    { month: 'January', amount: 'Tk 5,700', status: 'Paid' },
    { month: 'February', amount: 'Tk 5,700', status: 'Paid' },
    { month: 'March', amount: 'Tk 11,190', status: 'Due' },
  ];
}
