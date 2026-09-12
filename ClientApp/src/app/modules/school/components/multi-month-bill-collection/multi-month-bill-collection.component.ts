import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { BillMasterService, BillMasterResponse, MultiMonthBillCollectionRequest, MultiMonthMoneyReceiptResponse } from '../../services/bill-master.service';

import { FeeHeadService } from '../../services/fee-head.service';
import { EMPTY_GUID } from '../../../../core/constents';

@Component({
  selector: 'app-multi-month-bill-collection',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ButtonModule,
    DialogModule,
    InputTextModule,
    InputNumberModule,
    DropdownModule,
    ToastModule,
  ],
  providers: [MessageService],
  templateUrl: './multi-month-bill-collection.component.html',
  styleUrl: './multi-month-bill-collection.component.scss'
})
export class MultiMonthBillCollectionComponent implements OnInit {
  searchForm!: FormGroup;
  collectionForm!: FormGroup;
  
  unpaidBills: BillMasterResponse[] = [];
  feeHeads: any[] = [];
  studentName: string = '';
  studentId: string = '';
  totalDueAmount: number = 0;
  baseDueAmount: number = 0;
  isSearching: boolean = false;
  isSubmitting: boolean = false;
  
  reportDialog: boolean = false;
  currentReceipt: MultiMonthMoneyReceiptResponse | null = null;
  
  singleReportDialog: boolean = false;
  currentSingleReceipt: any = null;
  
  transactionTypes = [
    { label: 'Cash', value: 1 },
    { label: 'Bank', value: 2 },
    { label: 'Bkash', value: 3 },
  ];
  
  private fb = inject(FormBuilder);
  private billMasterService = inject(BillMasterService);
  private feeHeadService = inject(FeeHeadService);
  private messageService = inject(MessageService);
  
  ngOnInit() {
    this.searchForm = this.fb.group({
      stdCID: ['', Validators.required]
    });
    
    this.collectionForm = this.fb.group({
      collectionAmount: [0, [Validators.required, Validators.min(1)]],
      transactionType: [1, Validators.required],
      bankName: [''],
      accountNo: [''],
      transactionNo: [''],
      voucherNo: [''],
      particulars: [''],
      details: this.fb.array([])
    });

    this.loadFeeHeads();
  }

  loadFeeHeads() {
    this.feeHeadService.getFeeHeadDropdown().subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.feeHeads = res.data || [];
        }
      },
    });
  }

  get details(): FormArray {
    return this.collectionForm.get('details') as FormArray;
  }

  addDetailRow() {
    const newRow = this.fb.group({
      feeHeadId: [null, Validators.required],
      amount: [0, [Validators.required, Validators.min(1)]]
    });
    this.details.push(newRow);
    this.calculateTotal();
  }

  removeDetailRow(index: number) {
    this.details.removeAt(index);
    this.calculateTotal();
  }

  calculateTotal() {
    let extraDetailsTotal = 0;
    this.details.controls.forEach(control => {
      const amt = control.get('amount')?.value || 0;
      extraDetailsTotal += amt;
    });
    this.totalDueAmount = this.baseDueAmount + extraDetailsTotal;
    this.collectionForm.get('collectionAmount')?.setValue(this.totalDueAmount);
  }
  
  searchBills() {
    if (this.searchForm.invalid) return;
    
    this.isSearching = true;
    const stdCID = this.searchForm.get('stdCID')?.value;
    
    this.billMasterService.getBillMasters().subscribe({
      next: (res) => {
        this.isSearching = false;
        if (res.isSuccess && res.data) {
          // Filter by stdCID and not active
          this.unpaidBills = res.data.items.filter(
            x => x.stdCID === stdCID && !x.isActive
          ).sort((a, b) => {
            if (a.billYear !== b.billYear) return a.billYear - b.billYear;
            return a.billMonth - b.billMonth;
          });
          
          if (this.unpaidBills.length > 0) {
            this.studentId = this.unpaidBills[0].admissionId; 
            
            // Calculate base due amount by subtracting what was already partially paid
            this.baseDueAmount = this.unpaidBills.reduce((sum, b) => sum + (b.totalAmount - b.partialAmount), 0);
            this.calculateTotal(); // sets totalDueAmount and updates form
            
            this.messageService.add({ severity: 'success', summary: 'Success', detail: `Found ${this.unpaidBills.length} unpaid bills.` });
          } else {
            this.studentId = '';
            this.baseDueAmount = 0;
            this.calculateTotal();
            this.messageService.add({ severity: 'info', summary: 'Info', detail: 'No unpaid bills found for this student.' });
          }
        }
      },
      error: () => {
        this.isSearching = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to search bills.' });
      }
    });
  }
  
  getMonthName(monthNum: number): string {
    const months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    return months[monthNum - 1] || monthNum.toString();
  }
  
  submitCollection() {
    if (this.collectionForm.invalid || this.unpaidBills.length === 0) return;
    
    const amount = this.collectionForm.get('collectionAmount')?.value;
    if (amount <= 0) {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Collection amount must be greater than 0.' });
      return;
    }
    
    this.isSubmitting = true;
    const formVal = this.collectionForm.getRawValue();
    
    const request: MultiMonthBillCollectionRequest = {
      studentId: this.studentId,
      billMasterIds: this.unpaidBills.map(b => b.id),
      collectionAmount: amount,
      transactionType: formVal.transactionType,
      bankName: formVal.bankName,
      accountNo: formVal.accountNo,
      transactionNo: formVal.transactionNo,
      voucherNo: formVal.voucherNo,
      particulars: formVal.particulars,
      additionalDetails: formVal.details.map((d: any) => ({
        id: EMPTY_GUID,
        feeTemplateDetailId: EMPTY_GUID,
        feeHeadId: d.feeHeadId,
        amount: d.amount
      }))
    };
    
    this.billMasterService.processMultiMonthBill(request).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        if (res.isSuccess && res.data) {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: res.notificationMessage || 'Bills collected successfully.' });
          this.viewReport(res.data.voucherNo);
          this.unpaidBills = []; // clear list
          this.searchForm.reset();
        } else {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: res.errors?.join(', ') || 'Failed to collect bills.' });
        }
      },
      error: () => {
        this.isSubmitting = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to collect bills.' });
      }
    });
  }
  
  viewReport(voucherNo: string) {
    this.billMasterService.getMultiMonthMoneyReceipt(voucherNo).subscribe({
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
  
  viewSingleReport(billId: string) {
    this.billMasterService.getMoneyReceipt(billId).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.currentSingleReceipt = res.data;
          this.singleReportDialog = true;
        } else {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load receipt.' });
        }
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load receipt.' });
      }
    });
  }

  hideSingleReportDialog() {
    this.singleReportDialog = false;
    this.currentSingleReceipt = null;
  }
  
  printReport() {
    const printContent = document.getElementById('print-section');
    if (printContent) {
      const originalContents = document.body.innerHTML;
      document.body.innerHTML = printContent.innerHTML;
      window.print();
      document.body.innerHTML = originalContents;
      window.location.reload();
    }
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
}
