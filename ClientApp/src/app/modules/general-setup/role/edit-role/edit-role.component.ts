import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
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
import { CommonModule } from '@angular/common';
import { InputSwitch } from 'primeng/inputswitch';
import { Checkbox } from 'primeng/checkbox';
import { ActivatedRoute, Router } from '@angular/router';
import { Select } from 'primeng/select';
import { InputText } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { IconField } from 'primeng/iconfield';
import { InputIcon } from 'primeng/inputicon';
import { RadioButtonModule } from 'primeng/radiobutton';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';
import { EmptyStateComponent } from '../../../../shared/components/table/empty-state.component';
import { AuthService } from '../../../../core/services/auth.service';
import { ToastService } from '../../../../core/services/toast.service';
import { EMPTY_GUID } from '../../../../core/constents';
import { Severity } from '../../../../core/enums/toast-severity.enum';

@Component({
  selector: 'app-edit-role',
  templateUrl: './edit-role.component.html',
  styleUrls: ['./edit-role.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TableModule,
    DropdownModule,
    InputSwitch,
    Checkbox,
    Select,
    ErrorMessageComponent,
    InputText,
    ButtonModule,
    IconField,
    InputIcon,
    RadioButtonModule,
    EmptyStateComponent,
  ],
})
export class EditRoleComponent implements OnInit, AfterViewInit {
  @ViewChild('roleNameInput') roleNameInput!: ElementRef;
  @ViewChild('dt') dt!: Table;

  roleId: string = '';
  role: any = {};
  roleData: any = {};
  isLoading: boolean = false;
  menuItems: any[] = [];
  filteredMenuItems: any[] = [];
  searchTerm: string = '';
  cols: any[] = [];

  // Pagination properties
  paginatedMenuItems: any[] = [];
  currentPage = 1;
  pageSize = 8;
  totalMenuItems = 0;
  totalPages = 1;
  selectedEntry = 8;
  entryOptions = [
    { label: '8', value: 8 },
    { label: '16', value: 16 },
    { label: '24', value: 24 },
    { label: '32', value: 32 },
  ];

  // Global select all state
  globalSelectAll = false;

  editRoleForm = new FormGroup({
    roleName: new FormControl('', [Validators.required]),
    description: new FormControl(''),
    status: new FormControl('Active'),
  });

  constructor(
    private readonly authService: AuthService,
    private readonly toastService: ToastService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
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
    // Get role data from router state if available
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras.state as { role: any };
    if (state?.role) {
      // Save the complete role object
      console.log('tenantId', state.role.tenantId);
      this.role = {
        id: state.role.id,
        roleName: state.role.roleName,
        description: state.role.description,
        tenantId: state.role.tenantId || EMPTY_GUID,
        status: state.role.status || 'Active',
      };

      // Populate the form with the role data
      this.editRoleForm.patchValue({
        roleName: state.role.roleName,
        description: state.role.description,
        status: state.role.status,
      });

      // Store complete role data in sessionStorage to handle page reloads
      sessionStorage.setItem('editRoleData', JSON.stringify(this.role));
    } else {
      // Try to get role data from sessionStorage if available (for page reloads)
      const storedRoleData = sessionStorage.getItem('editRoleData');
      if (storedRoleData) {
        this.role = JSON.parse(storedRoleData);

        // Populate the form with the stored role data
        this.editRoleForm.patchValue({
          roleName: this.role.roleName,
          description: this.role.description,
          status: this.role.status,
        });
      }
    }
  }

  ngOnInit(): void {
    // Get role ID from route params
    this.route.params.subscribe((params) => {
      this.roleId = params['id'];
      console.log(this.role);
      if (this.roleId) {
        this.loadRoleDetails();
      }
    });
  }

  ngAfterViewInit(): void {
    // Focus on role name input field after view is initialized
    setTimeout(() => {
      if (this.roleNameInput) {
        this.roleNameInput.nativeElement.focus();
      }
    }, 0);
  }

  loadRoleDetails() {
    this.isLoading = true;

    // First try to use data from state/session if available
    const storedRoleData = sessionStorage.getItem('editRoleData');
    if (storedRoleData) {
      const parsedData = JSON.parse(storedRoleData);
      // Update form with stored data immediately
      this.updateFormWithRoleData(parsedData);
    }

    // Then fetch from API to ensure latest data
    this.authService.getRoleById(this.roleId).subscribe({
      next: (response: any) => {
        this.isLoading = false;
        if (response.isSuccess) {
          const roleData = response.data;
          this.loadMenuItems(roleData);
        } else {
          this.toastService.error(
            'Failed to load role details: ' + (response.errors?.[0] || ''),
            Severity.ERROR
          );
        }
      },
      error: () => {
        this.isLoading = false;
        this.toastService.error(
          'Failed to load role details. Please try again.',
          Severity.ERROR
        );
      },
    });
  }

  // Helper method to update form with role data
  private updateFormWithRoleData(roleData: any) {
    // Use setTimeout to ensure Angular's change detection cycle picks up the changes
    setTimeout(() => {
      this.editRoleForm.patchValue({
        roleName: roleData.roleName || '',
        description: roleData.description || '',
        status: roleData.status || 'Active',
      });
    }, 0);
  }

  loadMenuItems(roleMenus: any[] = []) {
    this.authService.getPermissionList().subscribe({
      next: (response: any) => {
        if (response.isSuccess) {
          const data = response.data;

          this.menuItems = data.map((item: any) => {
            // Find if this menu item has existing permissions
            const existingMenu = roleMenus.find((rm: any) => {
              return rm.sitemapId === item.id;
            });

            return {
              id: item.id,
              name: item.name,
              roleMenus: {
                canView: existingMenu ? existingMenu.canView : false,
                canAdd: existingMenu ? existingMenu.canAdd : false,
                canEdit: existingMenu ? existingMenu.canEdit : false,
                canDelete: existingMenu ? existingMenu.canDelete : false,
                canPreview: existingMenu ? existingMenu.canPreview : false,
                canExport: existingMenu ? existingMenu.canExport : false,
                canPrint: existingMenu ? existingMenu.canPrint : false,
              },
            };
          });

          // Sort menu items alphabetically by name in ascending order
          this.sortItems(this.menuItems);
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
          'Failed to load permission list. Please try again.',
          Severity.ERROR
        );
      },
    });
  }

  filterMenuItems() {
    if (!this.searchTerm) {
      this.filteredMenuItems = [...this.menuItems];
    } else {
      const searchTermLower = this.searchTerm.toLowerCase();
      this.filteredMenuItems = this.menuItems.filter((item) =>
        item.name.toLowerCase().includes(searchTermLower)
      );
    }

    // Apply pagination
    this.totalMenuItems = this.filteredMenuItems.length;
    this.totalPages = Math.ceil(this.totalMenuItems / this.pageSize);
    const startIndex = (this.currentPage - 1) * this.pageSize;
    this.paginatedMenuItems = this.filteredMenuItems.slice(
      startIndex,
      startIndex + this.pageSize
    );
  }

  sortItems(items: any[]) {
    return items.sort((a, b) => {
      const nameA = a.name.toLowerCase();
      const nameB = b.name.toLowerCase();
      return nameA.localeCompare(nameB);
    });
  }

  onCancel() {
    this.router.navigate(['/role']).then();
  }

  // Method for pagination
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
    this.searchTerm = input.value;
    this.currentPage = 1;
    this.filterMenuItems();
  }

  onSubmit() {
    if (this.editRoleForm.invalid) {
      this.toastService.error(
        'Please fill in all required fields.',
        Severity.ERROR
      );
      return;
    }

    this.roleData = {
      role: {
        id: this.roleId,
        roleName: this.editRoleForm.value.roleName ?? '',
        description: this.editRoleForm.value.description ?? '',
        tenantId: this.role.tenantId,
        isActive: this.editRoleForm.value.status === 'Active',
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
          roleId: this.roleId,
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

    console.log('roleData', this.roleData);

    this.authService.updateRole(this.roleData).subscribe({
      next: (response: any) => {
        if (response.isSuccess) {
          this.toastService.success(
            'Role updated successfully',
            Severity.SUCCESS
          );
          this.router.navigate(['/role']).then();
        } else {
          this.toastService.error(
            'Failed to update role: ' + (response.errors?.[0] || ''),
            Severity.ERROR
          );
        }
      },
      error: () => {
        this.toastService.error(
          'Failed to update role. Please try again.',
          Severity.ERROR
        );
      },
    });
  }

  // For form validation
  isInvalid(controlName: string): boolean {
    const control = this.editRoleForm.get(controlName);
    return !!(control && control.invalid && control.touched);
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

  // For Math operations in template
  protected readonly Math = Math;
}
