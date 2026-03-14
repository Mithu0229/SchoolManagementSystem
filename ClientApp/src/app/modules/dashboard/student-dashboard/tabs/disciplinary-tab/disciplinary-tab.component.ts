import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

interface DisciplinaryRow {
  date: string;
  category: string;
  status: string;
  note: string;
}

@Component({
  selector: 'app-student-dashboard-tab-disciplinary',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './disciplinary-tab.component.html',
  styleUrl: './disciplinary-tab.component.scss',
})
export class DisciplinaryTabComponent {
  readonly rows: DisciplinaryRow[] = [
    {
      date: '05 Jan 2026',
      category: 'Uniform',
      status: 'Resolved',
      note: 'Checked by class teacher',
    },
    {
      date: '22 Feb 2026',
      category: 'Homework',
      status: 'Resolved',
      note: 'Submitted next day',
    },
  ];
}
