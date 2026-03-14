import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

interface CalendarEvent {
  date: string;
  title: string;
  details: string;
}

@Component({
  selector: 'app-student-dashboard-tab-academic-calendar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './academic-calendar-tab.component.html',
  styleUrl: './academic-calendar-tab.component.scss',
})
export class AcademicCalendarTabComponent {
  readonly events: CalendarEvent[] = [
    {
      date: '18 Mar 2026',
      title: 'Class Test - English',
      details: 'Class test for chapter 4 and 5',
    },
    {
      date: '25 Mar 2026',
      title: 'Science Project Submission',
      details: 'Submit project model and report',
    },
    {
      date: '05 Apr 2026',
      title: 'Parent-Teacher Meeting',
      details: 'Meeting starts at 10:00 AM',
    },
  ];
}
