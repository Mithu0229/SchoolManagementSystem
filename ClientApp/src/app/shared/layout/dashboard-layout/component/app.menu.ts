import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { AppMenuitem } from './app.menuitem';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule, AppMenuitem, RouterModule],
  template: `
    <ul class="layout-menu">
      <ng-container *ngFor="let item of model; let i = index">
        <li
          class="items"
          app-menuitem
          *ngIf="!item.separator"
          [item]="item"
          [index]="i"
          [root]="true"
        ></li>
        <li *ngIf="item.separator" class="menu-separator fill-black"></li>
      </ng-container>
    </ul>
  `,
  styles: `
        .items {
            color: #ffffff !important;
        }
    `,
})
export class AppMenu {
  model: MenuItem[] = [];

  constructor(private readonly authService: AuthService) {
    this.authService.getMenuList().subscribe({
      next: (response: any) => {
        if (response.isSuccess) {
          this.model = response.data ?? [];
          localStorage.setItem('menuList', JSON.stringify(this.model));
        } else {
          console.error('Failed to load menu list:', response.errors);
        }
      },
    });
  }
}
