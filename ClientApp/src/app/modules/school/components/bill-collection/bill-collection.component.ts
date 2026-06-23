import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule } from '@angular/forms';
import {
  BillMasterService,
  BillMasterResponse,
  BillMasterRequest,
  BillDetailRequest
} from '../../services/bill-master.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { TableColumn, TableConfig } from '../../../../shared/components/table/table.interface';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-bill-collection',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableComponent,
    ButtonModule,
    DialogModule,
    InputTextModule,
    InputNumberModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './bill-collection.component.html',
  styleUrl: './bill-collection.component.scss'
})
export class BillCollectionComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  billDialog: boolean = false;
  billForm!: FormGroup;
  isSubmitting: boolean = false;
  currentBill: BillMasterResponse | null = null;

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private fb = inject(FormBuilder);
  private billMasterService = inject(BillMasterService);
  private messageService = inject(MessageService);

  ngOnInit() {
    this.initializeColumns();
    this.initializeTableConfig();
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'admissionRollNo', header: 'Roll No', sortable: true },
      { field: 'billMonth', header: 'Month', sortable: true },
      { field: 'billYear', header: 'Year', sortable: true },
      { field: 'totalAmount', header: 'Total Amount', sortable: true, dataType: 'number' },
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
          callback: (row) => this.editBill(row),
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
      searchPlaceholder: 'Search by roll no, month or year',
      emptyMessage: 'No bills found',
      showCreateButton: false,
      showCheckboxColumn: false
    };
  }

  editBill(bill: BillMasterResponse) {
    this.billMasterService.getBillMasterById(bill.id).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.currentBill = res.data;
          this.buildForm(res.data);
          this.billDialog = true;
        } else {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load bill details' });
        }
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load bill details' });
      }
    });
  }

  buildForm(bill: BillMasterResponse) {
    this.billForm = this.fb.group({
      id: [{ value: bill.id, disabled: true }],
      admissionId: [{ value: bill.admissionId, disabled: true }],
      admissionRollNo: [{ value: bill.admissionRollNo, disabled: true }],
      billMonth: [{ value: bill.billMonth, disabled: true }],
      billYear: [{ value: bill.billYear, disabled: true }],
      totalAmount: [bill.totalAmount],
      isActive: [{ value: bill.isActive, disabled: true }],
      details: this.fb.array(
        bill.details.map(d => this.fb.group({
          id: [{ value: d.id, disabled: true }],
          billMasterId: [{ value: d.billMasterId, disabled: true }],
          feeTemplateDetailId: [{ value: d.feeTemplateDetailId, disabled: true }],
          feeHeadId: [{ value: d.feeHeadId, disabled: true }],
          feeHeadName: [{ value: d.feeHeadName, disabled: true }],
          amount: [d.amount]
        }))
      )
    });
  }

  get details(): FormArray {
    return this.billForm.get('details') as FormArray;
  }

  hideDialog() {
    this.billDialog = false;
    this.currentBill = null;
  }

  submitBill() {
    if (!this.currentBill) return;

    this.isSubmitting = true;

    const formRaw = this.billForm.getRawValue();
    const request: BillMasterRequest = {
      id: formRaw.id,
      admissionId: formRaw.admissionId,
      billMonth: formRaw.billMonth,
      billYear: formRaw.billYear,
      totalAmount: formRaw.totalAmount,
      isActive: formRaw.isActive,
      details: formRaw.details.map((d: any): BillDetailRequest => ({
        id: d.id,
        feeTemplateDetailId: d.feeTemplateDetailId,
        feeHeadId: d.feeHeadId,
        amount: d.amount
      }))
    };

    this.billMasterService.updateBillMaster(request).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        if (res.isSuccess) {
          this.messageService.add({
            severity: 'success',
            summary: 'Successful',
            detail: res.notificationMessage || 'Bill updated successfully',
            life: 3000
          });
          this.tableComponent.loadData();
          this.hideDialog();
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.errors?.join(', ') || 'Failed to update bill'
          });
        }
      },
      error: () => {
        this.isSubmitting = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to update bill'
        });
      }
    });
  }
}
