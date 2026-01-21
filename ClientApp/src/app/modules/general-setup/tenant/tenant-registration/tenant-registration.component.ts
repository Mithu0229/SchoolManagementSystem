import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
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
import { Router } from '@angular/router';
import { FormBase } from '../../../../core/enums/form-base';
import { TenantService } from '../../../../core/services/tenant.service';
import { ToastService } from '../../../../core/services/toast.service';
import { EMPTY_GUID } from '../../../../core/constents';
import { FormErrorComponent } from '../../../../shared/components/form-error.component';

// Add interface for attachment type
interface AttachmentFile {
  base64: string;
  name: string;
  size: number;
}

@Component({
  selector: 'app-tenant-registration',
  imports: [
    SelectButtonModule,
    ReactiveFormsModule,
    CommonModule,
    DropdownModule,
    ButtonModule,
    InputTextModule,
    SelectModule,
    ToggleSwitchModule,
    DividerModule,
    FormErrorComponent,
  ],
  templateUrl: './tenant-registration.component.html',
  styleUrl: './tenant-registration.component.scss',
})
export class TenantRegistrationComponent extends FormBase implements OnInit {
  companyDomain: string | null = ''; // Default company domain, can be changed as needed
  form = new FormGroup({
    firstName: new FormControl(null, [
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
    private readonly router: Router
  ) {
    super();

    this.form.get('domain')?.valueChanges.subscribe(() => {
      this.form.get('email')?.updateValueAndValidity();
    });

    // Auto-populate System Pathway URL based on tenant legal business name
    this.form.get('tenantName')?.valueChanges.subscribe((tenantName) => {
      if (tenantName && tenantName.trim()) {
        const sanitizedName = this.sanitizeTenantName(tenantName);
      }
    });
  }

  ngOnInit() {
    this.form.get('domain')?.valueChanges.subscribe((value) => {
      this.companyDomain = value;
    });
  }

  private sanitizeTenantName(tenantName: string): string {
    return tenantName
      .toLowerCase()
      .trim()
      .replace(/[^a-z0-9\s-]/g, '') // Remove special characters except spaces and hyphens
      .replace(/\s+/g, '-') // Replace spaces with hyphens
      .replace(/-+/g, '-') // Replace multiple hyphens with single hyphen
      .replace(/^-|-$/g, ''); // Remove leading/trailing hyphens
  }

  onSubmit() {
    if (!this.form.valid) {
      this.markAllAsTouched(); // show all errors
      return;
    }
    if (this.form.valid) {
      const payload: any = {
        id: EMPTY_GUID,
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
            id: EMPTY_GUID,
            email: this.form.value.email ?? '',
            phoneNumber: this.form.value.phone ?? '',
            tenantId: EMPTY_GUID,
            firstName: this.form.value.firstName ?? '',
            lastName: this.form.value.lastName ?? '',
          },
        ],
      };

      this.tenantService.submitTenantRegistration(payload).subscribe({
        next: (res: any) => {
          if (res.isSuccess) {
            this.toastService.success(
              'Tenant has been created successfully. A payment link has been sent to the provided email address.'
            );
            this.resetForm();
          } else {
            this.toastService.error(
              'Failed to create tenant. Please try again later.'
            );
          }
        },
        error: () => {
          this.toastService.error(
            'Failed to create tenant. Please try again later.'
          );
        },
      });
    }
  }

  onCancel() {
    this.form.reset();
    this.router.navigate(['/tenant']);
  }
}
