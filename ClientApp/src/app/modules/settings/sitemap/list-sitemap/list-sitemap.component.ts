import { Component, signal, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { TableColumn, TableConfig } from '../../../../shared/components/table/table.interface';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
    selector: 'app-sitemap-management',
    imports: [TableComponent, ConfirmDialogComponent, ConfirmDialogComponent],
    templateUrl: './list-sitemap.component.html',
    styleUrl: './list-sitemap.component.scss'
})
export class ListSitemapComponent {
    @ViewChild(TableComponent) tableComponent!: TableComponent;
    selectedMenu = signal<any>(null);
    showDialog = signal(false);
    columns: TableColumn[] = [
        { field: 'nameWithParent', header: 'Name', sortable: true },
        { field: 'favIcon', header: 'Icon', sortable: true },
        { field: 'pageUrl', header: 'Path', sortable: true },
        {
            isActionColumn: true,
            field: 'Actions',
            header: 'Actions',
            actions: [
                {
                    label: 'Edit',
                    icon: 'pi pi-pencil',
                    callback: (row) => this.handleAction({ action: 'edit', rowData: row })
                }
                // {
                //     label: 'Delete',
                //     icon: 'pi pi-trash',
                //     styleClass: 'p-button-danger',
                //     callback: (row) => this.handleAction({ action: 'delete', rowData: row })
                // }
            ]
        }
    ];
    tableConfig: TableConfig = {
        pageSize: 10,
        pageSizeOptions: [5, 10, 25],
        showSearch: true,
        searchPlaceholder: 'Search here',
        emptyMessage: 'No sitemap available',
        showCreateButton: true,
        createButtonLabel: 'Create Sitemap'
    };
    constructor(private readonly route: Router) {}
    handleAction(event: { action: string; rowData: any }) {
        switch (event.action) {
            case 'edit':
                this.editSitemap(event.rowData);
                break;
            case 'delete':
                this.deleteSitemap(event.rowData);
                break;
        }
    }
    editSitemap(sitemap: any) {
        console.log('Edit type:', sitemap);
        this.route
            .navigate(['/sitemap/edit', sitemap.id], {
                state: {
                    sitemap: {
                        ...sitemap
                    }
                }
            })
            .then(() => {
                console.log('Navigation to edit sitemap successful');
            });
    }
    async deleteSitemap(type: any) {
        console.log('Delete type:', type);
    }
    onDeleteConfirmed() {}
    onCancel() {
        this.showDialog.set(false);
    }
    createSitemap() {
        this.route.navigate(['sitemap/create']).then();
    }
}
