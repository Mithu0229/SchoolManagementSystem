import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppLayout } from '../../../shared/layout/dashboard-layout/component/app.layout';
import { authGuard } from '../../../core/guards/auth.guard';
import { permissionGuard } from '../../../core/guards/permission.guard';

const routes: Routes = [
  {
    path: '',
    component: AppLayout,
    ////canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./list-tenant/list-tenant.component').then(
            (m) => m.TenantManagementComponent
          ),
        ////canActivate: [authGuard],
        data: {
          breadcrumb: 'Branch Dashboard',
          parent: [{ label: 'Branch Management', url: '/tenant' }],
        },
      },
      {
        path: 'register',
        loadComponent: () =>
          import('./tenant-registration/tenant-registration.component').then(
            (m) => m.TenantRegistrationComponent
          ),
        ////canActivate: [authGuard, permissionGuard('/tenant')],
        data: {
          breadcrumb: 'Branch Registration',
          parent: [
            { label: 'Branch', url: '/tenant' },
            { label: 'Branch Management', url: '/tenant' },
          ],
        },
      },
    ],
  },
  {
    path: '',
    component: AppLayout,
    //canActivate: [authGuard],
    children: [
      {
        path: 'edit',
        loadComponent: () =>
          import('./edit-tenant/edit-tenant.component').then(
            (m) => m.EditTenantComponent
          ),
        //canActivate: [authGuard],
        data: {
          breadcrumb: 'Update Branch Profile',
        },
      },
    ],
  },
  {
    path: '',
    component: AppLayout,
    //canActivate: [authGuard],
    children: [
      {
        path: 'edit/:id',
        loadComponent: () =>
          import('./edit-tenant/edit-tenant.component').then(
            (m) => m.EditTenantComponent
          ),
        ////canActivate: [authGuard, permissionGuard('/tenant')],
        data: {
          breadcrumb: 'Update Branch Profile',
          parent: [
            { label: 'Branch', url: '/tenant' },
            { label: 'Branch Management', url: '/tenant' },
          ],
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TenantRoutingModule {}
