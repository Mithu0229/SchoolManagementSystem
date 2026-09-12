import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { DialogModule } from 'primeng/dialog';
import {
  BillMasterService,
  PaidBillResponse
} from '../../services/bill-master.service';

@Component({
  selector: 'app-student-paid-bills',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ButtonModule,
    InputTextModule,
    ToastModule,
    DialogModule
  ],
  providers: [MessageService],
  templateUrl: './student-paid-bills.component.html',
  styleUrl: './student-paid-bills.component.scss'
})
export class StudentPaidBillsComponent implements OnInit {
  searchForm!: FormGroup;
  paidBills: PaidBillResponse[] = [];
  isSearching: boolean = false;
  studentName: string = '';
  studentId: string = '';
  
  reportDialog: boolean = false;
  currentReceipt: any = null;

  private fb = inject(FormBuilder);
  private billMasterService = inject(BillMasterService);
  private messageService = inject(MessageService);

  ngOnInit() {
    this.searchForm = this.fb.group({
      stdCID: ['', Validators.required]
    });
  }

  searchBills() {
    if (this.searchForm.invalid) return;

    this.isSearching = true;
    const stdCID = this.searchForm.get('stdCID')?.value;

    this.billMasterService.getPaidBillList({ search: stdCID, page: 1, pageSize: 1000 }).subscribe({
      next: (res) => {
        this.isSearching = false;
        if (res.isSuccess && res.data && res.data.items) {
          // Additional filter to be 100% sure it matches stdCID exactly
          this.paidBills = res.data.items.filter((x: any) => 
            x.stdCID && x.stdCID.toLowerCase() === stdCID.toLowerCase()
          );

          if (this.paidBills.length > 0) {
            this.studentName = this.paidBills[0].studentName || '';
            this.studentId = this.paidBills[0].stdCID || '';
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: `Found ${this.paidBills.length} paid bills.`,
            });
          } else {
            this.studentName = '';
            this.studentId = '';
            this.messageService.add({
              severity: 'info',
              summary: 'Info',
              detail: 'No paid bills found for this student.',
            });
          }
        }
      },
      error: () => {
        this.isSearching = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to search bills.',
        });
      },
    });
  }

  viewReport(billId: string) {
    this.billMasterService.getMoneyReceipt(billId).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.currentReceipt = res.data;
          this.reportDialog = true;
        } else {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load receipt.' });
        }
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load receipt.' });
      }
    });
  }

  hideReportDialog() {
    this.reportDialog = false;
    this.currentReceipt = null;
  }

  printSingleReport() {
    const printContent = document.getElementById('single-print-section');
    if (printContent) {
      const originalContents = document.body.innerHTML;
      document.body.innerHTML = printContent.innerHTML;
      window.print();
      document.body.innerHTML = originalContents;
      window.location.reload();
    }
  }

  printAllBills() {
    const printContent = document.getElementById('all-bills-print-section');
    if (printContent) {
      const originalContents = document.body.innerHTML;
      document.body.innerHTML = printContent.innerHTML;
      window.print();
      document.body.innerHTML = originalContents;
      window.location.reload();
    }
  }
}
