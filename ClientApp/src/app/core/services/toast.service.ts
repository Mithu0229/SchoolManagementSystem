import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Severity } from '../enums/toast-severity.enum';

@Injectable({
    providedIn: 'root'
})
export class ToastService {
    constructor(private readonly messageService: MessageService) {}

    show(severity: Severity, summary: string, detail: string, life: number = 3000) {
        this.messageService.add({ severity, summary, detail, life });
    }

    success(message: string, title: string = 'Success') {
        console.log('Success MSG:', message, ' this.messageService : ', this.messageService);
        this.show(Severity.SUCCESS, title, message);
    }
    error(message: string, title: string = 'Error') {
        this.show(Severity.ERROR, title, message);
    }
    info(message: string, title: string = 'Info') {
        this.show(Severity.INFO, title, message);
    }
    warn(message: string, title: string = 'Warning') {
        this.show(Severity.WARN, title, message);
    }

    clear() {
        this.messageService.clear();
    }
}
