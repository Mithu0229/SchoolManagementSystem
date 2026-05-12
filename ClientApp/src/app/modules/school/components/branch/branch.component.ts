import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Branch, BranchService } from '../../services/branch.service';
import { Institute, InstituteService } from '../../services/institute.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { TableColumn, TableConfig } from '../../../../shared/components/table/table.interface';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EMPTY_GUID } from '../../../../core/constents';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-branch',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableComponent,
    ButtonModule,
    DialogModule,
    InputTextModule,
    CheckboxModule,
    DropdownModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './branch.component.html',
  styleUrl: './branch.component.scss'
})
export class BranchComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  branches: Branch[] = [];
  institutes: Institute[] = [];
  branchDialog: boolean = false;
  branchForm: FormGroup;
  isEditMode: boolean = false;
  previewImage: string | ArrayBuffer | null = null;
  selectedFile: File | null = null;
  submitted: boolean = false;

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private fb = inject(FormBuilder);
  private branchService = inject(BranchService);
  private instituteService = inject(InstituteService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  constructor() {
    this.branchForm = this.fb.group({
      id: [''],
      branchName: ['', Validators.required],
      branchAddress: ['', Validators.required],
      contactPerson: ['', Validators.required],
      contactNumber: ['', Validators.required],
      homeThemeImagePath: [''],
      instituteId: ['', Validators.required],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.loadInstitutes();
    this.initializeColumns();
    this.initializeTableConfig();
  }

  loadInstitutes() {
    this.instituteService.getInstitutes().subscribe({
      next: (res) => {
        this.institutes = res.data?.items || [];
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load institutes' });
      }
    });
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'branchName', header: 'Branch Name', sortable: true },
      { field: 'contactPerson', header: 'Contact Person', sortable: true },
      { field: 'contactNumber', header: 'Contact No', sortable: true },
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
          callback: (row) => this.editBranch(row),
          visible: () => true,
        },
        {
          label: 'Delete',
          icon: 'pi pi-trash',
          styleClass: 'p-button-danger',
          callback: (row) => this.deleteBranch(row),
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
      emptyMessage: 'No branches found',
      showCreateButton: true,
      showCheckboxColumn: false,
      createButtonLabel: 'Add Branch',
    };
  }

  openNew() {
    this.branchForm.reset({ isActive: true });
    this.previewImage = null;
    this.selectedFile = null;
    this.isEditMode = false;
    this.submitted = false;
    this.branchDialog = true;
  }

  editBranch(branch: Branch) {
    this.branchForm.patchValue({ ...branch });
    this.previewImage = this.getImageUrl(branch.homeThemeImagePath);
    this.selectedFile = null;
    this.isEditMode = true;
    this.submitted = false;
    this.branchDialog = true;
  }

  deleteBranch(branch: Branch) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + branch.branchName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (branch.id) {
          this.branchService.deleteBranch(branch.id).subscribe({
            next: () => {
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Branch Deleted', life: 3000 });
              this.tableComponent.loadData();
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete branch' });
            }
          });
        }
      }
    });
  }

  hideDialog() {
    this.branchDialog = false;
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

  getImageUrl(imagePath: string | null | undefined): string | null {
    if (!imagePath) {
      return null;
    }

    if (imagePath.startsWith('http') || imagePath.startsWith('data:')) {
      return imagePath;
    }

    const apiBaseUrl = environment.apiUrl.replace(/\/api\/?$/, '');
    return `${apiBaseUrl}${imagePath}`;
  }

  private buildBranchFormData(): FormData {
    const formValue = this.branchForm.value;
    const formData = new FormData();

    formData.append('Id', formValue.id || EMPTY_GUID);
    formData.append('BranchName', formValue.branchName ?? '');
    formData.append('BranchAddress', formValue.branchAddress ?? '');
    formData.append('ContactPerson', formValue.contactPerson ?? '');
    formData.append('ContactNumber', formValue.contactNumber ?? '');
    formData.append('HomeThemeImagePath', formValue.homeThemeImagePath ?? '');
    formData.append('InstituteId', formValue.instituteId ?? '');
    formData.append('IsActive', String(formValue.isActive ?? false));

    if (this.selectedFile) {
      formData.append('HomeThemeImage', this.selectedFile);
    }

    return formData;
  }

  saveBranch() {
    this.submitted = true;

    if (this.branchForm.invalid) {
      return;
    }

    const formData = this.buildBranchFormData();

    if (this.isEditMode) {
      this.branchService.updateBranch(formData).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Branch Updated', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update branch' });
        }
      });
    } else {
      this.branchService.createBranch(formData).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Branch Created', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create branch' });
        }
      });
    }
  }
}
