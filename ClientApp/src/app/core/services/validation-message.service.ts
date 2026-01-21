import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ValidationMessagesService {
    private readonly messages = {
        required: 'This field is required.',
        email: 'Enter a valid email address.',
        minlength: (params: any) => `Minimum ${params.requiredLength} characters required.`,
        maxlength: (params: any) => `Maximum ${params.requiredLength} characters allowed.`,
        invalidUrl: 'Invalid URL format. The address must begin with http:// or https://.',
        alphaSpaceHyphen: 'Invalid format: Use letters, numbers, spaces, or hyphens.',
        domainMismatch: 'Email must belong to the Company Domain',
        invalidDomain: 'Please enter a valid domain name (e.g., example.com).',
        endDateBeforeStart: 'End Date must not be before Start Date',
        minTodayDate: 'Date must be today or later.',
        min: (params: any) => `Value must be greater than or equal to ${params.min}.`,
        max: (params: any) => `Value must be less than or equal to ${params.max}.`
    };

    getErrorMessage(error: any): string {
        const errorKey = Object.keys(error)[0] as keyof typeof this.messages;
        const errorValue = error[errorKey];
        const message = this.messages[errorKey];
        return typeof message === 'function' ? message(errorValue) : message || 'Invalid field';
    }
}
