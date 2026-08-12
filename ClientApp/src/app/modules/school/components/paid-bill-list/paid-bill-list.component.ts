import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  BillMasterService,
  PaidBillResponse,
} from '../../services/bill-master.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import {
  TableColumn,
  TableConfig,
} from '../../../../shared/components/table/table.interface';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-paid-bill-list',
  standalone: true,
  imports: [
    CommonModule,
    TableComponent,
    ButtonModule,
    DialogModule,
    ToastModule,
  ],
  providers: [MessageService],
  templateUrl: './paid-bill-list.component.html',
  styleUrl: './paid-bill-list.component.scss',
})
export class PaidBillListComponent implements OnInit {
  reportDialog: boolean = false;
  currentReceipt: any = null;

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  private billMasterService = inject(BillMasterService);
  private messageService = inject(MessageService);

  ngOnInit() {
    this.initializeColumns();
    this.initializeTableConfig();
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'studentName', header: 'Student Name', sortable: true },
      { field: 'stdCID', header: 'StdCID', sortable: true },
      { field: 'transactionType', header: 'Transaction Type', sortable: true },
      { field: 'monthName', header: 'Month', sortable: true },
      { field: 'billYear', header: 'Year', sortable: true },
      {
        field: 'totalAmount',
        header: 'Total Amount',
        sortable: true,
        dataType: 'number',
      },
      {
        isActionColumn: true,
        field: 'Actions',
        header: 'Actions',
        actions: [
          {
            label: 'Print Bill',
            icon: 'pi pi-print',
            callback: (row: any) => this.viewReport(row),
            visible: () => true,
          },
        ],
      },
    ];
  }

  initializeTableConfig(): void {
    this.tableConfig = {
      pageSize: 10,
      pageSizeOptions: [5, 10, 25],
      showSearch: true,
      searchPlaceholder: 'Search by student name, StdCID, month or year',
      emptyMessage: 'No paid bills found',
      showCreateButton: false,
      showCheckboxColumn: false,
    };
  }

  viewReport(bill: PaidBillResponse) {
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
      window.location.reload();
    }
  }
}
