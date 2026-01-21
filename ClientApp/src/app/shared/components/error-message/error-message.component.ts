import { Component, Input } from '@angular/core';
import { MessageModule } from 'primeng/message';
import { ErrorIconComponent } from '../icons/error-icon';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-error-message',
    standalone: true,
    imports: [MessageModule, ErrorIconComponent, CommonModule],
    templateUrl: './error-message.component.html',
    styleUrl: './error-message.component.scss'
})
export class ErrorMessageComponent {
    @Input() message: string = '';
}
