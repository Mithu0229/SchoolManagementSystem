import { Component, ElementRef } from '@angular/core';
import { AppMenu } from './app.menu';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [AppMenu],
  template: ` <div class="layout-sidebar">
    <app-menu></app-menu>
  </div>`,
  styles: `
        .layout-sidebar {
            background-color: #202152 !important;
            //z-index: 9999;
        }
  `,
})
export class AppSidebar {
  constructor(public el: ElementRef) {}
}
