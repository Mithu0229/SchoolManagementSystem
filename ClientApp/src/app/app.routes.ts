import { Routes } from '@angular/router';
import { UserLoginComponent } from './modules/auth/user-login/user-login.component';
import { UserRegisterComponent } from './modules/auth/user-register/user-register.component';
import { Notfound } from './shared/components/notfound/notfound';

export const routes: Routes = [
  //{ path: 'login', component: UserLoginComponent },
  {
    path: '',
    loadChildren: () =>
      import('./modules/auth/auth-routing.module').then(
        (m) => m.AuthRoutingModule
      ),
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('./modules/general-setup/users/user-routing.module').then(
        (m) => m.UserRoutingModule
      ),
  },
  {
    //ok
    path: 'sitemap',
    loadChildren: () =>
      import('./modules/settings/sitemap/sitemap-routing.module').then(
        (m) => m.SitemapRoutingModule
      ),
  },
  {
    //ok
    path: 'role',
    loadChildren: () =>
      import(
        './modules/general-setup/role/role-management-routing.module'
      ).then((m) => m.RoleManagementRoutingModule),
  },
  {
    path: 'tenant',
    loadChildren: () =>
      import('./modules/general-setup/tenant/tenant-routing.module').then(
        (m) => m.TenantRoutingModule
      ),
  },
  {
    path: 'application',
    loadChildren: () =>
      import(
        './modules/student-application/student-application-routing.module'
      ).then((m) => m.StudentApplicationRoutingModule),
  },

  { path: 'user-register', component: UserRegisterComponent },
  { path: 'notfound', component: Notfound },
  { path: '**', redirectTo: '/notfound' },
  //{ path: '', redirectTo: 'login', pathMatch: 'full' },
  //{ path: '**', redirectTo: '' },
];
