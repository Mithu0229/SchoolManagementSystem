import { Component, signal, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { InputText } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { TableComponent } from '../../../../shared/components/table/table.component';
import {
  TableColumn,
  TableConfig,
} from '../../../../shared/components/table/table.interface';
import { ToastService } from '../../../../core/services/toast.service';
import { PermissionService } from '../../../../core/services/permission.service';
import { Severity } from '../../../../core/enums/toast-severity.enum';
import { TenantService } from '../../../../core/services/tenant.service';

@Component({
  selector: 'app-list-tenant',
  imports: [
    ConfirmDialogComponent,
    TableComponent,
    InputText,
    TextareaModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  templateUrl: './list-tenant.component.html',
  styleUrl: './list-tenant.component.scss',
})
export class TenantManagementComponent {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  showDialog = signal(false);
  selectedTenant = signal<any>(null);

  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  password = '';
  reason = '';

  constructor(
    private readonly route: Router,
    private readonly toastService: ToastService,
    private readonly tenantService: TenantService,
    private readonly permissionService: PermissionService
  ) {}

  ngOnInit() {
    this.initializeTableConfig();
    this.initializeColumns();
  }

  initializeTableConfig() {
    this.tableConfig = {
      pageSize: 10,
      pageSizeOptions: [5, 10, 25],
      showSearch: false,
      searchPlaceholder: 'Search here',
      emptyMessage: 'No tenant available',
      showCreateButton: true,
      createButtonLabel: 'Register New Tenant',
      createButtonPermission: this.permissionService.canAdd(
        'Tenant',
        '/tenant'
      ),
      hideSerial: true,
      customFilters: [
        {
          field: 'IsActive',
          label: 'Filter by status',
          options: [
            { label: 'Active', value: true },
            { label: 'Inactive', value: false },
          ],
          placeholder: 'Select an option',
        },
      ],
    };
  }

  initializeColumns() {
    this.columns = [
      { field: 'tenantName', header: 'Tenant Name', sortable: true },
      { field: 'binNo', header: 'Business Number', sortable: true },
      { field: 'planName', header: 'Subscribed Plan', sortable: true },
      { field: 'tenantEmail', header: 'Tenant Admin', sortable: true },
      { field: 'countryName', header: 'Country', sortable: true },
      { field: 'province', header: 'Province', sortable: true },
      { field: 'city', header: 'City', sortable: true },
      { field: 'phoneNumber', header: 'Phone', sortable: true },
      {
        field: 'isActive',
        header: 'Status',
        sortable: true,
        dataType: 'boolean',
        width: '100px',
      },
    ];

    const hasActionPermissions =
      this.permissionService.canEdit('Tenant', '/tenant') ||
      this.permissionService.canDelete('Tenant', '/tenant');
    if (hasActionPermissions) {
      this.columns.push({
        isActionColumn: true,
        field: 'Actions',
        header: 'Actions',
        actions: [
          {
            label: 'Edit',
            icon: 'pi pi-pencil',
            callback: (row: any) =>
              this.handleAction({ action: 'edit', rowData: row }),
            visible: () => this.permissionService.canEdit('Tenant', '/tenant'),
          },
          {
            label: 'Delete',
            icon: 'pi pi-trash',
            styleClass: 'p-button-danger',
            callback: (row: any) =>
              this.handleAction({ action: 'delete', rowData: row }),
            visible: () =>
              this.permissionService.canDelete('Tenant', '/tenant'),
          },
        ],
      });
    }
  }

  handleAction(event: { action: string; rowData: any }) {
    switch (event.action) {
      case 'edit':
        this.editTenant(event.rowData);
        break;
      case 'delete':
        this.deleteTenant(event.rowData).then();
        break;
    }
  }

  editTenant(tenant: any) {
    this.route.navigate([`/tenant/edit/${tenant.id}`]).then();
  }

  async deleteTenant(tenant: any) {
    this.selectedTenant.set(tenant);
    this.showDialog.set(true);
    // Reset fields when opening the dialog
    this.password = '';
    this.reason = '';
  }

  onDeleteConfirmed() {
    // Validate both fields are required
    if (!this.reason.trim()) {
      this.toastService.error('Reason is required', Severity.ERROR);
      return;
    }

    if (!this.password) {
      this.toastService.error('Password is required', Severity.ERROR);
      return;
    }

    const payload = {
      id: this.selectedTenant()?.id,
      Password: this.password,
      reason: this.reason,
    };

    this.tenantService.deactivateTenant(payload).subscribe({
      next: (response: any) => {
        if (response.isSuccess) {
          this.toastService.success(
            `Tenant "${
              this.selectedTenant()?.tenantName
            }" has been deactivated successfully.`,
            Severity.SUCCESS
          );
          this.showDialog.set(false);
          this.tableComponent.loadData(true);
          this.selectedTenant.set(null);
          this.password = '';
          this.reason = '';
        } else {
          this.toastService.error(
            'Failed to deactivate tenant: ' + (response.errors?.[0] || ''),
            Severity.ERROR
          );
        }
      },
      error: (error: any) => {
        this.toastService.error(
          'Failed to deactivate tenant. Please try again.',
          Severity.ERROR
        );
        console.error('Delete tenant error:', error);
      },
    });
  }

  onCancel() {
    this.showDialog.set(false);
    this.password = '';
    this.reason = '';
  }

  registerTenant() {
    this.route.navigate(['/tenant/register']).then();
  }
}
