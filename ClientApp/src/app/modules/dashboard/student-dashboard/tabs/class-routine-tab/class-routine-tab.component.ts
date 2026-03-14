import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

interface RoutineRow {
  period: string;
  subject: string;
  time: string;
  room: string;
}

@Component({
  selector: 'app-student-dashboard-tab-class-routine',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './class-routine-tab.component.html',
  styleUrl: './class-routine-tab.component.scss',
})
export class ClassRoutineTabComponent {
  readonly rows: RoutineRow[] = [
    { period: '1st', subject: 'English', time: '08:00 - 08:40', room: '5A' },
    {
      period: '2nd',
      subject: 'Mathematics',
      time: '08:45 - 09:25',
      room: '5A',
    },
    { period: '3rd', subject: 'Science', time: '09:30 - 10:10', room: 'Lab-1' },
    { period: '4th', subject: 'Bangla', time: '10:30 - 11:10', room: '5A' },
  ];
}
