import { Component, Input, OnInit, signal, ViewChild } from '@angular/core';
import { TableComponent } from '../../../shared/components/table/table.component';
import {
  TableColumn,
  TableConfig,
} from '../../../shared/components/table/table.interface';
import { ToastService } from '../../../core/services/toast.service';
import { Router } from '@angular/router';
import { StudentService } from '../../../core/services/student.service';

@Component({
  selector: 'app-student-application-list',
  imports: [TableComponent],
  templateUrl: './student-application-list.component.html',
  styleUrl: './student-application-list.component.scss',
})
export class StudentApplicationListComponent implements OnInit {
  @ViewChild(TableComponent) tableComponent!: TableComponent;

  selectedDivision = signal<any>(null);
  showDialog = signal(false);
  columns: TableColumn[] = [];

  tableConfig!: TableConfig;

  constructor(
    private readonly studentService: StudentService,
    private readonly toastService: ToastService,
    private readonly route: Router
  ) {}

  ngOnInit(): void {
    this.initializeColumns();
    this.initializeTableConfig();
  }

  initializeColumns(): void {
    this.columns = [
      { field: 'fullName', header: 'Full Name', sortable: true },
      { field: 'fatherName', header: 'Father Name', sortable: true },
      { field: 'motherName', header: 'Mother Name', sortable: true },
      {
        field: 'applicationForClass',
        header: 'ApplicationForClass',
        sortable: true,
      },
      { field: 'Name', header: 'Local Guardian Name', sortable: true },
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
        // // {
        //   label: 'Delete',
        //   icon: 'pi pi-trash',
        //   styleClass: 'p-button-danger',
        //   callback: (row) =>
        //     this.handleAction({ action: 'delete', rowData: row }),
        //   visible: (row) => true,
        // }
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
      showCheckboxColumn: false,
      createButtonLabel: 'Create Student Application',
    };
  }
  selectedRows: any[] = [];
  @Input() preselectedRows: any = null;
  onSelectedRowsChange(rows: any[]) {
    this.studentService.getStudentById(rows[0].id).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.toastService.success(res.notificationMessage, 'Update Success');
          this.tableComponent.loadData();
        } else {
          this.toastService.error(res.errors?.[0], 'Update Failed');
        }
      },
    });
    this.selectedRows = rows;
  }

  handleAction(event: { action: string; rowData: any }) {
    switch (event.action) {
      case 'edit':
        this.editUser(event.rowData);
        break;
      case 'delete':
        this.deleteRole(event.rowData);
        break;
    }
  }

  editUser(user: any) {
    debugger;
    this.route
      .navigate(['/application/edit', user.id], {
        state: {
          user: {
            ...user,
          },
        },
      })
      .then(() => {
        console.log('Navigation to edit user successful');
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
    // if (this.selectedDivision())
    //   this.divisionService
    //     .deleteDivision(this?.selectedDivision()?.id ?? '')
    //     .subscribe({
    //       next: (res) => {
    //         if (res.isSuccess) {
    //           this.toastService.success(
    //             res.notificationMessage,
    //             'Delete Success'
    //           );
    //           this.tableComponent.loadData();
    //         } else {
    //           this.toastService.error(res.errors?.[0], 'Delete Failed');
    //         }
    //       },
    //     });
    this.showDialog.set(false);
    this.selectedDivision.set(null);
  }

  onCancel() {
    this.showDialog.set(false);
  }

  createUser() {
    this.route.navigate(['/application/create']);
  }
}
