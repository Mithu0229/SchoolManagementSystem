import { Component } from '@angular/core';

@Component({
    selector: 'app-error-icon',
    template: `
        <div class="mr-1">
            <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M8 2C4.688 2 2 4.688 2 8C2 11.312 4.688 14 8 14C11.312 14 14 11.312 14 8C14 4.688 11.312 2 8 2ZM8.6 11H7.4V9.8H8.6V11ZM8.6 8.6H7.4V5H8.6V8.6Z" fill="#C00000" />
            </svg>
        </div>
    `,
    styles: [':host { display: inline-block; }']
})
export class ErrorIconComponent {}
