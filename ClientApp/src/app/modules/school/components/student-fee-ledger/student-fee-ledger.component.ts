import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { StudentFeeLedger, StudentFeeLedgerService } from '../../services/student-fee-ledger.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { TableColumn, TableConfig } from '../../../../shared/components/table/table.interface';
import { StudentService } from '../../services/student.service';
import { AdmissionService } from '../../services/admission.service';
import { BranchService } from '../../services/branch.service';
import { AcademicClassService } from '../../services/academic-class.service';
import { FinancialYearService } from '../../services/financial-year.service';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EMPTY_GUID } from '../../../../core/constents';

@Component({
  selector: 'app-student-fee-ledger',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableComponent,
    ButtonModule,
    DialogModule,
    InputTextModule,
    InputNumberModule,
    CalendarModule,
    CheckboxModule,
    DropdownModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './student-fee-ledger.component.html',
  styleUrl: './student-fee-ledger.component.scss'
})
export class StudentFeeLedgerComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  studentFeeLedgers: StudentFeeLedger[] = [];
  studentFeeLedgerDialog: boolean = false;
  studentFeeLedgerForm: FormGroup;
  isEditMode: boolean = false;
  submitted: boolean = false;

  students: any[] = [];
  admissions: any[] = [];
  branches: any[] = [];
  classes: any[] = [];
  financialYears: any[] = [];

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private fb = inject(FormBuilder);
  private studentFeeLedgerService = inject(StudentFeeLedgerService);
  private studentService = inject(StudentService);
  private admissionService = inject(AdmissionService);
  private branchService = inject(BranchService);
  private academicClassService = inject(AcademicClassService);
  private financialYearService = inject(FinancialYearService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  constructor() {
    this.studentFeeLedgerForm = this.fb.group({
      id: [''],
      entryDate: [new Date(), Validators.required],
      studentId: [EMPTY_GUID, Validators.required],
      admissionId: [EMPTY_GUID, Validators.required],
      branchId: [EMPTY_GUID, Validators.required],
      classId: [EMPTY_GUID, Validators.required],
      financialYearId: [EMPTY_GUID, Validators.required],
      monthNo: [1, Validators.required],
      yearNo: [new Date().getFullYear(), Validators.required],
      feeAmount: [0, Validators.required],
      collectionAmount: [0, Validators.required],
      dueAmount: [0, Validators.required],
      memoNo: [''],
      voucherCode: [''],
      remarks: [''],
      isCancelled: [false],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.initializeColumns();
    this.initializeTableConfig();
    this.loadDropdowns();
  }

  loadDropdowns() {
    this.studentService.getStudentDropdown().subscribe(res => {
      if (res.isSuccess) this.students = res.data || [];
    });
    this.admissionService.getAdmissionDropdown().subscribe(res => {
      if (res.isSuccess) this.admissions = res.data || [];
    });
    this.branchService.getBranchDropdown().subscribe(res => {
      if (res.isSuccess) this.branches = res.data || [];
    });
    this.academicClassService.getAcademicClassDropdown().subscribe(res => {
      if (res.isSuccess) this.classes = res.data || [];
    });
    this.financialYearService.getFinancialYearDropdown().subscribe(res => {
      if (res.isSuccess) this.financialYears = res.data || [];
    });
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'entryDate', header: 'Date', sortable: true, dataType: 'date' },
      { field: 'memoNo', header: 'Memo No', sortable: true },
      { field: 'feeAmount', header: 'Fee', sortable: true },
      { field: 'collectionAmount', header: 'Paid', sortable: true },
      { field: 'dueAmount', header: 'Due', sortable: true },
      { field: 'isActive', header: 'Status', sortable: true, dataType: 'boolean' }
    ];

    this.columns.push({
      isActionColumn: true,
      field: 'Actions',
      header: 'Actions',
      actions: [
        {
          label: 'Edit',
          icon: 'pi pi-pencil',
          callback: (row) => this.editStudentFeeLedger(row),
          visible: () => true,
        },
        {
          label: 'Delete',
          icon: 'pi pi-trash',
          styleClass: 'p-button-danger',
          callback: (row) => this.deleteStudentFeeLedger(row),
          visible: () => true,
        }
      ],
    });
  }

  initializeTableConfig(): void {
    this.tableConfig = {
      pageSize: 10,
      pageSizeOptions: [5, 10, 25],
      showSearch: true,
      searchPlaceholder: 'Search here',
      emptyMessage: 'No ledger entries found',
      showCreateButton: true,
      showCheckboxColumn: false,
      createButtonLabel: 'Add Ledger Entry',
    };
  }

  openNew() {
    this.studentFeeLedgerForm.reset({
      entryDate: new Date(),
      studentId: EMPTY_GUID,
      admissionId: EMPTY_GUID,
      branchId: EMPTY_GUID,
      classId: EMPTY_GUID,
      financialYearId: EMPTY_GUID,
      monthNo: 1,
      yearNo: new Date().getFullYear(),
      feeAmount: 0,
      collectionAmount: 0,
      dueAmount: 0,
      isCancelled: false,
      isActive: true
    });
    this.isEditMode = false;
    this.submitted = false;
    this.studentFeeLedgerDialog = true;
  }

  editStudentFeeLedger(studentFeeLedger: StudentFeeLedger) {
    this.studentFeeLedgerForm.patchValue({
      ...studentFeeLedger,
      entryDate: studentFeeLedger.entryDate ? new Date(studentFeeLedger.entryDate) : null
    });
    this.isEditMode = true;
    this.submitted = false;
    this.studentFeeLedgerDialog = true;
  }

  deleteStudentFeeLedger(studentFeeLedger: StudentFeeLedger) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this ledger entry?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (studentFeeLedger.id) {
          this.studentFeeLedgerService.deleteStudentFeeLedger(studentFeeLedger.id).subscribe({
            next: () => {
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Ledger Entry Deleted', life: 3000 });
              this.tableComponent.loadData();
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete ledger entry' });
            }
          });
        }
      }
    });
  }

  hideDialog() {
    this.studentFeeLedgerDialog = false;
    this.submitted = false;
  }

  saveStudentFeeLedger() {
    this.submitted = true;

    if (this.studentFeeLedgerForm.invalid) {
      return;
    }

    const formValue = this.studentFeeLedgerForm.value;
    const payload: StudentFeeLedger = {
      ...formValue,
      id: formValue.id || EMPTY_GUID
    };

    if (this.isEditMode) {
      this.studentFeeLedgerService.updateStudentFeeLedger(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Ledger Entry Updated', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update ledger entry' });
        }
      });
    } else {
      this.studentFeeLedgerService.createStudentFeeLedger(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Ledger Entry Created', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create ledger entry' });
        }
      });
    }
  }
}
