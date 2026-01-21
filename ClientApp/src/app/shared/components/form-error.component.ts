import { Component, Input } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { ErrorMessageComponent } from './error-message/error-message.component';
import { ValidationMessagesService } from '../../core/services/validation-message.service';

@Component({
  selector: 'app-form-error',
  imports: [ErrorMessageComponent],
  template: `
    <app-error-message message="{{ getErrorMessage() }}"></app-error-message>
  `,
})
export class FormErrorComponent {
  @Input() control: AbstractControl<any, any> | null = null;
  @Input() isInvalid: boolean = false;

  constructor(
    private readonly validationMessageService: ValidationMessagesService
  ) {}

  getErrorMessage() {
    return this.isInvalid && this.control?.errors
      ? this.validationMessageService.getErrorMessage(this.control.errors)
      : '';
  }
}
