import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Router, RouterModule } from '@angular/router';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { LayoutService } from '../../service/layout.service';
import { ToastService } from '../../../../../core/services/toast.service';
import { ErrorMessageComponent } from '../../../../components/error-message/error-message.component';
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
  selector: 'app-topbar',
  templateUrl: './app-topbar.component.html',
  imports: [
    RouterModule,
    CommonModule,
    StyleClassModule,
    DividerModule,
    MenuModule,
    DialogModule,
    ButtonModule,
    InputTextModule,
    FormsModule,
    ToastModule,
    ReactiveFormsModule,
    SelectModule,
  ],
  styleUrls: ['./app-topbar.component.scss'],
})
export class AppTopbar implements OnInit {
  menuItems!: MenuItem[];
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
    newPassword: new FormControl(''),
    confirmPassword: new FormControl(''),
  });

  transferRoleForm = new FormGroup({
    password: new FormControl('', [Validators.required]),
    employee: new FormControl<any>(null, [Validators.required]),
  });

  constructor(
    public layoutService: LayoutService,
    private readonly router: Router,
    private readonly toastService: ToastService,
    private readonly userService: UserService
  ) {
    //this.tenantLogoUrl$ = this.getTenantLogoUrl();
    this.currentUser = null; //this.authService.getCurrentUser();
  }

  ngOnInit(): void {
    //this.loadTenantProfile();
    this.initializeMenuItems();
    this.loadCurrentUser();
  }

  loadEmployeeDetails(): void {
    this.selectedEmployee = this.transferRoleForm.get('employee')?.value;
  }

  private loadCurrentUser(): void {}

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
    this.userService.logout();
    this.router.navigate(['/login']);
  }

  showUserProfileDialog(): void {
    this.userProfileDialog = true;
    console.log('User Profile Dialog opened:', this.currentUser);
  }

  updateProfile(): void {
    this.router.navigate([`/tenant/edit`], { state: { id: this.tenantId } });
    this.userProfileDialog = false;
  }
}
