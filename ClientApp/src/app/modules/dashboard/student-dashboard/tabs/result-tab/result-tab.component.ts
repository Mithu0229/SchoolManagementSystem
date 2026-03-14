import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

interface ResultRow {
  subject: string;
  marks: number;
  grade: string;
  point: number;
}

@Component({
  selector: 'app-student-dashboard-tab-result',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './result-tab.component.html',
  styleUrl: './result-tab.component.scss',
})
export class ResultTabComponent {
  readonly rows: ResultRow[] = [
    { subject: 'English', marks: 84, grade: 'A', point: 4.0 },
    { subject: 'Mathematics', marks: 92, grade: 'A+', point: 5.0 },
    { subject: 'Science', marks: 88, grade: 'A', point: 4.0 },
    { subject: 'Bangla', marks: 81, grade: 'A', point: 4.0 },
  ];
}
