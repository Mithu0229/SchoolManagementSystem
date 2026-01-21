import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from '../../../core/guards/auth.guard';
import { AppLayout } from '../../../shared/layout/dashboard-layout/component/app.layout';

const routes: Routes = [
  {
    path: '',
    component: AppLayout,
    // canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./list-sitemap/list-sitemap.component').then(
            (m) => m.ListSitemapComponent
          ),
        //canActivate: [authGuard],
        data: {
          breadcrumb: 'Sitemap Management',
          parent: [{ label: 'Sitemap', url: '/sitemap' }],
        },
      },
      {
        path: 'create',
        loadComponent: () =>
          import('./create-sitemap/create-sitemap.component').then(
            (m) => m.CreateSitemapComponent
          ),
        //canActivate: [authGuard],
        data: {
          breadcrumb: 'Create Sitemap',
          parent: [
            { label: 'Sitemap', url: '/sitemap' },
            { label: 'Sitemap Management', url: '/sitemap' },
          ],
        },
      },
      {
        path: 'edit/:id',
        loadComponent: () =>
          import('./edit-sitemap/edit-sitemap.component').then(
            (m) => m.EditSitemapComponent
          ),
        //canActivate: [authGuard],
        data: {
          breadcrumb: 'Edit Sitemap',
          parent: [
            { label: 'Sitemap', url: '/sitemap' },
            { label: 'Sitemap Management', url: '/sitemap' },
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
export class SitemapRoutingModule {}
