import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { SelectButtonModule } from 'primeng/selectbutton';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { DividerModule } from 'primeng/divider';
import { MessageModule } from 'primeng/message';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBase } from '../../../../core/enums/form-base';
import { FormErrorComponent } from '../../../../shared/components/form-error.component';
import { TenantService } from '../../../../core/services/tenant.service';
import { ToastService } from '../../../../core/services/toast.service';
import { EMPTY_GUID } from '../../../../core/constents';

@Component({
  selector: 'app-edit-tenant',
  imports: [
    SelectButtonModule,
    ReactiveFormsModule,
    MessageModule,
    CommonModule,
    DropdownModule,
    ButtonModule,
    InputTextModule,
    SelectModule,
    ToggleSwitchModule,
    DividerModule,
    FormErrorComponent,
  ],
  templateUrl: './edit-tenant.component.html',
  styleUrl: './edit-tenant.component.scss',
})
export class EditTenantComponent extends FormBase {
  initialData!: any;
  form = new FormGroup({
    firstName: new FormControl('', [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(50),
    ]),
    lastName: new FormControl('', [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(50),
    ]),
    email: new FormControl('', [Validators.required, Validators.email]),
    phone: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(15),
    ]),
    city: new FormControl('', [
      Validators.minLength(2),
      Validators.maxLength(50),
    ]),
    province: new FormControl('', [
      Validators.minLength(2),
      Validators.maxLength(50),
    ]),
    street: new FormControl('', [
      Validators.minLength(2),
      Validators.maxLength(50),
    ]),
    postalCode: new FormControl('', [
      Validators.minLength(2),
      Validators.maxLength(50),
    ]),
    tenantName: new FormControl('', [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(50),
    ]),
    domain: new FormControl('', [Validators.required]),
    binNo: new FormControl('', [
      Validators.minLength(2),
      Validators.maxLength(50),
    ]),
  });

  constructor(
    private readonly tenantService: TenantService,
    private readonly toastService: ToastService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {
    super();

    this.form.get('domain')?.valueChanges.subscribe(() => {
      this.form.get('email')?.updateValueAndValidity();
    });
  }

  onSubmit() {
    if (!this.form.valid) {
      this.markAllAsTouched(); // show all errors
      return;
    }
    if (this.form.valid) {
      const payload: any = {
        id: this.initialData?.id ?? EMPTY_GUID,
        tenantName: this.form.value.tenantName ?? '',
        binNo: this.form.value.binNo ?? '',
        tenantEmail: this.form.value.email ?? '',
        phoneNumber: this.form.value.phone ?? '',
        domain: this.form.value.domain ?? '',
        street: this.form.value.street ?? '',
        city: this.form.value.city ?? '',
        province: this.form.value.province ?? '',
        postCode: this.form.value.postalCode ?? '',
        tenantUserList: [
          {
            id: EMPTY_GUID, // Adjust this if you get user id from initialData
            email: this.form.value.email ?? '',
            phoneNumber: this.form.value.phone ?? '',
            tenantId: this.initialData?.id ?? EMPTY_GUID,
            firstName: this.form.value.firstName ?? '',
            lastName: this.form.value.lastName ?? '',
          },
        ],
      };

      console.log('edit payload', payload);

      this.tenantService.updateTenantRegistration(payload).subscribe({
        next: (res: any) => {
          if (res.isSuccess) {
            this.toastService.success('Tenant has been updated successfully.');
            this.router.navigate(['/tenant']);
          } else {
            this.toastService.error(
              'Failed to update tenant. Please try again later.'
            );
          }
        },
        error: () => {
          this.toastService.error(
            'Failed to update tenant. Please try again later.'
          );
        },
      });
    }
  }

  onCancel() {
    this.router.navigate(['/tenant']);
  }
}
