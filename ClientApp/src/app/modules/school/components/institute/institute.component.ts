import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Institute, InstituteService } from '../../services/institute.service';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EMPTY_GUID } from '../../../../core/constents';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-institute',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    DialogModule,
    InputTextModule,
    CheckboxModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './institute.component.html',
  styleUrl: './institute.component.scss'
})
export class InstituteComponent implements OnInit {
  institutes: Institute[] = [];
  instituteDialog: boolean = false;
  instituteForm: FormGroup;
  isEditMode: boolean = false;
  previewImage: string | ArrayBuffer | null = null;
  selectedFile: File | null = null;
  submitted: boolean = false;

  private fb = inject(FormBuilder);
  private instituteService = inject(InstituteService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  constructor() {
    this.instituteForm = this.fb.group({
      id: [''],
      instituteName: ['', Validators.required],
      address: ['', Validators.required],
      contactNo: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      logoPath: [''],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.loadInstitutes();
  }

  loadInstitutes() {
    this.instituteService.getInstitutes().subscribe({
      next: (res) => {
        this.institutes = res.data?.items ?? [];
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load institutes' });
      }
    });
  }

  openNew() {
    this.instituteForm.reset({ isActive: true });
    this.previewImage = null;
    this.selectedFile = null;
    this.isEditMode = false;
    this.submitted = false;
    this.instituteDialog = true;
  }

  editInstitute(institute: Institute) {
    this.instituteForm.patchValue({ ...institute });
    this.previewImage = this.getLogoUrl(institute.logoPath);
    this.selectedFile = null;
    this.isEditMode = true;
    this.submitted = false;
    this.instituteDialog = true;
  }

  deleteInstitute(institute: Institute) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + institute.instituteName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (institute.id) {
          this.instituteService.deleteInstitute(institute.id).subscribe({
            next: () => {
              this.institutes = this.institutes.filter((val) => val.id !== institute.id);
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Institute Deleted', life: 3000 });
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete institute' });
            }
          });
        }
      }
    });
  }

  hideDialog() {
    this.instituteDialog = false;
    this.submitted = false;
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = e => this.previewImage = reader.result;
      reader.readAsDataURL(file);
    }
  }

  getLogoUrl(logoPath: string | null | undefined): string | null {
    if (!logoPath) {
      return null;
    }

    if (logoPath.startsWith('http') || logoPath.startsWith('data:')) {
      return logoPath;
    }

    const apiBaseUrl = environment.apiUrl.replace(/\/api\/?$/, '');
    return `${apiBaseUrl}${logoPath}`;
  }

  private buildInstituteFormData(): FormData {
    const formValue = this.instituteForm.value;
    const formData = new FormData();

    formData.append('Id', formValue.id || EMPTY_GUID);
    formData.append('InstituteName', formValue.instituteName ?? '');
    formData.append('Address', formValue.address ?? '');
    formData.append('ContactNo', formValue.contactNo ?? '');
    formData.append('Email', formValue.email ?? '');
    formData.append('LogoPath', formValue.logoPath ?? '');
    formData.append('IsActive', String(formValue.isActive ?? false));

    if (this.selectedFile) {
      formData.append('Logo', this.selectedFile);
    }

    return formData;
  }

  saveInstitute() {
    this.submitted = true;

    if (this.instituteForm.invalid) {
      return;
    }

    const formData = this.buildInstituteFormData();

    if (this.isEditMode) {
      this.instituteService.updateInstitute(formData).subscribe({
        next: (res) => {
          const updated = res.data;
          const index = this.institutes.findIndex(i => i.id === updated?.id);
          if (updated && index !== -1) {
            this.institutes[index] = updated;
          }
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Institute Updated', life: 3000 });
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update institute' });
        }
      });
    } else {
      this.instituteService.createInstitute(formData).subscribe({
        next: (res) => {
          const created = res.data;
          if (created) {
            this.institutes.push(created);
          }
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Institute Created', life: 3000 });
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create institute' });
        }
      });
    }
  }
}
