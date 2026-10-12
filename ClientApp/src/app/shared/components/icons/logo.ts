import { Component, Input } from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';

@Component({
  selector: 'app-logo',
  template: `
    <div
      class="logo-container"
      [style.height]="formatSize(height)"
      [style.max-height]="formatSize(maxHeight || height)"
    >
      <ng-container *ngIf="logoUrl; else defaultLogo">
        <img
          [src]="logoUrl"
          alt="Logo"
          class="tenant-logo"
          [style.height]="formatSize(height)"
          [style.max-height]="formatSize(maxHeight || height)"
          [style.width]="formatSize(width)"
          [style.max-width]="formatSize(maxWidth)"
          (error)="logoUrl = null"
        />
      </ng-container>

      <ng-template #defaultLogo>
        <img
          src="assets/Edugates_Logo1.png"
          alt="Edugates Logo"
          class="tenant-logo"
          [style.height]="formatSize(height)"
          [style.max-height]="formatSize(maxHeight || height)"
          [style.width]="formatSize(width)"
          [style.max-width]="formatSize(maxWidth)"
        />
      </ng-template>
    </div>
  `,
  imports: [NgIf, CommonModule],
  styles: [
    `
      .logo-container {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100%;
      }

      :host {
        display: inline-flex;
        align-items: center;
      }

      .tenant-logo {
        max-width: 100%;
        max-height: 100%;
        object-fit: contain;
        display: block;
      }
    `,
  ],
})
export class Logo {
  @Input() logoUrl: string | null | undefined;
  @Input() height: string | number = 52;
  @Input() width: string | number = 'auto';
  @Input() maxHeight: string | number = 52;
  @Input() maxWidth: string | number = 240;

  formatSize(val: string | number | undefined): string {
    if (val === undefined || val === null) return 'auto';
    return typeof val === 'number' ? `${val}px` : String(val);
  }
}

