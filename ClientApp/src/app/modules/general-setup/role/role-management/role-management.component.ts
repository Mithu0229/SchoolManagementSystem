import { Component, OnInit, ViewChild, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import {
  TableColumn,
  TableConfig,
} from '../../../../shared/components/table/table.interface';
import { AuthService } from '../../../../core/services/auth.service';
import { ToastService } from '../../../../core/services/toast.service';
import { PermissionService } from '../../../../core/services/permission.service';
import { Severity } from '../../../../core/enums/user-types.enum';

@Component({
  selector: 'app-management',
  imports: [
    FormsModule,
    NgIf,
    InputTextModule,
    TableComponent,
    ConfirmDialogComponent,
  ],
  templateUrl: './role-management.component.html',
  styleUrl: './role-management.component.scss',
})
export class RoleManagementComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  title: string = 'Role Management';

  // Signal for tracking dialog state
  showConfirmDialog = signal(false);
  selectedRole = signal<any>(null);
  deletePassword = '';
  hasAssignedUsers = signal(false);
  assignedUserCount = signal(0);
  retentionDays = signal(0);
  retentionHours = signal(0);
  columns: TableColumn[] = [];
  tableConfig!: TableConfig;

  constructor(
    private router: Router,
    private readonly authService: AuthService,
    private readonly toastService: ToastService,
    private readonly permissionService: PermissionService
  ) {}

  ngOnInit() {
    this.initializeTableConfig();
    this.initializeColumns();
  }

  initializeTableConfig() {
    this.tableConfig = {
      pageSize: 5,
      pageSizeOptions: [5, 10, 25, 50],
      defaultSortField: 'roleName',
      defaultSortOrder: 'asc',
      showSearch: true,
      searchPlaceholder: 'Search roles',
      showCreateButton: true,
      createButtonLabel: 'Create Role',
      createButtonPermission: this.permissionService.canAdd('Roles', '/role'),
      customFilters: [
        {
          field: 'IsActive',
          label: 'Filter by status',
          placeholder: 'Select status',
          options: [
            { label: 'All', value: null },
            { label: 'Active', value: true },
            { label: 'Inactive', value: false },
          ],
        },
      ],
    };
  }

  initializeColumns() {
    this.columns = [
      { field: 'roleName', header: 'Role', sortable: true },
      { field: 'description', header: 'Description', sortable: true },
      { field: 'managedBy', header: 'Managed By', sortable: true },
      {
        field: 'isActive',
        header: 'Status',
        sortable: true,
        dataType: 'boolean',
      },
    ];
    const hasActionPermissions =
      this.permissionService.canEdit('Roles', '/role') ||
      this.permissionService.canDelete('Roles', '/role');
    if (hasActionPermissions) {
      this.columns.push({
        isActionColumn: true,
        field: 'Actions',
        header: 'Actions',
        actions: [
          {
            label: 'Edit',
            icon: 'pi pi-pencil',
            callback: (row: any) => this.editRole(row),
            visible: () => this.permissionService.canEdit('Roles', '/role'),
          },
          {
            label: 'Delete',
            icon: 'pi pi-trash',
            styleClass: 'p-button-danger',
            callback: (row: any) => this.deleteRole(row),
            visible: () => this.permissionService.canDelete('Roles', '/role'),
          },
        ],
      });
    }
  }

  createRole(): void {
    this.router.navigate(['role/create']).then();
  }

  editRole(role: any) {
    this.router
      .navigate(['role/edit', role.id], {
        state: {
          role: {
            id: role.id,
            roleName: role.roleName,
            description: role.description,
            status: role.status,
            tenantId: role.tenantId,
          },
        },
      })
      .then();
  }

  deleteRole(role: any) {
    this.selectedRole.set(role);
    this.deletePassword = '';

    this.getAssignedUserByRoleId(role);
    this.showConfirmDialog.set(true);
  }

  onDeleteConfirmed() {
    if (!this.deletePassword) {
      this.toastService.error('Password is required', Severity.ERROR);
      return;
    }

    var deleteRole = {
      id: this.selectedRole()?.id,
      Password: this.deletePassword,
    };

    this.authService.deleteRole(deleteRole).subscribe({
      next: (response: any) => {
        if (response.isSuccess) {
          if (this.hasAssignedUsers()) {
            this.toastService.success(
              `Role "${
                this.selectedRole()?.roleName
              }" scheduled for deletion. Users will be notified.`,
              Severity.SUCCESS
            );
          } else {
            this.toastService.success(
              `Role "${this.selectedRole()?.roleName}" deleted successfully`,
              Severity.SUCCESS
            );
          }

          // Store reference to the table component before resetting state
          const tableRef = this.tableComponent;

          // Close dialog and reset state
          this.showConfirmDialog.set(false);
          this.selectedRole.set(null);
          this.deletePassword = '';

          // Force table refresh with a delay to allow Angular change detection to complete
          setTimeout(() => {
            if (tableRef) {
              // Reset pagination state
              tableRef.currentState.page = 0;
              tableRef.currentState.first = 0;
              tableRef.loadData();
            }
          }, 100);
        } else {
          this.toastService.error(
            'Failed to delete role: ' + (response.errors?.[0] || ''),
            Severity.ERROR
          );
        }
      },
      error: (error: any) => {
        this.toastService.error(
          'Failed to delete role. Please try again.',
          Severity.ERROR
        );
        console.error('Delete role error:', error);
      },
    });
  }

  onCancel() {
    if (this.selectedRole()?.managedBy !== 'System') {
      this.showConfirmDialog.set(false);
      this.selectedRole.set(null);
      this.deletePassword = '';
    } else {
      if (this.retentionDays() === 0 && this.retentionHours() === 0) {
        this.showConfirmDialog.set(false);
        this.selectedRole.set(null);
        this.deletePassword = '';
      } else {
        if (!this.deletePassword) {
          this.toastService.error('Password is required', Severity.ERROR);
          return;
        }
        const cancelRole = {
          id: this.selectedRole()?.id,
          password: this.deletePassword,
        };
        this.authService.cancelRoleDeletion(cancelRole).subscribe({
          next: (response: any) => {
            if (response.isSuccess) {
              this.toastService.success(
                `Role "${
                  this.selectedRole()?.roleName
                }" cancel role deletion successfully`,
                Severity.SUCCESS
              );

              // Store reference to the table component before resetting state
              const tableRef = this.tableComponent;

              // Close dialog and reset state
              this.showConfirmDialog.set(false);
              this.selectedRole.set(null);
              this.deletePassword = '';

              // Force table refresh
              setTimeout(() => {
                if (tableRef) {
                  tableRef.loadData();
                }
              }, 100);
            } else {
              this.toastService.error(
                'Failed to cancel role deletion: ' +
                  (response.errors?.[0] || ''),
                Severity.ERROR
              );
            }
          },
          error: (error: any) => {
            this.toastService.error(
              'Failed to cancel role deletion. Please try again.',
              Severity.ERROR
            );
            console.error('Cancel role deletion error:', error);
          },
        });
      }
    }
  }

  private getAssignedUserByRoleId(role: any) {
    this.authService.getAssignedUserByRoleId(role.id).subscribe({
      next: (response: any) => {
        if (response.isSuccess && role.managedBy === 'System') {
          const retentionData = response.data.retention;
          const assignedUserRole = response.data.count;
          this.hasAssignedUsers.set(true);
          this.assignedUserCount.set(assignedUserRole);
          if (retentionData) {
            this.retentionDays.set(retentionData.retentionDays);
            this.retentionHours.set(retentionData.retentionHours);
          } else {
            this.retentionDays.set(0);
            this.retentionHours.set(0);
          }
        } else {
          this.assignedUserCount.set(0);
          this.hasAssignedUsers.set(false);
          this.retentionDays.set(0);
          this.retentionHours.set(0);
        }
      },
    });
  }
}
