import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DropdownModule } from 'primeng/dropdown';
import {
  FormBuilder,
  FormGroup,
  FormArray,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  BillMasterService,
  BillMasterResponse,
  BillMasterRequest,
  BillDetailRequest,
} from '../../services/bill-master.service';
import { FeeHeadService } from '../../services/fee-head.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import {
  TableColumn,
  TableConfig,
} from '../../../../shared/components/table/table.interface';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EMPTY_GUID } from '../../../../core/constents';

@Component({
  selector: 'app-bill-collection',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableComponent,
    ButtonModule,
    DialogModule,
    DropdownModule,
    InputTextModule,
    InputNumberModule,
    ToastModule,
    ConfirmDialogModule,
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './bill-collection.component.html',
  styleUrl: './bill-collection.component.scss',
})
export class BillCollectionComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  billDialog: boolean = false;
  reportDialog: boolean = false;
  billForm!: FormGroup;
  isSubmitting: boolean = false;
  currentBill: BillMasterResponse | null = null;
  currentReceipt: any = null;
  feeHeads: any[] = [];

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private fb = inject(FormBuilder);
  private billMasterService = inject(BillMasterService);
  private feeHeadService = inject(FeeHeadService);
  private messageService = inject(MessageService);

  ngOnInit() {
    this.initializeColumns();
    this.initializeTableConfig();
    this.loadFeeHeads();
  }

  loadFeeHeads() {
    this.feeHeadService.getFeeHeadDropdown().subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.feeHeads = res.data || [];
        }
      },
    });
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'stdCID', header: 'StdCID', sortable: true },
      { field: 'billMonth', header: 'Month', sortable: true },
      { field: 'billYear', header: 'Year', sortable: true },
      {
        field: 'totalAmount',
        header: 'Total Amount',
        sortable: true,
        dataType: 'number',
      },
      {
        field: 'isActive',
        header: 'Status',
        sortable: true,
        dataType: 'boolean',
      },
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
        },
        {
          label: 'View Report',
          icon: 'pi pi-print',
          callback: (row) => this.viewReport(row),
          visible: () => true,
        },
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
      showCheckboxColumn: false,
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
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to load bill details',
          });
        }
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load bill details',
        });
      },
    });
  }

  buildForm(bill: BillMasterResponse) {
    this.billForm = this.fb.group({
      id: [{ value: bill.id, disabled: true }],
      admissionId: [{ value: bill.admissionId, disabled: true }],
      stdCID: [{ value: bill.stdCID, disabled: true }],
      billMonth: [{ value: bill.billMonth, disabled: true }],
      billYear: [{ value: bill.billYear, disabled: true }],
      totalAmount: [bill.totalAmount],
      isActive: [{ value: bill.isActive, disabled: true }],
      details: this.fb.array(
        bill.details.map((d) =>
          this.fb.group({
            id: [{ value: d.id, disabled: true }],
            billMasterId: [{ value: d.billMasterId, disabled: true }],
            feeTemplateDetailId: [
              { value: d.feeTemplateDetailId, disabled: true },
            ],
            feeHeadId: [{ value: d.feeHeadId, disabled: true }],
            feeHeadName: [{ value: d.feeHeadName, disabled: true }],
            amount: [{ value: d.amount, disabled: true }],
            isEditing: [false],
          }),
        ),
      ),
    });
  }

  get details(): FormArray {
    return this.billForm.get('details') as FormArray;
  }

  addDetailRow() {
    const newRow = this.fb.group({
      id: EMPTY_GUID,
      billMasterId: [this.currentBill?.id],
      feeTemplateDetailId: EMPTY_GUID,
      feeHeadId: [null],
      feeHeadName: [null],
      amount: [null],
      isEditing: [true],
    });
    this.details.push(newRow);
  }

  editDetailRow(index: number) {
    const group = this.details.at(index) as FormGroup;
    group.get('feeHeadId')?.enable();
    group.get('amount')?.enable();
    group.get('isEditing')?.setValue(true);
  }

  saveDetailRow(index: number) {
    const group = this.details.at(index) as FormGroup;
    group.get('feeHeadId')?.disable();
    group.get('amount')?.disable();
    group.get('isEditing')?.setValue(false);
    this.calculateTotal();
  }

  removeDetailRow(index: number) {
    this.details.removeAt(index);
    this.calculateTotal();
  }

  calculateTotal() {
    let total = 0;
    this.details.controls.forEach((control) => {
      const amt = control.get('amount')?.value || 0;
      total += amt;
    });
    this.billForm.get('totalAmount')?.setValue(total);
  }

  hideDialog() {
    this.billDialog = false;
    this.currentBill = null;
  }

  viewReport(bill: BillMasterResponse) {
    this.billMasterService.getMoneyReceipt(bill.id).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.currentReceipt = res.data;
          this.reportDialog = true;
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to load receipt',
          });
        }
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load receipt',
        });
      },
    });
  }

  hideReportDialog() {
    this.reportDialog = false;
    this.currentReceipt = null;
  }

  printReport() {
    const printContent = document.getElementById('print-section');
    if (printContent) {
      const originalContents = document.body.innerHTML;
      document.body.innerHTML = printContent.innerHTML;
      window.print();
      document.body.innerHTML = originalContents;
      window.location.reload(); // Reload to restore angular state after replacing body
    }
  }

  submitBill() {
    if (!this.currentBill) return;

    this.isSubmitting = true;

    const formRaw = this.billForm.getRawValue();
    const request: BillMasterRequest = {
      id: formRaw.id,
      admissionId: formRaw.admissionId,
      billMonth: formRaw.billMonth,
      stdCID: formRaw.stdCID,
      billYear: formRaw.billYear,
      totalAmount: formRaw.totalAmount,
      isActive: formRaw.isActive,
      details: formRaw.details.map(
        (d: any): BillDetailRequest => ({
          id: d.id,
          feeTemplateDetailId: d.feeTemplateDetailId,
          feeHeadId: d.feeHeadId,
          amount: d.amount,
        }),
      ),
    };

    this.billMasterService.updateBillMaster(request).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        if (res.isSuccess) {
          this.messageService.add({
            severity: 'success',
            summary: 'Successful',
            detail: res.notificationMessage || 'Bill updated successfully',
            life: 3000,
          });
          this.tableComponent.loadData();
          this.hideDialog();
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.errors?.join(', ') || 'Failed to update bill',
          });
        }
      },
      error: () => {
        this.isSubmitting = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to update bill',
        });
      },
    });
  }
}
