import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TableModule } from 'primeng/table';
import { StudentService } from '../../../../../core/services/student.service';

@Component({
  selector: 'app-sms-history-tab',
  standalone: true,
  imports: [CommonModule, TableModule],
  templateUrl: './sms-history-tab.component.html',
  styleUrl: './sms-history-tab.component.scss'
})
export class SmsHistoryTabComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private studentService = inject(StudentService);

  studentId: string | null = null;
  smsHistory: any[] = [];
  loading: boolean = false;

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.studentId = params['studentId'];
      if (this.studentId) {
        this.loadSmsHistory();
      }
    });
  }

  loadSmsHistory(): void {
    if (!this.studentId) return;
    
    this.loading = true;
    this.studentService.getSmsHistoryByStudent(this.studentId).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.smsHistory = res.data;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching SMS history:', err);
        this.loading = false;
      },
    });
  }
}
