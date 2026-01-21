import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  AfterViewInit,
} from '@angular/core';
import { DropdownModule } from 'primeng/dropdown';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Table, TableModule } from 'primeng/table';
import { ButtonDirective } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';
import { InputSwitch } from 'primeng/inputswitch';
import { Checkbox } from 'primeng/checkbox';
import { Router } from '@angular/router';
import { Select } from 'primeng/select';
import { IconField } from 'primeng/iconfield';
import { InputIcon } from 'primeng/inputicon';
import { AuthService } from '../../../../core/services/auth.service';
import { ToastService } from '../../../../core/services/toast.service';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';
import { EmptyStateComponent } from '../../../../shared/components/table/empty-state.component';
import { Severity } from '../../../../core/enums/toast-severity.enum';

interface Column {
  field: string;
  header: string;
}

@Component({
  selector: 'app-create-role',
  templateUrl: './create-role.component.html',
  imports: [
    CommonModule,
    DropdownModule,
    FormsModule,
    TableModule,
    ButtonDirective,
    InputText,
    InputSwitch,
    Checkbox,
    Select,
    IconField,
    InputIcon,
    ErrorMessageComponent,
    ReactiveFormsModule,
    EmptyStateComponent,
  ],
  styleUrls: ['./create-role.component.scss'],
})
export class CreateRoleComponent implements OnInit, AfterViewInit {
  roleData: any = {};
  role: any = {
    name: '',
    description: '',
    tenantId: '',
    status: 'active',
  };
  entryOptions = [
    { label: '8', value: 8 },
    { label: '16', value: 16 },
    { label: '24', value: 24 },
    { label: '32', value: 32 },
  ];
  selectedEntry = 8;

  searchText = '';
  menuItems: any[] = [];
  cols!: Column[];
  filteredMenuItems: any[] = [];
  @ViewChild('dt') dt!: Table;
  @ViewChild('roleNameInput') roleNameInput!: ElementRef;

  currentPage = 1;
  pageSize = 8;
  totalMenuItems = 0;
  totalPages = 1;

  sortField: string = 'name';
  sortOrder: number = 1;

  // Global select all state
  globalSelectAll = false;

  createRoleForm = new FormGroup({
    roleName: new FormControl('', [Validators.required]),
    description: new FormControl(''),
    status: new FormControl('active'),
  });

  constructor(
    private readonly authService: AuthService,
    private readonly toastService: ToastService,
    private readonly router: Router
  ) {
    this.cols = [
      { field: 'id', header: '# Serial' },
      { field: 'name', header: 'Menu Item' },
      { field: 'view', header: 'Can View?' },
      { field: 'edit', header: 'Can Edit?' },
      { field: 'create', header: 'Can Create?' },
      { field: 'delete', header: 'Can Delete?' },
      { field: 'preview', header: 'Can Preview?' },
      { field: 'export', header: 'Can Export?' },
      { field: 'print', header: 'Can Print?' },
    ];
  }

  ngOnInit() {
    this.pageSize = this.selectedEntry;
    const userLoginInfo = JSON.parse(
      localStorage.getItem('userLoginInfo') || '{}'
    );
    this.role.tenantId = userLoginInfo.tenantId;
    this.loadMenuItems();
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      if (this.roleNameInput && this.roleNameInput.nativeElement) {
        this.roleNameInput.nativeElement.focus();
      }
    }, 0);
  }

  loadMenuItems() {
    this.authService.getPermissionList().subscribe({
      next: (response: any) => {
        if (response.isSuccess) {
          const data = response.data;
          data.forEach((item: any) => {
            this.menuItems.push({
              id: item.id,
              name: item.name,
              roleMenus: {
                canView: false,
                canEdit: false,
                canAdd: false,
                canDelete: false,
                canPreview: false,
                canExport: false,
                canPrint: false,
              },
            });
          });
          this.totalMenuItems = this.menuItems.length;
          this.filterMenuItems();
        } else {
          this.toastService.error(
            'Failed to load permission list: ' + (response.errors?.[0] || ''),
            Severity.ERROR
          );
        }
      },
      error: () => {
        this.toastService.error(
          'Failed to load menu items. Please try again.',
          Severity.ERROR
        );
      },
    });
  }

  filterMenuItems() {
    let items = [...this.menuItems];

    if (this.searchText && this.searchText.trim() !== '') {
      items = items.filter((item) =>
        item.name.toLowerCase().includes(this.searchText.toLowerCase())
      );
    }

    this.sortItems(items);

    this.totalMenuItems = items.length;
    this.totalPages = Math.ceil(this.totalMenuItems / this.pageSize);

    const startIndex = (this.currentPage - 1) * this.pageSize;
    this.filteredMenuItems = items.slice(
      startIndex,
      startIndex + this.pageSize
    );
  }

  sortItems(items: any[]) {
    items.sort((a, b) => {
      let valueA: any;
      let valueB: any;

      if (this.sortField.includes('.')) {
        const parts = this.sortField.split('.');
        valueA = parts.reduce(
          (obj, key) => (obj && obj[key] !== undefined ? obj[key] : null),
          a
        );
        valueB = parts.reduce(
          (obj, key) => (obj && obj[key] !== undefined ? obj[key] : null),
          b
        );
      } else {
        valueA = a[this.sortField];
        valueB = b[this.sortField];
      }

      if (typeof valueA === 'string' && typeof valueB === 'string') {
        return this.sortOrder * valueA.localeCompare(valueB);
      } else if (
        (valueA === null || valueA === undefined) &&
        (valueB === null || valueB === undefined)
      ) {
        return 0;
      } else if (valueA === null || valueA === undefined) {
        return this.sortOrder * 1;
      } else if (valueB === null || valueB === undefined) {
        return this.sortOrder * -1;
      } else if (typeof valueA === 'boolean' && typeof valueB === 'boolean') {
        return this.sortOrder * (valueA === valueB ? 0 : valueA ? -1 : 1);
      } else {
        return this.sortOrder * (valueA - valueB);
      }
    });
  }

  sort(event: any) {
    const field = event.field || event;
    if (this.sortField === field) {
      this.sortOrder = -this.sortOrder;
    } else {
      this.sortField = field;
      this.sortOrder = 1;
    }
    this.filterMenuItems();
  }

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) {
      return;
    }
    this.currentPage = page;
    this.filterMenuItems();
  }

  getPaginationRange() {
    const range = [];
    const totalPages = Math.ceil(this.totalMenuItems / this.pageSize);

    let startPage = Math.max(1, this.currentPage - 2);
    let endPage = Math.min(totalPages, startPage + 4);

    if (endPage - startPage < 4) {
      startPage = Math.max(1, endPage - 4);
    }

    for (let i = startPage; i <= endPage; i++) {
      range.push(i);
    }

    return range;
  }

  onEntryChange(event: any) {
    this.pageSize = event.value;
    this.selectedEntry = event.value;
    this.currentPage = 1;
    this.filterMenuItems();
  }

  onSearchInput(event: Event) {
    const input = event.target as HTMLInputElement;
    this.searchText = input.value;
    this.currentPage = 1;
    this.filterMenuItems();
  }

  handleSubmit() {
    if (this.createRoleForm.valid) {
      this.roleData = {
        role: {
          roleName: this.createRoleForm.value.roleName ?? '',
          description: this.createRoleForm.value.description ?? '',
          tenantId: this.role.tenantId,
          isActive: this.createRoleForm.value.status === 'active',
        },
        roleMenus: this.menuItems
          .filter(
            (item) =>
              item.roleMenus.canView ||
              item.roleMenus.canAdd ||
              item.roleMenus.canEdit ||
              item.roleMenus.canDelete ||
              item.roleMenus.canPreview ||
              item.roleMenus.canExport ||
              item.roleMenus.canPrint
          )
          .map((item) => ({
            sitemapId: item.id,
            canView: item.roleMenus.canView || false,
            canAdd: item.roleMenus.canAdd || false,
            canEdit: item.roleMenus.canEdit || false,
            canDelete: item.roleMenus.canDelete || false,
            canPreview: item.roleMenus.canPreview || false,
            canExport: item.roleMenus.canExport || false,
            canPrint: item.roleMenus.canPrint || false,
            tenantId: this.role.tenantId,
          })),
      };

      this.authService.createRole(this.roleData).subscribe({
        next: (response: any) => {
          if (response.isSuccess) {
            this.toastService.success('Role created successfully.');
            this.resetForm();
            this.createRoleForm.reset({
              roleName: '',
              description: '',
              status: 'active',
            });
            this.router.navigate(['/role']).then();
          } else {
            this.toastService.error(
              response.errors?.[0] || 'Failed to create role',
              Severity.ERROR
            );
          }
        },
        error: () => {
          this.toastService.error(
            'Failed to create role. Please try again.',
            Severity.ERROR
          );
        },
      });
    } else {
      this.toastService.error(
        'Please enter a valid role and description.',
        Severity.ERROR
      );
    }
  }

  resetForm() {
    this.role = {
      name: '',
      description: '',
      tenantId: this.role.tenantId,
      status: 'active',
    };

    this.globalSelectAll = false;

    this.menuItems.forEach((item) => {
      item.roleMenus = {
        canView: false,
        canEdit: false,
        canAdd: false,
        canDelete: false,
        canPreview: false,
        canExport: false,
        canPrint: false,
      };
    });

    this.currentPage = 1;
    this.filterMenuItems();
  }

  cancelRole() {
    this.router.navigate(['/role']).then();
  }

  protected readonly Math = Math;

  isInvalid(controlName: string): boolean {
    const control = this.createRoleForm.get(controlName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  // Global select all functionality
  onGlobalSelectAll(checked: boolean) {
    this.globalSelectAll = checked;
    this.menuItems.forEach((item) => {
      item.roleMenus.canView = checked;
      item.roleMenus.canEdit = checked;
      item.roleMenus.canAdd = checked;
      item.roleMenus.canDelete = checked;
      item.roleMenus.canPreview = checked;
      item.roleMenus.canExport = checked;
      item.roleMenus.canPrint = checked;
    });
  }

  // Per-row select all functionality
  onRowSelectAll(menuItem: any, checked: boolean) {
    menuItem.roleMenus.canView = checked;
    menuItem.roleMenus.canEdit = checked;
    menuItem.roleMenus.canAdd = checked;
    menuItem.roleMenus.canDelete = checked;
    menuItem.roleMenus.canPreview = checked;
    menuItem.roleMenus.canExport = checked;
    menuItem.roleMenus.canPrint = checked;

    // Update global select all state
    this.updateGlobalSelectAllState();
  }

  // Check if all permissions for a row are selected
  isRowFullySelected(menuItem: any): boolean {
    return (
      menuItem.roleMenus.canView &&
      menuItem.roleMenus.canEdit &&
      menuItem.roleMenus.canAdd &&
      menuItem.roleMenus.canDelete &&
      menuItem.roleMenus.canPreview &&
      menuItem.roleMenus.canExport &&
      menuItem.roleMenus.canPrint
    );
  }

  // Update global select all state based on individual selections
  updateGlobalSelectAllState() {
    const allSelected = this.menuItems.every((item) =>
      this.isRowFullySelected(item)
    );
    this.globalSelectAll = allSelected;
  }

  // Handle individual permission change to update global state
  onPermissionChange() {
    this.updateGlobalSelectAllState();
  }
}
