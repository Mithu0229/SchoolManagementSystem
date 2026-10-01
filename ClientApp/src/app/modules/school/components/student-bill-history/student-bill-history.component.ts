import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import {
  BillMasterService,
  StudentBillHistoryResponse,
  StudentBillHistorySummaryResponse,
} from '../../services/bill-master.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import {
  TableColumn,
  TableConfig,
} from '../../../../shared/components/table/table.interface';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-student-bill-history',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TableComponent,
    ButtonModule,
    InputTextModule,
    DropdownModule,
    DialogModule,
    ToastModule,
    TagModule,
    TooltipModule,
  ],
  providers: [MessageService],
  templateUrl: './student-bill-history.component.html',
  styleUrl: './student-bill-history.component.scss',
})
export class StudentBillHistoryComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  apiUrl = '/BillMaster/get-student-bill-history-list';
  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  // KPI Summary
  summary: StudentBillHistorySummaryResponse = {
    totalBillsCount: 0,
    overallTotalAmount: 0,
    overallTotalPaidAmount: 0,
    overallTotalDueAmount: 0,
    overallTotalPaidCashAmount: 0,
    overallTotalPaidBkashAmount: 0,
  };
  isLoadingSummary = false;

  // Filter Form
  filterForm!: FormGroup;

  // Filter options
  months = [
    { label: 'All Months', value: null },
    { label: 'January', value: 1 },
    { label: 'February', value: 2 },
    { label: 'March', value: 3 },
    { label: 'April', value: 4 },
    { label: 'May', value: 5 },
    { label: 'June', value: 6 },
    { label: 'July', value: 7 },
    { label: 'August', value: 8 },
    { label: 'September', value: 9 },
    { label: 'October', value: 10 },
    { label: 'November', value: 11 },
    { label: 'December', value: 12 },
  ];

  years: { label: string; value: number | null }[] = [];

  statusOptions = [
    { label: 'All Statuses', value: null },
    { label: 'Fully Paid', value: 'paid' },
    { label: 'Partially Paid', value: 'partial' },
    { label: 'Due / Unpaid', value: 'unpaid' },
  ];

  // Receipt Dialog
  reportDialog = false;
  currentReceipt: any = null;

  private fb = inject(FormBuilder);
  private billMasterService = inject(BillMasterService);
  private messageService = inject(MessageService);

  ngOnInit() {
    this.initFilterForm();
    this.initYearOptions();
    this.initializeColumns();
    this.initializeTableConfig();
    this.loadSummary();
  }

  private initFilterForm() {
    this.filterForm = this.fb.group({
      search: [''],
      month: [null],
      year: [null],
      paymentStatus: [null],
    });
  }

  private initYearOptions() {
    const currentYear = new Date().getFullYear();
    this.years = [{ label: 'All Years', value: null }];
    for (let y = currentYear + 1; y >= currentYear - 5; y--) {
      this.years.push({ label: y.toString(), value: y });
    }
  }

  initializeColumns(): void {
    this.columns = [
      {
        field: 'studentName',
        header: 'Student Name',
        sortable: true,
      },
      {
        field: 'stdCID',
        header: 'StdCID',
        sortable: true,
      },
      {
        field: 'monthName',
        header: 'Month',
        sortable: true,
      },
      {
        field: 'billYear',
        header: 'Year',
        sortable: true,
      },
      {
        field: 'totalAmount',
        header: 'Total Amount',
        sortable: true,
        dataType: 'number',
      },
      {
        field: 'totalPaidAmount',
        header: 'Total Paid Amount',
        sortable: true,
        dataType: 'number',
      },
      {
        field: 'totalDueAmount',
        header: 'Total Due Amount',
        sortable: true,
        dataType: 'number',
      },
      {
        field: 'totalPaidBkashAmount',
        header: 'Total Paid bKash',
        sortable: true,
        dataType: 'number',
      },
      {
        field: 'totalPaidCashAmount',
        header: 'Total Paid Cash',
        sortable: true,
        dataType: 'number',
      },
      {
        field: 'formattedTransactionDate',
        header: 'Transaction Date',
        sortable: true,
      },
      {
        field: 'paymentStatus',
        header: 'Status',
        sortable: false,
      },
      {
        isActionColumn: true,
        field: 'Actions',
        header: 'Receipt',
        actions: [
          {
            label: 'Receipt',
            icon: 'pi pi-file-pdf',
            callback: (row: StudentBillHistoryResponse) => this.viewReceipt(row),
            visible: (row: StudentBillHistoryResponse) => row.totalPaidAmount > 0,
          },
        ],
      },
    ];
  }

  initializeTableConfig(): void {
    this.tableConfig = {
      pageSize: 10,
      pageSizeOptions: [10, 25, 50, 100],
      showSearch: true,
      searchPlaceholder: 'Search by Student Name, StdCID, Month, Year...',
      emptyMessage: 'No student bill history records found.',
      showCreateButton: false,
      showCheckboxColumn: false,
    };
  }

  loadSummary() {
    this.isLoadingSummary = true;
    const formVal = this.filterForm.value;
    const filters: any[] = [];

    if (formVal.month) {
      filters.push({ field: 'billmonth', value: formVal.month });
    }
    if (formVal.year) {
      filters.push({ field: 'billyear', value: formVal.year });
    }

    const payload = {
      search: formVal.search || '',
      filters: filters,
    };

    this.billMasterService.getStudentBillHistorySummary(payload).subscribe({
      next: (res) => {
        this.isLoadingSummary = false;
        if (res.isSuccess && res.data) {
          this.summary = res.data;
        }
      },
      error: () => {
        this.isLoadingSummary = false;
      },
    });
  }

  applyFilters() {
    if (!this.tableComponent) return;

    const formVal = this.filterForm.value;
    if (formVal.search !== undefined) {
      this.tableComponent.currentState.searchQuery = formVal.search;
    }

    this.tableComponent.currentState.customFilters = {};

    if (formVal.month) {
      this.tableComponent.currentState.customFilters['billmonth'] = formVal.month;
    }
    if (formVal.year) {
      this.tableComponent.currentState.customFilters['billyear'] = formVal.year;
    }
    if (formVal.paymentStatus) {
      this.tableComponent.currentState.customFilters['paymentstatus'] =
        formVal.paymentStatus;
    }

    this.tableComponent.currentState.page = 0;
    this.tableComponent.loadData();
    this.loadSummary();
  }

  resetFilters() {
    this.filterForm.reset({
      search: '',
      month: null,
      year: null,
      paymentStatus: null,
    });

    if (this.tableComponent) {
      this.tableComponent.resetFilters();
    }
    this.loadSummary();
  }

  viewReceipt(bill: StudentBillHistoryResponse) {
    this.billMasterService.getMoneyReceipt(bill.id).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.currentReceipt = res.data;
          this.reportDialog = true;
        } else {
          this.messageService.add({
            severity: 'warn',
            summary: 'Notice',
            detail: res.notificationMessage || 'No receipt details available for this bill.',
          });
        }
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load receipt details.',
        });
      },
    });
  }

  hideReportDialog() {
    this.reportDialog = false;
    this.currentReceipt = null;
  }

  printReceipt() {
    const printContent = document.getElementById('history-print-section');
    if (printContent) {
      const originalContents = document.body.innerHTML;
      document.body.innerHTML = printContent.innerHTML;
      window.print();
      document.body.innerHTML = originalContents;
      window.location.reload();
    }
  }
}
