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
        />
      </ng-container>

      <ng-template #defaultLogo>
        <div
          class="inline-flex items-center justify-center"
          style="width: 20, height: 20px"
        >
          <svg
            viewBox="0 0 96 96"
            xmlns="http://www.w3.org/2000/svg"
            role="img"
            aria-label="ABC badge"
            className="w-full h-full"
          >
            <circle
              cx="48"
              cy="48"
              r="46"
              fill="#fff"
              stroke="#363763"
              strokeWidth="4"
            />
            <circle cx="48" cy="48" r="40" fill="#363763" />
            <text
              x="50%"
              y="54%"
              textAnchor="middle"
              fontFamily="Inter, Arial, sans-serif"
              fontWeight="700"
              fontSize="36"
              fill="#fff"
              dominantBaseline="middle"
            >
              ABC
            </text>
          </svg>
        </div>
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
