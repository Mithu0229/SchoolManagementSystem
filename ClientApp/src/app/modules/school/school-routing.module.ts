import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { InstituteComponent } from './components/institute/institute.component';

const routes: Routes = [
  { path: 'institute', component: InstituteComponent },
  { path: '', redirectTo: 'institute', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SchoolRoutingModule { }
