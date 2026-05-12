import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FinancialYear, FinancialYearService } from '../../services/financial-year.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { TableColumn, TableConfig } from '../../../../shared/components/table/table.interface';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EMPTY_GUID } from '../../../../core/constents';

@Component({
  selector: 'app-financial-year',
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
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './financial-year.component.html',
  styleUrl: './financial-year.component.scss'
})
export class FinancialYearComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  financialYears: FinancialYear[] = [];
  financialYearDialog: boolean = false;
  financialYearForm: FormGroup;
  isEditMode: boolean = false;
  submitted: boolean = false;

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private fb = inject(FormBuilder);
  private financialYearService = inject(FinancialYearService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  constructor() {
    this.financialYearForm = this.fb.group({
      id: [''],
      finYearName: ['', Validators.required],
      fromDate: [null, Validators.required],
      toDate: [null, Validators.required],
      finCode: [0, Validators.required],
      remarks: [''],
      isCurrent: [false],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.initializeColumns();
    this.initializeTableConfig();
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'finYearName', header: 'Year Name', sortable: true },
      { field: 'fromDate', header: 'From Date', sortable: true, dataType: 'date' },
      { field: 'toDate', header: 'To Date', sortable: true, dataType: 'date' },
      { field: 'finCode', header: 'Fin Code', sortable: true },
      { field: 'isCurrent', header: 'Current', sortable: true, dataType: 'boolean' },
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
          callback: (row) => this.editFinancialYear(row),
          visible: () => true,
        },
        {
          label: 'Delete',
          icon: 'pi pi-trash',
          styleClass: 'p-button-danger',
          callback: (row) => this.deleteFinancialYear(row),
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
      emptyMessage: 'No financial years found',
      showCreateButton: true,
      showCheckboxColumn: false,
      createButtonLabel: 'Add Financial Year',
    };
  }

  openNew() {
    this.financialYearForm.reset({ finCode: 0, isCurrent: false, isActive: true });
    this.isEditMode = false;
    this.submitted = false;
    this.financialYearDialog = true;
  }

  editFinancialYear(financialYear: FinancialYear) {
    this.financialYearForm.patchValue({
      ...financialYear,
      fromDate: financialYear.fromDate ? new Date(financialYear.fromDate) : null,
      toDate: financialYear.toDate ? new Date(financialYear.toDate) : null
    });
    this.isEditMode = true;
    this.submitted = false;
    this.financialYearDialog = true;
  }

  deleteFinancialYear(financialYear: FinancialYear) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + financialYear.finYearName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (financialYear.id) {
          this.financialYearService.deleteFinancialYear(financialYear.id).subscribe({
            next: () => {
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Financial Year Deleted', life: 3000 });
              this.tableComponent.loadData();
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete financial year' });
            }
          });
        }
      }
    });
  }

  hideDialog() {
    this.financialYearDialog = false;
    this.submitted = false;
  }

  saveFinancialYear() {
    this.submitted = true;

    if (this.financialYearForm.invalid) {
      return;
    }

    const formValue = this.financialYearForm.value;
    const payload: FinancialYear = {
      ...formValue,
      id: formValue.id || EMPTY_GUID
    };

    if (this.isEditMode) {
      this.financialYearService.updateFinancialYear(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Financial Year Updated', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update financial year' });
        }
      });
    } else {
      this.financialYearService.createFinancialYear(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Financial Year Created', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create financial year' });
        }
      });
    }
  }
}
