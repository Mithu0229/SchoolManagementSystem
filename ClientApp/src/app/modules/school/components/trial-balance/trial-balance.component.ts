import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TrialBalanceItemDto, TrialBalanceService } from '../../services/trial-balance.service';

@Component({
  selector: 'app-trial-balance',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trial-balance.component.html',
  styleUrls: ['./trial-balance.component.scss'],
  providers: [DatePipe]
})
export class TrialBalanceComponent implements OnInit {

  fromDate: string = '';
  toDate: string = '';
  items: TrialBalanceItemDto[] = [];
  isLoading = false;

  constructor(
    private trialBalanceService: TrialBalanceService,
    private datePipe: DatePipe
  ) {
    const today = new Date();
    this.toDate = this.formatDate(today);
    
    // Set fromDate to first day of current month by default
    const firstDay = new Date(today.getFullYear(), today.getMonth(), 1);
    this.fromDate = this.formatDate(firstDay);
  }

  ngOnInit(): void {
    this.onSearch();
  }

  formatDate(date: Date): string {
    return this.datePipe.transform(date, 'yyyy-MM-dd') || '';
  }

  onSearch(): void {
    if (!this.fromDate || !this.toDate) {
      return;
    }

    this.isLoading = true;
    this.trialBalanceService.getTrialBalance(this.fromDate, this.toDate).subscribe({
      next: (res) => {
        this.items = res.items;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  downloadExcel(): void {
    // Implement if needed
  }
}
