import { Component, OnInit, signal } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Router, RouterModule } from '@angular/router';
import { map, Observable } from 'rxjs';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { LayoutService } from '../../service/layout.service';
import { AuthService } from '../../../../../core/services/auth.service';
import { TenantService } from '../../../../../core/services/tenant.service';
import { ToastService } from '../../../../../core/services/toast.service';
import { Severity } from '../../../../../core/enums/toast-severity.enum';
import { ToastModule } from 'primeng/toast';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { MenuModule } from 'primeng/menu';
import { Logo } from '../../../../components/icons/logo';
import { StyleClassModule } from 'primeng/styleclass';
import { CommonModule } from '@angular/common';
import { SelectModule } from 'primeng/select';
import { DividerModule } from 'primeng/divider';
import { UserService } from '../../../../../core/services/user.service';

@Component({
  selector: 'app-topbar-new',
  templateUrl: './app-topbar-new.component.html',
  imports: [
    RouterModule,
    CommonModule,
    StyleClassModule,
    DividerModule,
    Logo,
    MenuModule,
    DialogModule,
    ButtonModule,
    InputTextModule,
    FormsModule,
    ToastModule,
    ReactiveFormsModule,
    SelectModule,
  ],
  styleUrls: ['./app-topbar-new.component.scss'],
})
export class AppTopbarNew implements OnInit {
  menuItems!: MenuItem[];
  tenantLogoUrl$: Observable<string | undefined>;
  changePasswordDialog = false;
  isChangingPassword = false;
  userProfileDialog = false;
  transferRoleDialog = false;
  isTransferRole = false;
  employees: any[] = [];
  currentUser: any;
  tenantId: string | null = null;
  selectedEmployee: any;

  changePasswordForm = new FormGroup({
    currentPassword: new FormControl('', [Validators.required]),
    newPassword: new FormControl('', [Validators.required]),
    confirmPassword: new FormControl(''),
  });

  transferRoleForm = new FormGroup({
    password: new FormControl('', [Validators.required]),
    employee: new FormControl<any>(null, [Validators.required]),
  });

  constructor(
    public layoutService: LayoutService,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly tenantService: TenantService,
    private readonly toastService: ToastService,
    private readonly userService: UserService
  ) {
    this.tenantLogoUrl$ = this.getTenantLogoUrl();
    this.currentUser = this.authService.getCurrentUser();
  }

  ngOnInit(): void {
    this.loadTenantProfile();
    this.initializeMenuItems();
    this.loadCurrentUser();
  }
  private loadEmployees(): void {
    // this.userService.getUsersByTenant().subscribe({
    //     next: (response) => {
    //         if (response.isSuccess) {
    //             this.employees = response?.data || [];
    //         } else {
    //             this.toastService.error(response.errors?.[0] || 'Failed to load employees', Severity.ERROR);
    //         }
    //     },
    //     error: (error) => {
    //         console.error('Failed to load employees:', error);
    //         this.toastService.error('Failed to load employees', Severity.ERROR);
    //     }
    // });
  }

  loadEmployeeDetails(): void {
    this.selectedEmployee = this.transferRoleForm.get('employee')?.value;
  }

  private getTenantLogoUrl(): Observable<string | undefined> {
    return this.tenantService.currentTenant$.pipe(
      map((tenant) => tenant?.logoUrl),
      map((url) => (url === null ? undefined : url))
    );
  }

  private loadTenantProfile(): void {
    this.tenantService.loadTenantProfile().catch((error) => {
      console.error('Failed to load tenant profile:', error);
    });
  }

  private loadCurrentUser(): void {
    this.authService
      .loadCurrentUserDetails()
      .then(() => {
        this.currentUser = this.authService.getCurrentUser();
        this.tenantId = this.currentUser?.userLoginInfo?.tenantId || null;
      })
      .catch((error) => {
        console.error('Failed to load current user profile:', error);
      });
  }

  private initializeMenuItems(): void {
    const items: MenuItem[] = [
      {
        label: 'User Profile',
        icon: 'pi pi-user',
        command: () => this.showUserProfileDialog(),
      },
      { separator: true },
      {
        label: 'Sign Out',
        icon: 'pi pi-sign-out',
        command: () => this.logOut(),
      },
    ];
    // Filter out items that are not visible (if 'visible' is explicitly false)
    this.menuItems = items.filter((item) => item.visible !== false);
  }

  public get roleNames(): string {
    return (
      this.currentUser?.roles?.map((role: any) => role.roleName).join(',') || ''
    );
  }

  public get isTenantAdmin(): boolean {
    return (
      this.currentUser?.roles?.some((role: any) => role.roleName === 'Admin') ||
      false
    );
  }

  public get tenantName(): string | null {
    return this.currentUser?.userLoginInfo?.tenantName || null;
  }

  toggleDarkMode(): void {
    this.layoutService.layoutConfig.update((state) => ({
      ...state,
      darkTheme: !state.darkTheme,
    }));
  }

  logOut(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  showUserProfileDialog(): void {
    this.userProfileDialog = true;
    console.log('User Profile Dialog opened:', this.currentUser);
  }

  isInvalid(controlName: string): boolean {
    const control = this.changePasswordForm.get(controlName);
    return !!(control && control.invalid && control.touched);
  }

  isInvalidTransferRole(controlName: string): boolean {
    const control = this.transferRoleForm.get(controlName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  updateProfile(): void {
    this.router.navigate([`/tenant/edit`], { state: { id: this.tenantId } });
    this.userProfileDialog = false;
  }
}
