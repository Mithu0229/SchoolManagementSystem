import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BalanceSheetDto } from '../../models/accounting.model';
import { AccountingService } from '../../services/accounting.service';

@Component({
  selector: 'app-balance-sheet',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './balance-sheet.component.html',
  providers: [DatePipe]
})
export class BalanceSheetComponent implements OnInit {

  asOfDate: string = '';
  data: BalanceSheetDto | null = null;
  isLoading = false;

  constructor(
    private accountingService: AccountingService,
    private datePipe: DatePipe
  ) {
    const today = new Date();
    this.asOfDate = this.formatDate(today);
  }

  ngOnInit(): void {
    this.onSearch();
  }

  formatDate(date: Date): string {
    return this.datePipe.transform(date, 'yyyy-MM-dd') || '';
  }

  onSearch(): void {
    if (!this.asOfDate) {
      return;
    }

    this.isLoading = true;
    this.accountingService.getBalanceSheet(this.asOfDate).subscribe({
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
