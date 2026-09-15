import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { StudentService } from '../../../../../core/services/student.service';

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
export class FeesInfoTabComponent implements OnInit {
  summary: FeeSummary[] = [];
  history: FeeHistory[] = [];
  loading = true;

  constructor(private studentService: StudentService) {}

  ngOnInit(): void {
    const studentId = localStorage.getItem('studentId');
    if (studentId) {
      this.studentService.getFeeInfoByStudent(studentId).subscribe({
        next: (res) => {
          if (res.isSuccess && res.data) {
            this.summary = res.data.summary || [];
            this.history = res.data.history || [];
          }
          this.loading = false;
        },
        error: () => {
          this.loading = false;
        }
      });
    } else {
      this.loading = false;
    }
  }
}
