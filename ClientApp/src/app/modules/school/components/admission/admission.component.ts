import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Admission, AdmissionService } from '../../services/admission.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { TableColumn, TableConfig } from '../../../../shared/components/table/table.interface';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DropdownModule } from 'primeng/dropdown';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EMPTY_GUID } from '../../../../core/constents';
import { StudentService } from '../../services/student.service';
import { BranchService } from '../../services/branch.service';
import { AcademicSessionService } from '../../services/academic-session.service';
import { AcademicClassService } from '../../services/academic-class.service';
import { SectionService } from '../../services/section.service';
import { ShiftService } from '../../services/shift.service';
import { StudentGroupService } from '../../services/student-group.service';

@Component({
  selector: 'app-admission',
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
    ConfirmDialogModule,
    DropdownModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './admission.component.html',
  styleUrl: './admission.component.scss'
})
export class AdmissionComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  admissions: Admission[] = [];
  admissionDialog: boolean = false;
  admissionForm: FormGroup;
  isEditMode: boolean = false;
  submitted: boolean = false;

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  students: any[] = [];
  branches: any[] = [];
  academicSessions: any[] = [];
  academicClasses: any[] = [];
  sections: any[] = [];
  shifts: any[] = [];
  studentGroups: any[] = [];

  private fb = inject(FormBuilder);
  private admissionService = inject(AdmissionService);
  private studentService = inject(StudentService);
  private branchService = inject(BranchService);
  private academicSessionService = inject(AcademicSessionService);
  private academicClassService = inject(AcademicClassService);
  private sectionService = inject(SectionService);
  private shiftService = inject(ShiftService);
  private studentGroupService = inject(StudentGroupService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  constructor() {
    this.admissionForm = this.fb.group({
      id: [''],
      admissionDate: [new Date(), Validators.required],
      studentId: [EMPTY_GUID, Validators.required],
      branchId: [EMPTY_GUID, Validators.required],
      academicSessionId: [EMPTY_GUID, Validators.required],
      classId: [EMPTY_GUID, Validators.required],
      sectionId: [EMPTY_GUID],
      shiftId: [EMPTY_GUID],
      groupId: [EMPTY_GUID],
      rollNo: ['', Validators.required],
      isPassed: [false],
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
    this.studentService.getStudentDropdown().subscribe(res => this.students = res.data || []);
    this.branchService.getBranchDropdown().subscribe(res => this.branches = res.data || []);
    this.academicSessionService.getAcademicSessionDropdown().subscribe(res => this.academicSessions = res.data || []);
    this.academicClassService.getAcademicClassDropdown().subscribe(res => this.academicClasses = res.data || []);
    this.sectionService.getSectionDropdown().subscribe(res => this.sections = res.data || []);
    this.shiftService.getShiftDropdown().subscribe(res => this.shifts = res.data || []);
    this.studentGroupService.getStudentGroupDropdown().subscribe(res => this.studentGroups = res.data || []);
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'admissionDate', header: 'Date', sortable: true, dataType: 'date' },
      { field: 'rollNo', header: 'Roll No', sortable: true },
      { field: 'isPassed', header: 'Passed', sortable: true, dataType: 'boolean' },
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
          callback: (row) => this.editAdmission(row),
          visible: () => true,
        },
        {
          label: 'Delete',
          icon: 'pi pi-trash',
          styleClass: 'p-button-danger',
          callback: (row) => this.deleteAdmission(row),
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
      emptyMessage: 'No admissions found',
      showCreateButton: true,
      showCheckboxColumn: false,
      createButtonLabel: 'Add Admission',
    };
  }

  openNew() {
    this.admissionForm.reset({
      admissionDate: new Date(),
      studentId: EMPTY_GUID,
      branchId: EMPTY_GUID,
      academicSessionId: EMPTY_GUID,
      classId: EMPTY_GUID,
      sectionId: EMPTY_GUID,
      shiftId: EMPTY_GUID,
      groupId: EMPTY_GUID,
      isPassed: false,
      isCancelled: false,
      isActive: true
    });
    this.isEditMode = false;
    this.submitted = false;
    this.admissionDialog = true;
  }

  editAdmission(admission: Admission) {
    this.admissionForm.patchValue({
      ...admission,
      admissionDate: admission.admissionDate ? new Date(admission.admissionDate) : null
    });
    this.isEditMode = true;
    this.submitted = false;
    this.admissionDialog = true;
  }

  deleteAdmission(admission: Admission) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this admission?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (admission.id) {
          this.admissionService.deleteAdmission(admission.id).subscribe({
            next: () => {
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Admission Deleted', life: 3000 });
              this.tableComponent.loadData();
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete admission' });
            }
          });
        }
      }
    });
  }

  hideDialog() {
    this.admissionDialog = false;
    this.submitted = false;
  }

  saveAdmission() {
    this.submitted = true;

    if (this.admissionForm.invalid) {
      return;
    }

    const formValue = this.admissionForm.value;
    const payload: Admission = {
      ...formValue,
      id: formValue.id || EMPTY_GUID
    };

    if (this.isEditMode) {
      this.admissionService.updateAdmission(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Admission Updated', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update admission' });
        }
      });
    } else {
      this.admissionService.createAdmission(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Admission Created', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create admission' });
        }
      });
    }
  }
}
