import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppLayout } from '../../../shared/layout/dashboard-layout/component/app.layout';
import { authGuard } from '../../../core/guards/auth.guard';
import { permissionGuard } from '../../../core/guards/permission.guard';
import { publicGuard } from '../../../core/guards/public.guard';

const routes: Routes = [
  {
    path: '',
    component: AppLayout,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./role-management/role-management.component').then(
            (m) => m.RoleManagementComponent
          ),
        canActivate: [authGuard, permissionGuard('/role')],
        data: {
          breadcrumb: 'Role Management',
          parent: [{ label: 'Role', url: '/role' }],
        },
      },
      {
        path: 'create',
        loadComponent: () =>
          import('./create-role/create-role.component').then(
            (m) => m.CreateRoleComponent
          ),
        canActivate: [authGuard, permissionGuard('/role')],
        data: {
          breadcrumb: 'Create Role',
          parent: [
            { label: 'Role', url: '/role' },
            { label: 'Role Management', url: '/role' },
          ],
        },
      },
      {
        path: 'edit/:id',
        loadComponent: () =>
          import('./edit-role/edit-role.component').then(
            (m) => m.EditRoleComponent
          ),
        canActivate: [authGuard, permissionGuard('/role')],
        data: {
          breadcrumb: 'Edit Role',
          parent: [
            { label: 'Role', url: '/role' },
            { label: 'Role Management', url: '/role' },
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
export class RoleManagementRoutingModule {}
