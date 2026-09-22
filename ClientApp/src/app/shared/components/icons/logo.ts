import { Component, Input } from '@angular/core';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-logo',
  template: `
    <div class="logo-container">
      <ng-container *ngIf="logoUrl; else defaultLogo">
        <img
          [src]="logoUrl"
          alt="Logo"
          class="tenant-logo"
          [height]="56"
          [width]="56"
          (error)="logoUrl = null"
        />
      </ng-container>

      <ng-template #defaultLogo>
        <img
          src="assets/Edugates_Logo1.png"
          alt="Edugates Logo"
          class="tenant-logo"
          [height]="56"
          [width]="56"
        />
      </ng-template>
    </div>
  `,
  imports: [NgIf],
  styles: [
    `
      .logo-container {
        width: 56px;
        height: 56px;
        display: flex;
        justify-content: center;
        align-items: center;
      }

      :host {
        display: inline-block;
      }

      .tenant-logo {
        max-width: 100%;
        max-height: 100%;
        object-fit: contain;
      }
    `,
  ],
})
export class Logo {
  @Input() logoUrl: string | null | undefined;
}
