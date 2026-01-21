import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { EmptySearchIconComponent } from '../icons/empty-search-icon';
import { EmptyFilterIconComponent } from '../icons/empty-filter-icon';

@Component({
    selector: 'app-empty-state',
    standalone: true,
    imports: [CommonModule, ButtonModule, EmptySearchIconComponent, EmptyFilterIconComponent],
    template: ` <div class="flex flex-col items-center gap-4 py-16 bg-white">
        <!-- State: No results after filtering -->
        <ng-container *ngIf="isFiltered; else noData">
            <app-empty-filter-icon></app-empty-filter-icon>
            <span class="text-center text-2xl font-bold text-[#0a0a0a]">{{ 'No Result Found!' }}</span>
            <span class="text-gray-500 mt-1">We can't find any item matching your search criteria.</span>
            <button pButton label="Reset Filter" icon="pi pi-refresh" severity="primary" (click)="onReset()"></button>
        </ng-container>

        <!-- State: No data initially -->
        <ng-template #noData>
            <app-empty-search-icon></app-empty-search-icon>
            <span class="text-center text-2xl font-bold text-[#0a0a0a]">{{ 'There is no data to display!' }}</span>
        </ng-template>
    </div>`,
    styleUrl: './table.component.scss'
})
export class EmptyStateComponent {
    @Input() isFiltered: boolean = false;

    @Output() reset = new EventEmitter<void>();

    onReset() {
        this.reset.emit();
    }
}
