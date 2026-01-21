import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ToastService } from '../../../../core/services/toast.service';
import { Router } from '@angular/router';
import { CheckboxModule } from 'primeng/checkbox';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { FormErrorComponent } from '../../../../shared/components/form-error.component';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { FormBase } from '../../../../core/enums/form-base';
import {
  MenuAccessTypes,
  MenuTypes,
} from '../../../../core/enums/user-types.enum';
import { SitemapService } from '../../../../core/services/sitemap.service';

@Component({
  selector: 'app-edit-sitemap',
  imports: [
    CheckboxModule,
    CommonModule,
    ButtonModule,
    ReactiveFormsModule,
    FormErrorComponent,
    InputTextModule,
    SelectModule,
  ],
  templateUrl: './edit-sitemap.component.html',
  styleUrl: './edit-sitemap.component.scss',
})
export class EditSitemapComponent extends FormBase {
  parentMenus = [];
  menuTypes = [
    {
      label: MenuTypes.Page.label,
      value: MenuTypes.Page.value,
    },
    {
      label: MenuTypes.Tab.label,
      value: MenuTypes.Tab.value,
    },
    {
      label: MenuTypes.Step.label,
      value: MenuTypes.Step.value,
    },
    {
      label: MenuTypes.Feature.label,
      value: MenuTypes.Feature.value,
    },
    {
      label: MenuTypes.ParentMenu.label,
      value: MenuTypes.ParentMenu.value,
    },
  ];
  menuAccessTypes = [
    {
      label: MenuAccessTypes.SystemAdmin.label,
      value: MenuAccessTypes.SystemAdmin.value,
    },
    {
      label: MenuAccessTypes.TenantAdmin.label,
      value: MenuAccessTypes.TenantAdmin.value,
    },
    {
      label: MenuAccessTypes.Both.label,
      value: MenuAccessTypes.Both.value,
    },
  ];

  currentState: any = null;
  form: FormGroup = new FormGroup({
    name: new FormControl<string>('', [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(50),
    ]),
    favIcon: new FormControl<string>(''),
    pageUrl: new FormControl<string>('', Validators.required),
    sortingOrder: new FormControl<number>(0),
    isFeature: new FormControl<boolean>(false),
    isActive: new FormControl<boolean>(true),
    parentId: new FormControl<string | null>(null),
    isSidebarmenu: new FormControl<boolean>(false),
    menuType: new FormControl<string | null>(null, Validators.required),
    menuAccessType: new FormControl<string | null>(null, Validators.required),
  });

  constructor(
    private readonly toastService: ToastService,
    private readonly route: Router,
    private readonly sitemapService: SitemapService
  ) {
    super();
    this.sitemapService.getParentMenuList().subscribe((response) => {
      if (response.isSuccess) {
        this.parentMenus = response.data ?? [];
        const windowState = window.history.state;
        const state = windowState.sitemap;
        const selectedParent: any = this.parentMenus.find(
          (d: any) => d.Id === state.parentId
        );
        console.log('Selected: ', selectedParent);
        if (state) {
          this.currentState = state;
          //console.log('Current state:', this.currentState);
          this.form.patchValue({
            name: state.name,
            favIcon: state.favIcon,
            pageUrl: state.pageUrl,
            sortingOrder: state.sortingOrder,
            isFeature: state.isFeature,
            isActive: state.isActive,
            parentId: selectedParent?.Id ?? null,
            isSidebarmenu: state.isSidebarmenu,
            menuType: state.menuType,
            menuAccessType: state.menuAccessType,
          });
          this.form.get('division')?.disable();
        }
      } else {
        console.error('Failed to load division list:', response.errors);
      }
    });
  }
  onSubmit() {
    console.log('submitted: ', this.form);
    if (!this.form.valid) {
      this.markAllAsTouched(); // show all errors
      return;
    }
    if (this.form.valid) {
      const payload = {
        id: this.currentState.id,
        name: this.form.value.name,
        favIcon: this.form.value.favIcon,
        pageUrl: this.form.value.pageUrl,
        sortingOrder: this.form.value.sortingOrder,
        isFeature: this.form.value.isFeature,
        isActive: this.form.value.isActive,
        parentId: this.form.value.parentId,
        isSidebarmenu: this.form.value.isSidebarmenu,
        menuType: this.form.value.menuType,
        menuAccessType: this.form.value.menuAccessType,
      };

      this.sitemapService.editSitemap(payload).subscribe({
        next: (res) => {
          if (res.isSuccess) {
            this.toastService.success('Sitemap has been updated successfully.');
          } else {
            let errorMessage =
              'Failed to edit sitemap. Please try again later.';
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
            'Failed to edit sitemap. Please try again later.'
          );
        },
      });
      console.log('Payload to submit:', payload);
    }
  }

  onCancel() {
    this.route.navigate(['/sitemap']);
  }
}
