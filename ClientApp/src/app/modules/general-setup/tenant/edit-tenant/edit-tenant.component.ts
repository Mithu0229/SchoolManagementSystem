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
  isSuperAdmin = signal(false);

  twoFactorAuthOptions = [
    // { name: 'Authenticator App', value: 'AuthenticatorApp' },
    // { name: 'SMS', value: 'SMS' },
    { name: 'Email', value: 'Email' },
  ];
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
    tenantDomainList: new FormControl([]),
    tenantPath: new FormControl('', [Validators.required]),
    componayLogo: new FormControl<any>([], [Validators.maxLength(1)]),
    certificates: new FormControl([], [Validators.maxLength(5)]),
    twoFactorEnabled: new FormControl(false),
    authenticationMode: new FormControl<{ name: string; value: string } | null>(
      null
    ),
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

    // Auto-populate System Pathway URL based on tenant legal business name (only for super admin)
    this.form.get('tenantName')?.valueChanges.subscribe((tenantName) => {
      if (this.isSuperAdmin() && tenantName && tenantName.trim()) {
        const sanitizedName = this.sanitizeTenantName(tenantName);
      }
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
      console.log('formValue: ', this.form.value.certificates);

      const certificateAttachments = (this.form.value.certificates ?? []).map(
        (attachment: any) => {
          console.log('Processing certificate file:', attachment);
          return {
            id: attachment?.id ?? EMPTY_GUID,
            tenantId: attachment?.tenantId ?? EMPTY_GUID,
            attachmentId: attachment?.attachmentId ?? EMPTY_GUID,
            attachedFile: attachment?.attachmentId ? null : attachment.base64,
            attachmentName: attachment.name,
            fileSize: attachment.size,
            originalFileName: attachment.name,
          };
        }
      );

      console.log('certificateAttachments: ', certificateAttachments);
      // const payload: TenantRegistrationRequest = {
      //     id: this.oldTenantData?.id ?? EMPTY_GUID,
      //     tenantName: this.form.value.tenantName ?? this.oldTenantData.tenantName,
      //     binNo: this.form.value.binNo ?? this.oldTenantData?.binNo,
      //     tenantEmail: this.form.value.email ?? this.oldTenantData?.tenantEmail,
      //     phoneNumber: this.form.value.phone ?? this.oldTenantData?.phoneNumber,
      //     domain: this.form.value.domain ?? this.oldTenantData?.domain,
      //     tenantPath: this.form.value.tenantPath ?? this.oldTenantData?.tenantPath,
      //     currentPlanId: this.form.value.currentPlanId?.id ?? '',
      //     street: this.form.value.street ?? '',
      //     city: this.form.value.city ?? '',
      //     province: this.form.value.province ?? '',
      //     postCode: this.form.value.postalCode ?? '',
      //     countryId: this.form.value.country?.id ?? '',
      //     tenantUserList: [
      //         {
      //             ...this.tenantUser,
      //             email: this.form.value.email ?? '',
      //             phoneNumber: this.form.value.phone ?? '',
      //             twoFactorEnabled: this.form.value.twoFactorEnabled ?? false,
      //             authenticationMode: this.form.value.authenticationMode?.value ?? '',
      //             userProfile: {
      //                 ...this.tenantUser?.userProfile,
      //                 firstName: this.form.value.firstName ?? '',
      //                 lastName: this.form.value.lastName ?? '',
      //                 street: this.form.value.street ?? '',
      //                 city: this.form.value.city ?? '',
      //                 province: this.form.value.province ?? '',
      //                 postCode: this.form.value.postalCode ?? '',
      //                 countryId: this.form.value.country?.id ?? ''
      //             }
      //         }
      //     ],
      //     tenantDomainList:
      //         this.form.value.domain || this.oldTenantData?.domain
      //             ? [
      //                   {
      //                       id: this.oldTenantData?.tenantDomainList?.find((d: TenantDomain) => d.isDefault)?.id ?? EMPTY_GUID,
      //                       domainName: this.form.value.domain ?? this.oldTenantData?.domain,
      //                       isDefault: true,
      //                       tenantId: this.oldTenantData?.id ?? EMPTY_GUID
      //                   }
      //               ]
      //             : [],
      //     tenantAttachmentList:
      //         this.form.value.certificates?.map((attachment: any) => ({
      //             id: attachment?.id ?? EMPTY_GUID,
      //             tenantId: attachment?.tenantId ?? EMPTY_GUID,
      //             attachmentId: attachment?.attachmentId ?? EMPTY_GUID,
      //             // attachedFile: typeof attachment === 'string' ? attachment : '',
      //             // attachmentName: attachment?.attachmentName ?? '',
      //             // fileSize: attachment?.fileSize ?? '',
      //             // originalFileName: attachment?.originalFileName ?? ''
      //             attachedFile: attachment?.attachmentId ? null : attachment.base64,
      //             attachmentName: attachment.name,
      //             fileSize: attachment.size,
      //             originalFileName: attachment.name
      //         })) ?? []
      // };

      const payload: any = {};
      if ((this.form.value?.componayLogo ?? []).length > 0) {
        payload.tenantAttachmentList?.push({
          id: this.form.value?.componayLogo?.[0]?.id ?? EMPTY_GUID,
          tenantId: EMPTY_GUID,
          attachmentId:
            this.form.value?.componayLogo?.[0]?.attachmentId ?? EMPTY_GUID,
          // attachedFile: typeof this.form.value.componayLogo?.[0] === 'string' ? this.form.value.componayLogo?.[0] : '',
          // attachmentName: this.form.value?.componayLogo?.[0]?.attachmentName ?? '',
          // fileSize: this.form.value?.componayLogo?.[0]?.fileSize ?? '',
          // originalFileName: this.form.value?.componayLogo?.[0]?.originalFileName ?? ''
          attachedFile: this.form.value?.componayLogo?.[0]?.attachmentId
            ? null
            : this.form.value?.componayLogo?.[0]?.base64,
          attachmentName: this.form.value?.componayLogo?.[0]?.name,
          fileSize: this.form.value?.componayLogo?.[0]?.size,
          originalFileName: this.form.value?.componayLogo?.[0]?.name,
        });
      }

      console.log('edit payload', payload);

      this.tenantService.updateTenantRegistration(payload).subscribe({
        next: (res: any) => {
          if (res.isSuccess) {
            this.toastService.success('Tenant has been updated successfully.');
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
    if (this.isSuperAdmin()) {
      this.router.navigate(['/tenant']);
    } else this.form.reset(this.initialData);
  }
}
