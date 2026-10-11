import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CashBookReportDto } from '../../models/accounting.model';
import { AccountingService } from '../../services/accounting.service';

@Component({
  selector: 'app-cashbook',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cashbook.component.html',
  styleUrls: ['./cashbook.component.scss'],
  providers: [DatePipe]
})
export class CashbookComponent implements OnInit {

  startDate: string = '';
  endDate: string = '';
  items: CashBookReportDto[] = [];
  isLoading = false;

  constructor(
    private accountingService: AccountingService,
    private datePipe: DatePipe
  ) {
    const today = new Date();
    this.endDate = this.formatDate(today);
    
    // Set startDate to first day of current month by default
    const firstDay = new Date(today.getFullYear(), today.getMonth(), 1);
    this.startDate = this.formatDate(firstDay);
  }

  ngOnInit(): void {
    this.onSearch();
  }

  formatDate(date: Date): string {
    return this.datePipe.transform(date, 'yyyy-MM-dd') || '';
  }

  onSearch(): void {
    if (!this.startDate || !this.endDate) {
      return;
    }

    this.isLoading = true;
    this.accountingService.getCashBookReport(this.startDate, this.endDate).subscribe({
      next: (res) => {
        this.items = res;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }
}
