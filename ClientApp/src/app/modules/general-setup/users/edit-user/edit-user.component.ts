import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import { FormBase } from '../../../../core/enums/form-base';
import { IUser, UserService } from '../../../../core/services/user.service';
import { ToastService } from '../../../../core/services/toast.service';
import { TextareaComponent } from '../../../../shared/components/textarea/textarea.component';
import { FormErrorComponent } from '../../../../shared/components/form-error.component';
import { UserTypes } from '../../../../core/enums/fixedIds';
import { RoleService } from '../../../../core/services/role.service';
import { Subject, takeUntil } from 'rxjs';
import { MultiSelectModule } from 'primeng/multiselect';

@Component({
  selector: 'app-edit-user',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    InputTextModule,
    TextareaModule,
    SelectModule,
    ButtonModule,
    TextareaComponent,
    FormErrorComponent,
    MultiSelectModule,
  ],
  templateUrl: './edit-user.component.html',
  styleUrl: './edit-user.component.scss',
})
export class EditUserComponent extends FormBase implements OnInit, OnDestroy {
  tenantId: string | null = '';
  initialData: any = null;
  currentState: IUser | null = null;
  userTypes: any[] = Object.values(UserTypes);
  form = new FormGroup({
    firstName: new FormControl(null),
    lastName: new FormControl(null),
    email: new FormControl(null, [
      Validators.required,
      Validators.maxLength(150),
    ]),
    phoneNumber: new FormControl(null, [
      Validators.required,
      Validators.maxLength(50),
    ]),
    password: new FormControl(null),
    userType: new FormControl(null, [
      Validators.required,
      Validators.maxLength(50),
    ]),
    address: new FormControl('', [Validators.maxLength(1000)]),
    isActive: new FormControl(false),
    id: new FormControl(null),
    roles: new FormControl<any[]>([]),
  });

  constructor(
    private readonly userService: UserService,
    private readonly toastService: ToastService,
    private readonly route: Router,
    private readonly roleService: RoleService,
  ) {
    super();

    const windowState = window.history.state;
    const state = windowState.user;
    if (state) {
      this.currentState = state;
    }

    // if (state) {
    //   this.currentState = state;
    //   // this.form.patchValue({
    //   //   id: state.id,
    //   //   firstName: state.firstName,
    //   //   lastName: state.lastName,
    //   //   email: state.email,
    //   //   phoneNumber: state.phoneNumber,
    //   //   address: state.address,
    //   //   userType: state.userType,
    //   //   password: state.password,
    //   // });
    // }
  }
  ngOnInit(): void {
    this.loadRoles();
    this.fetchUserDetails(this.currentState?.id);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  showPassword = false;
  private fetchUserDetails(userId: any): void {
    this.userService
      .getUserById(userId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          if (response.isSuccess && response.data) {
            this.initialData = response.data;
            this.populateForm(response.data);
          } else {
            this.toastService.error('Failed to fetch user details');
          }
        },
        error: () => this.toastService.error('Error fetching user data'),
      });
  }

  private populateForm(user: any): void {
    let selectedRoles: any[] = [];
    if (user.userRoleList && user.userRoleList.length > 0) {
      selectedRoles = user.userRoleList.map((role: any) => ({
        id: role.roleId,
        roleName: role.roleName,
      }));
    }
    this.form.patchValue({
      firstName: user.firstName,
      lastName: user.lastName,
      email: user.email,
      userType: user.userType,
      //city: user.city,
      // //street: user.userProfile.street,
      // zipCode: user.userProfile.postCode,
      address: user.address,
      phoneNumber: user.phoneNumber,
      isActive: user.isActive ?? true,
      id: user.id,
      roles: selectedRoles,
    });
  }

  onSubmit() {
    if (!this.form.valid) {
      this.markAllAsTouched(); // show all errors
      return;
    }
    if (this.form.valid) {
      const payload: any = {
        ...this.currentState!,
        firstName: this.form.value.firstName ?? '',
        lastName: this.form.value.lastName ?? '',
        email: this.form.value.email ?? '',
        password: this.currentState?.password ?? '',
        phoneNumber: this.form.value.phoneNumber ?? '',
        address: this.form.value.address ?? '',
        userType: this.form.value.userType ?? '',
        isActive: this.form.value.isActive ?? false,
        userRoleList:
          this.form.value.roles?.map((role: any) => ({
            roleId: role.id ?? '',
            roleName: role.roleName ?? '',
          })) || [],
      };

      this.userService.editUser(payload).subscribe({
        next: (res) => {
          if (res.isSuccess) {
            this.toastService.success('User has been updated successfully.');
          } else {
            let errorMessage = 'Failed to update User. Please try again later.';
            if (res.notificationMessage && res.notificationMessage !== '') {
              errorMessage = res.notificationMessage;
            } else if (res.errors?.[0]) {
              errorMessage = res.errors[0];
            }
            this.toastService.error(errorMessage);
          }
        },
        error: () => {
          this.toastService.error(
            'Failed to update User. Please try again later.',
          );
        },
      });
    }
  }

  onCancel() {
    this.route.navigate(['/user']);
  }
  private readonly destroy$ = new Subject<void>();
  roleOptions: any[] = [];
  private loadRoles(): void {
    this.roleService
      .getRolesByTenant()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.roleOptions =
              response?.data?.map((role) => {
                return {
                  id: role.id,
                  roleName: role.roleName,
                };
              }) ?? [];
          }
        },
      });
  }
}
