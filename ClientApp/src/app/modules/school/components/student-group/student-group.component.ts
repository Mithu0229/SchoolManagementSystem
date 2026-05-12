import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { StudentGroup, StudentGroupService } from '../../services/student-group.service';
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
  selector: 'app-student-group',
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
  templateUrl: './student-group.component.html',
  styleUrl: './student-group.component.scss'
})
export class StudentGroupComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  studentGroups: StudentGroup[] = [];
  studentGroupDialog: boolean = false;
  studentGroupForm: FormGroup;
  isEditMode: boolean = false;
  submitted: boolean = false;

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private fb = inject(FormBuilder);
  private studentGroupService = inject(StudentGroupService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  constructor() {
    this.studentGroupForm = this.fb.group({
      id: [''],
      groupName: ['', Validators.required],
      groupDetails: [''],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.initializeColumns();
    this.initializeTableConfig();
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'groupName', header: 'Group Name', sortable: true },
      { field: 'groupDetails', header: 'Details', sortable: true },
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
          callback: (row) => this.editStudentGroup(row),
          visible: () => true,
        },
        {
          label: 'Delete',
          icon: 'pi pi-trash',
          styleClass: 'p-button-danger',
          callback: (row) => this.deleteStudentGroup(row),
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
      emptyMessage: 'No student groups found',
      showCreateButton: true,
      showCheckboxColumn: false,
      createButtonLabel: 'Add Student Group',
    };
  }

  openNew() {
    this.studentGroupForm.reset({ isActive: true });
    this.isEditMode = false;
    this.submitted = false;
    this.studentGroupDialog = true;
  }

  editStudentGroup(studentGroup: StudentGroup) {
    this.studentGroupForm.patchValue({
      ...studentGroup
    });
    this.isEditMode = true;
    this.submitted = false;
    this.studentGroupDialog = true;
  }

  deleteStudentGroup(studentGroup: StudentGroup) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + studentGroup.groupName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (studentGroup.id) {
          this.studentGroupService.deleteStudentGroup(studentGroup.id).subscribe({
            next: () => {
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Student Group Deleted', life: 3000 });
              this.tableComponent.loadData();
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete student group' });
            }
          });
        }
      }
    });
  }

  hideDialog() {
    this.studentGroupDialog = false;
    this.submitted = false;
  }

  saveStudentGroup() {
    this.submitted = true;

    if (this.studentGroupForm.invalid) {
      return;
    }

    const formValue = this.studentGroupForm.value;
    const payload: StudentGroup = {
      ...formValue,
      id: formValue.id || EMPTY_GUID
    };

    if (this.isEditMode) {
      this.studentGroupService.updateStudentGroup(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Student Group Updated', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update student group' });
        }
      });
    } else {
      this.studentGroupService.createStudentGroup(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Student Group Created', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create student group' });
        }
      });
    }
  }
}
