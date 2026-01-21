import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DividerModule } from 'primeng/divider';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-confirm-dialog',
    imports: [DialogModule, DividerModule, ButtonModule, CommonModule],
    templateUrl: './confirm-dialog.component.html'
})
export class ConfirmDialogComponent {
    @Input() visible = false;
    @Input() title = 'Confirmation';
    @Input() visibleAcceptButton = true;
    @Input() acceptLabel = 'Delete';
    @Input() rejectLabel = 'Cancel';
    @Input() acceptButtonStyleClass = 'p-button-danger';
    @Output() accept = new EventEmitter<void>();
    @Output() reject = new EventEmitter<void>();

    close() {
        this.visible = false;
        this.reject.emit();
    }
}
