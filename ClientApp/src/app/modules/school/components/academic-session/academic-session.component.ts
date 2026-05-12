import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AcademicSession, AcademicSessionService } from '../../services/academic-session.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { TableColumn, TableConfig } from '../../../../shared/components/table/table.interface';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EMPTY_GUID } from '../../../../core/constents';

@Component({
  selector: 'app-academic-session',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableComponent,
    ButtonModule,
    DialogModule,
    InputTextModule,
    CalendarModule,
    CheckboxModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './academic-session.component.html',
  styleUrl: './academic-session.component.scss'
})
export class AcademicSessionComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  academicSessions: AcademicSession[] = [];
  academicSessionDialog: boolean = false;
  academicSessionForm: FormGroup;
  isEditMode: boolean = false;
  submitted: boolean = false;

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private fb = inject(FormBuilder);
  private academicSessionService = inject(AcademicSessionService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  constructor() {
    this.academicSessionForm = this.fb.group({
      id: [''],
      sessionName: ['', Validators.required],
      fromDate: [null, Validators.required],
      toDate: [null, Validators.required],
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
      { field: 'sessionName', header: 'Session Name', sortable: true },
      { field: 'fromDate', header: 'From Date', sortable: true, dataType: 'date' },
      { field: 'toDate', header: 'To Date', sortable: true, dataType: 'date' },
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
          callback: (row) => this.editAcademicSession(row),
          visible: () => true,
        },
        {
          label: 'Delete',
          icon: 'pi pi-trash',
          styleClass: 'p-button-danger',
          callback: (row) => this.deleteAcademicSession(row),
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
      emptyMessage: 'No academic sessions found',
      showCreateButton: true,
      showCheckboxColumn: false,
      createButtonLabel: 'Add Academic Session',
    };
  }

  openNew() {
    this.academicSessionForm.reset({ isCurrent: false, isActive: true });
    this.isEditMode = false;
    this.submitted = false;
    this.academicSessionDialog = true;
  }

  editAcademicSession(academicSession: AcademicSession) {
    this.academicSessionForm.patchValue({
      ...academicSession,
      fromDate: academicSession.fromDate ? new Date(academicSession.fromDate) : null,
      toDate: academicSession.toDate ? new Date(academicSession.toDate) : null
    });
    this.isEditMode = true;
    this.submitted = false;
    this.academicSessionDialog = true;
  }

  deleteAcademicSession(academicSession: AcademicSession) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + academicSession.sessionName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (academicSession.id) {
          this.academicSessionService.deleteAcademicSession(academicSession.id).subscribe({
            next: () => {
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Academic Session Deleted', life: 3000 });
              this.tableComponent.loadData();
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete academic session' });
            }
          });
        }
      }
    });
  }

  hideDialog() {
    this.academicSessionDialog = false;
    this.submitted = false;
  }

  saveAcademicSession() {
    this.submitted = true;

    if (this.academicSessionForm.invalid) {
      return;
    }

    const formValue = this.academicSessionForm.value;
    const payload: AcademicSession = {
      ...formValue,
      id: formValue.id || EMPTY_GUID
    };

    if (this.isEditMode) {
      this.academicSessionService.updateAcademicSession(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Academic Session Updated', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update academic session' });
        }
      });
    } else {
      this.academicSessionService.createAcademicSession(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Academic Session Created', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create academic session' });
        }
      });
    }
  }
}
