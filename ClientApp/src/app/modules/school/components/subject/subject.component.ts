import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { SubjectService, SubjectItem, AssignedSubjectDto } from '../../services/subject.service';
import { AcademicClassService } from '../../services/academic-class.service';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TabViewModule } from 'primeng/tabview';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
import { MessageService, ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-subject',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ButtonModule,
    DialogModule,
    InputTextModule,
    CheckboxModule,
    ToastModule,
    ConfirmDialogModule,
    TabViewModule,
    DropdownModule,
    MultiSelectModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './subject.component.html',
  styleUrl: './subject.component.scss'
})
export class SubjectComponent implements OnInit {
  private subjectService = inject(SubjectService);
  private classService = inject(AcademicClassService);
  private fb = inject(FormBuilder);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  subjects: SubjectItem[] = [];
  classes: any[] = [];
  selectedClassId: string | null = null;
  assignedSubjects: AssignedSubjectDto[] = [];
  allSubjectOptions: any[] = [];
  selectedSubjectIds: string[] = [];

  dialogVisible: boolean = false;
  subjectForm!: FormGroup;
  isEditMode: boolean = false;
  loading: boolean = false;

  ngOnInit(): void {
    this.initForm();
    this.loadSubjects();
    this.loadClasses();
  }

  initForm(): void {
    this.subjectForm = this.fb.group({
      id: [null],
      subjectName: ['', Validators.required],
      subjectCode: [''],
      description: [''],
      isActive: [true]
    });
  }

  loadSubjects(): void {
    this.loading = true;
    this.subjectService.getSubjects({ page: 1, pageSize: 100 }).subscribe({
      next: (res) => {
        this.loading = false;
        if (res.isSuccess && res.data) {
          this.subjects = res.data.items || [];
          this.allSubjectOptions = this.subjects.map(s => ({
            label: s.subjectName + (s.subjectCode ? ` (${s.subjectCode})` : ''),
            value: s.id
          }));
        }
      },
      error: () => {
        this.loading = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load subjects.' });
      }
    });
  }

  loadClasses(): void {
    this.classService.getAcademicClassDropdown().subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.classes = (res.data || []).map((c: any) => ({
            label: c.name || c.text || c.className,
            value: c.id || c.value
          }));
          if (this.classes.length > 0 && !this.selectedClassId) {
            this.selectedClassId = this.classes[0].value;
            this.onClassChange();
          }
        }
      }
    });
  }

  onClassChange(): void {
    if (!this.selectedClassId) return;
    this.subjectService.getSubjectsByClass(this.selectedClassId).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.assignedSubjects = res.data;
          this.selectedSubjectIds = this.assignedSubjects.map(x => x.subjectId);
        }
      }
    });
  }

  saveClassAssignment(): void {
    if (!this.selectedClassId) {
      this.messageService.add({ severity: 'warn', summary: 'Warning', detail: 'Please select a class.' });
      return;
    }

    this.subjectService.assignClassSubjects({
      classId: this.selectedClassId,
      subjectIds: this.selectedSubjectIds
    }).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Class subjects assigned successfully.' });
          this.onClassChange();
        } else {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: res.notificationMessage || 'Assignment failed.' });
        }
      }
    });
  }

  openNewSubject(): void {
    this.isEditMode = false;
    this.initForm();
    this.dialogVisible = true;
  }

  editSubject(sub: SubjectItem): void {
    this.isEditMode = true;
    this.initForm();
    this.subjectForm.patchValue({
      id: sub.id,
      subjectName: sub.subjectName,
      subjectCode: sub.subjectCode,
      description: sub.description,
      isActive: sub.isActive
    });
    this.dialogVisible = true;
  }

  saveSubject(): void {
    if (this.subjectForm.invalid) {
      this.messageService.add({ severity: 'warn', summary: 'Validation', detail: 'Subject name is required.' });
      return;
    }

    const val: SubjectItem = this.subjectForm.value;
    const req$ = this.isEditMode
      ? this.subjectService.updateSubject(val)
      : this.subjectService.createSubject(val);

    req$.subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Subject saved successfully.' });
          this.dialogVisible = false;
          this.loadSubjects();
        } else {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: res.notificationMessage || 'Save failed.' });
        }
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err.message || 'Save failed.' });
      }
    });
  }

  confirmDelete(sub: SubjectItem): void {
    if (!sub.id) return;
    this.confirmationService.confirm({
      message: `Delete subject "${sub.subjectName}"?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.subjectService.deleteSubject(sub.id!).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Deleted', detail: 'Subject deleted.' });
            this.loadSubjects();
          }
        });
      }
    });
  }
}
