import { CommonModule } from '@angular/common';
import { Component, signal, computed } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Fluid } from 'primeng/fluid';
import { InputTextModule } from 'primeng/inputtext';
import { StyleClassModule } from 'primeng/styleclass';
import { MessageModule } from 'primeng/message';
import { ToastService } from '../../../core/services/toast.service';
import { InputOtpModule } from 'primeng/inputotp';
import { Router, RouterModule } from '@angular/router';
import { ErrorMessageComponent } from '../../../shared/components/error-message/error-message.component';
import { UserService } from '../../../core/services/user.service';
import { FormErrorComponent } from '../../../shared/components/form-error.component';
import { TextareaComponent } from '../../../shared/components/textarea/textarea.component';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { RadioButtonModule } from 'primeng/radiobutton';
import { UserTypes } from '../../../core/enums/fixedIds';
import { FormBase } from '../../../core/enums/form-base';

@Component({
  selector: 'app-user-register',
  standalone: true,
  imports: [
    CommonModule,
    ButtonModule,
    ReactiveFormsModule,
    Fluid,
    InputTextModule,
    StyleClassModule,
    MessageModule,
    InputOtpModule,
    RouterModule,
    ReactiveFormsModule,
    CommonModule,
    FormErrorComponent,
    TextareaComponent,
    InputTextModule,
    TextareaModule,
    SelectModule,
    DatePickerModule,
    RadioButtonModule,
    ButtonModule,
  ],
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.scss'],
})
export class UserRegisterComponent extends FormBase {
  userTypes: any[] = Object.values(UserTypes);
  form = new FormGroup({
    firstName: new FormControl(null),
    lastName: new FormControl(null),
    email: new FormControl(null, [
      Validators.required,
      Validators.maxLength(150),
    ]),
    phoneNumber: new FormControl(null, [
      Validators.required,
      Validators.maxLength(50),
    ]),
    password: new FormControl(null, [
      Validators.required,
      Validators.maxLength(50),
    ]),
    address: new FormControl('', [Validators.maxLength(1000)]),
    isActive: new FormControl(false),
  });

  constructor(
    private readonly userService: UserService,
    private readonly route: Router,
    private readonly toastService: ToastService
  ) {
    super();
  }

  onSubmit() {
    console.log('submitted: ', this.form);
    if (!this.form.valid) {
      // this.markAllAsTouched(); // show all errors
      return;
    }
    if (this.form.valid) {
      const payload = {
        firstName: this.form.value.firstName,
        lastName: this.form.value.lastName,
        email: this.form.value.email,
        phoneNumber: this.form.value.phoneNumber,
        password: this.form.value.password,
        userType: 4,
        address: this.form.value.address,
        isActive: false,
      };
      this.userService.createUser(payload).subscribe({
        next: (res) => {
          if (res.isSuccess) {
            this.toastService.success('User has been created successfully.');
            //this.resetForm();
          } else {
            let errorMessage = 'Failed to create User. Please try again later.';
            if (res.notificationMessage && res.notificationMessage !== '') {
              errorMessage = res.notificationMessage;
            } else if (res.errors?.[0]) {
              errorMessage = res.errors[0];
            }
            this.toastService.error(errorMessage);
          }
        },
        error: () => {
          this.toastService.error(
            'Failed to create User. Please try again later.'
          );
        },
      });
    }
  }

  onCancel() {
    this.route.navigate(['/login']);
  }
}
