import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule, TitleCasePipe, DecimalPipe } from '@angular/common';
import { BillMasterService } from '../../services/bill-master.service';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { environment } from '../../../../../environments/environment';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
//import { ConfirmDialogModule } from 'primeng/confirmdialog';
import {
  TableColumn,
  TableConfig,
} from '../../../../shared/components/table/table.interface';

@Component({
  selector: 'app-student-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableComponent,
    ButtonModule,
    DialogModule,
    InputTextModule,
    CheckboxModule,
    ToastModule,
    // ConfirmDialogModule,
  ],
  providers: [MessageService],
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.scss'],
})
export class StudentListComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  tableConfig: any = {
    editForm: true,
    deleteForm: false,
    exportButton: true,
    pdfButton: true,
  };

  columns: TableColumn[] = [
    { field: 'stdCID', header: 'Student Code' },
    { field: 'fullName', header: 'Name' },
    { field: 'studentPhone', header: 'Phone' },
    { field: 'studentEmail', header: 'Email' },
    { field: 'isActive', header: 'Active', dataType: 'boolean' },
  ];

  studentDialog: boolean = false;
  studentForm: FormGroup;
  submitted: boolean = false;

  reportDialog: boolean = false;
  currentReceipt: any = null;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private messageService: MessageService,
    private billMasterService: BillMasterService,
  ) {
    this.studentForm = this.fb.group({
      studentId: [null, Validators.required],
      stdCID: [{ value: '', disabled: true }],
      fullName: [{ value: '', disabled: true }],
      isActive: [false],
      password: [''],
      sendSms: [false],
    });
  }

  ngOnInit(): void {
    // We add the action columns manually so the app-table handles edit events correctly
    this.columns.push({
      // @ts-ignore
      isActionColumn: true,
      field: 'Actions',
      header: 'Actions',
      actions: [
        {
          label: 'Edit',
          icon: 'pi pi-pencil',
          callback: (row: any) => this.openEdit(row),
          visible: () => true,
        },
        {
          label: 'Print Bill',
          icon: 'pi pi-print',
          callback: (row: any) => this.viewReport(row),
          visible: () => true,
        },
        {
          label: 'Summary Report',
          icon: 'pi pi-file',
          callback: (row: any) => this.viewSummaryReport(row),
          visible: () => true,
        },
      ],
    } as any);
  }

  openEdit(student: any) {
    this.submitted = false;
    this.studentForm.patchValue({
      studentId: student.studentId,
      stdCID: student.stdCID,
      fullName: student.fullName,
      isActive: student.isActive,
      password: '',
      sendSms: false,
    });
    this.studentDialog = true;
  }

  hideDialog() {
    this.studentDialog = false;
    this.submitted = false;
  }

  saveStudent() {
    this.submitted = true;

    if (this.studentForm.invalid) {
      return;
    }

    const payload = {
      studentId: this.studentForm.get('studentId')?.value,
      isActive: this.studentForm.get('isActive')?.value,
      password: this.studentForm.get('password')?.value,
      sendSms: this.studentForm.get('sendSms')?.value,
    };

    this.http.put(`/StudentInfo/update-student-user`, payload).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Successful',
          detail: 'Student User Updated',
          life: 3000,
        });
        this.studentDialog = false;
        if (this.tableComponent) {
          this.tableComponent.loadData();
        } else {
          window.location.reload();
        }
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to update student user',
          life: 3000,
        });
      },
    });
  }

  viewReport(student: any) {
    this.billMasterService
      .getStudentPaidBillReport(student.studentId)
      .subscribe({
        next: (res) => {
          if (res.isSuccess && res.data) {
            this.currentReceipt = res.data;
            this.reportDialog = true;
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'No paid bills found for this student',
            });
          }
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to load receipt',
          });
        },
      });
  }

  hideReportDialog() {
    this.reportDialog = false;
    this.currentReceipt = null;
  }

  summaryReportDialog: boolean = false;
  currentSummary: any = null;

  viewSummaryReport(student: any) {
    const payload = {
      search: student.stdCID,
      filters: [],
      page: 1,
      pageSize: 100,
    };

    // First get the list to find the last transaction date
    this.billMasterService.getStudentBillHistoryList(payload).subscribe({
      next: (listRes) => {
        let lastTxDate = 'N/A';
        if (
          listRes.isSuccess &&
          listRes.data &&
          listRes.data.items &&
          listRes.data.items.length > 0
        ) {
          const items = listRes.data.items;
          const withDates = items.filter((i: any) => i.transactionDate);
          if (withDates.length > 0) {
            withDates.sort(
              (a: any, b: any) =>
                new Date(b.transactionDate).getTime() -
                new Date(a.transactionDate).getTime(),
            );
            lastTxDate =
              withDates[0].formattedTransactionDate ||
              withDates[0].transactionDate;
          }
        }

        // Now get the summary
        this.billMasterService.getStudentBillHistorySummary(payload).subscribe({
          next: (summaryRes) => {
            if (summaryRes.isSuccess && summaryRes.data) {
              this.currentSummary = {
                studentName: student.fullName,
                stdCID: student.stdCID,
                phone: student.studentPhone || 'N/A',
                className: student.className || 'N/A',
                date: new Date().toLocaleDateString(),
                totalAmount: summaryRes.data.overallTotalAmount,
                totalPaid: summaryRes.data.overallTotalPaidAmount,
                totalDue: summaryRes.data.overallTotalDueAmount,
                totalPaidCash: summaryRes.data.overallTotalPaidCashAmount,
                totalPaidBkash: summaryRes.data.overallTotalPaidBkashAmount,
                lastTransactionDate: lastTxDate,
              };
              this.summaryReportDialog = true;
            } else {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: 'Could not load summary.',
              });
            }
          },
          error: () =>
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Could not load summary.',
            }),
        });
      },
      error: () =>
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Could not load bill history.',
        }),
    });
  }

  hideSummaryReportDialog() {
    this.summaryReportDialog = false;
    this.currentSummary = null;
  }

  printSummaryReport() {
    const printContent = document.getElementById('print-summary-section');
    if (printContent) {
      const originalContents = document.body.innerHTML;
      document.body.innerHTML = printContent.innerHTML;
      window.print();
      document.body.innerHTML = originalContents;
      window.location.reload();
    }
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
}
