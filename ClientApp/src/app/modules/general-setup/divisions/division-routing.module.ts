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
                loadComponent: () => import('./division-list/division-list.component').then((m) => m.DivisionListComponent),
                //canActivate: [authGuard],
                data: {
                    breadcrumb: 'Division Management',
                    parent: [{ label: 'Division', url: '/division' }]
                }
            },
            // {
            //     path: 'create',
            //     loadComponent: () => import('./create-division/create-division.component').then((m) => m.CreateDivisionComponent),
            //     canActivate: [authGuard, permissionGuard('/division')],
            //     data: {
            //         breadcrumb: 'Create Division',
            //         parent: [
            //             { label: 'Division', url: '/division' },
            //             { label: 'Division Management', url: '/division' }
            //         ]
            //     }
            // },
            // {
            //     path: 'edit/:id',
            //     loadComponent: () => import('./edit-division/edit-division.component').then((m) => m.EditDivisionComponent),
            //     canActivate: [authGuard, permissionGuard('/division')],
            //     data: {
            //         breadcrumb: 'Edit Division',
            //         parent: [
            //             { label: 'Division', url: '/division' },
            //             { label: 'Division Management', url: '/division' }
            //         ]
            //     }
            // }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class DivisionRoutingModule {}