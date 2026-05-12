import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AcademicClass, AcademicClassService } from '../../services/academic-class.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { TableColumn, TableConfig } from '../../../../shared/components/table/table.interface';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EMPTY_GUID } from '../../../../core/constents';

@Component({
  selector: 'app-academic-class',
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
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './academic-class.component.html',
  styleUrl: './academic-class.component.scss'
})
export class AcademicClassComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  academicClasses: AcademicClass[] = [];
  academicClassDialog: boolean = false;
  academicClassForm: FormGroup;
  isEditMode: boolean = false;
  submitted: boolean = false;

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private fb = inject(FormBuilder);
  private academicClassService = inject(AcademicClassService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  constructor() {
    this.academicClassForm = this.fb.group({
      id: [''],
      className: ['', Validators.required],
      classDetails: [''],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.initializeColumns();
    this.initializeTableConfig();
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'className', header: 'Class Name', sortable: true },
      { field: 'classDetails', header: 'Class Details', sortable: true },
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
          callback: (row) => this.editAcademicClass(row),
          visible: () => true,
        },
        {
          label: 'Delete',
          icon: 'pi pi-trash',
          styleClass: 'p-button-danger',
          callback: (row) => this.deleteAcademicClass(row),
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
      emptyMessage: 'No academic classes found',
      showCreateButton: true,
      showCheckboxColumn: false,
      createButtonLabel: 'Add Academic Class',
    };
  }

  openNew() {
    this.academicClassForm.reset({ isActive: true });
    this.isEditMode = false;
    this.submitted = false;
    this.academicClassDialog = true;
  }

  editAcademicClass(academicClass: AcademicClass) {
    this.academicClassForm.patchValue({
      ...academicClass
    });
    this.isEditMode = true;
    this.submitted = false;
    this.academicClassDialog = true;
  }

  deleteAcademicClass(academicClass: AcademicClass) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + academicClass.className + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (academicClass.id) {
          this.academicClassService.deleteAcademicClass(academicClass.id).subscribe({
            next: () => {
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Academic Class Deleted', life: 3000 });
              this.tableComponent.loadData();
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete academic class' });
            }
          });
        }
      }
    });
  }

  hideDialog() {
    this.academicClassDialog = false;
    this.submitted = false;
  }

  saveAcademicClass() {
    this.submitted = true;

    if (this.academicClassForm.invalid) {
      return;
    }

    const formValue = this.academicClassForm.value;
    const payload: AcademicClass = {
      ...formValue,
      id: formValue.id || EMPTY_GUID
    };

    if (this.isEditMode) {
      this.academicClassService.updateAcademicClass(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Academic Class Updated', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update academic class' });
        }
      });
    } else {
      this.academicClassService.createAcademicClass(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Academic Class Created', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create academic class' });
        }
      });
    }
  }
}
