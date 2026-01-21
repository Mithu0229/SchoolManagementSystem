import { Component, OnInit, signal, ViewChild } from '@angular/core';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { TableColumn, TableConfig } from '../../../../shared/components/table/table.interface';
import { ToastService } from '../../../../core/services/toast.service';
import { Router } from '@angular/router';
import { DivisionService } from '../../../../core/services/division.service';

@Component({
  selector: 'app-division-list',
  imports: [TableComponent, ConfirmDialogComponent],
  templateUrl: './division-list.component.html',
  styleUrl: './division-list.component.scss'
})

export class DivisionListComponent implements OnInit {
    @ViewChild(TableComponent) tableComponent!: TableComponent;

    selectedDivision = signal<any>(null);
    showDialog = signal(false);
    columns: TableColumn[] = [];

    tableConfig!: TableConfig;

    constructor(
        private readonly divisionService: DivisionService,
        private readonly toastService: ToastService,
        private readonly route: Router,
        //private readonly permissionService: PermissionService
    ) {}

    ngOnInit(): void {
        this.initializeColumns();
        this.initializeTableConfig();
    }

    initializeColumns(): void {
        // Add the standard columns first
        this.columns = [
            { field: 'name', header: 'Division Name', sortable: true },
            { field: 'description', header: 'Description', sortable: true, dataType: 'long-text' },
        ];
        
        // Only add the action column if the user has permissions
        const hasActionPermissions = true;//this.permissionService.canEdit('Divisions', '/division') || 
                                    //this.permissionService.canDelete('Divisions', '/division');
        
        if (hasActionPermissions) {
            this.columns.push({
                isActionColumn: true,
                field: 'Actions',
                header: 'Actions',
                actions: [
                    {
                        label: 'Edit',
                        icon: 'pi pi-pencil',
                        callback: (row) => this.handleAction({ action: 'edit', rowData: row }),
                        //visible: () => this.permissionService.canEdit('Divisions', '/division')
                    },
                    {
                        label: 'Delete',
                        icon: 'pi pi-trash',
                        styleClass: 'p-button-danger',
                        callback: (row) => this.handleAction({ action: 'delete', rowData: row }),
                       // visible: () => this.permissionService.canDelete('Divisions', '/division')
                    }
                ]
            });
        }
    }

    initializeTableConfig(): void {
        this.tableConfig = {
            pageSize: 10,
            pageSizeOptions: [5, 10, 25],
            showSearch: true,
            searchPlaceholder: 'Search here',
            emptyMessage: 'No division available',
            showCreateButton: true,
            createButtonLabel: 'Create Division',
           // createButtonPermission: this.permissionService.canAdd('Divisions', '/division')
        };
    }

    handleAction(event: { action: string; rowData: any }) {
        switch (event.action) {
            case 'edit':
                this.editDivision(event.rowData);
                break;
            case 'delete':
                this.deleteDivision(event.rowData);
                break;
        }
    }

    editDivision(division: any) {
        // if (this.permissionService.canEdit('Divisions', '/division')) {
        //     this.route
        //         .navigate(['/division/edit', division.id], {
        //             state: {
        //                 division: {
        //                     ...division
        //                 }
        //             }
        //         })
        //         .then(() => {
        //             console.log('Navigation to edit project successful');
        //         });
        // }
    }

    async deleteDivision(division: any) {
        // if (this.permissionService.canDelete('Divisions', '/division')) {
        //     console.log('Delete division:', division);
        //     this.selectedDivision.set(division);
        //     this.showDialog.set(true);
        // }
    }

    onDeleteConfirmed() {
        console.log('deleted');
        if (this.selectedDivision())
            this.divisionService.deleteDivision(this?.selectedDivision()?.id ?? '').subscribe({
                next: (res) => {
                    if (res.isSuccess) {
                        this.toastService.success(res.notificationMessage, 'Delete Success');
                        this.tableComponent.loadData();
                    } else {
                        this.toastService.error(res.errors?.[0], 'Delete Failed');
                    }
                }
            });
        this.showDialog.set(false);
        this.selectedDivision.set(null);
    }

    onCancel() {
        this.showDialog.set(false);
    }

    createDivision() {
        this.route.navigate(['/division/create']);
    }
}
