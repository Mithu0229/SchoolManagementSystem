import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FeeHead, FeeHeadService } from '../../services/fee-head.service';
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
  selector: 'app-fee-head',
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
  templateUrl: './fee-head.component.html',
  styleUrl: './fee-head.component.scss'
})
export class FeeHeadComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  feeHeads: FeeHead[] = [];
  feeHeadDialog: boolean = false;
  feeHeadForm: FormGroup;
  isEditMode: boolean = false;
  submitted: boolean = false;

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private fb = inject(FormBuilder);
  private feeHeadService = inject(FeeHeadService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  constructor() {
    this.feeHeadForm = this.fb.group({
      id: [''],
      feeHeadName: ['', Validators.required],
      isMonthly: [false],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.initializeColumns();
    this.initializeTableConfig();
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'feeHeadName', header: 'Fee Head Name', sortable: true },
      { field: 'isMonthly', header: 'Monthly', sortable: true, dataType: 'boolean' },
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
          callback: (row) => this.editFeeHead(row),
          visible: () => true,
        },
        {
          label: 'Delete',
          icon: 'pi pi-trash',
          styleClass: 'p-button-danger',
          callback: (row) => this.deleteFeeHead(row),
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
      emptyMessage: 'No fee heads found',
      showCreateButton: true,
      showCheckboxColumn: false,
      createButtonLabel: 'Add Fee Head',
    };
  }

  openNew() {
    this.feeHeadForm.reset({ isMonthly: false, isActive: true });
    this.isEditMode = false;
    this.submitted = false;
    this.feeHeadDialog = true;
  }

  editFeeHead(feeHead: FeeHead) {
    this.feeHeadForm.patchValue({
      ...feeHead
    });
    this.isEditMode = true;
    this.submitted = false;
    this.feeHeadDialog = true;
  }

  deleteFeeHead(feeHead: FeeHead) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete ' + feeHead.feeHeadName + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (feeHead.id) {
          this.feeHeadService.deleteFeeHead(feeHead.id).subscribe({
            next: () => {
              this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Fee Head Deleted', life: 3000 });
              this.tableComponent.loadData();
            },
            error: () => {
              this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete fee head' });
            }
          });
        }
      }
    });
  }

  hideDialog() {
    this.feeHeadDialog = false;
    this.submitted = false;
  }

  saveFeeHead() {
    this.submitted = true;

    if (this.feeHeadForm.invalid) {
      return;
    }

    const formValue = this.feeHeadForm.value;
    const payload: FeeHead = {
      ...formValue,
      id: formValue.id || EMPTY_GUID
    };

    if (this.isEditMode) {
      this.feeHeadService.updateFeeHead(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Fee Head Updated', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update fee head' });
        }
      });
    } else {
      this.feeHeadService.createFeeHead(payload).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Fee Head Created', life: 3000 });
          this.tableComponent.loadData();
          this.hideDialog();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create fee head' });
        }
      });
    }
  }
}
