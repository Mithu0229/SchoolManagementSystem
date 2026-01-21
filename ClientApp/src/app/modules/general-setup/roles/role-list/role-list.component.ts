import { Component, OnInit, signal, ViewChild } from '@angular/core';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import {
  TableColumn,
  TableConfig,
} from '../../../../shared/components/table/table.interface';
import { ToastService } from '../../../../core/services/toast.service';
import { Router } from '@angular/router';
import { DivisionService } from '../../../../core/services/division.service';

@Component({
  selector: 'app-role-list',
  imports: [TableComponent],
  templateUrl: './role-list.component.html',
  styleUrl: './role-list.component.scss',
})
export class RoleListComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  selectedDivision = signal<any>(null);
  showDialog = signal(false);
  columns: TableColumn[] = [];

  tableConfig!: TableConfig;

  constructor(
    private readonly divisionService: DivisionService,
    private readonly toastService: ToastService,
    private readonly route: Router
  ) //private readonly permissionService: PermissionService
  {}

  ngOnInit(): void {
    this.initializeColumns();
    this.initializeTableConfig();
  }

  initializeColumns(): void {
    // Add the standard columns first
    this.columns = [
      { field: 'name', header: 'Role Name', sortable: true },
      {
        field: 'description',
        header: 'Description',
        sortable: true,
        dataType: 'long-text',
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
          callback: (row) =>
            this.handleAction({ action: 'edit', rowData: row }),
          visible: (row) => true,
        },
        {
          label: 'Delete',
          icon: 'pi pi-trash',
          styleClass: 'p-button-danger',
          callback: (row) =>
            this.handleAction({ action: 'delete', rowData: row }),
            visible: (row) => true,
        },
      ],
    });
  }

  initializeTableConfig(): void {
    this.tableConfig = {
      pageSize: 10,
      pageSizeOptions: [5, 10, 25],
      showSearch: true,
      searchPlaceholder: 'Search here',
      emptyMessage: 'No role available',
      showCreateButton: true,
      createButtonLabel: 'Create Role',
      
    };
  }

  handleAction(event: { action: string; rowData: any }) {
    switch (event.action) {
      case 'edit':
        this.editRole(event.rowData);
        break;
      case 'delete':
        this.deleteRole(event.rowData);
        break;
    }
  }

  editRole(role: any) {
    this.route
      .navigate(['/role/edit', role.id], {
        state: {
          role: {
            ...role,
          },
        },
      })
      .then(() => {
        console.log('Navigation to edit role successful');
      });
  }

  async deleteRole(role: any) {
    // if (this.permissionService.canDelete('Divisions', '/division')) {
    //     console.log('Delete division:', division);
    //     this.selectedDivision.set(division);
    //     this.showDialog.set(true);
    // }
  }

  onDeleteConfirmed() {
    console.log('deleted');
    if (this.selectedDivision())
      this.divisionService
        .deleteDivision(this?.selectedDivision()?.id ?? '')
        .subscribe({
          next: (res) => {
            if (res.isSuccess) {
              this.toastService.success(
                res.notificationMessage,
                'Delete Success'
              );
              this.tableComponent.loadData();
            } else {
              this.toastService.error(res.errors?.[0], 'Delete Failed');
            }
          },
        });
    this.showDialog.set(false);
    this.selectedDivision.set(null);
  }

  onCancel() {
    this.showDialog.set(false);
  }

  createRole() {
    this.route.navigate(['/role/create']);
  }
}
