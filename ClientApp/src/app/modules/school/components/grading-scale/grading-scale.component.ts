import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { GradingScaleService, GradingScaleResponse, GradingScaleRequest } from '../../services/grading-scale.service';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-grading-scale',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ButtonModule,
    DialogModule,
    InputTextModule,
    CheckboxModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './grading-scale.component.html',
  styleUrl: './grading-scale.component.scss'
})
export class GradingScaleComponent implements OnInit {
  private gradingScaleService = inject(GradingScaleService);
  private fb = inject(FormBuilder);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  gradingScales: GradingScaleResponse[] = [];
  selectedScale: GradingScaleResponse | null = null;
  dialogVisible: boolean = false;
  scaleForm!: FormGroup;
  isEditMode: boolean = false;
  loading: boolean = false;

  ngOnInit(): void {
    this.initForm();
    this.loadScales();
  }

  initForm(): void {
    this.scaleForm = this.fb.group({
      id: [null],
      scaleName: ['', Validators.required],
      description: [''],
      isDefault: [true],
      details: this.fb.array([])
    });
  }

  get detailsArray(): FormArray {
    return this.scaleForm.get('details') as FormArray;
  }

  createDetailGroup(minMark: number = 0, maxMark: number = 100, letterGrade: string = '', gradePoint: number = 0, remarks: string = ''): FormGroup {
    return this.fb.group({
      id: [null],
      minMark: [minMark, [Validators.required, Validators.min(0)]],
      maxMark: [maxMark, [Validators.required, Validators.max(100)]],
      letterGrade: [letterGrade, Validators.required],
      gradePoint: [gradePoint, [Validators.required, Validators.min(0)]],
      remarks: [remarks],
      orderIndex: [0]
    });
  }

  addDetailRow(minMark: number = 0, maxMark: number = 0, letterGrade: string = '', gradePoint: number = 0, remarks: string = ''): void {
    this.detailsArray.push(this.createDetailGroup(minMark, maxMark, letterGrade, gradePoint, remarks));
  }

  removeDetailRow(index: number): void {
    this.detailsArray.removeAt(index);
  }

  loadScales(): void {
    this.loading = true;
    this.gradingScaleService.getGradingScales().subscribe({
      next: (res) => {
        this.loading = false;
        if (res.isSuccess && res.data) {
          this.gradingScales = res.data;
          if (this.gradingScales.length > 0 && !this.selectedScale) {
            this.selectedScale = this.gradingScales.find(x => x.isDefault) || this.gradingScales[0];
          }
        }
      },
      error: () => {
        this.loading = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load grading scales.' });
      }
    });
  }

  selectScale(scale: GradingScaleResponse): void {
    this.selectedScale = scale;
  }

  openNew(): void {
    this.isEditMode = false;
    this.initForm();
    // Default preset matching standard grading scale
    const presets = [
      { min: 80, max: 100, grade: 'A+', gpa: 5.00 },
      { min: 70, max: 79, grade: 'A', gpa: 4.00 },
      { min: 60, max: 69, grade: 'A-', gpa: 3.50 },
      { min: 50, max: 59, grade: 'B', gpa: 3.00 },
      { min: 40, max: 49, grade: 'C', gpa: 2.00 },
      { min: 33, max: 39, grade: 'D', gpa: 1.00 },
      { min: 0, max: 32, grade: 'F', gpa: 0.00 },
    ];
    this.detailsArray.clear();
    presets.forEach(p => this.addDetailRow(p.min, p.max, p.grade, p.gpa));
    this.dialogVisible = true;
  }

  editScale(scale: GradingScaleResponse): void {
    this.isEditMode = true;
    this.initForm();
    this.scaleForm.patchValue({
      id: scale.id,
      scaleName: scale.scaleName,
      description: scale.description,
      isDefault: scale.isDefault
    });

    this.detailsArray.clear();
    if (scale.details && scale.details.length > 0) {
      scale.details.forEach(d => {
        this.addDetailRow(d.minMark, d.maxMark, d.letterGrade, d.gradePoint, d.remarks || '');
      });
    }
    this.dialogVisible = true;
  }

  saveScale(): void {
    if (this.scaleForm.invalid) {
      this.messageService.add({ severity: 'warn', summary: 'Validation', detail: 'Please provide all required fields.' });
      return;
    }

    const val: GradingScaleRequest = this.scaleForm.value;
    this.gradingScaleService.saveGradingScale(val).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Grading scale saved successfully.' });
          this.dialogVisible = false;
          this.loadScales();
        } else {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: res.notificationMessage || 'Save failed.' });
        }
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err.message || 'Save failed.' });
      }
    });
  }

  confirmDelete(scale: GradingScaleResponse): void {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete "${scale.scaleName}"?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.gradingScaleService.deleteGradingScale(scale.id).subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'Deleted', detail: 'Grading scale deleted.' });
            if (this.selectedScale?.id === scale.id) {
              this.selectedScale = null;
            }
            this.loadScales();
          }
        });
      }
    });
  }
}
