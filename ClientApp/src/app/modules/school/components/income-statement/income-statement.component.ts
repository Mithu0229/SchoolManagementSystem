import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IncomeStatementDto } from '../../models/accounting.model';
import { AccountingService } from '../../services/accounting.service';

@Component({
  selector: 'app-income-statement',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './income-statement.component.html',
  providers: [DatePipe]
})
export class IncomeStatementComponent implements OnInit {

  fromDate: string = '';
  toDate: string = '';
  data: IncomeStatementDto | null = null;
  isLoading = false;

  constructor(
    private accountingService: AccountingService,
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
    this.accountingService.getIncomeStatement(this.fromDate, this.toDate).subscribe({
      next: (res) => {
        this.data = res;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }
}
