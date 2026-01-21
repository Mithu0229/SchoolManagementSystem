import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppLayout } from '../../../shared/layout/dashboard-layout/component/app.layout';

const routes: Routes = [
    {
        path: '',
        component: AppLayout,
        //canActivate: [authGuard],
        children: [
            {
                path: '',
                loadComponent: () => import('./role-list/role-list.component').then((m) => m.RoleListComponent),
                //canActivate: [authGuard],
                data: {
                    breadcrumb: 'Role Management',
                    parent: [{ label: 'Role', url: '/role' }]
                }
            },
            {
                path: 'create',
                loadComponent: () => import('./create-role/create-role.component').then((m) => m.CreateRoleComponent),
                //canActivate: [authGuard, permissionGuard('/division')],
                data: {
                    breadcrumb: 'Create Role',
                    parent: [
                        { label: 'Role', url: '/role' },
                        { label: 'Role Management', url: '/role' }
                    ]
                }
            },
            {
                path: 'edit/:id',
                loadComponent: () => import('./edit-role/edit-role.component').then((m) => m.EditRoleComponent),
                //canActivate: [authGuard, permissionGuard('/division')],
                data: {
                    breadcrumb: 'Edit Role',
                    parent: [
                        { label: 'Role', url: '/role' },
                        { label: 'Role Management', url: '/role' }
                    ]
                }
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class RoleRoutingModule {}