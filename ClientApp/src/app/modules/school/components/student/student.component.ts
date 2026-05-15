import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Student, StudentService } from '../../services/student.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { TableColumn, TableConfig } from '../../../../shared/components/table/table.interface';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EMPTY_GUID } from '../../../../core/constents';

@Component({
  selector: 'app-student',
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
    DropdownModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './student.component.html',
  styleUrl: './student.component.scss'
})
export class StudentComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  students: Student[] = [];
  studentDialog: boolean = false;
  studentForm: FormGroup;
  isEditMode: boolean = false;
  submitted: boolean = false;

  genders = [
    { label: 'Male', value: 'Male' },
    { label: 'Female', value: 'Female' },
    { label: 'Other', value: 'Other' }
  ];

  bloodGroups = [
    { label: 'A+', value: 'A+' },
    { label: 'A-', value: 'A-' },
    { label: 'B+', value: 'B+' },
    { label: 'B-', value: 'B-' },
    { label: 'AB+', value: 'AB+' },
    { label: 'AB-', value: 'AB-' },
    { label: 'O+', value: 'O+' },
    { label: 'O-', value: 'O-' }
  ];

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private fb = inject(FormBuilder);
  private studentService = inject(StudentService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  constructor() {
    this.studentForm = this.fb.group({
      id: [''],
      studentCode: ['', Validators.required],
      studentName: ['', Validators.required],
      dateOfBirth: [null],
      gender: [''],
      bloodGroup: [''],
      mobileNo: [''],
      email: ['', Validators.email],
      dobNo: [''],
      guardianNID: [''],
      fatherName: [''],
      motherName: [''],
      guardianMobileNo: [''],
      presentAddress: [''],
      permanentAddress: [''],
      photoPath: [''],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.initializeColumns();
    this.initializeTableConfig();
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'studentCode', header: 'Code', sortable: true },
      { field: 'studentName', header: 'Name', sortable: true },
      { field: 'mobileNo', header: 'Mobile', sortable: true },
      { field: 'gender', header: 'Gender', sortable: true },
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
          callback: (row) => this.editStudent(row),
          visible: () => true,
        },
        {
          label: 'Delete',
          icon: 'pi pi-trash',
          styleClass: 'p-button-danger',
          callback: (row) => this.deleteStudent(row),
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
      emptyMessage: 'No students found',
      showCreateButton: true,
      showCheckboxColumn: false,
      createButtonLabel: 'Add Student',
    };
  }

  openNew() {
    this.studentForm.reset({ isActive: true });
    this.isEditMode = false;
    this.submitted = false;
    this.studentDialog = true;
  }

  editStudent(student: Student) {
    this.studentForm.patchValue({
      ...student,
      dateOfBirth: student.dateOfBirth ? new Date(student.dateOfBirth) : null
    });
    this.isEditMode = true;
    this.submitted = false;
    this.studentDialog = true;
  }

  deleteStudent(student: Student) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + student.studentName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (student.id) {
          this.studentService.deleteStudent(student.id).subscribe({
            next: () => {
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Student Deleted', life: 3000 });
              this.tableComponent.loadData();
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete student' });
            }
          });
        }
      }
    });
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

    const formValue = this.studentForm.value;
    const payload: Student = {
      ...formValue,
      id: formValue.id || EMPTY_GUID
    };

    if (this.isEditMode) {
      this.studentService.updateStudent(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Student Updated', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update student' });
        }
      });
    } else {
      this.studentService.createStudent(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Student Created', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create student' });
        }
      });
    }
  }
}
