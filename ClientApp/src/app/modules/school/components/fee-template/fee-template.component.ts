import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormArray } from '@angular/forms';
import { FeeTemplate, FeeTemplateService } from '../../services/fee-template.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { TableColumn, TableConfig } from '../../../../shared/components/table/table.interface';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EMPTY_GUID } from '../../../../core/constents';

@Component({
  selector: 'app-fee-template',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableComponent,
    ButtonModule,
    DialogModule,
    InputTextModule,
    InputNumberModule,
    CheckboxModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './fee-template.component.html',
  styleUrl: './fee-template.component.scss'
})
export class FeeTemplateComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  feeTemplates: FeeTemplate[] = [];
  feeTemplateDialog: boolean = false;
  feeTemplateForm: FormGroup;
  isEditMode: boolean = false;
  submitted: boolean = false;

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private fb = inject(FormBuilder);
  private feeTemplateService = inject(FeeTemplateService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  constructor() {
    this.feeTemplateForm = this.fb.group({
      id: [''],
      templateName: ['', Validators.required],
      classId: [EMPTY_GUID, Validators.required],
      groupId: [EMPTY_GUID],
      shiftId: [EMPTY_GUID],
      isActive: [true],
      details: this.fb.array([])
    });
  }

  get details() {
    return this.feeTemplateForm.get('details') as FormArray;
  }

  addDetail() {
    const detailForm = this.fb.group({
      id: [EMPTY_GUID],
      feeHeadId: [EMPTY_GUID, Validators.required],
      amount: [0, [Validators.required, Validators.min(0)]]
    });
    this.details.push(detailForm);
  }

  removeDetail(index: number) {
    this.details.removeAt(index);
  }

  ngOnInit() {
    this.initializeColumns();
    this.initializeTableConfig();
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'templateName', header: 'Template Name', sortable: true },
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
          callback: (row) => this.editFeeTemplate(row),
          visible: () => true,
        },
        {
          label: 'Delete',
          icon: 'pi pi-trash',
          styleClass: 'p-button-danger',
          callback: (row) => this.deleteFeeTemplate(row),
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
      emptyMessage: 'No templates found',
      showCreateButton: true,
      showCheckboxColumn: false,
      createButtonLabel: 'Add Template',
    };
  }

  openNew() {
    this.feeTemplateForm.reset({ isActive: true, classId: EMPTY_GUID });
    while (this.details.length) {
      this.details.removeAt(0);
    }
    this.isEditMode = false;
    this.submitted = false;
    this.feeTemplateDialog = true;
  }

  editFeeTemplate(feeTemplate: FeeTemplate) {
    this.isEditMode = true;
    this.submitted = false;
    
    this.feeTemplateForm.patchValue({
      id: feeTemplate.id,
      templateName: feeTemplate.templateName,
      classId: feeTemplate.classId,
      groupId: feeTemplate.groupId,
      shiftId: feeTemplate.shiftId,
      isActive: feeTemplate.isActive
    });

    while (this.details.length) {
      this.details.removeAt(0);
    }

    feeTemplate.details?.forEach(detail => {
      this.details.push(this.fb.group({
        id: [detail.id || EMPTY_GUID],
        feeHeadId: [detail.feeHeadId, Validators.required],
        amount: [detail.amount, [Validators.required, Validators.min(0)]]
      }));
    });

    this.feeTemplateDialog = true;
  }

  deleteFeeTemplate(feeTemplate: FeeTemplate) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + feeTemplate.templateName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (feeTemplate.id) {
          this.feeTemplateService.deleteFeeTemplate(feeTemplate.id).subscribe({
            next: () => {
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Template Deleted', life: 3000 });
              this.tableComponent.loadData();
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete template' });
            }
          });
        }
      }
    });
  }

  hideDialog() {
    this.feeTemplateDialog = false;
    this.submitted = false;
  }

  saveFeeTemplate() {
    this.submitted = true;

    if (this.feeTemplateForm.invalid) {
      return;
    }

    const formValue = this.feeTemplateForm.value;
    const payload: FeeTemplate = {
      ...formValue,
      id: formValue.id || EMPTY_GUID
    };

    if (this.isEditMode) {
      this.feeTemplateService.updateFeeTemplate(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Template Updated', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update template' });
        }
      });
    } else {
      this.feeTemplateService.createFeeTemplate(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Template Created', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create template' });
        }
      });
    }
  }
}
